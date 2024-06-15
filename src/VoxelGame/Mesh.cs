using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelGame;

using static Graphics;

internal class Mesh
{
    private readonly uint vao;
    private readonly uint vbo;
    private readonly uint ebo;

    public Mesh(VertexFormat vertexFormat, float[] vertices, uint[] indices)
    {
        vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);

        vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

        unsafe
        {
            fixed (float* buf = vertices)
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }

        ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);

        unsafe
        {
            fixed (uint* buf = indices)
                gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);
        }

        var offset = 0;
        foreach (var attribute in vertexFormat.attributes)
        {

            uint location = (uint)attribute.Attribute;
            gl.EnableVertexAttribArray(location);
            unsafe
            {
                gl.VertexAttribPointer(location, attribute.Size, VertexAttribPointerType.Float, false, vertexFormat.stride, (void*)offset);
            }
            offset += attribute.Size * sizeof(float);
        }

        gl.BindVertexArray(0);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
    }

    public void Draw()
    {
        gl.BindVertexArray(vao);
        unsafe
        {
            gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
}
