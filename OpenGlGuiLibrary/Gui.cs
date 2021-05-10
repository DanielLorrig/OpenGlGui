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
        public readonly GuiElementShader TextboxShader;

        public List<GuiElement> _guiObjects { get; protected set; } = new List<GuiElement>();
        public List<Button> Buttons { get; protected set; } = new List<Button>();
        protected GuiElement _clickedObject;
        int id=0;
        public Gui(IDisplaySettings displaySettings, NativeWindow nativeWindow) 
        {
            MouseEventManager = new MouseEventManager(nativeWindow, this);
            _nativeWindow = nativeWindow;
            _displaySettings = displaySettings;

            bool isDebug = true;

            if (isDebug)
            {
                string boxVertexShader = ".\\Shader\\BoxShader.vert";
                string boxFragShader = ".\\Shader\\BoxShader.frag";
                string elementGroupFragShader = ".\\Shader\\ElementGroupShader.frag";
                string labelFragShader = ".\\Shader\\LabelShader.frag";
                string textboxFragShader = ".\\Shader\\TextboxShader.frag";

                GuiElementShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
                BoxShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
                ElementGroupShader = new GuiElementShader(boxVertexShader, elementGroupFragShader, _displaySettings);
                LabelShader = new GuiElementShader(boxVertexShader, labelFragShader, _displaySettings);
                TextboxShader = new GuiElementShader(boxVertexShader, textboxFragShader, _displaySettings);
            }
            else
            {
                string boxVertexShader = ShaderCode.BoxVertexShader;
                string boxFragShader = ShaderCode.BoxFragmentShader;
                string elementGroupFragShader = ShaderCode.ElementGroupFragmentShader;
                string labelFragShader = ShaderCode.LabelFragmentShader;

                GuiElementShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
                BoxShader = new GuiElementShader(boxVertexShader, boxFragShader, _displaySettings);
                ElementGroupShader = new GuiElementShader(boxVertexShader, elementGroupFragShader, _displaySettings);
                LabelShader = new GuiElementShader(boxVertexShader, labelFragShader, _displaySettings);
            }
            
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
            LabelShader.Recompile();
            TextboxShader.Recompile();
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

    public static class ShaderCode
    {
        public static string BoxVertexShader = @"#version 400 core

                                                layout(location = 0) in vec3 aPosition;
                                                uniform vec2 iResolution;
                                                uniform vec2 transpose;
                                                uniform vec2 size;
                                                void main()
                                                {
	                                                gl_Position = vec4(aPosition.xy * size / iResolution + 2.0 * transpose / iResolution, 0.0, 1.0);
                                                }";

        public static string BoxFragmentShader = @"#version 400

                                                    out vec4 FragColor;
                                                    //in vec4 gl_FragCoord;
                                                    uniform vec2 iResolution;
                                                    uniform vec2 transpose;
                                                    uniform vec2 size;
                                                    uniform float isClicked;
                                                    uniform sampler2D texture0;
                                                    float sdRoundBox(in vec2 p, in vec2 b, in vec4 r)
                                                    {
                                                        r.xy = (p.x > 0.0) ? r.xy : r.zw;
                                                        r.x = (p.y > 0.0) ? r.x : r.y;

                                                        vec2 q = abs(p) - b + r.x;
                                                        return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r.x;
                                                    }
                                                    void main()
                                                    {
                                                        vec2 p = gl_FragCoord.xy - transpose - iResolution / 2.0;
                                                        vec2 q = size / 2.0;
                                                        float d = sdRoundBox(p, q, vec4(5.0));
                                                        float isInsideBorder = 1.0 - smoothstep(-3.0, 0.0, d);
                                                        vec3 border = vec3(0.0, 0.25, 0.25);
                                                        vec3 inside = vec3(0.0, 0.25, 0.4) * isInsideBorder;
                                                        vec3 col = border + inside;
                                                        col = col - isClicked * col * 0.45;
                                                        float visibility = max(0., 1. - step(-0.0, d));
                                                        FragColor = vec4(col, visibility);
                                                        vec2 texCoords = vec2(p.x / size.x, -p.y / size.y) - 0.5;
                                                        vec4 texture = texture(texture0, texCoords);
                                                        vec3 texCol = texture.xyz;
                                                        texCol = (1.0 - isClicked) * texCol + isClicked * vec3(0.45);
                                                        texture = vec4(texCol, texture.w);
                                                        vec4 objAndTextCol = vec4(col, 1.0) * (1.0 - texture.w) + texture * texture.w;
                                                        FragColor = objAndTextCol;
                                                    }";

        public static string ElementGroupFragmentShader = @"#version 400
                                                            out vec4 FragColor;
                                                            uniform vec2 iResolution;
                                                            uniform vec2 transpose;
                                                            uniform vec2 size;
                                                            uniform float isClicked;
                                                            float sdRoundBox(in vec2 p, in vec2 b, in vec4 r)
                                                            {
                                                                r.xy = (p.x > 0.0) ? r.xy : r.zw;
                                                                r.x = (p.y > 0.0) ? r.x : r.y;
                                                                vec2 q = abs(p) - b + r.x;
                                                                return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r.x;
                                                            }
                                                            void main()
                                                            {
                                                                vec2 p = gl_FragCoord.xy - transpose - iResolution / 2.0;
                                                                vec2 q = size / 2.0;
                                                                float d = sdRoundBox(p, q, vec4(2.0));
                                                                float isInsideBorder = 1.0 - smoothstep(-3.0, 0.0, d);
                                                                float isInside = step(0.0, -d);
                                                                FragColor = vec4(vec3(0.3),isInside);
                                                            }";

        public static string LabelFragmentShader = @"#version 400
                                                    out vec4 FragColor;
                                                    uniform vec2 iResolution;
                                                    uniform vec2 transpose;
                                                    uniform vec2 size;
                                                    uniform float isClicked;
                                                    uniform sampler2D texture0;
                                                    void main()
                                                    {
                                                        vec2 p = gl_FragCoord.xy - transpose - iResolution / 2.0;
                                                        vec2 q = size / 2.0;
                                                        vec2 texCoords = vec2(p.x / size.x, -p.y / size.y) - 0.5;
                                                        vec4 texture = texture(texture0, texCoords);
                                                        FragColor = texture;
                                                    }";
    }
}
