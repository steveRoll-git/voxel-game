using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelGame;
internal static class Graphics
{
    public static IWindow window;
    public static GL gl;

    static Graphics()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "My first Silk.NET program!";

        window = Window.Create(options);
        window.Load += Window_Load;
    }

    private static void Window_Load()
    {
        gl = window.CreateOpenGL();
    }
}
