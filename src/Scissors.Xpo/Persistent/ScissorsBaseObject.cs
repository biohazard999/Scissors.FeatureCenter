using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DevExpress.Xpo;

namespace Scissors.Xpo.Persistent
{
    [NonPersistent]
    public abstract class ScissorsBaseObject : XPBaseObject
    {
        protected ScissorsBaseObject(Session session) : base(session) { }

        protected new object GetPropertyValue([CallerMemberName]string propertyName = null)
            => base.GetPropertyValue(propertyName);

        protected new T GetPropertyValue<T>([CallerMemberName]string propertyName = null)
            => base.GetPropertyValue<T>(propertyName);

        protected bool SetPropertyValue<T>(ref T propertyValueHolder, T newValue, [CallerMemberName]string propertyName = null)
            => base.SetPropertyValue<T>(propertyName, ref propertyValueHolder, newValue);

        protected new XPCollection GetCollection([CallerMemberName] string propertyName = null)
            => base.GetCollection(propertyName);

        protected new XPCollection<T> GetCollection<T>([CallerMemberName] string propertyName = null)
            where T : class => base.GetCollection<T>(propertyName);

        protected new T GetDelayedPropertyValue<T>([CallerMemberName] string propertyName = null)
            => base.GetDelayedPropertyValue<T>(propertyName);

        protected bool SetDelayedPropertyValue<T>(T value, [CallerMemberName] string propertyName = null)
            => base.SetDelayedPropertyValue(propertyName, value);

        protected new object EvaluateAlias([CallerMemberName] string propertyName = null)
            => base.EvaluateAlias(propertyName);
    }
}
