using System;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates.ActionContainers;
using DevExpress.ExpressApp.Templates.ActionControls;
using Terminal.Gui;

namespace Scissors.ExpressApp.Console.Templates.ActionContainers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.FrameView" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionContainers.INavigationControl" />
    public class NavBarConsoleControl : FrameView, INavigationControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavBarConsoleControl"/> class.
        /// </summary>
        public NavBarConsoleControl() : base(null)
        {
            Height = Dim.Percent(100);
            Width = Dim.Percent(100);
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Sets the navigation action items.
        /// </summary>
        /// <param name="actionItems">The action items.</param>
        /// <param name="action">The action.</param>
        public void SetNavigationActionItems(ChoiceActionItemCollection actionItems, SingleChoiceAction action)
        {
            foreach(var item in actionItems)
            {
                Title = item.Caption;

                foreach(var subItem in item.Items)
                {
                    var navButton = new NavigationButton(subItem)
                    {
                        Width = Dim.Percent(100),
                        Height = 1,
                        Text = subItem.Caption,
                    };

                    navButton.ItemClicked += NavButton_ItemClicked;

                    Add(navButton);
                }
            }
        }

        private void NavButton_ItemClicked(object sender, EventArgs e)
            => RaiseExecute(((NavigationButton)sender).Item);

        /// <summary>
        /// Raises the execute.
        /// </summary>
        /// <param name="actionItem">The action item.</param>
        protected void RaiseExecute(ChoiceActionItem actionItem)
        {
            if(actionItem != null && Execute != null)
            {
                var args = new SingleChoiceActionControlExecuteEventArgs(actionItem);
                Execute(this, args);
            }
        }

        /// <summary>
        /// Occurs when [execute].
        /// </summary>
        public event EventHandler<SingleChoiceActionControlExecuteEventArgs> Execute;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.Button" />
    public class NavigationButton : Terminal.Gui.Button
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public ChoiceActionItem Item { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationButton"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public NavigationButton(ChoiceActionItem item) : base(item.Caption)
        {
            Item = item;
            Clicked = OnClicked;
        }

        /// <summary>
        /// Called when [clicked].
        /// </summary>
        protected virtual void OnClicked()
            => ItemClicked?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when [item clicked].
        /// </summary>
        public event EventHandler ItemClicked;
    }
}
