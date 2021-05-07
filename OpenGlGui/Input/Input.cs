using OpenGlGui.GuiElements;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlGui.Input
{
    public class Input
    {



        public void Run()
        {

        }
    }

    public class MouseEventManager
    {
        readonly NativeWindow _window;
        readonly Gui _gui;
        protected GuiElement _triggeredElement;

        MouseButton _pressedMouseButton;
        bool _mouseButtonIsDown = false;


        public MouseEventManager(NativeWindow window, Gui gui)
        {
            _window = window;
            _gui = gui;
        }

        public void MouseDown(MouseButtonEventArgs mouseButtonEvent)
        {
            if (!_mouseButtonIsDown)
            {
                _pressedMouseButton = mouseButtonEvent.Button;
                _mouseButtonIsDown = true;

                _triggeredElement = _gui.TryGetClickedObject(_window.MouseState.Position);

                if (_triggeredElement == null)
                {
                    _window.CursorGrabbed = true;
                    _window.CursorVisible = false;
                }
            }
        }

        public void MouseUp(MouseButtonEventArgs mouseButtonEvent)
        {
            if (_mouseButtonIsDown && mouseButtonEvent.Button == _pressedMouseButton)
            {
                _window.CursorGrabbed = false;
                _window.CursorVisible = true;
                _mouseButtonIsDown = false;
            }
        }
        public void MouseMove()
        {
            if (_triggeredElement != null && _triggeredElement is IMoveable)
            {
                var delta = _window.MouseState.Delta;
                delta.X *= -1;
                ((IMoveable)_triggeredElement).Move(delta);
            }
        }

        public enum MouseAction
        {
            LeftClick,
            Drag,
            RightClick
        }
    }

    public class MouseActionDataLink
    {
        Vector2 Position;
        MouseEventManager.MouseAction Action;
    }
}
