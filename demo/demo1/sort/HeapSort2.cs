using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast.Sort
{
    public class HeapSort2
    {
        public HeapSort2()
        {
            List<Int32> myList = new List<Int32> { 2, 5, 1, 1990, 0, 6, 9, 3, 7, 7, 4, 8, 500, 678 };

            int myLen = myList.Count;
            HeapSort2 myHeap = new HeapSort2(myList, myLen);

            Console.WriteLine("Original: {0}", myHeap.printList());
            myHeap.heapsort();
            Console.WriteLine("Sorted: {0}", myHeap.printList());
        }

        List<Int32> myList;
        public int myLen;

        public HeapSort2(List<Int32> myList, int myLen)
        {
            this.myLen = myLen;
            this.myList = myList;
        }

        public void heapsort()
        {
            int iValue;

            for (int i = myLen / 2; i >= 0; i--)
            {
                adjust(i, myLen - 1);
            }

            for (int i = myLen - 2; i >= 0; i--)
            {
                iValue = myList[i + 1];
                myList[i + 1] = myList[0];
                myList[0] = iValue;
                adjust(0, i);
            }
        }

        private void adjust(int i, int n)
        {
            int iPosition;
            int iChange;

            iPosition = myList[i];
            iChange = 2 * i;
            while (iChange <= n)
            {
                if (iChange < n && myList[iChange] < myList[iChange + 1])
                {
                    iChange++;
                }
                if (iPosition >= myList[iChange])
                {
                    break;
                }
                myList[iChange / 2] = myList[iChange];
                iChange *= 2;
            }
            myList[iChange / 2] = iPosition;
        }

        public string printList()
        {
            string myValue = "";
            for (int i = 0; i < myLen; i++)
            {
                myValue += myList[i] + " ";
            }
            return myValue;
        }

        
    }
}
