using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Egsp.Core
{
    public abstract class TriggerBase : MonoBehaviour, ITrigger
    {
        public const int OverlapResults = 10;
        protected readonly Color GizmosColor = new Color(1f, 1f, 0);
        
        [SerializeField] private TriggerEvent onEnter = new TriggerEvent();
        [SerializeField] private TriggerEvent onExit = new TriggerEvent();

        protected List<Mark> Marked = new List<Mark>();

        protected virtual void Update()
        {
            UnmarkAll();
            Overlap();
            ClearRuntimeResults();
            ClearExitOrNull();
        }

        protected abstract void Overlap();
        
        public virtual void OnEnter(UnityAction<Trigg> triggAction)
        {
            onEnter?.AddListener(triggAction);
        }

        public virtual void OnExit(UnityAction<Trigg> triggAction)
        {
            onExit?.AddListener(triggAction);
        }

        public virtual void Unsubscribe(UnityAction<Trigg> triggAction)
        {
            onEnter?.RemoveListener(triggAction);
            onExit?.RemoveListener(triggAction);
        }

        protected void MarkObject([NotNull] GameObject toMark)
        {
            for (var i = 0; i < Marked.Count; i++)
            {
                var mark = Marked[i];

                // Если объект существует, то просто помечаем его.
                if (mark.gameObject == toMark)
                {
                    mark.marked = true;
                    return;
                }
            }
            
            // Объект не был обозначен, значит его нет в списке.
            onEnter?.Invoke(new Trigg(this, toMark.gameObject));
            Marked.Add(new Mark(toMark));
        }

        private void UnmarkAll()
        {
            for (var i = Marked.Count-1; i >-1; i--)
            {
                Marked[i].marked = false;
            }
        }

        protected abstract void ClearRuntimeResults();

        protected void ClearExitOrNull()
        {
            for (var i = Marked.Count-1; i >-1; i--)
            {
                var mark = Marked[i];

                if (mark.gameObject == null)
                {
                    Marked.RemoveAt(i);
                    continue;
                }
                
                if(mark.marked)
                    continue;
                
                onExit?.Invoke(new Trigg(this, mark.gameObject));
                Marked.RemoveAt(i);
            }
        }

        [Serializable]
        public sealed class Mark
        {
            public bool marked;
            public GameObject gameObject;

            public Mark(GameObject gameObject)
            {
                marked = true;
                this.gameObject = gameObject;
            }

            public override string ToString()
            {
                return $"{gameObject.name} ({marked})";
            }
        }
    }
}