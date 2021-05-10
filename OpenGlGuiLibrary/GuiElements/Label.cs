using OpenTK.Mathematics;

namespace OpenGlGuiLibrary.GuiElements
{
    public class Label: GuiElement
    {
        public Label(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight, GuiElement parent = null)
            : base(size, position, shader, displaySettings, elementAnchor, parent)
        {
            SetText("Label");
        }
    }
}
