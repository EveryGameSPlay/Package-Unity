using System;
using System.Collections.Generic;

namespace Egsp.Core
{
    public class EventBus : IEventBus
    {
        public Dictionary<Type, List<WeakReference>> Subscribers { get; protected set; }
        
        public EventBus()
        {
            Subscribers = new Dictionary<Type, List<WeakReference>>();
        }
        
        public virtual void Subscribe<TSubscriberType>(TSubscriberType subscriber)
            where TSubscriberType : class
        {
            var subscriberType = typeof(TSubscriberType);

            if (Subscribers.ContainsKey(subscriberType))
            {
                // Добавляем подписчика в существующий список.
                var list = Subscribers[subscriberType];

                if (list.Exists(x => x.Target == subscriber) == false)
                    list.Add(new WeakReference(subscriber));
            }
            else
            {
                // Создаем новый список типов подписчиков.
                var list = new List<WeakReference>();
                list.Add(new WeakReference(subscriber));
                
                Subscribers.Add(subscriberType, list);
            }
        }

        public virtual void Raise<TSubscriberType>(Action<TSubscriberType> raiserAction)
            where TSubscriberType : class
        {
            var subscriberType = typeof(TSubscriberType);

            if (Subscribers.ContainsKey(subscriberType))
            {
                var list = Subscribers[subscriberType];

                // Проходимся по списку и вызываем всех живущих подписчиков.
                for (var i = 0; i < list.Count; i++)
                {
                    var subscriber = list[i].Target as TSubscriberType;

                    if (subscriber != null)
                        raiserAction(subscriber);
                }
                
                // Очистка листа от мертвых ссылок.
                list.RemoveAll(x => x.Target == null);
            }
        }
    }
}