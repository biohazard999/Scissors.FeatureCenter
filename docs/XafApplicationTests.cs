#if DebugTest
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Base.Tests;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Tests.Editors;
using DevExpress.ExpressApp.Tests.TestObjects;
using DevExpress.ExpressApp.Tests.XafApplicationTests_TestObjects;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Xpo.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using NUnit.Framework;

namespace DevExpress.ExpressApp.Tests {
	namespace XafApplicationTests_TestObjects {
		[DomainComponent]
		public class TestClassA : IObjectSpaceLink {
			public TestClassA(IObjectSpace objectSpace) {
				ObjectSpace = objectSpace;
			}
			public IObjectSpace ObjectSpace { get; set; }
		}
		[DomainComponent]
		public class TestClassB {
			public TestClassB() {
			}
			public String Name { get; set; }
		}
	}

	public class XafApplicationForTests2 : XafApplication {
        public static bool isCompatibilityChecked; 
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.AddRange(TestObjectSpaceProviders);
        }
        protected override LayoutManager CreateLayoutManagerCore(Boolean simple) {
            return null;
        }
		protected override void LoadUserDifferences() {
			if(!SkipUserDifferencesLoading) {
				base.LoadUserDifferences();
			}
		}
        protected override bool IsCompatibilityChecked {
            get {
                if(UseStaticIsCompatibilityCheckedFlag) {
                    return isCompatibilityChecked;
                }
                else {
                    return base.IsCompatibilityChecked;
                }
            }
            set {
                if(UseStaticIsCompatibilityCheckedFlag) {
                    isCompatibilityChecked = value;
                }
                else {
                    base.IsCompatibilityChecked = value;
                }
            }
        }
        protected override ShowViewStrategyBase CreateShowViewStrategy() {
            return new TestShowViewStrategy(this);
        }
        public XafApplicationForTests2() {
            TestObjectSpaceProviders = new List<IObjectSpaceProvider>();
            CheckedObjectSpaceProviders = new List<IObjectSpaceProvider>();
        }
        public XafApplicationForTests2(ITypesInfo typesInfo) : base(typesInfo) {
            TestObjectSpaceProviders = new List<IObjectSpaceProvider>();
        }
		public override DatabaseUpdaterBase CreateDatabaseUpdater(IObjectSpaceProvider objectSpaceProvider) {
			CreateDatabaseUpdater_CallCount++;
            CheckedObjectSpaceProviders.Add(objectSpaceProvider);
			return base.CreateDatabaseUpdater(objectSpaceProvider);
		}
		public void AddObjectSpaceProvider(IObjectSpaceProvider objectSpaceProvider) {
			objectSpaceProviders.Add(objectSpaceProvider);
		}
		public Boolean SkipUserDifferencesLoading;
        public List<IObjectSpaceProvider> TestObjectSpaceProviders;
		public Int32 CreateDatabaseUpdater_CallCount;
        public List<IObjectSpaceProvider> CheckedObjectSpaceProviders;
        public bool UseStaticIsCompatibilityCheckedFlag = false;
    }

    public class XafApplicationForTests : XafApplication {
        private Boolean createTestLayoutManagerByDefault;
		protected internal IObjectSpaceProvider objectSpaceProviderResult;
		protected internal Boolean? isObjectSpaceProviderOwnerResult;
		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
			if(objectSpaceProviderResult != null) {
				args.ObjectSpaceProviders.Add(objectSpaceProviderResult);
			}
			else {
				args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, true);
			}
			if(isObjectSpaceProviderOwnerResult.HasValue) {
				args.IsObjectSpaceProviderOwner = isObjectSpaceProviderOwnerResult.Value;
			}
		}
        protected override LayoutManager CreateLayoutManagerCore(Boolean simple) {
            if(createTestLayoutManagerByDefault) {
                return new TestLayoutManager();
            }
            else {
                return null;
            }
        }
        internal Action<IList<IModelApplication>> customizeUserDifferences;
        protected override void CustomizeUserDifferences(IList<IModelApplication> layers) {
            if(customizeUserDifferences != null) {
                customizeUserDifferences(layers);
            }
        }
		public override DatabaseUpdaterBase CreateDatabaseUpdater(IObjectSpaceProvider objectSpaceProvider) {
			if(CreateDatabaseUpdater_AllowBase) {
				return base.CreateDatabaseUpdater(objectSpaceProvider);
			}
			return CreateDatabaseUpdater_Result;
		}
        public ExpressApp.CheckCompatibilityType TestGetCheckCompatibilityType(IObjectSpaceProvider objectSpaceProvider) {
            return base.GetCheckCompatibilityType(objectSpaceProvider);
        }
		public XafApplicationForTests(IXpoDataStoreProvider dsProvider, IModelApplication modelApplication) {
			Setup(new ExpressApplicationSetupParameters("test app", new XPObjectSpaceProvider(dsProvider, true), new ControllersManager(), Modules));
			SetModelApplication(modelApplication);
		}
        public XafApplicationForTests() : base() { }
        public void DropModelForTests() {
            DropModel();
        }
		public IObjectSpace CreateLogonWindowObjectSpace() {
			return base.CreateLogonWindowObjectSpace(null);
		}
		public Boolean CreateTestLayoutManagerByDefault {
            get { return createTestLayoutManagerByDefault; }
            set { createTestLayoutManagerByDefault = value; }
        }
        public new Boolean SupportMasterDetailMode {
            get { return base.SupportMasterDetailMode; }
        }
        public DatabaseUpdaterBase CreateDatabaseUpdater_Result;
		public Boolean CreateDatabaseUpdater_AllowBase = true;
        public Type ModuleInfoType {
            get {
				if(objectSpaceProviders.Count > 0) {
					return objectSpaceProviders[0].ModuleInfoType;
				}
				else {
					return null;
				}
			}
        }
		public Boolean IsObjectSpaceProviderOwner {
			get { return isObjectSpaceProviderOwner; }
		}
    }

	public class CustomDatabaseUpdater : DatabaseUpdater {
		public int CheckCompatibility_CallCount = 0;

		public CustomDatabaseUpdater(IObjectSpaceProvider objectSpaceProvider, IList<ModuleBase> modules, String applicationName)
			: base(objectSpaceProvider, modules, applicationName, null) {
		}
		public override CompatibilityError CheckCompatibility() {
			CheckCompatibility_CallCount++;
			return null;
        }
	}

	public class CustomDatabaseUpdater_Q484423 : DatabaseUpdater {
        IList<IModuleInfo> versionInfoListForTest = new List<IModuleInfo>();

        public CustomDatabaseUpdater_Q484423(IObjectSpaceProvider objectSpaceProvider, IList<ModuleBase> modules, String applicationName)
            : base(objectSpaceProvider, modules, applicationName, null) {
                CustomLoadVersionInfoList += new EventHandler<CustomLoadVersionInfoListEventArgs>(CustomDatabaseUpdater_Q484423_CustomLoadVersionInfoList);
        }

        void CustomDatabaseUpdater_Q484423_CustomLoadVersionInfoList(object sender, CustomLoadVersionInfoListEventArgs e) {
            e.ModuleInfoList = versionInfoListForTest;
            e.Handled = true;
        }
        public IList<IModuleInfo> VersionInfoListForTest {
            get { return versionInfoListForTest; }
            set { versionInfoListForTest = value; }
        }
    }

    public class TestingWindowController : WindowController {
    }

    public class TestingViewController : ViewController {
    }

	public class TestingActionsCriteriaModificationsController : ModificationsController {
        public TestingActionsCriteriaModificationsController() {
            SimpleAction action = new SimpleAction(this, "A", "ObjectsCreation", null);
            action.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            RegisterActions(action);
        }
        protected override void OnViewChanged() {
            base.OnViewChanged();
            Active["ObjectType"] =
                (((DetailView)View).ObjectTypeInfo.Type == typeof(TestObject))
                ||
                (((DetailView)View).ObjectTypeInfo.Type == typeof(TinyTestObject));
        }
        protected override void OnActivated() {
            base.OnActivated();
            if(View.CurrentObject is TinyTestObject) {
                Actions["A"].TargetObjectsCriteria = (new BinaryOperator("Name", "aaaaa")).ToString();
            }
            else {
                Actions["A"].TargetObjectsCriteria = (new BinaryOperator("TestObjectName", "bbbbb")).ToString();
            }
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            Actions["A"].TargetObjectsCriteria = "";
        }
    }

    public class TestingActionsCriteriaViewController : ActionsCriteriaViewController {
		protected override ICollection<ActionBase> CollectAllActions() {
			if(CollectAllActions_Result != null) {
				return CollectAllActions_Result;
			}
			else {
				return base.CollectAllActions();
			}
		}
		protected override void UpdateAction(ActionBase action, String criteria) {
			base.UpdateAction(action, criteria);
			UpdateAction_CallCount++;
			if(ActionUpdating != null) {
				ActionUpdating(this, EventArgs.Empty);
			}

		}
		protected override void UpdateActionState() {
			base.UpdateActionState();
			UpdateActionState_CallCount++;
		}
		public Int32 UpdateAction_CallCount;
		public Int32 UpdateActionState_CallCount;
		public IList<ActionBase> CollectAllActions_Result;
		public event EventHandler ActionUpdating;
	}

	[Persistent]
    public class GuidKeyObject {
        [Key(true)]
        public Guid Key;
    }
	[Persistent]
    public class IntKeyObject {
        [Key(true)]
        public int Key;
    }
    
	[Persistent]
    public class SupportEvent : XPObject {
        private string subject = "";
        public SupportEvent(Session session) : base(session) { }
        public string Subject {
            get { return subject; }
            set { subject = value; }
        }
    }

    [Persistent]
    public class CreateViewBaseClass : XPObject {
        public CreateViewBaseClass(Session session) : base(session) { }
    }
    [Persistent]
    public class CreateViewDescendant1 : CreateViewBaseClass {
        public CreateViewDescendant1(Session session) : base(session) { }
    }
    [Persistent]
    public class CreateViewDescendant2 : CreateViewBaseClass {
        public CreateViewDescendant2(Session session) : base(session) { }
    }

    public class TestASPSessionValueManager {
        public string TargetValueKey = Guid.NewGuid().ToString();
        public static string CurrentSessionID;
        public static LightDictionary<string, LightDictionary<string, object>> Values = new LightDictionary<string, LightDictionary<string, object>>();
    }
    public class TestASPSessionValueManager<ValueType> : TestASPSessionValueManager, IValueManager<ValueType> {
        public TestASPSessionValueManager(string key) {
        }
        public ValueType Value {
            get {
                if(Values[CurrentSessionID] == null) {
                    Values[CurrentSessionID] = new LightDictionary<string, object>();
                }
                return (ValueType)Values[CurrentSessionID][TargetValueKey];
            }
            set {
                if(Values[CurrentSessionID] == null) {
                    Values[CurrentSessionID] = new LightDictionary<string, object>();
                }
                Values[CurrentSessionID][TargetValueKey] = value;
            }
        }
        public void Clear() {
            Value = default(ValueType);
        }
        public bool CanManageValue {
            get { return true; }
        }
    }

	public class SharedModules_SetupWithApplicationCalls_Module : TestModuleBase {
        public override void Setup(XafApplication application) {
            base.Setup(application);
            if(SetupWithApplication != null) {
                SetupWithApplication(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> SetupWithApplication;
    }

    public class TestSecurityModule : ModuleBase {
		protected internal override IEnumerable<Type> GetRegularTypes() {
			return null;
		}
	}

	[DomainComponent]
	[Browsable(false)]
	public class InvisibleDomainClass {
	}

    [TestFixture]
    public class XafApplicationTests : BaseDBTest {
		private Int32 application_ObjectSpaceCreated_CallCount;
		private IObjectSpaceProvider objectSpaceProviderResult;
		private Boolean isObjectSpaceProviderOwnerResult;
		private void SetCulture(String cultureName) {
			CultureInfo culture = new System.Globalization.CultureInfo(cultureName);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}
		private void Application_ObjectSpaceCreated(Object sender, ObjectSpaceCreatedEventArgs e) {
			application_ObjectSpaceCreated_CallCount++;
		}
        private void Application_DetailViewCreating(Object sender, DetailViewCreatingEventArgs e) {
            e.ViewID = "B";
        }
        private void Application_CreateCustomCollectionSource(Object sender, CreateCustomCollectionSourceEventArgs e) {
            CollectionSourceBase result = new CollectionSource(e.ObjectSpace, typeof(TestObject));
            e.CollectionSource = result;
        }
		private void Application_CreateCustomObjectSpaceProvider(Object sender, CreateCustomObjectSpaceProviderEventArgs e) {
			e.ObjectSpaceProviders.Add(objectSpaceProviderResult);
			e.IsObjectSpaceProviderOwner = isObjectSpaceProviderOwnerResult;
		}
		
		public override void SetUp() {
            base.SetUp();
            ((IModelSources)modelApplication).EditorDescriptors =
                new EditorDescriptors(new EditorDescriptor[] {
					new ListEditorDescriptor(new AliasAndEditorTypeRegistration("Default", typeof(Object), true, typeof(TestListEditor), true)),
                    new PropertyEditorDescriptor(new AliasAndEditorTypeRegistration("Default", typeof(Object), true, typeof(TestStringPropertyEditor), true))
				});
        }
        [Test]
        public void TestsHelperTest() {
            Assert.AreEqual(EasyTestTagHelper.TestAction + "=test_action", EasyTestTagHelper.FormatTestAction("test_action"));
            Assert.AreEqual(EasyTestTagHelper.TestField + "=test_field", EasyTestTagHelper.FormatTestField("test_field"));
            Assert.AreEqual(EasyTestTagHelper.TestAction + "=test_action", EasyTestTagHelper.FormatTestAction("test_action"));
            Assert.AreEqual(EasyTestTagHelper.TestTable + "=test_table", EasyTestTagHelper.FormatTestTable("test_table"));
        }
        [Test]
        public void TestFindViewsID() {
            //DictionaryNode info = new DictionaryXmlReader().ReadFromString(
            RegisterTypesForModel(typeof(SupportEvent));
            ReadDiffsFromXmlString(
                @"<Application>" +
                @"	<BOModel>" +
                @"		<Class Name=""DevExpress.ExpressApp.Tests.SupportEvent"" DefaultListView=""ListView"" DefaultDetailView=""DetailView"" DefaultLookupListView=""LookupListView""/>" +
                @"	</BOModel>" +
                @"	<Views>" +
                @"		<ListView ID=""ListView"" ClassName=""DevExpress.ExpressApp.Tests.SupportEvent"" IsNewNode=""True""/>" +
                @"		<ListView ID=""LookupListView"" ClassName=""DevExpress.ExpressApp.Tests.SupportEvent"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""DetailView"" ClassName=""DevExpress.ExpressApp.Tests.SupportEvent"" IsNewNode=""True""/>" +
                @"	</Views>" +
                @"</Application>");
            //XafApplication application = new TestApplication(this, new Dictionary(info));
            XafApplication application = new TestApplication(modelApplication);
            Assert.AreEqual("ListView", application.FindListViewId(typeof(SupportEvent)));
            Assert.AreEqual("DetailView", application.FindDetailViewId(typeof(SupportEvent)));
            Assert.AreEqual("LookupListView", application.FindLookupListViewId(typeof(SupportEvent)));
        }
        [Test]
        public void TestCreateViewById() {
            RegisterTypesForModel(typeof(TestObject));
            UseClass<TestObject>();
            ReadDiffsFromXmlString(String.Format(
                @"<Application>" +
                @"	<Views>" +
                @"		<ListView ID=""View1"" ClassName=""{0}"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""View2"" ClassName=""{0}"" IsNewNode=""True""/>" +
                @"	</Views>" +
                @"</Application>", typeof(TestObject).FullName));

            CollectionSource dataSource = new CollectionSource(ObjectSpace, typeof(TestObject));
            XafApplication application = new TestApplication(this, modelApplication);

            View listView = application.CreateListView("View1", dataSource, true);
            Assert.AreEqual("View1", listView.Id);
            Assert.IsTrue(listView.IsRoot);
            Assert.AreEqual(dataSource, ((ListView)listView).CollectionSource);
            Assert.IsFalse(((XPBaseCollection)dataSource.List).IsLoaded);

            DetailView detailView = application.CreateDetailView(ObjectSpace, "View2", false, ObjectSpace.CreateObject<TestObject>());
            Assert.AreEqual("View2", detailView.Id);
            Assert.IsFalse(detailView.IsRoot);
            Assert.IsNotNull(detailView.CurrentObject);
            Assert.IsFalse(((XPBaseCollection)dataSource.List).IsLoaded);
        }
        [Test]
        public void TestProcessShortcut1() {
            RegisterTypesForModel(typeof(TestObject));
            TestObject testObject = new TestObject(Session);
            testObject.Save();

            ReadDiffsFromXmlString(@"
				<Application>
					<BOModel>
						<Class Name=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" DefaultDetailView=""DetailView"" />
					</BOModel>
					<Views>
						<DetailView ID=""DetailView"" ClassName=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" IsNewNode=""True""/>
					</Views>
				</Application>");

            ViewShortcut shortcut = new ViewShortcut(typeof(TestObject), testObject.Oid.ToString(), /*DetailViewInfoNodeWrapper.NodeName*/"DetailView");
            XafApplication application = new TestApplication(this, modelApplication);
            DetailView detailView = (DetailView)application.ProcessShortcut(shortcut);
            Assert.AreEqual(detailView.CurrentObject, detailView.ObjectSpace.GetObject(testObject));

            shortcut = new ViewShortcut(typeof(TestObject), testObject.Oid.ToString(), /*DetailViewInfoNodeWrapper.NodeName*/"DetailView");
            application = new TestApplication(this, modelApplication);
            detailView = (DetailView)application.ProcessShortcut(shortcut);
            Assert.AreEqual(detailView.CurrentObject, detailView.ObjectSpace.GetObject(testObject));
        }
        [Test]
        public void TestCreateDetailView1() {
            TestObject testObject = ObjectSpace.CreateObject<TestObject>();
            testObject.Save();
            ObjectSpace.CommitChanges();
            ReadDiffsFromXmlString(@"
				<Application>
					<BOModel>
						<Class Name=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" DefaultDetailView=""DetailViewB""  IsNewNode=""True"" />
					</BOModel>
					<Views>
						<DetailView ID=""DetailViewA"" ClassName=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" IsNewNode=""True""/>
						<DetailView ID=""DetailViewB"" ClassName=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" IsNewNode=""True""/>
						<DetailView ID=""DetailViewC"" ClassName=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" IsNewNode=""True""/>
					</Views>
				</Application>");
            XafApplication application = new TestApplication(this, modelApplication);

            DetailView detailView = application.CreateDetailView(ObjectSpace, testObject);
            Assert.AreEqual("DetailViewB", detailView.Id);

            IObjectSpace objectSpace = CreateObjectSpace();
            detailView = application.CreateDetailView(objectSpace, "DetailViewC", true, objectSpace.GetObject(testObject));
            Assert.AreEqual("DetailViewC", detailView.Id);
        }
        [Test]
        public void TestCreateDetailViewWithoutObjectKey() {
            RegisterTypesForModel(typeof(TestObject));
            ReadDiffsFromXmlString(@"
				<Application>
					<BOModel>
						<Class Name=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" DefaultDetailView=""DetailViewA"" />
					</BOModel>
					<Views>
						<DetailView ID=""DetailViewA"" ClassName=""DevExpress.ExpressApp.Tests.TestObjects.TestObject"" IsNewNode=""True""/>
					</Views>
				</Application>");
            XafApplication application = new TestApplication(this, modelApplication);
            View detailView = application.ProcessShortcut(new ViewShortcut(typeof(TestObject), null, "DetailViewA"));
            Assert.IsNotNull(detailView);
            Assert.IsNull(detailView.CurrentObject);

            TestObject testObject = ObjectSpace.CreateObject<TestObject>();
            testObject.Save();
            ObjectSpace.CommitChanges();

            detailView = application.ProcessShortcut(new ViewShortcut(typeof(TestObject), null, "DetailViewA"));
            Assert.IsNotNull(detailView);
            Assert.AreEqual(testObject, ObjectSpace.GetObject(detailView.CurrentObject));

            testObject = ObjectSpace.CreateObject<TestObject>();
            testObject.Save();
            ObjectSpace.CommitChanges();
            try {
                detailView = application.ProcessShortcut(new ViewShortcut(typeof(TestObject), null, "DetailViewA"));
                Assert.Fail();
            } catch(Exception e) {
                Assert.IsTrue(e.InnerException.Message.Contains("there are '2' objects to show"));
            }
        }
        [Test]
        public void TestCreateDetailViewWithInvalidKey() {
            RegisterTypesForModel(typeof(TestObject));

            TestObject testObject = ObjectSpace.CreateObject<TestObject>();
            testObject.Save();
            ObjectSpace.CommitChanges();
            string invalidObjectKey = "-1000";

            XafApplication application = new TestApplication(this, modelApplication);
            ViewShortcut shortcut = new ViewShortcut(typeof(TestObject), invalidObjectKey, application.GetDetailViewId(typeof(TestObject)));
            try {
                View detailView = application.ProcessShortcut(shortcut);
                Assert.Fail();
            }
            catch(Exception e) {
                Assert.IsTrue(e.Message.Contains(@"Requested object is not found"));
            }
        }
        [Test]
        public void TestCreateDetailViewWithEditMode() {
            DetailView detailView;
            RegisterTypesForModel(typeof(TestObject));
            XafApplication application = new TestApplication(this, modelApplication);

            TestObject testObject = ObjectSpace.CreateObject<TestObject>();
            ObjectSpace.CommitChanges();

            ViewShortcut shortcut = new ViewShortcut("TestObject_DetailView", testObject.Oid.ToString());
            detailView = application.ProcessShortcut(shortcut) as DetailView;
            Assert.IsNotNull(detailView);
            Assert.AreEqual(ViewEditMode.View, detailView.ViewEditMode);

            shortcut[DetailView.ViewEditModeKeyName] = "View";
            detailView = application.ProcessShortcut(shortcut) as DetailView;
            Assert.IsNotNull(detailView);
            Assert.AreEqual(ViewEditMode.View, detailView.ViewEditMode);

            shortcut[DetailView.ViewEditModeKeyName] = "Edit";
            detailView = application.ProcessShortcut(shortcut) as DetailView;
            Assert.IsNotNull(detailView);
            Assert.AreEqual(ViewEditMode.Edit, detailView.ViewEditMode);
        }
        [Test]
        public void TestSelectProperDetailView() {
            UseClass<CreateViewBaseClass>();
            ReadDiffsFromXmlString(
                @"<Application>" +
                @"	<BOModel>" +
                @"		<Class Name=""DevExpress.ExpressApp.Tests.CreateViewBaseClass"" DefaultDetailView=""BaseClass_DetailView"" IsNewNode=""True""/>" +
                @"		<Class Name=""DevExpress.ExpressApp.Tests.CreateViewDescendant1"" DefaultDetailView=""Descendant1_DetailView"" IsNewNode=""True""/>" +
                @"		<Class Name=""DevExpress.ExpressApp.Tests.CreateViewDescendant2"" DefaultDetailView=""Descendant2_DetailView"" IsNewNode=""True""/>" +
                @"	</BOModel>" +
                @"	<Views >" +
                @"		<ListView ID=""BaseClass_ListView"" ClassName=""DevExpress.ExpressApp.Tests.CreateViewBaseClass"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""BaseClass_DetailView"" ClassName=""DevExpress.ExpressApp.Tests.CreateViewBaseClass"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""BaseClass_Custom_DetailView"" ClassName=""DevExpress.ExpressApp.Tests.CreateViewBaseClass"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""Descendant1_DetailView"" ClassName=""DevExpress.ExpressApp.Tests.CreateViewDescendant1"" IsNewNode=""True""/>" +
                @"		<DetailView ID=""Descendant2_DetailView"" ClassName=""DevExpress.ExpressApp.Tests.CreateViewDescendant2"" IsNewNode=""True""/>" +
                @"	</Views>" +
                @"</Application>");

            TestApplication manager = new TestApplication(this, modelApplication);
            ListView listView = new ListView(new CollectionSource(ObjectSpace, typeof(CreateViewBaseClass)), new TestListEditor(null));
            listView.DetailViewId = "BaseClass_Custom_DetailView";


            IObjectSpace objectSpace = CreateObjectSpace();
            DetailView detailView = manager.CreateDetailView(objectSpace, objectSpace.CreateObject<CreateViewBaseClass>());
            Assert.AreEqual("BaseClass_DetailView", detailView.Id);

            objectSpace = CreateObjectSpace();
            detailView = manager.CreateDetailView(objectSpace, objectSpace.CreateObject<CreateViewBaseClass>(), listView);
            Assert.AreEqual("BaseClass_Custom_DetailView", detailView.Id);

            objectSpace = CreateObjectSpace();
            detailView = manager.CreateDetailView(objectSpace, objectSpace.CreateObject<CreateViewDescendant1>(), listView);
            Assert.AreEqual("Descendant1_DetailView", detailView.Id);

            objectSpace = CreateObjectSpace();
            detailView = manager.CreateDetailView(objectSpace, objectSpace.CreateObject<CreateViewDescendant2>(), listView);
            Assert.AreEqual("Descendant2_DetailView", detailView.Id);
        }
        [Test]
        public void TestListViewWithoutControl() {
            RegisterTypesForModel(typeof(TestObject));
            ReadDiffsFromXmlString(
                @"<Application>
					<Views>
						<ListView ID=""ListView1"" ClassName=""" + typeof(TestObject) + @""" IsNewNode=""True"">
							<Columns>
								<ColumnInfo PropertyName=""StrProperty"" />
							</Columns>
						</ListView>
					</Views>
				</Application>"
                );
            TestApplication application = new TestApplication(new XPObjectSpaceProvider(this));
            application.SetModelApplication(modelApplication);

            TestListEditor.CreateControlsCount = 0;
            ListView lv = application.CreateListView(
                "ListView1", new CollectionSource(application.CreateObjectSpace(), typeof(TestObject)), true);
            Assert.AreEqual(0, TestListEditor.CreateControlsCount);
        }
        [Test]
        public void TestSetThreadUICulture() {
            CultureInfo savedCultureInfo = Thread.CurrentThread.CurrentCulture;
            CultureInfo savedUICultureInfo = Thread.CurrentThread.CurrentUICulture;
            try {
                TestApplication app = new TestApplication();
                SetCulture("en-US");
                app.InitializeCulturePublic("ru", "");
                Assert.AreEqual("ru", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

                SetCulture("en-US");
                app.InitializeCulturePublic("ru-RU", "");
                Assert.AreEqual("ru", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

                SetCulture("en-US");
                app.InitializeCulturePublic("ru-RU", CaptionHelper.UserLanguage);
                Assert.AreEqual("ru", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

                SetCulture("en-US");
                app.InitializeCulturePublic("ru-RU", CaptionHelper.DefaultLanguage);
                Assert.AreEqual(CultureInfo.InvariantCulture.TwoLetterISOLanguageName, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

                SetCulture("en-US");
                app.InitializeCulturePublic("ru-RU", "de-DE");
                Assert.AreEqual("de", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            }
            finally {
                Thread.CurrentThread.CurrentCulture = savedCultureInfo;
                Thread.CurrentThread.CurrentUICulture = savedUICultureInfo;
            }
        }
        [Test]
        public void TestCreateDetailView2() {
            UseClass<BugReport>();
            IObjectSpace objectSpace1 = CreateObjectSpace();
            IObjectSpace objectSpace2 = CreateObjectSpace();
            ListView sourceView = new ListView(new CollectionSource(objectSpace1, typeof(BugReport)), new TestListEditor(), true);

            ReadDiffsFromXmlString(String.Format(@"
				<Application>
					<BOModel>
			    		<Class Name=""{0}"" DefaultDetailView=""A""  IsNewNode=""True"" />
					</BOModel>
					<Views>
						<DetailView ID=""A"" ClassName=""{0}"" IsNewNode = ""True""/>
					</Views>
				</Application>", typeof(Technology).FullName));

            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);

            DetailView detailView = application.CreateDetailView(objectSpace1, objectSpace1.CreateObject<Technology>(), sourceView);
            Assert.IsFalse(detailView.IsRoot);

            detailView = application.CreateDetailView(objectSpace2, objectSpace2.CreateObject<Technology>(), sourceView);
            Assert.IsTrue(detailView.IsRoot);
        }
        [Test]
        public void TestCreateAnotherDetailViewByViewId() {
            UseClass<BugReport>();
            IObjectSpace objectSpace1 = CreateObjectSpace();
            IObjectSpace objectSpace2 = CreateObjectSpace();
            ListView sourceView = new ListView(new CollectionSource(objectSpace1, typeof(BugReport)), new TestListEditor(), true);

            ReadDiffsFromXmlString(String.Format(@"
				<Application>
					<BOModel>
			    		<Class Name=""{0}"" DefaultDetailView=""A""  IsNewNode=""True"" />
					</BOModel>
					<Views>
						<DetailView ID=""A"" ClassName=""{0}"" IsNewNode = ""True""/>
						<DetailView ID=""B"" ClassName=""{0}"" IsNewNode = ""True""/>
					</Views>
				</Application>", typeof(Technology).FullName));

            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            application.DetailViewCreating += new EventHandler<DetailViewCreatingEventArgs>(Application_DetailViewCreating);

            DetailView detailView = application.CreateDetailView(objectSpace1, objectSpace1.CreateObject<Technology>(), sourceView);
            Assert.AreEqual("B", detailView.Id);
        }
        [Test]
        public void TestObjectProviderDispose() {
            string connectionString = "Integrated Security=SSPI;Pooling=false;Data Source=(local);Initial Catalog=" + SqlServerDBName;
            XafApplicationForTests application = new XafApplicationForTests();
            application.Setup("App With Default ObjectSpaceProvider", connectionString, new string[0] { });
            XPObjectSpaceProvider objectSpaceProvider = application.ObjectSpaceProvider as XPObjectSpaceProvider;
            Assert.IsNotNull(objectSpaceProvider);
            application.Dispose();
            Assert.IsTrue(objectSpaceProvider.IsDisposed);

            using(XPObjectSpaceProvider createdObjectSpaceProvider = new XPObjectSpaceProvider(new ConnectionStringDataStoreProvider(connectionString), true)) {
                application = new XafApplicationForTests();
                application.Setup("App With created ObjectSpaceProvider", createdObjectSpaceProvider);
                application.Dispose();
                Assert.IsFalse(createdObjectSpaceProvider.IsDisposed);

                application = new XafApplicationForTests();
                application.Setup("App With create via event ObjectSpaceProvider", connectionString, new string[0] { });
                application.CreateCustomObjectSpaceProvider += delegate(object sender, CreateCustomObjectSpaceProviderEventArgs args) {
                    args.ObjectSpaceProvider = createdObjectSpaceProvider;
                };
                application.Dispose();
                Assert.IsFalse(createdObjectSpaceProvider.IsDisposed);
            }

        }
        [Test] // B36234
        public void TestTitle() {
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);

            modelApplication.Title = "";
            application.Title = "";
            Assert.AreEqual("", application.Title);

            modelApplication.Title = "A";
            application.Title = "";
            Assert.AreEqual("A", application.Title);

            modelApplication.Title = "";
            application.Title = "B";
            Assert.AreEqual("B", application.Title);

            modelApplication.Title = "A";
            application.Title = "B";
            Assert.AreEqual("B", application.Title);

            modelApplication.Title = "";
            application.Title = "";
            Assert.AreEqual("", application.Title);
        }
        [Test] // B134558
        public void Test_CreateListView1() {
            RegisterTypesForModel(typeof(TestObject));
            UseClass<TestObject>();
            ReadDiffsFromXmlString(String.Format(@"
				<Application>
					<BOModel>
			    		<Class Name=""{0}"" DefaultListView=""A""  IsNewNode=""True"" />
					</BOModel>
					<Views>
						<ListView ID=""A"" ClassName=""{0}"" EditorInfo=""Default""  IsNewNode=""True"" />
					</Views>
				</Application>", typeof(TestObject).FullName));

            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            ListView listView = application.CreateListView(ObjectSpace, typeof(TestObject), true);
            Assert.IsNotNull(listView);
        }
		[Test]
		public void Test_CreateListView2() {
			RegisterTypesForModel(typeof(TestObject));
			XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
			ListView listView = application.CreateListView(typeof(TestObject), true);
			Assert.IsNotNull(listView);
			Assert.IsNotNull(listView.ObjectSpace);
		}
		[Test]
		public void Test_CreateDetailView() {
			RegisterTypesForModel(typeof(TestClassA));
			XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);

			IObjectSpace objectSpace = new NonPersistentObjectSpace(TypesInfo);
			Object obj = new TestClassA(objectSpace);
			DetailView detailView = application.CreateDetailView(obj);
			Assert.IsNotNull(detailView);
			Assert.AreEqual(objectSpace, detailView.ObjectSpace);

			objectSpace = new NonPersistentObjectSpace(TypesInfo);
			obj = new TestClassA(objectSpace);
			detailView = application.CreateDetailView(obj, true);
			Assert.IsNotNull(detailView);
			Assert.AreEqual(objectSpace, detailView.ObjectSpace);
		}
		[Test] // B155130
        public void TestCreateListViewByIncorrectID() {
            RegisterTypesForModel(typeof(TestObject));
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            try {
                application.CreateListView("dummy", new CollectionSource(ObjectSpace, typeof(TestObject)), true);
                Assert.Fail("Exception is expected!");
            }
            catch(Exception e) {
                Assert.IsTrue(e.Message.Contains("node was not found"));
            }
            try {
                application.CreateListView(GetDetailView<TestObject>().Id, new CollectionSource(ObjectSpace, typeof(TestObject)), true);
                Assert.Fail("Exception is expected!");
            }
            catch(Exception e) {
                Assert.IsTrue(e.Message.Contains("'IModelDetailView' node was passed"));
                Assert.IsTrue(e.Message.Contains("'IModelListView' node was expected"));
            }
        }
        [Test]//B143527
        public void TestCreateTemplate() {
            TestApplication application = new TestApplication();
            //
            //Test default template.
            bool fired = false;
            application.DefaultTemplate = new TestFrameTemplate();
            application.CustomizeTemplate += delegate(object sender, CustomizeTemplateEventArgs e) {
                fired = true;
                Assert.IsNotNull(e.Template);
            };
            Assert.AreSame(application.DefaultTemplate, application.CreateTemplate("Default template should be created and the CustomizeTemplate event should fire"));
            Assert.IsTrue(fired);
            //
            //Test custom template.
            fired = false;
            IFrameTemplate customTemplate = new TestFrameTemplate();
            application.CreateCustomTemplate += delegate(object sender, CreateCustomTemplateEventArgs e) {
                e.Template = customTemplate;
            };
            Assert.AreSame(customTemplate, application.CreateTemplate("Custom template should be created and the CustomizeTemplate event should fire"));
            Assert.IsTrue(fired);
        }
        [Test]
        public void TestApplicationMustKeepSynchronizedAspectAndAspectIndex() {
            XafApplicationForTests application1 = new XafApplicationForTests();
            application1.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application1.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            XafApplicationForTests application2 = new XafApplicationForTests();
            application2.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application2.Setup(new ExpressApplicationSetupParameters("Test application", new XPObjectSpaceProvider(this),
                new ControllersManager(), /*new PropertyEditorsFactory(), *//*new Dictionary(), */application2.Modules));
            application1.Logon();
            application2.Logon();
            foreach(XafApplicationForTests application in new XafApplicationForTests[] { application1, application2 }) {
                application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
                Assert.AreEqual(true, object.ReferenceEquals(application.CurrentAspectProvider, ((ModelApplicationBase)application.Model).CurrentAspectProvider));
                //ICurrentAspectProvider currentAspectProvider = application.CurrentAspectProvider;
                ((ModelApplicationBase)application.Model).AddAspect("ru");
                ((ModelApplicationBase)application.Model).AddAspect("en");
                ModelApplicationBase modelApplication = (ModelApplicationBase)application.Model;
                //application.Model.Aspects.Add("ru");
                //application.Model.Aspects.Add("en");

                application.CurrentAspectProvider.CurrentAspect = "ru";
                //Assert.AreEqual(1, application.Model.CurrentAspectIndex);
                Assert.AreEqual("ru", modelApplication.CurrentAspect);
                Assert.AreEqual("ru", application.CurrentAspectProvider.CurrentAspect.Substring(0, 2));
                application.CurrentAspectProvider.CurrentAspect = "en";
                //Assert.AreEqual(2, modelApplication.CurrentAspectIndex);
                Assert.AreEqual("en", modelApplication.CurrentAspect);
                Assert.AreEqual("en", application.CurrentAspectProvider.CurrentAspect.Substring(0, 2));
                application.CurrentAspectProvider.CurrentAspect = "fr";
                //Assert.AreEqual(0, modelApplication.CurrentAspectIndex);
            }
        }
        [Test]
        public void TestDictionaryAndModelApplicationMustHaveSynchronizedAspects() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.Setup("app1", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            application.Logon();
            //Dictionary model = application.Model;
            ModelApplicationBase modelApplication = (ModelApplicationBase)application.Model;
            ICurrentAspectProvider currentAspectProvider = application.CurrentAspectProvider;
            modelApplication.AddAspect("ru");
            modelApplication.AddAspect("en");

            currentAspectProvider.CurrentAspect = "ru";
            Assert.AreEqual("ru", modelApplication.CurrentAspect);

            currentAspectProvider.CurrentAspect = "en";
            Assert.AreEqual("en", modelApplication.CurrentAspect);

            currentAspectProvider.CurrentAspect = "fr";
            Assert.AreEqual(string.Empty, modelApplication.CurrentAspect);
        }
        [Test] // S33920
        public void TestCollectionSourceCreatingEvent() {
            UseClass<TestObject>();
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
            CollectionSourceBase collectionSource = application.CreateCollectionSource(ObjectSpace, null, null);
            Assert.AreEqual(typeof(TestObject), collectionSource.ObjectTypeInfo.Type);
        }
        [Test]
        public void TestSharedModules_SecurityIsNotShared() {
            ValueManager.Reset();
            ValueManager.Clear();
            ValueManager.ValueManagerType = typeof(TestASPSessionValueManager<>).GetGenericTypeDefinition();

            SharedTestApplication.ClearCustomizeModules();
            SharedTestApplication.CustomizeApplication += delegate(object sender, EventArgs e) {
                ((SharedTestApplication)sender).Security = new EmptySecuritySystemTest();
            };

            TestASPSessionValueManager.CurrentSessionID = "user1";

            SharedTestApplication.dataStoreProvider = this;

            string securityInstanceKey = ((TestASPSessionValueManager)ValueManager.GetValueManager<ISecurityStrategyBase>("SecuritySystem_ISecurityStrategyBase")).TargetValueKey;

            SharedTestApplication user1Application = new SharedTestApplication();
            user1Application.Setup();
            Assert.IsNotNull(user1Application.Security);
            Assert.AreEqual(user1Application.Security, SecuritySystem.Instance);

            TestASPSessionValueManager.CurrentSessionID = "user2";
            SharedTestApplication user2Application = new SharedTestApplication();
            user2Application.Setup();
            Assert.AreEqual(user2Application.Security, SecuritySystem.Instance);

            Assert.IsTrue(user1Application.Security != user2Application.Security);

            Assert.AreEqual(user1Application.Security, TestASPSessionValueManager.Values["user1"][securityInstanceKey]);
            Assert.AreEqual(user2Application.Security, TestASPSessionValueManager.Values["user2"][securityInstanceKey]);
        }
        [Test]
        public void TestSharedModules_SetupWithApplicationCalls() {
            ValueManager.Reset();
            ValueManager.ValueManagerType = typeof(TestASPSessionValueManager<>).GetGenericTypeDefinition();

            List<object> setupWithApplicationCalls = new List<object>();
            SharedTestApplication.ClearCustomizeModules();
            SharedTestApplication.CustomizeApplication += delegate(object sender, EventArgs e) {
                SharedModules_SetupWithApplicationCalls_Module module = new SharedModules_SetupWithApplicationCalls_Module();
                module.SetupWithApplication += delegate(object sender1, EventArgs e1) {
                    setupWithApplicationCalls.Add(((SharedModules_SetupWithApplicationCalls_Module)sender1).Application);
                };
                ((SharedTestApplication)sender).Modules.Add(module);
            };

            TestASPSessionValueManager.CurrentSessionID = "user1";

            SharedTestApplication.dataStoreProvider = this;
            SharedTestApplication user1Application = new SharedTestApplication();
            user1Application.Setup();
            Assert.AreEqual(1, setupWithApplicationCalls.Count);
            Assert.AreEqual(user1Application, setupWithApplicationCalls[0]);

            SharedTestApplication user2Application = new SharedTestApplication();
            user1Application.Setup();
            Assert.AreEqual(2, setupWithApplicationCalls.Count);
            Assert.AreEqual(user2Application, setupWithApplicationCalls[1]);
        }
        [Test]
        public void TestCreateCollectionSource() {
            RegisterTypesForModel(typeof(TestObject));
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            IModelListView modelListView = modelApplication.Views.GetDefaultListView<TestObject>();

            application.DefaultCollectionSourceMode = CollectionSourceMode.Normal;

            // The application.DefaultCollectionSourceMode is ignored in this CreateCollectionSource overload.

            CollectionSourceBase collectionSource =
                application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>(), true, CollectionSourceMode.Proxy);
            // The collectionSource.Mode is always Normal in server mode.
            Assert.AreEqual(CollectionSourceMode.Normal, collectionSource.Mode);
            Assert.IsTrue(collectionSource.Collection is XPServerCollectionSource);

            collectionSource = application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>(), false, CollectionSourceMode.Proxy);
            Assert.IsTrue(collectionSource.Collection is ProxyCollection);
            ProxyCollection proxyCollection = (ProxyCollection)collectionSource.Collection;
            Assert.IsTrue(proxyCollection.OriginalCollection is XPCollection);

            // The application.DefaultCollectionSourceMode is ignored in this CreateCollectionSource overload.

            modelListView.UseServerMode = true;
            collectionSource = application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>(), CollectionSourceMode.Proxy);
            // The collectionSource.Mode is always Normal in server mode.
            Assert.AreEqual(CollectionSourceMode.Normal, collectionSource.Mode);
            Assert.IsTrue(collectionSource.Collection is XPServerCollectionSource);

            modelListView.UseServerMode = false;
            collectionSource = application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>(), CollectionSourceMode.Proxy);
            Assert.IsTrue(collectionSource.Collection is ProxyCollection);
            proxyCollection = (ProxyCollection)collectionSource.Collection;
            Assert.IsTrue(proxyCollection.OriginalCollection is XPCollection);

            // The application.DefaultCollectionSourceMode is taken into account in this CreateCollectionSource overload.

            application.DefaultCollectionSourceMode = CollectionSourceMode.Proxy;
            modelListView.UseServerMode = true;
            collectionSource = application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>());
            // The collectionSource.Mode is always Normal in server mode.
            Assert.AreEqual(CollectionSourceMode.Normal, collectionSource.Mode);
            Assert.IsTrue(collectionSource.Collection is XPServerCollectionSource);

            modelListView.UseServerMode = false;
            collectionSource = application.CreateCollectionSource(ObjectSpace, typeof(TestObject), ModelHelper.GetListViewId<TestObject>());
            Assert.IsTrue(collectionSource.Collection is ProxyCollection);
            proxyCollection = (ProxyCollection)collectionSource.Collection;
            Assert.IsTrue(proxyCollection.OriginalCollection is XPCollection);
        }
        [Test] // B147510
        public void TestChangeCurrentAspectProvider() {
            RegisterTypesForModel(typeof(TestObject));
            ((ModelApplicationBase)modelApplication).CurrentAspectProvider = new CurrentAspectProvider();

            Assert.AreEqual("Test Object", GetBOClass<TestObject>().Caption);

            ((ModelApplicationBase)modelApplication).AddAspect("ru");
            ((ModelApplicationBase)modelApplication).AddAspect("en");

            ((ModelApplicationBase)modelApplication).CurrentAspectProvider.CurrentAspect = "ru";

            GetBOClass<TestObject>().Caption = "TestRU";
            Assert.AreEqual("TestRU", GetBOClass<TestObject>().Caption);
            ICurrentAspectProvider oldAspectProvider = ((ModelApplicationBase)modelApplication).CurrentAspectProvider;
            ((ModelApplicationBase)modelApplication).CurrentAspectProvider = new CurrentAspectProvider(oldAspectProvider.CurrentAspect);
            ((ModelApplicationBase)modelApplication).CurrentAspectProvider.CurrentAspect = CaptionHelper.DefaultLanguage;

            Assert.AreEqual("Test Object", GetBOClass<TestObject>().Caption);
            ((ModelApplicationBase)modelApplication).CurrentAspectProvider = oldAspectProvider;

            Assert.AreEqual("TestRU", GetBOClass<TestObject>().Caption);
        }
        [Test] // B147513
        public void TestExtraLayers() {
            XafApplicationForTests application1 = new XafApplicationForTests();
            application1.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application1.CreateCustomUserModelDifferenceStore += new EventHandler<CreateCustomModelDifferenceStoreEventArgs>(delegate(object sender, CreateCustomModelDifferenceStoreEventArgs e) {
                e.AddExtraDiffStore("MyStore", new StringModelStore(@"<Application Title=""Extra"" Company=""Extra"" />"));
                e.Store = new StringModelStore(@"<Application Title=""User"" />");
                e.Handled = true;
            });
            application1.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            application1.Logon();
            Assert.AreEqual("User", application1.Model.Title);
            Assert.AreEqual("Extra", application1.Model.Company);
        }
        [Test] // B147513
        public void TestLogonException() {
            XafApplicationForTests application1 = new XafApplicationForTests();
            application1.DatabaseUpdateMode = DatabaseUpdateMode.Never; 
            application1.LoggedOn += delegate(object sender, LogonEventArgs e) {
                throw new Exception("application1.LoggedOn");
            };
            application1.LogonFailed += delegate(object sender, LogonFailedEventArgs e) {
                throw new Exception("application1.LogonFailed");
            };

            application1.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            try {
                application1.Logon();
            }
            catch(Exception e) {
                Assert.AreEqual("application1.LogonFailed", e.Message);
                Assert.IsNull(e.InnerException);
                Exception catchException = (Exception)e.Data[XafApplication.XafApplicationLogonCatchExceptionKey];
                Assert.AreEqual("application1.LoggedOn", catchException.Message);
            }
        }
        [Test]
        public void TestProcessShortcut2() {
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never; 
            IModelDashboardView dashboardViewModel = modelApplication.Views.AddNode<IModelDashboardView>();
            dashboardViewModel.Id = "A";

            ViewShortcut viewShortcut = new ViewShortcut();
            viewShortcut.ViewId = "A";
            DashboardView view = application.ProcessShortcut(viewShortcut) as DashboardView;
            Assert.IsNotNull(view);
            Assert.AreEqual("A", view.Id);
        }
        [Test] // Q240326
        public void TestDelayedViewItemsInitialization() {
            RegisterTypesForModel(typeof(TestObject));
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            application.CreateTestLayoutManagerByDefault = true;

            application.DelayedViewItemsInitialization = false;
            IObjectSpace objectSpace = ObjectSpace;
            TestObject obj = objectSpace.CreateObject<TestObject>();
            DetailView detailView = application.CreateDetailView(ObjectSpace, obj);
            Assert.IsFalse(detailView.DelayedItemsInitialization);
            detailView.CreateControls();
            Assert.IsNotNull(detailView.FindItem("StringProperty").Control);

            application.DelayedViewItemsInitialization = true;
            objectSpace = CreateObjectSpace();
            obj = objectSpace.CreateObject<TestObject>();
            detailView = application.CreateDetailView(objectSpace, obj);
            Assert.IsTrue(detailView.DelayedItemsInitialization);
            detailView.CreateControls();
            Assert.IsNull(detailView.FindItem("StringProperty").Control);
        }
        [Test] // Q256258
        public void TestGetObjectSpaceToShowViewFrom() {
            UseClasses(typeof(Person), typeof(Phone));
            RegisterTypesForModel(typeof(Person), typeof(Phone));

            // Create the test environment.
            IModelListView listViewModel = (IModelListView)modelApplication.Views[ModelHelper.GetListViewId<Phone>()];
            listViewModel.MasterDetailMode = MasterDetailMode.ListViewAndDetailView;
            XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.ShowViewStrategy = new TestShowViewStrategy(application);
            IObjectSpace objectSpace1 = ObjectSpace;
            Person obj = objectSpace1.CreateObject<Person>();
            IMemberInfo memberInfo = TypesInfo.FindTypeInfo(typeof(Person)).FindMember("Phones");
            PropertyCollectionSource collectionSource = new PropertyCollectionSource(objectSpace1, typeof(Person), obj, memberInfo);
            ListView listView = new ListView(collectionSource, application, false);
            listView.SetModel(listViewModel);
            // Check the test environment.
            Assert.IsTrue(application.SupportMasterDetailMode);
            Assert.IsTrue(listView.CollectionSource is PropertyCollectionSource);
            Assert.IsTrue(memberInfo.IsAggregated);
            Assert.IsNotNull(listView.EditView);

            Frame frame = new Frame(application, TemplateContext.NestedFrame);
            frame.SetView(listView);
            IObjectSpace objectSpace2 = application.GetObjectSpaceToShowDetailViewFrom(frame, null);
            Assert.AreNotEqual(objectSpace1, objectSpace2);
        }
        [Test]
        public void TestSetSecurityOnLoading() {
            XafApplication application = new XafApplicationForTests();
            application.BeginInit();
            SecuritySystemTest security = new SecuritySystemTest();
            security.GetModuleType_Result = typeof(TestSecurityModule);
            application.Security = security;
            application.EndInit();
            Assert.IsNull(application.Modules.FindModule(typeof(TestSecurityModule)));
            Assert.IsNotNull(application.Security);

            application = new XafApplicationForTests();
            SecuritySystemTest security2 = new SecuritySystemTest();
            security2.GetModuleType_Result = typeof(TestSecurityModule);
            application.Security = security2;
            Assert.IsNotNull(application.Modules.FindModule(typeof(TestSecurityModule)));
            Assert.IsNotNull(application.Security);
        }
        [Test] // B182155
        public void TestDesignTimeProperties() {
            ITypeInfo typeInfo = TypesInfo.FindTypeInfo(typeof(XafApplication));
            List<IMemberInfo> designTimeProperties = new List<IMemberInfo>();
            designTimeProperties.Add(typeInfo.FindMember("ResourcesExportedToModel"));
            designTimeProperties.Add(typeInfo.FindMember("DefaultCollectionSourceMode"));
            designTimeProperties.Add(typeInfo.FindMember("ApplicationName"));
            designTimeProperties.Add(typeInfo.FindMember("ConnectionString"));
            designTimeProperties.Add(typeInfo.FindMember("TablePrefixes"));
			designTimeProperties.Add(typeInfo.FindMember("MaxLogonAttemptCount"));
            designTimeProperties.Add(typeInfo.FindMember("CheckCompatibilityType"));
            designTimeProperties.Add(typeInfo.FindMember("EnableModelCache"));

            foreach (IMemberInfo memberInfo in designTimeProperties) {
                Assert.IsNotNull(memberInfo);
            }

            foreach(IMemberInfo memberInfo in typeInfo.Members) {
                BrowsableAttribute browsableAttribute = memberInfo.FindAttribute<BrowsableAttribute>();
                Boolean isVisible =
                    ((browsableAttribute == null) || browsableAttribute.Browsable)
                    &&
                    memberInfo.IsPublic
                    &&
                    memberInfo.IsProperty;
                if(isVisible && (designTimeProperties.IndexOf(memberInfo) < 0)) {
                    Assert.Fail(String.Format(@"The ""{0}"" member should not be visible at design time. Mark it by the [Browsable(false)] attribute.", memberInfo.Name));
                }
            }
        }
		[Test]
		public void TestCheckCompatibility_CustomDatabaseUpdater() {
			XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
			CustomDatabaseUpdater updater = new CustomDatabaseUpdater(CreateObjectSpaceProvider(), new List<ModuleBase>(), "test");
			app.CreateDatabaseUpdater_Result = updater;
			app.CreateDatabaseUpdater_AllowBase = false;
			updater.CheckCompatibility_CallCount = 0;
			app.CheckCompatibility();
			Assert.AreEqual(1, updater.CheckCompatibility_CallCount);
		}
        [Test]
        public void TestCheckCompatibility_CheckVersionForAllModules_Q484423() {
            XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
            IList<ModuleBase> modules = new List<ModuleBase>();
            TestModule testModule = new TestModule("Module1", "3.0.0.0");
            TestModule testModule2 = new TestModule("Module2", "1.0.0.0");
            modules.Add(testModule);
            modules.Add(testModule2);
            CustomDatabaseUpdater_Q484423 updater = new CustomDatabaseUpdater_Q484423(CreateObjectSpaceProvider(), modules, null);

            ModuleInfo info1 = new ModuleInfo(Session);
            info1.Version = "2.0.0.0";
            info1.Name = "Module1";
            ModuleInfo info2 = new ModuleInfo(Session);
            info2.Version = "2.0.0.0";
            info2.Name = "Module2";

            updater.VersionInfoListForTest.Add(info1);
            updater.VersionInfoListForTest.Add(info2);

            CompatibilityError error = updater.CheckCompatibility();
            Assert.IsTrue(error is CompatibilityDatabaseIsOldError);
            IList<CompatibilityError> errors = updater.CheckCompatibilityForAllModules();

            Assert.AreEqual(2, errors.Count);
            Assert.IsTrue(errors[0] is CompatibilityDatabaseIsOldError);
            Assert.IsTrue(errors[1] is CompatibilityApplicationIsOldError);

            info1 = new ModuleInfo(Session);
            info1.Version = "3.0.0.0";
            info1.Name = "Module1";
            info2 = new ModuleInfo(Session);
            info2.Version = "1.0.0.0";
            info2.Name = "Module2";

            updater.VersionInfoListForTest.Clear();

            updater.VersionInfoListForTest.Add(info1);
            updater.VersionInfoListForTest.Add(info2);

            error = updater.CheckCompatibility();
            Assert.IsNull(error);
            errors = updater.CheckCompatibilityForAllModules();
            Assert.IsNull(errors);
        }
		[Test]
		public void Test_CreateDatabaseUpdater() {
			XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
			app.CreateDatabaseUpdater_AllowBase = true;
			DatabaseUpdaterBase updater = app.CreateDatabaseUpdater(app.ObjectSpaceProvider);
			Assert.IsNotNull(updater); //default updater created successfully
            Assert.AreEqual(CheckCompatibilityType.ModuleInfo, app.CheckCompatibilityType);
            Assert.IsTrue(updater is DatabaseUpdater);
		}
        [Test]
        public void Test_CreateDatabaseSchemaUpdater() {
            XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
            app.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            app.CreateDatabaseUpdater_AllowBase = true;
            DatabaseUpdaterBase updater = app.CreateDatabaseUpdater(app.ObjectSpaceProvider);
            Assert.IsNotNull(updater); //default updater created successfully
            Assert.AreEqual(CheckCompatibilityType.DatabaseSchema, app.CheckCompatibilityType);
            Assert.IsTrue(updater is DatabaseSchemaUpdater);
        }
        [Test]
        public void Test_CreateDatabaseUpdaterBaseOnObjectSpaceProvider() {
            XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
            app.CreateDatabaseUpdater_AllowBase = true;
            app.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            app.ObjectSpaceProvider.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;
            DatabaseUpdaterBase updater = app.CreateDatabaseUpdater(app.ObjectSpaceProvider);
            Assert.IsNotNull(updater);
            Assert.That(updater, Is.InstanceOf<DatabaseUpdater>());
        }
        [Test]
        public void Test_CreateDatabaseUpdaterBaseOnObjectSpaceProvider2() {
            XafApplicationForTests app = new XafApplicationForTests(this, modelApplication);
            app.CreateDatabaseUpdater_AllowBase = true;
            app.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;
            app.ObjectSpaceProvider.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            DatabaseUpdaterBase updater = app.CreateDatabaseUpdater(app.ObjectSpaceProvider);
            Assert.IsNotNull(updater);
            Assert.That(updater, Is.InstanceOf<DatabaseSchemaUpdater>());
        }
        [Test]
        public void Test_RegisterTypesInfoCheckCompatibilityTypeDatabaseSchema() {
            XafTypesInfo.HardReset();
            XafApplicationForTests app = new XafApplicationForTests();
            app.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            XPObjectSpaceProvider provider = new XPObjectSpaceProvider(this, true);
            app.Setup("test app", provider);
            Assert.IsNull(XafTypesInfo.Instance.FindTypeInfo("DevExpress.ExpressApp.Xpo.Updating.ModuleInfo"));
        }
        [Test]
        public void Test_RegisterTypesInfoCheckCompatibilityTypeModuleInfo() {
            XafTypesInfo.HardReset();
            XafApplicationForTests app = new XafApplicationForTests();
            app.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;
            XPObjectSpaceProvider provider = new XPObjectSpaceProvider(this, true);
            app.Setup("test app", provider);
            Assert.IsNotNull(XafTypesInfo.Instance.FindTypeInfo("DevExpress.ExpressApp.Xpo.Updating.ModuleInfo"));
        }
        [Test]
        public void Test_GetCheckCompatibilityType() {
            XafApplicationForTests app = new XafApplicationForTests();
            Assert.AreEqual(ExpressApp.CheckCompatibilityType.ModuleInfo, app.CheckCompatibilityType);
            XPObjectSpaceProvider provider = new XPObjectSpaceProvider(this, true);
            Assert.AreEqual(ExpressApp.CheckCompatibilityType.ModuleInfo, app.TestGetCheckCompatibilityType(provider));
            provider.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            Assert.AreEqual(ExpressApp.CheckCompatibilityType.DatabaseSchema, app.TestGetCheckCompatibilityType(provider));
        }
        [Test] // B211669
        public void TestModelDoesNotContainsInvisibleTypes() {
            TestModuleBase module = new TestModuleBase();
            module.AdditionalExportedTypes.Add(typeof(InvisibleDomainClass));
            ApplicationModulesManager modulesManager = new ApplicationModulesManager();
            modulesManager.AddModule(module);
            TestApplication application = new TestApplication(this, modulesManager);
            IModelApplication model = application.Model;
            IModelClass classModel = model.BOModel.GetClass(typeof(InvisibleDomainClass));
            Assert.IsNull(classModel);
        }
        [Test]  // T7770
        public void TestCustomizeUserDifferences() {
            Action<XafApplicationForTests> setup = delegate(XafApplicationForTests application) {
                application.Setup("TestApplication", CreateObjectSpaceProvider());
                application.CreateDatabaseUpdater_AllowBase = false;
                application.CreateDatabaseUpdater_Result = new CustomDatabaseUpdater(CreateObjectSpaceProvider(), application.Modules, application.ApplicationName);
            };
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                setup(application);
                Assert.AreEqual(null, application.Model.Description);
                application.Logon();
                Assert.AreEqual(null, application.Model.Description);
            }
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                application.customizeUserDifferences = delegate(IList<IModelApplication> layers) {
                    ModelApplicationBase customLayer = ApplicationCreator.CreateModelApplication();
                    customLayer.Id = "CustomLayer";
                    ((IModelApplication)customLayer).Description = "TestDescription";
                    layers.Add((IModelApplication)customLayer);
                };
                setup(application);
                Assert.AreEqual(null, application.Model.Description);
                application.Logon();
                Assert.AreEqual("TestDescription", application.Model.Description);
                Assert.AreEqual("CustomLayer", ApplicationModelTestsHelper.GetLastLayer((ModelApplicationBase)application.Model).Id);
            }
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                application.customizeUserDifferences = delegate(IList<IModelApplication> layers) {
                    ModelApplicationBase customLayer1 = ApplicationCreator.CreateModelApplication();
                    customLayer1.Id = "CustomLayer1";
                    ((IModelApplication)customLayer1).Title = "TestTitle";
                    layers.Add((IModelApplication)customLayer1);
                    ModelApplicationBase customLayer2 = ApplicationCreator.CreateModelApplication();
                    customLayer2.Id = "CustomLayer2";
                    ((IModelApplication)customLayer2).Company = "TestCompany";
                    layers.Add((IModelApplication)customLayer2);
                };
                setup(application);
                Assert.AreEqual(null, application.Model.Description);
                application.Logon();
                Assert.AreEqual("TestTitle", application.Model.Title);
                Assert.AreEqual("TestCompany", application.Model.Company);
                Assert.AreEqual("CustomLayer2", ApplicationModelTestsHelper.GetLastLayer((ModelApplicationBase)application.Model).Id);
            }
        }
        [Test]  // T7770
        public void TestCustomizeUserDifferences_ErrorCases() {
            Action<XafApplicationForTests> setup = delegate(XafApplicationForTests application) {
                application.Setup("TestApplication", CreateObjectSpaceProvider());
                application.CreateDatabaseUpdater_AllowBase = false;
                application.CreateDatabaseUpdater_Result = new CustomDatabaseUpdater(CreateObjectSpaceProvider(), application.Modules, application.ApplicationName);
            };
            Predicate<InvalidOperationException> isExpectedException = delegate(InvalidOperationException e) {
                return e.StackTrace.StartsWith("   at " + typeof(XafApplication).FullName + ".CheckCustomizedUserDifferences");
            };
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                application.customizeUserDifferences = delegate(IList<IModelApplication> layers) {
                    layers.Clear();
                };
                setup(application);
                try {
                    application.Logon();
                }
                catch(InvalidOperationException e) {
                    Assert.IsTrue(isExpectedException(e));
                }
            }
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                application.customizeUserDifferences = delegate(IList<IModelApplication> layers) {
                    layers.Add(layers[0]);
                };
                setup(application);
                try {
                    application.Logon();
                }
                catch(InvalidOperationException e) {
                    Assert.IsTrue(isExpectedException(e));
                }
            }
        }
        [Test]  // Q404226
        public void TestCurrentAspectProviderIsSetInModelWhenUserIsNotLoggedIn() {
            using(XafApplicationForTests application = new XafApplicationForTests()) {
                application.CustomCheckCompatibility += (sender, args) => { args.Handled = true; }; 
                application.Setup("TestApplication", CreateObjectSpaceProvider());
                ICurrentAspectProvider currentAspectProvider = application.CurrentAspectProvider;
                Assert.IsNotNull(currentAspectProvider);
                Assert.AreEqual(currentAspectProvider, ((IModelApplicationServices)application.Model).CurrentAspectProvider);
                application.Logon();
                Assert.AreEqual(currentAspectProvider, ((IModelApplicationServices)application.Model).CurrentAspectProvider);
            }
        }
        [Test] //B213104
        public void TestCustomizeLanguagesList() {
            ModelApplicationCreator.DebugTest_ClearCreatorList();

            XafApplicationForTests application = new XafApplicationForTests();
            application.CustomizeLanguagesList += delegate(object sender, CustomizeLanguagesListEventArgs e) {
                e.Languages.Add("ru-RU");
            };
            application.Setup("app1", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            Assert.AreEqual(2, ((ModelApplicationBase)application.Model).AspectCount);
            Assert.AreEqual("ru-RU", ((ModelApplicationBase)application.Model).Aspects[0]);
            ModelApplicationCreator.DebugTest_ClearCreatorList();
        }
        [Test] // Q403503
        public void TestModuleInfoTypeIsNotNullOnCustomizeTypesInfo() {
            XafApplicationForTests application = new XafApplicationForTests();
            ApplicationModulesManager modulesManager = new ApplicationModulesManager();
            modulesManager.CustomizeTypesInfo += delegate(object sender, CustomizeTypesInfoEventArgs e) {
                Assert.IsNotNull(application.ModuleInfoType);
            };
            application.Setup("App", new XPObjectSpaceProvider(this), modulesManager, null);
        }
        [Test] // S172038
        public void TestModuleInfoTypeIsNull_DatabaseSchemaCheckCompatibility() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            ApplicationModulesManager modulesManager = new ApplicationModulesManager();
            XPObjectSpaceProvider provider = new XPObjectSpaceProvider(this);
            application.Setup("App", provider, modulesManager, null);
            
            List<DevExpress.Xpo.Metadata.XPClassInfo> classes = provider.XPDictionary.Classes as List<DevExpress.Xpo.Metadata.XPClassInfo>;
            Assert.IsNotNull(classes);
            Assert.AreEqual(0, classes.Count);
        }
        [Test] // S172038
        public void TestModuleInfoTypeIsNull_ObjectSpaceProviderWithoutCheckCompatibility() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.CheckCompatibilityType = CheckCompatibilityType.ModuleInfo;
            ApplicationModulesManager modulesManager = new ApplicationModulesManager();
            XPObjectSpaceProvider provider = new XPObjectSpaceProvider(this);
            provider.SchemaUpdateMode = SchemaUpdateMode.None;
            application.Setup("App", provider, modulesManager, null);

            List<DevExpress.Xpo.Metadata.XPClassInfo> classes = provider.XPDictionary.Classes as List<DevExpress.Xpo.Metadata.XPClassInfo>;
            Assert.IsNotNull(classes);
            Assert.AreEqual(0, classes.Count);
        }
        [Test]
        public void Test_CreateCreateDefaultDummySecurity() {
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);
            xafApplication.Setup();
            Assert.IsTrue(xafApplication.Security is SecurityDummy);
            XafTypesInfo.Reset();
        }
        [Test]
        public void Test_CreateObjectSpaceProvider() {
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProvider2 = new TestObjectSpaceProvider();
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider2);
            xafApplication.Setup();
            Assert.AreEqual(2, xafApplication.ObjectSpaceProviders.Count);
            XafTypesInfo.Reset();
        }
        [Test]
        public void Test_CreateObjectSpace() {
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProvider2 = new TestObjectSpaceProvider();
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider2);
            ((IList)objectSpaceProvider1.EntityStore.RegisteredEntities).Add(typeof(SimpleTestClassA));
            ((IList)objectSpaceProvider2.EntityStore.RegisteredEntities).Add(typeof(SimpleTestClassB));
            xafApplication.Setup();

            objectSpaceProvider1.CreateObjectSpaceCallCount = 0;
            objectSpaceProvider2.CreateObjectSpaceCallCount = 0;
            xafApplication.CreateObjectSpace(typeof(SimpleTestClassA));
            Assert.AreEqual(1, objectSpaceProvider1.CreateObjectSpaceCallCount);
            Assert.AreEqual(0, objectSpaceProvider2.CreateObjectSpaceCallCount);

            objectSpaceProvider1.CreateObjectSpaceCallCount = 0;
            objectSpaceProvider2.CreateObjectSpaceCallCount = 0;
            xafApplication.CreateObjectSpace(typeof(SimpleTestClassB));
            Assert.AreEqual(0, objectSpaceProvider1.CreateObjectSpaceCallCount);
            Assert.AreEqual(1, objectSpaceProvider2.CreateObjectSpaceCallCount);

            XafTypesInfo.Reset();
        }
		[Test]
        public void Test_GetObjectSpaceToShowDetailViewFrom() {
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProvider2 = new TestObjectSpaceProvider();
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider2);
            ((IList)objectSpaceProvider1.EntityStore.RegisteredEntities).Add(typeof(SimpleTestClassA));
            ((IList)objectSpaceProvider2.EntityStore.RegisteredEntities).Add(typeof(SimpleTestClassB));
            xafApplication.Setup();

            Frame frame = new Frame(xafApplication, TemplateContext.View);

            objectSpaceProvider1.CreateObjectSpaceCallCount = 0;
            objectSpaceProvider2.CreateObjectSpaceCallCount = 0;
            xafApplication.GetObjectSpaceToShowDetailViewFrom(frame, typeof(SimpleTestClassA));
            Assert.AreEqual(1, objectSpaceProvider1.CreateObjectSpaceCallCount);
            Assert.AreEqual(0, objectSpaceProvider2.CreateObjectSpaceCallCount);

            objectSpaceProvider1.CreateObjectSpaceCallCount = 0;
            objectSpaceProvider2.CreateObjectSpaceCallCount = 0;
            xafApplication.GetObjectSpaceToShowDetailViewFrom(frame, typeof(SimpleTestClassB));
            Assert.AreEqual(0, objectSpaceProvider1.CreateObjectSpaceCallCount);
            Assert.AreEqual(1, objectSpaceProvider2.CreateObjectSpaceCallCount);

            XafTypesInfo.Reset();
        }
        [Test]
        public void Test_CreateCustomObjectSpaceProviderEventArgs() {
            TestObjectSpaceProvider objectSpaceProviderA = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProviderB = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProviderC = new TestObjectSpaceProvider();
            TestObjectSpaceProvider objectSpaceProviderD = new TestObjectSpaceProvider();
            CreateCustomObjectSpaceProviderEventArgs eventArgs = new CreateCustomObjectSpaceProviderEventArgs("");

            eventArgs.ObjectSpaceProvider = objectSpaceProviderA;
            Assert.AreEqual(1, eventArgs.ObjectSpaceProviders.Count);
            Assert.AreEqual(objectSpaceProviderA, eventArgs.ObjectSpaceProvider);
            Assert.AreEqual(objectSpaceProviderA, eventArgs.ObjectSpaceProviders[0]);

            eventArgs.ObjectSpaceProvider = objectSpaceProviderB;
            Assert.AreEqual(1, eventArgs.ObjectSpaceProviders.Count);
            Assert.AreEqual(objectSpaceProviderB, eventArgs.ObjectSpaceProvider);
            Assert.AreEqual(objectSpaceProviderB, eventArgs.ObjectSpaceProviders[0]);

            eventArgs.ObjectSpaceProviders.Clear();
            Assert.AreEqual(null, eventArgs.ObjectSpaceProvider);

            eventArgs.ObjectSpaceProviders.Add(objectSpaceProviderA);
            eventArgs.ObjectSpaceProviders.Add(objectSpaceProviderB);
            eventArgs.ObjectSpaceProviders.Add(objectSpaceProviderC);
            eventArgs.ObjectSpaceProviders.Add(objectSpaceProviderD);
            Assert.AreEqual(4, eventArgs.ObjectSpaceProviders.Count);
            Assert.AreEqual(objectSpaceProviderA, eventArgs.ObjectSpaceProvider);

            eventArgs.ObjectSpaceProvider = objectSpaceProviderC;
            Assert.AreEqual(4, eventArgs.ObjectSpaceProviders.Count);
            Assert.AreEqual(objectSpaceProviderC, eventArgs.ObjectSpaceProvider);
            Assert.IsTrue(eventArgs.ObjectSpaceProviders.Contains(objectSpaceProviderA));
            Assert.IsTrue(eventArgs.ObjectSpaceProviders.Contains(objectSpaceProviderB));
            Assert.IsTrue(eventArgs.ObjectSpaceProviders.Contains(objectSpaceProviderC));
            Assert.IsTrue(eventArgs.ObjectSpaceProviders.Contains(objectSpaceProviderD));
        }
		[Test]
		public void Test_CreateNestedObjectSpace() {
			XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
			TestObjectSpaceProvider objectSpaceProvider = new TestObjectSpaceProvider();
			xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider);
			xafApplication.Setup();

			xafApplication.ObjectSpaceCreated += new EventHandler<ObjectSpaceCreatedEventArgs>(Application_ObjectSpaceCreated);
			application_ObjectSpaceCreated_CallCount = 0;
			IObjectSpace parentObjectSpace = xafApplication.CreateObjectSpace();
			Assert.AreEqual(1, application_ObjectSpaceCreated_CallCount);

			application_ObjectSpaceCreated_CallCount = 0;
			xafApplication.CreateNestedObjectSpace(parentObjectSpace);
			Assert.AreEqual(1, application_ObjectSpaceCreated_CallCount);
		}
		[Test]
		public void Test_CheckCompatibility_SeveralObjectSpaceProviders() {
			XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
			TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
			TestObjectSpaceProvider objectSpaceProvider2 = new TestObjectSpaceProvider();
			TestObjectSpaceProvider objectSpaceProvider3 = new TestObjectSpaceProvider();
			TestObjectSpaceProvider objectSpaceProvider4 = new TestObjectSpaceProvider();
			xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);
			xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider2);
			xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider3);
			xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider4);
			objectSpaceProvider1.ModuleInfoType = typeof(ModuleInfo);
			objectSpaceProvider3.ModuleInfoType = typeof(ModuleInfo);
			xafApplication.Setup();
			xafApplication.Modules.Clear();

			xafApplication.CreateDatabaseUpdater_CallCount = 0;
			xafApplication.CheckCompatibility();
			Assert.AreEqual(2, xafApplication.CreateDatabaseUpdater_CallCount);
		}
        [Test] //S173659
        public void Test_CheckCompatibility_SkipObjectSpaceProviders() {
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            TestObjectSpaceProvider objectSpaceProvider1 = new TestObjectSpaceProvider();
            objectSpaceProvider1.ModuleInfoType = typeof(ModuleInfo);
            objectSpaceProvider1.SchemaUpdateMode = SchemaUpdateMode.DatabaseAndSchema;
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider1);

            TestObjectSpaceProvider objectSpaceProvider2 = new TestObjectSpaceProvider();
            objectSpaceProvider2.ModuleInfoType = typeof(ModuleInfo);
            objectSpaceProvider2.SchemaUpdateMode = SchemaUpdateMode.None;
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider2);

            TestObjectSpaceProvider objectSpaceProvider3 = new TestObjectSpaceProvider();
            objectSpaceProvider3.ModuleInfoType = typeof(ModuleInfo);
            objectSpaceProvider3.SchemaUpdateMode = SchemaUpdateMode.DatabaseAndSchema;
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider3);

            TestObjectSpaceProvider objectSpaceProvider4 = new TestObjectSpaceProvider();
            objectSpaceProvider4.ModuleInfoType = typeof(ModuleInfo);
            objectSpaceProvider4.SchemaUpdateMode = SchemaUpdateMode.None;
            xafApplication.TestObjectSpaceProviders.Add(objectSpaceProvider4);
            
            xafApplication.Setup();
            xafApplication.Modules.Clear();

            xafApplication.CheckCompatibility();
            Assert.AreEqual(2, xafApplication.CheckedObjectSpaceProviders.Count);
            Assert.AreEqual(objectSpaceProvider1, xafApplication.CheckedObjectSpaceProviders[0]);
            Assert.AreEqual(objectSpaceProvider3, xafApplication.CheckedObjectSpaceProviders[1]);
        }
        [Test]
        public void Test_CheckCompatibility_MultiThread() {
            UseClasses(new List<Type>() { typeof(DevExpress.Persistent.BaseImpl.Event), typeof(DevExpress.Persistent.BaseImpl.Task) }.ToArray());
            BaseXafTest.ExecuteInMultiThreads(
                () => {
                    XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
                    xafApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                    xafApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                    xafApplication.DatabaseVersionMismatch += delegate (object sender, DatabaseVersionMismatchEventArgs e) {
                        e.Updater.Update();
                        e.Handled = true;
                    };
                    xafApplication.TestObjectSpaceProviders.Add(CreateObjectSpaceProvider());
                    xafApplication.Setup();
                    xafApplication.CheckCompatibility();
                },
                100
            );
        }
        [Test]
        public void Test_CheckCompatibility_MultiThread_SeveralApplicationsRequireOneCheck() {
            XafApplicationForTests2.isCompatibilityChecked = false;
            int customCheckCompatibilityCallCount = 0;
            int databaseVersionMismatchCallCount = 0;
            BaseXafTest.ExecuteInMultiThreads(
                () => {
                    XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
                    xafApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                    xafApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                    xafApplication.UseStaticIsCompatibilityCheckedFlag = true;
                    xafApplication.DatabaseVersionMismatch += delegate (object sender, DatabaseVersionMismatchEventArgs e) {
                        databaseVersionMismatchCallCount++;
                        e.Updater.Update();
                        e.Handled = true;
                    };
                    xafApplication.CustomCheckCompatibility += (s, e) => {
                        customCheckCompatibilityCallCount++;
                    };
                    xafApplication.TestObjectSpaceProviders.Add(CreateObjectSpaceProvider());
                    xafApplication.Setup();
                    xafApplication.CheckCompatibility();
                },
                100
            );
            Assert.AreEqual(1, customCheckCompatibilityCallCount);
            Assert.AreEqual(1, databaseVersionMismatchCallCount);
        }
        [Test]//Previous test does not fall in batch run
        public void Test_CheckCompatibility_MultiThread_OneApplication() {
            XafApplicationForTests2.isCompatibilityChecked = false;
            int customCheckCompatibilityCallCount = 0;
            int databaseVersionMismatchCallCount = 0;
            XafApplicationForTests2 xafApplication = new XafApplicationForTests2();
            xafApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            xafApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            xafApplication.UseStaticIsCompatibilityCheckedFlag = true;
            xafApplication.DatabaseVersionMismatch += delegate (object sender, DatabaseVersionMismatchEventArgs e) {
                databaseVersionMismatchCallCount++;
                e.Updater.Update();
                e.Handled = true;
            };
            xafApplication.CustomCheckCompatibility += (s, e) => {
                customCheckCompatibilityCallCount++;
            };
            xafApplication.TestObjectSpaceProviders.Add(CreateObjectSpaceProvider());
            xafApplication.Setup();
            BaseXafTest.ExecuteInMultiThreads(
                () => {
                    xafApplication.CheckCompatibility();
                },
                100
            );
            Assert.AreEqual(1, customCheckCompatibilityCallCount);
            Assert.AreEqual(1, databaseVersionMismatchCallCount);
        }
        [Test]
		public void Test_UserDifferencesLoaded() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);

            int userDifferencesLoadedCallCount = 0;
            application.UserDifferencesLoaded += (sender, e) => userDifferencesLoadedCallCount++;
            application.Logon();
            Assert.AreEqual(1, userDifferencesLoadedCallCount);
        }
        [Test]
        public void Test_ModelChanged_OnSetup() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            int modelChanged = 0;

            application.ModelChanged += (sender, e) => modelChanged++;
            application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            Assert.AreEqual(1, modelChanged);
        }
        [Test]
        public void Test_ModelChanged_OnLogon() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);

            int modelChanged = 0;
            application.ModelChanged += (sender, e) => modelChanged++;
            application.Logon();
            Assert.AreEqual(1, modelChanged);
        }
		[Test]
		public void Test_ModelChanged_OnDropModel() {
			XafApplicationForTests application = new XafApplicationForTests();
			application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
			application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
			application.Logon();

			int modelChanged = 0;
			application.ModelChanged += (sender, e) => modelChanged++;
			application.DropModelForTests();
			Assert.AreEqual(1, modelChanged);
		}
        [Test]
        public void Test_DropOldModel_OnLogon() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);

            IModelApplication model = application.Model;
            Assert.Greater(((ModelApplicationBase)model).LayersCount, 0);
            application.Logon();
            Assert.AreEqual(0, ((ModelApplicationBase)model).LayersCount);
        }
        [Test]
        public void Test_DropOldModel_OnDropModel() {
            XafApplicationForTests application = new XafApplicationForTests();
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            application.Setup("Test application", new XPObjectSpaceProvider(this), new ApplicationModulesManager(), null);
            application.Logon();

            IModelApplication model = application.Model;
            Assert.Greater(((ModelApplicationBase)model).LayersCount, 0);
            application.DropModelForTests();
            Assert.AreEqual(0, ((ModelApplicationBase)model).LayersCount);
        }
		[Test]
		public void Test_FindModelClass() {
			RegisterTypesForModel(typeof(TestObject));
			XafApplicationForTests application = new XafApplicationForTests(this, modelApplication);
			Assert.IsNotNull(application.Model.BOModel);
			Assert.AreEqual(null, application.FindModelClass(null));
		}        
		[Test]
		public void Test_FindDetailViewId_XafDataViewRecord() {
			UseClass(typeof(TestObject));
			RegisterTypesForModel(typeof(TestObject));
			
			ObjectSpace.CreateObject(typeof(TestObject));
			ObjectSpace.CommitChanges();

			TestApplication application = new TestApplication(modelApplication);
			CollectionSource collectionSource = new CollectionSource(ObjectSpace, typeof(TestObject), CollectionSourceDataAccessMode.DataView);
			ListView listView = new ListView(collectionSource, new TestListEditor());

			XafDataViewRecord obj = (XafDataViewRecord)collectionSource.List[0];
			Assert.AreEqual("TestObject_DetailView", application.FindDetailViewId(obj, listView));
			Assert.AreEqual("TestObject_DetailView", application.FindDetailViewId(obj, null));
		}
		[Test]
		public void Test_FindDetailViewId_XafInstantFeedbackRecord() {
			UseClass(typeof(TestObject));
			RegisterTypesForModel(typeof(TestObject));

			IObjectSpaceProvider objectSpaceProvider = CreateObjectSpaceProvider();
			IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
			TestApplication application = new TestApplication(modelApplication);
			CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(TestObject), CollectionSourceDataAccessMode.InstantFeedback);
			ListView listView = new ListView(collectionSource, new TestListEditor());

			XafInstantFeedbackRecord obj = new XafInstantFeedbackRecord(typeof(TestObject), 1, 0);
			Assert.AreEqual("TestObject_DetailView", application.FindDetailViewId(obj, listView));
			Assert.AreEqual("TestObject_DetailView", application.FindDetailViewId(obj, null));
		}
		[Test]
		public void Test_IsObjectSpaceProviderOwner() {
			XafApplicationForTests application1 = new XafApplicationForTests();
			application1.CreateCustomObjectSpaceProvider += Application_CreateCustomObjectSpaceProvider;
			objectSpaceProviderResult = new NonPersistentObjectSpaceProvider();
			isObjectSpaceProviderOwnerResult = false;
			application1.Setup();
			Assert.AreEqual(1, application1.ObjectSpaceProviders.Count);
			Assert.AreEqual(objectSpaceProviderResult, application1.ObjectSpaceProviders[0]);
			Assert.AreEqual(false, application1.IsObjectSpaceProviderOwner);

			XafApplicationForTests application2 = new XafApplicationForTests();
			application2.CreateCustomObjectSpaceProvider += Application_CreateCustomObjectSpaceProvider;
			objectSpaceProviderResult = new NonPersistentObjectSpaceProvider();
			isObjectSpaceProviderOwnerResult = true;
			application2.Setup();
			Assert.AreEqual(1, application2.ObjectSpaceProviders.Count);
			Assert.AreEqual(objectSpaceProviderResult, application2.ObjectSpaceProviders[0]);
			Assert.AreEqual(true, application2.IsObjectSpaceProviderOwner);

			XafApplicationForTests application3 = new XafApplicationForTests();
			application3.objectSpaceProviderResult = new NonPersistentObjectSpaceProvider();
			application3.isObjectSpaceProviderOwnerResult = false;
			application3.Setup();
			Assert.AreEqual(1, application3.ObjectSpaceProviders.Count);
			Assert.AreEqual(false, application3.IsObjectSpaceProviderOwner);

			XafApplicationForTests application4 = new XafApplicationForTests();
			application4.objectSpaceProviderResult = new NonPersistentObjectSpaceProvider();
			application4.isObjectSpaceProviderOwnerResult = true;
			application4.Setup();
			Assert.AreEqual(1, application4.ObjectSpaceProviders.Count);
			Assert.AreEqual(true, application4.IsObjectSpaceProviderOwner);
		}
		[Test]
		public void Test_CreateDetailView_MultiThreads() {
			RegisterTypesForModel(typeof(NonPersistentClass1));
            NonPersistentTypeInfoSource nonPersistentTypeInfoSource = FindNonPersistentTypeInfoSource();

			ModelNode views = (ModelNode)modelApplication.Views;
			Assert.AreEqual(null, views.Nodes);

			TestApplication application = new TestApplication(modelApplication);
			BaseXafTest.ExecuteInMultiThreads(
				() => {
					IObjectSpace objectSpace = new NonPersistentObjectSpace(application.TypesInfo, nonPersistentTypeInfoSource);
					Object obj = objectSpace.CreateObject(typeof(NonPersistentClass1));
					DetailView detailView = application.CreateDetailView(objectSpace, obj, null);
					Assert.AreEqual("NonPersistentClass1_DetailView", detailView.Model.Id);
				},
				100
			);
			Assert.AreEqual(3, views.Nodes.Count);
			Assert.AreEqual(typeof(NonPersistentClass1).FullName, ((IModelObjectView)views.Nodes[0]).ModelClass.Name);
			Assert.AreEqual(typeof(NonPersistentClass1).FullName, ((IModelObjectView)views.Nodes[1]).ModelClass.Name);
			Assert.AreEqual(typeof(NonPersistentClass1).FullName, ((IModelObjectView)views.Nodes[2]).ModelClass.Name);
		}
		[Test, Ignore("TODO")]
		public void Test_Dispose_MultiThreads() {
			TestApplication application = new TestApplication(modelApplication);
			application.Dispose();
			application.Dispose();
			BaseXafTest.ExecuteInMultiThreads(
				() => {
					application.Dispose();
				},
				100
			);
			Assert.AreEqual(true, application.IsDisposed);
		}
		[Test]
		public void Test_CreateLogonWindowObjectSpace() {
			XafApplicationForTests xafApplication = new XafApplicationForTests();
			xafApplication.TypesInfo.RegisterEntity(typeof(TestClassB));
			IEntityStore typeInfoSource = ((TypesInfo)xafApplication.TypesInfo).FindEntityStore(typeof(NonPersistentTypeInfoSource));
			Assert.AreEqual(true, typeInfoSource.RegisteredEntities.Contains(typeof(TestClassB)));

			IObjectSpace objectSpace = xafApplication.CreateLogonWindowObjectSpace();
			Assert.AreEqual(typeof(NonPersistentObjectSpace), objectSpace.GetType());
			Assert.AreEqual(true, objectSpace.IsKnownType(typeof(TestClassB)));
		}
        [Test]
        public void Test_SetupPopupWindow() {
            TestApplication application = new TestApplication();
            Assert.AreEqual(0, application.OnPopupWindowCreatingCounter);
            Assert.AreEqual(0, application.OnPopupWindowCreatedCounter);
            Window popupWindow = application.CreatePopupWindow(TemplateContext.PopupWindow, "", new Controller[] { });

            Assert.AreEqual(1, application.OnPopupWindowCreatingCounter);
            Assert.AreEqual(1, application.OnPopupWindowCreatedCounter);
        }
        [Test]
        public void Test_OptimizedControllersCreation() {
            TestApplication application = new TestApplication();
            ExpressApplicationSetupParameters expressApplicationSetupParameters = new ExpressApplicationSetupParameters(
                "", null, new ControllersManager(), new ModuleList());
            application.Setup(expressApplicationSetupParameters);
            ListView listView = new ListView();

            // CreateNestedFrame

            application.OptimizedControllersCreation = false;
            application.CreateNestedFrame(null, TemplateContext.NestedFrame, listView);
            Assert.AreEqual(null, application.CreateControllers_View);
            Assert.AreEqual(null, application.CreateControllersCore_View);

            application.OptimizedControllersCreation = true;
            application.CreateNestedFrame(null, TemplateContext.NestedFrame, listView);
            Assert.AreEqual(listView, application.CreateControllers_View);
            Assert.AreEqual(listView, application.CreateControllersCore_View);

            // CreateWindow

            application.OptimizedControllersCreation = false;
            application.CreateWindow(TemplateContext.View, null, true, true, listView);
            Assert.AreEqual(null, application.CreateControllers_View);
            Assert.AreEqual(null, application.CreateControllersCore_View);

            application.OptimizedControllersCreation = true;
            application.CreateWindow(TemplateContext.View, null, true, true, listView);
            Assert.AreEqual(listView, application.CreateControllers_View);
            Assert.AreEqual(listView, application.CreateControllersCore_View);
        }
        [Test]
        public void ShowViewStrategyChanged_CreateShowViewStrategy_Null() {
            XafApplicationForTests application = new XafApplicationForTests();
            int showViewStrategyChangedCallCount = 0;
            application.ShowViewStrategyChanged += (s , e) => {
                showViewStrategyChangedCallCount++;
            };
            Assert.IsNull(application.ShowViewStrategy);
            Assert.AreEqual(0, showViewStrategyChangedCallCount);
        }
        [Test]
        public void ShowViewStrategyChanged_CreateShowViewStrategy_NotNull() {
            XafApplicationForTests2 application2 = new XafApplicationForTests2();
            int showViewStrategyChangedCallCount = 0;
            application2.ShowViewStrategyChanged += (s, e) => {
                showViewStrategyChangedCallCount++;
            };
            showViewStrategyChangedCallCount = 0;
            Assert.IsNotNull(application2.ShowViewStrategy);
            Assert.AreEqual(1, showViewStrategyChangedCallCount);
        }
        [Test]
        public void ShowViewStrategyChanged_ShowViewStrategyCreatedInConstructor() {
            TestApplication application3 = new TestApplication();
            int showViewStrategyChangedCallCount = 0;
            application3.ShowViewStrategyChanged += (s, e) => {
                showViewStrategyChangedCallCount++;
            };
            showViewStrategyChangedCallCount = 0;
            Assert.IsNotNull(application3.ShowViewStrategy);
            Assert.AreEqual(0, showViewStrategyChangedCallCount);
        }
        [Test]
        public void ShowViewStrategyChanged_NotRaisedIfNonNewShowViewStrategy() {
            XafApplicationForTests application = new XafApplicationForTests();
            int showViewStrategyChangedCallCount = 0;
            application.ShowViewStrategyChanged += (s, e) => {
                showViewStrategyChangedCallCount++;
            };
            TestShowViewStrategy testShowViewStrategy = new TestShowViewStrategy(application);
            application.ShowViewStrategy = testShowViewStrategy;
            Assert.AreEqual(1, showViewStrategyChangedCallCount);

            showViewStrategyChangedCallCount = 0;
            application.ShowViewStrategy = testShowViewStrategy;
            Assert.AreEqual(0, showViewStrategyChangedCallCount);
        }
    }
}
#endif
