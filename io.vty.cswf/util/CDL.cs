using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace io.vty.cswf.util
{
    /// <summary>
    /// Providers a util to sync thead by count. 
    /// </summary>
    public class CDL
    {
        /// <summary>
        /// the total count.
        /// </summary>
        public virtual int count { get; protected set; }

        /// <summary>
        /// current count.
        /// </summary>
        public virtual int current { get; protected set; }

        /// <summary>
        /// the default constructor by total count.
        /// </summary>
        /// <param name="c"></param>
        public CDL(int c)
        {
            this.count = c;
            this.current = 0;
        }

        /// <summary>
        /// wait median count.
        /// </summary>
        /// <param name="c"></param>
        public virtual void wait(int c)
        {
            lock (this)
            {
                while (this.current < c)
                {
                    Monitor.Wait(this);
                }
            }
        }

        /// <summary>
        /// wait total count.
        /// </summary>
        public virtual void wait()
        {
            lock (this)
            {
                while (this.current < this.count)
                {
                    Monitor.Wait(this);
                }
            }
        }

        /// <summary>
        /// complete one.
        /// </summary>
        public virtual void done()
        {
            lock (this)
            {
                this.current++;
                if (this.current > this.count)
                {
                    throw new Exception("done count greater total count");
                }
                Monitor.Pulse(this);
            }
        }
    }
}
