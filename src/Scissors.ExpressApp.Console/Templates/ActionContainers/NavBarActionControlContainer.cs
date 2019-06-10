using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionContainers;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.Persistent.Base;

namespace Scissors.ExpressApp.Console.Templates.ActionContainers
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.IActionControlContainer" />
    public class NavBarActionControlContainer : IActionControlContainer
    {
        private IDictionary<string, IActionControl> actionControlByActionId = new Dictionary<string, IActionControl>();

        /// <summary>
        /// Gets or sets the parent view.
        /// </summary>
        /// <value>
        /// The parent view.
        /// </value>
        public Terminal.Gui.View ParentView { get; set; }

        /// <summary>
        /// Gets or sets the action category.
        /// </summary>
        /// <value>
        /// The action category.
        /// </value>
        public string ActionCategory { get; set; } = NavigationHelper.DefaultContainerId;

        /// <summary>
        /// Adds the parametrized action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <returns></returns>
        public IParametrizedActionControl AddParametrizedActionControl(string actionId, Type valueType) => (IParametrizedActionControl)LogError(actionId);

        /// <summary>
        /// Adds the simple action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns></returns>
        public ISimpleActionControl AddSimpleActionControl(string actionId) => (ISimpleActionControl)LogError(actionId);

        /// <summary>
        /// Finds the action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IActionControl FindActionControl(string actionId) => actionControlByActionId.ContainsKey(actionId) ? actionControlByActionId[actionId] : null;

        /// <summary>
        /// Gets the action controls.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<IActionControl> GetActionControls() => actionControlByActionId.Values;

        private object LogError(string actionId)
        {
            Tracing.Tracer.LogWarning("Cannot add action control (Id = {0}) to the SidePanelActionControlContainer: the navigation action should be of the SingleChoiceAction type.", actionId);
            return null;
        }

        /// <summary>
        /// Adds the single choice action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="isHierarchical">if set to <c>true</c> [is hierarchical].</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ISingleChoiceActionControl AddSingleChoiceActionControl(string actionId, bool isHierarchical, SingleChoiceActionItemType itemType)
        {
            var control = new NavigationControl()
            {
                ActionId = actionId,
                X = 0,
                Y = 0,
                Height = Terminal.Gui.Dim.Percent(100),
                Width = Terminal.Gui.Dim.Percent(20),
            };

            ParentView.Add(control);

            actionControlByActionId.Add(actionId, control);
            return control;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.FrameView" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionContainers.INavigationControl" />
    public class NavigationControl : Terminal.Gui.FrameView, ISingleChoiceActionControl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationControl"/> class.
        /// </summary>
        public NavigationControl() : base("Foo")
        {

        }

        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        /// <value>
        /// The action identifier.
        /// </value>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets the native control.
        /// </summary>
        /// <value>
        /// The native control.
        /// </value>
        public object NativeControl => this;

        /// <summary>
        /// Occurs when [execute].
        /// </summary>
        public event EventHandler<SingleChoiceActionControlExecuteEventArgs> Execute;
        /// <summary>
        /// Occurs when [native control disposed].
        /// </summary>
#pragma warning disable CS0067
        public event EventHandler NativeControlDisposed;
#pragma warning restore CS0067

        /// <summary>
        /// Sets the caption.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetCaption(string caption) => Title = caption;

        /// <summary>
        /// Sets the choice action items.
        /// </summary>
        /// <param name="choiceActionItems">The choice action items.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetChoiceActionItems(ChoiceActionItemCollection choiceActionItems)
        {
            Clear();
            foreach(var item in choiceActionItems)
            {
                foreach(var i in item.Items)
                {
                    var button = new SimpleActionMenuBarItem(i);
                    button.Execute += Button_Execute;
                    Add(button);
                }
            }
        }

        private void Button_Execute(object sender, EventArgs e)
            => Execute?.Invoke(this, new SingleChoiceActionControlExecuteEventArgs(((SimpleActionMenuBarItem)sender).ActionItem));

        /// <summary>
        /// Sets the confirmation message.
        /// </summary>
        /// <param name="confirmationMessage">The confirmation message.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetConfirmationMessage(string confirmationMessage) { }
        /// <summary>
        /// Sets the enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetEnabled(bool enabled) { }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetImage(string imageName) { }

        /// <summary>
        /// Sets the paint style.
        /// </summary>
        /// <param name="paintStyle">The paint style.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetPaintStyle(ActionItemPaintStyle paintStyle) { }

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetSelectedItem(ChoiceActionItem selectedItem) { }

        /// <summary>
        /// Sets the shortcut.
        /// </summary>
        /// <param name="shortcutString">The shortcut string.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetShortcut(string shortcutString) { }

        /// <summary>
        /// Sets the show items on click.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetShowItemsOnClick(bool value) { }

        /// <summary>
        /// Sets the tool tip.
        /// </summary>
        /// <param name="toolTip">The tool tip.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetToolTip(string toolTip) { }

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetVisible(bool visible) { }

        /// <summary>
        /// Updates the specified items changed information.
        /// </summary>
        /// <param name="itemsChangedInfo">The items changed information.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(IDictionary<object, ChoiceActionItemChangesType> itemsChangedInfo)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.Button" />
    /// <seealso cref="Terminal.Gui.MenuItem" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.ISimpleActionControl" />
    public class SimpleActionMenuBarItem : Terminal.Gui.Button
    {
        /// <summary>
        /// Gets or sets the action item.
        /// </summary>
        /// <value>
        /// The action item.
        /// </value>
        public ChoiceActionItem ActionItem { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleActionMenuBarItem"/> class.
        /// </summary>
        /// <param name="actionItem">The action item.</param>
        public SimpleActionMenuBarItem(ChoiceActionItem actionItem) : base(actionItem.Caption)
        {
            ActionItem = actionItem;
            Clicked = OnClicked;
        }

        private void OnClicked() => Execute?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when [execute].
        /// </summary>
        public event EventHandler Execute;
    }
}
