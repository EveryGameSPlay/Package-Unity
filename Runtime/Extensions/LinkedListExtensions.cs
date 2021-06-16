using System;
using System.Collections.Generic;
using Egsp.Core;

namespace Egsp.Core
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Присоединяет другой список в конец текущего.
        /// </summary>
        public static LinkedList<T> Join<T>(this LinkedList<T> source, LinkedList<T> another)
        {
            foreach (var value in another)
            {
                source.AddLast(value);
            }

            return source;
        }
        
        /// <summary>
        /// Присоединяет другой список в конец текущего. Перед присоединением можно вызвать действие над объектом.
        /// </summary>
        public static LinkedList<T> Join<T>(this LinkedList<T> source, LinkedList<T> another, Action<T> action)
        {
            foreach (var value in another)
            {
                action(value);
                source.AddLast(value);
            }

            return source;
        }

        /// <summary>
        /// Убирает один элемент из списка. Возвращает ссылку на оригинальный список. EGSP.
        /// </summary>
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

        /// <summary>
        /// Убирает все подходящие элементы из списка. Возвращает ссылку на оригинальный список.
        /// </summary>
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
        
        /// <summary>
        /// Возвращает первый найденный объект или ничего. Удобно для использования со структурами, т.к. структуры не
        /// могут быть null.
        /// </summary>
        public static Option<LinkedListNode<T>> FirstOrNone<T>(this LinkedList<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof (source));
            if (predicate == null)
                throw new ArgumentNullException(nameof (predicate));

            var node = source.First;
            if(node == null)
                return Option<LinkedListNode<T>>.None;

            while (node != null)
            {
                if (predicate(node.Value))
                    return node;

                node = node.Next;
            }
            
            return Option<LinkedListNode<T>>.None;
        }

        /// <summary>
        /// Применяет к первому найденному объекту переданный метод.
        /// </summary>
        public static void Apply<T>(this LinkedList<T> source, Func<T, bool> predicate,
            Action<LinkedListNode<T>> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof (source));
            if (predicate == null)
                throw new ArgumentNullException(nameof (predicate));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var node = source.First;
            if (node == null)
                return;

            while (node != null)
            {
                if (predicate(node.Value))
                {
                    action(node);
                    return;
                }
                node = node.Next;
            }
        }
    }
}