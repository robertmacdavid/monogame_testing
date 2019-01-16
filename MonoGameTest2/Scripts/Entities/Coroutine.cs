using System;
using System.Collections.Generic;
using MonoGameTest2.DataStructures;
using MonoGameTest2.Managers;

namespace MonoGameTest2.Desktop.Scripts.Entities
{
    public class Coroutine : IComparable<Coroutine>
    {
        public double nextCallTime;
        public IEnumerator<double> enumerator;
        public bool IsRunning { get; private set; }


        public Coroutine(IEnumerator<Double> coroutine)
        {
            this.enumerator = coroutine;
            // define an iterator that yields execution delays
            // coroutine = Coroutine(iterator)
            // coroutine.Start()
        }

        public void Start()
        {
            this.enumerator.Reset();

            IsRunning = true;
            Update();

            if (IsRunning)
                GameManager.Instance.CoroutineQueue.Add(this);

        }

        public bool IsDue()
        {
            return nextCallTime <= GameManager.Instance.CurrentTimeMS;
        }

        public void Kill()
        {
            IsRunning = false;
        }

        public int CompareTo(Coroutine that)
        {
            return this.nextCallTime.CompareTo(that.nextCallTime);
        }

        public void Update()
        {
            if (!IsRunning) return;
            IsRunning = this.enumerator.MoveNext();
            nextCallTime = GameManager.Instance.CurrentTimeMS + enumerator.Current;
        }
    }



    public class CoroutineQueue
    {
        private PriorityQueue<Coroutine> queue;

        public CoroutineQueue()
        {
            queue = new PriorityQueue<Coroutine>();
        }

        public void Add(Coroutine coroutine)
        {
            queue.Enqueue(coroutine);
        }

        public int Count()
        {
            return queue.Count();
        }

        public void Update()
        {
            while(!queue.IsEmpty() && queue.Peek().IsDue())
            {
                Coroutine coroutine = queue.Dequeue();
                coroutine.Update();
                if (coroutine.IsRunning)
                {
                    queue.Enqueue(coroutine);
                }
            }
        }
    }
}
