using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast.Sort
{
    public class RadixSort_Numeros
    {
        public static void RadixSort(int[] vetor)
        {
            int i;
            int[] b;
            int maior = vetor[0];
            int exp = 1;

            b = new int[vetor.Length];

            for (i = 0; i < vetor.Length; i++)
            {
                if (vetor[i] > maior)
                    maior = vetor[i];
            }

            while (maior / exp > 0)
            {
                int[] bucket = new int[10];
                for (i = 0; i < vetor.Length; i++)
                    bucket[(vetor[i] / exp) % 10]++;
                for (i = 1; i < 10; i++)
                    bucket[i] += bucket[i - 1];
                for (i = vetor.Length - 1; i >= 0; i--)
                    b[--bucket[(vetor[i] / exp) % 10]] = vetor[i];
                for (i = 0; i < vetor.Length; i++)
                    vetor[i] = b[i];
                exp *= 10;
            }
        }
    }
}
