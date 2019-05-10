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

    public interface IModelBuilder<T>
    {
        IModelBuilder<T> ConfigureAttribute<TAttr>(Action<TAttr> action, Func<TAttr, bool> predicate = null) where TAttr : Attribute;
        TAttr FindAttribute<TAttr>(Func<TAttr, bool> predicate = null) where TAttr : Attribute;
        PropertyBuilder<TProp, T> For<TProp>(Expression<Func<T, TProp>> property);
        string NestedListViewId<TRet>(Expression<Func<T, TRet>> expr) where TRet : IEnumerable;
        IModelBuilder<T> RemoveAttribute<TAttr>(Func<TAttr, bool> predicate = null) where TAttr : Attribute;
        IModelBuilder<T> RemoveAttribute(Type attributeType);
        IModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new();
        IModelBuilder<T> WithAttribute(Attribute attribute);
        IModelBuilder<T> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute;

        string DefaultDetailView { get; }
        string DefaultListView { get; }
        string DefaultLookupListView { get; }

        ExpressionHelper<T> Exp { get; }
        ITypeInfo TypeInfo { get; }
        Type TargetType { get; }
    }

    public class ModelBuilder<T> : BuilderManager, ITypeInfoProvider, IModelBuilder<T>
    {
        public ModelBuilder(ITypeInfo typeInfo) => TypeInfo = typeInfo;

        public ITypeInfo TypeInfo { get; }
        public Type TargetType { get; } = typeof(T);
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ExpressionHelper<T> Exp { get; } = new ExpressionHelper<T>();

        public virtual string DefaultDetailView => ModelNodeIdHelper.GetDetailViewId(typeof(T));
        public virtual string DefaultListView => ModelNodeIdHelper.GetListViewId(typeof(T));
        public virtual string DefaultLookupListView => ModelNodeIdHelper.GetLookupListViewId(typeof(T));
        public virtual string NestedListViewId<TRet>(Expression<Func<T, TRet>> expr)
            where TRet : IEnumerable
                => ModelNodeIdHelper.GetNestedListViewId(typeof(T), Exp.Property(expr));

        public IModelBuilder<T> WithAttribute(Attribute attribute)
        {
            TypeInfo.AddAttribute(attribute);
            return this;
        }

        public IModelBuilder<T> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            return WithAttribute((Attribute)attribute);
        }

        public IModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IModelBuilder<T> RemoveAttribute(Type attributeType)
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
        public IModelBuilder<T> RemoveAttribute<TAttr>(Func<TAttr, bool> predicate = null)
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
        public IModelBuilder<T> ConfigureAttribute<TAttr>(Action<TAttr> action, Func<TAttr, bool> predicate = null)
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
