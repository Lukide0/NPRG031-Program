namespace Tuif.Dom;

public class Switch : Node
{
    public delegate void OnChangeFunc(int value);

    public string[] Options { get; private set; }
    public int Selected { get; private set; }

    public Color FocusColor = new Color(255, 0, 0);
    public OnChangeFunc? OnChage;

    private Text _text;

    public Switch(uint width, uint height) : base(width, height)
    {
        Options = new string[] { "None" };
        Selected = 0;
        _text = new Text(Options[0], Color.DefaultForeground);
    }

    public void SetOptions(string[] options)
    {
        Options = options;
        Selected = 0;
    }

    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        switch (info.Key)
        {
            case ConsoleKey.Escape:
                RemoveFocus();
                focusedNode = GetParent();
                focusedNode.SetFocus();

                return focusedNode != this;

            case ConsoleKey.Enter:
            case ConsoleKey.RightArrow:
                Selected = (Selected + 1) % Options.Length;

                if (OnChage != null)
                {
                    OnChage(Selected);
                }

                Terminal.RequestRender();
                break;
            case ConsoleKey.LeftArrow:
                if (Selected == 0)
                {
                    Selected = Options.Length - 1;
                }
                else
                {
                    Selected = Selected - 1;
                }

                if (OnChage != null)
                {
                    OnChage(Selected);
                }

                Terminal.RequestRender();
                break;
            default:
                return true;
        }

        return true;
    }

    public override void Render(Buffer buff)
    {
        uint centerY = (_height - 1) / 2;

        Color col = (_focused) ? FocusColor : Color.DefaultForeground;

        buff.Write('◄', _posX, centerY + _posY, col);
        buff.Write('►', _posX + _width - 1, centerY + _posY, col);

        _text.UpdateSize(_width - 4, 1);
        _text.SetPos(_posX + 2, centerY + _posY);
        _text.Data = Options[Selected];

        _text.Render(buff);
    }
}