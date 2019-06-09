using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionContainers;
using DevExpress.Persistent.Base;

namespace Scissors.ExpressApp.Console.Templates.ActionContainers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.View" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.IActionContainer" />
    public class NavBarActionContainer : Terminal.Gui.View, IActionContainer
    {
        private INavigationControl control;
        private readonly List<ActionBase> actions;
        private SingleChoiceAction singleChoiceAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavBarActionContainer"/> class.
        /// </summary>
        public NavBarActionContainer() : base()
        {
            ContainerId = NavigationHelper.DefaultContainerId;
            actions = new List<ActionBase>();
        }

        /// <summary>
        /// Specifies an Action Container's identifier.
        /// </summary>
        /// <value>
        /// A string holding the Action Container's identifier.
        /// </value>
        public string ContainerId { get; set; }
        /// <summary>
        /// Adds a specified Action to an Action Container's <see cref="P:DevExpress.ExpressApp.Templates.IActionContainer.Actions" /> collection and creates its control.
        /// </summary>
        /// <param name="action">An <see cref="T:DevExpress.ExpressApp.Actions.ActionBase" /> object that represents the Action to be registered within the current Action Container.</param>
        public void Register(ActionBase action)
        {
            if(!(action is SingleChoiceAction choiceAction))
            {
                Tracing.Tracer.LogWarning("Cannot register action (Id = {0}) in the NavigationActionContainer: the navigation action should be of the SingleChoiceAction type.", action.Id);
                return;
            }

            if(control is IDisposable disposable)
            {
                disposable.Dispose();
            }
            RemoveAll();
            control = CreateNavigationControl();
            OnNavigationControlCreated();
            control.SetNavigationActionItems(choiceAction.Items, choiceAction);
            Add((Terminal.Gui.View)control);
            singleChoiceAction = choiceAction;
            singleChoiceAction.RaiseCustomizeControl(control);
        }

        /// <summary>
        /// Provides access to a collection of Actions which are displayed by an Action Container.
        /// </summary>
        /// <value>
        /// A ReadOnlyCollection&lt;<see cref="T:DevExpress.ExpressApp.Actions.ActionBase" />&gt; collection containing Actions of the current Action Container.
        /// </value>
        public ReadOnlyCollection<ActionBase> Actions => actions.AsReadOnly();

        /// <summary>
        /// Creates the navigation control.
        /// </summary>
        /// <returns></returns>
        protected virtual INavigationControl CreateNavigationControl() => new NavBarConsoleControl();

        /// <summary>
        /// Called when [navigation control created].
        /// </summary>
        protected virtual void OnNavigationControlCreated() => NavigationControlCreated?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when [navigation control created].
        /// </summary>
        public event EventHandler<EventArgs> NavigationControlCreated;

        /// <summary>
        /// Prevents an entity implementing the <see cref="T:DevExpress.ExpressApp.ISupportUpdate" /> interface from being updated until the <see cref="M:DevExpress.ExpressApp.ISupportUpdate.EndUpdate" /> method is called.
        /// </summary>
        public void BeginUpdate() { }

        /// <summary>
        /// Unlocks an entity implementing the <see cref="T:DevExpress.ExpressApp.ISupportUpdate" /> interface after a call to the <see cref="M:DevExpress.ExpressApp.ISupportUpdate.BeginUpdate" /> method, and causes an immediate update.
        /// </summary>
        public void EndUpdate()
            => Terminal.Gui.Application.Refresh();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }
    }
}
