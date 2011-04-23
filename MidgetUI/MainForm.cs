using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

using DockingSuite;

using Midget;
using Midget.Cameras;
using MidgetCommand;

namespace MidgetUI
{
	/// <summary>
	/// the main window used to display the 4 viewports and menu controls
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuPrimatives;
		private System.Windows.Forms.MenuItem mnuPrimativeMesh;
		private System.Windows.Forms.MenuItem mnuPrimativePolygon;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshTeapot;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshSphere;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshTorus;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshBox;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshCylinder;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshPolygon;
		private System.Windows.Forms.MenuItem mnuPrimativesMeshText;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuEditUndo;
		private System.Windows.Forms.MenuItem mnuEditRedo;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ToolBar toolbar;
		private System.Windows.Forms.ImageList toolbarIcons;
		private System.Windows.Forms.MenuItem mnuFileOpen;
		private System.Windows.Forms.MenuItem mnuFileNew;
		private System.Windows.Forms.MenuItem mnuFileSave;
		private System.Windows.Forms.ToolBarButton toolbarDelete;
		private System.Windows.Forms.ToolBarButton toolbarNew;
		private System.Windows.Forms.ToolBarButton toolbarOpen;
		private System.Windows.Forms.ToolBarButton toolbarSave;
		private System.Windows.Forms.ToolBarButton toolbarSpaceA1;
		private System.Windows.Forms.ToolBarButton toolbarSpaceB1;
		private System.Windows.Forms.ToolBarButton toolbarSpaceB2;
		private System.Windows.Forms.ToolBarButton toolbarSpaceC1;
		private System.Windows.Forms.ToolBarButton toolbarSpaceC2;
		private System.Windows.Forms.ToolBarButton toolbarMove;
		private System.Windows.Forms.ToolBarButton toolbarRotate;
		private System.Windows.Forms.ToolBarButton toolbarScale;
		private System.Windows.Forms.ToolBarButton toolbarSelect;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem mnuDynamics;
		private System.Windows.Forms.MenuItem mnuForces;
		private System.Windows.Forms.MenuItem mnuGravity;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem mnuDynamicsToKeyframes;
		private System.Windows.Forms.MenuItem mnuRender;
		private System.Windows.Forms.MenuItem mnuRenderRenderCurrent;
		private System.Windows.Forms.MenuItem mnuRenderSettings;
		private System.Windows.Forms.MenuItem mnuRenderBatch;
		private RenderPreview currentFrameRenderWindow;
		
		private FileRenderSettings renderSettings;

		private RenderSettingsForm renderSettingsForm;
		private System.Windows.Forms.MenuItem mnuModify;
		private System.Windows.Forms.MenuItem mnuModifyGroup;
		private System.Windows.Forms.MenuItem mnuModifyUngroup;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuViewShowObjectPanel;
		private System.Windows.Forms.MenuItem mnuViewShowModifierPanel;
		private System.Windows.Forms.MenuItem mnuPrimativesCurves;
		private System.Windows.Forms.MenuItem mnuCurvesNewCurve;
		private System.Windows.Forms.MenuItem mnuCurvesAddControlPoint;
		private System.Windows.Forms.MenuItem mnuCurveSelectPath;
		private System.Windows.Forms.MenuItem mnuSpacer1;
		private System.Windows.Forms.MenuItem mnuSpacer2;
		private System.Windows.Forms.MenuItem mnuDynamicsAddParticleSystem;
		private System.Windows.Forms.MenuItem mnuViewObjectSelector;
		private System.Windows.Forms.MenuItem mnuViewShowMaterialEditor;
		private MidgetUI.TabBar tabBar1;
		private DockingSuite.DockHost dockHost;
		private DockingSuite.DockPanel dockPanel;
		private DockingSuite.DockControl objectDockControl;
		private MidgetUI.ObjectShelf objectShelf;
		private DockingSuite.DockControl modifierDockControl;
		private DockingSuite.DockControl objectSelectorDock;
		private MidgetUI.ObjectSelector objectSelector1;
		private DockingSuite.DockControl materialsDock;
		private MidgetUI.MaterialEditor materialEditor1;
		private System.Windows.Forms.Panel pnlMain;
		private TimerControlProject.TimelineControl timelineControl;
		private Midget.DXViewPort dxViewPort;
		private MidgetUI.ModifierShelf modifierShelf1;
		private System.Windows.Forms.MenuItem mnuForcesForce;

		private ArrayList selectedObjects;

		public MainForm()
		{
			// TODO:	get rid of this crap. This is a temporary fix to the MagicShelf 
			//			selection bug
			string something = Midget.Command.CommandManager.Instance.ToString();
			something = DeviceManager.Instance.ToString();	// init the DM
			something = SceneManager.Instance.ToString();	// init the SM
			
			selectedObjects = new ArrayList();
			
			Midget.Events.EventFactory.DeleteObject +=new Midget.Events.Object.Lifetime.DeleteObjectEventHandler(EventFactory_DeleteObject);
			Midget.Events.EventFactory.SelectAdditionalObject +=new Midget.Events.Object.Selection.SelectAdditionalObjectEventHandler(EventFactory_SelectAdditionalObject);
			Midget.Events.EventFactory.DeselectObjects +=new Midget.Events.Object.Selection.DeselectObjectEventHandler(EventFactory_DeselectObjects);
			Midget.Events.EventFactory.CreateObject +=new Midget.Events.Object.Lifetime.CreateObjectEventHandler(EventFactory_CreateObject);
			Midget.Events.EventFactory.Transformation +=new Midget.Events.Object.Transformation.TransformationEventHandler(EventFactory_Transformation);

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			renderSettings = new FileRenderSettings();
			renderSettings.Camera = CameraFactory.Instance.GetExistingCamera(0);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileNew = new System.Windows.Forms.MenuItem();
			this.mnuFileOpen = new System.Windows.Forms.MenuItem();
			this.mnuFileSave = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuEdit = new System.Windows.Forms.MenuItem();
			this.mnuEditUndo = new System.Windows.Forms.MenuItem();
			this.mnuEditRedo = new System.Windows.Forms.MenuItem();
			this.mnuView = new System.Windows.Forms.MenuItem();
			this.mnuViewShowObjectPanel = new System.Windows.Forms.MenuItem();
			this.mnuViewShowModifierPanel = new System.Windows.Forms.MenuItem();
			this.mnuViewObjectSelector = new System.Windows.Forms.MenuItem();
			this.mnuViewShowMaterialEditor = new System.Windows.Forms.MenuItem();
			this.mnuModify = new System.Windows.Forms.MenuItem();
			this.mnuModifyGroup = new System.Windows.Forms.MenuItem();
			this.mnuModifyUngroup = new System.Windows.Forms.MenuItem();
			this.mnuPrimatives = new System.Windows.Forms.MenuItem();
			this.mnuPrimativeMesh = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshTeapot = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshSphere = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshTorus = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshBox = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshCylinder = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshPolygon = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesMeshText = new System.Windows.Forms.MenuItem();
			this.mnuPrimativePolygon = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuPrimativesCurves = new System.Windows.Forms.MenuItem();
			this.mnuCurvesNewCurve = new System.Windows.Forms.MenuItem();
			this.mnuCurvesAddControlPoint = new System.Windows.Forms.MenuItem();
			this.mnuCurveSelectPath = new System.Windows.Forms.MenuItem();
			this.mnuDynamics = new System.Windows.Forms.MenuItem();
			this.mnuForces = new System.Windows.Forms.MenuItem();
			this.mnuGravity = new System.Windows.Forms.MenuItem();
			this.mnuForcesForce = new System.Windows.Forms.MenuItem();
			this.mnuDynamicsToKeyframes = new System.Windows.Forms.MenuItem();
			this.mnuDynamicsAddParticleSystem = new System.Windows.Forms.MenuItem();
			this.mnuRender = new System.Windows.Forms.MenuItem();
			this.mnuRenderRenderCurrent = new System.Windows.Forms.MenuItem();
			this.mnuSpacer1 = new System.Windows.Forms.MenuItem();
			this.mnuRenderSettings = new System.Windows.Forms.MenuItem();
			this.mnuSpacer2 = new System.Windows.Forms.MenuItem();
			this.mnuRenderBatch = new System.Windows.Forms.MenuItem();
			this.toolbar = new System.Windows.Forms.ToolBar();
			this.toolbarSpaceA1 = new System.Windows.Forms.ToolBarButton();
			this.toolbarNew = new System.Windows.Forms.ToolBarButton();
			this.toolbarOpen = new System.Windows.Forms.ToolBarButton();
			this.toolbarSave = new System.Windows.Forms.ToolBarButton();
			this.toolbarSpaceB1 = new System.Windows.Forms.ToolBarButton();
			this.toolbarSpaceB2 = new System.Windows.Forms.ToolBarButton();
			this.toolbarSelect = new System.Windows.Forms.ToolBarButton();
			this.toolbarMove = new System.Windows.Forms.ToolBarButton();
			this.toolbarRotate = new System.Windows.Forms.ToolBarButton();
			this.toolbarScale = new System.Windows.Forms.ToolBarButton();
			this.toolbarSpaceC1 = new System.Windows.Forms.ToolBarButton();
			this.toolbarSpaceC2 = new System.Windows.Forms.ToolBarButton();
			this.toolbarDelete = new System.Windows.Forms.ToolBarButton();
			this.toolbarIcons = new System.Windows.Forms.ImageList(this.components);
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.tabBar1 = new MidgetUI.TabBar();
			this.dockHost = new DockingSuite.DockHost();
			this.dockPanel = new DockingSuite.DockPanel();
			this.objectDockControl = new DockingSuite.DockControl();
			this.objectShelf = new MidgetUI.ObjectShelf();
			this.modifierDockControl = new DockingSuite.DockControl();
			this.modifierShelf1 = new MidgetUI.ModifierShelf();
			this.objectSelectorDock = new DockingSuite.DockControl();
			this.objectSelector1 = new MidgetUI.ObjectSelector();
			this.materialsDock = new DockingSuite.DockControl();
			this.materialEditor1 = new MidgetUI.MaterialEditor();
			this.pnlMain = new System.Windows.Forms.Panel();
			this.timelineControl = new TimerControlProject.TimelineControl();
			this.dxViewPort = new Midget.DXViewPort();
			this.dockHost.SuspendLayout();
			this.dockPanel.SuspendLayout();
			this.objectDockControl.SuspendLayout();
			this.modifierDockControl.SuspendLayout();
			this.objectSelectorDock.SuspendLayout();
			this.materialsDock.SuspendLayout();
			this.pnlMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile,
																					this.mnuEdit,
																					this.mnuView,
																					this.mnuModify,
																					this.mnuPrimatives,
																					this.mnuDynamics,
																					this.mnuRender});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileNew,
																					this.mnuFileOpen,
																					this.mnuFileSave,
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileNew
			// 
			this.mnuFileNew.Index = 0;
			this.mnuFileNew.Text = "&New";
			this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Index = 1;
			this.mnuFileOpen.MdiList = true;
			this.mnuFileOpen.Text = "&Open";
			this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
			// 
			// mnuFileSave
			// 
			this.mnuFileSave.Index = 2;
			this.mnuFileSave.Text = "&Save";
			this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 3;
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Index = 1;
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuEditUndo,
																					this.mnuEditRedo});
			this.mnuEdit.Text = "&Edit";
			this.mnuEdit.Popup += new System.EventHandler(this.mnuEdit_Popup);
			// 
			// mnuEditUndo
			// 
			this.mnuEditUndo.Enabled = ((bool)(configurationAppSettings.GetValue("mnuEditUndo.Enabled", typeof(bool))));
			this.mnuEditUndo.Index = 0;
			this.mnuEditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.mnuEditUndo.Text = "&Undo";
			this.mnuEditUndo.Click += new System.EventHandler(this.mnuEditUndo_Click);
			// 
			// mnuEditRedo
			// 
			this.mnuEditRedo.Enabled = ((bool)(configurationAppSettings.GetValue("mnuEditRedo.Enabled", typeof(bool))));
			this.mnuEditRedo.Index = 1;
			this.mnuEditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.mnuEditRedo.Text = "&Redo";
			this.mnuEditRedo.Click += new System.EventHandler(this.mnuEditRedo_Click);
			// 
			// mnuView
			// 
			this.mnuView.Index = 2;
			this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuViewShowObjectPanel,
																					this.mnuViewShowModifierPanel,
																					this.mnuViewObjectSelector,
																					this.mnuViewShowMaterialEditor});
			this.mnuView.Text = "&View";
			// 
			// mnuViewShowObjectPanel
			// 
			this.mnuViewShowObjectPanel.Index = 0;
			this.mnuViewShowObjectPanel.Text = "Show &Object Panel";
			this.mnuViewShowObjectPanel.Click += new System.EventHandler(this.mnuViewShowObjectDock_Click);
			// 
			// mnuViewShowModifierPanel
			// 
			this.mnuViewShowModifierPanel.Index = 1;
			this.mnuViewShowModifierPanel.Text = "Show &Modifier Panel";
			this.mnuViewShowModifierPanel.Click += new System.EventHandler(this.mnuViewShowModifierPanel_Click);
			// 
			// mnuViewObjectSelector
			// 
			this.mnuViewObjectSelector.Index = 2;
			this.mnuViewObjectSelector.Text = "Show &Object Selector";
			this.mnuViewObjectSelector.Click += new System.EventHandler(this.mnuViewObjectSelector_Click);
			// 
			// mnuViewShowMaterialEditor
			// 
			this.mnuViewShowMaterialEditor.Index = 3;
			this.mnuViewShowMaterialEditor.Text = "Show Material Editor";
			this.mnuViewShowMaterialEditor.Click += new System.EventHandler(this.mnuViewShowMaterialEditor_Click);
			// 
			// mnuModify
			// 
			this.mnuModify.Index = 3;
			this.mnuModify.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuModifyGroup,
																					  this.mnuModifyUngroup});
			this.mnuModify.Text = "&Modify";
			// 
			// mnuModifyGroup
			// 
			this.mnuModifyGroup.Index = 0;
			this.mnuModifyGroup.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
			this.mnuModifyGroup.Text = "&Group";
			this.mnuModifyGroup.Click += new System.EventHandler(this.mnuModifyGroup_Click);
			// 
			// mnuModifyUngroup
			// 
			this.mnuModifyUngroup.Index = 1;
			this.mnuModifyUngroup.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftG;
			this.mnuModifyUngroup.Text = "&Ungroup";
			this.mnuModifyUngroup.Click += new System.EventHandler(this.mnuModifyUngroup_Click);
			// 
			// mnuPrimatives
			// 
			this.mnuPrimatives.Index = 4;
			this.mnuPrimatives.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuPrimativeMesh,
																						  this.mnuPrimativePolygon,
																						  this.mnuPrimativesCurves});
			this.mnuPrimatives.Text = "&Primatives";
			// 
			// mnuPrimativeMesh
			// 
			this.mnuPrimativeMesh.Index = 0;
			this.mnuPrimativeMesh.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.mnuPrimativesMeshTeapot,
																							 this.mnuPrimativesMeshSphere,
																							 this.mnuPrimativesMeshTorus,
																							 this.mnuPrimativesMeshBox,
																							 this.mnuPrimativesMeshCylinder,
																							 this.mnuPrimativesMeshPolygon,
																							 this.mnuPrimativesMeshText});
			this.mnuPrimativeMesh.Text = "&Mesh";
			// 
			// mnuPrimativesMeshTeapot
			// 
			this.mnuPrimativesMeshTeapot.Index = 0;
			this.mnuPrimativesMeshTeapot.Text = "&Teapot";
			this.mnuPrimativesMeshTeapot.Click += new System.EventHandler(this.mnuPrimativesMeshTeapot_Click);
			// 
			// mnuPrimativesMeshSphere
			// 
			this.mnuPrimativesMeshSphere.Index = 1;
			this.mnuPrimativesMeshSphere.Text = "&Sphere";
			this.mnuPrimativesMeshSphere.Click += new System.EventHandler(this.mnuPrimativesMeshSphere_Click);
			// 
			// mnuPrimativesMeshTorus
			// 
			this.mnuPrimativesMeshTorus.Index = 2;
			this.mnuPrimativesMeshTorus.Text = "T&orus";
			this.mnuPrimativesMeshTorus.Click += new System.EventHandler(this.mnuPrimativesMeshTorus_Click);
			// 
			// mnuPrimativesMeshBox
			// 
			this.mnuPrimativesMeshBox.Index = 3;
			this.mnuPrimativesMeshBox.Text = "&Box";
			this.mnuPrimativesMeshBox.Click += new System.EventHandler(this.mnuPrimativesMeshBox_Click);
			// 
			// mnuPrimativesMeshCylinder
			// 
			this.mnuPrimativesMeshCylinder.Index = 4;
			this.mnuPrimativesMeshCylinder.Text = "&Cylinder";
			this.mnuPrimativesMeshCylinder.Click += new System.EventHandler(this.mnuPrimativesMeshCylinder_Click);
			// 
			// mnuPrimativesMeshPolygon
			// 
			this.mnuPrimativesMeshPolygon.Index = 5;
			this.mnuPrimativesMeshPolygon.Text = "N-sided &Polygon";
			this.mnuPrimativesMeshPolygon.Click += new System.EventHandler(this.mnuPrimativesMeshPolygon_Click);
			// 
			// mnuPrimativesMeshText
			// 
			this.mnuPrimativesMeshText.Index = 6;
			this.mnuPrimativesMeshText.Text = "Text";
			this.mnuPrimativesMeshText.Click += new System.EventHandler(this.mnuPrimativesMeshText_Click);
			// 
			// mnuPrimativePolygon
			// 
			this.mnuPrimativePolygon.Index = 1;
			this.mnuPrimativePolygon.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.menuItem1});
			this.mnuPrimativePolygon.Text = "&Polygon";
			this.mnuPrimativePolygon.Click += new System.EventHandler(this.mnuPrimativePolygon_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "[Empty]";
			// 
			// mnuPrimativesCurves
			// 
			this.mnuPrimativesCurves.Index = 2;
			this.mnuPrimativesCurves.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.mnuCurvesNewCurve,
																								this.mnuCurvesAddControlPoint,
																								this.mnuCurveSelectPath});
			this.mnuPrimativesCurves.Text = "&Curves";
			// 
			// mnuCurvesNewCurve
			// 
			this.mnuCurvesNewCurve.Index = 0;
			this.mnuCurvesNewCurve.Text = "&New Curve";
			this.mnuCurvesNewCurve.Click += new System.EventHandler(this.mnuCurvesNewCurve_Click);
			// 
			// mnuCurvesAddControlPoint
			// 
			this.mnuCurvesAddControlPoint.Index = 1;
			this.mnuCurvesAddControlPoint.Text = "&Add Control Point";
			this.mnuCurvesAddControlPoint.Click += new System.EventHandler(this.mnuCurvesAddControlPoint_Click);
			// 
			// mnuCurveSelectPath
			// 
			this.mnuCurveSelectPath.Index = 2;
			this.mnuCurveSelectPath.Text = "&Select Curve As Path";
			this.mnuCurveSelectPath.Click += new System.EventHandler(this.mnuCurveSelectPath_Click);
			// 
			// mnuDynamics
			// 
			this.mnuDynamics.Index = 5;
			this.mnuDynamics.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuForces,
																						this.mnuDynamicsAddParticleSystem});
			this.mnuDynamics.Text = "&Dynamics";
			// 
			// mnuForces
			// 
			this.mnuForces.Index = 0;
			this.mnuForces.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuGravity,
																					  this.mnuForcesForce,
																					  this.mnuDynamicsToKeyframes});
			this.mnuForces.Text = "Forces";
			// 
			// mnuGravity
			// 
			this.mnuGravity.Index = 0;
			this.mnuGravity.Text = "&Gravity";
			this.mnuGravity.Click += new System.EventHandler(this.mnuGravity_Click);
			// 
			// mnuForcesForce
			// 
			this.mnuForcesForce.Index = 1;
			this.mnuForcesForce.Text = "&Force";
			this.mnuForcesForce.Click += new System.EventHandler(this.mnuForcesForce_Click);
			// 
			// mnuDynamicsToKeyframes
			// 
			this.mnuDynamicsToKeyframes.Index = 2;
			this.mnuDynamicsToKeyframes.Text = "Forces to &Keyframes";
			this.mnuDynamicsToKeyframes.Click += new System.EventHandler(this.mnuDynamicsToKeyframes_Click);
			// 
			// mnuDynamicsAddParticleSystem
			// 
			this.mnuDynamicsAddParticleSystem.Index = 1;
			this.mnuDynamicsAddParticleSystem.Text = "Add &Particle System";
			this.mnuDynamicsAddParticleSystem.Click += new System.EventHandler(this.mnuDynamicsAddParticleSystem_Click);
			// 
			// mnuRender
			// 
			this.mnuRender.Index = 6;
			this.mnuRender.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuRenderRenderCurrent,
																					  this.mnuSpacer1,
																					  this.mnuRenderSettings,
																					  this.mnuSpacer2,
																					  this.mnuRenderBatch});
			this.mnuRender.Text = "&Render";
			// 
			// mnuRenderRenderCurrent
			// 
			this.mnuRenderRenderCurrent.Index = 0;
			this.mnuRenderRenderCurrent.Text = "&Render Current Frame";
			this.mnuRenderRenderCurrent.Click += new System.EventHandler(this.mnuRenderRenderCurrent_Click);
			// 
			// mnuSpacer1
			// 
			this.mnuSpacer1.Index = 1;
			this.mnuSpacer1.Text = "-";
			// 
			// mnuRenderSettings
			// 
			this.mnuRenderSettings.Index = 2;
			this.mnuRenderSettings.Text = "Render &Settings";
			this.mnuRenderSettings.Click += new System.EventHandler(this.mnuRenderSettings_Click);
			// 
			// mnuSpacer2
			// 
			this.mnuSpacer2.Index = 3;
			this.mnuSpacer2.Text = "-";
			// 
			// mnuRenderBatch
			// 
			this.mnuRenderBatch.Index = 4;
			this.mnuRenderBatch.Text = "&Batch Render";
			this.mnuRenderBatch.Click += new System.EventHandler(this.mnuRenderBatch_Click);
			// 
			// toolbar
			// 
			this.toolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.toolbarSpaceA1,
																					   this.toolbarNew,
																					   this.toolbarOpen,
																					   this.toolbarSave,
																					   this.toolbarSpaceB1,
																					   this.toolbarSpaceB2,
																					   this.toolbarSelect,
																					   this.toolbarMove,
																					   this.toolbarRotate,
																					   this.toolbarScale,
																					   this.toolbarSpaceC1,
																					   this.toolbarSpaceC2,
																					   this.toolbarDelete});
			this.toolbar.DropDownArrows = true;
			this.toolbar.ImageList = this.toolbarIcons;
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.ShowToolTips = true;
			this.toolbar.Size = new System.Drawing.Size(992, 28);
			this.toolbar.TabIndex = 2;
			this.toolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolbar_ButtonClick);
			// 
			// toolbarSpaceA1
			// 
			this.toolbarSpaceA1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolbarNew
			// 
			this.toolbarNew.ImageIndex = 0;
			this.toolbarNew.ToolTipText = "New Scene";
			// 
			// toolbarOpen
			// 
			this.toolbarOpen.ImageIndex = 1;
			this.toolbarOpen.ToolTipText = "Open a file";
			// 
			// toolbarSave
			// 
			this.toolbarSave.ImageIndex = 2;
			this.toolbarSave.ToolTipText = "Save scene";
			// 
			// toolbarSpaceB1
			// 
			this.toolbarSpaceB1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolbarSpaceB2
			// 
			this.toolbarSpaceB2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolbarSelect
			// 
			this.toolbarSelect.ImageIndex = 7;
			this.toolbarSelect.Pushed = true;
			this.toolbarSelect.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolbarSelect.ToolTipText = "Enter object selection mode";
			// 
			// toolbarMove
			// 
			this.toolbarMove.ImageIndex = 4;
			this.toolbarMove.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolbarMove.ToolTipText = "Move selected object";
			// 
			// toolbarRotate
			// 
			this.toolbarRotate.ImageIndex = 5;
			this.toolbarRotate.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolbarRotate.ToolTipText = "Rotate selected object";
			// 
			// toolbarScale
			// 
			this.toolbarScale.ImageIndex = 6;
			this.toolbarScale.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolbarScale.ToolTipText = "Scale selected object";
			// 
			// toolbarSpaceC1
			// 
			this.toolbarSpaceC1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolbarSpaceC2
			// 
			this.toolbarSpaceC2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolbarDelete
			// 
			this.toolbarDelete.ImageIndex = 3;
			this.toolbarDelete.ToolTipText = "Delete";
			// 
			// toolbarIcons
			// 
			this.toolbarIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarIcons.ImageStream")));
			this.toolbarIcons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// openFileDialog
			// 
			this.openFileDialog.RestoreDirectory = true;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.RestoreDirectory = true;
			// 
			// tabBar1
			// 
			this.tabBar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabBar1.Location = new System.Drawing.Point(0, 28);
			this.tabBar1.Name = "tabBar1";
			this.tabBar1.Size = new System.Drawing.Size(992, 56);
			this.tabBar1.TabIndex = 13;
			// 
			// dockHost
			// 
			this.dockHost.Colors.ActiveTitleBarBackground1 = System.Drawing.SystemColors.Highlight;
			this.dockHost.Colors.ActiveTitleBarBackground2 = System.Drawing.SystemColors.InactiveCaptionText;
			this.dockHost.Controls.Add(this.dockPanel);
			this.dockHost.Dock = System.Windows.Forms.DockStyle.Right;
			this.dockHost.Location = new System.Drawing.Point(736, 84);
			this.dockHost.Name = "dockHost";
			this.dockHost.Size = new System.Drawing.Size(256, 569);
			this.dockHost.TabIndex = 14;
			// 
			// dockPanel
			// 
			this.dockPanel.AutoHide = false;
			this.dockPanel.Controls.Add(this.objectDockControl);
			this.dockPanel.Controls.Add(this.modifierDockControl);
			this.dockPanel.Controls.Add(this.objectSelectorDock);
			this.dockPanel.Controls.Add(this.materialsDock);
			this.dockPanel.DockedHeight = 569;
			this.dockPanel.DockedWidth = 0;
			this.dockPanel.FloatingHeight = 400;
			this.dockPanel.Location = new System.Drawing.Point(4, 0);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.SelectedTab = this.objectDockControl;
			this.dockPanel.Size = new System.Drawing.Size(252, 569);
			this.dockPanel.TabIndex = 0;
			this.dockPanel.Text = "Docked Panel";
			// 
			// objectDockControl
			// 
			this.objectDockControl.Controls.Add(this.objectShelf);
			this.objectDockControl.Guid = new System.Guid("ca5001ed-0960-40fc-92a4-87977bd62c73");
			this.objectDockControl.Location = new System.Drawing.Point(0, 20);
			this.objectDockControl.Name = "objectDockControl";
			this.objectDockControl.Size = new System.Drawing.Size(252, 526);
			this.objectDockControl.TabImage = null;
			this.objectDockControl.TabIndex = 0;
			this.objectDockControl.Text = "Object";
			// 
			// objectShelf
			// 
			this.objectShelf.AutoScroll = true;
			this.objectShelf.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectShelf.Location = new System.Drawing.Point(0, 0);
			this.objectShelf.Name = "objectShelf";
			this.objectShelf.Size = new System.Drawing.Size(252, 526);
			this.objectShelf.TabIndex = 0;
			// 
			// modifierDockControl
			// 
			this.modifierDockControl.Controls.Add(this.modifierShelf1);
			this.modifierDockControl.Guid = new System.Guid("72738861-35cf-48e6-9130-4557f8e0acf1");
			this.modifierDockControl.Location = new System.Drawing.Point(0, 20);
			this.modifierDockControl.Name = "modifierDockControl";
			this.modifierDockControl.Size = new System.Drawing.Size(252, 526);
			this.modifierDockControl.TabImage = null;
			this.modifierDockControl.TabIndex = 1;
			this.modifierDockControl.Text = "Modifiers";
			this.modifierDockControl.Visible = false;
			// 
			// modifierShelf1
			// 
			this.modifierShelf1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modifierShelf1.Location = new System.Drawing.Point(0, 0);
			this.modifierShelf1.Name = "modifierShelf1";
			this.modifierShelf1.Size = new System.Drawing.Size(252, 526);
			this.modifierShelf1.TabIndex = 0;
			// 
			// objectSelectorDock
			// 
			this.objectSelectorDock.Controls.Add(this.objectSelector1);
			this.objectSelectorDock.Guid = new System.Guid("e67b9fdb-1e3c-4bb6-bf0e-aa331bdada30");
			this.objectSelectorDock.Location = new System.Drawing.Point(0, 20);
			this.objectSelectorDock.Name = "objectSelectorDock";
			this.objectSelectorDock.Size = new System.Drawing.Size(252, 526);
			this.objectSelectorDock.TabImage = null;
			this.objectSelectorDock.TabIndex = 2;
			this.objectSelectorDock.Text = "Groups";
			this.objectSelectorDock.Visible = false;
			// 
			// objectSelector1
			// 
			this.objectSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectSelector1.Location = new System.Drawing.Point(0, 0);
			this.objectSelector1.Name = "objectSelector1";
			this.objectSelector1.Size = new System.Drawing.Size(252, 526);
			this.objectSelector1.TabIndex = 0;
			// 
			// materialsDock
			// 
			this.materialsDock.AutoScroll = true;
			this.materialsDock.Controls.Add(this.materialEditor1);
			this.materialsDock.Guid = new System.Guid("7962550c-2643-442f-9865-229eb1f40451");
			this.materialsDock.Location = new System.Drawing.Point(0, 20);
			this.materialsDock.Name = "materialsDock";
			this.materialsDock.Size = new System.Drawing.Size(252, 526);
			this.materialsDock.TabImage = null;
			this.materialsDock.TabIndex = 3;
			this.materialsDock.Text = "Materials";
			this.materialsDock.Visible = false;
			// 
			// materialEditor1
			// 
			this.materialEditor1.AutoScroll = true;
			this.materialEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditor1.Location = new System.Drawing.Point(0, 0);
			this.materialEditor1.Name = "materialEditor1";
			this.materialEditor1.Size = new System.Drawing.Size(252, 526);
			this.materialEditor1.TabIndex = 0;
			// 
			// pnlMain
			// 
			this.pnlMain.Controls.Add(this.timelineControl);
			this.pnlMain.Controls.Add(this.dxViewPort);
			this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMain.Location = new System.Drawing.Point(0, 84);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Size = new System.Drawing.Size(736, 569);
			this.pnlMain.TabIndex = 15;
			// 
			// timelineControl
			// 
			this.timelineControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.timelineControl.Location = new System.Drawing.Point(0, 489);
			this.timelineControl.Name = "timelineControl";
			this.timelineControl.Size = new System.Drawing.Size(736, 80);
			this.timelineControl.TabIndex = 14;
			// 
			// dxViewPort
			// 
			this.dxViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dxViewPort.Location = new System.Drawing.Point(0, 0);
			this.dxViewPort.Name = "dxViewPort";
			this.dxViewPort.Size = new System.Drawing.Size(736, 569);
			this.dxViewPort.TabIndex = 13;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 653);
			this.Controls.Add(this.pnlMain);
			this.Controls.Add(this.dockHost);
			this.Controls.Add(this.tabBar1);
			this.Controls.Add(this.toolbar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Menu = this.mnuMain;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Midget";
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.dockHost.ResumeLayout(false);
			this.dockPanel.ResumeLayout(false);
			this.objectDockControl.ResumeLayout(false);
			this.modifierDockControl.ResumeLayout(false);
			this.objectSelectorDock.ResumeLayout(false);
			this.materialsDock.ResumeLayout(false);
			this.pnlMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// try/catch block for user testing, not debugging
#if (!DEBUG)
			try
			{
#endif
			Application.Run(new MainForm());
#if (!DEBUG)
			}
			catch (Exception e)
			{
#if (!DEBUG)

				StreamWriter writer = File.AppendText(Application.StartupPath + "\\debug.log");
				//writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToShortDateString());
				//writer.WriteLine("  :");
				//writer.WriteLine("  :{0}", message);
				writer.WriteLine("|\t" + e.Message + "\n\n\n" + e.StackTrace);
				writer.WriteLine("|----------------------------------------");
				writer.Flush();
				writer.Close();				
				MessageBox.Show("Somebody set up us the bomb!  Thank you for helping make Midget a better product"); 
#endif
				
#if (DEBUG)
				MessageBox.Show("Somebody set up us the bomb! (Details below.)\n\n" + e.Message + 
					"\n\n\n" + e.StackTrace, "Holy crap! Your computer sucks!", MessageBoxButtons.OK, 
					MessageBoxIcon.Hand);
#endif
			}
#endif
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
#if (!DEBUG)	
			frmSplash SplashScreen = new frmSplash();
#endif
			
			// print the app name, and current app build #
			this.Text = "Midget 3D - [build " +
				Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + 
				Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." + 
				Assembly.GetExecutingAssembly().GetName().Version.Build.ToString() + 
				"]";

			// re-center the viewport control
			dxViewPort.ReCenter();

			// initialize the file dialogs
			openFileDialog.InitialDirectory = Application.LocalUserAppDataPath;
			openFileDialog.Filter = "Midget3D files (*.mig)|*.mig|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 1;
			saveFileDialog.InitialDirectory = Application.LocalUserAppDataPath;
			saveFileDialog.Filter = "Midget3D files (*.mig)|*.mig|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 1;
		}

		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mnuPrimativesMeshTeapot_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshTeapot, null);
		}

		private void mnuPrimativesMeshSphere_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshSphere, null);
		}

		private void mnuPrimativesMeshTorus_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshTorus, null);
		}

		private void mnuPrimativesMeshBox_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshBox, null);
		}

		private void mnuPrimativesMeshCylinder_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshCylinder, null);
		}

		private void mnuPrimativesMeshPolygon_Click(object sender, System.EventArgs e)
		{
			InputBoxResult result = InputBox.Show("Number of sides:", "Edit Polygon", "5", null);
			
			if (result.OK)
			{
				Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshPolygon, result.Text);
			}
		}

		private void mnuPrimativesMeshText_Click(object sender, System.EventArgs e)
		{
			InputBoxResult result = InputBox.Show("Please enter text to be displayed:", "Edit Text", "Midget3D", null);
			
			if (result.OK)
			{
				Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.MeshText, result.Text);
			}
		}

		private void mnuPrimativePolygon_Click(object sender, System.EventArgs e)
		{
			// TODO - primitive polygon
		}

		private void mnuEditUndo_Click(object sender, System.EventArgs e)
		{			
			if(Midget.Command.CommandManager.Instance.UndoAvailable)
				Midget.Command.CommandManager.Instance.Undo();
		}

		private void mnuEditRedo_Click(object sender, System.EventArgs e)
		{
			if(Midget.Command.CommandManager.Instance.RedoAvailable)
				Midget.Command.CommandManager.Instance.Redo();
		}

		private void mnuEdit_Popup(object sender, System.EventArgs e)
		{
			this.mnuEditUndo.Enabled = Midget.Command.CommandManager.Instance.UndoAvailable;
			this.mnuEditRedo.Enabled = Midget.Command.CommandManager.Instance.RedoAvailable;
		
		}

		private void toolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolbarNew)
			{
				NewFile();
			}
			else if (e.Button == toolbarOpen)
			{
				OpenFile();
			}
			else if (e.Button == toolbarSave)
			{
				SaveFile();
			}
			else if (e.Button == toolbarDelete)
			{
				Midget.Events.EventFactory.Instance.GenerateDeleteObjectRequestEvent(this,selectedObjects);
			}
			else if (e.Button == toolbarSelect)
			{
				Midget.Events.EventFactory.Instance.GenerateSwitchEditModeEvent(this,EditMode.None);
				toolbarMove.Pushed = false;
				toolbarRotate.Pushed = false;
				toolbarScale.Pushed = false;
			}
			else if (e.Button == toolbarMove)
			{
				Midget.Events.EventFactory.Instance.GenerateSwitchEditModeEvent(this,EditMode.Move);
				toolbarSelect.Pushed = false;
				toolbarRotate.Pushed = false;
				toolbarScale.Pushed = false;
			}
			else if (e.Button == toolbarRotate)
			{
				Midget.Events.EventFactory.Instance.GenerateSwitchEditModeEvent(this,EditMode.Rotate);
				toolbarSelect.Pushed = false;
				toolbarMove.Pushed = false;
				toolbarScale.Pushed = false;
			}
			else if (e.Button == toolbarScale)
			{
				Midget.Events.EventFactory.Instance.GenerateSwitchEditModeEvent(this, EditMode.Scale);
				toolbarSelect.Pushed = false;
				toolbarMove.Pushed = false;
				toolbarRotate.Pushed = false;
			}
			else
			{
				MessageBox.Show(this, "This button hasn't been implemented yet.", "Not Implemented", MessageBoxButtons.OK, 
					MessageBoxIcon.Information);
			}
		}

		private void NewFile()
		{	
			Midget.Events.EventFactory.Instance.GenerateNewSceneRequestEvent(this);
		}

		private void OpenFile()
		{
			//			Application.LocalUserAppDataPath //ExecutablePath
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (openFileDialog.FileName.Trim().Length != 0)
				{
					try
					{
						Midget.Command.UserActionCommand.OpenScene(openFileDialog.FileName);
					}
					catch
					{
						MessageBox.Show(this, "Sorry, this file type is incompatible with this version of Midget3D.", 
							"File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void SaveFile()
		{
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (saveFileDialog.FileName.Trim().Length != 0)
				{
					try
					{
						SceneManager.Instance.SaveScene( saveFileDialog.FileName );
					}
					catch
					{
						if (MessageBox.Show(this, "Sorry, your file could not be saved. Perhaps you are using a Mac.", 
							"File Save Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
						{
							SaveFile();
						}
					}
						

				}
			}
		}

		private void mnuFileNew_Click(object sender, System.EventArgs e)
		{
			NewFile();
		}

		private void mnuFileOpen_Click(object sender, System.EventArgs e)
		{
			OpenFile();
		}

		private void mnuFileSave_Click(object sender, System.EventArgs e)
		{
			SaveFile();
		}

		private void mnuGravity_Click(object sender, System.EventArgs e)
		{
			if (selectedObjects.Count == 0)
			{
				MessageBox.Show("No object selected.");
				return;
			}
			
			Midget.Events.EventFactory.Instance.GenerateAddDyanmicRequestEvent(this,(IObject3D)selectedObjects[selectedObjects.Count - 1],new Gravity());
		}

		private void mnuDynamicsToKeyframes_Click(object sender, System.EventArgs e)
		{
			foreach(IObject3D obj in selectedObjects)
			{
				obj.DynamicsToKeys(timelineControl.StartFrame, timelineControl.EndFrame, timelineControl.Index);
			}
		}

		private void mnuRenderRenderCurrent_Click(object sender, System.EventArgs e)
		{
			currentFrameRenderWindow = new RenderPreview(DeviceManager.Instance,SceneManager.Instance, renderSettings);
			
			currentFrameRenderWindow.Show();

			currentFrameRenderWindow.Render(SceneManager.Instance.CurrentFrameIndex);
		}

		private void mnuRenderSettings_Click(object sender, System.EventArgs e)
		{
			if(renderSettingsForm == null)
			{
				renderSettingsForm = new RenderSettingsForm(CameraFactory.Instance,renderSettings);
				renderSettingsForm.Closed += new EventHandler(renderSettingsForm_Closed);
				renderSettingsForm.Show();
			}

			renderSettingsForm.Select();
			
		}

		private void renderSettingsForm_Closed(object sender, EventArgs e)
		{
			//TODO : REMOVE DELEGATE
			renderSettingsForm = null;
		}

		private void mnuRenderBatch_Click(object sender, System.EventArgs e)
		{
			
			FileRenderThreadFactory factory = new FileRenderThreadFactory(DeviceManager.Instance,SceneManager.Instance);
			
			RenderProgress renderProgress = new RenderProgress(factory.NewFileRender(renderSettings));

			renderProgress .ShowDialog();
		}

		private void mnuModifyGroup_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateGroupRequestEvent(this,selectedObjects);
		}

		private void mnuViewShowObjectDock_Click(object sender, System.EventArgs e)
		{
			objectDockControl.EnsureVisible(dockHost);
		}

		private void mnuViewShowModifierPanel_Click(object sender, System.EventArgs e)
		{
			modifierDockControl.EnsureVisible(dockHost);
		}

		private void mnuCurvesNewCurve_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.Curve,null);
		}

		private void mnuCurvesAddControlPoint_Click(object sender, System.EventArgs e)
		{
			if (selectedObjects[0] is Curve)
			{
				((Curve)selectedObjects[0]).AddCtrlPt();
			}
			else 
			{
				MessageBox.Show(this, "Please select a curve to add your Control Point to.  If there is no curve, then please create one.", 
					"Control Point Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
			}
		}

		private void mnuCurveSelectPath_Click(object sender, System.EventArgs e)
		{
			// check for selected object
			if (selectedObjects.Count == 0)
			{
				MessageBox.Show(this, "Please select an object for the path, first.", "No Object Selected", 
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// prompt the user for a curve
			CurveSelectBoxResult result = CurveSelectBox.Show("Please select a curve:", "Select a Curve", "", 200, 200);

			// init and error checking
			IObject3D objToMove = (IObject3D)selectedObjects[selectedObjects.Count - 1];
			Curve path;
			if (result.SelectedCurve != null)
			{
				path = result.SelectedCurve;
			}
			else
			{
				MessageBox.Show(this, "No curve was selected.", "Curve Selection Error", MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
				return;
			}

			// set the object to follow the given curve
			objToMove.HasPath = true;
			objToMove.Path = path;
			objToMove.PathFrameAmount = timelineControl.EndFrame;

			// re-render
			DeviceManager.Instance.UpdateViews();
		}

		private void mnuDynamicsAddParticleSystem_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateCreateObjectRequestEvent(this,ObjectFactory.ObjectTypes.ParticleSystem,null);
		}

		private void mnuViewObjectSelector_Click(object sender, System.EventArgs e)
		{
			objectDockControl.EnsureVisible(dockHost);
		}

		private void mnuViewShowMaterialEditor_Click(object sender, System.EventArgs e)
		{
			materialsDock.EnsureVisible(dockHost);
		}

		private void EventFactory_SelectAdditionalObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Add(e.Object);
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_DeselectObjects(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			selectedObjects.Remove(e.Object);
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_DeleteObject(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_CreateObject(object sender, Midget.Events.Object.SingleObjectEventArgs e)
		{
			DeviceManager.Instance.UpdateViews();
		}

		private void EventFactory_Transformation(object sender, Midget.Events.Object.MultiObjectEventArgs e)
		{
			DeviceManager.Instance.UpdateViews();
		}

		private void mnuModifyUngroup_Click(object sender, System.EventArgs e)
		{
			Midget.Events.EventFactory.Instance.GenerateUngroupRequestEvent(this, (IObject3D)selectedObjects[selectedObjects.Count-1]);
		}

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			if (this.WindowState == FormWindowState.Maximized)
			{
				dxViewPort.ReCenter();
			}
		}

		private void mnuForcesForce_Click(object sender, System.EventArgs e)
		{
			if (selectedObjects.Count == 0)
			{
				MessageBox.Show("No object selected.");
				return;
			}
			
			Midget.Events.EventFactory.Instance.GenerateAddDyanmicRequestEvent(this,(IObject3D)selectedObjects[selectedObjects.Count - 1],new GeneralForce());
		}
	}
}
