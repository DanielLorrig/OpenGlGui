using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenGlGuiLibrary.GuiElements
{
    public abstract class GuiElement
    {
        //protected DisplaySettings _displaySettings;
        //protected ElementAnchors _anchor;
        //protected Vector2i _centerToAnchorMultiplier;
        //protected Vector2i _anchorToCenterMultiplier;
        //public Vector2 Size { get; protected set; }
        //protected Vector2 _relativeAnchorPosition { get; set; } // relativ to parent corner.
        public Placement Placement { get; private set; }
        public Texture Texture { get; private set; }
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

            //_relativeAnchorPosition = position;
            //Size = size;
            //_displaySettings = displaySettings;
            //_anchor = elementAnchor;

            //SetDirections();
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

        

        //protected void SetDirections()
        //{
        //    _centerToAnchorMultiplier = GetCenterToElementAnchorMultiplier(_anchor);
        //    _anchorToCenterMultiplier = _centerToAnchorMultiplier * -1;
        //}
        //protected Vector2i GetCenterToElementAnchorMultiplier(ElementAnchors elementAnchor)
        //{
        //    var result = Vector2i.Zero;
        //    switch (elementAnchor)
        //    {
        //        case ElementAnchors.BottomLeft:
        //            result = new Vector2i(-1, -1);
        //            break;
        //        case ElementAnchors.TopLeft:
        //            result = new Vector2i(-1, 1);
        //            break;
        //        case ElementAnchors.TopRight:
        //            result = new Vector2i(1, 1);
        //            break;
        //        case ElementAnchors.BottomRight:
        //            result = new Vector2i(1, -1);
        //            break;
        //    }
        //    return result;
        //}

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

        //public Vector2 GetiResolution()
        //{
        //    return new Vector2(_displaySettings.Width, _displaySettings.Height);
        //}
        //public Vector2 GetSize()
        //{
        //    return new Vector2(Size.X, Size.Y);
        //}
        //public Vector2 GetTransposeVector()
        //{
        //    Vector2 transposeVector = GetElementCenterAbsolute();

        //    return transposeVector;
        //}

        //protected Vector2 GetAbsoluteAnchorPosition() // Pixelspace
        //{
        //    Vector2i directionMultiplier = Vector2i.Zero;
        //    if (_parent != null)
        //    {
        //        var pos = _parent.GetElementCornerAbsolute(_anchor) + _relativeAnchorPosition * _anchorToCenterMultiplier;
        //        return pos;
        //    }
        //    else
        //    {
        //        var pos = _relativeAnchorPosition * _anchorToCenterMultiplier + new Vector2(_displaySettings.Width / 2f, _displaySettings.Height / 2f) * _centerToAnchorMultiplier;
        //        return pos;
        //    }
        //}

        //public Vector2 GetWorldTopRightPosition()
        //{
        //    var center = GetElementCenterAbsolute();
        //    Vector2 transposeVector =  center + Size * 0.5f;

        //    return transposeVector;
        //}

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


        //public enum ElementAnchors
        //{
        //    BottomLeft,
        //    TopLeft,
        //    TopRight,
        //    BottomRight
        //}
    }
    public enum ElementAnchors
    {
        BottomLeft,
        TopLeft,
        TopRight,
        BottomRight
    }
}
