using System;

namespace Scissors.ExpressApp.LabelEditor.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public static class LabelEditorAliases
    {
        /// <summary>
        /// The label string editor
        /// </summary>
        public static readonly string LabelStringEditor = Constants.LabelStringEditor;

        /// <summary>
        /// 
        /// </summary>
        public static class Constants
        {
            /// <summary>
            /// The label string editor
            /// </summary>
            public const string LabelStringEditor = nameof(LabelStringEditor);
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Types
        {
            /// <summary>
            /// Gets or sets the label string editor.
            /// </summary>
            /// <value>
            /// The label string editor.
            /// </value>
            public static Type LabelStringEditor { get; set; }
        }
    }
}
