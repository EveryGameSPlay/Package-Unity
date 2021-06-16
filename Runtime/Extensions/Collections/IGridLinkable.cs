namespace Egsp.Core
{
    /// <summary>
    /// Объект, который будет использоваться LinkedGrid.
    /// </summary>
    public interface IGridLinkable<TLinkable> where TLinkable : class
    {
        TLinkable Up { get; set; }
        TLinkable Right { get; set; }
        TLinkable Down { get; set; }
        TLinkable Left { get; set; }
    }
}