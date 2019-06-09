using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Localization;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.Console.Templates;
using Scissors.ExpressApp.Console.Templates.ActionControls.Binding;
using Terminal.Gui;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.XafApplication" />
    /// <seealso cref="DevExpress.ExpressApp.Utils.ISupportSplashScreen" />
    public class ConsoleApplication : XafApplication, ISupportSplashScreen
    {
        private bool exiting;
        private int logonAttemptCount = 0;
        private readonly bool executeStartupLogicBeforeClosingLogonWindow = false;
        private bool isLoggedOff = false;
        private bool isLogonFormDisplayed = false;
        private IFrameTemplateFactory frameTemplateFactory;

        /// <summary>
        /// The ignore user model diffs
        /// </summary>
        protected bool IgnoreUserModelDiffs;

        /// <summary>
        /// Gets the toplevel.
        /// </summary>
        /// <value>
        /// The toplevel.
        /// </value>
        public Toplevel Toplevel { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsLogonFormDisplayed => isLogonFormDisplayed;

        /// <summary>
        /// Gets or sets the frame template factory.
        /// </summary>
        /// <value>
        /// The frame template factory.
        /// </value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFrameTemplateFactory FrameTemplateFactory
        {
            get
            {
                if(frameTemplateFactory == null)
                {
                    frameTemplateFactory = CreateFrameTemplateFactory();
                }
                return frameTemplateFactory;
            }
            set
            {
                Guard.ArgumentNotNull(value, "FrameTemplateFactory");
                frameTemplateFactory = value;
            }
        }

        static ConsoleApplication() => ConsoleSimpleActionBinding.Register();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleApplication"/> class.
        /// </summary>
        public ConsoleApplication() : this(Application.Top) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleApplication"/> class.
        /// </summary>
        /// <param name="toplevel">The toplevel.</param>
        public ConsoleApplication(Toplevel toplevel)
        {
            Toplevel = toplevel;
            SplashScreen = new Core.SplashScreen(Toplevel, "Creating Application...", Title);
        }

        /// <summary>
        /// Occurs when [custom handle exception].
        /// </summary>
        public event EventHandler<CustomHandleExceptionEventArgs> CustomHandleException;
        /// <summary>
        /// Occurs when [logon form showing].
        /// </summary>
        public event EventHandler<ConsoleWindowShowingEventArgs> LogonFormShowing;

        /// <summary>
        /// The splash
        /// </summary>
        private ISplash splash;
        /// <summary>
        /// The support update splash
        /// </summary>
        protected ISupportUpdateSplash SupportUpdateSplash;
        /// <summary>
        /// Gets or sets the splash screen.
        /// </summary>
        /// <value>
        /// The splash screen.
        /// </value>
        public ISplash SplashScreen
        {
            get => splash;
            set
            {
                if((value != splash) && (splash != null))
                {
                    StopSplash();
                }
                splash = value;
                SupportUpdateSplash = splash as ISupportUpdateSplash;
            }
        }

        /// <summary>
        /// Creates the layout manager core.
        /// </summary>
        /// <param name="simple">if set to <c>true</c> [simple].</param>
        /// <returns></returns>
        protected override LayoutManager CreateLayoutManagerCore(bool simple)
            => new ConsoleLayoutManager();

        /// <summary>
        /// Removes the splash.
        /// </summary>
        public virtual void RemoveSplash()
            => SplashScreen = null;

        /// <summary>
        /// Starts the splash.
        /// </summary>
        public virtual void StartSplash()
        {
            splash?.Start();
            UpdateStatus("", Title, "");
        }

        /// <summary>
        /// Stops the splash.
        /// </summary>
        public virtual void StopSplash()
            => splash?.Stop();


        /// <summary>
        /// Updates the splash.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="additionalParams">The additional parameters.</param>
        public virtual void UpdateSplash(string context, string caption, string description, params object[] additionalParams)
        {
            if(SupportUpdateSplash != null)
            {
                SupportUpdateSplash.UpdateSplash(string.IsNullOrEmpty(caption) ? Title : caption, description, additionalParams);
            }
        }

        /// <summary>
        /// Triggers the <see cref="E:DevExpress.ExpressApp.XafApplication.StatusUpdating" /> event.
        /// </summary>
        /// <param name="context">A string that specifies the current context.</param>
        /// <param name="title">A string that specifies the status message title.</param>
        /// <param name="message">A string that specifies the status message.</param>
        /// <param name="additionalParams">An array of additional parameters associated with the status message.</param>
        public override void UpdateStatus(string context, string title, string message, params object[] additionalParams)
        {
            base.UpdateStatus(context, title, message, additionalParams);
            UpdateSplash(context, title, message, additionalParams);
        }

        /// <summary>
        /// Provides access to the application's main Window.
        /// </summary>
        /// <value>
        /// A <see cref="T:DevExpress.ExpressApp.Window" /> object that represents the current application's main Window.
        /// </value>
        public override DevExpress.ExpressApp.Window MainWindow => ShowViewStrategy?.MainWindow;

        /// <summary>
        /// Creates the show view strategy.
        /// </summary>
        /// <returns></returns>
        protected override ShowViewStrategyBase CreateShowViewStrategy()
            => new ConsoleShowViewStrategyBase(this);

        /// <summary>
        /// Specifies the application's Show View Strategy.
        /// </summary>
        /// <value>
        /// A <see cref="T:DevExpress.ExpressApp.ShowViewStrategyBase" /> descendant that represents the application's Show View Strategy.
        /// </value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ConsoleShowViewStrategyBase ShowViewStrategy
        {
            get => base.ShowViewStrategy as ConsoleShowViewStrategyBase;
            set => base.ShowViewStrategy = value;
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Restart()
        {
            ShowViewStrategy.CloseAllWindows();
            Setup(new ExpressApplicationSetupParameters(ApplicationName, Security, ObjectSpaceProviders, ControllersManager, Modules));
            ShowStartupWindow();
        }

        /// <summary>
        /// Shows the startup window.
        /// </summary>
        protected virtual void ShowStartupWindow()
        {
            UpdateStatus(ApplicationStatusMessageId.ShowStartupWindow.ToString(), "", ApplicationStatusMessagesLocalizer.Active.GetLocalizedString(ApplicationStatusMessageId.ShowStartupWindow));
            ShowViewStrategy.ShowStartupWindow();
        }

        /// <summary>
        /// Exits the core.
        /// </summary>
        protected override void ExitCore()
        {
            base.ExitCore();
            exiting = true;
            Application.RequestStop();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            try
            {
                Tick.In("WinApplication.Start()");

                DoLogon();
                if(!executeStartupLogicBeforeClosingLogonWindow)
                {
                    DoStartupLogic();
                }
                if(IsLoggedOn && !exiting)
                {
                    Tracing.Tracer.LogSeparator("Application startup done");
                    Tracing.Tracer.LogSeparator("Application running");
                    Tick.Out("WinApplication.Start()");
                    Tick.DumpResults();
                    DoApplicationRun();
                    if(!IgnoreUserModelDiffs)
                    {
                        SaveModelChanges();
                    }
                }
            }
            catch(Exception e)
            {
                HandleException(e);
            }
            finally
            {
                LogOffCore(false);
            }
            Tracing.Tracer.LogSeparator("Application stopping");
        }

        /// <summary>
        /// Does the application run.
        /// </summary>
        protected virtual void DoApplicationRun()
            => Application.Run(Toplevel);

        /// <summary>
        /// Does the startup logic.
        /// </summary>
        protected void DoStartupLogic()
        {
            if(IsLoggedOn)
            {
                try
                {
                    ProcessStartupActions();
                    if(!exiting)
                    {
                        //if (overlayFormHandle == null)
                        //{
                        //StartSplash();
                        //}
                        StopSplash();
                        ShowStartupWindow();
                    }
                    else
                    {
                        return;
                    }
                }
                finally
                {
                    StopLoading();
                }
            }
        }

        private bool LogOffCore(bool canCancel)
        {
            if(IsLoggedOn)
            {
                var loggingOffEventArgs = new LoggingOffEventArgs(SecuritySystem.LogonParameters, canCancel);
                OnLoggingOff(loggingOffEventArgs);
                if(!canCancel || !loggingOffEventArgs.Cancel)
                {
                    var isMainWindowShown = ShowViewStrategy.MainWindow != null;
                    if(ShowViewStrategy.CloseAllWindows())
                    {
                        if(!IgnoreUserModelDiffs)
                        {
                            SaveModelChanges();
                        }
                        isLoggedOn = false;
                        SecuritySystem.Instance.Logoff();
                        OnLoggedOff();
                        isLoggedOff = true;
                        //ResetUserDifferences();
                        return isMainWindowShown;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Processes the startup actions.
        /// </summary>
        protected virtual void ProcessStartupActions()
        {
            var startUpActions = new List<PopupWindowShowAction>();
            foreach(var module in Modules)
            {
                startUpActions.AddRange(module.GetStartupActions());
            }
            Tracing.Tracer.LogValue("startUpActions", startUpActions.Count);
            foreach(var action in startUpActions)
            {
                Tracing.Tracer.LogVerboseText("startUpAction: {0}, Enabled: {1}, Active: {2}", action.Id, action.Enabled, action.Active);
                if(action.Enabled && action.Active)
                {
                    StopSplash();
                    Tracing.Tracer.LogValue("startUpAction executing", action.Id);
                    action.Application = this;
                    action.HandleException += Action_HandleException;
                    try
                    {
                        using(var helper = new PopupWindowShowActionHelper(action))
                        {
                            helper.ShowPopupWindow();
                        }
                        Tracing.Tracer.LogText("startUpAction executed", action.Id);
                    }
                    finally
                    {
                        action.HandleException -= Action_HandleException;
                    }
                }
            }
        }
        private void Action_HandleException(object sender, HandleExceptionEventArgs e)
        {
            if(!e.Handled)
            {
                HandleException(e.Exception);
                e.Handled = true;
            }
        }

        private void StartLoading(PopupWindowShowActionExecuteEventArgs logonWindowArgs)
            => StartSplash();

        /// <summary>
        /// Calculates the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="viewID">The view identifier.</param>
        /// <returns></returns>
        protected internal new TemplateContext CalculateContext(TemplateContext context, string viewID)
            => base.CalculateContext(context, viewID);

        /// <summary>
        /// Logons the specified logon window arguments.
        /// </summary>
        /// <param name="logonWindowArgs">The <see cref="PopupWindowShowActionExecuteEventArgs"/> instance containing the event data.</param>
        protected override void Logon(PopupWindowShowActionExecuteEventArgs logonWindowArgs)
        {
            try
            {
                StartLoading(logonWindowArgs);
                base.Logon(logonWindowArgs);
                if(logonWindowArgs != null)
                {
                    logonWindowArgs.CanCloseWindow = true;
                }
                logonAttemptCount = 0;
                if(executeStartupLogicBeforeClosingLogonWindow)
                {
                    DoStartupLogic();
                }
            }
            catch(Exception e)
            {
                StopLoading();
                if(logonWindowArgs != null)
                {
                    logonAttemptCount++;
                    if(logonAttemptCount < MaxLogonAttemptCount)
                    {
                        HandleException(e);
                        logonWindowArgs.CanCloseWindow = false;
                    }
                    else
                    {
                        logonWindowArgs.CanCloseWindow = true;
                        HandleException(new UserFriendlyException(UserVisibleExceptionId.LogonAttemptsAmountedToLimitWin, e));
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        private void StopLoading()
            => StopSplash();

        private void DoLogon()
        {
            do
            {
                isLoggedOff = false;
                if(SecuritySystem.Instance.NeedLogonParameters)
                {
                    Tracing.Tracer.LogText("Logon With Parameters");
                    var showLogonAction = CreateLogonAction();
                    var helper = new PopupWindowShowActionHelper(showLogonAction);
#pragma warning disable 0618
                    using(var popupWindow = helper.CreatePopupWindow(false))
                    {
#pragma warning restore 0618
                        ShowLogonWindow(popupWindow);
                    }
                }
                else
                {
                    Logon(null);
                }
            } while(isLoggedOff);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        public void HandleException(Exception e)
        {
            if(e is ActionExecutionException)
            {
                e = e.InnerException;
            }
            var args = new CustomHandleExceptionEventArgs(PreprocessException(e), e);

            CustomHandleException?.Invoke(this, args);

            if(!args.Handled)
            {
                HandleExceptionCore(args.Exception);
            }
        }

        /// <summary>
        /// Preprocesses the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected virtual Exception PreprocessException(Exception e)
        {
            Tracing.Tracer.LogError(e);
            if(!System.Diagnostics.Debugger.IsAttached)
            {
                if((e is DevExpress.Xpo.DB.Exceptions.SqlExecutionErrorException ||
                    e.InnerException is DevExpress.Xpo.DB.Exceptions.SqlExecutionErrorException))
                {
                    e = new UserFriendlyException(UserVisibleExceptionId.UserFriendlySqlException, e);
                }
                if(e is CompatibilityException compatibilityException)
                {
                    if(compatibilityException.Error is CompatibilityUnableToOpenDatabaseError)
                    {
                        e = new UserFriendlyException(UserVisibleExceptionId.UserFriendlyConnectionFailedException, e);
                    }
                    else if(compatibilityException.Error is CompatibilityCheckVersionsError)
                    {
                    }
                    else
                    {
                        e = new UserFriendlyException(UserVisibleExceptionId.UserFriendlyConnectionFailedException, e);
                    }
                }
            }
            return e;
        }

        /// <summary>
        /// Shows the logon window.
        /// </summary>
        /// <param name="popupWindow">The popup window.</param>
        protected virtual void ShowLogonWindow(ConsoleWindow popupWindow)
        {
            StopSplash();
            try
            {
                isLogonFormDisplayed = true;

                var args = new ConsoleWindowShowingEventArgs(popupWindow, popupWindow.Form);

                LogonFormShowing?.Invoke(this, args);

                popupWindow.OnShowing(args);
                popupWindow.ShowDialog();
            }
            finally
            {
                isLogonFormDisplayed = false;
            }
        }

        /// <summary>
        /// Handles the exception core.
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void HandleExceptionCore(Exception e)
        {
            var width = 70;
            var height = 6;

            if(e.Message.Contains(Environment.NewLine))
            {
                width = e.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Max(m => m.Length) + 6;
                height += e.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length;
            }

            var buttons = Debugger.IsAttached ? new[] { "Ok", "Show StackTrace" } : new[] { "Ok" };

            var result = MessageBox.ErrorQuery(width, height, string.IsNullOrEmpty(Title) ? "Error" : Title, e.Message, buttons);
            if(result == 1)
            {
                MessageBoxEx.Query(Toplevel.Bounds.Width - 4, Toplevel.Bounds.Height - 4, "StackTrace", e.StackTrace, "Ok");
            }
        }

        /// <summary>
        /// Creates the window core.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="controllers">The controllers.</param>
        /// <param name="isMain">if set to <c>true</c> [is main].</param>
        /// <param name="activateControllersImmediately">if set to <c>true</c> [activate controllers immediately].</param>
        /// <returns></returns>
        protected override DevExpress.ExpressApp.Window CreateWindowCore(TemplateContext context, ICollection<Controller> controllers, bool isMain, bool activateControllersImmediately)
        {
            Tracing.Tracer.LogVerboseValue("ConsoleApplication.CreateWindowCore.activateControllersImmediately", activateControllersImmediately);
            return new ConsoleWindow(this, context, controllers, isMain, activateControllersImmediately);
        }

        /// <summary>
        /// Creates the popup window core.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="controllers">The controllers.</param>
        /// <returns></returns>
        protected override DevExpress.ExpressApp.Window CreatePopupWindowCore(TemplateContext context, ICollection<Controller> controllers)
            => new ConsoleWindow(this, context, controllers, false, true);

        /// <summary>
        /// Creates the frame template factory.
        /// </summary>
        /// <returns></returns>
        protected virtual IFrameTemplateFactory CreateFrameTemplateFactory()
                    => new DefaultFrameTemplateFactory();

        /// <summary>
        /// Creates the default template.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override IFrameTemplate CreateDefaultTemplate(TemplateContext context)
            => FrameTemplateFactory.CreateTemplate(context);
    }
}
