using OpenGlGuiLibrary.GuiElements;
using OpenGlGuiLibrary.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.Reflection;

namespace OpenGlGuiLibrary
{
    public class Gui
    {
        NativeWindow _nativeWindow;
        public readonly MouseEventManager MouseEventManager;
        protected readonly IDisplaySettings _displaySettings;
        
        public readonly GuiElementShader ElementGroupShader;
        public readonly GuiElementShader GuiElementShader;
        public readonly GuiElementShader BoxShader;
        public readonly GuiElementShader LabelShader;

        public List<GuiElement> _guiObjects { get; protected set; } = new List<GuiElement>();
        public List<Button> Buttons { get; protected set; } = new List<Button>();
        protected GuiElement _clickedObject;
        int id=0;
        public Gui(IDisplaySettings displaySettings, NativeWindow nativeWindow) 
        {
            string boxVertexShader = ".\\Shader\\BoxShader.vert";
            string boxFragShader = ".\\Shader\\BoxShader.frag";

            MouseEventManager = new MouseEventManager(nativeWindow, this);
            _nativeWindow = nativeWindow;
            _displaySettings = displaySettings;
            GuiElementShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
            BoxShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
            ElementGroupShader = new GuiElementShader(boxVertexShader, ".\\Shader\\ElementGroupShader.frag", _displaySettings);
            LabelShader = new GuiElementShader(boxVertexShader, ".\\Shader\\LabelShader.frag", _displaySettings);
        }

        public void RenderAll()
        {
            foreach (var item in _guiObjects)
            {
                item.Render();
            }
        }

        public void AddGuiElement(GuiElement guiElement)
        {
            _guiObjects.Add(guiElement);
        }
        public void AddButton(Button guiObject)
        {
            guiObject.SetText("test " + id);
            id++;
            Buttons.Add(guiObject);
            _guiObjects.Add(guiObject);
        }

        //public bool IsAnyGuiClicked(Vector2i coordinates)
        //{
        //    foreach (var item in Buttons)
        //    {
        //        if (item.IsInCoordinates(coordinates))
        //        {
        //            _clickedObject = item;
        //            return true;
        //        }
                
        //    }

        //    _clickedObject = null;
        //    return false;
        //}

        public void Unclick()
        {
            if (_clickedObject != null)
            {
                _clickedObject.IsClicked = 0;
            }
        }

        public void RecompileShader()
        {
            GuiElementShader.Recompile();
            BoxShader.Recompile();
            ElementGroupShader.Recompile();
        }

        public GuiElement TryGetClickedObject(Vector2 position)
        {
            position.Y = _displaySettings.Height - position.Y;
            position -= 0.5f * new Vector2(_displaySettings.Width, _displaySettings.Height);

            foreach (var element in _guiObjects)
            {
                var possiblyClickedElement = element.IsInCoordinates(position);
                if (possiblyClickedElement != null)
                {
                    _clickedObject = possiblyClickedElement;
                    return _clickedObject;
                }
            }
            _clickedObject = null;
            return _clickedObject;
        }
    }
}
