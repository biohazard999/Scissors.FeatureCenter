using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using Scissors.ExpressApp.Console.Templates;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ShowViewStrategyBase" />
    public class ConsoleShowViewStrategyBase : ShowViewStrategyBase
    {
        /// <summary>
        /// Occurs when [startup window load].
        /// </summary>
        public event EventHandler StartupWindowLoad;
        /// <summary>
        /// Occurs when [console window showing].
        /// </summary>
        public event EventHandler<ConsoleWindowShowingEventArgs> ConsoleWindowShowing;
        private bool internalCloseMainWindow = false;
        /// <summary>
        /// The optimized controllers creation
        /// </summary>
        protected bool OptimizedControllersCreation;

        /// <summary>
        /// The windows
        /// </summary>
        protected List<ConsoleWindow> WindowList = new List<ConsoleWindow>();
        /// <summary>
        /// Gets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public ReadOnlyCollection<ConsoleWindow> Windows
            => WindowList.AsReadOnly();

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <value>
        /// The main window.
        /// </value>
        public ConsoleWindow MainWindow
            => Windows.FirstOrDefault(window => window.IsMain);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleShowViewStrategyBase"/> class.
        /// </summary>
        /// <param name="application">An <see cref="T:DevExpress.ExpressApp.XafApplication" /> descendant object that represents the application that will use the instantiated Show View Strategy.</param>
        public ConsoleShowViewStrategyBase(XafApplication application) : base(application)
        {
        }

        /// <summary>
        /// Shows the message core.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowMessageCore(MessageOptions options) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view from common view.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewFromCommonView(ShowViewParameters parameters, ShowViewSource showViewSource) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view from lookup view.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewFromLookupView(ShowViewParameters parameters, ShowViewSource showViewSource) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view from nested view.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewFromNestedView(ShowViewParameters parameters, ShowViewSource showViewSource) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view in current window.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewInCurrentWindow(ShowViewParameters parameters, ShowViewSource showViewSource) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view in modal window.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sourceFrame">The source frame.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewInModalWindow(ShowViewParameters parameters, ShowViewSource sourceFrame) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view in new window.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Window ShowViewInNewWindow(ShowViewParameters parameters, ShowViewSource showViewSource) => throw new NotImplementedException();

        /// <summary>
        /// Shows the view in popup window core.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="okDelegate">The ok delegate.</param>
        /// <param name="cancelDelegate">The cancel delegate.</param>
        /// <param name="okButtonCaption">The ok button caption.</param>
        /// <param name="cancelButtonCaption">The cancel button caption.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void ShowViewInPopupWindowCore(View view, Action okDelegate, Action cancelDelegate, string okButtonCaption, string cancelButtonCaption) => throw new NotImplementedException();

        /// <summary>
        /// Creates the window.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="showViewSource">The show view source.</param>
        /// <param name="isMain">if set to <c>true</c> [is main].</param>
        /// <returns></returns>
        protected virtual ConsoleWindow CreateWindow(ShowViewParameters parameters, ShowViewSource showViewSource, bool isMain)
        {
            ConsoleWindow window = null;
            try
            {
                var context = isMain ? TemplateContext.ApplicationWindow : CalculateTemplateContext(parameters);
                if(OptimizedControllersCreation
                        || (parameters.TargetWindow == TargetWindow.NewWindow) || (parameters.TargetWindow == TargetWindow.NewModalWindow))
                {
                    window = (ConsoleWindow)Application.CreateWindow(context, parameters.Controllers, true, isMain, parameters.CreatedView);
                }
                else
                {
                    window = (ConsoleWindow)Application.CreateWindow(context, parameters.Controllers, isMain);
                }
                AddWindow(window);
                if(!isMain && showViewSource != null)
                {
                    window.SetView(parameters.CreatedView, showViewSource.SourceFrame);
                }
            }
            catch
            {
                if(window != null)
                {
                    RemoveWindow(window);
                    window.Dispose();
                }
                throw;
            }
            return window;
        }
        private void AddWindow(ConsoleWindow window)
        {
            if(!Windows.Contains(window))
            {
                window.Closed += Window_Closed;
                window.Closing += Window_Closing;
                window.Disposed += Window_Disposed;
                WindowList.Add(window);
                AfterAddWindow(window);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(!e.Cancel)
            {
                OnWindowClosing((ConsoleWindow)sender, e);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = (ConsoleWindow)sender;
            window.Closed -= Window_Closed;
            OnWindowClosed(window);
            if(!internalCloseMainWindow && Windows.Count == 0 && !((ConsoleApplication)Application).IsLogonFormDisplayed)
            {
                ExitApplication();
            }
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        protected virtual void ExitApplication()
        {
            Terminal.Gui.Application.RequestStop();
            Environment.Exit(0);
        }

        /// <summary>
        /// Gets the child windows.
        /// </summary>
        /// <param name="winWindow">The win window.</param>
        /// <returns></returns>
        protected virtual List<ConsoleWindow> GetChildWindows(ConsoleWindow winWindow) =>
            new List<ConsoleWindow>();

        /// <summary>
        /// Called when [window closing].
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnWindowClosing(ConsoleWindow window, CancelEventArgs e)
        {
            var childWindows = GetChildWindows(window);
            if(!CloseWindows(childWindows))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Called when [window closed].
        /// </summary>
        /// <param name="window">The window.</param>
        protected virtual void OnWindowClosed(ConsoleWindow window)
            => RemoveWindow(window);

        private void Window_Disposed(object sender, EventArgs e)
        {
            var window = (ConsoleWindow)sender;
            window.Closed -= Window_Closed;
            window.Closing -= Window_Closing;
            window.Disposed -= Window_Disposed;
            RemoveWindow(window);
        }
        /// <summary>
        /// Afters the add window.
        /// </summary>
        /// <param name="window">The window.</param>
        protected virtual void AfterAddWindow(ConsoleWindow window) { }

        private void RemoveWindow(ConsoleWindow window) => WindowList.Remove(window);

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="window">The window.</param>
        protected virtual void ShowWindow(ConsoleWindow window)
            => window.Show();

        /// <summary>
        /// Calculates the template context.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected virtual TemplateContext CalculateTemplateContext(ShowViewParameters parameters)
        {
            var context = parameters.Context;
            if(context == TemplateContext.Undefined)
            {
                if(parameters.Controllers.Exists((Controller c) => { return (c is DialogController); }))
                {
                    context = TemplateContext.PopupWindow;
                }
                else
                {
                    context = TemplateContext.View;
                }
            }
            return context;
        }

        /// <summary>
        /// Closes all windows.
        /// </summary>
        /// <returns></returns>
        public virtual bool CloseAllWindows()
        {
            internalCloseMainWindow = true;
            try
            {
                return CloseWindows(Windows);
            }
            finally
            {
                internalCloseMainWindow = false;
            }
        }

        /// <summary>
        /// Shows the startup window.
        /// </summary>
        public void ShowStartupWindow()
            => ShowStartupWindowCore();

        private bool CloseWindows(ICollection<ConsoleWindow> windows)
        {
            var clone = new List<ConsoleWindow>(windows);
            foreach(var win in clone)
            {
                if(!win.IsClosing && !win.Close())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Shows the startup window core.
        /// </summary>
        /// <returns></returns>
        protected virtual ConsoleWindow ShowStartupWindowCore()
        {
            var showViewParameters = new ShowViewParameters
            {
                Context = TemplateContext.ApplicationWindow
            };
            var startupWindow = CreateWindow(showViewParameters, null, true);
            startupWindow.Form.Load += new EventHandler(StartupWindow_Load);
            OnConsoleWindowShowing(new ConsoleWindowShowingEventArgs(startupWindow, startupWindow.Form));
            ShowWindow(startupWindow);
            return startupWindow;
        }

        private void StartupWindow_Load(object sender, EventArgs e)
        {
            var startupForm = (ConsoleForm)sender;
            startupForm.Load -= new EventHandler(StartupWindow_Load);
            var startupWindow = FindWindowByForm(startupForm);
            OnStartupWindowLoad(startupWindow);
        }

        /// <summary>
        /// Called when [startup window load].
        /// </summary>
        /// <param name="startupWindow">The startup window.</param>
        protected virtual void OnStartupWindowLoad(ConsoleWindow startupWindow)
            => StartupWindowLoad?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Finds the window by form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        protected ConsoleWindow FindWindowByForm(ConsoleForm form)
        {
            foreach(var window in Windows)
            {
                if(window.Form == form)
                {
                    return window;
                }
            }
            return null;
        }

        /// <summary>
        /// Raises the <see cref="E:ConsoleWindowShowing" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleWindowShowingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnConsoleWindowShowing(ConsoleWindowShowingEventArgs args)
        {
            ConsoleWindowShowing?.Invoke(this, args);
            if(args.Window != null)
            {
                args.Window.OnShowing(args);
            }
        }
    }
}
