using Scissors.ExpressApp.Console.Templates;
using DevExpress.ExpressApp.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp;

namespace Scissors.ExpressApp.Console
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.FrameTemplateFactoryBase" />
    public class DefaultFrameTemplateFactory : FrameTemplateFactoryBase
    {
        /// <summary>
        /// Creates the nested frame template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreateNestedFrameTemplate() => null;//return new NestedFrameTemplate();
        /// <summary>
        /// Creates the popup window template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreatePopupWindowTemplate() => null;//return new PopupForm();
        /// <summary>
        /// Creates the lookup control template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreateLookupControlTemplate() => null;//return new LookupControlTemplate();
        /// <summary>
        /// Creates the lookup window template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreateLookupWindowTemplate() => null;//return new LookupForm();
        /// <summary>
        /// Creates the application window template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreateApplicationWindowTemplate() => new ConsoleForm();//return new MainForm();
        /// <summary>
        /// Creates the view template.
        /// </summary>
        /// <returns></returns>
        protected override IFrameTemplate CreateViewTemplate() => null;//return new DetailViewForm();
    }
}
