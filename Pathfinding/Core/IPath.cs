using System.Collections.Generic;

namespace Pathfinding
{
    public interface IPath<TPoint>
    {
        TPoint Current { get; }
        
        IEnumerable<TPoint> Points { get; }

        bool Continue(TPoint origin);
    }
}