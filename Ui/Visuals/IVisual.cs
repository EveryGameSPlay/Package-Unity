namespace Egsp.Core.Ui
{
    public interface IVisual<TVisual>
    {
        bool InAnimation { get; }
        
        void Enable();

        void Disable();
    }
}