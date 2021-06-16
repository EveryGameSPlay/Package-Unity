namespace Egsp.Core.Ui
{
    /// <summary>
    /// Визуальный элемент, который связан с контекстом.
    /// </summary>
    public abstract class ContextedVisual : Visual, IContextedVisual
    {
        private IContext _context;

        public IContext Context
        {
            get => _context;
            set => _context = value;
        }
    }
}