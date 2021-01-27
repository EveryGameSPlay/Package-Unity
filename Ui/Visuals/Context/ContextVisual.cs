namespace Egsp.Core.Ui
{
    public class ContextVisual : Visual, IContextVisual
    {
        private IContext _context;

        public IContext Context
        {
            get => _context;
            set
            {
                _context = value;
                if (_context != null)
                    Bus = _context.Bus;
            }
        }

        public IEventBus Bus { get; private set; }
    }
}