using Terminal.Gui;

namespace Scissors.ExpressApp.Console.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.Console.ISplash" />
    /// <seealso cref="Scissors.ExpressApp.Console.ISupportUpdateSplash" />
    public class SplashScreen : ISplash, ISupportUpdateSplash
    {
        readonly Toplevel toplevel;
        readonly string initialText;
        readonly string applicationName;
        Dialog dialog;
        ProgressBar progress;
        Label label;
        Application.RunState runState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        /// <param name="toplevel">The toplevel.</param>
        /// <param name="initialText">The initial text.</param>
        /// <param name="applicationName">Name of the application.</param>
        public SplashScreen(Toplevel toplevel, string initialText, string applicationName)
        {
            this.applicationName = applicationName;
            this.toplevel = toplevel;
            this.initialText = initialText;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is started; otherwise, <c>false</c>.
        /// </value>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Sets the display text.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        public void SetDisplayText(string displayText)
        {
            label.Text = displayText;
            progress?.Pulse();
        }

        /// <summary>
        /// Updates the splash.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="additionalParams">The additional parameters.</param>
        public void UpdateSplash(string caption, string description, params object[] additionalParams)
        {
            if(dialog != null && !string.IsNullOrEmpty(caption))
            {
                dialog.Title = caption;
            }

            if(label != null && !string.IsNullOrEmpty(description))
            {
                label.Text = description;
                label.Width = description.Length;
            }

            progress?.Pulse();
            Application.Refresh();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if(IsStarted)
            {
                return;
            }

            IsStarted = true;

            dialog = new Dialog(applicationName, toplevel.Frame.Width - 2, toplevel.Frame.Height - 1);

            progress = new ProgressBar()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Height = 1,
                Width = Dim.Percent(50),
            };

            label = new Label(initialText)
            {
                X = Pos.Center(),
                Y = Pos.Center() + 1,
                Height = 1,
                Width = Dim.Percent(50)
            };

            dialog.Add(progress, label);

            runState = Application.Begin(dialog);

            Application.RunLoop(runState, false);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if(IsStarted)
            {
                IsStarted = false;
                Application.End(runState);
            }
        }
    }
}
