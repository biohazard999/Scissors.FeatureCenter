using DevExpress.ExpressApp.DC;
using Scissors.Data;
using System;
using System.Collections;
using System.Linq.Expressions;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelBuilder<T>
    {
        /// <summary>
        /// Configures the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IModelBuilder<T> ConfigureAttribute<TAttr>(Action<TAttr> action, Func<TAttr, bool> predicate = null) where TAttr : Attribute;

        /// <summary>
        /// Finds the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        TAttr FindAttribute<TAttr>(Func<TAttr, bool> predicate = null) where TAttr : Attribute;

        /// <summary>
        /// Fors the specified property.
        /// </summary>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        PropertyBuilder<TProp, T> For<TProp>(Expression<Func<T, TProp>> property);

        /// <summary>
        /// Nesteds the ListView identifier.
        /// </summary>
        /// <typeparam name="TRet">The type of the ret.</typeparam>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        string NestedListViewId<TRet>(Expression<Func<T, TRet>> expr) where TRet : IEnumerable;

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IModelBuilder<T> RemoveAttribute<TAttr>(Func<TAttr, bool> predicate = null) where TAttr : Attribute;

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        IModelBuilder<T> RemoveAttribute(Type attributeType);

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        IModelBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configureAction = null) where TAttribute : Attribute, new();

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        IModelBuilder<T> WithAttribute(Attribute attribute);

        /// <summary>
        /// Withes the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        IModelBuilder<T> WithAttribute<TAttribute>(TAttribute attribute, Action<TAttribute> configureAction = null) where TAttribute : Attribute;

        /// <summary>
        /// Gets the default detail view.
        /// </summary>
        /// <value>
        /// The default detail view.
        /// </value>
        string DefaultDetailView { get; }

        /// <summary>
        /// Gets the default ListView.
        /// </summary>
        /// <value>
        /// The default ListView.
        /// </value>
        string DefaultListView { get; }

        /// <summary>
        /// Gets the default lookup ListView.
        /// </summary>
        /// <value>
        /// The default lookup ListView.
        /// </value>
        string DefaultLookupListView { get; }


        /// <summary>
        /// Gets the exp.
        /// </summary>
        /// <value>
        /// The exp.
        /// </value>
        ExpressionHelper<T> Exp { get; }

        /// <summary>
        /// Gets the type information.
        /// </summary>
        /// <value>
        /// The type information.
        /// </value>
        ITypeInfo TypeInfo { get; }

        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        Type TargetType { get; }
    }
}
