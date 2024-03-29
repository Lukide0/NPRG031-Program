namespace Tuif.Dom;

public class Button : Node
{
    public delegate void OnClickFunc();

    public Color Background = new Color(255, 0, 0);
    public Color Foreground = Color.DefaultForeground;

    public Text Text;

    public OnClickFunc? OnClick = null;
    private Frame _frame;

    /// <summary>
    /// Konstruktor tlačítka s určenou šířkou, výškou a textem.
    /// </summary>
    /// <param name="width">Šířka tlačítka.</param>
    /// <param name="height">Výška tlačítka.</param>
    /// <param name="text">Text tlačítka.</param>
    public Button(uint width, uint height, string text) : base(width, height)
    {
        _frame = new Frame(width, height);
        _frame.BorderStyle = Frame.Style.Double;
        _frame.BorderColor = Foreground;
        Text = new Text(width - 2, height - 2, text, Foreground);

        _frame.SetChild(Text);
    }

    /// <inheritdoc/>
    public override void UpdateSize(uint width, uint height)
    {
        base.UpdateSize(width, height);
        _frame.UpdateSize(width, height);
    }
    /// <inheritdoc/>
    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        switch (info.Key)
        {
            case ConsoleKey.Enter:
                RemoveFocus();
                Click();
                break;
            case ConsoleKey.Escape:
                break;
            default:
                return true;
        }

        RemoveFocus();
        focusedNode = GetParent();
        focusedNode.SetFocus();
        return focusedNode != this;
    }

    /// <summary>
    /// Spustí akci při stisknutí tlačítka.
    /// </summary>
    public void Click()
    {
        if (OnClick != null)
        {
            OnClick();
        }
    }
    /// <inheritdoc/>
    public override void Render(Buffer buff)
    {
        _frame.SetPos(_posX, _posY);

        if (_focused)
        {
            for (uint y = 1; y < _height - 1; y++)
            {
                for (uint x = 1; x < _width - 1; x++)
                {
                    buff.Write(' ', x + _posX, y + _posY, Foreground, Background);
                }
            }
        }
        _frame.Render(buff);
    }
}
