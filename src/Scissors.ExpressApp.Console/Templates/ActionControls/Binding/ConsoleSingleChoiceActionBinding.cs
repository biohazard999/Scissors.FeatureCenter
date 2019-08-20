using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.ExpressApp.Templates.ActionControls.Binding;

namespace Scissors.ExpressApp.Console.Templates.ActionControls.Binding
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Templates.ActionControls.Binding.SingleChoiceActionBinding" />
    public class ConsoleSingleChoiceActionBinding : SingleChoiceActionBinding
    {
        /// <summary>
        /// Registers this instance.
        /// </summary>
        public static new void Register()
            => ActionBindingFactory.Instance.Register(Name, CanCreate, Create);
        
        private static ActionBinding Create(ActionBase action, IActionControl actionControl)
            => new ConsoleSingleChoiceActionBinding((SingleChoiceAction)action, (ISingleChoiceActionControl)actionControl);
        
        private void Action_HandleException(object sender, HandleExceptionEventArgs e)
        {
            if(!e.Handled)
            {
                ((ConsoleApplication)((SingleChoiceAction)sender).Controller.Application).HandleException(e.Exception);
                e.Handled = true;
            }
        }

        //protected override void DoExecute(ChoiceActionItem item)
        //{
        //    BindingHelper.EndCurrentEdit(Form.ActiveForm);
        //    base.DoExecute(item);
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSingleChoiceActionBinding"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionControl">The action control.</param>
        public ConsoleSingleChoiceActionBinding(SingleChoiceAction action, ISingleChoiceActionControl actionControl)
            : base(action, actionControl)
            => Action.HandleException += Action_HandleException;

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
