using System;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TType">The type of the type.</typeparam>
    public interface IPropertyBuilder<T, TType>
    {
        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        IPropertyBuilder<T, TType> RemoveAttribute<TAttribute>() where TAttribute : Attribute;
        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        IPropertyBuilder<T, TType> RemoveAttribute(Attribute attribute);
        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        IPropertyBuilder<T, TType> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new();
        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        IPropertyBuilder<T, TType> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute;

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string PropertyName { get; }
        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value>
        /// The member information.
        /// </value>
        IMemberInfo MemberInfo { get; }
    }
}
