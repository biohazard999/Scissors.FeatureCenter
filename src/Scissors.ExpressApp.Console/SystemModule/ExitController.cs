using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace Scissors.ExpressApp.Console.SystemModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.WindowController" />
    public class ExitController : WindowController
    {
        /// <summary>
        /// Gets the exit action.
        /// </summary>
        /// <value>
        /// The exit action.
        /// </value>
        public SimpleAction ExitAction { get; }

        /// <summary>
        /// The exit action identifier
        /// </summary>
        public const string ExitActionId = "Exit";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitController"/> class.
        /// </summary>
        public ExitController()
        {
            TargetWindowType = WindowType.Main;
            ExitAction = new SimpleAction(this, ExitActionId, "Exit")
            {
                Caption = "Exit",
                ImageName = "Action_Exit"
            };
            ExitAction.Execute += ExitAction_OnExecute;
        }

        private void ExitAction_OnExecute(object sender, SimpleActionExecuteEventArgs args)
            => Exit();

        /// <summary>
        /// Exits this instance.
        /// </summary>
        protected virtual void Exit()
            => Window.Close();
    }
}
