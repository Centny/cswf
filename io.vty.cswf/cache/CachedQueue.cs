using io.vty.cswf.log;
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
        private static readonly ILog L = Log.New();
        protected Timer ClearTimer;
        public int MaxIdle = 0;

        public CachedQueue(int period = 30000, int idle = 0)
        {
            this.ClearTimer = new Timer(this.DoClear, null, period, period);
            this.MaxIdle = idle;
        }

        protected void DoClear(object state)
        {
            this.Clear(this.MaxIdle);
        }

        protected void Clear(int idle)
        {
            var cleared = 0;
            while (this.Count > idle)
            {
                T result;
                if (!this.TryDequeue(out result))
                {
                    break;
                }
                if (result is IDisposable)
                {
                    (result as IDisposable).Dispose();
                }
                cleared += 1;
            }
            if (cleared > 0)
            {
                L.D("CachedQueue clear {0} item for timeout", cleared);
            }
        }
        public virtual void Clear()
        {
            L.D("CachedQueue do clear all cached");
            this.Clear(0);
        }
    }
}
