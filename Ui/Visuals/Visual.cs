using UnityEngine;

namespace Egsp.Core.Ui
{
    public abstract class Visual<TVisual> : MonoBehaviour, IVisual<TVisual>
    {
        public bool InAnimation { get; private set; }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        protected void EnterAnimation() => InAnimation = true;
        protected void ExitAnimation() => InAnimation = false;
    }
}