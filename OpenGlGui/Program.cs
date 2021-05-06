using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenGlGui
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSetting = new NativeWindowSettings();

            gameWindowSettings.UpdateFrequency = 60.0;
            gameWindowSettings.RenderFrequency = 60.0;
            nativeWindowSetting.Size = new Vector2i(1600, 900);

            var display = new Display(gameWindowSettings, nativeWindowSetting);

            display.Run();
        }
    }
}
