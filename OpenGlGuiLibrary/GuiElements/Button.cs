using OpenTK.Mathematics;
using System;

namespace OpenGlGuiLibrary.GuiElements
{
    public class Button : GuiElement
    {
        public Button(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight, GuiElement parent = null)
            : base(size, position, shader, displaySettings, elementAnchor, parent) {
            SetText("Button");
        }
    }
}
