namespace Egsp.Core.Pathfinding.NonAlloc
{
    public interface IPathResponse<TPoint>
    {
        bool Ready { get; }
        
        Path<TPoint> Path { get; }
    }
}