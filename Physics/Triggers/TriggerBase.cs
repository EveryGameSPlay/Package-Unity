using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Egsp.Core
{
    public abstract class TriggerBase : MonoBehaviour, ITrigger
    {
        /// <summary>
        /// Размер буффера результатов.
        /// </summary>
        public const int OverlapResults = 10;
        
        public static readonly Color GizmosColor = new Color(1f, 1f, 0);
        
        [SerializeField] private TriggerEvent onEnter = new TriggerEvent();
        [SerializeField] private TriggerEvent onExit = new TriggerEvent();
        
        protected LinkedList<TriggerMark> Marked = new LinkedList<TriggerMark>();

        protected virtual void Update()
        {
            UnmarkAll();
            Overlap();
            ClearRuntimeResults();
            ClearExitOrNull();
        }

        /// <summary>
        /// Поиск всех объектов, входящих в триггер.
        /// </summary>
        protected abstract void Overlap();
        
        private void OnEnterInternal(GameObject enteredObject)
        {
            OnEnter(enteredObject);
            onEnter?.Invoke(new Trigg(this, enteredObject));
        }

        private void OnExitInternal(GameObject exitObject)
        {
            OnExit(exitObject);
            onExit?.Invoke(new Trigg(this, exitObject));
        }
        
        protected virtual void OnEnter(GameObject enteredObject)
        {
        }

        protected virtual void OnExit(GameObject exitObject)
        {
        }
        
        public virtual void OnEnterSubscribe(UnityAction<Trigg> triggAction)
        {
            onEnter?.AddListener(triggAction);
        }

        public virtual void OnExitSubscribe(UnityAction<Trigg> triggAction)
        {
            onExit?.AddListener(triggAction);
        }

        public virtual void Unsubscribe(UnityAction<Trigg> triggAction)
        {
            onEnter?.RemoveListener(triggAction);
            onExit?.RemoveListener(triggAction);
        }

        /// <summary>
        /// Помечает объект, как входящий в триггер.
        /// </summary>
        protected void MarkObject([NotNull] GameObject toMark)
        {
            for (var node = Marked.First; node != null;)
            {
                // value - struct of Mark
                // Если объект существует, то просто помечаем его.
                if (node.Value.gameObject == toMark)
                {
                    node.Value = node.Value.SetMark(true);
                    return;
                }
                node = node.Next;
            }
            
            // Объект не был обозначен, значит его нет в списке.
            OnEnterInternal(toMark);
            Marked.AddLast(new TriggerMark(toMark));
        }

        private void UnmarkAll()
        {
            for (var node = Marked.Last; node != null;)
            {
                node.Value = node.Value.SetMark(false);
                node = node.Previous;
            }
        }

        /// <summary>
        /// Очищает пометки объектов, которые вышли или более не существуют.
        /// </summary>
        protected void ClearExitOrNull()
        {
            // Проходимся по связному списку в обратном порядке.
            for (var node = Marked.Last; node != null;)
            {
                if (node.Value.gameObject == null)
                {
                    var previous = node.Previous;
                    Marked.Remove(node);
                    node = previous;
                    continue;
                }

                if (node.Value.marked)
                {
                    node = node.Previous;
                    continue;
                }

                OnExitInternal(node.Value.gameObject);
                
                var previous2 = node.Previous;
                Marked.Remove(node);
                node = previous2;
            }
        }
        
        protected abstract void ClearRuntimeResults();
        
        protected abstract void OnDrawGizmos();

        protected abstract Color GetGizmosColor(); 

        /// <summary>
        /// Структура, которая хранит информацию о пометке объекта, входящего в триггер.
        /// </summary>
        [Serializable]
        public struct TriggerMark
        {
            public bool marked;
            public GameObject gameObject;

            public TriggerMark(GameObject gameObject)
            {
                marked = true;
                this.gameObject = gameObject;
            }

            public override string ToString()
            {
                return $"{gameObject.name} ({marked})";
            }

            public TriggerMark SetMark(bool mark)
            {
                marked = mark;
                return this;
            }
        }
    }
}