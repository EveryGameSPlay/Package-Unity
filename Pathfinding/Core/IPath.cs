using System.Collections.Generic;

namespace Egsp.Core.Pathfinding
{
    /// <summary>
    /// Интерфейс представления пути с методами для перехода между точками.
    /// </summary>
    /// <typeparam name="TPoint"></typeparam>
    public interface IPath<TPoint>
    {
        TPoint Current { get; }
        
        IEnumerable<TPoint> Points { get; }

        bool Continue(TPoint origin);
    }
}