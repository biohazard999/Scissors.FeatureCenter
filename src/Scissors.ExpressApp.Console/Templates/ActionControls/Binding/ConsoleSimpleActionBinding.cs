using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.ExpressApp.Templates.ActionControls.Binding;

namespace Scissors.ExpressApp.Console.Templates.ActionControls.Binding
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.Binding.SimpleActionBinding" />
    public class ConsoleSimpleActionBinding : SimpleActionBinding
    {
        /// <summary>
        /// Registers this instance.
        /// </summary>
        public static new void Register()
            => ActionBindingFactory.Instance.Register(Name, CanCreate, Create);

        private static ActionBinding Create(ActionBase action, IActionControl actionControl)
            => new ConsoleSimpleActionBinding((SimpleAction)action, (ISimpleActionControl)actionControl);

        private void Action_HandleException(object sender, HandleExceptionEventArgs e)
        {
            if(!e.Handled)
            {
                ((ConsoleApplication)((SimpleAction)sender).Controller.Application).HandleException(e.Exception);
                e.Handled = true;
            }
        }

        private bool ShouldForceEndCurrentEdit()
        {
            if(Action.Data.TryGetValue("ForceEndCurrentEditOnActionClick", out var result))
            {
                return (bool)result;
            }
            return true;
        }

        /// <summary>
        /// Does the execute.
        /// </summary>
        protected override void DoExecute()
        {
            if(ShouldForceEndCurrentEdit())
            {
                //BindingHelper.EndCurrentEdit(Form.ActiveForm);
            }
            base.DoExecute();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSimpleActionBinding"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionControl">The action control.</param>
        public ConsoleSimpleActionBinding(SimpleAction action, ISimpleActionControl actionControl)
            : base(action, actionControl) => Action.HandleException += Action_HandleException;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public override void Dispose()
        {
            Action.HandleException -= Action_HandleException;
            base.Dispose();
        }
    }
}
