using System;
using JetBrains.Annotations;

namespace Egsp.Core.Pathfinding.NonAlloc
{
    /// <summary>
    /// Ответ на запрос поиска пути.
    /// </summary>
    public struct PathResponse<TPoint>
    {
        // Ссылка на токен.
        [CanBeNull] public readonly PathRequestToken<TPoint> Token;

        public bool Ready => Token == null ? true : Token.IsReady;
        
        // Данное значение используется только при отсутствии токена.
        private Path<TPoint> _path;
        
        /// <summary>
        /// Конструктор для синхронного пути. Не занимается место под токен.
        /// </summary>
        public PathResponse(Path<TPoint> path)
        {
            Token = null;
            _path = path;
        }
        
        /// <summary>
        /// Конструктор для асинхронного/многопоточного пути.
        /// </summary>
        public PathResponse(PathRequestToken<TPoint> token)
        {
            Token = token;
            _path = Path<TPoint>.EmptyPath;
        }
    }

    public static class PathResponseExtension
    {
        public static ref Path<TPoint> GetPathRef<TPoint>(this PathResponse<TPoint> response)
        {
            if (response.Token == null)
                return ref Path<TPoint>.EmptyPath;
            
            return ref response.Token.GetPathRef();
        }
    }
}