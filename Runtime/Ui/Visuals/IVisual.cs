namespace Egsp.Core.Ui
{
    /// <summary>
    /// Базовый интерфейс для всех визуальных элементов интерфейса, содержащих логику.
    /// </summary>
    public interface IVisual
    {
        bool InAnimation { get; }
        
        void Enable();

        void Disable();
    }
}