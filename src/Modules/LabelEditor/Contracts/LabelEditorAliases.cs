using System;

namespace Scissors.ExpressApp.LabelEditor.Contracts
{
    public static class LabelEditorAliases
    {
        public static readonly string LabelStringEditor = Consts.LabelStringEditor;

        public static class Consts
        {
            public const string LabelStringEditor = nameof(LabelStringEditor);
        }
        
        public static class Types
        {
            public static Type LabelStringEditor { get; set; }
        }
    }
}
