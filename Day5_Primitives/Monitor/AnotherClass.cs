using System.Threading;

namespace Monitor
{
    // TODO: Use SpinLock to protect this structure.
    public class AnotherClass
    {
        private int value;
        SpinLock spinLock = new SpinLock();

        public int Counter
        {
            get
            {
                return value;
            }
            set
            {
                bool lockTaken = false;
                try
                {
                    spinLock.Enter(ref lockTaken);
                    this.value = value;
                }
                finally
                {
                    if (lockTaken) spinLock.Exit(false);
                }
               
            }
        }

        public void Increase()
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
                value++;
            }
            finally
            {
                if (lockTaken) spinLock.Exit(false);
            }
        }

        public void Decrease()
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
                value--;
            }
            finally
            {
                if (lockTaken) spinLock.Exit(false);
            }
        }
    }
}
