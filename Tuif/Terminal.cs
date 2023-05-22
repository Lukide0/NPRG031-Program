using System.Runtime.InteropServices;
using System.Text;

using Tuif.Event;

namespace Tuif;

public class Terminal
{
    /// <summary>
    /// Buffer terminálu
    /// </summary>
    public Buffer Buffer { get; }

    /// <summary>
    /// Hlavní Node terminálu
    /// </summary>
    private Dom.Node? _document;

    /// <summary>
    /// Node, který je aktivní
    /// </summary>
    private Dom.Node? _focusedNode;

    /// <summary>
    /// Horní odsazení
    /// </summary>
    private uint _screenOffsetY = 0;

    /// <summary>
    /// List Node v zadní vrstvě
    /// </summary>
    private List<Dom.Node> _backLayer = new List<Dom.Node>();

    /// <summary>
    /// <list Node v přední vrstvě
    /// </summary>
    private List<Dom.Node> _frontLayer = new List<Dom.Node>();

    /// <summary>
    /// ANSI sekvence na posun kurzoru
    /// </summary>
    private string _moveCursor = string.Empty;

    /// <summary>
    /// Manager eventů
    /// </summary>
    private Event.EventManager<float> _frameManager;

    /// <summary>
    /// Doba trvání jednoho rámce v milisekundách
    /// </summary>
    private const float _frameMs = 50f;

    /// <summary>
    /// Indikuje, zda má Loop běžet
    /// </summary>
    private static bool _run = false;

    /// <summary>
    /// Indikuje, zda-li má loop čekat na vstup
    /// </summary>
    private static bool _blockRender = false;

    /// <summary>
    /// Indikuje, zda se má vykreslit rámec
    /// </summary>
    private static bool _render = false;

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="width">Šířka terminálu</param>
    /// <param name="height">Výška terminálu</param>
    public Terminal(uint width, uint height)
    {
        Buffer = new Buffer(width, height);
        _frameManager = new EventManager<float>();
    }

    /// <summary>
    /// Smaže styly
    /// </summary>
    public void Clean()
    {
        Console.WriteLine("\x1b[0m");
    }

    /// <summary>
    /// Registruje posluchače události pro zpracování rámce
    /// </summary>
    /// <param name="listener">Posluchač události pro zpracování rámce</param>
    public void RegisterFrameHandle(Event.EventListener<float> listener)
    {
        _frameManager.Attach(listener);
    }

    /// <summary>
    /// Nastaví hlavní Node
    /// </summary>
    /// <param name="node"></param>
    public void SetDocument(Dom.Node node)
    {
        _document = node;
        _backLayer.Add(node);
        _focusedNode = node;
        _focusedNode.SetFocus();
    }

    /// <summary>
    /// Nastaví blokování čtení
    /// </summary>
    public static void SetBlockRead()
    {
        _blockRender = true;
    }

    /// <summary>
    /// Nastaví neblokující čtení
    /// </summary>
    public static void SetNonBlockRead()
    {
        _blockRender = false;
    }

    /// <summary>
    /// Požaduje vykreslení
    /// </summary>
    public static void RequestRender()
    {
        _render = true;
    }

    /// <summary>
    /// Skryje kurzor
    /// </summary>
    public void HideCursor()
    {
        Console.Write("\x1b[?25l");
    }

    /// <summary>
    /// Zobrazí kurzor
    /// </summary>
    public void ShowCursor()
    {
        Console.Write("\x1b[?25h");
    }

    /// <summary>
    /// Připraví terminál. Smaže znaky a pokusí se nastavit šířku a výšku terminálu
    /// </summary>
    public void Setup()
    {
        Console.Write($"\x1b[2J\x1b[H");
        SetWindowSize((int)Buffer.Width, (int)Buffer.Height);
    }

    /// <summary>
    /// Provádí vykreslování
    /// </summary>
    public void Render()
    {
        foreach (var node in _backLayer)
        {
            node.Render(Buffer);
        }

        foreach (var node in _frontLayer)
        {
            node.Render(Buffer);
        }

        // Posune kursor do levého horního rohu
        Console.Write("\x1b[0;0H");

        uint startIndex = (Buffer.Width * _screenOffsetY);

        StringBuilder sb = new StringBuilder($"\x1b[48;2;{Buffer.Background[startIndex].GetSequence()};38;2;{Buffer.Foreground[startIndex].GetSequence()}m{Buffer.Data[startIndex]}");

        for (uint i = startIndex + 1; i < Buffer.Size; ++i)
        {
            if (i % Buffer.Width == 0)
                sb.Append("\x1b[1E");

            if (Buffer.Foreground[i] != Buffer.Foreground[i - 1])
            {
                sb.Append($"\x1b[38;2;{Buffer.Foreground[i].GetSequence()}m");
            }

            if (Buffer.Background[i] != Buffer.Background[i - 1])
            {
                sb.Append($"\x1b[48;2;{Buffer.Background[i].GetSequence()}m");
            }

            sb.Append(Buffer.Data[i]);

        }

        sb.Append(_moveCursor);
        _moveCursor = string.Empty;

        Console.Write(sb);
    }

    /// <summary>
    /// Hlavní smyčka programu
    /// </summary>
    public void Loop()
    {
        _run = true;
        Render();

        long lastTime = DateTime.Now.Ticks;
        long timeNow;
        while (_run)
        {
            timeNow = DateTime.Now.Ticks;
            float delta = (timeNow - lastTime) / (float)TimeSpan.TicksPerMillisecond;
            if (delta > _frameMs)
            {
                lastTime = timeNow;
                if (_frameManager.Count() != 0)
                {
                    // zjistí, zda-li je vyžádáno překreslení
                    _render |= _frameManager.NotifyAll(delta);
                }
            }

            if (_blockRender || Console.KeyAvailable)
            {
                bool handled = false;
                ConsoleKeyInfo info = Console.ReadKey(true);

                // Speciální vstup
                if ((info.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    switch (info.Key)
                    {
                        case ConsoleKey.DownArrow:
                            _screenOffsetY = (uint)Math.Min(Math.Max(0, Buffer.Height - Console.BufferHeight - 1), _screenOffsetY + 1);
                            handled = true;
                            RequestRender();
                            break;
                        case ConsoleKey.UpArrow:
                            _screenOffsetY = (uint)Math.Max(0, (int)_screenOffsetY - 1);
                            handled = true;
                            RequestRender();
                            break;
                        default:
                            break;
                    }
                }

                // Pokusí se zjisti, zda nebyl event vyřešen
                if (!handled && (_focusedNode is null || !_focusedNode.HandleKey(info, ref _focusedNode)))
                {
                    _run = false;
                    break;
                }
            }

            RenderTick();
        }
    }

    private void RenderTick()
    {
        if (_render)
        {
            Buffer.Clear();
            Render();
            _render = false;
        }
    }

    /// <summary>
    /// Přidá Node do vykreslení
    /// </summary>
    /// <param name="node">Node, který se má přidat</param>
    /// <param name="frontLayer">Určuje, zda se má Node přidat do přední vrstvy (true) nebo do pozadí (false). Výchozí hodnota je false
    public void AddToRender(Dom.Node node, bool frontLayer = false)
    {
        if (frontLayer)
        {
            _frontLayer.Add(node);
        }
        else
        {
            _backLayer.Add(node);
        }
    }

    /// <summary>
    /// Odebere Node z vykreslení
    /// </summary>
    /// <param name="node">Node, který se má odebrat</param>
    public void RemoveFromRender(Dom.Node node)
    {
        _frontLayer.Remove(node);
        _backLayer.Remove(node);
    }

    /// <summary>
    /// Zastavý hlavní smyčku
    /// </summary>
    public static void StopLoop()
    {
        _run = false;
    }

    /// <summary>
    /// Pohne kurzor na požadovanou pozici
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveCursor(uint x, uint y)
    {
        _moveCursor = $"\x1b[{y + 1};{x + 1}H";
    }

    #region Configuration

#if WINDOWS
    private const int STDIN_HANDLE = -10;
    private const int STDOUT_HANDLE = -11;

    private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;
    private const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
    private const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    public static bool Init()
    {
        var stdin = GetStdHandle(STDIN_HANDLE);
        var stdout = GetStdHandle(STDOUT_HANDLE);

        if (!GetConsoleMode(stdin, out uint stdinMode) || !GetConsoleMode(stdout, out uint stdoutMode))
        {
            return false;
        }

        stdinMode |= DISABLE_NEWLINE_AUTO_RETURN;
        stdoutMode |= ENABLE_PROCESSED_OUTPUT | ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
        stdoutMode &= ~ENABLE_WRAP_AT_EOL_OUTPUT;

        return SetConsoleMode(stdin, stdinMode) && SetConsoleMode(stdout, stdoutMode);

    }

    private static void SetWindowSize(int width, int height)
    {
        try
        {
            Console.WindowWidth = width;
            Console.WindowHeight = height;

        }
        catch (IOException)
        { }
    }
#else
    public static bool Init() => true;
    private static void SetWindowSize(int width, int height)
    {
        Console.Write($"\x1b[8;{height};{width + 1}t");
    }
#endif

    #endregion
}
