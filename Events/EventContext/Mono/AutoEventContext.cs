namespace Egsp.Core
{
    public class AutoEventContext : EventContext
    {
        private void Awake()
        {
            SetupContextToEntities();
        }
    }
}