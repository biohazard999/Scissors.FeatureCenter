#if DebugTest
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Model;
using DevExpress.Data.Filtering.Helpers;

namespace DevExpress.ExpressApp.Tests.TestObjects {
	public class TestListEditor : ListEditor, IControlOrderProvider, ISupportUpdate, ISupportAppearanceCustomization, IInstantFeedbackSupport {
		private object focusedObject;
		private bool refreshed = false;
		private int beginUpdateCount;
		private int endUpdateCount;
		private DevExpress.ExpressApp.Templates.IContextMenuTemplate contextMenuTemplate;
		private Object dataSource;
		private Object editorControl = null;
		private void dataSource_ListChanged(object sender, ListChangedEventArgs e) {
			if(ManageFocusedObject) {
				if(e.ListChangedType == ListChangedType.Reset) {
					if((ListHelper.GetList(dataSource) != null) && (ListHelper.GetList(dataSource).Count > 0)) {
						focusedObject = ListHelper.GetList(dataSource)[0];
					}
				}
			}
		}

        protected override List<IModelSynchronizable> CreateModelSynchronizers() {
            List<IModelSynchronizable> result = base.CreateModelSynchronizers();
            result.Add(new TestModelSynchronizer());
            return result;
		}
		protected override object CreateControlsCore() {
			CreateControlsCount++;
			if((List != null) && (List.Count > 0) && (focusedObject == null)) {
				focusedObject = List[0];
			}
            if(EditorControl != null) {
                return EditorControl;
            }
            else {
                return new System.Windows.Forms.Control();
            }
		}
		protected override void AssignDataSourceToControl(Object dataSource) {
			DataBindCount++;
			if(dataSource is IBindingList) {
				((IBindingList)dataSource).ListChanged -= new ListChangedEventHandler(dataSource_ListChanged);
			}
			this.dataSource = dataSource;
			if(dataSource is IBindingList) {
				((IBindingList)dataSource).ListChanged += new ListChangedEventHandler(dataSource_ListChanged);
			}
		}
		protected override void OnAllowEditChanged() {
			base.OnAllowEditChanged();
			OnAllowEditChangedCallCount++;
		}
	
		public TestListEditor() : base() { }
		public TestListEditor(bool manageFocusedObject)
			: base() {
			this.ManageFocusedObject = manageFocusedObject;
		}
		public TestListEditor(IModelListView modelListView) : base(modelListView) { }
        public override void Dispose() {
            if(dataSource is IBindingList) {
                ((IBindingList)dataSource).ListChanged -= new ListChangedEventHandler(dataSource_ListChanged);
            }
            base.Dispose();
        }
		~TestListEditor() {
			DestructorCallCount++;
		}
		public override void SaveModel() {
			base.SaveModel();
			IsSynchronizeInfoCalled = true;
		}
		public void ExecuteProcessSelectedItem() {
			OnFocusedObjectChanged();
			OnProcessSelectedItem();
		}
		public new void OnSelectionChanged() {
			base.OnSelectionChanged();
		}
		public new void OnFocusedObjectChanged() {
			base.OnFocusedObjectChanged();
		}
		public new void LockSelectionEvents() {
			base.LockSelectionEvents();
		}
		public new void UnlockSelectionEvents() {
			base.UnlockSelectionEvents();	
		}
		public override IList GetSelectedObjects() {
			if(selectedObjects != null) {
				return selectedObjects;
			}
			if(focusedObject == null) {
				return new object[0] { };
			}
			return new object[] { focusedObject };
		}
		public void SetContextMenuTemplate(DevExpress.ExpressApp.Templates.IContextMenuTemplate contextMenuTemplate) {
			this.contextMenuTemplate = contextMenuTemplate;
		}
		public void SetSelectionType(SelectionType selectionType) {
			this.selectionType = selectionType;
		}
		public string GetDisplayableProperty(string memberName) {
			return base.GetDisplayablePropertyName(memberName);
		}
		public int GetIndexByObject(Object obj) {
			if(List != null) {
				return List.IndexOf(obj);
			}
			else {
				return -1;
			}
		}
		public Object GetObjectByIndex(int index) {
			GetByIndexCallCount++;
			if((index >= 0) && (index < List.Count)) {
				return List[index];
			}
			return null;
		}
		public IList GetOrderedObjects() {
			GetOrderedObjectsCallCount++;
			return List;
		}
		public override void Refresh() {
			RefreshCallCount++;
			Refreshed = true;
		}
        public void RaiseNewObjectCreated(object obj) {
            OnNewObjectCreated(obj);
        }
        public void RaiseNewObjectCanceled() {
            OnNewObjectCanceled();
        }
		public void RaiseCustomizeAppearance(CustomizeAppearanceEventArgs args) {
			CustomizeAppearance(this, args);
		}
		public override Boolean SupportsDataAccessMode(CollectionSourceDataAccessMode dataAccessMode) {
			return true;
		}
		public override object FocusedObject {
			get { return focusedObject; }
			set {
				if(OnFocusedObjectChanging()) {
					focusedObject = value;
					FocusedObjectSetCallCount++;
					OnFocusedObjectChanged();
				}
			}
		}
		public bool Refreshed {
			get { return refreshed; }
			set { refreshed = value; }
		}
		public override SelectionType SelectionType {
			get { return selectionType; }
		}
		public override DevExpress.ExpressApp.Templates.IContextMenuTemplate ContextMenuTemplate {
			get { return contextMenuTemplate; }
		}
		public override string[] RequiredProperties {
			get {
				return VisibleProperties.Split(';');
			}
		}
		public SelectionType selectionType = SelectionType.Full;
		public IList selectedObjects;
		public int GetByIndexCallCount = 0;
		public int GetOrderedObjectsCallCount = 0;
		public String VisibleProperties = "";
		public Int32 FocusedObjectSetCallCount;
		public Int32 OnAllowEditChangedCallCount;
		public Object EditorControl {
			get {
				return editorControl;
			}
			set {
				editorControl = value;
			}
		}
		public bool ManageFocusedObject {
			get;
			set;
		}
		public int BeginUpdateCount {
			get { return beginUpdateCount; }
			set {
				beginUpdateCount = value;
			}
		}
		public int EndUpdateCount {
			get { return endUpdateCount; }
			set {
				endUpdateCount = value;
			}
		}
		public event EventHandler<CustomizeAppearanceEventArgs> CustomizeAppearance;

		// IInstantFeedbackSupport
		XafInstantFeedbackRecord IInstantFeedbackSupport.GetInstantFeedbackRecordByKey(Object objectKeyValue) {
			GetInstantFeedbackRecordByKey_CallCount++;
			return null;
		}
		XafInstantFeedbackRecord IInstantFeedbackSupport.GetInstantFeedbackRecordByObject(Object obj) {
			GetInstantFeedbackRecordByObject_CallCount++;
			return null;
		}
		EvaluatorContextDescriptor IInstantFeedbackSupport.GetEvaluatorContextDescriptor(IObjectSpace objectSpace) {
			GetEvaluatorContextDescriptor_CallCount++;
			return null;
		}
		Object IInstantFeedbackSupport.GetRecordMemberValue(XafInstantFeedbackRecord instantFeedbackRecord, String memberName) {
			GetRecordMemberValue_CallCount++;
			return null;
		}

		public Int32 GetInstantFeedbackRecordByKey_CallCount;
		public Int32 GetInstantFeedbackRecordByObject_CallCount;
		public Int32 GetEvaluatorContextDescriptor_CallCount;
		public Int32 GetRecordMemberValue_CallCount;

		public static bool IsSynchronizeInfoCalled;
		public static int DestructorCallCount = 0;
		public static int CreateControlsCount = 0;
		public static int DataBindCount = 0;
		public static int RefreshCallCount = 0;

		#region ISupportUpdate Members
		public void BeginUpdate() {
			beginUpdateCount++;
		}
		public void EndUpdate() {
			endUpdateCount++;
		}
		#endregion
	}
}
#endif