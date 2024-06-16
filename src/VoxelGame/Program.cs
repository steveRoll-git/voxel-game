using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace VoxelGame;

using static Graphics;
using static ResourceUtils;

internal static class Program
{
    private static Mesh mesh;

    private static Shader shader;

    private static float x;

    public static void Main(string[] args)
    {
        window.Load += OnLoad;
        window.Update += OnUpdate;
        window.Render += OnRender;

        window.Run();
    }

    private static unsafe void OnLoad()
    {
        gl.ClearColor(System.Drawing.Color.CornflowerBlue);

        var vertexFormat = new VertexFormat(
            new(VertexAttribute.Position, 3),
            new(VertexAttribute.TexCoords, 2)
        );

        mesh = new Mesh(vertexFormat, new float[,] {
            { 0.5f, 0.5f, 0.0f, 0, 0 },
            { 0.5f, -0.5f, 0.0f, 0, 1 },
            { -0.5f, -0.5f, 0.0f, 1, 1 },
            { -0.5f, 0.5f, 0.0f, 1, 0 },
        }, [
            0u, 1u, 3u,
            1u, 2u, 3u
        ]);

        shader = new Shader(ReadResource("shaders.default.vert"), ReadResource("shaders.default.frag"));
    }

    private static void OnUpdate(double deltaTime)
    {
        x += (float)deltaTime * 0.1f;
    }

    private static unsafe void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        mesh.Draw(Matrix4x4.CreateTranslation(x, 0, 0));
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            window.Close();
    }
}
