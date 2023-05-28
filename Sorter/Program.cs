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
        var frame = new Tuif.Dom.Layout.Gridbox(0,0, 2, 1);
        var img = new Tuif.Dom.Image();
        var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Data/logo.png"));

        if (img.Load(path))
        {

            frame.SetCell(0,0, img);
            frame.SetCell(1,0, new Tuif.Dom.Text("Press any key", Color.DefaultForeground));

            term.AddToRender(frame);
            term.Render();


            Console.ReadKey();

            term.RemoveFromRender(frame);
            term.Buffer.Clear();
        }
    }
}
