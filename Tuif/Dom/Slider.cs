using System.Diagnostics;

namespace Tuif.Dom;

/// <summary>
/// Třída reprezentující posuvník.
/// </summary>
public class Slider : Node
{
    public int Value = 0;
    public Color Bar = Color.DefaultForeground;
    public bool Enabled = true;
    public bool ShowPercentage = true;
    private int _maxValue = 100;
    private int _minValue = 0;
    private int _step = 1;

    /// <summary>
    /// Konstruktor posuvníku s určenou šířkou.
    /// </summary>
    /// <param name="width">Šířka posuvníku.</param>
    public Slider(uint width) : base(width, 1)
    {
        Debug.Assert(width >= 5);
    }

    /// <summary>
    /// Nastaví krok posuvníku.
    /// </summary>
    /// <param name="step">Krok posuvníku.</param>
    public void SetStep(uint step)
    {
        _step = (int)step;
    }

    /// <summary>
    /// Nastaví limit hodnot posuvníku.
    /// </summary>
    /// <param name="min">Minimální hodnota.</param>
    /// <param name="max">Maximální hodnota.</param>
    public void SetLimit(int min, int max)
    {
        _minValue = min;
        _maxValue = max;
    }

    /// <inheritdoc/>
    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        if (info.Key == ConsoleKey.LeftArrow && Enabled)
        {
            Value -= _step;
            if (Value <= _minValue)
            {
                Value = _minValue;
            }
            Terminal.RequestRender();
        }
        else if (info.Key == ConsoleKey.RightArrow && Enabled)
        {
            Value += _step;
            if (Value >= _maxValue)
            {
                Value = _maxValue;
            }
            Terminal.RequestRender();
        }
        else if (info.Key == ConsoleKey.Escape)
        {
            RemoveFocus();
            focusedNode = GetParent();
            focusedNode.SetFocus();

            return focusedNode != this;
        }

        return true;
    }

    /// <inheritdoc/>
    public override void UpdateSize(uint width, uint height)
    {
        _posY += (height - 1) / 2;
        _width = width;
    }

    /// <inheritdoc/>
    public override void Render(Buffer buff)
    {
        float percentage = (Value / (float)_maxValue);
        uint fill = (uint)Math.Round(_width * percentage);
        Color color = (_focused) ? Bar : Color.DefaultForeground;

        for (uint x = 0; x < fill; ++x)
        {
            buff.Write('█', _posX + x, _posY, color);
        }

        for (uint i = fill; i < _width; ++i)
        {
            buff.Write(' ', _posX + i, _posY);
        }

        if (ShowPercentage)
        {
            string tmp = ((int)(percentage * 100)).ToString() + '%';
            buff.Write(tmp, (uint)(_posX + (_width - tmp.Length) / 2), _posY, Color.DefaultForeground);
        }
    }
}
