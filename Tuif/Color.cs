namespace Tuif;

public class Color
{
    public static readonly Color DefaultBackground = new Color(0x151820);
    public static readonly Color DefaultForeground = new Color(255,255,255);

    public byte R;
    public byte G;
    public byte B;

    public Color(byte red, byte green, byte blue) 
    {
        this.R = red;
        this.G = green;
        this.B = blue;
    }

    public Color(uint hex) 
    {
        // zachová jen 8 spodních bitů => 0xRRGGBB
        hex &= 0xFFFFFF;

        this.R = (byte)(hex >> 16);
        this.G = (byte)((hex & 0xFF00) >> 8);
        this.B = (byte)(hex & 0xFF);
    }

    public string GetSequence() 
    {
        return $"{R};{G};{B}";
    }
}
