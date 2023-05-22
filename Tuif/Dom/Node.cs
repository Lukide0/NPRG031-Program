using Tuif.Event;

namespace Tuif.Dom;

/// <summary>
/// Abstraktní třída reprezentující element v hierarchii.
/// </summary>
public abstract class Node
{
    protected uint _posX = 0;
    protected uint _posY = 0;
    protected uint _width;
    protected uint _height;

    protected bool _focused;
    private Node? _parent;

    /// <summary>
    /// Konstruktor Node s určenou šířkou, výškou a rodičem.
    /// </summary>
    /// <param name="width">Šířka elementu.</param>
    /// <param name="height">Výška elementu.</param>
    /// <param name="parent">Rodičovský element (nepovinný).</param>
    public Node(uint width, uint height, Node? parent = null)
    {
        _width = width;
        _height = height;
        _parent = parent;
        _focused = false;
    }

    /// <summary>
    /// Nastaví fokus na element a požádá o překreslení.
    /// </summary>
    public void SetFocus()
    {
        _focused = true;
        Terminal.RequestRender();
    }

    /// <summary>
    /// Odebere fokus z element.
    /// </summary>
    public void RemoveFocus()
    {
        _focused = false;
    }

    /// <summary>
    /// Vrátí rodiče elementu.
    /// Pokud element nemá rodiče, vrátí sebe sama.
    /// </summary>
    /// <returns>Rodičovský element.</returns>
    public Node GetParent()
    {
        if (_parent is null)
            return this;
        else
            return _parent;
    }

    /// <summary>
    /// Nastaví rodiče elementu.
    /// </summary>
    /// <param name="parent">Rodičovský element.</param>
    public void SetParent(Node parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// Vrátí šířku elementu.
    /// </summary>
    /// <returns>Šířka elementu.</returns>
    public uint GetWidth() => _width;

    /// <summary>
    /// Vrátí výšku elementu.
    /// </summary>
    /// <returns>Výška elementu.</returns>
    public uint GetHeigh() => _height;

    /// <summary>
    /// Zpracuje klávesovou událost na elementu.
    /// </summary>
    /// <param name="info">Informace o klávesové události.</param>
    /// <param name="focusedNode">Odkaz na aktuálně zaměřený element.</param>
    /// <returns>
    /// True, pokud element zpracoval klávesovou událost.
    /// False, pokud element nezpracoval klávesovou událost.
    /// </returns>
    public virtual bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        return false;
    }

    /// <summary>
    /// Nastaví pozici elementu.
    /// </summary>
    /// <param name="x">X-ová souřadnice pozice.</param>
    /// <param name="y">Y-ová souřadnice pozice.</param>
    public void SetPos(uint x, uint y)
    {
        _posX = x;
        _posY = y;
    }

    /// <summary>
    /// Aktualizuje velikost elementu.
    /// </summary>
    /// <param name="width">Nová šířka elementu.</param>
    /// <param name="height">Nová výška elementu.</param>
    public virtual void UpdateSize(uint width, uint height)
    {
        _width = width;
        _height = height;
    }

    /// <summary>
    /// Metoda pro vykreslení elementu na buffer.
    /// </summary>
    /// <param name="buff">Buffer pro vykreslení.</param>
    public abstract void Render(Buffer buff);
}
