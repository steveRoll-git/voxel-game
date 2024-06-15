using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using System.Reflection;

namespace VoxelGame;

using static Graphics;
using static ResourceUtils;

internal static class Program
{
    private static Mesh mesh;
    private static Mesh mesh2;

    private static Shader shader;

    public static void Main(string[] args)
    {
        window.Load += OnLoad;
        window.Update += OnUpdate;
        window.Render += OnRender;

        window.Run();
    }

    private static unsafe void OnLoad()
    {
        gl.ClearColor(Color.CornflowerBlue);

        var vertexFormat = new VertexFormat(
            new(VertexAttribute.Position, 3),
            new(VertexAttribute.TexCoords, 2)
        );

        mesh = new Mesh(vertexFormat, [
            0.5f, 0.5f, 0.0f, 0, 0,
            0.5f, -0.5f, 0.0f, 0, 1,
            -0.5f, -0.5f, 0.0f, 1, 1,
            -0.5f, 0.5f, 0.0f, 1, 0,
        ], [
            0u, 1u, 3u,
            1u, 2u, 3u
        ]);

        mesh2 = new Mesh(vertexFormat, [
            0.7f, 0.5f, 0.0f, 0, 0,
            0.7f, -0.5f, 0.0f, 0, 1,
            -0.3f, -0.5f, 0.0f, 1, 1,
            -0.3f, 0.5f, 0.0f, 1, 0,
        ], [
            0u, 1u, 3u,
            1u, 2u, 3u
        ]);

        shader = new Shader(ReadResource("shaders.default.vert"), ReadResource("shaders.default.frag"));
    }

    private static void OnUpdate(double deltaTime)
    {
    }

    private static unsafe void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        mesh.Draw();
        mesh2.Draw();
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            window.Close();
    }
}
