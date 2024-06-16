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

    private static float time;

    private static Matrix4x4 viewMatrix;
    private static Matrix4x4 projectionMatrix;

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
            { 0.5f, 0.5f, -0.5f, 0, 0 },
            { 0.5f, -0.5f, -0.5f, 0, 1 },
            { -0.5f, -0.5f, -0.5f, 1, 1 },
            { -0.5f, 0.5f, -0.5f, 1, 0 },
            { 0.5f, 0.5f, 0.5f, 0, 0 },
            { 0.5f, -0.5f, 0.5f, 0, 1 },
            { -0.5f, -0.5f, 0.5f, 1, 1 },
            { -0.5f, 0.5f, 0.5f, 1, 0 },
        }, [
            0, 1, 3,
            1, 2, 3,
            4, 5, 7,
            5, 6, 7,
        ]);

        shader = new Shader(ReadResource("shaders.default.vert"), ReadResource("shaders.default.frag"));

        viewMatrix = Matrix4x4.CreateTranslation(0, 0, -2);
        projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 2, (float)window.Size.X / window.Size.Y, 0.1f, 100);

        shader.SendMat4("projection", projectionMatrix);
        shader.SendMat4("view", viewMatrix);
    }

    private static void OnUpdate(double deltaTime)
    {
        time += (float)deltaTime;
    }

    private static unsafe void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        mesh.Draw(Matrix4x4.CreateRotationZ(time) * Matrix4x4.CreateRotationY(time / 2));
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            window.Close();
    }
}
