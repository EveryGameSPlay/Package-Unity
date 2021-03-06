﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using URandom = UnityEngine.Random;

namespace Egsp.Core
{
    /// <summary>
    /// Класс, содержащий методы для операций со случайностями
    /// </summary>
    public static class RandomUtils
    {
        /// <summary>
        /// Следует ли игнорировать незначительные ошибки
        /// </summary>
        public static bool IgnoreNotCriticalExceptions = true;
        
        /// <summary>
        /// Получение случайного элемента из списка с помощью весов.
        /// Автоматическая сортировка по возрастанию.
        /// </summary>
        /// <param name="objects">Список объектов</param>
        /// <param name="selector">Функция определения веса объекта. Объект должен вернуть число - вес</param>
        public static T SelectByWeight<T>(IEnumerable<T> objects, Func<T, float> selector)
        {
            // Если список пустой
            if (objects.Count() == 0)
            {
                return default(T);
            }

            // Сортировка по возрастанию
            var orderedObjects = objects.OrderBy(x => selector(x));

            return SelectByWeightInternal(orderedObjects, selector);
        }

        /// <param name="objects">Список объектов</param>
        /// <param name="selector">Функция определения веса объекта. Объект должен вернуть число - вес</param>
        public static T SelectByWeightOrdered<T>(IEnumerable<T> orderedObjects, Func<T, float> selector)
        {
            // Если список пустой
            if (orderedObjects.Count() == 0)
            {
                return default(T);
            }
            
            return SelectByWeightInternal(orderedObjects, selector);
        }

        private static T SelectByWeightInternal<T>(IEnumerable<T> ordered, Func<T, float> selector)
        {
            
            // Сумма всех весов
            var total = 0f;

            foreach (var obj in ordered)
            {
                total += selector(obj);
            }

            var top = 0f;
            var pointer = ((float) new Random().NextDouble()) * total;
            
            foreach (var obj in ordered)
            {
                // Сдвигаем верхнюю границу
                top += selector(obj);
                
                if (pointer <= top)
                    return obj;
            }
            
            // Сюда код не должен доходить
            if (IgnoreNotCriticalExceptions)
            {
                // Возвращаем элемент с наибольшим весом
                return ordered.Last();
            }
            else
            {
                throw new Exception($"Случайный объект не был выбран с помощью весов " +
                                    $"{pointer} pointer; " +
                                    $"{total} total; " +
                                    $"{selector(ordered.Last())} biggest weight" +
                                    $" (SelectByWeight)");
            }
        }

        /// <summary>
        /// Генерирует случайное число. Если число меньше шанса или равно шансу, то результат true
        /// </summary>
        /// <param name="chance">Шанс от 0 включительно до 100 включительно</param>
        public static bool ProkeChance(float chance)
        {
            var randomValue = ((float) new Random().NextDouble()) * 100;

            if (randomValue <= chance)
                return true;

            return false;
        }
        
        /// <summary>
        /// Генерирует случайное число. Если число меньше шанса или равно шансу, то результат true
        /// </summary>
        /// <param name="chance">Шанс от 0 включительно до 1 включительно</param>
        public static bool ProkeChance01(float chance)
        {
            var randomValue = ((float) new Random().NextDouble());

            if (randomValue <= chance)
                return true;

            return false;
        }
        
        /// <summary>
        /// Генерирует случайный цвет. 
        /// </summary>
        public static Color Color(float minAlpha = 1)
        {
            return new Color(URandom.Range(0.1f,1),URandom.Range(0.1f,1),URandom.Range(0.1f,1),
                URandom.Range(minAlpha,1));
        }
    }
}