using OpenGlGuiLibrary;
using OpenGlGuiLibrary.GuiElements;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace OpenGlGui
{
    public class Display : GameWindow
    {
        Shader _shader;
        Gui _gui;
        bool _drawOnRenderFrame = true;

        DisplaySettings _displaySettings;

        float[] vertices =
            {
            -1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f,
            1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f,
            1.0f, 1.0f, 0.0f,
            1.0f, -1.0f, 0.0f
        };

        int _vertexBufferObject;
        int _vertexArrayObject;

        public Display(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSetting) : base(gameWindowSettings, nativeWindowSetting)
        {
            this.Title = "Open TK Window";
            this._displaySettings = new DisplaySettings(nativeWindowSetting.Size.X, nativeWindowSetting.Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = KeyboardState.GetSnapshot();
            //var mouse = this.MouseState;


            if (input.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }
            else if (input.IsKeyDown(Keys.Space))
            {
                _shader.Recompile();
                _gui.RecompileShader();
            }
            else if ((input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt)) && input.IsKeyDown(Keys.Up))
            {
            }

            else if (CheckInputForNumbersAndProcess())
            {

            }
            else if ((input.IsKeyDown(Keys.Enter) && !_lastKeyState.IsKeyDown(Keys.Enter)) || (input.IsKeyDown(Keys.KeyPadEnter) && !_lastKeyState.IsKeyDown(Keys.KeyPadEnter)))
            {
                try
                {
                    var layernumber = Convert.ToInt32(_input);
                    _input = "";
                }
                catch (Exception)
                {
                    _input = "";
                    //throw;
                }
            }

            _lastKeyState = input;
            base.OnUpdateFrame(e);
        }

        string _input = "";
        KeyboardState _lastKeyState;
        bool CheckInputForNumbersAndProcess()
        {
            KeyboardState input = KeyboardState;
            if ((input.IsKeyDown(Keys.D0) && !_lastKeyState.IsKeyDown(Keys.D0)) || (input.IsKeyDown(Keys.KeyPad0) && !_lastKeyState.IsKeyDown(Keys.KeyPad0)))
            {
                _input += "0";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D1) && !_lastKeyState.IsKeyDown(Keys.D1)) || (input.IsKeyDown(Keys.KeyPad1) && !_lastKeyState.IsKeyDown(Keys.KeyPad1)))
            {
                _input += "1";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D2) && !_lastKeyState.IsKeyDown(Keys.D2)) || (input.IsKeyDown(Keys.KeyPad2) && !_lastKeyState.IsKeyDown(Keys.KeyPad2)))
            {
                _input += "2";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D3) && !_lastKeyState.IsKeyDown(Keys.D3)) || (input.IsKeyDown(Keys.KeyPad3) && !_lastKeyState.IsKeyDown(Keys.KeyPad3)))
            {
                _input += "3";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D4) && !_lastKeyState.IsKeyDown(Keys.D4)) || (input.IsKeyDown(Keys.KeyPad4) && !_lastKeyState.IsKeyDown(Keys.KeyPad4)))
            {
                _input += "4";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D5) && !_lastKeyState.IsKeyDown(Keys.D5)) || (input.IsKeyDown(Keys.KeyPad5) && !_lastKeyState.IsKeyDown(Keys.KeyPad5)))
            {
                _input += "5";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D6) && !_lastKeyState.IsKeyDown(Keys.D6)) || (input.IsKeyDown(Keys.KeyPad6) && !_lastKeyState.IsKeyDown(Keys.KeyPad6)))
            {
                _input += "6";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D7) && !_lastKeyState.IsKeyDown(Keys.D7)) || (input.IsKeyDown(Keys.KeyPad7) && !_lastKeyState.IsKeyDown(Keys.KeyPad7)))
            {
                _input += "7";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D8) && !_lastKeyState.IsKeyDown(Keys.D8)) || (input.IsKeyDown(Keys.KeyPad8) && !_lastKeyState.IsKeyDown(Keys.KeyPad8)))
            {
                _input += "8";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D9) && !_lastKeyState.IsKeyDown(Keys.D9)) || (input.IsKeyDown(Keys.KeyPad9) && !_lastKeyState.IsKeyDown(Keys.KeyPad9)))
            {
                _input += "9";
                return true;
            }
            return false;
        }
        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.CreateBuffers(1, out _vertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.CreateVertexArrays(1, out _vertexArrayObject);
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _shader = new Shader(".\\Shader\\shader.vert", ".\\Shader\\shader.frag");
            _shader.Use();

            _gui = new Gui(_displaySettings, this);

            var elementGroup = new ElementGroup(new Vector2i(500,700), new Vector2i(100,100), _gui.ElementGroupShader, _displaySettings, ElementAnchors.TopRight);

            _gui.AddGuiElement(elementGroup);

            var button = new Button(new Vector2i(250, 35), new Vector2i(50, 100), _gui.GuiElementShader, _displaySettings, ElementAnchors.TopRight, elementGroup);
            button.SetText("Hello World!");

            button = new Button(new Vector2i(250, 35), new Vector2i(50, 150), _gui.GuiElementShader, _displaySettings, ElementAnchors.TopRight, elementGroup);
            button.SetText("Bunny!");

            button = new Button(new Vector2i(250, 35), new Vector2i(50, 200), _gui.GuiElementShader, _displaySettings, ElementAnchors.TopRight, elementGroup);
            button.SetText("AliVollNixSchuld");

            var label = new Label(new Vector2i(250, 30), new Vector2i(50, 50), _gui.LabelShader, _displaySettings, ElementAnchors.TopRight, elementGroup);
            label.SetText("Layer: 5 von 100");

            //int temp;
            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            _displaySettings.Resize(e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Offset.Y > 0)
            {
            }
            else
            {
            }

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _gui.MouseEventManager.MouseDown(e);
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _gui.MouseEventManager.MouseUp(e);
            _gui.Unclick();

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            _gui.MouseEventManager.MouseMove();
            base.OnMouseMove(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            //Console.WriteLine(GL.GetError().ToString());

            //GL.BindVertexArray(_vertexArrayObject);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            _gui.RenderAll();

            //_guiObjectShader.Use();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            //_guiObjectShader.UseAgain();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            SwapBuffers();
            

            base.OnRenderFrame(e);
        }
    }
}
