using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Данный компонент лишь реализует интерфейс IContextEntity.
    /// </summary>
    public abstract class ContextedMonoBehaviour : MonoBehaviour, IContextEntity
    {
        private IContext _context;

        public IContext Context
        {
            get => _context;
            set => _context = value;
        }
    }
}