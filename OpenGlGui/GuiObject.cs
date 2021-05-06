using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OpenGlGui
{
    public abstract class GuiObject
    {
        public Vector2i Size { get; protected set; }

        ///Refers to top left position of the bounding box
        public Vector2i Position { get; protected set; }
        public Texture Texture { get; private set; }

        public GuiObject(Vector2i size, Vector2i position)
        {
            Position = position;
            Size = size;
        }
        public int Id { get; protected set; }

        protected Bitmap CreateBitmapImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(Size.X, Size.Y);

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 25, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            var intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            var intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(Size.X, Size.Y));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.FromArgb(0, 255, 255, 255));
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            StringFormat sf = new StringFormat();
            //sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(20, 20, 20)), (Size.X - intWidth) / 2, (Size.Y - intHeight) / 2);
            objGraphics.Flush();

            return (objBmpImage);
        }

        public List<GuiObject> Children { get; private set; } = new List<GuiObject>();
    }

    public class GuiObjectShader : Shader
    {
        protected float[] _vertices;
        protected int _vertexBuffer;
        protected int _vertexArray;

        protected DisplaySettings _displaySettings;
        protected ElementAnchors _anchor;
        //public Vector2i Size { get; protected set; }
        //public Vector2i Position { get; protected set; } = new Vector2i(50, 50);

        //public Texture _texture;

        public GuiObjectShader(string vertexPath, string fragmentPath, DisplaySettings displaySettings, ElementAnchors ElementAnchor = ElementAnchors.TopRight) : base(vertexPath, fragmentPath)
        {
            _displaySettings = displaySettings;
            _anchor = ElementAnchor;

            CreateVertices();
            CreateVertexBuffer();
            CreateVertexArray();

            //var bitmap = CreateBitmapImage("12.34");
            //bitmap.Save("test.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //_texture = Texture.LoadFromImage(bitmap);
            Use();
        }


        protected void CreateVertices()
        {
            float[] vertices =
            {
                -Size.X/2, Size.Y/2, 0f,
                -Size.X/2, -Size.Y/2, 0f,
                Size.X/2, Size.Y/2, 0f,

                -Size.X/2, -Size.Y/2, 0f,
                Size.X/2, Size.Y/2, 0f,
                Size.X/2, -Size.Y/2, 0f,
               //-.5f, .50f, 0.0f,
               // -.5f, -.50f, 0.0f,
               // .50f, .50f, 0.0f,
               // -.50f, -.50f, 0.0f,
               // .50f, -.50f, 0.0f,
               // .50f, .50f, 0.0f
            };
            _vertices = vertices;
        }

        public void GlobalToLocalCoordinates()
        {

        }

        private void CreateVertexBuffer()
        {
            GL.CreateBuffers(1, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Length*sizeof(float)), handle.AddrOfPinnedObject(),
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

        public override void Use(double? iTime = null, int index = 0)
        {
            _texture.Use(TextureUnit.Texture0);

            GL.UseProgram(handle);
            GL.BindVertexArray(_vertexArray);
            Console.WriteLine(GL.GetError().ToString());

            int location = GL.GetUniformLocation(handle, "iResolution");
            GL.Uniform2(location, (float)_displaySettings.Width, (float)_displaySettings.Height);



            
            var transpose = GL.GetUniformLocation(handle, "transpose");
            GL.Uniform2(transpose, GetTransposeVector());

            var sizeLocation = GL.GetUniformLocation(handle, "size");
            GL.Uniform2(sizeLocation, Size.X/2f, Size.Y/2f);

            

            //GL.UniformMatrix4(1, false, ref scale);
            Console.WriteLine(GL.GetError().ToString());
            
        }

        public void Render(GuiObject guiObject)
        {

        }
        public void UseAgain()
        {
            _texture.Use(TextureUnit.Texture0);

            GL.UseProgram(handle);
            GL.BindVertexArray(_vertexArray);
            Console.WriteLine(GL.GetError().ToString());

            int location = GL.GetUniformLocation(handle, "iResolution");
            GL.Uniform2(location, _displaySettings.Width / 2f, _displaySettings.Height / 2f);




            var transpose = GL.GetUniformLocation(handle, "transpose");
            GL.Uniform2(transpose, new Vector2((float)Position.X - _displaySettings.Width / 2f, (float)Position.Y+50 - _displaySettings.Height / 2f));

            var sizeLocation = GL.GetUniformLocation(handle, "size");
            GL.Uniform2(sizeLocation, Size.X / 2f, Size.Y / 2f);



            //GL.UniformMatrix4(1, false, ref scale);
            Console.WriteLine(GL.GetError().ToString());

        }
        private Vector2 GetTransposeVector()
        {
            Vector2 transposeVector = new Vector2();

            switch (_anchor)
            {
                case ElementAnchors.BottomLeft:
                    transposeVector = new Vector2((float)Position.X - _displaySettings.Width / 2f, (float)Position.Y - _displaySettings.Height / 2f);
                    break;
                case ElementAnchors.TopLeft:
                    transposeVector = new Vector2((float)Position.X - _displaySettings.Width / 2f, -(float)Position.Y + _displaySettings.Height / 2f);
                    break;
                case ElementAnchors.TopRight:
                    transposeVector = new Vector2(-(float)Position.X + _displaySettings.Width / 2f, -(float)Position.Y + _displaySettings.Height / 2f);
                    break;
                case ElementAnchors.BottomRight:
                    transposeVector = new Vector2(-(float)Position.X + _displaySettings.Width / 2f, (float)Position.Y - _displaySettings.Height / 2f);
                    break;
                default:
                    break;
            }


            return transposeVector;
        }
        public enum ElementAnchors
        {
            BottomLeft,
            TopLeft,
            TopRight,
            BottomRight
        }
    }

    public class DisplaySettings
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public DisplaySettings(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

    }
}
