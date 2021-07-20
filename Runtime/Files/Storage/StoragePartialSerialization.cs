namespace Egsp.Core
{
    public static partial class Storage
    {
        public static void SwitchSerializer(NotNull<ISerializer> serializer)
        {
            Common.Serializer = serializer.Value;
            Current.Serializer = serializer.Value;
        }
    }
}