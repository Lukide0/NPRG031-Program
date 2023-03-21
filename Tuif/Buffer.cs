using System.Text;

namespace Tuif;

public class Buffer
{
    public uint Width { get; }
    public uint Height { get; }
    public int Size { get; }
    public Color[] Foreground;
    public Color[] Background;
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

    public void Write(char ch, uint x, uint y)
    {
        uint index = GetIndex(x,y);

        Data[index] = ch;
    }

    public void Write(char ch, uint x, uint y, Color fg, Color bg)
    {
        uint index = GetIndex(x,y);

        Data[index] = ch;
        Foreground[index] = fg;
        Background[index] = bg;
    }

    public void Write(char ch, uint x, uint y, Color fg)
    {
        uint index = GetIndex(x,y);

        Data[index] = ch;
        Foreground[index] = fg;
    }

    public void Write(string text, uint x, uint y)
    {
        uint index = GetIndex(x,y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index + i] = text[i];
        }
    }

    public void Write(string text, uint x, uint y, Color fg)
    {
        uint index = GetIndex(x,y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index + i] = text[i];
            Foreground[index + i] = fg;
        }
    }

    public void Write(string text, uint x, uint y, Color fg, Color bg)
    {
        uint index = GetIndex(x,y);

        for (int i = 0; i < text.Length; ++i)
        {
            Data[index] = text[i];
            Foreground[index] = fg;
            Background[index] = bg;
        }
    }

    public void Clear()
    {
        Foreground = Enumerable.Repeat(Color.DefaultForeground, Size).ToArray();
        Background = Enumerable.Repeat(Color.DefaultBackground, Size).ToArray();
        Data = Enumerable.Repeat(' ', Size).ToArray();
    }

    public uint GetIndex(uint x, uint y) => y * Width + x;

    public char Read(uint x, uint y) => Data[GetIndex(x,y)];

}
