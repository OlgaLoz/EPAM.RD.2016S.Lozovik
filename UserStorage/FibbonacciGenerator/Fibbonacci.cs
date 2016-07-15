using System.Collections;
using System.Collections.Generic;

namespace FibbonacciGenerator
{
    public class Fibbonacci : IEnumerator<int>
    {
        private int prevNumber;
        private int number = 1;

        public int Current => number;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            int tempNumber = number;
            number += prevNumber;
            prevNumber = tempNumber;
            return true;
        }

        public void Reset()
        {
            prevNumber = 0;
            number = 1;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
