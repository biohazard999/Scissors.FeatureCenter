using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using Scissors.ExpressApp;
using Scissors.FeatureCenter.Modules.LabelEditorDemos.BusinessObjects;

namespace Scissors.FeatureCenter.Modules.LabelEditorDemos.Controllers
{
    public class LabelDemoModelObjectViewController : BusinessObjectViewController<LabelDemoModel>
    {
        public SimpleAction LoremIpsumSimpleAction { get; }

        public LabelDemoModelObjectViewController()
        {
            LoremIpsumSimpleAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(LoremIpsumSimpleAction)}", PredefinedCategory.Edit)
            {
                Caption = "Insert Lorem Ipsum",
                ImageName = "BO_Skull",
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = "Inserts the famous lorem ipsum text into the selected demo object",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            };

            LoremIpsumSimpleAction.Execute += (s, e) =>
            {
                var txt = LoremIpsum(10, 10, 10, 10, 5);
                CurrentObject.Text = txt;
            };
        }

        static string LoremIpsum(
            int minWords,
            int maxWords,
            int minSentences,
            int maxSentences,
            int numParagraphs)
        {

            var words = new[]
            {
                "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
            };

            var rand = new Random();

            var numSentences = rand.Next(maxSentences - minSentences) + minSentences + 1;

            var numWords = rand.Next(maxWords - minWords) + minWords + 1;

            var result = new StringBuilder();

            for(var p = 0; p < numParagraphs; p++)
            {
                for(var s = 0; s < numSentences; s++)
                {
                    for(var w = 0; w < numWords; w++)
                    {
                        if(w > 0)
                        {
                            result.Append(" ");
                        }

                        if(rand.Next(0, 100) % 2 == 0)
                        {
                            result.Append("<b>");
                            result.Append(words[rand.Next(words.Length)]);
                            result.Append("</b>");
                        }
                        else
                        {
                            result.Append(words[rand.Next(words.Length)]);
                        }
                    }
                    result.Append(". ");
                }
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
    }
}