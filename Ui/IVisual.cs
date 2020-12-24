using Sirenix.OdinInspector;

namespace Egsp.Core.Ui
{
    public interface IVisual<TVisual>
    {
        void Enable();

        void Disable();
    }

    public abstract class SerializedVisual<TVisual> : SerializedMonoBehaviour, IVisual<TVisual>
    {
        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}