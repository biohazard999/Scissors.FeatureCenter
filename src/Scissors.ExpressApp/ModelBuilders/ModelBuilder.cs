using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.NodeGenerators;
using Scissors.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Scissors.ExpressApp.ModelBuilders
{
    public static class ModelBuilder
    {
        public static ITypeInfo FindTypeInfo<T>(this ITypesInfo typesInfo)
            => typesInfo.FindTypeInfo(typeof(T));

        public static ModelBuilder<T> Create<T>(ITypesInfo typesInfo)
            => new ModelBuilder<T>(typesInfo.FindTypeInfo<T>());
    }

    public class ModelBuilder<T> : BuilderManager, ITypeInfoProvider
    {
        public ModelBuilder(ITypeInfo typeInfo) => TypeInfo = typeInfo;

        public ITypeInfo TypeInfo { get; }
        public readonly Type TargetType = typeof(T);
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public readonly ExpressionHelper<T> Exp = new ExpressionHelper<T>();

        public virtual string DefaultDetailView => ModelNodeIdHelper.GetDetailViewId(typeof(T));
        public virtual string DefaultListView => ModelNodeIdHelper.GetListViewId(typeof(T));
        public virtual string DefaultLookupListView => ModelNodeIdHelper.GetLookupListViewId(typeof(T));
        public virtual string NestedListViewId<TRet>(Expression<Func<T, TRet>> expr)
            where TRet : IEnumerable
                => ModelNodeIdHelper.GetNestedListViewId(typeof(T), Exp.Property(expr));

        public ModelBuilder<T> WithAttribute(Attribute attribute)
        {
            TypeInfo.AddAttribute(attribute);
            return this;
        }

        public ModelBuilder<T> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            return WithAttribute((Attribute)attribute);
        }

        public ModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ModelBuilder<T> RemoveAttribute(Type attributeType)
        {
            if(TypeInfo is TypeInfo)
            {
                var att = (TypeInfo as TypeInfo).FindAttribute(attributeType);

                if(att != null)
                {
                    (TypeInfo as TypeInfo).RemoveAttribute(att);
                }
            }

            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ModelBuilder<T> RemoveAttribute<TAttr>(Func<TAttr, bool> predicate = null)
            where TAttr : Attribute
        {
            var attr = FindAttribute(predicate);

            if(attr != null && TypeInfo is TypeInfo)
            {
                (TypeInfo as TypeInfo).RemoveAttribute(attr);
            }

            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TAttr FindAttribute<TAttr>(Func<TAttr, bool> predicate = null)
            where TAttr : Attribute
                => TypeInfo.Attributes.OfType<TAttr>().FirstOrDefault(predicate ?? (attr => true));

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ModelBuilder<T> ConfigureAttribute<TAttr>(Action<TAttr> action, Func<TAttr, bool> predicate = null)
            where TAttr : Attribute
        {
            var attr = FindAttribute(predicate);

            if(attr != null)
            {
                action(attr);
            }

            return this;
        }

        public PropertyBuilder<TProp, T> For<TProp>(Expression<Func<T, TProp>> property)
        {
            var builder = PropertyBuilder.PropertyBuilderFor<TProp, T>(TypeInfo.FindMember(Exp.Property(property)));
            
            AddBuilder(builder);

            return builder;
        }
    }
}
