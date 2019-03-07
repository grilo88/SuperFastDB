using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast
{
    public class RadixSort_Strings
    {
        public RadixSort_Strings()
        {
            // na ordem descrição do alfabeto para a classificação radix (mudar isso para mudar a ordem)
            var alphabet = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            ValidityTest(alphabet);


            Stopwatch stopWatch = new Stopwatch();

            var testStringNumbers = new List<int>() { 50, 100, 200, 500, 1000, 1500 };
            var numSamples = 5;
            var maxWordLength = 7;
            var minWordLength = 1;

            foreach (int i in testStringNumbers)
            {

                var list = CreateListOfStrings(i, minWordLength, maxWordLength, alphabet);

                var times1 = new List<Double>();
                var times2 = new List<Double>();
                for (int j = 1; j <= numSamples; j++)
                {
                    var list2 = Clone<string>(list).ToList(); //List.Sort() is an in-place sort				
                    stopWatch.Start();
                    list2.Sort();
                    stopWatch.Stop();
                    times1.Add(stopWatch.ElapsedTicks);
                    stopWatch.Reset();

                    stopWatch.Start();
                    var sorted = LSDSort(list, alphabet, maxWordLength);
                    stopWatch.Stop();
                    times2.Add(stopWatch.ElapsedTicks);

                }
                var avg1 = times1.Sum() / times1.Count();
                var avg2 = times2.Sum() / times2.Count();
                Console.WriteLine(String.Format("built-in sort - n:[{0}] avg time(ticks):[{1}]", i, avg1));
                Console.WriteLine(String.Format("radix    sort - n:[{0}] avg time(ticks):[{1}]", i, avg2));
                Console.WriteLine(String.Format("                n:[{0}] ratio:[{1}]", i, avg1 / avg2));
            }
        }

        // tipo de dígito menos significativo
        private static List<string> LSDSort(List<string> input, string alphabet, int maxLength)
        {
            var workingList = input;
            var tempResult = new List<string>();

            var alphaDict = new Dictionary<char, int>();
            for (int i = 0; i < alphabet.Length; i++)
                alphaDict.Add(alphabet[i], i + 1);

            // loop para cada índice de char (começando pelo menos - menos significativo - char				
            for (int charLoc = maxLength - 1; charLoc >= 0; charLoc--)
            {

                var queues = new Queue<string>[alphabet.Length + 1];
                for (int i = 0; i < alphabet.Length + 1; i++)
                    queues[i] = new Queue<string>();
                // coloque as diferentes cadeias nas filas apropriadas
                foreach (var str in workingList)
                {
                    int queueIndex = 0;
                    if (charLoc < str.Length)
                    {
                        char cr = str[charLoc];
                        queueIndex = alphaDict[cr];
                    }
                    queues[queueIndex].Enqueue(str);
                }
                // combine todas as filas
                for (int queueIndex = 0; queueIndex <= alphabet.Length; queueIndex++)
                {
                    var queue = queues[queueIndex];
                    if (queue != null)
                    {

                        tempResult.AddRange(queue.ToArray());

                    }
                }
                workingList = tempResult;
                tempResult = new List<string>();
            }

            return workingList;
        }

        private static void ValidityTest(string alphabet)
        {

            var testStrings = new List<string>() { "ABBA", "ABRA", "CADABRA", "FIFTY", "BARRELS", "OF WINE", "ON", "THE", "WALL", "ANJNFF", "GIBBERISH", "MORE GIBBERISH", "1222", "A1234", "NCC1701", " 1 " };
            Console.WriteLine("Original List:      " + String.Join("|", testStrings.ToArray()));

            var copy = Clone<string>(testStrings).ToList();

            copy.Sort();

            var LSDSorted = LSDSort(testStrings, alphabet, 12);
            Console.WriteLine("List.Sort() output: " + String.Join("|", copy.ToArray()));
            Console.WriteLine("LSDSort()   output: " + String.Join("|", LSDSorted.ToArray()));
            bool match = true;
            for (int i = 0; i < copy.Count; i++)
            {
                match = match && copy[i] == LSDSorted[i];
                Debug.Assert(copy[i] == LSDSorted[i]);
            }
            Console.WriteLine(match);
        }

        // cria uma lista de n strings geradas aleatoriamente entre os caracteres minSize e maxSize(distribuição uniforme)
        private static List<string> CreateListOfStrings(long n, int minSize, int maxSize, string chars)
        {
            var result = new List<string>();
            var rand = new Random(DateTime.Now.Ticks.GetHashCode());
            for (int i = 1; i <= n; i++)
            {
                var numOfChars = rand.Next(minSize, maxSize);
                result.Add(new string(Enumerable.Repeat(chars, numOfChars)
              .Select(s => s[rand.Next(s.Length)]).ToArray()));
            }
            return result;
        }

        private static IList<T> Clone<T>(IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
