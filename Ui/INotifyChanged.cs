using System;

namespace Egsp.Core.Ui
{
    public interface INotifyChanged<TObject>
    {
        event Action<TObject> NotifyChanged;
    }
}