using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp.Console.Templates;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Window" />
    public class ConsoleWindow : Window
    {
        /// <summary>
        /// Occurs when [showing].
        /// </summary>
        public event EventHandler<ConsoleWindowShowingEventArgs> Showing;
        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler Closed;
        /// <summary>
        /// Occurs when [closing].
        /// </summary>
        public event CancelEventHandler Closing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWindow"/> class.
        /// </summary>
        /// <param name="application">An <see cref="T:DevExpress.ExpressApp.XafApplication" /> object that provides methods and properties to manage the current application. This value is assigned to the <see cref="P:DevExpress.ExpressApp.Frame.Application" /> property.</param>
        /// <param name="context">A <see cref="T:DevExpress.ExpressApp.TemplateContext" /> object representing the created Window's context. This value is assigned to the <see cref="P:DevExpress.ExpressApp.Frame.Context" /> property.</param>
        /// <param name="controllers">A ICollection&lt;<see cref="T:DevExpress.ExpressApp.Controller" />&gt; Controllers collection.</param>
        /// <param name="isMain">true if the created Window is main; otherwise, false. This value is assigned to the <see cref="P:DevExpress.ExpressApp.Window.IsMain" /> property.</param>
        /// <param name="activateControllersImmediately">true if Controllers are created immediately after the Window has been created and before the Window's <see cref="P:DevExpress.ExpressApp.Window.Template" /> is created; false if Controllers are created after the Window's <see cref="P:DevExpress.ExpressApp.Window.Template" /> has been created.</param>
        public ConsoleWindow(XafApplication application, TemplateContext context, ICollection<Controller> controllers, bool isMain, bool activateControllersImmediately)
            : base(application, context, controllers, isMain, activateControllersImmediately)
        {
            Tracing.Tracer.LogVerboseValue("ConsoleWindow.activateControllersImmediately", activateControllersImmediately);
            CreateTemplate();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is closing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is closing; otherwise, <c>false</c>.
        /// </value>
        public bool IsClosing { get; set; }

        /// <summary>
        /// Raises the <see cref="E:Showing" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleWindowShowingEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnShowing(ConsoleWindowShowingEventArgs args)
            => Showing?.Invoke(this, args);


        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void ShowDialog()
        {
            //return Form.ShowDialog();
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public void Show()
            => Terminal.Gui.Application.Run(Form);

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public ConsoleForm Form => (ConsoleForm)Template;

        /// <summary>
        /// Called when [template changed].
        /// </summary>
        protected override void OnTemplateChanged()
        {
            base.OnTemplateChanged();
            if(Form != null)
            {
                SubscribeToForm();
            }
        }

        private void SubscribeToForm()
        {
            if(Form != null)
            {
                Form.Closing -= Form_Closing;
                Form.Closing += Form_Closing;
                Form.Closed += Form_Closed;
            }
        }

        private void UnsubscribeFromForm()
        {
            if(Form != null)
            {
                Form.Closing -= Form_Closing;
                Form.Closed -= Form_Closed;

            }
        }

        /// <summary>
        /// Disposes the core.
        /// </summary>
        protected override void DisposeCore()
        {
            UnsubscribeFromForm();
            base.DisposeCore();
        }

        /// <summary>
        /// Called when [template changing].
        /// </summary>
        protected override void OnTemplateChanging()
        {
            UnsubscribeFromForm();
            base.OnTemplateChanging();
        }

        /// <summary>
        /// Closes the Window and optionally refreshes its parent Window.
        /// </summary>
        /// <param name="isForceRefresh">true if the parent Window must be refreshed; otherwise, false.</param>
        /// <returns>
        /// true, if the Window has been successfully closed; otherwise, false.
        /// </returns>
        public override bool Close(bool isForceRefresh)
        {
            if(!IsClosing && (Form != null))
            {
                Form.Close();
            }
            return Form == null;
        }

        private void Form_Closing(object sender, CancelEventArgs e)
            => DoOnFormClosing(e);

        private bool isClosing;

        private void ProcessFormClosing()
        {
            isClosing = true;
            try
            {
                if(View != null)
                {
                    View.Close(false);
                }
                LockUpdates();
                DeactivateViewControllers();
            }
            finally
            {
                isClosing = false;
            }
        }

        /// <summary>
        /// Gets the window text for log.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetWindowTextForLog()
            => Form.Title.ToString();

        private bool checkCanClose = true;

        private void DoOnFormClosing(CancelEventArgs e)
        {
            if(!isClosing)
            {
                Tracing.Tracer.LogText("Window closing: " + GetWindowTextForLog());
                SaveTemplateModel();
                SaveViewModel();
                checkCanClose = false;
                try
                {
                    Closing?.Invoke(this, e);
                }
                finally
                {
                    checkCanClose = true;
                }
                if(!e.Cancel)
                {
                    e.Cancel = !CanClose();
                }

                if(!e.Cancel)
                {
                    checkCanClose = false;
                    try
                    {
                        ProcessFormClosing();
                    }
                    finally
                    {
                        checkCanClose = true;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this instance can close.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can close; otherwise, <c>false</c>.
        /// </returns>
        public bool CanClose()
        {
            if(checkCanClose && (View != null))
            {
                return View.CanClose();
            }
            return true;
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            Tracing.Tracer.LogText("Window closed: " + GetWindowTextForLog());
            //if (LastActiveExplorer == Form)
            //{
            //    LastActiveExplorer = null;
            //}
            OnWindowClosed();
        }

        private void OnWindowClosed()
            => Closed?.Invoke(this, EventArgs.Empty);
    }
}
