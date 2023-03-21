namespace Tuif.Dom;

public class Checkbox : Node
{
    public delegate void OnChangeFunc(bool value);

    public bool Value = false;
    public Color FillColor = Color.DefaultForeground;    
    public OnChangeFunc? OnChange;


    private uint _gap = 3;
    private string _label;


    public Checkbox(uint width, uint height, string text) : base(width, height)
    {
        _label = text;
    }

    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        switch (info.Key)
        {
            case ConsoleKey.Enter:
                Value = !Value;
                
                if (OnChange != null) 
                {
                    OnChange(Value);
                }

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

    public override void Render(Buffer buff)
    {
        uint centerX = (_width - (uint)_label.Length - _gap) / 2;
        uint centerY = (_height - 1) / 2;

        if (Value) 
        {
            buff.Write("██", centerX + _posX, centerY + ((byte)_posY), FillColor);
        }
        else 
        {
            buff.Write("░░", centerX + _posX, centerY + ((byte)_posY), FillColor);
        }

        if (_focused) 
        {
            buff.Write(_label, centerX + _posX + _gap, centerY + _posY, FillColor);
        }
        else 
        {
            buff.Write(_label, centerX + _posX + _gap, centerY + _posY);
        }
    }
}