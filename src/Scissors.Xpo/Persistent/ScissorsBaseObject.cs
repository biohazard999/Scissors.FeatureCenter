using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DevExpress.Xpo;

namespace Scissors.Xpo.Persistent
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.Xpo.XPBaseObject" />
    [NonPersistent]
    public abstract class ScissorsBaseObject : XPBaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorsBaseObject"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        protected ScissorsBaseObject(Session session) : base(session) { }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new object GetPropertyValue([CallerMemberName]string propertyName = null)
            => base.GetPropertyValue(propertyName);

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new T GetPropertyValue<T>([CallerMemberName]string propertyName = null)
            => base.GetPropertyValue<T>(propertyName);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyValueHolder">The property value holder.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected bool SetPropertyValue<T>(ref T propertyValueHolder, T newValue, [CallerMemberName]string propertyName = null)
            => base.SetPropertyValue<T>(propertyName, ref propertyValueHolder, newValue);

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new XPCollection GetCollection([CallerMemberName] string propertyName = null)
            => base.GetCollection(propertyName);

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new XPCollection<T> GetCollection<T>([CallerMemberName] string propertyName = null)
            where T : class => base.GetCollection<T>(propertyName);

        /// <summary>
        /// Gets the delayed property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new T GetDelayedPropertyValue<T>([CallerMemberName] string propertyName = null)
            => base.GetDelayedPropertyValue<T>(propertyName);

        /// <summary>
        /// Sets the delayed property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected bool SetDelayedPropertyValue<T>(T value, [CallerMemberName] string propertyName = null)
            => base.SetDelayedPropertyValue(propertyName, value);

        /// <summary>
        /// Evaluates the alias.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected new object EvaluateAlias([CallerMemberName] string propertyName = null)
            => base.EvaluateAlias(propertyName);
    }
}
