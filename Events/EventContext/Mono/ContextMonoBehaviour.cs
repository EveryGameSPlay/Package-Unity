using UnityEngine;

namespace Egsp.Core
{
    public abstract class ContextMonoBehaviour : MonoBehaviour, IContextEntity
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