using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    public static class PropertyBuilder
    {
        public static PropertyBuilder<TPropertyType, TClassType> PropertyBuilderFor<TPropertyType, TClassType>(IMemberInfo member)
            => new PropertyBuilder<TPropertyType, TClassType>(member);
    }

    public interface IPropertyBuilder<T, TType>
    {
        IPropertyBuilder<T, TType> RemoveAttribute<TAttribute>() where TAttribute : Attribute;
        IPropertyBuilder<T, TType> RemoveAttribute(Attribute attribute);
        IPropertyBuilder<T, TType> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new();
        IPropertyBuilder<T, TType> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute;

        string PropertyName { get; }
        IMemberInfo MemberInfo { get; }
    }

    public class PropertyBuilder<TProperty, TClass> : BuilderManager, IPropertyBuilder<TProperty, TClass>
    {
        public IMemberInfo MemberInfo { get; }
        public string PropertyName => MemberInfo.Name;

        public PropertyBuilder(IMemberInfo memberInfo)
            => MemberInfo = memberInfo;

        public IPropertyBuilder<TProperty, TClass> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            MemberInfo.AddAttribute(attribute);

            return this;
        }

        public IPropertyBuilder<TProperty, TClass> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IPropertyBuilder<TProperty, TClass> RemoveAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            var att = MemberInfo.FindAttribute<TAttribute>();

            return RemoveAttribute(att);
        }

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
