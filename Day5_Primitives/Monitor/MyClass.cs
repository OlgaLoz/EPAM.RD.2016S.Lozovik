using static System.Threading.Monitor;

namespace Monitor
{
    // TODO: Use Monitor (not lock) to protect this structure.
    public class MyClass
    {
        private int value;

        private readonly object lockObj = new object();

        public int Counter
        {
            get
            {
                return value;
            }
            set
            {
                Enter(lockObj);
                try
                {
                    this.value = value;
                }
                finally
                {
                    Exit(lockObj);
                }
            }
        }

        public void Increase()
        {
            Enter(lockObj);
            try
            {
                value++;
            }
            finally 
            {
                Exit(lockObj);    
            }
        }

        public void Decrease()
        {
            Enter(lockObj);
            try
            {
                value--;
            }
            finally
            {
                Exit(lockObj);
            }
        }
    }
}
