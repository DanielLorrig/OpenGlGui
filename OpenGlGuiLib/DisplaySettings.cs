namespace OpenGlGui
{
    public class DisplaySettings: IDisplaySettings
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public DisplaySettings(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
