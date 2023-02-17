using OpenGlGuiLibrary.GuiElements;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Runtime.InteropServices;

namespace OpenGlGuiLibrary
{
    public class GuiElementShader : Shader
    {
        protected IDisplaySettings _displaySettings;

        protected float[] _vertices = new float[]
        {
            -1.0f, 1.0f, 0.0f,
             -1.0f, -1.0f, 0.0f,
             1.0f, 1.0f, 0.0f,
             -1.0f, -1.0f, 0.0f,
             1.0f, -1.0f, 0.0f,
             1.0f, 1.0f, 0.0f
        };

        protected int _vertexBuffer;
        protected int _vertexArray;

        public GuiElementShader(string vertexPath, string fragmentPath, IDisplaySettings displaySettings) : base(vertexPath, fragmentPath)
        {
            _displaySettings = displaySettings;

            CreateVertexBuffer();
            CreateVertexArray();

            Use();
        }

        private void CreateVertexBuffer()
        {
            GL.CreateBuffers(1, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Length * sizeof(float)), handle.AddrOfPinnedObject(),
                    BufferUsageHint.StaticDraw);
            }
            finally
            {
                handle.Free();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        }

        private void CreateVertexArray()
        {
            GL.CreateVertexArrays(1, out _vertexArray);
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        //_texture.Use(TextureUnit.Texture0);

        //GL.UseProgram(handle);
        //GL.BindVertexArray(_vertexArray);
        //Console.WriteLine(GL.GetError().ToString());         

        ////GL.UniformMatrix4(1, false, ref scale);
        //Console.WriteLine(GL.GetError().ToString());


        public void Render(GuiElement guiObject)
        {
            this.Use();
            SetUniforms(guiObject);

            if (guiObject.Texture != null)
            {
                guiObject.Texture.Use(TextureUnit.Texture0);
            }

            GL.BindVertexArray(_vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        protected virtual void SetUniforms(GuiElement guiObject)
        {
            int color = GL.GetUniformLocation(handle, "color");
            GL.Uniform3(color, guiObject.Style.Color);

            int location = GL.GetUniformLocation(handle, "iResolution");
            GL.Uniform2(location, guiObject.Placement.GetiResolution());

            //var objectPosition = guiObject.Position;
            var transpose = GL.GetUniformLocation(handle, "transpose");
            GL.Uniform2(transpose, guiObject.Placement.GetTransposeVector());

            var sizeLocation = GL.GetUniformLocation(handle, "size");
            GL.Uniform2(sizeLocation, guiObject.Placement.GetSize());

            var isClicked = GL.GetUniformLocation(handle, "isClicked");
            GL.Uniform1(isClicked, guiObject.IsClicked);
        }

    }
}
