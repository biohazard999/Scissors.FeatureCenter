using Scissors.ExpressApp.Console.Templates;
using System;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ConsoleWindowShowingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWindowShowingEventArgs"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="form">The form.</param>
        /// <param name="isModal">if set to <c>true</c> [is modal].</param>
        public ConsoleWindowShowingEventArgs(ConsoleWindow window, ConsoleForm form, bool isModal)
        {
            Window = window;
            Form = form;
            IsModal = isModal;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWindowShowingEventArgs"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="form">The form.</param>
        public ConsoleWindowShowingEventArgs(ConsoleWindow window, ConsoleForm form) :
            this(window, form, false)
        {
        }
        
        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public ConsoleForm Form { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether this instance is modal.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modal; otherwise, <c>false</c>.
        /// </value>
        public bool IsModal { get; private set; }
        
        /// <summary>
        /// Gets the window.
        /// </summary>
        /// <value>
        /// The window.
        /// </value>
        public ConsoleWindow Window { get; private set; }
    }
}
