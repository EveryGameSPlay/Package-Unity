using System;
using System.Collections.Generic;

namespace Egsp.Extensions.Collections
{
    public static class LinkedListExtensions
    {
        public static LinkedList<T> Join<T>(this LinkedList<T> source, LinkedList<T> another)
        {
            foreach (var value in another)
            {
                source.AddLast(value);
            }

            return source;
        }
        
        public static LinkedList<T> Join<T>(this LinkedList<T> source, LinkedList<T> another, Action<T> action)
        {
            foreach (var value in another)
            {
                action(value);
                source.AddLast(value);
            }

            return source;
        }

        public static LinkedList<T> Remove<T>(this LinkedList<T> source, Func<T,bool> predicate)
        {
            var node = source.First;
            while (node != null)
            {
                var next = node.Next;
                if (predicate(node.Value))
                {
                    source.Remove(node);
                    break;
                }

                node = next;
            }

            return source;
        }

        public static LinkedList<T> RemoveAll<T>(this LinkedList<T> source, Func<T, bool> predicate)
        {
            var node = source.First;
            while (node != null)
            {
                var next = node.Next;
                if (predicate(node.Value))
                {
                    source.Remove(node);
                }

                node = next;
            }

            return source;
        }

        public static LinkedList<T> ToLinkedList<T>(this List<T> list)
        {
            var linkedList = new LinkedList<T>();

            for (var i = 0; i < list.Count; i++)
            {
                linkedList.AddLast(list[i]);
            }

            return linkedList;
        }
    }
}