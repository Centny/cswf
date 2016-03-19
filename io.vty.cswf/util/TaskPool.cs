using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.util
{
    public class TaskPool
    {
        public static TaskPool Shared = new TaskPool();
        public static void Queue(Action<object> action, object state)
        {
            Shared.Add(action, state);
        }
        public class Item
        {
            public Action<object> Act;
            public object State;
        }
        public int MaximumConcurrency { get; set; }
        public Queue<Item> Queued { get; protected set; }
        public ICollection<Item> Running { get; protected set; }


        public TaskPool(int max = 8)
        {
            this.MaximumConcurrency = max;
            this.Queued = new Queue<Item>();
            this.Running = new List<Item>();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Add(Action<object> action, object state)
        {
            var item = new Item { Act = action, State = state };
            if (this.Running.Count < this.MaximumConcurrency)
            {
                this.run(item);
            }
            else
            {
                this.Queued.Enqueue(item);
            }
        }
        protected virtual void run(Item item)
        {
            this.Running.Add(item);
            new Task(i =>
            {
                var act = i as Item;
                try
                {
                    this.OnStart(act);
                    act.Act(act.State);
                    this.OnDone(act);
                }
                catch (Exception e)
                {
                    this.OnDone(act, e);
                }
                this.Running.Remove(act);
                this.Next();
            }, item).Start();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual void Next()
        {
            if (this.Queued.Count < 1)
            {
                return;
            }
            this.run(this.Queued.Dequeue());
        }

        protected virtual void OnStart(Item item)
        {

        }
        protected virtual void OnDone(Item item, Exception e = null)
        {

        }



    }
}
