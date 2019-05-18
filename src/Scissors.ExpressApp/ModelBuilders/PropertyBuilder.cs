using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyBuilder
    {
        /// <summary>
        /// Properties the builder for.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
        /// <typeparam name="TClassType">The type of the class type.</typeparam>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public static PropertyBuilder<TPropertyType, TClassType> PropertyBuilderFor<TPropertyType, TClassType>(IMemberInfo member)
            => new PropertyBuilder<TPropertyType, TClassType>(member);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    public class PropertyBuilder<TProperty, TClass> : BuilderManager, IPropertyBuilder<TProperty, TClass>
    {
        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value>
        /// The member information.
        /// </value>
        public IMemberInfo MemberInfo { get; }
        
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName => MemberInfo.Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder{TProperty, TClass}"/> class.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        public PropertyBuilder(IMemberInfo memberInfo)
            => MemberInfo = memberInfo;

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        public IPropertyBuilder<TProperty, TClass> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            MemberInfo.AddAttribute(attribute);

            return this;
        }

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        public IPropertyBuilder<TProperty, TClass> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IPropertyBuilder<TProperty, TClass> RemoveAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            var att = MemberInfo.FindAttribute<TAttribute>();

            return RemoveAttribute(att);
        }

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IPropertyBuilder<TProperty, TClass> RemoveAttribute(Attribute attribute)
        {
            if(attribute != null && MemberInfo is XafMemberInfo)
            {
                (MemberInfo as XafMemberInfo).RemoveAttribute(attribute);
            }

            return this;
        }
    }
}
