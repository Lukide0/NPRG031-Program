namespace Sorter.Algo;

public class SortData
{
    public enum Commands
    {
        Swap,
        Highlight,
        Set,
    };
 
    public static Tuif.Color SwapAColor = new Tuif.Color(0xFE7F2D);
    public static Tuif.Color SwapBColor = new Tuif.Color(0x8B80F9);
 
    public static Tuif.Color HighlightAColor = new Tuif.Color(0xFEC29A);
    public static Tuif.Color HighlightBColor = new Tuif.Color(0xADE7FF);
    public int IndexA;
    public Tuif.Color ColorA = SwapAColor;
    public int IndexB;
    public Tuif.Color ColorB = SwapBColor;
    public Commands Command;

    public SortData(int a, int b, Tuif.Color colorA, Tuif.Color colorB, Commands command = Commands.Swap) 
    {
        IndexA = a;
        ColorA = colorA;
        IndexB = b;
        ColorB = colorB;
        Command = command;

    }
    public SortData(int a, int b, Commands command = Commands.Swap)
    {
        IndexA = a;
        IndexB = b;
        Command = command;
    }

    public static SortData Swap(int a, int b) 
    {
        return new SortData(a, b, SortData.Commands.Swap);
    }

    public static SortData Highlight(int a, int b) 
    {
        return new SortData(a, b, HighlightAColor, HighlightBColor, Commands.Highlight);
    }

    public static SortData Set(int a, int value) 
    {
        return new SortData(a, value, Commands.Set);
    }
}

public static class Sort
{
    public delegate List<SortData> SortAlgo(int[] elements);

    public static List<SortData> BubbleSort(int[] elements)
    {
        List<SortData> data = new List<SortData>();

        for (int i = 0; i < elements.Length; i++)
        {
            for (int j = 0; j < elements.Length - i - 1; j++)
            {
                data.Add(SortData.Highlight(j, j + 1));
                if (elements[j] > elements[j + 1])
                {
                    int tmp = elements[j];
                    elements[j] = elements[j + 1];
                    elements[j + 1] = tmp;
                    data.Add(SortData.Swap(j + 1, j));
                }
            }
        }
        return data;
    }

    public static List<SortData> HeapSort(int[] elements)
    {
        List<SortData> data = new List<SortData>();
        int size = elements.Length;

        for (int i = size / 2 - 1; i >= 0; i--)
        {
            Heapify(elements, size, i, data);
        }
        for (int i = size - 1; i >= 0; i--)
        {
            data.Add(SortData.Swap(0, i));

            var tempVar = elements[0];
            elements[0] = elements[i];
            elements[i] = tempVar;
            Heapify(elements, i, 0, data);
        }

        return data;
    }

    private static void Heapify(int[] array, int size, int index, List<SortData> data)
    {
        int largestIndex = index;
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;

        if (leftChild < size)
        {
            data.Add(SortData.Highlight(leftChild, largestIndex));
            if (array[leftChild] > array[largestIndex])
            {
                largestIndex = leftChild;
            }
        }

        if (rightChild < size)
        {
            data.Add(SortData.Highlight(rightChild, largestIndex));

            if (array[rightChild] > array[largestIndex])
            {
                largestIndex = rightChild;
            }
        }

        if (largestIndex != index)
        {
            data.Add(SortData.Swap(index, largestIndex));

            int tempVar = array[index];
            array[index] = array[largestIndex];
            array[largestIndex] = tempVar;

            Heapify(array, size, largestIndex, data);
        }
    }


    public static List<SortData> CountingSort(int[] elements)
    {
        List<SortData> data = new List<SortData>();

        int size = elements.Length;
        int maxElement = elements[0];
        int maxElementIndex = 0;

        for (int i = 1; i < size; i++)
        {
            data.Add(SortData.Highlight(i, maxElementIndex));
            if (elements[i] > maxElement)
            {
                maxElement = elements[i];
                maxElementIndex = i;
            }

        }

        int[] occurrences = new int[maxElement + 1];

        for (int i = 0; i < maxElement + 1; i++)
        {
            occurrences[i] = 0;
        }

        for (int i = 0; i < size; i++)
        {
            occurrences[elements[i]]++;
        }
        for (int i = 0, j = 0; i <= maxElement; i++)
        {
            while (occurrences[i] > 0)
            {
                data.Add(SortData.Set(j, i));
                elements[j] = i;
                j++;
                occurrences[i]--;
            }
        }

        return data;
    }

    public static List<SortData> QuickSort(int[] elements) 
    {
        List<SortData> data = new List<SortData>();

        QuickSortArray(elements, 0, elements.Length - 1, data);

        return data;
    }

    private static void QuickSortArray(int[] array, int leftIndex, int rightIndex, List<SortData> data)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex];
        while (i <= j)
        {
            while (array[i] < pivot)
            {
                data.Add(SortData.Highlight(pivot, i));
                i++;
            }

            while (array[j] > pivot)
            {
                data.Add(SortData.Highlight(pivot, j));
                j--;
            }
            if (i <= j)
            {
                data.Add(SortData.Highlight(i, j));
                data.Add(SortData.Swap(j, i));
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j) 
        {
            QuickSortArray(array, leftIndex, j, data);
        }
        if (i < rightIndex) 
        {
            QuickSortArray(array, i, rightIndex, data);
        }
    }
}
