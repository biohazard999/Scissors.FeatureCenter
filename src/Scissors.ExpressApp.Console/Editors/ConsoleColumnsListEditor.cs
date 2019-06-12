using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace Scissors.ExpressApp.Console.Editors
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevExpress.ExpressApp.Editors.ColumnsListEditor" />
    public abstract class ConsoleColumnsListEditor : ColumnsListEditor
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.Console.Editors.ConsoleColumnsListEditor" />
    public class GridListEditor : ConsoleColumnsListEditor
    {
        /// <summary>
        /// Gets or sets the model ListView.
        /// </summary>
        /// <value>
        /// The model ListView.
        /// </value>
        public IModelListView ModelListView { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridListEditor" /> class.
        /// </summary>
        public GridListEditor(IModelListView modelListView)
            => ModelListView = modelListView;

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public override IList<ColumnWrapper> Columns
            => throw new NotImplementedException();

        /// <summary>
        /// Gets the type of the selection.
        /// </summary>
        /// <value>
        /// The type of the selection.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public override SelectionType SelectionType
            => SelectionType.Full;

        /// <summary>
        /// Gets the selected objects.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override IList GetSelectedObjects()
            => new object[] { };

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Refresh()
            => throw new NotImplementedException();

        /// <summary>
        /// Adds the column core.
        /// </summary>
        /// <param name="columnInfo">The column information.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override ColumnWrapper AddColumnCore(IModelColumn columnInfo)
            => throw new NotImplementedException();

        private object dataSource;

        /// <summary>
        /// Assigns the data source to control.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void AssignDataSourceToControl(object dataSource)
            => this.dataSource = dataSource;

        /// <summary>
        /// Creates the controls core.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object CreateControlsCore()
            => new Terminal.Gui.Label("Here comes the grid");
    }
}
