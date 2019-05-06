using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp
{
    public abstract class BusinessObjectViewController<TObjectType> : BusinessObjectViewController<ObjectView, TObjectType>
        where TObjectType : class
    {
    }

    public abstract class BusinessObjectViewController<TView, TObjectType> : ViewController<TView>
        where TObjectType : class
        where TView : ObjectView
    {
        public event EventHandler<CurrentObjectChangingEventArgs<TObjectType>> CurrentObjectChanging;
        public event EventHandler<CurrentObjectChangedEventArgs<TObjectType>> CurrentObjectChanged;

        protected BusinessObjectViewController()
            => TargetObjectType = typeof(TObjectType);

        protected override void OnActivated()
        {
            base.OnActivated();

            UnsubscribeFromViewEvents();
            SubscribeToViewEvents();
        }

        protected override void OnDeactivated()
        {
            UnsubscribeFromViewEvents();

            base.OnDeactivated();
        }

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

        protected virtual void UnsubscribeFromViewEvents()
        {
            if(View != null)
            {
                View.QueryCanChangeCurrentObject -= View_QueryCanChangeCurrentObject;
                View.CurrentObjectChanged -= View_CurrentObjectChanged;
            }
        }

        void View_QueryCanChangeCurrentObject(object sender, CancelEventArgs e)
        {
            var args = new CurrentObjectChangingEventArgs<TObjectType>(e.Cancel, CurrentObject);
            OnCurrentObjectChanging((TView)sender, args);
            e.Cancel = args.Cancel;
        }

        protected virtual void OnCurrentObjectChanging(TView view, CurrentObjectChangingEventArgs<TObjectType> e)
            => CurrentObjectChanging?.Invoke(this, e);

        void View_CurrentObjectChanged(object sender, EventArgs e)
            => OnCurrentObjectChanged((TView)sender, new CurrentObjectChangedEventArgs<TObjectType>(CurrentObject));

        protected virtual void OnCurrentObjectChanged(TView view, CurrentObjectChangedEventArgs<TObjectType> args)
            => CurrentObjectChanged?.Invoke(this, args);

        public TObjectType CurrentObject
            => View?.CurrentObject as TObjectType;

        public IEnumerable<TObjectType> SelectedObjects
        {
            get
            {
                foreach(var item in View?.SelectedObjects?.OfType<TObjectType>())
                {
                    yield return item;
                }
            }
        }
    }

    public class CurrentObjectChangedEventArgs<TObjectType> : EventArgs
        where TObjectType : class
    {
        public readonly TObjectType CurrentObject;

        public CurrentObjectChangedEventArgs(TObjectType obj)
            => CurrentObject = obj;
    }

    public class CurrentObjectChangingEventArgs<TObjectType> : EventArgs
        where TObjectType : class
    {
        public readonly TObjectType CurrentObject;

        public bool Cancel { get; set; }

        public CurrentObjectChangingEventArgs(TObjectType obj) : this(false, obj)
        {
        }

        public CurrentObjectChangingEventArgs(bool cancel, TObjectType obj)
        {
            Cancel = cancel;
            CurrentObject = obj;
        }
    }
}
