using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace io.vty.cswf.cache
{
    public class CachedQueue<T> : ConcurrentQueue<T>
    {
        protected Timer ClearTimer;
        public int MaxIdle = 0;

        public CachedQueue(int period = 30000, int idle = 0)
        {
            this.ClearTimer = new Timer(this.DoClear, null, period, period);
        }

        protected void DoClear(object state)
        {
            this.Clear(this.MaxIdle);
        }
        
        protected void Clear(int idle)
        {
            while (this.Count > idle)
            {
                T result;
                if (!this.TryDequeue(out result))
                {
                    continue;
                }
                if (result is IDisposable)
                {
                    (result as IDisposable).Dispose();
                }
            }
        }
        public virtual void Clear()
        {
            this.Clear(0);
        }
    }
}
