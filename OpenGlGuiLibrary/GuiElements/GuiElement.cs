using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenGlGuiLibrary.GuiElements
{
    public abstract class GuiElement
    {
        public Placement Placement { get; private set; }
        public Texture Texture { get; private set; }

        public Style Style { get; protected set; } = new Style();

        protected readonly GuiElementShader _shader;
        public int Id { get; protected set; }
        public string Text { get; protected set; }
        public float IsClicked { get; set; } = 0;

        public GuiElement Parent { get; private set; }
        public GuiElement(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.BottomLeft, GuiElement parent = null)
        {
            SetParent(parent);
            _shader = shader;

            Placement = new Placement(size, position, this, displaySettings, elementAnchor);
        }
        private void SetParent(GuiElement parent)
        {
            if (parent == null)
            {
                return;
            }
            if (!parent.HasAncestor(this))
            {
                Parent = parent;
                Parent.Children.Add(this);
            }
            else
            {
                throw new Exception("You tried to make an GuiElement its own ancestor");
            }

        }
        private bool HasAncestor(GuiElement guiElement)
        {
            if (Parent == null)
            {
                return false;
            }
            if (Parent != null)
            {
                return Parent.HasAncestor(guiElement);
            }
            else return true;
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
            Bitmap objBmpImage = new Bitmap(Convert.ToInt32(Placement.Size.X), Convert.ToInt32(Placement.Size.Y));

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 22, FontStyle.Bold, GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            var intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            var intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(Convert.ToInt32(Placement.Size.X), Convert.ToInt32(Placement.Size.Y)));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.FromArgb(0, 255, 255, 255));
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            StringFormat sf = new StringFormat();
            //sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(20, 20, 20)), (Placement.Size.X - intWidth) / 2, (Placement.Size.Y - intHeight) / 2);
            objGraphics.Flush();

            return objBmpImage;
        }

        public List<GuiElement> Children { get; private set; } = new List<GuiElement>();

        public void Render()
        {
            _shader.Render(this);
            Children.ForEach(x => x.Render());
        }

        public GuiElement IsInCoordinates(Vector2 coordinates)
        {
            bool isInCoordinates = Placement.IsInCoordinates(coordinates);

            if (isInCoordinates)
            {
                if (Children.Any())
                {
                    foreach (var child in Children)
                    {
                        var clickedChild = child.IsInCoordinates(coordinates);
                        if (clickedChild != null)
                        {
                            return clickedChild;
                        }
                    }
                }
                IsClicked = 1;
                return this;
            }

            return null;
        }

    }

    public enum ElementAnchors
    {
        BottomLeft,
        TopLeft,
        TopRight,
        BottomRight
    }

    public class Style
    {
        public Vector3 Color { get; set; }
        public Vector3 BackgroundColor { get; set; }
        public Vector3 BorderColor { get; set; }
    }
}
