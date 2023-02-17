using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlGuiLibrary.GuiElements
{
    public class TrackBar : GuiElement
    {
        private Bar _bar;
        public IntRange ValueRange { get; set; }

        private int _currentValue;
        public int CurrentValue { 
            get { return _currentValue; } 
            set { CurrentValue = Math.Max(0, Math.Min(ValueRange.Max, value)); } 
        }
        public TrackBar(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.BottomLeft, GuiElement parent = null) : base(size, position, shader, displaySettings, elementAnchor, parent)
        {

        }

        public double NormalizedBarPosition()
        {
            return (double)CurrentValue / (double)ValueRange.Span;
        }
    }

    public class Bar : GuiElement
    {
        public Bar(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.BottomLeft, GuiElement parent = null) : base(size, position, shader, displaySettings, elementAnchor, parent)
        {
        }
    }

    public struct IntRange
    {
        public int Min;
        public int Max;

        public int Span => Max - Min;
    }

    public struct Vector2Range
    {
        public Vector2 Min;
        public Vector2 Max;
    }
}
