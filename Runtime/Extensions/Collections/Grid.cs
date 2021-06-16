using System.Collections.Generic;
using UnityEngine;

using System;
using Egsp.Core;

namespace Egsp.Core
{
    public interface IGridListener<TObject>
    {
        /// <summary>
        /// Вызывается при изменении значения сетки. Передает два индекса и изменяемы объект.
        /// </summary>
        void OnChangeObject(int x,int y,TObject changed);

        /// <summary>
        /// Вызывается при удалении объекта из сетки.
        /// </summary>
        void OnRemoveObject(int x, int y,TObject removed);

        /// <summary>
        /// Вызывается при создании объекта. Объект является значением default(TObject).
        /// </summary>
        void OnCreateObject(int x, int y, TObject created);
    }

    public abstract class Grid
    {
        /// <summary>
        /// Количество ячеек по горизонтали
        /// </summary>
        public int Width { get; protected set; }
        
        /// <summary>
        ///  Количество ячеек по вертикали
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Количество ячеек.
        /// </summary>
        public int Count => Width * Height;
        
        public CellParams CellParamsValue { get; protected set; } = CellParams.One;
        
        public SceneParams SceneParamsValue { get; set; } = new SceneParams(Vector3.zero);

        public abstract void WorldToIndex(Vector3 position, out int x, out int y);

        public abstract bool InBounds(Vector3 position);

        public abstract bool InBounds(int x, int y);
        
        /// <summary>
        /// Параметры ячеек сетки.
        /// </summary>
        public struct CellParams
        {
            private static CellParams _one = new CellParams(1,1);
        
            public readonly float Width;
            public readonly float Height;

            public static CellParams One => _one;
        
            public CellParams(float width, float height)
            {
                Width = width;
                Height = height;
            }
        }
        
        /// <summary>
        /// Параметры сетки относительно сцены.
        /// </summary>
        public struct SceneParams
        {
            public readonly Vector3 Offset;

            public SceneParams(Vector3 offset)
            {
                Offset = offset;
            }
        }
    }
    
    public class Grid<TObject> : Grid
    {
        /// <summary>
        /// Массив объектов сетки
        /// </summary>
        protected List<List<TObject>> GridList2D { get; set; }
        
        public TypedBus<IGridListener<TObject>> Bus { get; protected set; } = new TypedBus<IGridListener<TObject>>();
        

        public TObject this[int x, int y]
        {
            get => GridList2D[x][y];
            set => GridList2D[x][y] = value;
        }

        public Grid(int width, int height, float cellWidth = 1, float cellHeight = 1)
        {
            this.Width = width;
            this.Height = height;

            CellParamsValue = new CellParams(cellWidth, cellHeight);

            GridList2D = new List<List<TObject>>(Width);

            for (var x = 0; x < Width; x++)
            {
                GridList2D.Add(new List<TObject>(Height));
                for (var y = 0; y < Height; y++)
                {
                    GridList2D[x].Add(default(TObject));
                }
            }

        }

        /// <param name="createTObject">Функция создания объекта</param>
        public Grid(int width, int height, Func<TObject> createTObject)
            : this(width, height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridList2D[x][y] = createTObject();
                }
            }
        }

        /// <param name="createTObject">Функция создания объекта</param>
        public Grid(int width, int height, float cellWidth, float cellHeight, Func<TObject> createTObject)
            : this(width, height, cellWidth, cellHeight)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridList2D[x][y] = createTObject();
                }
            }
        }
        
        /// <param name="createTObject">Функция создания объекта</param>
        public Grid(int width, int height, Func<int,int,TObject> createTObject)
            : this(width, height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridList2D[x][y] = createTObject(x,y);
                }
            }
        }

        /// <param name="createTObject">Функция создания объекта. Получает координаты ячейки</param>
        public Grid(int width, int height, float cellWidth, float cellHeight, Func<int, int, TObject> createTObject)
            : this(width, height, cellWidth, cellHeight)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridList2D[x][y] = createTObject(x, y);
                }
            }
        }

        /// <summary>
        /// Получение индексов из мировой позиции. 
        /// Возвращает отрицательные числа тоже
        /// </summary>
        public override void WorldToIndex(Vector3 worldPos, out int x, out int y)
        {
            worldPos -= SceneParamsValue.Offset;
            
            x = Mathf.FloorToInt(worldPos.x / CellParamsValue.Width);
            y = Mathf.FloorToInt(worldPos.y / CellParamsValue.Height);
        }
        
        /// <summary>
        /// Находятся ли индексы в пределах сетки
        /// </summary>
        public override bool InBounds(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
                return true;

            return false;
        }
        
        /// <summary>
        /// Находится ли мировая позиция в пределах сетки
        /// </summary>
        public override bool InBounds(Vector3 worldPos)
        {
            int x, y;
            WorldToIndex(worldPos,out x,out y);

            return InBounds(x, y);
        }

        
        
        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Выполняемый метод</param>
        public void PopObject(int x, int y, Action<TObject> popAction)
        {
            if (InBounds(x,y))
                popAction(GridList2D[x][y]);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Выполняемый метод</param>
        public void PopObject(Vector3 worldPos, Action<TObject> popAction)
        {
            int x, y;
            WorldToIndex(worldPos,out x,out y);
            // Проверка на отрицательный индекс внутри
            PopObject(x, y, popAction);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Передает кроме объекта индексы в сетке</param>
        public void PopObject(int x,int y, Action<int,int,TObject> popAction)
        {
            if (InBounds(x,y))
                popAction(x, y, GridList2D[x][y]);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Передает кроме объекта индексы в сетке</param>
        public void PopObject(Vector3 worldPos, Action<int,int,TObject> popAction)
        {
            int x, y;
            WorldToIndex(worldPos, out x, out y);
            // Проверка на отрицательный индекс внутри
            PopObject(x, y, popAction);
        }

        /// <summary>
        /// Получение объекта по индексу
        /// </summary>
        public TObject GetObject(int x,int y)
        {
            if (InBounds(x,y))
                return GridList2D[x][y];

            return default(TObject);
        }

        /// <summary>
        /// Получение объекта по мировым координатам
        /// </summary>
        public TObject GetObject(Vector3 worldPos)
        {
            int x, y;
            WorldToIndex(worldPos, out x, out y);
            return GetObject(x, y);
        }

        /// <summary>
        /// Устанавливает новый объект (ссылочный тип).
        /// Изменяет значение (значимый тип)
        /// </summary>
        public void SetObject(int x,int y, TObject newObject)
        {
            if (InBounds(x,y))
            {
                GridList2D[x][y] = newObject;
                GridObjectChanged(x,y,newObject);
            }
        }

        /// <summary>
        /// Устанавливает новый объект (ссылочный тип).
        /// Изменяет значение (значимый тип)
        /// </summary>
        public void SetObject(Vector3 worldPos, TObject newObject)
        {
            int x, y;
            WorldToIndex(worldPos,out x, out y);
            SetObject(x, y, newObject);
        }

        /// <summary>
        /// Проходится по каждой ячейке и вызывает метод.
        /// </summary>
        /// <param name="action">Вызываемый метод. Аргументами являются индексы в сетке</param>
        public void ForEach(Action<int,int,TObject> action)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    action(x, y,GridList2D[x][y]);
                }
            }
        }

        /// <summary>
        /// Проходится по каждой ячейке и устанавливает значение initFunction.
        /// </summary>
        /// <param name="initFunction"></param>
        public void ForEachSet(Func<int, int, TObject> initFunction)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var tObject = initFunction(x, y);

                    GridList2D[x][y] = tObject;
                }
            }
        }

        /// <summary>
        /// Изменяет размер сетки
        /// </summary>
        /// <param name="newWidth">Новая ширина</param>
        /// <param name="newHeight">Новая длина</param>
        public void Resize(int newWidth, int newHeight)
        {
            var oldWidth = Width;
            var oldHeight = Height;

            Width = newWidth;
            Height = newHeight;
            
            // Высоту менять перед шириной, т.к. при изменении высоты на не нужно создавать новые столбцы
            List<Action> onGridObjectCreatedAction = new List<Action>();
            List<Action> onGridObjectDeletedAction = new List<Action>();
            
            if (newHeight != oldHeight)
            {
                // Новая высота меньше текущей
                if (newHeight < oldHeight)
                {
                    // Проходимся по всем столбцам
                    for (var x = 0; x < oldWidth; x++)
                    {
                        // Проходимся сверху вниз у столбца
                        for (var y = oldHeight - 1; y > newHeight - 1; y--)
                        {
                            var lx = x;
                            var ly = y;
                            OnGridObjectRemoved(lx,ly, GridList2D[lx][ly]);
                            
                            GridList2D[x].RemoveAt(y);
                        }
                        GridList2D[x].Capacity = newHeight;
                    }
                }
                else // Новая высота больше текущей
                {
                    for (var x = 0; x < oldWidth; x++)
                    {
                        GridList2D[x].Capacity = newHeight;
                        for (var s = newHeight - oldHeight; s > 0; s--)
                        {
                            // Добавляем пустышки в столбец
                            GridList2D[x].Add(default(TObject));
                            
                            var lx = x;
                            var ly = newHeight - s;
                            OnGridObjectCreated(lx, ly, default(TObject));
                        }
                    }
                }
            }
           
            
            // Изменяем ширину
            if (newWidth != oldWidth)
            {
                // Новая ширина меньше текущей
                if (newWidth < oldWidth)
                {
                    // Удаляем столбцы
                    for (var x = oldWidth-1; x > newWidth-1; x--)
                    {
                        for (var y = 0; y < newHeight; y++)
                        {
                            var lx = x;
                            var ly = y;
                            // Оповещаем об удалении
                            OnGridObjectRemoved(lx, ly, GridList2D[lx][ly]);
                        }
                        // Удаляем столбец (от конца)
                        GridList2D.RemoveAt(x);
                    }
                }
                else // Новая ширина больше текущей
                {
                    // Добавляем новые столбцы
                    for (var s = newWidth - oldWidth; s > 0; s--)
                    {
                        var newColumn = new List<TObject>(newHeight);
                        GridList2D.Add(newColumn);
                        // Оповещаем о создании ячеек
                        for (var y = 0; y < newHeight; y++)
                        {
                            var lx = newWidth - s;
                            var ly = y;
                            
                            GridList2D[lx].Add(default(TObject));
                            // Debug.Log($"{lx}:{ly} lxly");
                            OnGridObjectCreated(lx, ly, default(TObject));
                        }
                    }
                }

                GridList2D.Capacity = newWidth;

            }
        }
        
        /// <summary>
        /// Вызывать при изменении объекта (ссылочный тип)
        /// </summary>
        /// <param name="value">Изменяемый объект</param>
        public void GridObjectChanged(int x,int y,TObject value)
        {
            Bus.Raise(l => l.OnChangeObject(x, y, value));
        }

        private void OnGridObjectCreated(int x,int y,TObject value)
        {
            Bus.Raise(l => l.OnCreateObject(x, y, value));
        }

        private void OnGridObjectRemoved(int x,int y,TObject value)
        {
            Bus.Raise(l => l.OnRemoveObject(x, y, value));
        }

        /// <summary>
        /// Возвращает двумерный массив элементов сетки.
        /// </summary>
        public TObject[,] ToArray2D()
        {
            var array = new TObject[Width,Height];
            
            ForEach((x,y,obj) =>
            {
                array[x, y] = obj;
            });

            return array;
        }

    }
}