using System.Runtime.InteropServices;
using System.Text;

using Tuif.Event;


namespace Tuif;

public class Terminal
{

    public Buffer Buffer { get; }

    private Dom.Node? _document;
    private Dom.Node? _focusedNode;

    private uint _screenOffset = 0;

    private List<Dom.Node> _backLayer = new List<Dom.Node>();
    private List<Dom.Node> _frontLayer = new List<Dom.Node>();
    private string _moveCursor = string.Empty;

    private Event.EventManager<float> _frameManager;

    private const float _frameMs = 50f;
    private static bool _run = false;
    private static bool _blockRender = false;
    private static bool _render = false;

    private static Mutex _mutex = new Mutex();

    public Terminal(uint width, uint height)
    {
        Buffer = new Buffer(width, height);
        _frameManager = new EventManager<float>();
    }

    public void Clean()
    {
        Console.WriteLine("\x1b[0m");
    }

    public void RegisterFrameHandle(Event.EventListener<float> listener)
    {
        _frameManager.Attach(listener);
    }

    public void SetDocument(Dom.Node node)
    {
        _document = node;
        _backLayer.Add(node);
        _focusedNode = node;
        _focusedNode.SetFocus();
    }

    public static void SetBlockRead()
    {
        _mutex.WaitOne();
        _blockRender = true;
        _mutex.ReleaseMutex();
    }

    public static void SetNonBlockRead()
    {
        _mutex.WaitOne();
        _blockRender = false;
        _mutex.ReleaseMutex();
    }

    public static void RequestRender()
    {
        _mutex.WaitOne();
        _render = true;
        _mutex.ReleaseMutex();
    }

    public void HideCursor()
    {
        Console.Write("\x1b[?25l");
    }

    public void ShowCursor()
    {
        Console.Write("\x1b[?25h");
    }


    public void Setup()
    {
        Console.Write($"\x1b[2J\x1b[H");
        SetWindowSize((int)Buffer.Width, (int)Buffer.Height);
    }

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

        Console.Write("\x1b[0;0H");

        uint startIndex = (Buffer.Width * _screenOffset);

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
                    _mutex.WaitOne();
                    _render |= _frameManager.NotifyAll(delta);
                    _mutex.ReleaseMutex();
                }
            }


            if (_blockRender || Console.KeyAvailable)
            {
                bool handled = false;
                ConsoleKeyInfo info = Console.ReadKey(true);
                if ((info.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    switch (info.Key)
                    {
                        case ConsoleKey.DownArrow:
                            _screenOffset = (uint)Math.Min(Math.Max(0,Buffer.Height - Console.BufferHeight - 1), _screenOffset + 1);
                            handled = true;
                            RequestRender();
                            break;
                        case ConsoleKey.UpArrow:
                            _screenOffset = (uint)Math.Max(0, (int)_screenOffset - 1);
                            handled = true;
                            RequestRender();
                            break;
                        default:
                            break;
                    }
                }

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

    public void RemoveFromRender(Dom.Node node) 
    {
        _frontLayer.Remove(node);
        _backLayer.Remove(node);
    }

    public static void StopLoop()
    {
        _run = false;
    }

    public void MoveCursor(uint x, uint y)
    {
        _moveCursor = $"\x1b[{y + 1};{x + 1}H";
    }

    public Buffer GetBuffer() => Buffer;

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
