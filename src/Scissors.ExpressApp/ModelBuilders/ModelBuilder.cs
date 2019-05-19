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
    /// <summary>
    /// 
    /// </summary>
    public static class ModelBuilder
    {
        /// <summary>
        /// Finds the type information.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typesInfo">The types information.</param>
        /// <returns></returns>
        public static ITypeInfo FindTypeInfo<T>(this ITypesInfo typesInfo)
            => typesInfo.FindTypeInfo(typeof(T));

        /// <summary>
        /// Creates the specified types information.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typesInfo">The types information.</param>
        /// <returns></returns>
        public static ModelBuilder<T> Create<T>(ITypesInfo typesInfo)
            => new ModelBuilder<T>(typesInfo.FindTypeInfo<T>());

        /// <summary>
        /// Creates the specified types information.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="typesInfo">The types information.</param>
        /// <returns></returns>
        public static TBuilder Create<TBuilder, T>(ITypesInfo typesInfo)
            where TBuilder : IModelBuilder<T>
           => (TBuilder)Activator.CreateInstance(typeof(TBuilder), typesInfo.FindTypeInfo<T>());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelBuilder<T> : BuilderManager, ITypeInfoProvider, IModelBuilder<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBuilder{T}"/> class.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        public ModelBuilder(ITypeInfo typeInfo) => TypeInfo = typeInfo;

        /// <summary>
        /// Gets the type information.
        /// </summary>
        /// <value>
        /// The type information.
        /// </value>
        public ITypeInfo TypeInfo { get; }
        
        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType { get; } = typeof(T);

        /// <summary>
        /// Gets the exp.
        /// </summary>
        /// <value>
        /// The exp.
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ExpressionHelper<T> Exp { get; } = new ExpressionHelper<T>();

        /// <summary>
        /// Gets the default detail view.
        /// </summary>
        /// <value>
        /// The default detail view.
        /// </value>
        public virtual string DefaultDetailView => ModelNodeIdHelper.GetDetailViewId(typeof(T));
        
        /// <summary>
        /// Gets the default ListView.
        /// </summary>
        /// <value>
        /// The default ListView.
        /// </value>
        public virtual string DefaultListView => ModelNodeIdHelper.GetListViewId(typeof(T));

        /// <summary>
        /// Gets the default lookup ListView.
        /// </summary>
        /// <value>
        /// The default lookup ListView.
        /// </value>
        public virtual string DefaultLookupListView => ModelNodeIdHelper.GetLookupListViewId(typeof(T));

        /// <summary>
        /// Nesteds the ListView identifier.
        /// </summary>
        /// <typeparam name="TRet">The type of the ret.</typeparam>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        public virtual string NestedListViewId<TRet>(Expression<Func<T, TRet>> expr)
            where TRet : IEnumerable
                => ModelNodeIdHelper.GetNestedListViewId(typeof(T), Exp.Property(expr));

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public IModelBuilder<T> WithAttribute(Attribute attribute)
        {
            TypeInfo.AddAttribute(attribute);
            return this;
        }

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        public IModelBuilder<T> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute
        {
            configureAction?.Invoke(attribute);
            return WithAttribute((Attribute)attribute);
        }

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        public IModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new()
            => WithAttribute(new TAttribute(), configureAction);

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TAttr FindAttribute<TAttr>(Func<TAttr, bool> predicate = null)
            where TAttr : Attribute
                => TypeInfo.Attributes.OfType<TAttr>().FirstOrDefault(predicate ?? (attr => true));

        /// <summary>
        /// Configures the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Fors the specified property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public PropertyBuilder<TProp, T> For<TProp>(Expression<Func<T, TProp>> property)
        {
            var builder = PropertyBuilder.PropertyBuilderFor<TProp, T>(TypeInfo.FindMember(Exp.Property(property)));

            AddBuilder(builder);

            return builder;
        }
    }
}
