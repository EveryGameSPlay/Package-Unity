using System;

namespace Egsp.Extensions.Graphs
{
    [Obsolete("Under construction.")][Serializable]
    public class Edge<TVertex>
        where TVertex : Vertex<TVertex>
    {
        public Edge(TVertex from, TVertex to)
        {
            From = from;
            To = to;
        }
        public TVertex From { get; private set; }
        public TVertex To { get; private set; }
    }
}