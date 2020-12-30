using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Extensions.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Egsp.RandomTools
{
    public class WeightedItem<TItem> 
    {
        /// <summary>
        /// Вес объекта.
        /// </summary>
        public float Weight { get; set; }
        
        [NotNull]
        public TItem Value { get; private set; }

        public WeightedItem([ItemNotNull] TItem value, float weight)
        {
            Value = value;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{Value.ToString()} : {Weight}";
        }

        public static float Selector(WeightedItem<TItem> item)
        {
            return item.Weight;
        }
    }

    public interface IWeightBalancer<TItem>
    {
        string Name { get; }
        void Balance(IEnumerable<WeightedItem<TItem>> source, WeightedItem<TItem> balanceItem, float step,
            int sourceCount = -1);
    }

    /// <summary>
    /// Данный балансер просто перекидывает степень изменения на другой объект в положительную сторону.
    /// Не гарантировано что балансирован будет другой объект.
    /// </summary>
    public class ThrowOverBalancer<TItem> : IWeightBalancer<TItem>
    {
        private readonly WeightedList<TItem> _weightedList;

        public ThrowOverBalancer([NotNull] WeightedList<TItem> weightedList)
        {
            _weightedList = weightedList;
        }

        public string Name => typeof(ThrowOverBalancer<TItem>).Name;

        public void Balance([NotNull]IEnumerable<WeightedItem<TItem>> source,
            [NotNull]WeightedItem<TItem> balanceItem, float step, int sourceCount = -1)
        {
            if (step == 0)
                return;

            // Уменьшение веса основного обекта.
            balanceItem.Weight -= step;
            if (balanceItem.Weight < _weightedList.Minimum)
                balanceItem.Weight = _weightedList.Minimum;

            // Если количество элементов неизвестно.
            if(sourceCount == -1) 
                sourceCount = source.Count();

            // В коллекции содержится только небалансируемый элемент.
            if (sourceCount == 1)
            {
                balanceItem.Weight += step;
                return;
            }

            // Выборка случайного объекта, которому увеличиваем вес.
            var randomPicked = source.RandomBySeed();
            randomPicked.Weight += step;
            
            if(randomPicked == balanceItem)
                Debug.LogWarning($"Coincidence {randomPicked.Value.ToString()}");

            if (randomPicked.Weight > _weightedList.Maximum)
                randomPicked.Weight = _weightedList.Maximum;

            return;
        }
    }

    /// <summary>
    /// Ничего не балансирует.
    /// </summary>
    public class NothingBalancer<TItem> : IWeightBalancer<TItem>
    {
        public string Name => typeof(NothingBalancer<TItem>).Name;
        public void Balance(IEnumerable<WeightedItem<TItem>> source,
            WeightedItem<TItem> balanceItem, float step, int sourceCount = -1)
        {
            return;
        }
    }
    
    public sealed class WeightedList<TItem> : IList<WeightedItem<TItem>>
    {
        /// <summary>
        /// Балансировщик значений веса объектов.
        /// </summary>
        [NotNull]
        public IWeightBalancer<TItem> WeightBalancer { get; private set; }
        
        /// <summary>
        /// Список элементов. Неупорядоченный.
        /// </summary>
        [NotNull]
        private List<WeightedItem<TItem>> _list;

        /// <summary>
        /// Список элементов. Упорядоченный.
        /// </summary>
        [NotNull]
        public IEnumerable<WeightedItem<TItem>> OrderedList
        {
            get => _cachedOrderList;
            private set => _cachedOrderList = value;
        }

        [CanBeNull]
        private IEnumerable<WeightedItem<TItem>> _cachedOrderList;
        
        /// <summary>
        /// Минимальный вес элементов.
        /// </summary>
        public float Minimum { get; private set; }
        /// <summary>
        /// Максимальный вес элементов.
        /// </summary>
        public float Maximum { get; private set; }
        /// <summary>
        /// Степень изменения веса при рандоме.
        /// </summary>
        public float Step { get; set; }

        public WeightedList(int capacity = 0)
        {
            _list = new List<WeightedItem<TItem>>(capacity);
            WeightBalancer = new NothingBalancer<TItem>();
            CacheOrder();
        }

        public WeightedList(float minimum, int capacity = 0) : this(capacity)
        {
            Minimum = minimum;
        }
        
        public WeightedList(float maximum, float minimum, int capacity = 0) : this(minimum,capacity)
        {
            Maximum = maximum;
        }

        /// <summary>
        /// Кэширование очереди.
        /// </summary>
        public void CacheOrder()
        {
            OrderedList = _list.OrderBy(x => x.Weight);
        }

        /// <summary>
        /// Возвращает случайный объект и балансирует коллекцию.
        /// </summary>
        public WeightedItem<TItem> Pick()
        {
            var item = RandomUtils.SelectByWeightOrdered(OrderedList, WeightedItem<TItem>.Selector);
            if(item == null)
                throw new NullReferenceException();
            
            // Балансировка.
            WeightBalancer.Balance(OrderedList, item, Step,Count);
            return item;
        }

        /// <summary>
        /// Устанавливает балансировщик.
        /// </summary>
        public void SetBalancer([NotNull]IWeightBalancer<TItem> balancer)
        {
            if(balancer == null)
                throw new NullReferenceException();

            WeightBalancer = balancer;
        }


        // INTERFACE --------------------------------------------------------

        public IEnumerator<WeightedItem<TItem>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель упорядоченной коллекции.
        /// </summary>
        public IEnumerator<WeightedItem<TItem>> GetEnumeratorOrder()
        {
            return OrderedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add([NotNull]WeightedItem<TItem> item)
        {
            if (item == null)
                throw new NullReferenceException();
            
            _list.Add(item);
            CacheOrder();
        }

        /// <summary>
        /// Добавляет элемент и устанавливает ему минимальный вес.
        /// </summary>
        public WeightedItem<TItem> AddItem([NotNull] TItem item)
        {
            if (item == null)
                throw new NullReferenceException();
            
            var weightedItem = new WeightedItem<TItem>(item, Minimum);
            Add(weightedItem);
            
            return weightedItem;
        }
        
        public WeightedItem<TItem> AddItem([NotNull] TItem item, float weight)
        {
            var weightedItem = AddItem(item);
            weightedItem.Weight = weight;
            
            return weightedItem;
        }

        public void Clear()
        {
            _list.Clear();
            CacheOrder();
        }

        public bool Contains([NotNull]WeightedItem<TItem> item)
        {
            return _list.Contains(item);
        }

        public bool ContainsItem([NotNull]TItem item)
        {
            return _list.Exists(x => x.Value.Equals(item));
        }

        public void CopyTo(WeightedItem<TItem>[] array, int arrayIndex)
        {
            _list.CopyTo(array,arrayIndex);
        }

        public bool Remove(WeightedItem<TItem> item)
        {
            if (item == null)
                return false;
            
            var removed = _list.Remove(item);
            
            CacheOrder();
            return removed;
        }

        public bool RemoveItem([CanBeNull]TItem item)
        {
            if (item == null)
                return false;

            var coincidence = _list
                .FirstOrDefault(x => x.Value.Equals(item));

            if (coincidence == null)
                return false;

            _list.Remove(coincidence);
            
            CacheOrder();
            return true;
        }

        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public int IndexOf(WeightedItem<TItem> item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, WeightedItem<TItem> item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public WeightedItem<TItem> this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                CacheOrder();
            }
        }

        public static WeightedList<TItem> FromList(IList<TItem> list,
            float minimum = 10f, float maximum = 100f, float defaultWeight = 50f)
        {
            var weightedList = new WeightedList<TItem>(maximum, minimum, list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                weightedList.AddItem(list[i], defaultWeight);
            }

            return weightedList;
        }
    }
}