using OpenTK.Mathematics;
using System;

namespace OpenGlGui.GuiElements
{
    public class Button : GuiElement
    {
        //public Button(Vector2i size, Vector2i position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight)
        //    : base(size, position, shader, displaySettings, elementAnchor) { }

        public Button(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight, GuiElement parent = null)
            : base(size, position, shader, displaySettings, elementAnchor, parent) {
            SetText("Button");
        }
    }

    public class Label: GuiElement
    {
        public Label(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight, GuiElement parent = null)
            : base(size, position, shader, displaySettings, elementAnchor, parent)
        {
            SetText("Label");
        }
    }
}
