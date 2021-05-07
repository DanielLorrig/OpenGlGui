using OpenTK.Mathematics;

namespace OpenGlGui.GuiElements
{
    public class ElementGroup : GuiElement, IMoveable
    {
        public ElementGroup(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight)
            : base(size, position, shader, displaySettings, elementAnchor) { }

        public void Move(Vector2 delta)
        {
            if (IsClicked > 0)
            {
                _relativeAnchorPosition += delta;
            }
        }
    }

    public interface IMoveable
    {
        void Move(Vector2 delta);
    }
}
