using OpenTK.Mathematics;

namespace OpenGlGuiLibrary.GuiElements
{
    public class Placement
    {
        protected DisplaySettings _displaySettings;
        protected ElementAnchors _anchor;
        protected Vector2i _centerToAnchorMultiplier;
        protected Vector2i _anchorToCenterMultiplier;
        public Vector2 _relativeAnchorPosition { get; protected set; } // relativ to parent corner.
        public Vector2 Size { get; protected set; }
        protected GuiElement _guiElement;

        public Placement(Vector2 size, Vector2 position, GuiElement guiElement, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.BottomLeft)
        {
            Size = size;
            _relativeAnchorPosition = position;
            _guiElement = guiElement;
            _displaySettings = displaySettings;
            _anchor = elementAnchor;

            SetDirections();
        }

        public void SetPosition(Vector2 newPosition)
        {
            if (_guiElement.Parent != null)
            {
                var parentSize = _guiElement.Parent.Placement.Size;
                var positionInParent = parentSize - newPosition;
                if (positionInParent.X < 0 || positionInParent.X > parentSize.X || positionInParent.Y < 0 || positionInParent.Y > parentSize.Y)
                {
                    //invalid position
                    return;
                }
                else
                {
                    _relativeAnchorPosition = newPosition;
                }
            }
            _relativeAnchorPosition = newPosition;
        }
        protected void SetDirections()
        {
            _centerToAnchorMultiplier = GetCenterToElementAnchorMultiplier(_anchor);
            _anchorToCenterMultiplier = _centerToAnchorMultiplier * -1;
        }
        protected Vector2i GetCenterToElementAnchorMultiplier(ElementAnchors elementAnchor)
        {
            var result = Vector2i.Zero;
            switch (elementAnchor)
            {
                case ElementAnchors.BottomLeft:
                    result = new Vector2i(-1, -1);
                    break;
                case ElementAnchors.TopLeft:
                    result = new Vector2i(-1, 1);
                    break;
                case ElementAnchors.TopRight:
                    result = new Vector2i(1, 1);
                    break;
                case ElementAnchors.BottomRight:
                    result = new Vector2i(1, -1);
                    break;
            }
            return result;
        }

        protected Vector2 GetElementCenterAbsolute()
        {
            var center = GetAbsoluteAnchorPosition() + Size / 2 * _anchorToCenterMultiplier;
            return center;
        }
        protected Vector2 GetElementCornerAbsolute(ElementAnchors elementAnchor)
        {
            var multiplier = GetCenterToElementAnchorMultiplier(elementAnchor);
            var center = GetElementCenterAbsolute();
            var corner = center + Size / 2 * multiplier;
            return corner;
        }

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
            Vector2 transposeVector = GetElementCenterAbsolute();

            return transposeVector;
        }

        protected Vector2 GetAbsoluteAnchorPosition() // Pixelspace
        {
            Vector2i directionMultiplier = Vector2i.Zero;
            if (_guiElement.Parent != null)
            {
                var pos = _guiElement.Parent.Placement.GetElementCornerAbsolute(_anchor) + _relativeAnchorPosition * _anchorToCenterMultiplier;
                return pos;
            }
            else
            {
                var pos = _relativeAnchorPosition * _anchorToCenterMultiplier + new Vector2(_displaySettings.Width / 2f, _displaySettings.Height / 2f) * _centerToAnchorMultiplier;
                return pos;
            }
        }

        public Vector2 GetWorldTopRightPosition()
        {
            var center = GetElementCenterAbsolute();
            Vector2 transposeVector = center + Size * 0.5f;

            return transposeVector;
        }
        public bool IsInCoordinates(Vector2 coordinates)
        {
            var maxPos = GetWorldTopRightPosition();
            var diff = maxPos - coordinates;

            if (diff.X >= 0 && diff.X <= Size.X && diff.Y <= Size.Y && diff.Y >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
