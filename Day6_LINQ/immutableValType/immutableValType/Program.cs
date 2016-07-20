using System;

namespace immutableValType
{

    public interface IChangeable
    {
        void Change(int a, int b);
    }
    // Point  размерный тип.
    internal struct Point : IChangeable
    {
        private int _x, _y;
        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public void Change(int x, int y)
        {
            _x = x; _y = y;
        }
        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }
    }
    public sealed class Program
    {
        public static void Main()
        {
            Point p = new Point(1, 1);
            Console.WriteLine(p);
            p.Change(2, 2);
            Console.WriteLine(p);
            object o = p;
            Console.WriteLine(o);
            ((Point)o).Change(3, 3);
            ((IChangeable)o).Change(4, 4);
            Console.WriteLine(o);
            Console.ReadLine();
        }
    }
}
