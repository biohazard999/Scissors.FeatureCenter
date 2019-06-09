using NStack;
using Terminal.Gui;

namespace Scissors.ExpressApp.Console.Templates
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Terminal.Gui.Dialog" />
    public class DialogEx : Dialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogEx"/> class with an optional set of buttons to display
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="width">Width for the dialog.</param>
        /// <param name="height">Height for the dialog.</param>
        /// <param name="buttons">Optional buttons to lay out at the bottom of the dialog.</param>
        public DialogEx(ustring title, int width, int height, params Button[] buttons) : base(title, width, height, buttons)
        {

        }

        /// <summary>
        /// This method is invoked by Application.Begin as part of the Application.Run after
        /// the views have been laid out, and before the views are drawn for the first time.
        /// </summary>
        public override void WillPresent()
            => base.WillPresent();
    }
}
