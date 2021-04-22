using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;

namespace Egsp.Extensions.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Возвращает первый объект заданного типа. Может вернуть null.
        /// </summary>
        public static T FindType<T>(this IEnumerable source) where T : class
        {
            T coincidence = null;

            foreach (var obj in source)
            {
                coincidence = obj as T;

                // Если есть совпадение
                if (coincidence != null)
                {
                    return coincidence;
                }
            }

            return coincidence;
        }

        /// <summary>
        /// Производит поиск по типу объекта в коллекции. Если объект являяется структурой,
        /// то будет возвращена его копия.
        /// </summary>
        public static Option<T> FindTypeOption<T>(this IEnumerable source)
        {
            foreach (var obj in source)
            {
                if (obj is T casted)
                    return casted;
            }

            return Option<T>.None;
        } 

        /// <summary>
        /// Возвращает все типы заданного типа. Может вернуть пустой список.
        /// </summary>
        public static List<T> FindTypes<T>(this IEnumerable source) where T : class
        {    
            var coincidences = new List<T>();

            foreach (var obj in source)
            {
                var coincidence = source as T;

                // Если тип совпадает
                if (coincidence != null)
                {
                    coincidences.Add(coincidence);
                }
            }

            return coincidences;
        }
        
        /// <summary>
        /// Производит поиск по типу объекта в коллекции. Если объект являяется структурой,
        /// то будет возвращена его копия.
        /// </summary>
        public static List<T> FindTypesOption<T>(this IEnumerable source)
        {
            var coincidences = new List<T>();
            
            foreach (var obj in source)
            {
                if (obj is T casted)
                    coincidences.Add(casted);
            }
            return coincidences;
        } 

        /// <summary>
        /// Убирает все элементы списка являющиеся типом T.
        /// </summary>
        public static void RemoveByType<T>(this IList source)
        {
            for (var i = source.Count - 1; i > -1; i--)
            {
                if(source[i] is T)
                    source.RemoveAt(i);
            }
        }
        
        /// <summary>
        /// Возвращает индекс наибольшего элемента.
        /// Если в коллекции нет элементов, то будет возвращено -1
        /// </summary>
        public static int MaxIndex<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            var maxIndex = -1;
            var maxValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (selector(value).CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }
        
        /// <summary>
        /// Возвращает индекс наименьшего элемента.
        /// Если в коллекции нет элементов, то будет возвращено -1
        /// </summary>
        public static int MinIndex<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            // Изменить Func - сделать сравнение в Func
            
            var minIndex = -1;
            var minValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (minValue == null)
                {
                    minValue = value;
                    minIndex = index;
                }
                else if (selector(value).CompareTo(selector(minValue)) < 0 || minIndex == -1)
                {
                    minIndex = index;
                    minValue = value;
                }
                index++;
            }
            return minIndex;
        }

        /// <summary>
        /// Возвращает случайный элемент.
        /// При многочисленных выховах в один момент времени может выдавать один результат.
        /// Про подобное поведение читайте в официальной документации System.Random.
        /// </summary>
        public static T Random<T>(this IEnumerable<T> collection)
        {
            var randomIndex = new System.Random().Next(0,collection.Count());

            return collection.ElementAt(randomIndex);
        }

        private static int _seed;
        /// <summary>
        /// При вызове данного метода ключ будет менять значение.
        /// </summary>
        public static T RandomBySeed<T>(this IEnumerable<T> collection)
        {
            if (_seed == int.MaxValue)
                _seed = int.MinValue;

            _seed++;
            
            var randomIndex = new System.Random(_seed).Next(0,collection.Count());
            return collection.ElementAt(randomIndex);
        }
        
        /// <summary>
        /// При вызове данного метода ключ будет менять значение. Данная версия метода работает быстрее оригинальной.
        /// </summary>
        public static T RandomBySeed<T>(this IEnumerable<T> collection, int count)
        {
            if (_seed == int.MaxValue)
                _seed = int.MinValue;

            _seed++;

            var randomIndex = new System.Random(_seed).Next(0, count);
            return collection.ElementAt(randomIndex);
        }
        
        /// <summary>
        /// Возвращает случайный элемент.
        /// </summary>
        public static T Random<T>(this IEnumerable<T> collection, int collectionCount)
        {
            var randomIndex = new System.Random().Next(0,collectionCount);

            return collection.ElementAt(randomIndex);
        }

        /// <summary>
        /// Проходит по всем элементам и выполняет действие.
        /// </summary>
        public static IEnumerable ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        /// <summary>
        /// Возвращает первый найденный объект или ничего. Удобно для использования со структурами, т.к. структуры не
        /// могут быть null.
        /// </summary>
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof (source));
            if (predicate == null)
                throw new ArgumentNullException(nameof (predicate));
            foreach (T source1 in source)
            {
                if (predicate(source1))
                    return source1;
            }

            return Option<T>.None;
        }
    }
}