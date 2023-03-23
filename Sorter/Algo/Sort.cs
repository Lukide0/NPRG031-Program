namespace Sorter.Algo;

public class SortData
{
    public enum Commands
    {
        Swap,
        Highlight,
        Set,
    };
    public int IndexA;
    public int IndexB;
    public Commands Command;

    public SortData(int a, int b, Commands command = Commands.Swap)
    {
        IndexA = a;
        IndexB = b;
        Command = command;
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
                data.Add(new SortData(j, j + 1, SortData.Commands.Highlight));
                if (elements[j] > elements[j + 1])
                {
                    int tmp = elements[j];
                    elements[j] = elements[j + 1];
                    elements[j + 1] = tmp;
                    data.Add(new SortData(j + 1, j));
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
            data.Add(new SortData(0, i));

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
            data.Add(new SortData(leftChild, largestIndex, SortData.Commands.Highlight));
            if (array[leftChild] > array[largestIndex])
            {
                largestIndex = leftChild;
            }
        }

        if (rightChild < size)
        {
            data.Add(new SortData(rightChild, largestIndex, SortData.Commands.Highlight));

            if (array[rightChild] > array[largestIndex])
            {
                largestIndex = rightChild;
            }
        }

        if (largestIndex != index)
        {
            data.Add(new SortData(index, largestIndex));

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
            data.Add(new SortData(i, maxElementIndex, SortData.Commands.Highlight));
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
                data.Add(new SortData(j, i, SortData.Commands.Set));
                elements[j] = i;
                j++;
                occurrences[i]--;
            }
        }

        return data;
    }
}