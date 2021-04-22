using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Egsp.Core.Pathfinding.NonAlloc
{
    // Структура пути теперь не должна занимать места в куче отдельным объектом, т.к. будет находиться внутри Response
    public struct Path<TPoint>
    {
        public static Path<TPoint> EmptyPath = new Path<TPoint>();
        
        /// <summary>
        /// Проверяет необходимость смены текущей точки.
        /// </summary>
        private readonly Func<TPoint, TPoint, bool> _continueFunc;
        
        [CanBeNull] public TPoint Current { get; private set; }
        [CanBeNull] public List<TPoint> Points { get; private set; }

        /// <summary>
        /// Продолжено ли движение по пути.
        /// </summary>
        private bool _continued;

        /// <summary>
        /// Индекс текущей точки.
        /// </summary>
        private int _index;

        public Path(List<TPoint> points = null)
        {
            Points = points;
            Current = default(TPoint);
            _continued = false;
            _index = 0;
            
            _continueFunc = null;
        }

        public Path([NotNull] List<TPoint> points, Func<TPoint, TPoint, bool> continueFunc)
        {
            if(points == null)
                throw new EmptyPathException();
            
            Points = points;
            Current = default(TPoint);
            _continued = false;
            _index = 0;

            _continueFunc = continueFunc;

            // Изначально указатель никуда не указывает и его нужно сдвинуть на первый элемент.
            MovePointer();
        }
    

        /// <summary>
        /// Проверка на возможность продолжения перемещения.
        /// </summary>
        public bool Continue(TPoint origin)
        {
            if (Points == null)
                return false;
            
            // Если нужно двигаться к следующей точке.
            if (_continueFunc(origin, Current))
            {
                MovePointer();
            }

            // Продолжен ли путь.
            return _continued;
        }

        /// <summary>
        /// Передвигаем указатель на следующий элемент. 
        /// </summary>
        private void MovePointer()
        {
            // Если есть элементы в коллекции точек.
            if (_index < Points.Count)
            {
                Current = Points[_index++];
                _continued = true;
            }
            else
            {
                _continued = false;
            }
        }
    }

    public class EmptyPathException : Exception
    {
        public EmptyPathException()
            : base("Path is empty! Try using EmptyPath<TPoint> instead of Path<TPoint>")
        {
        }
    }
}