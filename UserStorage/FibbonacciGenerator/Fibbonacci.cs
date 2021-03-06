﻿using System;
using System.Collections;
using System.Collections.Generic;
using Storage.Interfaces.Generator;

namespace FibbonacciGenerator
{
    [Serializable]
    public class FibonacciGenerator : IGenerator
    {
        private readonly IEnumerator<int> fibonacciIterator;
        
        public FibonacciGenerator()
        {
             fibonacciIterator = new Fibbonacci();
        }

        public int CurrentId => fibonacciIterator.Current;

        public void LoadState(int currentState)
        {
            while (fibonacciIterator.Current < currentState)
            {
                fibonacciIterator.MoveNext();
            }
        }

        public int GetNextId()
        {
            if (fibonacciIterator.MoveNext())
            {
                return fibonacciIterator.Current;
            }

            throw new InvalidOperationException("No more id.");           
        }

        [Serializable]
        private class Fibbonacci : IEnumerator<int>
        {
            private int prevNumber;
            private int number = 1;

            public int Current => number;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    checked
                    {
                        int tempNumber = number;
                        number += prevNumber;
                        prevNumber = tempNumber;
                    }
                }
                catch (OverflowException)
                {
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                prevNumber = 0;
                number = 1;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
