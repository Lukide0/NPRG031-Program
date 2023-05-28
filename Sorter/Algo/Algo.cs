using Tuif;
using Tuif.Dom;

namespace Sorter.Algo;

public class Algo : Tuif.Dom.Node
{
    public Sort.SortAlgo Algorithm = Sort.BubbleSort;
    public bool IsSorted { get; private set; }

    private int[] _elements;
    private int _elementsCount;
    private Color _primaryColor = SortData.HighlightAColor;
    private Color _secondaryColor = SortData.HighlightBColor;

    private List<SortData> _sortData = new List<SortData>();
    private int _dataIndex = 0;

    private int _primaryIndex = -1, _secondaryIndex = -1;
    private Rect _rectangle = new Rect(2, 0);
    private bool _run = false;

    public Algo(uint width, uint height) : base(width, height)
    {
        IsSorted = false;
        _elementsCount = (ushort)Math.Min(_width / 2, _height);

        var random = new Random();
        _elements = Enumerable.Range(1, _elementsCount).OrderBy(x => random.Next()).ToArray();

        Init();
    }

    /// <summary>
    /// Resetuje animaci a vygeneruje nové pole
    /// </summary>
    public void Reset()
    {
        _elementsCount = (ushort)Math.Min(_width / 2, _height);

        var random = new Random();
        _elements = Enumerable.Range(1, _elementsCount).OrderBy(x => random.Next()).ToArray();

        _primaryIndex = -1;
        _secondaryIndex = -1;

        Init();

        Terminal.RequestRender();
    }

    /// <summary>
    /// Připraví instrukce
    /// </summary>
    private void Init()
    {
        IsSorted = false;
        _dataIndex = 0;
        int[] tmp = new int[_elementsCount];
        _elements.CopyTo(tmp, 0);

        _sortData = Algorithm(tmp);
    }

    /// <summary>
    /// Nastaví třídící algorimus
    /// </summary>
    /// <param name="algo"></param>
    public void SetAlgo(Sort.SortAlgo algo) 
    {
        Algorithm = algo;
    }

    /// <summary>
    /// Spustí celou animaci
    /// </summary>
    public void Run()
    {
        Terminal.SetNonBlockRead();
        _run = true;
    }

    /// <summary>
    /// Zastaví animaci
    /// </summary>
    public void Stop()
    {
        Terminal.SetBlockRead();
        _run = false;
    }

    /// <inheritdoc/>
    public override void UpdateSize(uint width, uint height)
    {
        uint tmpWidth = _width;
        uint tmpHeight = _height;
        base.UpdateSize(width, height);

        if (width != tmpWidth || height != _height)
        {
            Reset();
        }

        if (_width > _elementsCount * 2)
        {
            _posX = (_width - (uint)_elementsCount * 2);
        }

        if (_height > _elementsCount)
        {
            _posY = (uint)(height - _elementsCount);
        }
    }

    /// <summary>
    /// Provede 1 krok animace, pokud není pole setřízeno
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>True, pokud je animace u konce</returns>
    public bool RunStep(float delta)
    {
        if (IsSorted)
        {
            _run = false;
            return _run;
        }
    
        if (_run)
        {
            Step();
        }
        return _run;
    }

    /// <inheritdoc/>
    public override bool HandleKey(ConsoleKeyInfo info, ref Node focusedNode)
    {
        switch (info.Key)
        {
            case ConsoleKey.Escape:
                RemoveFocus();
                focusedNode = GetParent();
                focusedNode.SetFocus();
                return focusedNode != this;
            case ConsoleKey.N:
                SetFocus();
                Step();
                break;
            case ConsoleKey.R:
                Reset();
                break;
            case ConsoleKey.S:
                Stop();
                break;
            case ConsoleKey.U:
                Run();
                break;
            default:
                break;
        }
        return true;
    }

    /// <summary>
    /// Provede 1 krok animace
    /// </summary>
    public void Step()
    {
        if (_dataIndex >= _sortData.Count)
        {
            _primaryIndex = -1;
            _secondaryIndex = -1;

            IsSorted = true;
            Terminal.RequestRender();
            return;
        }

        SortData data = _sortData[_dataIndex++];
        _primaryColor = data.ColorA;
        _secondaryColor = data.ColorB;

        switch (data.Command)
        {
        case SortData.Commands.Swap:
            int tmp = _elements[data.IndexA];
            _elements[data.IndexA] = _elements[data.IndexB];
            _elements[data.IndexB] = tmp;
            
            _primaryIndex = data.IndexA;
            _secondaryIndex = data.IndexB;
            break;
        case SortData.Commands.Highlight:
            _primaryIndex = data.IndexA;
            _secondaryIndex = data.IndexB;
            break;
        case SortData.Commands.Set:
            _elements[data.IndexA] = data.IndexB;
            _primaryIndex = data.IndexA;
            _secondaryIndex = -1;
            break;
        }

        Terminal.RequestRender();
    }

    /// <inheritdoc/> 
    public override void Render(Tuif.Buffer buff)
    {
        for (uint i = 0; i < _elementsCount; i++)
        {
            int el = _elements[i];

            _rectangle.UpdateSize(2, (uint)el);
            _rectangle.SetPos(i * 2 + _posX, (uint)(_elementsCount - el) + _posY);

            if (i == _primaryIndex)
            {
                _rectangle.FillColor = _primaryColor;
            }
            else if (i == _secondaryIndex)
            {
                _rectangle.FillColor = _secondaryColor;
            }
            else
            {
                _rectangle.FillColor = Color.DefaultForeground;
            }
            _rectangle.Render(buff);
        }
    }
}
