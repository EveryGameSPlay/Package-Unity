using System;
using System.Collections.Generic;

namespace Egsp.Extensions.Collections
{
    // TODO: Вместо использования интерфейса объектов сетки, лучше использовать обертки по типу Node в LinkedList.
    
    public class LinkedGrid<TLinkable> : Grid<TLinkable> where TLinkable : class, IGridLinkable<TLinkable>
    {
        protected LinkedGrid(int width, int height)
            : base(width, height)
        {
            
        }
        
        public LinkedGrid(int width, int height, Func<TLinkable> createTObject)
            : base(width,height)
        {
            GridList2D = new List<List<TLinkable>>(Width);
            for (var x = 0; x < Width; x++)
            {
                GridList2D[x] = new List<TLinkable>(Height);

                for (var y = 0; y < Height; y++)
                {
                    var linkable = createTObject();

                    if (x > 0)
                    {
                        var previousPoint = GridList2D[x - 1][y];
                        linkable.Left = previousPoint;
                        previousPoint.Right = linkable;
                    }

                    if (y > 0)
                    {
                        var previousPoint = GridList2D[x][y - 1];
                        linkable.Down = previousPoint;
                        previousPoint.Up = linkable;
                    }

                    GridList2D[x][y] = linkable;
                }
            }
        }
        
        public LinkedGrid(int width, int height, Func<int, int, TLinkable> createTObject) 
            : base(width,height)
        {
            GridList2D = new List<List<TLinkable>>(Width);
            for (var x = 0; x < Width; x++)
            {
                GridList2D[x] = new List<TLinkable>(Height);

                for (var y = 0; y < Height; y++)
                {
                    var linkable = createTObject(x,y);

                    if (x > 0)
                    {
                        var previousPoint = GridList2D[x - 1][y];
                        linkable.Left = previousPoint;
                        previousPoint.Right = linkable;
                    }

                    if (y > 0)
                    {
                        var previousPoint = GridList2D[x][y - 1];
                        linkable.Down = previousPoint;
                        previousPoint.Up = linkable;
                    }

                    GridList2D[x][y] = linkable;
                }
            }
        }

        public LinkedGrid(int width, int height, float cellWidth, float cellHeight,
            Func<TLinkable> createTObject) 
            : base(width, height, cellWidth, cellHeight)
        {
            for (var x = 0; x < Width; x++)
            {
                GridList2D[x] = new List<TLinkable>(Height);

                for (var y = 0; y < Height; y++)
                {
                    var linkable = createTObject();

                    if (x > 0)
                    {
                        var previousPoint = GridList2D[x - 1][y];
                        linkable.Left = previousPoint;
                        previousPoint.Right = linkable;
                    }

                    if (y > 0)
                    {
                        var previousPoint = GridList2D[x][y - 1];
                        linkable.Down = previousPoint;
                        previousPoint.Up = linkable;
                    }

                    GridList2D[x][y] = linkable;
                }
            }
        }

        public LinkedGrid(int width, int height, float cellWidth, float cellHeight,
            Func<int, int, TLinkable> createTObject)
            : base(width, height, cellWidth, cellHeight)
        {
            for (var x = 0; x < Width; x++)
            {
                GridList2D[x] = new List<TLinkable>(Height);

                for (var y = 0; y < Height; y++)
                {
                    var linkable = createTObject(x,y);

                    if (x > 0)
                    {
                        var previousPoint = GridList2D[x - 1][y];
                        linkable.Left = previousPoint;
                        previousPoint.Right = linkable;
                    }

                    if (y > 0)
                    {
                        var previousPoint = GridList2D[x][y - 1];
                        linkable.Down = previousPoint;
                        previousPoint.Up = linkable;
                    }

                    GridList2D[x][y] = linkable;
                }
            }
        }


        /// <summary>
        /// Создает новую связную сетку на основе обычной
        /// </summary>
        public static LinkedGrid<TLinkable> ToLinkedGrid<TLinkable>(Grid<TLinkable> grid)
            where TLinkable : class, IGridLinkable<TLinkable>
        {
            var linkedGrid = new LinkedGrid<TLinkable>(grid.Width, grid.Height);
            
            linkedGrid.Cell = grid.Cell;

            linkedGrid.GridList2D = new List<List<TLinkable>>(linkedGrid.Width);
            
            for (var x = 0; x < linkedGrid.Width; x++)
            {
                linkedGrid.GridList2D.Add(new List<TLinkable>(linkedGrid.Height));

                for (var y = 0; y < linkedGrid.Height; y++)
                {
                    var linkable = grid[x, y];

                    if (x > 0)
                    {
                        var previousPoint = linkedGrid.GridList2D[x - 1][y];
                        linkable.Left = previousPoint;
                        previousPoint.Right = linkable;
                    }

                    if (y > 0)
                    {
                        var previousPoint = linkedGrid.GridList2D[x][y - 1];
                        linkable.Down = previousPoint;
                        previousPoint.Up = linkable;
                    }

                    linkedGrid.GridList2D[x].Add(linkable);
                }
            }

            return linkedGrid;
        }
    }
}