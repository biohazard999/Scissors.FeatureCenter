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

    public class PropertyBuilder<T, TType> : BuilderManager
    {
        public readonly IMemberInfo MemberInfo;
        public string PropertyName => MemberInfo.Name;

        public PropertyBuilder(IMemberInfo memberInfo)
            => MemberInfo = memberInfo;

        public PropertyBuilder<T, TType> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            MemberInfo.AddAttribute(attribute);

            return this;
        }

        public PropertyBuilder<T, TType> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyBuilder<T, TType> RemoveAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            var att = MemberInfo.FindAttribute<TAttribute>();

            return RemoveAttribute(att);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public PropertyBuilder<T, TType> RemoveAttribute(Attribute attribute)
        {
            if(attribute != null && MemberInfo is XafMemberInfo)
            {
                (MemberInfo as XafMemberInfo).RemoveAttribute(attribute);
            }

            return this;
        }

    }
}
