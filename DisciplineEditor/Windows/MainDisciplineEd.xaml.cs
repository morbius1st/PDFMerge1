#define DML0 // not yet used
#define DML1 // not yet used
// #define DML2  // turns on or off bool flags / button enable flags only / listbox idex set
// #define DML3  // various status messages
// #define DML4  // update status status messages

#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using AndyShared.ClassificationFileSupport;
using SettingsManager;
using AndyShared.FileSupport.FileNameSheetPDF;
using UtilityLibrary;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using AndyShared.Support;
using JetBrains.Annotations;
using CSLibraryIo.CommonFileFolderDialog;
using DebugCode;
using DisciplineEditor.Support;


#endregion

// projname: DisciplineEditor
// itemname: MainWindow
// username: jeffs
// created:  8/3/2024 9:48:58 PM

namespace DisciplineEditor.Windows
{
	/* editing process
	 * listbox has values
	 * fields are empty
	 * > two options are available
	 *	* select or new
	 *  +->if select
	 *	|   update fields
	 *	|   enable only: edit, remove, cancel
	 *  |   +>if edit
	 *  |   |  unlock fields
	 *	|   |  enable only cancel
	 *  |   |  if change field
	 *  |   |    has changes = true / enable update
	 *  |   |    if update
	 *  |   |      save data
	 *  |   |      reset to no data / reset button status / lock fields
	 *  |   +>if cancel
	 *  |   |  reset to no data / reset button status / lock fields
	 *  |   +>if remove
	 *  |      remove item from list
	 *  |      reset to no data / reset button status / lock fields
	 *  |
	 *  +-> if new
	 *        unlock fields
	 *        enable only cancel
	 *        +> if change all fields  (validate key once entered)
	 *        |   | enable update
	 *        |   +> if update
	 *        |        create new entry
	 *        |        reset to no data / reset button status / lock fields
	 *        +> if cancel
	 *             reset to no data / reset button status / lock fields
	 *
	 * reset ==
	 *   has changes = false
	 *   disable all buttons ex: select & new
	 *   disable field textboxes
	 *
	 */

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged, IWin
	{
		private enum OpStatus
		{
			// general reset
			ES_RESET,

			// existing data read
			ES_EXISTREADEMPTY,

			// existing data read
			ES_EXISTREADNOTEMPTY,

			// new item button selected
			ES_NEWITEM,

			// creating the first (and new) item
			ES_1STITEM,

			// partial new / 1st item
			ES_COMMONITEM,

			// an item has been selected to be edited in the field area
			ES_SELECTANDEDITITEM,

			// an item has been selected in the listbox
			ES_SELECTITEM,

			// request user to select existing or new item
			ES_CHOOSEITEM,

			// exit editing an item and lose changes
			ES_DONEEDITITEM,

			// the changes for the current item were saved but still can edit the item
			ES_SAVEDITEM,

			// edit an item button selected
			ES_EDITITEM,

			// new collection button selected
			ES_NEWCOLL,

			// cancel collection button selected
			ES_CANCELCOLL,
		}

		/*
		private DataManager<DisciplinesDataSet> disciplineMgr;
		// actual data file information
		public string DataFilePath => disciplineMgr?.Path?.SettingFilePath ?? "null";
		public string DataFilePathEllipsised =>
			CsStringUtil.EllipsisifyString(
				disciplineMgr?.Path?.SettingFilePath ?? "null",
				CsStringUtil.JustifyHoriz.CENTER, 80);
		public string DataFileName => disciplineMgr?.Path?.FileName ?? "null";
		public string DataFileExists => disciplineMgr?.Path?.Exists.ToString() ?? "null";
		*/


	#region private fields

		private const bool SHOW_LOCATION = true;


		private const string FT_SEL_ITEM = "Edit Item";
		private const string FT_EDITING_ITEM = "Editing Item";
		private const string FT_1ST_NEW_ITEM = "Creating First Item";
		private const string FT_NEW_ITEM = "Enter new Item";
		private const string FT_CANCEL_EDIT_ITEM = "Cancelling Edit this Item";
		private const string FT_CHOOSE_ITEM = "Select existing or create a new item";
		private const string FT_MT_COLLECTION = "Collection is Empty";
		private const string FT_RESET = "Select Option";
		private const string FT_KEY_NOT_UNIQUE = "Current Key is Not Unique";

		private CSLibraryIo.Root csIoR;
		private FileAndFolderDialog dlg;

		private FileManager fm;
		private DataFileManager dfm;
		private SettingsSupport uss;

		private string textBoxString = "initialized\n";
		private string fieldsTitle = "Selected Item";
		private string fieldsTitleTemp;

		private DisciplineListData lbxSelectedItem;
		private UserSettingInfo<UserSettingDataFile> selectedOrig;
		private int lbxSelectedIndex = 0;

		private string itemKey;
		private string itemTitle;
		private string itemDescription;

		private bool removeEnabled = false;
		private bool updateEnabled = false;
		private bool editEnabled = false;
		private bool cancelEnabled = false;
		private bool newEnabled = false;

		private bool itemHasChanges = false;
		private bool itemCanEdit = false;
		private bool itemCanSave = false;

		private bool editingNew;
		private bool editingSelected;


		private bool newCollEnabled;
		private bool cancelCollEnabled;
		private bool saveCollEnabled;
		private bool saveAsCollEnabled;
		private bool openCollEnabled;
		private bool deleteCollEnabled;
		private bool closeCollEnabled;


		private bool collection_Changed = false;

		private bool canSaveCollection = false;

		private string itemDisplayName;
		private bool itemLocked;

		private string itemDisciplineCode;
		private int recentSelectedIndex;
		private RecentItem recentSelectedItem;

		private bool itemUnLocked;
		private bool collectionOpened;
		private bool headerChanged;

	#endregion

	#region ctor

		public MainWindow()
		{
			startMsg();

			DM.init(5, this);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);

			DM.Start0();

			fm = new FileManager(this);

			DM.Start0(false, "start SettingsSupport");
			Uss = new SettingsSupport();
			DM.End0("complete SettingsSupport");

			DM.Start0(false, "start DataFileManager");
			Dfm = new DataFileManager(this, fm, uss);

			DM.End0("complete DataFileManager");

			DM.Start0(false, "start window InitializeComponent");
			InitializeComponent();
			DM.End0("complete window InitializeComponent");

			config();

			DM.End0();
		}

		// config process
		// NOTE:
		// ** the discipline path and filename shall ONLY be held in usersettings
		// config settings
		//	* (settingssupport) open the usersettings file
		//		> determine if data file path info exists (as in, not null / empty) / got path
		//			but not that the data file exists
		//  * (datafilemanager) determine the status of the discipline data file
		//		and take steps depending if 
		//			yes (got path and exists) / no (got path not exists) / null (no path)
		//			> yes  - open(path)
		//			> no   - new(path)
		//			> null - get path / new(path) 
		//		> configsettings()
		//			> uss.validate usersettings / open file or create file / read data
		//			> dfm.validate data file per the above (yes / no / null)
		//			> new routine (initDataFile)
		//				> if (yes) - open() ( => dfm.open)
		//				> if (no) - create() (=> dfm.create) / open() (=> dfm.open)
		//				> if (null) dfm.request data file location (=> dfm.GetNewDataFile) / open() (=> dfm.open)


		private void config()
		{
			DM.Start0();

			try
			{
				configSettings();
			}
			catch
			{
				return;
			}

			initData();

			lbxRecent.UnselectAll();

			DM.End0();
		}

		private void startMsg()
		{
			Debug.Write("\n");
			Debug.WriteLine("*".Repeat(50));
			Debug.WriteLine($"***  {"Discipline Editor",-42}***");
			Debug.WriteLine($"***  {DateTime.Now,-42}***");
			Debug.WriteLine("*".Repeat(50));
			Debug.Write("\n");
		}

		// validate / setup / and-or read the settings file
		// validate / setup / and-or read the discipline data file
		private void configSettings()
		{
			DM.Start0();

			// got a good user settings file?
			if (!Uss.GetUserSettingsFile())
			{
				TaskDialogManager.CannotCreateUserSettings(UserSettings.Path.SettingFilePath);
				this.Close();
				Application.Current.Shutdown();
				throw new InvalidProgramException();
			}

			dfm.SetDataFileStatus();

			if (!dfm.InitDataFile())
			{
				this.Close();
				Application.Current.Shutdown();
				throw new InvalidProgramException();
			}

			DM.End0();
		}

		private void initData()
		{
			DM.Start0();

			if (dfm.DataFilePath != null &&
				dfm.DisciplineDataFileExists)
			{
			#if DML3
				DM.Stat0("status: read existing");
			#endif

				try
				{
					dfm.Read();
					OnPropertyChange(nameof(DisciplineView));

					Collection_Opened = true;
					Collection_Changed = false;
				}
				catch
				{
					Application.Current.Shutdown();
					return;
				}

				// ObservableDictionary<string, DisciplineListData> d = Disciplines;
				// DisciplineListData d1 = d.First().Value;

				if (Disciplines.Count > 0)
				{
				#if DML3
					DM.Stat0("status: existing HAS items");
				#endif
					// tbl("status: existing HAS items");
					// updateStatus(OpStatus.ES_EXISTREADNOTEMPTY);

					LbxSelectedIndex = 0;
					selectItemForEditing();
					// selectAndEditItem();
				}
				else
				{
				#if DML3
					DM.Stat0("status: existing has NO items");
				#endif
					lbxSelectedIndex = -1;
					updateStatus(OpStatus.ES_EXISTREADEMPTY);
				}
			}
			else
			{
			#if DML3
				DM.Stat0("status: data file does not exist");
			#endif

				lbxSelectedIndex = -1;

				Collection_Opened = false;

				updateStatus(OpStatus.ES_RESET);

				UpdateCollectionButtons();
			}

			OnPropertyChange(nameof(Disciplines));

			UpdateCollectionProps();

			configCollChangedEvent();

			DM.End0();
		}

		private void configCollChangedEvent()
		{
			DM.InOut0("configured?");

			if (dfm == null || dfm.DdList == null)
			{
				DM.Stat0("*** NOT configured ***");
				return;
			}

			DM.Stat0("*** IS configured ***");

			dfm.DdList.CollectionChanged
				+= DisciplinesOnCollectionChanged;
		}

	#endregion

	#region public properties

		public string TextBoxString
		{
			get => textBoxString;
			set
			{
				textBoxString = value;
				OnPropertyChange();
			}
		}

		public ObservableDictionary<string, DisciplineListData> Disciplines
		{
			get
			{
				DM.InOut0(DM.IN_OUT_STRING, "getting Disciplines");
				return dfm.DdList ?? null;
			}
			// private set
			// {
			// 	disciplineMgr.Data.DisciplineList = value;
			// 	OnPropertyChange();
			// }
		}

		public ICollectionView DisciplineView
		{
			get
			{
				DM.InOut0(DM.IN_OUT_STRING, "getting DisciplineView");
				return dfm.DisciplineView;
			}
		}

		public DisciplineListData LbxSelectedItem
		{
			get => lbxSelectedItem;
			set
			{
				if (value != null && value.Equals(lbxSelectedItem)) return;

				DM.Start0();

				if (lbxSelectedItem != null) lbxSelectedItem.DataIsEditing = false;

			#if DML3
				DM.Stat0($"item: {value?.DisciplineData?.DisciplineCode ?? "null"}");
			#endif

				if (lbxSelectedIndex == -1)
				{
					lbxSelectedItem = null;

					updateItemValues();

					DM.End0("end 1");

					// OnPropertyChange();

					return;
				}

				lbxSelectedItem = value;

				// selectItemForEditing();

				updateItemValues();
				
				updateItemProps();

				OnPropertyChange();

				updateStatus(OpStatus.ES_SELECTITEM);

				DM.End0();
			}
		}

		public int LbxSelectedIndex
		{
			get => lbxSelectedIndex;
			set
			{
				DM.InOut0();
				lbxSelectedIndex = value;
				OnPropertyChange();
			}
		}

		// public string SelectedKey
		// {
		// 	get => selected?.DisciplineData?.Key ?? null;
		// 	set
		// 	{
		// 		if (value.Trim().Equals(selected.DisciplineData.Key)) return;
		//
		// 		disciplineMgr.Data.UpdateKey(selected.Key, value);
		//
		// 		OnPropertyChange();
		// 	}
		// }

		public string FieldsTitle
		{
			get => fieldsTitle;
			set
			{
				fieldsTitle = value;
				OnPropertyChange();
			}
		}

		private string FieldsTitleTemp
		{
			set
			{
				if (value.IsVoid())
				{
					fieldsTitle = fieldsTitleTemp;
				}
				else
				{
					fieldsTitleTemp = fieldsTitle;
				}

				fieldsTitle = value;
			}
		}

		public SettingsSupport Uss
		{
			get => uss;
			private set
			{
				uss = value;
				OnPropertyChange();
			}
		}

		public DataFileManager Dfm
		{
			get => dfm;
			private set
			{
				dfm = value;
				OnPropertyChange();
			}
		}

		// recent list
		public RecentItem RecentSelectedItem
		{
			get => recentSelectedItem;
			set
			{
				DM.InOut0();

				recentSelectedItem = value;
				OnPropertyChange();
			}
		}

		public int RecentSelectedIndex
		{
			get => recentSelectedIndex;
			set
			{
				DM.InOut0();

				recentSelectedIndex = value;
				OnPropertyChange();
			}
		}


	#region fields

		public string ItemKey
		{
			get => itemKey;

			set
			{
				if ((value?.Trim() ?? "").Equals(itemKey ?? "") )
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, $"set value| >{value ?? "null"}<>{itemKey ?? "null"}<");

				itemKey = value;

				OnPropertyChange();

				if (itemKey != null) ItemHasChanges = true;
			}
		}

		public const string DISC_CODE = nameof(ItemDisciplineCode);

		public string ItemDisciplineCode
		{
			get => itemDisciplineCode;
			set
			{
				if (value == itemDisciplineCode)
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, $"set value| >{value}<>{itemDisciplineCode}<");
				itemDisciplineCode = value;
				OnPropertyChange();

				if (itemDisciplineCode != null) ItemHasChanges = true;
			}
		}

		public const string TITLE = nameof(ItemTitle);

		public string ItemTitle
		{
			get => itemTitle;
			set
			{
				if (value == itemTitle)
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, "set value");
				itemTitle = value;
				OnPropertyChange();

				if (itemDisplayName.IsVoid()) ItemDisplayName = value;
				if (itemDescription.IsVoid()) ItemDescription = value;

				if (itemTitle != null) ItemHasChanges = true;
			}
		}

		public const string DISP_NAME = nameof(ItemDisplayName);

		public string ItemDisplayName
		{
			get => itemDisplayName;
			set
			{
				if (value == itemDisplayName)
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, "set value");
				itemDisplayName = value;
				OnPropertyChange();

				if (itemTitle.IsVoid()) ItemTitle = value;
				if (itemDescription.IsVoid()) ItemDescription = value;

				if (itemDisplayName != null) ItemHasChanges = true;
			}
		}

		public const string DESC = nameof(ItemDescription);

		public string ItemDescription
		{
			get => itemDescription;
			set
			{
				if (value == itemDescription)
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, "set value");

				itemDescription = value;
				OnPropertyChange();

				if (itemTitle.IsVoid()) ItemTitle = value;
				if (itemDisplayName.IsVoid()) ItemDisplayName = value;

				if (itemDescription != null) ItemHasChanges = true;
			}
		}

		public const string IS_LOCKED = nameof(ItemLocked);

		public bool ItemLocked
		{
			get => itemLocked;
			set
			{
				if (value == itemLocked)
				{
					DM.InOut0(DM.IN_OUT_STRING, "no change - values match");
					return;
				}

				DM.InOut0(DM.IN_OUT_STRING, "set value");

				itemLocked = value;
				OnPropertyChange();

				ItemHasChanges = true;

				OnPropertyChange(nameof(ItemLockableCanEdit));
				OnPropertyChange(nameof(RemoveItem_Enabled));
			}
		}

		public bool ItemIsEditing => lbxSelectedItem?.DataIsEditing ?? false;

	#endregion


	#region status

	#region field status

		public bool IsEditing => EditingNew || EditingSelected;

		public bool EditingNew
		{
			get => editingNew;
			set
			{
				if (value == editingNew) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				editingNew = value;
				OnPropertyChange();
				OnPropertyChange(nameof(IsEditing));
			}
		}

		public bool EditingSelected
		{
			get => editingSelected;
			set
			{
				if (value == editingSelected) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif


				editingSelected = value;
				OnPropertyChange();
				OnPropertyChange(nameof(IsEditing));
			}
		}


		public bool ItemCanEdit
		{
			get => itemCanEdit;
			set
			{
			#if DML2
				DM.Stat0($"status to: ItemCanEdit to| {value}");
			#endif

				// tbl($"ItemCanEdit to| {value}");
				if (value == itemCanEdit) return;
				itemCanEdit = value;
				OnPropertyChange();
				OnPropertyChange(nameof(ItemLockableCanEdit));
			}
		}

		public bool ItemLockableCanEdit => itemCanEdit && !itemLocked;

		public bool ItemCanSave
		{
			get => itemCanSave;
			set
			{
			#if DML2
				DM.Stat0($"ItemCanSave set to {value}");
			#endif
				if (itemCanSave == value) return;


				itemCanSave = value;
				OnPropertyChange();
			}
		}

		public bool ItemHasChanges
		{
			get => itemHasChanges;
			private set
			{
			#if DML2
				DM.Stat0( $"status to: ItemHasChanges to| {value}");
			#endif
				// tbl($"ItemHasChanges to| {value}");
				itemHasChanges = value;
				OnPropertyChange();
				updateCanSave();
			}
		}

	#endregion

	#region item edit

		public bool NewItem_Enabled
		{
			get => newEnabled && Collection_Status;
			set
			{
			#if DML2
				DM.InOut0(DM.IN_OUT_STRING, $"value {value} | coll_stat {Collection_Status}");
			#endif
				if (value == newEnabled  && Collection_Status) return;
				newEnabled = value   && Collection_Status;
				OnPropertyChange();
			}
		}

		// public bool SelectItem_Enabled
		// {
		// 	get => selectItemEnabled;
		// 	set
		// 	{
		// 		if (value == selectItemEnabled && Collection_Status) return;
		// 		selectItemEnabled = value && Collection_Status;
		// 		OnPropertyChange();
		// 	}
		// }

		public bool EditItem_Enabled
		{
			get => editEnabled && Collection_Status;
			set
			{
			#if DML2
				DM.InOut0(DM.IN_OUT_STRING, $"value {value} | coll_stat {Collection_Status}");
			#endif

				if (value == editEnabled   && Collection_Status) return;
				editEnabled = value  && Collection_Status;
				OnPropertyChange();
			}
		}

		public bool SaveItem_Enabled
		{
			// get => updateEnabled;
			get => updateEnabled && Collection_Status && ItemCanSave;
			set
			{
			#if DML2
				DM.Stat0($"SaveItem_Enabled set to {value}");
			#endif
				// if (value == updateEnabled) return;
				updateEnabled = value;
				OnPropertyChange();
			}
		}

		public bool RemoveItem_Enabled
		{
			get => removeEnabled && Collection_Status && !itemLocked;
			set
			{
			#if DML2
				DM.InOut0(DM.IN_OUT_STRING, $"value {value} | coll_stat {Collection_Status}");
			#endif

				if (value == removeEnabled   && Collection_Status) return;
				removeEnabled = value   && Collection_Status;
				OnPropertyChange();
			}
		}

		public bool DoneEditItem_Enabled
		{
			get => cancelEnabled && Collection_Status;
			set
			{
			#if DML2
				DM.InOut0(DM.IN_OUT_STRING, $"value {value} | coll_stat {Collection_Status}");
			#endif

				if (value == cancelEnabled   && Collection_Status) return;
				cancelEnabled = value   && Collection_Status;
				OnPropertyChange();
			}
		}

	#endregion

	#region collection

		// public bool Collection_Modified => collection_Changed || headerChanged;

		public bool HeaderChanged
		{
			set
			{
				if (value == headerChanged) return;
				headerChanged = value;

				Collection_Changed = value;

				// OnPropertyChange(nameof(Collection_Modified));

				UpdateCollectionButtons();
			}
		}

		public bool Collection_Opened
		{
			get => collectionOpened;
			set
			{
				if (value == collectionOpened) return;
				collectionOpened = value;

				dfm.DataFileOpened = value;

				OnPropertyChange();
			}
		}

		public bool Collection_Changed
		{
			get => collection_Changed;
			set
			{
				if (value == collection_Changed) return;

			#if DML2
				DM.Stat0($"Collection_Changed to| {value}");
			#endif

				collection_Changed = value;
				OnPropertyChange();
				// OnPropertyChange(nameof(Collection_Modified));
			}
		}

		public bool Collection_Status
		{
			get =>  dfm?.DdList != null;
			set { OnPropertyChange(); }
		}


		public bool OpenColl_Enabled
		{
			get => openCollEnabled;
			set
			{
				if (value == openCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				openCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool NewColl_Enabled
		{
			get => newCollEnabled;
			set
			{
				if (value == newCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				newCollEnabled = value;
				OnPropertyChange();
				OnPropertyChange(nameof(ExitEnabled));
			}
		}

		public bool SaveColl_Enabled
		{
			get => saveCollEnabled;
			set
			{
				if (value == saveCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				saveCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool SaveAsColl_Enabled
		{
			get => saveAsCollEnabled;
			set
			{
				if (value == saveAsCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				saveAsCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool CloseColl_Enabled
		{
			get => closeCollEnabled;
			set
			{
				if (value == closeCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				closeCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool DeleteColl_Enabled
		{
			get => deleteCollEnabled;
			set
			{
				if (value == deleteCollEnabled) return;
			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				deleteCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool CancelColl_Enabled
		{
			get => cancelCollEnabled;
			set
			{
				if (value == cancelCollEnabled) return;

			#if DML2
				DM.Stat0($"set to: {value}");
			#endif

				cancelCollEnabled = value;
				OnPropertyChange();
			}
		}

	#endregion

		// system
		public bool ExitEnabled
		{
			get => NewColl_Enabled || !collection_Changed;
			// set
			// {
			// 	if (value == exitEnabled) return;
			// 	exitEnabled = value;
			// 	OnPropertyChange();
			// }
		}

	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		private void updateStatus(OpStatus status)
		{
			DM.Start0(false,  $"processing {status}");


			switch (status)
			{
			// reset to base status
			case OpStatus.ES_RESET:
				{
				#if DML4
					DM.Stat0("status to: Reset");
				#endif

					TbkLastUpdateStatus.Text = "ES_RESET";

					FieldsTitle = FT_RESET;

					// flags
					ItemCanEdit = false;
					ItemCanSave = false;
					ItemHasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					// buttons
					NewItem_Enabled = false;
					EditItem_Enabled = false;
					SaveItem_Enabled = false;
					RemoveItem_Enabled = false;
					DoneEditItem_Enabled = false;

					clearItemProps();

					break;
				}

			case OpStatus.ES_EXISTREADEMPTY:
				{
				#if DML4
					DM.Stat0("status to: existing IS empty");
				#endif
					// tbl("status: existing IS empty");


					updateStatus(OpStatus.ES_RESET);

					TbkLastUpdateStatus.Text = "ES_EXISTREADEMPTY";

					// // flags                      // per es_reset
					// ItemCanEdit = false;			 // per es_reset
					// ItemCanSave = false;			 // per es_reset
					// ItemHasChanges = false;		 // per es_reset
					//								 // per es_reset
					// EditingNew = false;			 // per es_reset
					// EditingSelected = false;		 // per es_reset
					//								 // per es_reset
					// // buttons					 // per es_reset
					// NewItem_Enabled = false;		 // per es_reset
					// EditItem_Enabled = false;	 // per es_reset
					// SaveItem_Enabled = false;	 // per es_reset
					// RemoveItem_Enabled = false;	 // per es_reset
					// DoneEditItem_Enabled = false; // per es_reset

					NewItem_Enabled = true;

					UpdateCollectionButtons();

					// OpenColl_Enabled = false;
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;
					// SaveAsColl_Enabled = true;
					// CloseColl_Enabled = true;
					// DeleteColl_Enabled = true;
					// CancelColl_Enabled = false;

					break;
				}


			case OpStatus.ES_EXISTREADNOTEMPTY:
				{
				#if DML4
					DM.Stat0("status to: existing NOT empty");
				#endif

					// tbl("status: existing NOT empty");

					updateStatus(OpStatus.ES_RESET);

					TbkLastUpdateStatus.Text = "ES_EXISTREADNOTEMPTY";

					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// EditingNew = false;
					// EditingSelected = false;
					//
					// NewItem_Enabled = true;
					// SaveItem_Enabled = false;
					// RemoveItem_Enabled = false;
					// EditItem_Enabled = false;
					// DoneEditItem_Enabled = false;
					//
					// ItemKey = null;
					// ItemTitle = null;
					// ItemDescription = null;

					NewColl_Enabled = true;
					SaveColl_Enabled = false;
					CancelColl_Enabled = false;

					break;
				}

			case OpStatus.ES_COMMONITEM:
				{
				#if DML4
					DM.Stat0("status to: blank item");
				#endif

					// tbl("status: blank item");

					ItemCanEdit = false;
					ItemCanSave = false;
					ItemHasChanges = false;

					NewItem_Enabled = false;
					SaveItem_Enabled = false;

					NewColl_Enabled = false;
					SaveColl_Enabled = false;

					break;
				}

			case OpStatus.ES_SAVEDITEM:
				{
				#if DML4
					DM.Stat0("status to: item saved: start");
				#endif
					FieldsTitle = FT_SEL_ITEM;

					updateStatus(OpStatus.ES_COMMONITEM);

					TbkLastUpdateStatus.Text = "ES_SAVEDITEM";

					// common does this
					//
					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// NewItem_Enabled = false;
					// SaveItem_Enabled = false;
					//
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

				#if DML4
					DM.Stat0("status to: list item saved: settings");
				#endif

					ItemCanEdit = true;
					// ItemCanSave = false; // match
					// ItemHasChanges = false;  // match

					EditingNew = false;
					EditingSelected = true;

					// NewItem_Enabled = false; // match
					EditItem_Enabled = false;
					// SaveItem_Enabled = false; // match
					RemoveItem_Enabled = true;
					DoneEditItem_Enabled = true;

					// NewColl_Enabled = false;  // match
					// SaveColl_Enabled = false;  // match

					break;
				}


			// begin new item
			case OpStatus.ES_NEWITEM:
				{
				#if DML4
					DM.Stat0("status to: new item: start");
				#endif

					FieldsTitle = FT_NEW_ITEM;

					updateStatus(OpStatus.ES_COMMONITEM);

					TbkLastUpdateStatus.Text = "ES_NEWITEM";

					// common does this
					//
					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// NewItem_Enabled = false;
					// SaveItem_Enabled = false;
					//
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

					clearItemProps();

				#if DML4
					DM.Stat0("status to: new item: settings");
				#endif

					ItemCanEdit = true;

					EditingNew = true;
					EditingSelected = false;

					EditItem_Enabled = false;
					RemoveItem_Enabled = false;
					DoneEditItem_Enabled = true;


					break;
				}
			case OpStatus.ES_1STITEM:
				{
				#if DML4
					DM.Stat0("status to: 1st item: start");
				#endif

					FieldsTitle = FT_1ST_NEW_ITEM;

					updateStatus(OpStatus.ES_COMMONITEM);

					TbkLastUpdateStatus.Text = "ES_1STITEM";

					// common does this
					//
					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// NewItem_Enabled = false;
					// SaveItem_Enabled = false;
					//
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

					clearItemProps();

				#if DML4
					DM.Stat0("status to: 1st item: settings");
				#endif
					// tbl("status: 1st item");

					ItemCanEdit = true;

					EditingNew = false;
					EditingSelected = false;

					EditItem_Enabled = false;
					RemoveItem_Enabled = false;
					DoneEditItem_Enabled = false;

					break;
				}
			case OpStatus.ES_SELECTANDEDITITEM:
				{
				#if DML4
					DM.Stat0("status to: list item selected to edit: start");
				#endif
					FieldsTitle = FT_SEL_ITEM;

					updateStatus(OpStatus.ES_COMMONITEM);

					TbkLastUpdateStatus.Text = "ES_SELECTANDEDITITEM";

					// common does this
					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// NewItem_Enabled = false;
					// SaveItem_Enabled = false;
					//
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

				#if DML4
					DM.Stat0("status to: list item selected to edit: settings");
				#endif

					ItemCanEdit = true;

					EditingNew = false;
					EditingSelected = true;

					RemoveItem_Enabled = true;
					EditItem_Enabled = false;
					DoneEditItem_Enabled = true;

					break;
				}
			case OpStatus.ES_SELECTITEM:
				{
				#if DML4
					DM.Stat0("status to: list item selected: start");
				#endif
					FieldsTitle = FT_SEL_ITEM;

					updateStatus(OpStatus.ES_COMMONITEM);

					TbkLastUpdateStatus.Text = "ES_SELECTITEM";

					// common does this
					//
					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;
					//
					// NewItem_Enabled = false;
					// SaveItem_Enabled = false;
					//
					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

				#if DML4
					DM.Stat0("status to: list item selected: settings");
				#endif

					// ItemCanEdit = false;
					// ItemCanSave = false;
					// ItemHasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					NewItem_Enabled = true;
					EditItem_Enabled = true;
					// SaveItem_Enabled = false;
					RemoveItem_Enabled = false;
					DoneEditItem_Enabled = false;

					// NewColl_Enabled = false;
					// SaveColl_Enabled = false;

					break;
				}
			case OpStatus.ES_EDITITEM:
				{
				#if DML4
					DM.Stat0("status to: editing item");
				#endif
					// tbl("status: editing item");

					FieldsTitle = FT_EDITING_ITEM;

					TbkLastUpdateStatus.Text = "ES_EDITITEM";

					ItemCanEdit = true;
					ItemCanSave = true;
					ItemHasChanges = false;

					EditingNew = false;
					EditingSelected = true;

					NewItem_Enabled = false;
					SaveItem_Enabled = true;
					RemoveItem_Enabled = true;
					EditItem_Enabled = false;
					DoneEditItem_Enabled = true;

					break;
				}

			case OpStatus.ES_DONEEDITITEM:
				{
				#if DML4
					DM.Stat0("status to: done editing item");
				#endif

					// tbl("status: cancel editing item");

					FieldsTitle = FT_CHOOSE_ITEM;

					TbkLastUpdateStatus.Text = "ES_DONEEDITITEM";

					ItemCanEdit = false;
					ItemCanSave = false;
					ItemHasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					NewItem_Enabled = true;
					SaveItem_Enabled = false;
					RemoveItem_Enabled = false;
					EditItem_Enabled = false;
					DoneEditItem_Enabled = false;

					clearItemProps();

					break;
				}


			case OpStatus.ES_CHOOSEITEM:
				{
				#if DML4
					DM.Stat0("status to: choose item");
				#endif
					// tbl("status: choose item");

					FieldsTitle = FT_CHOOSE_ITEM;

					TbkLastUpdateStatus.Text = "ES_CHOOSEITEM";

					ItemCanEdit = false;
					ItemCanSave = false;
					ItemHasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					NewItem_Enabled = true;
					SaveItem_Enabled = false;
					RemoveItem_Enabled = false;
					EditItem_Enabled = false;
					DoneEditItem_Enabled = false;

					break;
				}


			case OpStatus.ES_NEWCOLL:
				{
				#if DML4
					DM.Stat0("status to: new collection");
				#endif
					// tbl("status: new collection");

					updateStatus(OpStatus.ES_1STITEM);

					TbkLastUpdateStatus.Text = "ES_NEWCOLL";


					// ItemCanEdit = false;	         // common
					// ItemCanEdit = true;			 // ES_1STITEM

					// ItemCanSave = false;	         // common
					// ItemHasChanges = false;	     // common

					// EditingNew = false;			 // ES_1STITEM
					// EditingSelected = false;		 // ES_1STITEM

					// NewItem_Enabled = false;      // common
					// EditItem_Enabled = false;	 // ES_1STITEM
					// SaveItem_Enabled = false;     // common
					// RemoveItem_Enabled = false;	 // ES_1STITEM
					// DoneEditItem_Enabled = false; // ES_1STITEM

					// NewColl_Enabled = false;      // common
					// SaveColl_Enabled = false;     // common


					EditingNew = true;
					DoneEditItem_Enabled = true;

					// OpenColl_Enabled = false;
					// CancelColl_Enabled = false;

					break;
				}
			case OpStatus.ES_CANCELCOLL:
				{
				#if DML4
					DM.Stat0("status to: cancel collection");
				#endif
					// abandon changes and restore from file
					// tbl("status: cancel collection");

					TbkLastUpdateStatus.Text = "ES_CANCELCOLL";

					FieldsTitle = FT_MT_COLLECTION;

					ItemCanEdit = true;
					ItemHasChanges = false;

					NewItem_Enabled = false;
					DoneEditItem_Enabled = false;
					EditItem_Enabled = false;
					SaveItem_Enabled = false;
					RemoveItem_Enabled = false;

					NewColl_Enabled = true;
					SaveColl_Enabled = false;
					CancelColl_Enabled = false;

					clearItemProps();

					break;
				}
			}

			DM.End0();
		}

		private void updateCanSave()
		{
			DM.Start0();

			if (!editingSelected &&
				!isKeyUnique(itemKey))
			{
				ItemCanSave = false;
				DM.End0("end 1");
				return;
			}

			bool a = !itemKey.IsVoid();
			bool b = !itemTitle.IsVoid();
			bool c = !itemDescription.IsVoid();
			bool f = !itemDisplayName.IsVoid();
			bool g = !itemDisciplineCode.IsVoid();
			bool d = itemHasChanges;

			bool e = a && b && c && d && f && g;

			ItemCanSave = e;

			SaveItem_Enabled = e;

			DM.End0("end", $"result: {e}");
		}

		public void UpdateCollectionButtons()
		{
			DM.Start0();
			// tbl($"@UpdateCollectionButtons");  //| is editing {IsEditing} | file exists {dfm.DisciplineDataFileExists}");
			//                                                                      DisciplineDataFileExists
			//                                                                      |        collection
			//                                                                      v        changed
			// open - open an existing data file                   open   | true  | false  | 
			// only when no current data file (closed or deleted)         | false |        | 
			// true when dfm.ddinit=false;						          | 	  |        | 
			//													          | 	  |        | 
			// new - create a new data file						   new    | true  | false  | 
			// only when no current data file (closed or deleted)         | false |        | 
			//													          |  	  |        | 
			// save - the current data							   save   | true  | true   | true
			// true when has changes and not editing			          | false |        | 
			// else false										          | 	  |        | 
			//													          | 	  |        | 
			// saveas - save as a new file						   saveas | true  | true   | false 
			// same as save - this also will close				          | false |        | 
			// the current data file and open the new			          | 	  |        | 
			// data file										          | 	  |        | 
			//													          | 	  |        | 
			// close - close the current data file				   close  | true  | true   | false
			// only when there is a current data file and		          | false |        | 
			// there are no changes								          | 	  |        | 
			//													          | 	  |        | 
			// delete - the current data file					   delete | true  | true   | false
			// only when there is a current data file and		          | false |        | 
			// there are no changes								          | 	  |        | 
			// provide warning of possible permanent data loss	          | 	  |        | 
			//													          | 	  |        | 
			// cancel - cancel all changes						   cancel | true  | true   | true
			// only when there is a current data file and		          | false |        | 
			// there are changes								          | 	  |        | 
			// provide warning of possible permanent data loss	          | 	  |        | 

			if (IsEditing)
			{
				TbkLastCollButton.Text = "is editing";

			#if DML3
				DM.Stat0("is editing");
			#endif
				// tbl($"@UpdateCollectionButtons| is editing");
				OpenColl_Enabled = false;
				NewColl_Enabled = false;
				SaveColl_Enabled = false;
				SaveAsColl_Enabled = false;
				CloseColl_Enabled = false;
				DeleteColl_Enabled = false;
				CancelColl_Enabled = false;

				OnPropertyChange(nameof(ExitEnabled));

				DM.End0();
				return;
			}


			if (dfm.DisciplineDataFileExists)
			{
			#if DML3
				DM.Stat0("file DOES exist");
			#endif
				// tbl($"@UpdateCollectionButtons| file DOES exists");
				OpenColl_Enabled = false;
				NewColl_Enabled  = false;

				// if (Collection_Modified)
				if (Collection_Changed)
				{
					TbkLastCollButton.Text = "DOES exist / HAS changes";

				#if DML3
					DM.Stat0("HAS changes");
				#endif
					// tbl($"@UpdateCollectionButtons| DOES has changes");

					SaveColl_Enabled   = true;
					CancelColl_Enabled = true;

					SaveAsColl_Enabled = false;
					CloseColl_Enabled  = false;
					DeleteColl_Enabled = false;
				}
				else
				{
					TbkLastCollButton.Text = "DOES exist / NO changes";

				#if DML3
					DM.Stat0("NO changes");
				#endif
					// tbl($"@UpdateCollectionButtons| NOT has changes");

					CloseColl_Enabled = true;
					DeleteColl_Enabled = true;
					SaveAsColl_Enabled = true;

					SaveColl_Enabled   = false;
					CancelColl_Enabled = false;
				}
			}
			else
			{
				TbkLastCollButton.Text = "does NOT exists";

			#if DML3
				DM.Stat0("file NOT exists");
			#endif
				// tbl($"@UpdateCollectionButtons| file NOT exists");

				OpenColl_Enabled = true;
				NewColl_Enabled  = true;

				CloseColl_Enabled  = false;
				DeleteColl_Enabled = false;

				SaveColl_Enabled   = false;
				SaveAsColl_Enabled = false;
				CancelColl_Enabled = false;
			}

			OnPropertyChange(nameof(ExitEnabled));

			DM.End0();
		}

		public void UpdateCollectionProps()
		{
			DM.InOut0();

			OnPropertyChange(nameof(Dfm));
			OnPropertyChange(nameof(Uss));
		}

	#endregion

	#region private methods

		private bool isKeyUnique(string key)
		{
			if (!dfm.DisciplineDataMgrIsInit) return false;

			if (dfm.IsNewKey(key))
			{
				FieldsTitleTemp = null;
				return true;
			}

			FieldsTitleTemp = FT_KEY_NOT_UNIQUE;
			return false;
		}

		// item routines
		private void clearItemProps()
		{
			DM.Start0();

			ItemKey = null;
			ItemDisciplineCode = null;
			ItemTitle = null;
			ItemDisplayName = null;
			ItemDescription = null;
			ItemLocked = false;

			lbxSelectedItem = null;

			DM.End0();
		}

		// private void clearItemFields()
		// {
		// 	itemKey = null;
		// 	itemTitle = null;
		// 	itemDisplayName = null;
		// 	itemDescription = null;
		// 	itemLocked = false;
		// }


		// one of the list items has been 
		// selected and chosen to ge edited in the
		// items list
		private void selectAndEditItem()
		{
			DM.Start0();

			if (lbxSelectedItem == null)
			{
				DM.End0("end 1");
				return;
			}

			updateItemValues();

			updateItemProps();

			updateStatus(OpStatus.ES_SELECTANDEDITITEM);

			UpdateCollectionButtons();

			DM.End0();
		}

		// one of the list items has been selected
		private void selectItemForEditing()
		{
			DM.Start0();

			// updateItemValues();
			//
			// updateItemProps();

			updateStatus(OpStatus.ES_SELECTITEM);

			UpdateCollectionButtons();

			DM.End0();
		}

		private void updateItemValues()
		{
			DM.InOut0();

			if (lbxSelectedItem == null)
			{
				return;
			}

			itemKey = lbxSelectedItem.DisciplineData.OrderCode;
			itemLocked = lbxSelectedItem.DisciplineData.Locked;
			itemDisciplineCode = lbxSelectedItem.DisciplineData.DisciplineCode;
			itemTitle = lbxSelectedItem.DisciplineData.Title;
			itemDisplayName = lbxSelectedItem.DisciplineData.DisplayName;
			itemDescription = lbxSelectedItem.DisciplineData.Description;

			lbxSelectedItem.DataIsEditing = true;
		}

		private void updateItemProps()
		{
			DM.InOut0();
			OnPropertyChange(nameof(ItemKey));
			OnPropertyChange(nameof(ItemLocked));
			OnPropertyChange(nameof(ItemDisciplineCode));
			OnPropertyChange(nameof(ItemTitle));
			OnPropertyChange(nameof(ItemDisplayName));
			OnPropertyChange(nameof(ItemDescription));

			OnPropertyChange(nameof(ItemCanEdit));
			OnPropertyChange(nameof(ItemLockableCanEdit));
			OnPropertyChange(nameof(RemoveItem_Enabled));
		}

		private void removeItem()
		{
			DM.Start0();

			if (!dfm.Remove(lbxSelectedItem.OrderCode))
			{
				// tbl("remove FAILED");
				DM.End0("end 1 - remove FAILED");
				return;
			}

			clearItemProps();

			OnPropertyChange(nameof(Disciplines));

			DM.End0();
		}

		private void saveNewItem()
		{
			DM.Start0();

			lbxSelectedItem = dfm.Add(itemKey, itemDisciplineCode,
				itemTitle, itemDisplayName, itemDescription, itemLocked);

			OnPropertyChange(nameof(Disciplines));

			DM.End0();
		}

		private void saveSelectedItem()
		{
			DM.Start0();

			if (itemKey.Equals(lbxSelectedItem.DisciplineData.OrderCode))
			{
				saveSelectedSameKey();
			}
			else
			{
				if (!saveSelectedChangeKey()) return;
			}

			// updateStatus(OpStatus.ES_RESET);

			DM.End0();
		}


		// item helpers
		private void saveSelectedSameKey()
		{
			DM.Start0();

			lbxSelectedItem.DisciplineData.DisciplineCode = itemDisciplineCode;
			lbxSelectedItem.DisciplineData.Title = itemTitle;
			lbxSelectedItem.DisciplineData.DisplayName = itemDisplayName;
			lbxSelectedItem.DisciplineData.Description = itemDescription;
			lbxSelectedItem.DisciplineData.Locked = itemLocked;

			DM.End0();
		}

		private bool saveSelectedChangeKey()
		{
			DM.Start0();

			string keyCurr = lbxSelectedItem.DisciplineData.OrderCode;
			string keyNew = itemKey;

			if (dfm.UpdateKey(keyCurr, keyNew).GetValueOrDefault(false) != true)
			{
				DM.End0("end 1");
				return false;
			}

			// key updated

			lbxSelectedItem = dfm.Find(keyNew);

			saveSelectedSameKey();

			DM.End0();

			return true;
		}


		// collection routines
		private void openCollection()
		{
			DM.Start0();

			bool? result = dfm.OpenA();

			if (result == true)
			{
				initData();

				Collection_Changed = false;

				// updateStatus(OpStatus.ES_NEWCOLL);
				// UpdateCollectionButtons();

				lbxRecent.Items.Refresh();

				RecentSelectedIndex = 0;
			}
			// false == existing is missing - message already handled
			// null = no op

			DM.End0();
		}

		private void openRecentCollection()
		{
			DM.Start0();

			if (recentSelectedIndex < 0) return;

			if (dfm.OpenB(recentSelectedItem.FileNameNoExt,
					recentSelectedItem.FolderPath))
			{
				initData();

				Collection_Changed = false;
				Collection_Opened = true;

				// updateStatus(OpStatus.ES_NEWCOLL);
				// UpdateCollectionButtons();

				lbxRecent.Items.Refresh();

				RecentSelectedIndex = 0;
			}

			DM.End0();
		}

		private void createNewCollection()
		{
			DM.Start0();

			if (dfm.CreateNew())
			{
				configCollChangedEvent();

				Collection_Changed = true;
				Collection_Opened = true;

				updateStatus(OpStatus.ES_NEWCOLL);
				UpdateCollectionButtons();

				lbxRecent.Items.Refresh();
			}

			DM.End0();
		}

		private void saveCollection()
		{
			DM.Start0();

			if (!Collection_Changed)
			{
				// tbl("collection not changed and not saved");
				DM.End0("end 1");
				return;
			}

			dfm.Save();

			Collection_Changed = false;

			dfm.ResetHeaderChanges(true);

			UpdateCollectionButtons();

			DM.End0();
		}

		/// <summary>
		/// make a copy of the collection - since this will make a copy
		/// of the data file, un-saved information needs to be saved first
		/// </summary>
		private void saveAsCollection()
		{
			DM.Start0();
			dfm.Save();

			bool? result = dfm.SaveAs();

			if (result == false)
			{
				TaskDialogManager.DisciplineDataFileSaveAsFailed();
			}
			else if (result == true)
			{
				initData();

				UpdateCollectionButtons();
			}
			// null - no op

			Collection_Changed = false;

			UpdateCollectionButtons();

			DM.End0();
		}

		private void deleteCollection()
		{
			DM.Start0();
			FilePath<FileNameSimple> filePath = dfm.DataFilePath;

			bool? result = dfm.Delete();

			if (result == false)
			{
				TaskDialogManager.DisciplineDataFileDeleteFailed();
			}
			else if (result == true)
			{
				initData();

				Collection_Changed = false;

				UpdateCollectionButtons();

				uss.RemoveFromRecent(uss.MakeRecentItem(filePath));

				lbxRecent.Items.Refresh();
			}

			DM.End0();
		}

		private void closeCollection()
		{
			DM.Start0();

			dfm.Close();

			lbxRecent.UnselectAll();

			initData();

			UpdateCollectionButtons();

			DM.End0();
		}

		private void cancelCollection()
		{
			DM.Start0();

			// eliminate any changes and return to the prior collection
			dfm.Cancel();

			Collection_Opened = false;

			OnPropertyChange(nameof(Disciplines));

			initData();

			UpdateCollectionButtons();

			DM.End0();
		}

		private void resetOpSettings()
		{
			collection_Changed = false;
			headerChanged = false;

			OnPropertyChange(nameof(Collection_Changed ));
			// OnPropertyChange(nameof(Collection_Modified ));
		}

		/* file / folder selection notes & process
		 * set extension as a const = xml in the app settings data set but do not save
		 * set default file name as a const = "Disciplines-01" in the app data set ... same
		 * set initial default location to user's document folder (via KnownFolders.Documents.Path);
		 *
		 * save to setting file
		 * usersettings last discipline file location / filename-no extn
		 *
		 * case 1
		 * * user file does not exist
		 * * which implies that the data file does not exist -
		 * but allow user to choose an existing
		 * or
		 * create user file
		 * have user create a file
		 * use the file
		 * save the file
		 *
		 *
		 * case 2
		 * * user file exists
		 * * no data file
		 * * same as case 1
		 *
		 * case 3
		 * * user file and data file exists
		 * create quick select button to use the current
		 * or to make new or
		 * to select different
		 *
		 * case 4
		 * current saved
		 * create new
		 * same as case 1
		 *
		 * case 5
		 * current saved
		 * save current as new (save as)
		 *
		 *
		 * adjustments
		 * ui adjustments
		 * > need fields: current folder path (abbreviated)
		 * > current file name
		 * > buttons for data file: delete, open, new, saveas, cancel, close
		 *		(all available except when editing a record or when there is no data file [current deleted])
		 * > move debug data to first panel
		 * 
		 * * setup default information
		 * > data file path = user's document folder
		 * > data file name = Disciplines-01
		 * > data file extension = xml
		 * * create file IO class
		 * > generic routines to get an existing file / get existing folder / save (create) a file
		 * > include error handling - KISS
		 *
		 * * create datafile class - place there as public constants
		 * > uses file IO class to get / create a file / saveas a file
		 * > handles setup upon start
		 *		* if folder / file not configured - request new
		 *		* if exists, read & set selected to first item
		 *
		 * processes:
		 * * case 1 startup - no user setting file & no data file - auto request user to create
		 *		> provided: save to user settings / save user settings - proceed
		 *		> user cancels, exit (with message)
		 * * startup - has user setting file & no data file - auto request user to create
		 *		> same as case 1
		 * * startup - has user setting file & has data file - data file does not exist
		 *		> let user know, file does not exist
		 *		> same as case 1
		 * * startup -  has user setting file & has data file - data file does exist
		 *		> open data file - populate data
		 *		> continue normal
		 */


		//
		// private void selectFile()
		// {
		// 	// CommonFileDialogFilter filter = new CommonFileDialogFilter();
		// 	//
		// 	// filter.DisplayName = "Discipline Data File";
		// 	// filter.Extensions.Add("xml");
		// 	// filter.ShowExtensions = true;
		//
		// 	dlg = new FileAndFolderDialog();
		// 	// dlg.Filters.Add(filter);
		// 	
		// 	dlg.Title = "Select Discipline Data File";
		// 	dlg.Settings.AddToMostRecentlyUsedList = false;
		// 	dlg.Settings.AllowPropertyEditing = false;
		// 	dlg.Settings.AllowNonFileSysItems = false;
		// 	dlg.Settings.ShowPlacesList = true;
		//
		// 	dlg.Dialog.IsFolderPicker = false;
		// 	dlg.Dialog.Multiselect = false;
		// 	dlg.Dialog.InitialDirectory = UserSettings.Data.DisciplineDataFileFolder;
		//
		// 	dlg.Dialog.DefaultFileName = $"{UserSettings.Data.DisciplineDataFileName}.xml";
		// 	dlg.Dialog.DefaultExtension = ".xml";
		// 	dlg.Dialog.EnsureFileExists = true;
		//
		// 	CommonFileDialogResult a =  dlg.Dialog.ShowDialog(this);
		//
		// 	selectedDataFile = dlg.Dialog.FileName;
		//
		// 	
		// }
		//
		// private void selectFolder()
		// {
		// 	CommonFileDialogFilter filter = new CommonFileDialogFilter();
		//
		// 	// filter.DisplayName = "Discipline Data File";
		// 	// filter.Extensions.Add("xml");
		// 	// filter.ShowExtensions = true;
		//
		// 	dlg = new FileAndFolderDialog();
		//
		// 	
		//
		// 	// dlg.Filters.Add(filter);
		// 	
		// 	dlg.Title = "Select Discipline Data File";
		// 	dlg.Settings.AddToMostRecentlyUsedList = false;
		// 	dlg.Settings.AllowPropertyEditing = false;
		// 	dlg.Settings.AllowNonFileSysItems = false;
		// 	dlg.Settings.ShowPlacesList = true;
		//
		// 	dlg.Dialog.IsFolderPicker = true;
		// 	dlg.Dialog.Multiselect = false;
		// 	dlg.Dialog.InitialDirectory = UserSettings.Data.DisciplineDataFileFolder;
		//
		// 	dlg.Dialog.DefaultFileName = $"{UserSettings.Data.DisciplineDataFileName}.xml";
		// 	dlg.Dialog.DefaultExtension = "xml";
		// 	dlg.Dialog.EnsureFileExists = true;
		//
		// 	CommonFileDialogResult a =  dlg.Dialog.ShowDialog(this);
		//
		// 	selectedDataFile = dlg.Dialog.FileName;
		// }
		//
		// private void getNewFile()
		// {
		// 	CommonSaveFileDialog sdg = new CommonSaveFileDialog("name");
		//
		// 	sdg.AllowPropertyEditing = false;
		// 	sdg.AlwaysAppendDefaultExtension = true;
		// 	sdg.DefaultExtension = "xml";
		// 	sdg.DefaultFileName = UserSettings.Data.DisciplineDataFileName;
		// 	sdg.EnsurePathExists = true;
		// 	sdg.InitialDirectory = UserSettings.Data.DisciplineDataFileFolder;
		// 	sdg.OverwritePrompt = true;
		// 	sdg.ShowPlacesList = true;
		// 	sdg.Title = "Save New Discipline File";
		// 	sdg.IsExpandedMode=true;
		//
		// 	CommonFileDialogFilter filter = new CommonFileDialogFilter();
		// 	filter.DisplayName = "Discipline files";
		// 	filter.Extensions.Add("xml");
		// 	
		// 	sdg.Filters.Add(filter);
		//
		// 	CommonFileDialogResult a = sdg.ShowDialog(this);
		//
		// 	if (a != CommonFileDialogResult.Ok) return;
		//
		// 	string s = sdg.FileName;
		//
		// 	IKnownFolder d = KnownFolders.Documents;
		//
		// 	string p = d.Path;
		// 	bool px = d.PathExists;
		// }


		[DebuggerStepThrough]
		public void tbl(string text)
		{
			TextBoxString += text + "\n";

			Debug.WriteLine(text);
		}

		public void tb(string text)
		{
			TextBoxString += text;

			Debug.Write(text);
		}

		public void DebugMsgLine(string text)
		{
			TextBoxString += text + "\n";
		}

		public void DebugMsg(string text)
		{
			TextBoxString += text;
		}

	#endregion

	#region event consuming

		// collection buttons
		private void BtnOpenColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			openCollection();

			DM.End0();
		}

		private void BtnNewColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			// make a new collection
			createNewCollection();

			DM.End0();
		}

		private void BtnSaveColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			saveCollection();

			DM.End0();
		}

		private void BtnSaveAsColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			saveAsCollection();

			DM.End0();
		}

		private void BtnCloseColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			closeCollection();

			DM.End0();
		}

		private void BtnDeleteColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			deleteCollection();

			DM.End0();
		}

		private void BtnCancelColl_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			cancelCollection();

			DM.End0();
		}

		// item buttons
		private void BtnNewItem_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			Lbx.UnselectAll();

			if (lbxSelectedItem != null) lbxSelectedItem.DataIsEditing = false;

			updateStatus(OpStatus.ES_NEWITEM);

			UpdateCollectionButtons();
			DM.End0();
		}

		private void BtnEditItem_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			if (lbxSelectedItem != null) lbxSelectedItem.DataIsEditing = false;

			updateStatus(OpStatus.ES_EDITITEM);

			UpdateCollectionButtons();

			DM.End0();
		}

		private void BtnSaveItem_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			if (!editingSelected)
			{
				saveNewItem();
			}
			else
			{
				saveSelectedItem();
			}

			// Collection_Changed = true;

			DisciplinesOnCollectionChanged(null, null);

			selectAndEditItem();

			// updateStatus(OpStatus.ES_SAVEDITEM);

			DM.End0();
		}

		private void BtnRemoveItem_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			// if (!removeItem()) operationFailed();

			removeItem();


			updateStatus(OpStatus.ES_CHOOSEITEM);

			// must follow updateStatus
			UpdateCollectionButtons();

			DM.End0();
		}

		private void BtnDoneEditItem_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);

			Lbx.UnselectAll();

			if (lbxSelectedItem != null) lbxSelectedItem.DataIsEditing = false;
			lbxSelectedItem = null;

			// LbxSelectedIndex = -1;

			updateStatus(OpStatus.ES_DONEEDITITEM);

			// must follow updateStatus
			UpdateCollectionButtons();

			DM.End0();
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}


		// recent
		private void LbxRecent_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);
			openRecentCollection();
			DM.End0();
		}

		private void BtnRemoveRecent_OnClick(object sender, RoutedEventArgs e)
		{
			DM.Start0();
			int idx = (int) ((Button) sender).Tag;
			// string filePathNoEx = (string) ((Button) sender).Content;

			uss.RemoveFromRecent(idx);

			DM.End0();

		}

		// list box
		private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DM.Start0(SHOW_LOCATION);
			// if (selected.DataIsEditing) return;

			lbxSelectedItem.DataIsEditing = false;

			selectAndEditItem();

			e.Handled = true;

			DM.End0();
		}

		private void Control_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void ChkBox_LostFocus(object sender, RoutedEventArgs e)
		{
			DM.Start0();

			ItemLocked = lbxSelectedItem.DisciplineData.Locked;
			OnPropertyChange(nameof(IsLoaded));

			DM.End0();
		}

		private void Tbx_LostFocus(object sender, RoutedEventArgs e)
		{
			DM.Start0();
			TextBox tbx = sender as TextBox;

			updateItemFromSelected((string) tbx.Tag);

			DM.End0();
			// OnPropertyChange(nameof(Selected));
			// OnPropertyChange(nameof(ItemTitle));

			// BindingExpression be = tbx.GetBindingExpression(TextBox.TextProperty);
			//
			// Debug.WriteLine("*** TBX CHANGED ***");
		}

		private void updateItemFromSelected(string property)
		{
			DM.Start0();
		#if DML3
			DM.Stat0($"selected: {property}");
		#endif
			switch (property)
			{
			case DISC_CODE:
				{
					ItemDisciplineCode = lbxSelectedItem.DisciplineData.DisciplineCode;
					break;
				}
			case TITLE:
				{
					ItemTitle = lbxSelectedItem.DisciplineData.Title;
					break;
				}
			case DISP_NAME:
				{
					ItemDisplayName = lbxSelectedItem.DisciplineData.DisplayName;
					break;
				}
			case DESC:
				{
					ItemDescription = lbxSelectedItem.DisciplineData.Description;
					break;
				}
			}

			OnPropertyChange(property);

			DM.End0();
		}

		private void Lbx_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			Lbx.UnselectAll();
		}


		// init design routines
		private void BtnClear1_OnClick(object sender, RoutedEventArgs e)
		{
			// disciplineMgr = new DataManager<DisciplinesDataSet>();
			createNewCollection();

			OnPropertyChange(nameof(Collection_Status));

			updateStatus(OpStatus.ES_RESET);
			// updateStatus(OpStatus.ES_RESETFIELDS);
		}

		private void BtnShow1_OnClick(object sender, RoutedEventArgs e)
		{
			TextBoxString = "";

			showDisciplines();
		}

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			createNewCollection();

			addInitData();

			OnPropertyChange(nameof(Collection_Status));

			updateStatus(OpStatus.ES_RESET);
			// updateStatus(OpStatus.ES_RESETFIELDS);
		}

		private void BtnTest2_OnClick(object sender, RoutedEventArgs e)
		{
			HeaderChanged = true;
			Collection_Opened = true;
			Collection_Changed = true;

		}

		// debug routines
		private void addInitData()
		{
			// showLocation();

			dfm.Add("A"  , "A"  ,   "Architectural", "Architectural", "Architectural Sheets", false);
			dfm.Add("S"  , "S"  ,   "Structural"   , "Structural"   , "Structural Sheets",    false);
			dfm.Add("M"  , "M"  ,   "Mechanical"   , "Mechanical"   , "Mechanical Sheets",    false);
			dfm.Add("E"  , "E"  ,   "Electrical"   , "Electrical"   , "Electrical Sheets",    false);
			dfm.Add("P"  , "P"  ,   "Plumbing"     , "Plumbing"     , "Plumbing Sheets",      false);
			dfm.Add("T24", "T24",   "T24 Sheets"   , "T24 Sheets"   , "T24 Sheets Sheets",    false);

			dfm.AddCombo("T24");

			dfm.Write();
		}

		private void showDisciplines()
		{
			// showLocation();

			tbl($"{"main",-8} {"inner",-8}");
			tbl($"{"key",-8} {"key",-8} {"title",-14} description");
			tbl("");

			foreach (KeyValuePair<string, DisciplineListData> dld
					in dfm.DdList)
			{
				tbl($">{dld.Key}< {dld.Value.OrderCode, -8} {dld.Value.DisciplineData.Title,-14} {dld.Value.DisciplineData.Description}");
			}

			tbl("");
		}

		private void DisciplinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			DM.Start0(false, "^^^ collection changed ^^^");

			Collection_Changed = true;
			SaveColl_Enabled = true;
			CancelColl_Enabled = true;

			DM.End0();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion

		// private void Lbx_SourceUpdated(object sender, DataTransferEventArgs e)
		// {
		// 	Debug.WriteLine($"****\n***   got source updated   ({e.Property.Name})   ***\n***\n");
		// }
		//
		// private void Lbx_OnTargetUpdated(object sender, DataTransferEventArgs e)
		// {
		// 	Debug.WriteLine($"****\n***   got target updated   ({e.Property.Name})   ***\n***\n");
		// }



	}
}