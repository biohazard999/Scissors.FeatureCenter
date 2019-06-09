using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Actions;
using Scissors.ExpressApp.Console.Templates;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using Terminal.Gui;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PopupWindowShowActionHelper : IDisposable
    {
        private readonly PopupWindowShowAction action;
        private bool isSizable = true;
        private Size size = Size.Empty;
        private bool maximized = false;
        private void Action_CustomizeTemplate(object sender, CustomizeTemplateEventArgs e)
        {
            var template = (IWindowTemplate)e.Template;
            if(template != null)
            {
                template.IsSizeable = isSizable;
                //if(template is PopupFormBase popupForm)
                //{
                //    popupForm.CustomSize = size;
                //}
            }
        }

        private void Application_CustomizeTemplate(object sender, CustomizeTemplateEventArgs e)
        {
            ((XafApplication)sender).CustomizeTemplate -= Application_CustomizeTemplate;
            if(e.Template is IWindowTemplate formTemplate
                && (!size.IsEmpty || maximized)
                && formTemplate is Terminal.Gui.Window)
            {
                if(formTemplate is ISupportStoreSettings)
                {
                    ((ISupportStoreSettings)formTemplate).SettingsReloaded += (sender1, e1) =>
                    {
                        if(!maximized)
                        {
                            var bounds = ((Terminal.Gui.Window)sender1).Bounds;
                            ((Terminal.Gui.Window)sender1).Bounds = new Rect(bounds.X, bounds.Y, size.Width, size.Height);
                        }
                        else
                        {
                            //((Terminal.Gui.Window)sender1).WindowState = System.Windows.Forms.FormWindowState.Maximized;
                        }
                    };
                }
                else
                {
                    if(!maximized)
                    {
                        var bounds = ((Terminal.Gui.Window)formTemplate).Bounds;
                        ((Terminal.Gui.Window)formTemplate).Bounds = new Rect(bounds.X, bounds.Y, size.Width, size.Height);
                    }
                    else
                    {
                        //((System.Windows.Forms.Form)formTemplate).WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindowShowActionHelper"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public PopupWindowShowActionHelper(PopupWindowShowAction action)
        {
            Guard.ArgumentNotNull(action, "action");
            this.action = action;
        }

        /// <summary>
        /// Shows the popup window.
        /// </summary>
        public void ShowPopupWindow()
            => ShowPopupWindow(true);

        /// <summary>
        /// Shows the popup window.
        /// </summary>
        /// <param name="createAllControllers">if set to <c>true</c> [create all controllers].</param>
        /// <exception cref="ArgumentNullException">args.View</exception>
        public void ShowPopupWindow(bool createAllControllers)
        {
            var args = action.GetPopupWindowParams();
            if(args.View == null)
            {
                throw new ArgumentNullException("args.View");
            }
            isSizable = args.IsSizeable;
            size = new Size(args.Size.Width, args.Size.Height);
            maximized = args.Maximized;
            var newShowViewParameters = new ShowViewParameters(args.View)
            {
                Context = ((ConsoleApplication)action.Application).CalculateContext(args.Context, args.View.Id)
            };

            newShowViewParameters.Controllers.Add(args.DialogController);
            if(action.IsModal)
            {
                newShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            }
            newShowViewParameters.CreateAllControllers = createAllControllers;
            action.CustomizeTemplate += Action_CustomizeTemplate;
            action.Application.CustomizeTemplate += Application_CustomizeTemplate;
            action.Application.ShowViewStrategy.ShowView(newShowViewParameters, new ShowViewSource(null, null));
            action.Application.CustomizeTemplate -= Application_CustomizeTemplate;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
            => action.CustomizeTemplate -= Action_CustomizeTemplate;

        #region Obsolete 10.2
        /// <summary>
        /// Creates the popup window.
        /// </summary>
        /// <param name="createAllControllers">if set to <c>true</c> [create all controllers].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">args.View</exception>
        [Obsolete("Use the ShowPopupWindow method instead."), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ConsoleWindow CreatePopupWindow(bool createAllControllers)
        {
            var args = action.GetPopupWindowParams();
            if(args.View == null)
            {
                throw new ArgumentNullException("args.View");
            }
            var controllers = new List<Controller>(args.DialogController.Controllers)
            {
                args.DialogController
            };
            var result = (ConsoleWindow)action.Application.CreatePopupWindow(args.Context, args.View.Id, createAllControllers, controllers.ToArray());
            result.SetView(args.View, null);
            result.Template.IsSizeable = args.IsSizeable;
            if(result.Template is ISupportStoreSettings)
            {
                ((ISupportStoreSettings)result.Template).SetSettings(action.Application.GetTemplateCustomizationModel(result.Template));
            }
            action.OnCustomizeTemplate(result.Template, result.Context);
            return result;
        }
        #endregion
    }
}
