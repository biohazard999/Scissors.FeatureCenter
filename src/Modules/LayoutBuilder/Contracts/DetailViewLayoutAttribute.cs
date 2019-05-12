using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;

namespace Scissors.ExpressApp.LayoutBuilder.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DetailViewLayoutBuilderAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string DetailViewId { get; }

        /// <summary>
        /// 
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailViewId"></param>
        /// <param name="layout"></param>
        public DetailViewLayoutBuilderAttribute(string detailViewId, Layout layout)
        {
            DetailViewId = detailViewId;
            Layout = layout;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Layout : LayoutItem
    {
        /// <summary>
        /// </summary>
        public Layout() : base(nameof(Layout))
        {
            Main = new Main();
            Items.Add(Main);
        }

        /// <summary>
        /// </summary>
        public Main Main { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Main : LayoutGroup
    {
        /// <summary>
        /// </summary>
        public Main() : base(nameof(Main))
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LayoutGroup : LayoutItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public LayoutGroup(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerticalGroup : LayoutGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public VerticalGroup(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HorizontalGroup : LayoutGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public HorizontalGroup(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmptySpaceItem : LayoutGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public EmptySpaceItem(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ViewItem : LayoutGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewItemId"></param>
        public ViewItem(string id, string viewItemId) : base(id)
            => ViewItemId = viewItemId;

        /// <summary>
        /// 
        /// </summary>
        public string ViewItemId { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TabGroup : LayoutGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public TabGroup(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Tab : LayoutGroup
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public Tab(string id) : base(id)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LayoutItem : IEnumerable<LayoutItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// 
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        public LayoutItem(string id, int? index = null)
        {
            Id = id;
            Index = index;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<LayoutItem> Items { get; } = new List<LayoutItem>();

        /// <summary>       
        /// </summary>
        /// <returns></returns>
        public IEnumerator<LayoutItem> GetEnumerator() => Items.OrderBy(item => item.Index).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
