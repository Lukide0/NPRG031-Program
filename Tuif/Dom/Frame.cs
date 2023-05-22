namespace Tuif.Dom;

public class Frame : Node
{
    public enum Style
    {
        Single,
        Double,

    }

    public Style BorderStyle = Style.Single;
    public Color BorderColor = new Color(0xFFFFFF);
    private Node? _child;

    public Frame(uint width, uint height, Node? child = null) : base(width, height)
    {
        _child = child;
    }

    public Frame(Node child, Style border = Style.Single) : base(child.GetWidth() + 2, child.GetHeigh() + 2)
    {
        _child = child;
        BorderStyle = border;
    }

    public void SetChild(Node child)
    {
        _child = child;
        SetParent(_child.GetParent());
    }
    /// <inheritdoc/>
    public override void UpdateSize(uint width, uint height)
    {
        base.UpdateSize(width, height);

        if (_child is not null)
        {
            _child.UpdateSize(_width - 2, _height - 2);
            _child.SetPos(_posX + 1, _posY + 1);
        }
    }

    /// <inheritdoc/>
    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        if (_child is null)
        {
            return false;
        }

        focusedNode = _child;
        RemoveFocus();
        focusedNode.SetFocus();

        return focusedNode.HandleKey(info, ref focusedNode);
    }

    /// <inheritdoc/>
    public override void Render(Buffer buff)
    {
        char verticalCh, horizontalCh, topLCh, topRCh, bottomLCh, bottomRCh;

        if (BorderStyle == Style.Single)
        {
            horizontalCh = '─';
            verticalCh = '│';
            topLCh = '┌';
            topRCh = '┐';
            bottomLCh = '└';
            bottomRCh = '┘';
        }
        else
        {
            horizontalCh = '═';
            verticalCh = '║';
            topLCh = '╔';
            topRCh = '╗';
            bottomLCh = '╚';
            bottomRCh = '╝';
        }

        uint index = _posY * buff.Width + _posX;
        uint indexBottom = index + buff.Width * (_height - 1);
        uint indexTopRight = index + _width - 1;
        uint indexBottomRight = indexBottom + _width - 1;

        // top and bottom
        for (uint i = 1; i < _width - 1; i++)
        {
            buff.Foreground[index + i] = BorderColor;
            buff.Data[index + i] = BorderChar.MergeChars(buff.Data[index + i], horizontalCh);

            buff.Data[indexBottom + i] = BorderChar.MergeChars(buff.Data[indexBottom + i], horizontalCh);
            buff.Foreground[indexBottom + i] = BorderColor;
        }

        // left and right
        for (uint i = 1; i < _height - 1; i++)
        {
            uint tmp = index + buff.Width * i;

            buff.Foreground[tmp] = BorderColor;
            buff.Data[tmp] = BorderChar.MergeChars(buff.Data[tmp], verticalCh);

            buff.Data[tmp + _width - 1] = BorderChar.MergeChars(buff.Data[tmp + _width - 1], verticalCh);
            buff.Foreground[tmp + _width - 1] = BorderColor;
        }

        // corners:
        buff.Data[index] = BorderChar.MergeChars(buff.Data[index], topLCh);
        buff.Foreground[index] = BorderColor;

        buff.Data[indexTopRight] = BorderChar.MergeChars(buff.Data[indexTopRight], topRCh);
        buff.Foreground[indexTopRight] = BorderColor;

        buff.Data[indexBottom] = BorderChar.MergeChars(buff.Data[indexBottom], bottomLCh);
        buff.Foreground[indexBottom] = BorderColor;

        buff.Data[indexBottomRight] = BorderChar.MergeChars(buff.Data[indexBottomRight], bottomRCh);
        buff.Foreground[indexBottomRight] = BorderColor;

        if (_child is not null)
        {
            _child.SetPos(_posX + 1, _posY + 1);
            _child.Render(buff);
        }
    }
}
