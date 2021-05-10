using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGlGuiLibrary.GuiElements
{
    public class Textbox: GuiElement, ISelectable
    {
        public Textbox(Vector2 size, Vector2 position, GuiElementShader shader, DisplaySettings displaySettings, ElementAnchors elementAnchor = ElementAnchors.TopRight, GuiElement parent = null)
            : base(size, position, shader, displaySettings, elementAnchor, parent)
        {
            SetText("Textbox");
        }

        public float GetActivePosition()
        {
            return 0;
        }
    }

    public interface ISelectable
    {
        float GetActivePosition();
    }

    public class TextboxShader : GuiElementShader
    {
        public TextboxShader(string vertexPath, string fragmentPath, IDisplaySettings displaySettings):base(vertexPath, fragmentPath, displaySettings)
        { }

        public void Render(Textbox textbox)
        {
            this.Use();
            SetUniforms(textbox);

            if (textbox.Texture != null)
            {
                textbox.Texture.Use(TextureUnit.Texture0);
            }

            GL.BindVertexArray(_vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        protected virtual void SetUniforms(Textbox textbox)
        {
            int location = GL.GetUniformLocation(handle, "iResolution");
            GL.Uniform2(location, textbox.Placement.GetiResolution());

            //var objectPosition = guiObject.Position;
            var transpose = GL.GetUniformLocation(handle, "transpose");
            GL.Uniform2(transpose, textbox.Placement.GetTransposeVector());

            var sizeLocation = GL.GetUniformLocation(handle, "size");
            GL.Uniform2(sizeLocation, textbox.Placement.GetSize());

            var isClicked = GL.GetUniformLocation(handle, "isClicked");
            GL.Uniform1(isClicked, textbox.IsClicked);

            var activePosition = GL.GetUniformLocation(handle, "activePosition");
            GL.Uniform1(activePosition, textbox.GetActivePosition());
        }
    }
}
