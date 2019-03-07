using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast
{
    class HeapSort
    {
        public HeapSort()
        {
            int[] arr = { 10, 64, 7, 99, 32, 18 };
            HeapSort hs = new HeapSort();
            hs.PerformHeapSort(arr);
            
        }

        private int heapSize;

        private void BuildHeap(int[] arr)
        {
            heapSize = arr.Length - 1;
            for (int i = heapSize / 2; i >= 0; i--)
            {
                Heapify(arr, i);
            }
        }

        private void Swap(int[] arr, int x, int y)//function to swap elements
        {
            int temp = arr[x];
            arr[x] = arr[y];
            arr[y] = temp;
        }
        private void Heapify(int[] arr, int index)
        {
            int left = 2 * index;
            int right = 2 * index + 1;
            int largest;

            if (left <= heapSize && arr[left] > arr[index])
            {
                largest = left;
            }
            else
            {
                largest = index;
            }
            if (right <= heapSize && arr[right] > arr[largest])
            {
                largest = right;
            }
            else
            {
                largest = index;
            }

            if (largest != index)
            {
                Swap(arr, index, largest);
                Heapify(arr, largest);
            }
        }
        public void PerformHeapSort(int[] arr)
        {
            BuildHeap(arr);
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Swap(arr, 0, i);
                heapSize--;
                Heapify(arr, 0);
            }
            DisplayArray(arr);
        }
        private void DisplayArray(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            { Console.Write("[{0}]", arr[i]); }
        }
    }
}
