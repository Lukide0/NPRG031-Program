using Tuif;
using Tuif.Dom;
using Tuif.Dom.Layout;

using Sorter.Algo;

namespace Sorter;

public class App
{
    public Terminal Term;

    private Gridbox MainGrid;
    private Gridbox SettingsGrid;
    private Algo.Algo AlgoNode;

    private Button ResetBtn;
    private Button NextStepBtn;
    private Button StopBtn;
    private Button RunBtn;

    private Switch SwitchAlgo;

    private string[] _sortsName = new string[] { "Bubble sort", "Heap sort", "Counting sort" };
    private Sort.SortAlgo[] _sortsFunc = new Sort.SortAlgo[] { Sort.BubbleSort, Sort.HeapSort, Sort.CountingSort };

    public App(Terminal term)
    {
        Term = term;

        MainGrid = new Gridbox(term.Buffer.Width, term.Buffer.Height, 1, 2);

        SettingsGrid = new Gridbox(MainGrid.CellWidth, MainGrid.CellHeight, 5, 1);
        AlgoNode = new Algo.Algo(MainGrid.CellWidth, MainGrid.CellHeight);

        MainGrid.SetCell(0, 0, AlgoNode);
        MainGrid.SetCell(0, 1, SettingsGrid);


        ResetBtn = new Button(0, 0, "(R)eset");
        NextStepBtn = new Button(0, 0, "(N)ext step");
        RunBtn = new Button(0, 0, "Run");
        StopBtn = new Button(0, 0, "(S)top");

        SwitchAlgo = new Switch(0,1);

        ResetBtn.OnClick = AlgoNode.Reset;
        NextStepBtn.OnClick = AlgoNode.Step;
        RunBtn.OnClick = AlgoNode.Run;
        StopBtn.OnClick = AlgoNode.Stop;
        
        SwitchAlgo.SetOptions(_sortsName);
        SwitchAlgo.OnChage = ChangeAlgo;

        SettingsGrid.SetCell(0, 0, ResetBtn);
        SettingsGrid.SetCell(1, 0, NextStepBtn);
        SettingsGrid.SetCell(2, 0, RunBtn);
        SettingsGrid.SetCell(3, 0, StopBtn);
        SettingsGrid.SetCell(4, 0, SwitchAlgo);

        Term.SetDocument(MainGrid);
        Term.HideCursor();
        Term.RegisterFrameHandle(AlgoNode.RunStep);
    }

    private void ChangeAlgo(int index) 
    {
        AlgoNode.SetAlgo(_sortsFunc[index]);
        AlgoNode.Reset();
    }

    public void Run()
    {
        AlgoNode.SetAlgo(Sort.HeapSort);

        Term.Setup();
        Terminal.SetBlockRead();
        Term.Loop();
        Term.Clean();
    }
}