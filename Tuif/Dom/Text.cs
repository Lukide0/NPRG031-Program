namespace Tuif.Dom;

/// <summary>
/// Třída reprezentující textový uzel.
/// </summary>
public class Text : Node
{
    /// <summary>
    /// Textová data.
    /// </summary>
    public string Data;

    /// <summary>
    /// Barva textu.
    /// </summary>
    public Color Color = Color.DefaultForeground;

    /// <summary>
    /// Konstruktor textového uzlu s určenou šířkou, výškou, textem a barvou.
    /// </summary>
    /// <param name="width">Šířka uzlu.</param>
    /// <param name="height">Výška uzlu.</param>
    /// <param name="text">Textová data.</param>
    /// <param name="color">Barva textu.</param>
    public Text(uint width, uint height, string text, Color color) : base(width, height)
    {
        Data = text;
        Color = color;
    }

    /// <summary>
    /// Konstruktor textového uzlu s určeným textem a barvou.
    /// </summary>
    /// <param name="text">Textová data.</param>
    /// <param name="color">Barva textu.</param>
    public Text(string text, Color color) : base((uint)text.Length, 1)
    {
        Data = text;
        Color = color;
    }

    /// <inheritdoc/>
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
