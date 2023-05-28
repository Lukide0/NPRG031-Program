using Tuif;
using System.Reflection;

namespace Sorter;
class Program
{
    static void Main()
    {
        if (!Terminal.Init())
        {
            Console.WriteLine("Failed to setup terminal");
            return;
        }

        // připraví terminál 150x50
        Terminal term = new Terminal(150,50);
        term.Setup();

        Console.CancelKeyPress += delegate {
            term.Clean();
        };
        
        StartScreen(term);

        App app = new App(term);
        app.Run();

    }

    private static void StartScreen(Terminal term) 
    {
        var img = new Tuif.Dom.Image();
        var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Data/logo.png"));

        if (img.Load(path))
        {
            img.UpdateSize((uint)Console.BufferWidth, (uint)Console.BufferHeight);
            term.AddToRender(img);
            term.Render();

            Thread.Sleep(1000);

            term.RemoveFromRender(img);
        }
    }
}
