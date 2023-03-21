namespace Sorter.Algo;

public class SortData
{
    public int IndexA;
    public int IndexB;
    public bool Swap;

    public SortData(int a, int b, bool swap = true)
    {
        IndexA = a;
        IndexB = b;
        Swap = swap;
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
                data.Add(new SortData(j, j + 1, false));
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
            data.Add(new SortData(leftChild, largestIndex, false));
            if (array[leftChild] > array[largestIndex])
            {
                largestIndex = leftChild;
            }
        }

        if (rightChild < size)
        {
            data.Add(new SortData(rightChild, largestIndex, false));
            
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

    public static List<SortData> MergeSort(int[] elements) 
    {
        List<SortData> data = new List<SortData>();

        return data;
    }
}