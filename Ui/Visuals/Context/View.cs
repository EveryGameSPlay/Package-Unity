using System;
using UnityEngine;

namespace Egsp.Core.Ui
{
    public interface IView : IContext
    {
        void RootCanvas(Canvas rootCanvas);
    }
    
    [RequireComponent(typeof(Canvas))]
    public class View : Context, IView
    {
        [Tooltip("Канвасы помимо родного, которые должны быть в одном контексте.")]
        [SerializeField] private Canvas[] linkedRoots;

        protected override void Awake()
        {
            base.Awake();
            
            var ownCanvas = GetComponent<Canvas>();
            if(ownCanvas == null)
                throw new NullReferenceException();

            foreach (var root in linkedRoots)
            {
                RootCanvas(root);
            }
        }
        
        public void RootCanvas(Canvas rootCanvas)
        {
            FindAllEntities(rootCanvas);
            FindAllDetectors(rootCanvas);
        }
    }
}