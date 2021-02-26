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
    }
}