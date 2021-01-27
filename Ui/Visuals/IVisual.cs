namespace Egsp.Core.Ui
{
    public interface IVisual
    {
        bool InAnimation { get; }
        
        void Enable();

        void Disable();
    }
}