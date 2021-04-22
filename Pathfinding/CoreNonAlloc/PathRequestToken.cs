using System;

namespace Egsp.Core.Pathfinding.NonAlloc
{
    /// <summary>
    /// Прослойка между запросом и поиском пути.
    /// </summary>
    public sealed class PathRequestToken<TPoint>
    {
        public bool IsReady { get; private set; }
        
        private Path<TPoint> _path;

        [Obsolete("GetPathRef instead.")]
        public Path<TPoint> Path
        {
            get => _path;
            private set => _path = value;
        }

        public void Ready(Path<TPoint> path)
        {   
            IsReady = true;
            _path = path;
        }

        public ref Path<TPoint> GetPathRef()
        {
            return ref _path;
        }
    }
}