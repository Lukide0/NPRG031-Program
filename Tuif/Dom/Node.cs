using Tuif.Event;

namespace Tuif.Dom;

public abstract class Node
{
    protected uint _posX = 0;
    protected uint _posY = 0;
    protected uint _width;
    protected uint _height;

    protected bool _focused;
    private Node? _parent;
    public Node(uint width, uint height, Node? parent = null)
    {
        _width = width;
        _height = height;
        _parent = parent;
        _focused = false;
    }

    public void SetFocus() 
    {
        _focused = true;
        Terminal.RequestRender();
    }

    public void RemoveFocus() 
    {
        _focused = false;
    }

    public Node GetParent() {
        if (_parent is null)
            return this;
        else
            return _parent;
    }
    public void SetParent(Node parent) 
    {
        _parent = parent;
    }

    public uint GetWidth() => _width;
    public uint GetHeigh() => _height;

    public virtual bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode) 
    {
        return false;
    }
    public void SetPos(uint x, uint y)
    {
        _posX = x;
        _posY = y;
    }

    public virtual void UpdateSize(uint width, uint height)
    {
        _width = width;
        _height = height;
    }

    public abstract void Render(Buffer buff);
}