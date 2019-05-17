#if DebugTest
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DevExpress.ExpressApp.Base.Tests;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;

namespace DevExpress.ExpressApp.Tests.TestObjects {
    public interface IDBTest {
        IObjectSpace CreateObjectSpace();
        IObjectSpaceProvider CreateObjectSpaceProvider();
    }
    public class TestApplicationFactory {
		private IDBTest owner;

		public TestApplicationFactory(IDBTest owner) {
			this.owner = owner;
		}
		public TestApplication CreateApplication() {
			return CreateApplication(null, null);
		}

		public TestApplication CreateApplication(ModuleBase module) {
			return CreateApplication(module, (Controller)null);
		}
		public TestApplication CreateApplication(ModuleBase module, Controller controller, params Controller[] controllers) {
			TestApplication application = new TestApplication();
			application.CreateAllControllers = false;
			application.CloneControllers = true;

			if(module != null) {
				application.Modules.Add(module);
			}
			if(controller != null) {
				application.Controllers.Add(controller);
			}
			if(controllers != null) {
				application.Controllers.AddRange(controllers);
			}

			application.DetailViewCreating += delegate(object sender, DetailViewCreatingEventArgs e) {
				e.View = new TestDetailViewFactory(owner).CreateView(e.ObjectSpace, e.Obj, e.IsRoot);
			};

			application.Setup("test app", owner.CreateObjectSpaceProvider(), new string[0], null);
			return application;
		}
	}

	public class TestApplication : XafApplication {
        class FinalizationMarker {
            ~FinalizationMarker() {
                TestApplication.DestructorCallCount++;
            }
        }
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly FinalizationMarker finalizationMarker = new FinalizationMarker();
		private IFrameTemplate defaultTemplate;
		private LayoutManager layoutManager;
		private ListEditor defaultListEditor;
        private ListView defaultListView;
        private DetailView defaultDetailView;
        private Type defaultLayoutManagerType;
		private Type defaultTemplateType;
        private Boolean delayedDetailViewDataLoading;

        public void OnLoggedOnForTests() {
            LogonEventArgs args = new LogonEventArgs(null);
            OnLoggedOn(args);
        }

		protected XPObjectSpaceProvider objectSpaceProvider;
		protected override void OnListViewCreating(ListViewCreatingEventArgs args) {
			base.OnListViewCreating(args);
			if(args.View == null) {
				args.View = defaultListView;
			}
		}
        protected override void OnDetailViewCreating(DetailViewCreatingEventArgs args) {
            base.OnDetailViewCreating(args);
            if(args.View == null) {
                args.View = defaultDetailView;
            }
        }
		protected override ListEditor CreateListEditorCore(IModelListView modelListView, CollectionSourceBase collectionSource) {
			ListEditor result = null;
			if(defaultListEditor != null) {
				result = defaultListEditor;
			}
			else {
				if(modelListView != null) {
					result = base.CreateListEditorCore(modelListView, collectionSource);
				}
				if(result == null) {
					result = new TestListEditor(modelListView);
				}
			}
			return result;
		}
		protected override IFrameTemplate CreateDefaultTemplate(TemplateContext context) {
			if(defaultTemplate != null) {
				return defaultTemplate;
			}
			else if(defaultTemplateType != null) {
				return (IFrameTemplate)Activator.CreateInstance(defaultTemplateType);
			}
			else {
				return new TestFrameTemplate();
			}
		}
		protected override ShowViewStrategyBase CreateShowViewStrategy() {
			return new TestShowViewStrategy(this);
		}
		protected override LayoutManager CreateLayoutManagerCore(bool simple) {
			if(layoutManager != null) {
				return layoutManager;
			}
			else if(defaultLayoutManagerType != null) {
				return (LayoutManager)Activator.CreateInstance(defaultLayoutManagerType);
			}
			else {
				return new TestLayoutManager();
			}
		}
		protected override List<Controller> CreateControllersCore(Type baseType, View view) {
            CreateControllersCore_CallCount++;
            CreateControllersCore_View = view;
            return base.CreateControllersCore(baseType, view);
		}
		protected override IList<Controller> CreateControllers(Type baseType, bool createAllControllers, ICollection<Controller> additional, View view) {
			CreateControllers_View = view;
			List<Controller> result = new List<Controller>();
			if(CreateAllControllers) {
				result.AddRange(base.CreateControllers(baseType, createAllControllers, additional, view));
			}
			Controllers.ForEach(delegate(Controller currentController) {
				if(CloneControllers) {
                    result.Add(currentController.Clone(Model));
				}
				else {
					result.Add(currentController);
				}
			});
			return result;
		}
		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			if(disposing) {
				if(objectSpaceProvider != null) {
					objectSpaceProvider.Dispose();
					objectSpaceProvider = null;
				}
			}
		}
		protected override void OnCustomCheckCompatibility(CustomCheckCompatibilityEventArgs args) {
			args.Handled = true;
			base.OnCustomCheckCompatibility(args);
		}
		protected override CollectionSourceBase CreateCollectionSourceCore(IObjectSpace objectSpace, Type objectType, CollectionSourceDataAccessMode dataAccessMode, CollectionSourceMode mode) {
			CreateCollectionSourceCore_IsServerMode = (dataAccessMode == CollectionSourceDataAccessMode.Server);
			return base.CreateCollectionSourceCore(objectSpace, objectType, dataAccessMode, mode);
		}
		protected override IObjectSpace CreateObjectSpaceCore(Type objectType) {
			LastCreatedObjectSpace = base.CreateObjectSpaceCore(objectType);
			return LastCreatedObjectSpace;
		}

		public TestApplication() {
			ShowViewStrategyBase newShowViewStrategyBase = CreateShowViewStrategy();
			if(newShowViewStrategyBase != null) {
				base.ShowViewStrategy = newShowViewStrategyBase;
			}
		}
		public TestApplication(IModelApplication modelApplication)
			: this() {
			SetModelApplication(modelApplication);
		}
		public TestApplication(IXpoDataStoreProvider dsProvider)
			: this() {
			objectSpaceProvider = new XPObjectSpaceProvider(dsProvider, true);
			Setup("test app", objectSpaceProvider);
		}
		public TestApplication(IObjectSpaceProvider provider)
			: this() {
			Setup("test app", provider);
		}
		public TestApplication(IXpoDataStoreProvider dsProvider, ApplicationModulesManager amm)
			: this() {
			objectSpaceProvider = new XPObjectSpaceProvider(dsProvider, true);
			Setup("test app", objectSpaceProvider, amm, null);
		}
		public TestApplication(IXpoDataStoreProvider dsProvider, IModelApplication modelApplication)
			: this() {
			objectSpaceProvider = new XPObjectSpaceProvider(dsProvider, true);
			Setup(new ExpressApplicationSetupParameters("test app",
				objectSpaceProvider,
				new ControllersManager(), Modules));
			SetModelApplication(modelApplication);
		}
		public TestApplication(IXpoDataStoreProvider dsProvider, ISecurity security)
			: this() {
			objectSpaceProvider = new XPObjectSpaceProvider(dsProvider, true);
			Setup(new ExpressApplicationSetupParameters("test app", security,
				objectSpaceProvider,
				new ControllersManager(), Modules));
		}
		public TestApplication(IEnumerable<ModuleBase> modules, IEnumerable<Type> domainComponents)
			: this() {
                Initialize(modules, domainComponents);
		}
        public override void LogOff()
        {
            base.OnLoggedOff();
        }
        private void Initialize(IEnumerable<ModuleBase> modules, IEnumerable<Type> domainComponents) {
            ModuleList moduleList = new ModuleList();
            foreach(ModuleBase module in modules) {
                moduleList.Add(module);
            }
            objectSpaceProvider = new XPObjectSpaceProvider(new MemoryDataStoreProvider(), true);
			ExpressApplicationSetupParameters parameters = new ExpressApplicationSetupParameters("test app",
                objectSpaceProvider,
                new ControllersManager(),
                moduleList);
            parameters.DomainComponents = domainComponents != null ? domainComponents : Type.EmptyTypes;
            Setup(parameters);
        }
		public override ConfirmationResult AskConfirmation(ConfirmationType confirmationType) {
			AskConfirmationCount++;
			return base.AskConfirmation(confirmationType);
		}
		public void InitializeCulturePublic(string userLanguage, string preferredLanguage) {
			SetLanguage(CalculateLanguageName(userLanguage, preferredLanguage));
			SetFormattingCulture(CalculateFormattingCultureName("", userLanguage, preferredLanguage));
		}
        public override IObjectSpace GetObjectSpaceToShowDetailViewFrom(Frame sourceFrame, Type objectType) {
            IObjectSpace result = ShowViewStrategy.NextObjectSpace;
            if(result == null) {
                result = base.GetObjectSpaceToShowDetailViewFrom(sourceFrame, objectType);
            }
            return result;
        }
        public override Boolean ShowDetailViewFrom(Frame sourceFrame) {
			if(ShowDetailViewFromResult != null) {
				return ShowDetailViewFromResult.Value;
			}
			else {
				return base.ShowDetailViewFrom(sourceFrame);
			}
		}
		public override ControllerType CreateController<ControllerType>() {
			LastCreatedController = base.CreateController<ControllerType>();
			return (ControllerType)LastCreatedController;
		}
        public new View CreateView(IModelView viewModel) {
            return base.CreateView(viewModel);
        }
		public new TestShowViewStrategy ShowViewStrategy {
			get { return (TestShowViewStrategy)base.ShowViewStrategy; }
		}
		public new Boolean IsDisposed {
			get { return base.IsDisposed; }
		}
		public LayoutManager DefaultLayoutManager {
			get { return layoutManager; }
			set { layoutManager = value; }
		}
		public Type DefaultLayoutManagerType {
			get { return defaultLayoutManagerType; }
			set { defaultLayoutManagerType = value; }
		}
		public Window DetailViewFrame {
			get { return MainWindow_; }
			set { MainWindow_ = value; }
		}
		public IFrameTemplate DefaultTemplate {
			get { return defaultTemplate; }
			set { defaultTemplate = value; }
		}
		public Type DefaultTemplateType {
			get { return defaultTemplateType; }
			set { defaultTemplateType = value; }
		}
		public override Window MainWindow { get { return MainWindow_; } }
		public ListEditor DefaultListEditor {
			get { return defaultListEditor; }
			set { defaultListEditor = value; }
		}
        public ListView DefaultListView {
            get { return defaultListView; }
            set { defaultListView = value; }
        }
        public DetailView DefaultDetailView {
            get { return defaultDetailView; }
            set { defaultDetailView = value; }
        }

		public Boolean CreateCollectionSourceCore_IsServerMode;
		public Controller LastCreatedController = null;
		public List<Controller> Controllers = new List<Controller>();
		public bool CreateAllControllers = true;
		public bool CloneControllers = false;
		public Boolean? ShowDetailViewFromResult;
		public Window MainWindow_;
		public int AskConfirmationCount = 0;
		public static int DestructorCallCount = 0;
        public override Boolean IsDelayedDetailViewDataLoadingEnabled {
            get { return delayedDetailViewDataLoading; }
            set { delayedDetailViewDataLoading = value; }
        }
        public int OnPopupWindowCreatingCounter = 0;
        public int OnPopupWindowCreatedCounter = 0;
		public IObjectSpace LastCreatedObjectSpace;
        public View CreateControllers_View;
        public View CreateControllersCore_View;
        public Int32 CreateControllersCore_CallCount;

        protected override void OnPopupWindowCreating(TemplateContext context, ICollection<Controller> controllers) {
            OnPopupWindowCreatingCounter++;
            base.OnPopupWindowCreating(context, controllers);
        }
        protected override void OnPopupWindowCreated(Window popupWindow) {
            OnPopupWindowCreatedCounter++;
            base.OnPopupWindowCreated(popupWindow);
        }
	}
	
	public class SharedTestApplication : TestApplication {
		private void SharedTestApplication_CreateCustomObjectSpaceProvider(object sender, CreateCustomObjectSpaceProviderEventArgs e) {
			objectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, true);
			e.ObjectSpaceProvider = objectSpaceProvider;
		}
		public static IXpoDataStoreProvider dataStoreProvider;
        public static void ClearStaticMembers() {
            dataStoreProvider = null;
            CustomizeApplication = null;
        }
        protected override bool IsSharedModel {
            get {
                return true;
            }
        }
		public SharedTestApplication() {
			if(CustomizeApplication != null) {
				CustomizeApplication(this, EventArgs.Empty);
			}
			this.CreateCustomObjectSpaceProvider += new EventHandler<CreateCustomObjectSpaceProviderEventArgs>(SharedTestApplication_CreateCustomObjectSpaceProvider);
		}
		public static void ClearCustomizeModules() {
			CustomizeApplication = null;
		}
		public static event EventHandler CustomizeApplication;
	}
}
#endif
