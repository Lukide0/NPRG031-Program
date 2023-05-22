using System.Text;

namespace Tuif;

public class Buffer
{
    /// <summary>
    /// Šířka bufferu
    /// </summary>
    public uint Width { get; }

    /// <summary>
    /// Výška bufferu
    /// </summary>
    public uint Height { get; }

    /// <summary>
    /// Velikost bufferu (výška * šířka)
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Barva popředí
    /// </summary>
    public Color[] Foreground;

    /// <summary>
    /// Barva pozadí
    /// </summary>
    public Color[] Background;

    /// <summary>
    /// Znak
    /// </summary>
    public char[] Data;

    public Buffer(uint width, uint height)
    {
        Size = (int)(width * height);
        Width = width;
        Height = height;

        Foreground = Enumerable.Repeat(Color.DefaultForeground, Size).ToArray();
        Background = Enumerable.Repeat(Color.DefaultBackground, Size).ToArray();
        Data = Enumerable.Repeat(' ', Size).ToArray();
    }

    /// <summary>
    /// Zapíše znak na zadanou pozici.
    /// </summary>
    /// <param name="ch">Znak k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    public void Write(char ch, uint x, uint y)
    {
        uint index = GetIndex(x, y);

        Data[index] = ch;
    }
    /// <summary>
    /// Zapíše znak na zadanou pozici s určenými barvami popředí a pozadí.
    /// </summary>
    /// <param name="ch">Znak k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <param name="fg">Barva popředí.</param>
    /// <param name="bg">Barva pozadí.</param>
    public void Write(char ch, uint x, uint y, Color fg, Color bg)
    {
        uint index = GetIndex(x, y);

        Data[index] = ch;
        Foreground[index] = fg;
        Background[index] = bg;
    }
    /// <summary>
    /// Zapíše znak na zadanou pozici s určenou barvou popředí.
    /// </summary>
    /// <param name="ch">Znak k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <param name="fg">Barva popředí.</param>
    public void Write(char ch, uint x, uint y, Color fg)
    {
        uint index = GetIndex(x, y);

        Data[index] = ch;
        Foreground[index] = fg;
    }

    /// <summary>
    /// Zapíše text na zadanou pozici.
    /// </summary>
    /// <param name="text">Text k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    public void Write(string text, uint x, uint y)
    {
        uint index = GetIndex(x, y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index + i] = text[i];
        }
    }
    /// <summary>
    /// Zapíše text na zadanou pozici s určenou barvou popředí.
    /// </summary>
    /// <param name="text">Text k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <param name="fg">Barva popředí.</param>

    public void Write(string text, uint x, uint y, Color fg)
    {
        uint index = GetIndex(x, y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index + i] = text[i];
            Foreground[index + i] = fg;
        }
    }

    /// <summary>
    /// Zapíše text na zadanou pozici s určenými barvami popředí a pozadí.
    /// </summary>
    /// <param name="text">Text k zapsání.</param>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <param name="fg">Barva popředí.</param>
    /// <param name="bg">Barva pozadí.</param>
    public void Write(string text, uint x, uint y, Color fg, Color bg)
    {
        uint index = GetIndex(x, y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index] = text[i];
            Foreground[index] = fg;
            Background[index] = bg;
        }
    }

    /// <summary>
    /// Smaže hodnoty v bufferu
    /// </summary>
    public void Clear()
    {
        Foreground = Enumerable.Repeat(Color.DefaultForeground, Size).ToArray();
        Background = Enumerable.Repeat(Color.DefaultBackground, Size).ToArray();
        Data = Enumerable.Repeat(' ', Size).ToArray();
    }
    /// <summary>
    /// Převede X-ovou a Y-ovou souřadnici na index
    /// </summary>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <returns>Index</returns>
    public uint GetIndex(uint x, uint y) => y * Width + x;

    /// <summary>
    /// Získá znak na zadané pozici
    /// </summary>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    /// <returns>Znak</returns>
    public char Read(uint x, uint y) => Data[GetIndex(x, y)];

}
