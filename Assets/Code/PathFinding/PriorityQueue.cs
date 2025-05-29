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

        public T Contain(T t)
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
                if(heap[next].CompareTo(heap[now]) < 0)
                    break;
                (heap[now], heap[next]) = (heap[next], heap[now]);
                now = next;
            }
        }

        public T Pop()
        {
            T result = heap[0];
            int lastIndex = heap.Count - 1;
            heap[0] = heap[^1];
            heap.RemoveAt(lastIndex);
            lastIndex--;
            int now = 0;
            while (true)
            {
                int left = now * 2 + 1;
                int right = now * 2 + 2;
                int next = now;
                
                if(left < lastIndex && heap[next].CompareTo(heap[left]) < 0)
                    next = left;
                if(right < lastIndex && heap[next].CompareTo(heap[right]) < 0)
                    next = right;
                if (next == now)
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