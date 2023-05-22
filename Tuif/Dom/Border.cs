namespace Tuif.Dom;

public static class BorderChar
{
    /// <summary>
    /// Metoda pro spojení dvou znaků na základě typu ohraničení.
    /// </summary>
    /// <param name="oldChar">Původní znak.</param>
    /// <param name="newChar">Nový znak.</param>
    /// <returns>Znak vzniklý spojením původního a nového znaku na základě typu ohraničení.</returns>
    public static char MergeChars(char oldChar, char newChar)
    {
        Border a = LookupBorder(oldChar), b = LookupBorder(newChar);

        if (a == Border.Unknown)
        {
            return newChar;
        }
        else
        {
            return LookupChar(a | b);
        }
    }

    /// <summary>
    /// Metoda pro vyhledání znaku pro zadaný typ ohraničení.
    /// </summary>
    /// <param name="border">Typ ohraničení.</param>
    /// <returns>Znak pro zadaný typ ohraničení.</returns>
    public static char LookupChar(Border border)
    {
        switch (border)
        {
            case Border.LeftSingle | Border.RightSingle: return '─';
            case Border.TopSingle | Border.DownSingle: return '│';
            case Border.RightSingle | Border.DownSingle: return '┌';
            case Border.LeftSingle | Border.DownSingle: return '┐';
            case Border.TopSingle | Border.RightSingle: return '└';
            case Border.LeftSingle | Border.TopSingle: return '┘';
            case Border.TopSingle | Border.DownSingle | Border.RightSingle: return '├';
            case Border.TopSingle | Border.DownSingle | Border.LeftSingle: return '┤';
            case Border.LeftSingle | Border.RightSingle | Border.DownSingle: return '┬';
            case Border.LeftSingle | Border.RightSingle | Border.TopSingle: return '┴';
            case Border.LeftSingle | Border.RightSingle | Border.TopSingle | Border.DownSingle: return '┼';

            case Border.LeftDouble | Border.RightDouble: return '═';
            case Border.TopDouble | Border.DownDouble: return '║';
            case Border.RightDouble | Border.DownDouble: return '╔';
            case Border.LeftDouble | Border.DownDouble: return '╗';
            case Border.TopDouble | Border.RightDouble: return '╚';
            case Border.LeftDouble | Border.TopDouble: return '╝';
            case Border.TopDouble | Border.DownDouble | Border.RightDouble: return '╠';
            case Border.TopDouble | Border.DownDouble | Border.LeftDouble: return '╣';
            case Border.LeftDouble | Border.RightDouble | Border.DownDouble: return '╦';
            case Border.LeftDouble | Border.RightDouble | Border.TopDouble: return '╩';
            case Border.LeftDouble | Border.RightDouble | Border.TopDouble | Border.DownDouble: return '╬';

            case Border.RightSingle | Border.DownDouble: return '╓';
            case Border.RightDouble | Border.DownSingle: return '╒';

            case Border.LeftDouble | Border.DownSingle: return '╕';
            case Border.LeftSingle | Border.DownDouble: return '╖';

            case Border.TopSingle | Border.RightDouble: return '╘';
            case Border.TopDouble | Border.RightSingle: return '╙';

            case Border.LeftDouble | Border.TopSingle: return '╛';
            case Border.LeftSingle | Border.TopDouble: return '╜';

            case Border.TopSingle | Border.DownSingle | Border.RightDouble: return '╞';
            case Border.TopDouble | Border.DownDouble | Border.RightSingle: return '╟';

            case Border.LeftDouble | Border.TopSingle | Border.DownSingle: return '╡';
            case Border.LeftSingle | Border.TopDouble | Border.DownDouble: return '╢';

            case Border.LeftDouble | Border.RightDouble | Border.DownSingle: return '╤';
            case Border.LeftSingle | Border.RightSingle | Border.DownDouble: return '╥';

            case Border.LeftDouble | Border.RightDouble | Border.TopSingle: return '╧';
            case Border.LeftSingle | Border.RightSingle | Border.TopDouble: return '╨';

            case Border.LeftDouble | Border.RightDouble | Border.TopSingle | Border.DownSingle: return '╪';
            case Border.LeftSingle | Border.RightSingle | Border.TopDouble | Border.DownDouble: return '╫';
            default:
                return ' ';
        }
    }

    /// <summary>
    /// Metoda pro vyhledání typu ohraničení na základě zadaného znaku.
    /// </summary>
    /// <param name="ch">Znak použitý pro vyhledání typu ohraničení.</param>
    /// <returns>Typ ohraničení.</returns>
    public static Border LookupBorder(char ch)
    {
        switch (ch)
        {
            case '─': return Border.LeftSingle | Border.RightSingle;
            case '│': return Border.TopSingle | Border.DownSingle;
            case '┌': return Border.RightSingle | Border.DownSingle;
            case '┐': return Border.LeftSingle | Border.DownSingle;
            case '└': return Border.TopSingle | Border.RightSingle;
            case '┘': return Border.LeftSingle | Border.TopSingle;
            case '├': return Border.TopSingle | Border.DownSingle | Border.RightSingle;
            case '┤': return Border.TopSingle | Border.DownSingle | Border.LeftSingle;
            case '┬': return Border.LeftSingle | Border.RightSingle | Border.DownSingle;
            case '┴': return Border.LeftSingle | Border.RightSingle | Border.TopSingle;
            case '┼': return Border.LeftSingle | Border.RightSingle | Border.TopSingle | Border.DownSingle;

            case '═': return Border.LeftDouble | Border.RightDouble;
            case '║': return Border.TopDouble | Border.DownDouble;
            case '╔': return Border.RightDouble | Border.DownDouble;
            case '╗': return Border.LeftDouble | Border.DownDouble;
            case '╚': return Border.TopDouble | Border.RightDouble;
            case '╝': return Border.LeftDouble | Border.TopDouble;
            case '╠': return Border.TopDouble | Border.DownDouble | Border.RightDouble;
            case '╣': return Border.TopDouble | Border.DownDouble | Border.LeftDouble;
            case '╦': return Border.LeftDouble | Border.RightDouble | Border.DownDouble;
            case '╩': return Border.LeftDouble | Border.RightDouble | Border.TopDouble;
            case '╬': return Border.LeftDouble | Border.RightDouble | Border.TopDouble | Border.DownDouble;

            case '╓': return Border.RightSingle | Border.DownDouble;
            case '╒': return Border.RightDouble | Border.DownSingle;

            case '╕': return Border.LeftDouble | Border.DownSingle;
            case '╖': return Border.LeftSingle | Border.DownDouble;

            case '╘': return Border.TopSingle | Border.RightDouble;
            case '╙': return Border.TopDouble | Border.RightSingle;

            case '╛': return Border.LeftDouble | Border.TopSingle;
            case '╜': return Border.LeftSingle | Border.TopDouble;

            case '╞': return Border.TopSingle | Border.DownSingle | Border.RightDouble;
            case '╟': return Border.TopDouble | Border.DownDouble | Border.RightSingle;

            case '╡': return Border.LeftDouble | Border.TopSingle | Border.DownSingle;
            case '╢': return Border.LeftSingle | Border.TopDouble | Border.DownDouble;

            case '╤': return Border.LeftDouble | Border.RightDouble | Border.DownSingle;
            case '╥': return Border.LeftSingle | Border.RightSingle | Border.DownDouble;

            case '╧': return Border.LeftDouble | Border.RightDouble | Border.TopSingle;
            case '╨': return Border.LeftSingle | Border.RightSingle | Border.TopDouble;

            case '╪': return Border.LeftDouble | Border.RightDouble | Border.TopSingle | Border.DownSingle;
            case '╫': return Border.LeftSingle | Border.RightSingle | Border.TopDouble | Border.DownDouble;
            default:
                return Border.Unknown;
        }
    }
}

/// <summary>
/// Typ ohraničení
/// </summary>
[Flags]
public enum Border
{
    Unknown = 0,

    TopSingle = 0b00_00_01_00,
    DownSingle = 0b00_01_00_00,
    LeftSingle = 0b01_00_00_00,
    RightSingle = 0b00_00_00_01,

    TopDouble = 0b00_00_11_00,
    DownDouble = 0b00_11_00_00,
    LeftDouble = 0b11_00_00_00,
    RightDouble = 0b00_00_00_11,
}
