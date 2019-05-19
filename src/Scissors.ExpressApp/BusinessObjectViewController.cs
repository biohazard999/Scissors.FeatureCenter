using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="BusinessObjectViewController{ObjectView, TObjectType}" />
    public abstract class BusinessObjectViewController<TObjectType> : BusinessObjectViewController<ObjectView, TObjectType>
        where TObjectType : class
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="BusinessObjectViewController{ObjectView, TObjectType}" />
    public abstract class BusinessObjectViewController<TView, TObjectType> : ViewController<TView>
        where TObjectType : class
        where TView : ObjectView
    {
        /// <summary>
        /// Occurs when [current object changing].
        /// </summary>
        public event EventHandler<CurrentObjectChangingEventArgs<TObjectType>> CurrentObjectChanging;
        /// <summary>
        /// Occurs when [current object changed].
        /// </summary>
        public event EventHandler<CurrentObjectChangedEventArgs<TObjectType>> CurrentObjectChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectViewController{TView, TObjectType}"/> class.
        /// </summary>
        protected BusinessObjectViewController()
            => TargetObjectType = typeof(TObjectType);

        /// <summary>
        /// Called when [activated].
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();

            UnsubscribeFromViewEvents();
            SubscribeToViewEvents();
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        protected override void OnDeactivated()
        {
            UnsubscribeFromViewEvents();

            base.OnDeactivated();
        }

        /// <summary>
        /// Subscribes to view events.
        /// </summary>
        protected virtual void SubscribeToViewEvents()
        {
            if(View != null)
            {
                View.QueryCanChangeCurrentObject -= View_QueryCanChangeCurrentObject;
                View.QueryCanChangeCurrentObject += View_QueryCanChangeCurrentObject;
                View.CurrentObjectChanged -= View_CurrentObjectChanged;
                View.CurrentObjectChanged += View_CurrentObjectChanged;
            }
        }

        /// <summary>
        /// Unsubscribe's from view events.
        /// </summary>
        protected virtual void UnsubscribeFromViewEvents()
        {
            if(View != null)
            {
                View.QueryCanChangeCurrentObject -= View_QueryCanChangeCurrentObject;
                View.CurrentObjectChanged -= View_CurrentObjectChanged;
            }
        }

        /// <summary>
        /// Handles the QueryCanChangeCurrentObject event of the View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        void View_QueryCanChangeCurrentObject(object sender, CancelEventArgs e)
        {
            var args = new CurrentObjectChangingEventArgs<TObjectType>(e.Cancel, CurrentObject);
            OnCurrentObjectChanging((TView)sender, args);
            e.Cancel = args.Cancel;
        }

        /// <summary>
        /// Called when [current object changing].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="e">The <see cref="CurrentObjectChangingEventArgs{TObjectType}"/> instance containing the event data.</param>
        protected virtual void OnCurrentObjectChanging(TView view, CurrentObjectChangingEventArgs<TObjectType> e)
            => CurrentObjectChanging?.Invoke(this, e);

        /// <summary>
        /// Handles the CurrentObjectChanged event of the View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void View_CurrentObjectChanged(object sender, EventArgs e)
            => OnCurrentObjectChanged((TView)sender, new CurrentObjectChangedEventArgs<TObjectType>(CurrentObject));

        /// <summary>
        /// Called when [current object changed].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The <see cref="CurrentObjectChangedEventArgs{TObjectType}"/> instance containing the event data.</param>
        protected virtual void OnCurrentObjectChanged(TView view, CurrentObjectChangedEventArgs<TObjectType> args)
            => CurrentObjectChanged?.Invoke(this, args);

        /// <summary>
        /// Gets the current object.
        /// </summary>
        /// <value>
        /// The current object.
        /// </value>
        public TObjectType CurrentObject
            => View?.CurrentObject as TObjectType;

        /// <summary>
        /// Gets the selected objects.
        /// </summary>
        /// <value>
        /// The selected objects.
        /// </value>
        public IEnumerable<TObjectType> SelectedObjects => View?.SelectedObjects?.OfType<TObjectType>() ?? new TObjectType[0] { };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="System.EventArgs" />
    public class CurrentObjectChangedEventArgs<TObjectType> : EventArgs
        where TObjectType : class
    {
        /// <summary>
        /// The current object
        /// </summary>
        public readonly TObjectType CurrentObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentObjectChangedEventArgs{TObjectType}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public CurrentObjectChangedEventArgs(TObjectType obj)
            => CurrentObject = obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <seealso cref="System.EventArgs" />
    public class CurrentObjectChangingEventArgs<TObjectType> : EventArgs
        where TObjectType : class
    {
        /// <summary>
        /// The current object
        /// </summary>
        public readonly TObjectType CurrentObject;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CurrentObjectChangingEventArgs{TObjectType}"/> is cancel.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentObjectChangingEventArgs{TObjectType}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public CurrentObjectChangingEventArgs(TObjectType obj) : this(false, obj)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentObjectChangingEventArgs{TObjectType}"/> class.
        /// </summary>
        /// <param name="cancel">if set to <c>true</c> [cancel].</param>
        /// <param name="obj">The object.</param>
        public CurrentObjectChangingEventArgs(bool cancel, TObjectType obj)
        {
            Cancel = cancel;
            CurrentObject = obj;
        }
    }
}
