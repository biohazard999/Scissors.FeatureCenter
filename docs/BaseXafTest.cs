#if DebugTest
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Localization;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using NUnit.Framework;

namespace DevExpress.ExpressApp.Tests {
    public static class BaseXafTestHelper {
        public static XafTestContext.Builder SetupInterfaceExtenders(this XafTestContext.Builder builder, BaseXafTest testFixture) {
            builder.RegisterModelExtender(testFixture.CreateModelExtenderAdapter());
            return builder;
        }
    }
    public class BaseXafTest {
        [DebuggerDisplay("{testFixture.GetType().FullName,nq}")]
        class TestFixtureToModelExtenderAdapter : IModelExtender {
            private readonly BaseXafTest testFixture;
            public TestFixtureToModelExtenderAdapter(BaseXafTest testFixture) {
                this.testFixture = testFixture;
            }
            void IModelExtender.ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
                testFixture.AddCustomerInterfaceExtenders(extenders);
            }
        }
        public IModelExtender CreateModelExtenderAdapter() {
            return new TestFixtureToModelExtenderAdapter(this);
        }

        private ITypesInfo typesInfo;
        private XafTestContext testContext;
		protected IModelApplication modelApplication;

		protected void ReadDiffsFromXmlString(string diffs) {
			ModelApplicationBase diffsLayer = ApplicationCreator.CreateModelApplication();
			ApplicationModelTestsHelper.AddLayer((ModelApplicationBase)modelApplication, diffsLayer);
			ReadModel(diffs, diffsLayer);
		}
		protected void ReadModel(string xml) {
			ReadModel(xml, ((ModelApplicationBase)modelApplication).LastLayer);
		}
		protected void ReadModel(string xml, IModelNode node) {
			ModelApplicationBase application = node as ModelApplicationBase;
			if(application != null && application.IsMaster) {
				node = application.LastLayer;
			}
			new ModelXmlReader().ReadFromString(node, string.Empty, xml);
		}

		protected IModelApplication CreateTestModel(IEnumerable<Type> boModelTypes, params string[] layerXmls) {
			ModelApplicationBase model = ApplicationCreator.CreateModelApplication();
			ApplicationModelTestsHelper.AddLayer(model, ApplicationCreator.CreateModelApplication());
			if(layerXmls != null) {
				ModelXmlReader reader = new ModelXmlReader();
				foreach(string layerXml in layerXmls) {
					ModelApplicationBase layer = ApplicationCreator.CreateModelApplication();
					if(!string.IsNullOrEmpty(layerXml)) {
						reader.ReadFromString(layer, string.Empty, layerXml);
					}
					ApplicationModelTestsHelper.AddLayer(model, layer);
				}
			}
			((IModelSources)model).BOModelTypes = boModelTypes != null ? boModelTypes : Type.EmptyTypes;
			return (IModelApplication)model;
		}
		protected ModelApplicationCreator ApplicationCreator { get { return testContext.GetModelApplicationCreator(); } }
		protected ModelApplicationBase CreateTestLayer() {
			return ApplicationCreator.CreateModelApplication();
		}
		protected ModelApplicationBase CreateTestLayer(string id) {
			ModelApplicationBase result = CreateTestLayer();
			result.Id = id;
			return result;
		}

		protected void RegisterTypesForModel(params Type[] types) {
			RegisterTypesForModel((IModelSources)modelApplication, types);
		}
		protected void RegisterTypesForModel(IModelSources app, params Type[] types) {
			app.BOModelTypes = types;
			UseClasses(types);
		}
		protected void RegisterEditorsForModel(params EditorDescriptor[] editors) {
			((IModelSources)modelApplication).EditorDescriptors = new EditorDescriptors(editors);
		}
		protected void CreateCustomCalculatedMember(Type type, String name, Type memberType, String expression) {
			TypeInfo typeInfo = (TypeInfo)TypesInfo.FindTypeInfo(type);
			if(typeInfo.FindMember(name) == null) {
				typeInfo.CreateMember(name, memberType, expression);
			}
		}

		protected IModelClass GetBOClass<T>() {
			return modelApplication.BOModel.GetClass(typeof(T));
		}
		protected virtual IModelListView GetListView<T>() {
			return modelApplication.Views.GetDefaultListView<T>();
		}
		protected IModelDetailView GetDetailView<T>() {
			return modelApplication.Views.GetDefaultDetailView<T>();
		}
		protected IModelRootNavigationItems GetNavigationItems() {
			return ((IModelApplicationNavigationItems)modelApplication).NavigationItems;
		}

		protected void UseClass<TClass>() {
			UseClass(typeof(TClass));
		}
		protected void UseClass(Type type) {
			TypesInfo.RegisterEntity(type);
		}
		protected void UseClasses(params Type[] types) {
			foreach(Type type in types) {
				UseClass(type);
			}
		}

		protected virtual bool IsRequiredAssembly(String assemblyName) {
			return false;
		}
		protected virtual void AddCustomerInterfaceExtenders(ModelInterfaceExtenders modelInterfaceExtenders) {
		}
		protected virtual void AddResourceLocalizers(IList<IXafResourceLocalizer> localizers) {
		}
        protected virtual ITypesInfo CreateTypesInfo() {
            return XafTestContextHelper.CreateTypesInfo();
        }
		protected virtual void DisposeTypesInfo() {
            typesInfo = null;
			XafTypesInfo.DebugTest_Recreate();
		}
        protected virtual XafTestContext CreateTestContext(ITypesInfo typesInfo) {
            return new XafTestContext.Builder().
                SetTypesInfo(typesInfo).
                SetupForTestFixture(this, !NeedRefreshTestedModulesAndControllers, assemblyName => IsRequiredAssembly(assemblyName.Name)).
                SetupInterfaceExtenders(this).
                CreateTestContext();
        }
        [OneTimeSetUp]
        public virtual void FixtureSetUp() {
            ImageLoader.Reset();
            if(!NeedHardResetTypesInfo) {
                typesInfo = CreateTypesInfo();
                typesInfo.LoadTypesForTestFixture(this, assemblyName => IsRequiredAssembly(assemblyName.Name));
            }
        }
		[OneTimeTearDown]
		public virtual void FixtureTearDown() {
			DisposeTypesInfo();
            ImageLoader.Reset();
        }
        [SetUp]
        public virtual void SetUp() {
            Tracing.Close();
            Tracing.Initialize("", "0");
            CaptionHelper.Setup(null);
            if(NeedHardResetTypesInfo) {
                typesInfo = CreateTypesInfo();
                typesInfo.LoadTypesForTestFixture(this, assemblyName => IsRequiredAssembly(assemblyName.Name));
            }
            testContext = CreateTestContext(typesInfo);
            modelApplication = (IModelApplication)testContext.GetModelApplicationCreator().CreateModelApplication();
            modelApplication.
                AddLayer(testContext, "AutoGeneratedLayer").
                AddLayer(testContext, "DiffLayer").
                SetEditorDescriptors(testContext);
            List<IXafResourceLocalizer> localizers = new List<IXafResourceLocalizer>();
            AddResourceLocalizers(localizers);
            if(localizers.Count > 0) {
                modelApplication.SetLocalizers(localizers);
                CaptionHelper.Setup(modelApplication);
            }
        }
        [TearDown]
		public virtual void TearDown() {
            SecuritySystem.SetInstance(null);
			modelApplication = null;
            CaptionHelper.Setup(null);
            testContext.Dispose();
            testContext = null;
			while(Application.OpenForms.Count != 0) {
				try {
					Application.OpenForms[0].Close();
				}
				catch {
				}
			}
		}

        protected virtual bool NeedRefreshTestedModulesAndControllers {
            get { return false; }
        }
        protected virtual bool NeedHardResetTypesInfo {
			get { return false; }
		}
		protected ITypesInfo TypesInfo {
			get { return testContext.GetTypesInfo(); }
		}
		protected NonPersistentTypeInfoSource FindNonPersistentTypeInfoSource() {
			return (NonPersistentTypeInfoSource)((TypesInfo)TypesInfo).FindEntityStore(typeof(NonPersistentTypeInfoSource));
		}

        protected T GetModule<T>() where T : ModuleBase {
            return testContext.GetModule<T>();
        }
        public static IModelNavigationItem CreateModelItem(ModelApplicationCreator applicationCreator, string itemId) {
			return CreateModelItem(applicationCreator, itemId, "");
		}
		public static IModelNavigationItem CreateModelItem(ModelApplicationCreator applicationCreator, string itemId, string viewId) {
			IModelNavigationItem modelItem = (IModelNavigationItem)applicationCreator.CreateNode(itemId, typeof(IModelNavigationItem));
			modelItem.AddNode<IModelNavigationItems>("Items");
			if(!string.IsNullOrEmpty(viewId)) {
				modelItem.View = (IModelListView)applicationCreator.CreateNode(viewId, typeof(IModelListView));
			}
			return modelItem;
		}

		public static void CheckMembersVisibility(ITypesInfo typesInfo, Type type, IList<String> visibleMembers) {
			ITypeInfo typeInfo = typesInfo.FindTypeInfo(type);
			foreach(IMemberInfo memberInfo in typeInfo.Members) {
				BrowsableAttribute browsableAttribute = memberInfo.FindAttribute<BrowsableAttribute>();
				if((browsableAttribute == null) || browsableAttribute.Browsable) {
					String message = String.Format(
						"The '{0}.{1}' member should be marked by the Browsable(false) attribute.", memberInfo.Owner.Name, memberInfo.Name);
					Assert.AreEqual(true, visibleMembers.Contains(memberInfo.Name), message);
				}
			}
		}

		public static void ExecuteInMultiThreads(List<ThreadMethod> methods, Int32 threadsCount) {
            XafTestContextHelper.ExecuteInMultiThreads(methods.ConvertAll<Action>(m => new Action(m)), threadsCount);
		}
		public static void ExecuteInMultiThreads(ThreadMethod method1, ThreadMethod method2, Int32 threadsCount) {
			ExecuteInMultiThreads(new List<ThreadMethod>() { method1, method2 }, threadsCount);
		}
		public static void ExecuteInMultiThreads(Action method, Int32 threadsCount) {
            XafTestContextHelper.ExecuteInMultiThreads(method, threadsCount);
		}
	}

	public delegate void ThreadMethod();

    public static class ModelHelper {
        public static string GetDetailViewId<T>() {
            return GetDetailViewId(typeof(T));
        }
        public static string GetDetailViewId(Type type) {
            return ModelDetailViewNodesGenerator.GetDetailViewId(type);
        }
        public static string GetListViewId<T>() {
            return GetListViewId(typeof(T));
        }
        public static string GetListViewId(Type type) {
            return ModelListViewNodesGenerator.GetListViewId(type);
        }
        public static string GetLookupListViewId<T>() {
            return GetLookupListViewId(typeof(T));
        }
        public static string GetLookupListViewId(Type type) {
            return ModelLookupViewNodesGeneratorHelper.GetLookupListViewId(type);
        }
        public static string GetNestedListViewId<T>(string propertyName) {
            return ModelNestedListViewNodesGeneratorHelper.GetNestedListViewId(typeof(T), propertyName);
        }
        public static string GetNestedListViewId(Type type, string propertyName) {
            return GetNestedListViewId(type, propertyName);
        }
    }
    public enum TestsInitializationMode { ForTest, ForTestFixture }
    public abstract class BaseXafTests {
        private readonly TestsInitializationMode initializationMode;
        private readonly List<Type> exportesTypes;
        private readonly List<ModuleBase> modules;
        private readonly List<Controller> controllers;
        private ITypesInfo typesInfo;
        private StubModule moduleWithModelInterfaceExtender;
        private Boolean needExtendModel;
        private void SetUpCore() {
            typesInfo = CreateTypesInfo();
            moduleWithModelInterfaceExtender = new StubModule();
            needExtendModel = false;
            RegisterModules();
            RegisterControllers();
            RegisterExportedTypes();
        }
        private void TearDownCore() {
            EraseTypesInfo();
            typesInfo = null;
            moduleWithModelInterfaceExtender = null;
            needExtendModel = false;
            exportesTypes.Clear();
            modules.Clear();
            controllers.Clear();
        }
        protected BaseXafTests(TestsInitializationMode initializationMode) {
            this.initializationMode = initializationMode;
            exportesTypes = new List<Type>();
            modules = new List<ModuleBase>();
            controllers = new List<Controller>();
        }
		protected ITypesInfo TypesInfo {
			get { return typesInfo; }
		}
        public BaseXafTests() : this(TestsInitializationMode.ForTest) { }
        protected virtual ITypesInfo CreateTypesInfo() {
            XafTypesInfo.DebugTest_Recreate();
            return XafTypesInfo.Instance;
        }
        protected virtual void EraseTypesInfo() {
            XafTypesInfo.DebugTest_Recreate();
        }
        protected void RegisterExportedTypes(params Type[] exportedTypes) {
            this.exportesTypes.AddRange(exportedTypes);
            foreach(Type exportedType in exportedTypes) {
                typesInfo.RegisterEntity(exportedType);
            }
        }
        protected void RegisterControllers(params Controller[] controllers) {
            this.controllers.AddRange(controllers);
        }
        protected void RegisterModules(params ModuleBase[] modules) {
            this.modules.AddRange(modules);
        }
        protected void AddModelInterfaceExtender<TargetInterface, ExtenderInterface>() {
            needExtendModel = true;
            moduleWithModelInterfaceExtender.AddModelInterfaceExtender<TargetInterface, ExtenderInterface>();
        }
        protected IModelApplication GetModelApplication() {
            return GetModelApplication(null);
        }
        protected IModelApplication GetModelApplication(String userDiffsXml) {
            return GetModelApplication(new String[0], userDiffsXml);
        }
        protected IModelApplication GetModelApplication(String[] designedDiffsXmls, String userDiffsXml) {
            ApplicationModelManager modelManager = GetApplicationModelManager(designedDiffsXmls);
            ModelApplicationBase userLayer = modelManager.CreateLayerByStore(ModelApplicationLayerIds.UserDiffs, new StringModelStore(userDiffsXml));
            return (IModelApplication)modelManager.CreateModelApplication(new ModelApplicationBase[] { userLayer });
        }
        protected ModelApplicationCreator GetModelApplicationCreator(params String[] designedDiffsXmls) {
            ApplicationModelManager modelManager = GetApplicationModelManager(designedDiffsXmls);
            return modelManager.DebugTest_GetCreator();
        }
        private ApplicationModelManager GetApplicationModelManager(params String[] designedDiffsXmls) {
            ApplicationModelManager modelManager = new ApplicationModelManager();
            List<ModuleBase> allModules = new List<ModuleBase>(modules);
            if(needExtendModel) {
                allModules.Add(moduleWithModelInterfaceExtender);
            }
            for(int i = 0; i < designedDiffsXmls.Length; i++) {
                string diffXml = designedDiffsXmls[i];
                StubModule moduleWithDifference = new StubModule();
                moduleWithDifference.SetDifference(diffXml);
                allModules.Add(moduleWithDifference);
            }
            modelManager.Setup(typesInfo, exportesTypes, allModules, controllers, Type.EmptyTypes, new String[0], null, null);
            return modelManager;
        }
        protected IModelClass GetBOClass<T>(IModelApplication modelApplication) {
            return modelApplication.BOModel.GetClass(typeof(T));
        }
        protected IModelListView GetListView<T>(IModelApplication modelApplication) {
            return modelApplication.Views.GetDefaultListView<T>();
        }
        protected IModelDetailView GetDetailView<T>(IModelApplication modelApplication) {
            return modelApplication.Views.GetDefaultDetailView<T>();
        }
        protected void ReadModel(string xml, IModelNode node) {
            ModelApplicationBase application = node as ModelApplicationBase;
            if(application != null && application.IsMaster) {
                node = application.LastLayer;
            }
            if(((ModelNode)node).IsSlave) {
                ((ModelNode)node).SetIsNewNode(true);
            }
            new ModelXmlReader().ReadFromString(node, string.Empty, xml);
        }
        public static void CheckLostModuleControllers(ModuleBase module) {
            Type[] availableControllers = DevExpress.ExpressApp.Core.ControllersManager.CollectControllerTypesFromAssembly(module.GetType().Assembly);
            List<Type> lostControllers = new List<Type>();
            List<Type> registeredControllers = new List<Type>(module.GetControllerTypes());
            foreach(Type controllerType in availableControllers) {
                if(!registeredControllers.Contains(controllerType)) {
                    lostControllers.Add(controllerType);
                }
            }
            Assert.AreEqual(new Type[0], lostControllers.ToArray());
        }
        [OneTimeSetUp]
        public virtual void FixtureSetUp() {
            if(initializationMode == TestsInitializationMode.ForTestFixture) {
                SetUpCore();
            }
        }
        [OneTimeTearDown]
        public virtual void FixtureTearDown() {
            if(initializationMode == TestsInitializationMode.ForTestFixture) {
                TearDownCore();
            }
        }
        [SetUp]
        public virtual void SetUp() {
            if(initializationMode == TestsInitializationMode.ForTest) {
                SetUpCore();
            }
        }
        [TearDown]
        public virtual void TearDown() {
            if(initializationMode == TestsInitializationMode.ForTest) {
                TearDownCore();
            }
        }
    }

	class StubModule : ModuleBase {
        private readonly ModelInterfaceExtenders modelInterfaceExtenders;
        public StubModule() {
            DiffsStore = ModelStoreBase.Empty;
            modelInterfaceExtenders = new ModelInterfaceExtenders();
        }
        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) { }
		protected internal override IEnumerable<Type> GetRegularTypes() {
			return null;
		}
		protected override IEnumerable<Type> GetDeclaredControllerTypes() { return Type.EmptyTypes; }
        protected override IEnumerable<Type> GetDeclaredExportedTypes() { return Type.EmptyTypes; }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) { return ModuleUpdater.EmptyModuleUpdaters; }
        protected override ModuleTypeList GetRequiredModuleTypesCore() { return new ModuleTypeList(); }
        protected internal override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory) { }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            foreach(Type extendedInterface in modelInterfaceExtenders.GetExtendedInterfaces()) {
                foreach(Type interfaceExtender in modelInterfaceExtenders.GetInterfaceExtenders(extendedInterface)) {
                    extenders.Add(extendedInterface, interfaceExtender);
                }
            }
        }
        public void SetDifference(String diffXml) {
            DiffsStore = new StringModelStore(diffXml);
        }
        public void AddModelInterfaceExtender<TargetInterface, ExtenderInterface>() {
            modelInterfaceExtenders.Add<TargetInterface, ExtenderInterface>();
        }
    }
}
#endif
