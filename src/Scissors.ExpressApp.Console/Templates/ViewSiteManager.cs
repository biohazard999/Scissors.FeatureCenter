using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Utils;
using Control = Terminal.Gui.View;

namespace Scissors.ExpressApp.Console.Templates
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewSiteManager : IDisposable
    {
        private Control viewSiteControl;
        private bool isViewSiteEmpty;
        private bool isViewCreateControlsProcessing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewSiteManager"/> class.
        /// </summary>
        public ViewSiteManager() => isViewSiteEmpty = true;

        private void OnViewChanged()
        {
            if(GetIsViewSiteReady())
            {
                ClearViewSite();
                FillViewSite();
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            if(!isViewCreateControlsProcessing && GetIsViewSiteReady())
            {
                ClearViewSite();
                FillViewSite((Control)View.Control);
            }
        }

        private void FillViewSite()
        {
            if(View != null)
            {
                EnsureViewControl();
                FillViewSite((Control)View.Control);
            }
        }

        private void EnsureViewControl()
        {
            if(!View.IsControlCreated)
            {
                isViewCreateControlsProcessing = true;
                try
                {
                    View.CreateControls();
                }
                finally
                {
                    isViewCreateControlsProcessing = false;
                }
            }
        }

        private bool GetIsViewSiteReady() => viewSiteControl != null;

        private void FillViewSite(Control control)
        {
            Guard.ArgumentNotNull(control, "control");
            if(!isViewSiteEmpty)
            {
                throw new InvalidOperationException("ViewSite is not empty.");
            }
            //viewSiteControl.SuspendLayout();
            try
            {
                if(control is ISupportUpdate)
                {
                    ((ISupportUpdate)control).BeginUpdate();
                }

                var form = viewSiteControl.SuperView;
                if(form != null)
                {
                    control.Bounds = viewSiteControl.Bounds;
                }
                viewSiteControl.Add(control);
                if(control is ISupportUpdate)
                {
                    ((ISupportUpdate)control).EndUpdate();
                }
            }
            finally
            {
                //viewSiteControl.ResumeLayout();
                isViewSiteEmpty = false;
            }
        }

        private void ClearViewSite()
        {
            if(!isViewSiteEmpty)
            {
                try
                {
                    viewSiteControl.Clear();
                }
                finally
                {
                    isViewSiteEmpty = true;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => ViewSiteControl = null;

        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void SetView(View view)
        {
            if(View != null)
            {
                View.ControlsCreated -= new EventHandler(View_ControlsCreated);
            }
            View = view;
            if(View != null)
            {
                View.ControlsCreated += new EventHandler(View_ControlsCreated);
            }
            OnViewChanged();
        }


        /// <summary>
        /// Gets or sets the view site control.
        /// </summary>
        /// <value>
        /// The view site control.
        /// </value>
        public Control ViewSiteControl
        {
            get => viewSiteControl;
            set
            {
                if(viewSiteControl != null)
                {
                    //viewSiteControl.HandleCreated -= new EventHandler(ViewSiteControl_HandleCreated);
                }

                viewSiteControl = value;

                if(viewSiteControl != null)
                {
                    if(isViewSiteEmpty)
                    {
                        FillViewSite();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        [Browsable(false)]
        public View View { get; private set; }
    }
}
