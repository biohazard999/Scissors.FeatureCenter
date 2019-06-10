using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionContainers;
using DevExpress.ExpressApp.Templates.ActionControls;
using NStack;
using Scissors.ExpressApp.Console.Templates.ActionContainers;
using Terminal.Gui;

namespace Scissors.ExpressApp.Console.Templates
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.Window" />
    public class WindowEx : Terminal.Gui.Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal.Gui.Window"/> class with an optional title and a set frame.
        /// </summary>
        /// <param name="frame">Frame.</param>
        /// <param name="title">Title.</param>
        public WindowEx(Rect frame, ustring title) : base(frame, title)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowEx"/> class with an optional title.
        /// </summary>
        /// <param name="title">Title.</param>
        public WindowEx(ustring title) : base(title)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowEx"/> with
        /// the specified frame for its location, with the specified border
        /// an optional title.
        /// </summary>
        /// <param name="frame">Frame.</param>
        /// <param name="padding">Number of characters to use for padding of the drawn frame.</param>
        /// <param name="title">Title.</param>
        public WindowEx(Rect frame, ustring title, int padding) : base(frame, title, padding)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowEx"/> with
        /// the specified frame for its location, with the specified border
        /// an optional title.
        /// </summary>
        /// <param name="padding">Number of characters to use for padding of the drawn frame.</param>
        /// <param name="title">Title.</param>
        public WindowEx(ustring title, int padding) : base(title, padding)
        {

        }

        /// <summary>
        /// This method is invoked by Application.Begin as part of the Application.Run after
        /// the views have been laid out, and before the views are drawn for the first time.
        /// </summary>
        public override void WillPresent()
        {
            Load?.Invoke(this, EventArgs.Empty);
            base.WillPresent();
        }

        /// <summary>
        /// Occurs when [load].
        /// </summary>
        public event EventHandler Load;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.Toplevel" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.IFrameTemplate" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.IWindowTemplate" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.IActionControlsSite" />
    public class ConsoleForm : Terminal.Gui.Toplevel, IFrameTemplate, IWindowTemplate, IActionControlsSite
    {
        /// <summary>
        /// Gets the console window.
        /// </summary>
        /// <value>
        /// The console window.
        /// </value>
        public WindowEx ConsoleWindow { get; }
        /// <summary>
        /// Gets the menu bar.
        /// </summary>
        /// <value>
        /// The menu bar.
        /// </value>
        public XafMenuBar MenuBar { get; }
        ///// <summary>
        ///// Gets the navigation.
        ///// </summary>
        ///// <value>
        ///// The navigation.
        ///// </value>
        //public NavBarActionList Navigation { get; }
        /// <summary>
        /// Gets the action container exit.
        /// </summary>
        /// <value>
        /// The action container exit.
        /// </value>
        public XafMenuBarActionContainer ActionContainerExit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleForm"/> class.
        /// </summary>
        public ConsoleForm() : base()
        {
            Width = Dim.Percent(100);
            Height = Dim.Percent(100);
            X = 0;
            Y = 0;

            MenuBar = new XafMenuBar
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(100),
                Height = 1
            };

            ConsoleWindow = new WindowEx("")
            {
                Width = Dim.Percent(100),
                Height = Dim.Percent(100),
                X = 0,
                Y = 1,
            };

            ActionContainerExit = new XafMenuBarActionContainer(MenuBar, "Exit")
            {
                ActionCategory = "Exit"
            };

            MenuBar.ActionContainers.Add(ActionContainerExit);

            NavBarActionControlContainer = new NavBarActionControlContainer
            {
                ActionCategory = NavigationHelper.DefaultContainerId,
                ParentView = ConsoleWindow,
            };
            
            ConsoleWindow.Load += ConsoleWindow_Load;

            Add(MenuBar, ConsoleWindow);
        }

        internal void Close()
        {
            var args = new CancelEventArgs(false);
            Closing?.Invoke(this, args);
            if(args.Cancel)
            {
                return;
            }

            Terminal.Gui.Application.RequestStop();
            Terminal.Gui.Application.Refresh();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void ConsoleWindow_Load(object sender, EventArgs e)
            => Load?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when [load].
        /// </summary>
        public event EventHandler Load;
        /// <summary>
        /// Occurs when [closing].
        /// </summary>
        public event EventHandler<CancelEventArgs> Closing;
        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler Closed;

        ///// <summary>
        ///// Provides access to a Template's Action Containers.
        ///// </summary>
        ///// <returns>
        ///// An instance of the ICollection&lt;<see cref="T:DevExpress.ExpressApp.Templates.IActionContainer" />&gt; collection that contains the current Template's Action Containers.
        ///// </returns>
        //public ICollection<IActionContainer> GetContainers() => new IActionContainer[] { Navigation };

        ///// <summary>
        ///// Provides access to a Template's Action Container that contains the Actions with the <see cref="P:DevExpress.ExpressApp.Actions.ActionBase.Category" /> property set to Unspecified.
        ///// </summary>
        ///// <value>
        ///// An instance of the class that implement the <see cref="T:DevExpress.ExpressApp.Templates.IActionContainer" /> interface.
        ///// </value>
        ///// <exception cref="NotImplementedException"></exception>
        //IActionControlContainer IActionControlsSite.DefaultContainer => NavBarActionControlContainer;

        ///// <summary>
        ///// Gets the action controls.
        ///// </summary>
        ///// <value>
        ///// The action controls.
        ///// </value>
        //public IEnumerable<IActionControl> ActionControls => MenuBar.ActionControls;
        ///// <summary>
        ///// Gets the action containers.
        ///// </summary>
        ///// <value>
        ///// The action containers.
        ///// </value>
        //public IEnumerable<IActionControlContainer> ActionContainers => MenuBar.ActionContainers;

        ///// <summary>
        ///// Provides access to a Template's Action Container that contains the Actions with the <see cref="P:DevExpress.ExpressApp.Actions.ActionBase.Category" /> property set to Unspecified.
        ///// </summary>
        ///// <value>
        ///// An instance of the class that implement the <see cref="T:DevExpress.ExpressApp.Templates.IActionContainer" /> interface.
        ///// </value>
        //public IActionContainer DefaultContainer { get; }
        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void SetView(DevExpress.ExpressApp.View view)
        {

        }

        /// <summary>
        /// Displays the specified status messages on a Template (e.g. in its status bar).
        /// </summary>
        /// <param name="statusMessages">An object representing the ICollection&lt;string&gt; collection of status messages.</param>
        public void SetStatus(ICollection<string> statusMessages)
        {

        }

        /// <summary>
        /// Sets the specified caption to a Window Template.
        /// </summary>
        /// <param name="caption">A string value that represents a caption to be set to the current Window Template.</param>
        public void SetCaption(string caption) => ConsoleWindow.Title = caption;



        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get => ConsoleWindow.Title.ToString(); set => ConsoleWindow.Title = value; }

        /// <summary>
        /// Indicates whether a Window Template represents a resizable control.
        /// </summary>
        /// <value>
        /// true if the Template is resizable; otherwise, false.
        /// </value>
        public bool IsSizeable { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the nav bar action control container.
        /// </summary>
        /// <value>
        /// The nav bar action control container.
        /// </value>
        public NavBarActionControlContainer NavBarActionControlContainer { get; set; }

        IEnumerable<IActionControl> IActionControlsSite.ActionControls => ActionContainerExit.GetActionControls();

        IEnumerable<IActionControlContainer> IActionControlsSite.ActionContainers => new IActionControlContainer[] { ActionContainerExit, NavBarActionControlContainer };

        IActionControlContainer IActionControlsSite.DefaultContainer => ActionContainerExit;

        ICollection<IActionContainer> IFrameTemplate.GetContainers() => new IActionContainer[] {  };

        IActionContainer IFrameTemplate.DefaultContainer => null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.MenuBar" />
    public class XafMenuBar : MenuBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XafMenuBar"/> class.
        /// </summary>
        public XafMenuBar() : base(new MenuBarItem[] { }) { }

        /// <summary>
        /// Gets or sets the action controls.
        /// </summary>
        /// <value>
        /// The action controls.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [ReadOnly(false), TypeConverter(typeof(CollectionConverter))]
        public List<IActionControl> ActionControls { get; set; } = new List<IActionControl>();

        /// <summary>
        /// Gets or sets the action containers.
        /// </summary>
        /// <value>
        /// The action containers.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [ReadOnly(false), TypeConverter(typeof(CollectionConverter))]
        public List<IActionControlContainer> ActionContainers { get; set; } = new List<IActionControlContainer>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.MenuBarItem" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.IActionControlContainer" />
    public class XafMenuBarActionContainer : MenuBarItem, IActionControlContainer
    {
        /// <summary>
        /// The xaf menu bar
        /// </summary>
        public readonly XafMenuBar XafMenuBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="XafMenuBarActionContainer"/> class.
        /// </summary>
        /// <param name="xafMenuBar">The xaf menu bar.</param>
        /// <param name="title">The title.</param>
        public XafMenuBarActionContainer(XafMenuBar xafMenuBar, string title) : base(title, new MenuItem[] { })
        {
            XafMenuBar = xafMenuBar;
            XafMenuBar.Menus = XafMenuBar.Menus.Concat(new[] { this }).ToArray();
        }

        /// <summary>
        /// Gets or sets the action category.
        /// </summary>
        /// <value>
        /// The action category.
        /// </value>
        public string ActionCategory { get; set; }

        /// <summary>
        /// Adds the parametrized action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IParametrizedActionControl AddParametrizedActionControl(string actionId, Type valueType) => throw new NotImplementedException();

        /// <summary>
        /// Adds the simple action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns></returns>
        public ISimpleActionControl AddSimpleActionControl(string actionId)
        {
            var item = new SimpleActionMenuBarItem(actionId);

            Children = Children.Concat(new[] { item }).ToArray();

            return item;
        }

        /// <summary>
        /// Adds the single choice action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="isHierarchical">if set to <c>true</c> [is hierarchical].</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ISingleChoiceActionControl AddSingleChoiceActionControl(string actionId, bool isHierarchical, SingleChoiceActionItemType itemType) => throw new NotImplementedException();

        /// <summary>
        /// Finds the action control.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns></returns>
        public IActionControl FindActionControl(string actionId)
            => Children.OfType<IActionControl>().FirstOrDefault(item => item.ActionId == actionId);

        /// <summary>
        /// Gets the action controls.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IActionControl> GetActionControls()
            => Children.OfType<IActionControl>();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.MenuItem" />
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.ISimpleActionControl" />
    public class SimpleActionMenuBarItem : MenuItem, ISimpleActionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleActionMenuBarItem"/> class.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        public SimpleActionMenuBarItem(string actionId) : base(string.Empty, string.Empty, null)
        {
            ActionId = actionId;
            Action = DoExecute;
        }

        private void DoExecute()
            => Execute?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Gets the action identifier.
        /// </summary>
        /// <value>
        /// The action identifier.
        /// </value>
        public string ActionId { get; }

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
        public event EventHandler<EventArgs> Execute;
#pragma warning disable CS0067
        /// <summary>
        /// Occurs when [native control disposed].
        /// </summary>
        public event EventHandler NativeControlDisposed;
#pragma warning restore CS0067

        /// <summary>
        /// Sets the caption.
        /// </summary>
        /// <param name="caption">The caption.</param>
        public void SetCaption(string caption)
            => Title = caption;

        /// <summary>
        /// Sets the confirmation message.
        /// </summary>
        /// <param name="confirmationMessage">The confirmation message.</param>
        public void SetConfirmationMessage(string confirmationMessage)
        {
        }

        /// <summary>
        /// Sets the enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public void SetEnabled(bool enabled)
        {
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        public void SetImage(string imageName)
        {
        }

        /// <summary>
        /// Sets the paint style.
        /// </summary>
        /// <param name="paintStyle">The paint style.</param>
        public void SetPaintStyle(ActionItemPaintStyle paintStyle)
        {
        }

        /// <summary>
        /// Sets the shortcut.
        /// </summary>
        /// <param name="shortcutString">The shortcut string.</param>
        public void SetShortcut(string shortcutString)
        {
        }

        /// <summary>
        /// Sets the tool tip.
        /// </summary>
        /// <param name="toolTip">The tool tip.</param>
        public void SetToolTip(string toolTip)
            => Help = toolTip;

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public void SetVisible(bool visible)
        {
        }
    }
}
