namespace Tuif.Dom;

public class Rect : Node
{

    public char FillChar = '█';
    public Color FillColor = Color.DefaultForeground;
    public Rect(uint width, uint height) : base(width, height)
    {
    }
    /// <inheritdoc/>
    public override void Render(Buffer buff)
    {
        for (uint y = 0; y < _height; y++)
        {
            uint index = buff.GetIndex(_posX, _posY + y);
            for (int x = 0; x < _width; x++)
            {
                buff.Data[index + x] = FillChar;
                buff.Foreground[index + x] = FillColor;
            }
        }
    }
}
