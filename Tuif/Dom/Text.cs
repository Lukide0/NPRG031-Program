namespace Tuif.Dom;

public class Text : Node
{
    public string Data;
    public Color Color = Color.DefaultForeground;

    public Text(uint width, uint height, string text, Color color) : base(width, height)
    {
        Data = text;
        Color = color;
    }

    public Text(string text, Color color) : base((uint)text.Length, 1)
    {
        Data = text;
        Color = color;
    }

    public override void Render(Buffer buff)
    {
        uint centerY = (_height - 1) / 2;

        if (_width < Data.Length) 
        {       
            buff.Write(Data.Substring(0, (int)_width - 2) + "..", _posX, centerY + _posY);

        }
        else 
        {
            uint centerX = (_width - (uint)Data.Length) / 2;
            buff.Write(Data, centerX + _posX, centerY + _posY);
        }


    }
}