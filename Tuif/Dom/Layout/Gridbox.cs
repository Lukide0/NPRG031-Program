using Tuif.Event;

namespace Tuif.Dom.Layout;

public class Gridbox : Node
{
    private Node[,] _cells;
    private uint _columns;
    private uint _rows;
    public uint CellWidth { get; private set; }
    public uint CellHeight { get; private set; }
    private int[] _focusedCell = new int[] { 0, 0 };
    private int[] _focusedCellPrev = new int[] { int.MaxValue, int.MaxValue };

    public Color FocusColor = new Color(0xFF0000);
    public Gridbox(uint width, uint height, uint rows, uint columns) : base(width, height)
    {
        _columns = columns;
        _rows = rows;
        _cells = new Node[rows, columns];

        CellWidth = _width / columns;
        CellHeight = _height / rows;
    }

    public void SetCell(uint row, uint column, Node node)
    {
        node.SetParent(this);
        _cells[row, column] = node;
    }

    public override void UpdateSize(uint width, uint height)
    {
        base.UpdateSize(width, height);
        CellWidth = _width / _columns;
        CellHeight = _height / _rows;
    }

    private Node? GetFocusedNode()
    {
        return _cells[_focusedCell[0], _focusedCell[1]];
    }
    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        switch (info.Key)
        {
            case ConsoleKey.DownArrow:
                _focusedCell[0] += 1;
                break;
            case ConsoleKey.UpArrow:
                _focusedCell[0] -= 1;
                break;
            case ConsoleKey.LeftArrow:
                _focusedCell[1] -= 1;
                break;
            case ConsoleKey.RightArrow:
                _focusedCell[1] += 1;
                break;
            case ConsoleKey.Enter:
                Node? tmp = GetFocusedNode();
                if (tmp == null)
                {
                    break;
                }

                RemoveFocus();
                focusedNode = tmp;
                focusedNode.SetFocus();
                break;
            case ConsoleKey.Q:
            case ConsoleKey.Escape:
                {
                    RemoveFocus();
                    focusedNode = GetParent();
                    focusedNode.SetFocus();

                    if (focusedNode == this)
                    {
                        return false;
                    }

                    break;
                }
            default:
                break;
        }

        if (_focusedCell[0] < 0)
        {
            _focusedCell[0] = 0;
        }
        else
        {
            _focusedCell[0] = Math.Min(_focusedCell[0], (int)_rows - 1);
        }
        if (_focusedCell[1] < 0)
        {
            _focusedCell[1] = 0;
        }
        else
        {
            _focusedCell[1] = Math.Min(_focusedCell[1], (int)_columns - 1);
        }

        Terminal.RequestRender();
        return true;
    }

    public override void Render(Buffer buff)
    {
        char h = BorderChar.LookupChar(Border.LeftSingle | Border.RightSingle);
        char v = BorderChar.LookupChar(Border.TopSingle | Border.DownSingle);

        for (uint i = 0; i < _rows; ++i)
        {
            for (uint j = 0; j < _columns; ++j)
            {
                if (_cells[i, j] is not null)
                {
                    _cells[i, j].SetPos(CellWidth * j + 1 + _posX, CellHeight * i + 1 + _posY);
                    _cells[i, j].UpdateSize(CellWidth - 2, CellHeight - 2);
                    _cells[i, j].Render(buff);
                }
            }
        }

        if (!_focused)
        {
            return;
        }

        uint posY = (uint)_focusedCell[0] * CellHeight + _posY;
        uint posX = (uint)_focusedCell[1] * CellWidth + _posX;

        // top and bottom border
        for (uint x = 0; x < CellWidth - 1; ++x)
        {
            buff.Write(h, posX + x, posY, FocusColor);
            buff.Write(h, posX + x, posY + CellHeight - 1, FocusColor);
        }

        // left and right border
        for (uint y = 0; y < CellHeight - 1; ++y)
        {
            buff.Write(v, posX, posY + y, FocusColor);
            buff.Write(v, posX + CellWidth - 1, posY + y, FocusColor);
        }

        buff.Write(BorderChar.LookupChar(Border.RightSingle | Border.DownSingle), posX, posY, FocusColor);
        buff.Write(BorderChar.LookupChar(Border.LeftSingle | Border.DownSingle), posX + CellWidth - 1, posY, FocusColor);
        buff.Write(BorderChar.LookupChar(Border.RightSingle | Border.TopSingle), posX, posY + CellHeight - 1, FocusColor);
        buff.Write(BorderChar.LookupChar(Border.LeftSingle | Border.TopSingle), posX + CellWidth - 1, posY + CellHeight - 1, FocusColor);
    }
}