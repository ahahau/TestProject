using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.PathFinding
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public List<T> heap = new List<T>();
        public int Count => heap.Count;

        public void Clear() => heap?.Clear();

        public T Contains(T t)
        {
            int index = heap.IndexOf(t);
            if (index < 0) return default(T);
            return heap[index];
        }

        public void Push(T data)
        {
            heap.Add(data);
            int now = heap.Count - 1;
            while (now > 0)
            {
                int next = (now - 1) / 2;
                if(heap[now].CompareTo(heap[next]) < 0)
                    break;

                (heap[now], heap[next]) = (heap[next], heap[now]);
                
                now = next;
            }
        }

        public T Pop()
        {
            T result = heap[0];

            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);
            //여기서부터 연우가 화장실 감.
            lastIndex--; //하나가 빠졌으니 인덱스를 줄여준다.
            
            //이제 내려가면서 자기자리를 찾아가면 된다.
            int now = 0;
            while (true)
            {
                int left = now*2 + 1;
                int right = now*2 + 2;

                int next = now;

                if (left <= lastIndex && heap[next].CompareTo(heap[left]) < 0)
                    next = left;

                if (right <= lastIndex && heap[next].CompareTo(heap[right]) < 0)
                    next = right;
                
                if(next == now)
                    break;

                (heap[now], heap[next]) = (heap[next], heap[now]);

                now = next;
            }
            
            return result;
        }

        public T Peek()
        {
            return heap.Count == 0 ? default(T) : heap[0];
        }
    }
}