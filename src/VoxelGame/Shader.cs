using Silk.NET.OpenGL;
using System.Text;
using System.Numerics;

namespace VoxelGame;

using static Graphics;

internal class Shader
{
    private readonly uint programHandle;

    public static Shader CurrentShader { get; private set; }

    public Shader(string vertexCode, string fragmentCode)
    {
        vertexCode = PreprocessShaderCode(vertexCode);
        uint vertexShader = gl.CreateShader(ShaderType.VertexShader);
        gl.ShaderSource(vertexShader, vertexCode);

        gl.CompileShader(vertexShader);

        gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
        if (vStatus != (int)GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + gl.GetShaderInfoLog(vertexShader));

        fragmentCode = PreprocessShaderCode(fragmentCode);
        uint fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(fragmentShader, fragmentCode);

        gl.CompileShader(fragmentShader);

        gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
        if (fStatus != (int)GLEnum.True)
            throw new Exception("Fragment shader failed to compile: " + gl.GetShaderInfoLog(fragmentShader));

        programHandle = gl.CreateProgram();

        gl.AttachShader(programHandle, vertexShader);
        gl.AttachShader(programHandle, fragmentShader);

        gl.LinkProgram(programHandle);

        gl.GetProgram(programHandle, ProgramPropertyARB.LinkStatus, out int lStatus);
        if (lStatus != (int)GLEnum.True)
            throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(programHandle));

        gl.DetachShader(programHandle, vertexShader);
        gl.DetachShader(programHandle, fragmentShader);
        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        if (CurrentShader == this)
        {
            return;
        }
        gl.UseProgram(programHandle);
        CurrentShader = this;
    }

    public void SendMat4(string uniform, Matrix4x4 mat4)
    {
        Use();
        var location = gl.GetUniformLocation(programHandle, uniform);
        unsafe
        {
            gl.UniformMatrix4(location, 1, false, (float*)&mat4);
        }
    }

    public static string PreprocessShaderCode(string code)
    {
        var sb = new StringBuilder();
        sb.AppendLine("#version 330 core");
        foreach (var attribute in Enum.GetValues<VertexAttribute>())
        {
            sb.AppendLine($"#define ATTRIBUTE_LOCATION_{Enum.GetName(attribute).ToUpper()} {(uint)attribute}");
        }
        sb.AppendLine("#line 1");
        sb.Append(code);
        return sb.ToString();
    }
}
