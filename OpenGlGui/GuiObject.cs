using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenGlGui
{
    public class GuiObject
    {
        //protected float[] _vertices;
        protected DisplaySettings _displaySettings;
        protected ElementAnchors _anchor;
        public Vector2i Size { get; protected set; }
        public Vector2i Position { get; protected set; }
        public Texture Texture { get; private set; }
        private readonly GuiObjectShader _shader;
        public int Id { get; protected set; }
        public string Text { get; private set; }

        public float IsClicked { get; set; } = 0;

        public GuiObject(Vector2i size, Vector2i position, GuiObjectShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight)
        {
            _shader = shader;
            Position = position;
            Size = size;
            _displaySettings = displaySettings;
            _anchor = elementAnchor;
        }

        public void SetText(string text)
        {
            Text = text;
            var bitmap = CreateBitmapImage(text);
            bitmap.Save("test.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            Texture = Texture.LoadFromImage(bitmap);
        }
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


        public Vector2 GetiResolution()
        {
            return new Vector2(_displaySettings.Width, _displaySettings.Height);
        }
        public Vector2 GetSize()
        {
            return new Vector2(Size.X, Size.Y);
        }
        public Vector2 GetTransposeVector()
        {
            Vector2 transposeVector = new Vector2();

            switch (_anchor)
            {
                case ElementAnchors.BottomLeft:
                    transposeVector = new Vector2((float)Position.X - _displaySettings.Width / 2f + (float)Size.X / 2f, (float)Position.Y - _displaySettings.Height / 2f + (float)Size.Y / 2f);
                    break;
                case ElementAnchors.TopLeft:
                    transposeVector = new Vector2((float)Position.X - _displaySettings.Width / 2f + (float)Size.X / 2f, -(float)Position.Y + _displaySettings.Height / 2f - (float)Size.Y / 2f);
                    break;
                case ElementAnchors.TopRight:
                    transposeVector = new Vector2(-(float)Position.X + _displaySettings.Width / 2f - (float)Size.X / 2f, -(float)Position.Y + _displaySettings.Height / 2f - (float)Size.Y / 2f);
                    break;
                case ElementAnchors.BottomRight:
                    transposeVector = new Vector2(-(float)Position.X + _displaySettings.Width / 2f - (float)Size.X / 2f, (float)Position.Y - _displaySettings.Height / 2f + (float)Size.Y / 2f);
                    break;
                default:
                    break;
            }

            return transposeVector;
        }

        public Vector2i GetWorldTopRightPosition()
        {
            Vector2i transposeVector = new Vector2i();

            switch (_anchor)
            {
                case ElementAnchors.BottomLeft:
                    transposeVector = Position;
                    break;
                case ElementAnchors.TopLeft:
                    transposeVector = new Vector2i(Position.X, -Position.Y + _displaySettings.Height + Size.Y/2);
                    break;
                case ElementAnchors.TopRight:
                    transposeVector = new Vector2i(-Position.X + _displaySettings.Width, -Position.Y + _displaySettings.Height);
                    break;
                case ElementAnchors.BottomRight:
                    transposeVector = Position;
                    break;
                default:
                    break;
            }

            return transposeVector;
        }

        public bool IsInCoordinates(Vector2i coordinates)
        {
            // Ich glaub ich hab oben und unten verwechselt.. *seufz*
            coordinates.Y = _displaySettings.Height - coordinates.Y;
            var maxPos = GetWorldTopRightPosition();
            var diff = maxPos - coordinates;

            if (diff.X >= 0 && diff.X <= Size.X && diff.Y <= Size.Y && diff.Y >= 0)
            {
                Console.WriteLine("I was clicked!! for real!! My Text is: " + this.Text);
                IsClicked = 1;
                return true;
            }
            return false;
        }
        public void Render()
        {
            _shader.Render(this);
        }

        public enum ElementAnchors
        {
            BottomLeft,
            TopLeft,
            TopRight,
            BottomRight
        }
    }

    public class Gui
    {
        public List<GuiObject> _guiObjects { get; protected set; } = new List<GuiObject>();
        protected GuiObject _clickedObject;
        int id=0;
        public Gui() { }

        public void RenderAll()
        {
            foreach (var item in _guiObjects)
            {
                item.Render();
            }
        }

        public void AddGuiObject(GuiObject guiObject)
        {
            guiObject.SetText("test " + id);
            id++;
            _guiObjects.Add(guiObject);
        }

        public bool IsAnyGuiClicked(Vector2i coordinates)
        {
            foreach (var item in _guiObjects)
            {
                if (item.IsInCoordinates(coordinates))
                {
                    _clickedObject = item;
                    return true;
                }
                
            }

            _clickedObject = null;
            return false;
        }

        public void Unclick()
        {
            if (_clickedObject != null)
            {
                _clickedObject.IsClicked = 0;
            }
        }
    }
}
