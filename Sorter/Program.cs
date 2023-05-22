using Tuif;

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

        App app = new App(term);
        app.Run();
    }
}
