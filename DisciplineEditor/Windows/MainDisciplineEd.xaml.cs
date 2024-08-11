#region using

using System.Collections.Generic;
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
using JetBrains.Annotations;

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
	public partial class MainWindow : Window, INotifyPropertyChanged
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
			ES_BLANKITEM,
			// an item has been selected
			ES_SELECTITEM,
			// request user to select existing or new item
			ES_CHOOSEITEM,
			// exit editing an item and lose changes
			ES_CANCELEDITITEM,
			// edit an item button selected
			ES_EDITITEM,
			// new collection button selected
			ES_NEWCOLL,
			// cancel collection button selected
			ES_CANCELCOLL,
		}


	#region private fields

		private const string FT_SEL_ITEM = "Edit Item";
		private const string FT_EDITING_ITEM = "Editing Item";
		private const string FT_1ST_NEW_ITEM = "Creating First Item";
		private const string FT_NEW_ITEM = "Enter new Item";
		private const string FT_CANCEL_EDIT_ITEM = "Cancelling Edit this Item";
		private const string FT_CHOOSE_ITEM = "Select existing or create a new item";
		private const string FT_MT_COLLECTION = "Collection is Empty";
		private const string FT_RESET = "Select Option";
		private const string FT_KEY_NOT_UNIQUE = "Current Key is Not Unique";


		private DataManager<DisciplinesDataSet> disciplineMgr;
		private string textBoxString = "initialized\n";
		private string fieldsTitle = "Selected Item";
		private string fieldsTitleTemp;

		private DisciplineListData selected;
		// private DisciplineListData proposed;

		private string itemKey;
		private string itemTitle;
		private string itemDescription;

		private bool removeEnabled = false;
		private bool updateEnabled = false;
		private bool editEnabled = false;
		// private bool selectItemEnabled = false;
		private bool cancelEnabled = false;
		private bool newEnabled = false;

		private bool hasChanges = false;
		private bool canEdit = false;
		private bool canSave = false;

		private bool editingNew;
		private bool editingSelected;

		private int selectedIndex = 0;

		private bool newCollEnabled;
		private bool cancelCollEnabled;
		private bool saveCollEnabled;

		// private bool exitEnabled;

		private bool collectionChanged = false;
		private bool canSaveCollection = false;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			config();

		}

		private void config()
		{
			disciplineMgr = new DataManager<DisciplinesDataSet>();

			configCollChangedEvent();

			initData();
		}

		private void initData()
		{
			if (disciplineMgr.Path.Exists)
			{
				tbl("status: read existing");

				disciplineMgr.Admin.Read();

				if (Disciplines.Count > 0)
				{
					tbl("status: existing HAS items");
					selectedIndex = 0;
					selectItem();
				}
				else
				{
					selectedIndex = -1;
					updateStatus(OpStatus.ES_EXISTREADEMPTY);
				}
			}
			else
			{
				updateStatus(OpStatus.ES_RESET);

				NewColl_Enabled = true;
			}

			OnPropertyChange(nameof(Disciplines));

			updateDataFileProps();
		}

		private void updateDataFileProps()
		{
			OnPropertyChange(nameof(DataFilePath));
			OnPropertyChange(nameof(DataFileName));
			OnPropertyChange(nameof(DataFileExists));
		}

		private void configCollChangedEvent()
		{
			disciplineMgr.Data.DisciplineList.CollectionChanged
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
			get => disciplineMgr?.Data.DisciplineList ?? null;
			// private set
			// {
			// 	disciplineMgr.Data.DisciplineList = value;
			// 	OnPropertyChange();
			// }
		}

		public DisciplineListData Selected
		{
			get => selected;
			set
			{
				showLocation();

				selected = value;
				OnPropertyChange();

				selectItem();
			}
		}

		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				if (value == selectedIndex) return;
				selectedIndex = value;
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

		// data manager

		public string DataFilePath => disciplineMgr?.Path?.SettingFilePath ?? "null";

		public string DataFileName => disciplineMgr?.Path?.FileName ?? "null";

		public string DataFileExists => disciplineMgr?.Path?.Exists.ToString() ?? "null";




	#region fields

		public string ItemKey
		{
			get => itemKey;

			set
			{
				if ((value?.Trim() ?? "").Equals(itemKey)) return;
				
				itemKey = value;

				OnPropertyChange();

				if (itemKey!=null) HasChanges = true;
			}
		}

		public string ItemTitle
		{
			get => itemTitle;
			set
			{
				if (value == itemTitle) return;
				itemTitle = value;
				OnPropertyChange();

				if (itemTitle!=null) HasChanges = true;
			}
		}

		public string ItemDescription
		{
			get => itemDescription;
			set
			{
				if (value == itemDescription) return;
				itemDescription = value;
				OnPropertyChange();

				if (itemDescription!=null) HasChanges = true;
			}
		}

	#endregion


	#region status

	#region field status

		public bool CanEdit
		{
			get => canEdit;
			set
			{
				if (value == canEdit) return;
				canEdit = value;
				OnPropertyChange();
			}
		}

		private void updateCanSave()
		{
			if (!editingSelected &&
				!isKeyUnique(itemKey))
			{
				CanSave = false;
				return;
			}

			bool a = !itemKey.IsVoid();
			bool b = !itemTitle.IsVoid();
			bool c = !itemDescription.IsVoid();
			bool d = hasChanges;

			bool e = a && b && c && d;

			CanSave = e;

			SaveItem_Enabled = e;
		}

		private bool isKeyUnique(string key)
		{
			if (disciplineMgr == null) return false;

			if (disciplineMgr.Data.IsNewKey(key))
			{
				FieldsTitleTemp = null;
				return true;
			}

			FieldsTitleTemp = FT_KEY_NOT_UNIQUE;
			return false;
		}

		public bool EditingNew
		{
			get => editingNew;
			set
			{
				if (value == editingNew) return;
				editingNew = value;
				OnPropertyChange();
			}
		}

		public bool EditingSelected
		{
			get => editingSelected;
			set
			{
				if (value == editingSelected) return;
				editingSelected = value;
				OnPropertyChange();
			}
		}

		public bool CanSave
		{
			get => canSave;
			set
			{
				if (canSave == value) return;
				canSave = value;
				OnPropertyChange();
				// OnPropertyChange(nameof(SaveItem_Enabled));
			}
		}

		public bool HasChanges
		{
			get => hasChanges;
			private set
			{
				hasChanges = value;
				OnPropertyChange();
				updateCanSave();
				// OnPropertyChange(nameof(SaveItem_Enabled));
			}
		}



	#endregion

	#region item edit

		public bool NewItem_Enabled
		{
			get => newEnabled && Collection_status;
			set
			{
				if (value == newEnabled  && Collection_status) return;
				newEnabled = value   && Collection_status;
				OnPropertyChange();
			}
		}

		// public bool SelectItem_Enabled
		// {
		// 	get => selectItemEnabled;
		// 	set
		// 	{
		// 		if (value == selectItemEnabled && Collection_status) return;
		// 		selectItemEnabled = value && Collection_status;
		// 		OnPropertyChange();
		// 	}
		// }

		public bool EditItem_Enabled
		{
			get => editEnabled && Collection_status;
			set
			{
				if (value == editEnabled   && Collection_status) return;
				editEnabled = value  && Collection_status;
				OnPropertyChange();
			}
		}

		public bool SaveItem_Enabled
		{
			// get => updateEnabled;
			get => updateEnabled && Collection_status && CanSave;
			set
			{
				if (value == updateEnabled) return;
				updateEnabled = value;
				OnPropertyChange();
			}
		}

		public bool Remove_Enabled
		{
			get => removeEnabled && Collection_status;
			set
			{
				if (value == removeEnabled   && Collection_status) return;
				removeEnabled = value   && Collection_status;
				OnPropertyChange();
			}
		}

		public bool CancelItem_Enabled
		{
			get => cancelEnabled && Collection_status;
			set
			{
				if (value == cancelEnabled   && Collection_status) return;
				cancelEnabled = value   && Collection_status;
				OnPropertyChange();
			}
		}

	#endregion

	#region collection

		public bool Collection_Changed
		{
			get => collectionChanged;
			set
			{
				if (value == collectionChanged) return;
				collectionChanged = value;
				OnPropertyChange();
			}
		}

		public bool Collection_status
		{
			get =>  Disciplines != null;
			set { OnPropertyChange(); }
		}

		public bool NewColl_Enabled
		{
			get => newCollEnabled;
			set
			{
				if (value == newCollEnabled) return;
				newCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool SaveColl_Enabled
		{
			get => saveCollEnabled;
			set
			{
				if (value == saveCollEnabled) return;
				saveCollEnabled = value;
				OnPropertyChange();
			}
		}

		public bool CancelColl_Enabled
		{
			get => cancelCollEnabled;
			set
			{
				if (value == cancelCollEnabled) return;
				cancelCollEnabled = value;
				OnPropertyChange();
			}
		}

	#endregion

		// system
		public bool ExitEnabled
		{
			get => !SaveColl_Enabled;
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

	#endregion

	#region private methods

		private void updateStatus(OpStatus status)
		{
			switch (status)
			{
			// reset to base status
			case OpStatus.ES_RESET:
				{
					tbl("status: Reset");
					FieldsTitle = FT_RESET;

					CanEdit = false;
					CanSave = false;
					HasChanges = false;

					EditingNew = false;
					EditingSelected = false;


					NewItem_Enabled = false;
					SaveItem_Enabled = false;
					Remove_Enabled = false;
					EditItem_Enabled = false;
					CancelItem_Enabled = false;

					clearItemProps();

					OnPropertyChange(nameof(Disciplines));

					break;
				}

			case OpStatus.ES_EXISTREADEMPTY:
				{
					tbl("status: existing has NO items");

					// CanEdit = false;
					// CanSave = false;
					// HasChanges = false;
					//
					// EditingNew = false;
					// EditingSelected = false;
					//
					// NewItem_Enabled = true;
					// SaveItem_Enabled = false;
					// Remove_Enabled = false;
					// EditItem_Enabled = false;
					// CancelItem_Enabled = false;
					//
					// ItemKey = null;
					// ItemTitle = null;
					// ItemDescription = null;
					
					updateStatus(OpStatus.ES_RESET);

					NewItem_Enabled = true;

					EditItem_Enabled = true;
					Remove_Enabled = false;
					CancelItem_Enabled = false;

					break;
				}

			
			case OpStatus.ES_EXISTREADNOTEMPTY:
				{
					tbl("status: existing has NO items");

					// CanEdit = false;
					// CanSave = false;
					// HasChanges = false;
					//
					// EditingNew = false;
					// EditingSelected = false;
					//
					// NewItem_Enabled = true;
					// SaveItem_Enabled = false;
					// Remove_Enabled = false;
					// EditItem_Enabled = false;
					// CancelItem_Enabled = false;
					//
					// ItemKey = null;
					// ItemTitle = null;
					// ItemDescription = null;

					EditItem_Enabled = false;
					Remove_Enabled = false;
					CancelItem_Enabled = true;

					break;
				}

			case OpStatus.ES_BLANKITEM:
				{
					tbl("status: blank item");

					CanEdit = false;
					CanSave = false;
					HasChanges = false;

					NewItem_Enabled = false;
					SaveItem_Enabled = false;

					

					break;
				}

			// begin new item
			case OpStatus.ES_NEWITEM:
				{
					FieldsTitle = FT_NEW_ITEM;

					updateStatus(OpStatus.ES_BLANKITEM);

					clearItemProps();

					tbl("status: new item");

					CanEdit = true;

					EditingNew = true;
					EditingSelected = false;

					EditItem_Enabled = false;
					Remove_Enabled = false;
					CancelItem_Enabled = true;


					break;
				}
			case OpStatus.ES_1STITEM:
				{
					FieldsTitle = FT_1ST_NEW_ITEM;

					updateStatus(OpStatus.ES_BLANKITEM);

					clearItemProps();

					tbl("status: 1st item");

					CanEdit = true;

					EditingNew = false;
					EditingSelected = false;

					EditItem_Enabled = false;
					Remove_Enabled = false;
					CancelItem_Enabled = false;

					break;
				}
			case OpStatus.ES_SELECTITEM:
				{
					FieldsTitle = FT_SEL_ITEM;

					updateStatus(OpStatus.ES_BLANKITEM);

					tbl("status: item selected");

					CanEdit = true;

					EditingNew = false;
					EditingSelected = true;

					Remove_Enabled = true;
					EditItem_Enabled = false;
					CancelItem_Enabled = true;

					break;
				}
			case OpStatus.ES_EDITITEM:
				{
					tbl("status: editing item");

					FieldsTitle = FT_EDITING_ITEM;

					CanEdit = true;
					CanSave = true;
					HasChanges = false;

					EditingNew = false;
					EditingSelected = true;

					NewItem_Enabled = false;
					SaveItem_Enabled = true;
					Remove_Enabled = true;
					EditItem_Enabled = false;
					CancelItem_Enabled = true;

					break;
				}

			case OpStatus.ES_CANCELEDITITEM:
				{
					tbl("status: cancel editing item");

					FieldsTitle = FT_CHOOSE_ITEM;

					CanEdit = false;
					CanSave = false;
					HasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					NewItem_Enabled = true;
					SaveItem_Enabled = false;
					Remove_Enabled = false;
					EditItem_Enabled = false;
					CancelItem_Enabled = false;

					clearItemProps();

					break;
				}


			case OpStatus.ES_CHOOSEITEM:
				{
					tbl("status: choose item");

					FieldsTitle = FT_CHOOSE_ITEM;

					CanEdit = false;
					CanSave = false;
					HasChanges = false;

					EditingNew = false;
					EditingSelected = false;

					NewItem_Enabled = true;
					SaveItem_Enabled = false;
					Remove_Enabled = false;
					EditItem_Enabled = false;
					CancelItem_Enabled = false;

					break;
				}


			case OpStatus.ES_NEWCOLL:
				{
					tbl("status: new collection");

					NewColl_Enabled = false;
					SaveColl_Enabled = false;
					CancelColl_Enabled = true;

					updateStatus(OpStatus.ES_1STITEM);

					break;
				}
			case OpStatus.ES_CANCELCOLL:
				{
					tbl("status: cancel collection");

					FieldsTitle = FT_MT_COLLECTION;

					CanEdit = true;
					HasChanges = false;

					NewItem_Enabled = false;
					CancelItem_Enabled = false;
					EditItem_Enabled = false;
					SaveItem_Enabled = false;
					Remove_Enabled = false;

					NewColl_Enabled = true;

					SaveColl_Enabled = false;
					CancelColl_Enabled = false;

					clearItemProps();

					break;
				}

			}
		}


		private void tbl(string text)
		{
			TextBoxString += text + "\n";

			Debug.WriteLine(text);
		}

		private void tb(string text)
		{
			TextBoxString += text;

			Debug.Write(text);
		}


		// item routines
		private void clearItemProps()
		{
			showLocation();

			ItemKey = null;
			ItemTitle = null;
			ItemDescription = null;
		}

		private void clearItemFields()
		{
			itemKey = null;
			itemTitle = null;
			itemDescription = null;
		}

		private void selectItem()
		{
			showLocation();

			if (selected == null) return;

			updateStatus(OpStatus.ES_SELECTITEM);

			itemKey = selected.DisciplineData.Key;
			itemTitle = selected.DisciplineData.Title;
			itemDescription = selected.DisciplineData.Description;

			updateItemProps();
		}

		private void updateItemProps()
		{
			OnPropertyChange(nameof(ItemKey));
			OnPropertyChange(nameof(ItemTitle));
			OnPropertyChange(nameof(ItemDescription));
		}

		private bool removeItem()
		{
			showLocation();

			return disciplineMgr.Data.Remove(itemKey);
		}

		private void saveNewItem()
		{
			showLocation();

			selected = disciplineMgr.Data.Add(itemKey, itemTitle, itemDescription);

			OnPropertyChange(nameof(Disciplines));
		}

		private void saveSelectedItem()
		{
			showLocation();

			if (itemKey.Equals(selected.DisciplineData.Key))
			{
				saveSelectedSameKey();
			}
			else
			{
				if (!saveSelectedChangeKey()) return;
			}

			updateStatus(OpStatus.ES_RESET);
		}

		// item helpers
		private void saveSelectedSameKey()
		{
			showLocation();

			selected.DisciplineData.Title = itemTitle;
			selected.DisciplineData.Description = itemDescription;
		}

		private bool saveSelectedChangeKey()
		{
			showLocation();

			string keyCurr = selected.DisciplineData.Key;
			string keyNew = itemKey;

			if (disciplineMgr.Data.UpdateKey(keyCurr, keyNew).GetValueOrDefault(false) != true) return false;

			// key updated

			selected = disciplineMgr.Data.Find(keyNew);

			return true;
		}


		// collection routines
		private void createEmptyCollection()
		{
			showLocation();

			disciplineMgr.Data.Reset();

			updateDataFileProps();

			OnPropertyChange(nameof(Disciplines));

			tbl("created\n");
			tbl($"file name| {disciplineMgr.Path.FileNameNoExt}");
			tbl($"file path| {disciplineMgr.Path.SettingFilePath}");
		}

		private void saveCollection()
		{
			showLocation();

			disciplineMgr.Admin.Write();

			NewColl_Enabled = true;
			SaveColl_Enabled = false;
			CancelColl_Enabled = true;
		}


		// system routines
		private void operationFailed([CallerMemberName] string memberName = "")
		{
			tbl($"the operation| {memberName} failed");
		}


		// debug routines
		private void addInitData()
		{
			showLocation();

			disciplineMgr.Data.Add("A", "Architectural", "Architectural Sheets");
			disciplineMgr.Data.Add("S", "Structural"   , "Structural Sheets");
			disciplineMgr.Data.Add("M", "Mechanical"   , "Mechanical Sheets");
			disciplineMgr.Data.Add("E", "Electrical"   , "Electrical Sheets");
			disciplineMgr.Data.Add("P", "Plumbing"     , "Plumbing Sheets");
			disciplineMgr.Data.Add("T24", "Plumbing"     , "Plumbing Sheets");

			disciplineMgr.Data.ComboDisciplines.Add("T24");

			disciplineMgr.Admin.Write();
		}

		private void showDisciplines()
		{
			showLocation();

			tbl($"{"main",-8} {"inner",-8}");
			tbl($"{"key",-8} {"key",-8} {"title",-14} description");
			tbl("");

			foreach (KeyValuePair<string, DisciplineListData> dld in disciplineMgr.Data.DisciplineList)
			{
				tbl($">{dld.Key}< {dld.Value.Key, -8} {dld.Value.DisciplineData.Title,-14} {dld.Value.DisciplineData.Description}");
			}

			tbl("");
		}

		private void showBtnLocation([CallerMemberName] string memberName = "")
		{
			tbl($"@BTN: {memberName}");
		}

		private void showLocation([CallerMemberName] string memberName = "")
		{
			tbl($"@ {memberName}");
		}

	#endregion

	#region event consuming

		// collection
		private void BtnNewColl_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			// make a new collection

			createEmptyCollection();

			updateStatus(OpStatus.ES_NEWCOLL);
			// updateStatus(OpStatus.ES_NEWITEM);

		}

		private void BtnSaveColl_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			// save the collection
			saveCollection();


		}

		private void BtnCancelColl_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			// eliminate any changes and return to an empty collection

			// createEmptyCollection();
			//
			// updateStatus(OpStatus.ES_CANCELCOLL);

			disciplineMgr.Data.Reset();

			initData();
		}


		// items
		private void BtnNewItem_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			updateStatus(OpStatus.ES_NEWITEM);
		}

		// private void BtnSelectItem_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	ItemKey = selected.DisciplineData.Key;
		// 	ItemTitle = selected.DisciplineData.Title;
		// 	ItemDescription = selected.DisciplineData.Description;
		//
		// 	updateStatus(OpStatus.ES_SELECTITEM);
		// }

		private void BtnEditItem_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			updateStatus(OpStatus.ES_EDITITEM);
		}

		private void BtnSaveItem_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			if (!editingSelected)
			{
				saveNewItem();
			}
			else
			{
				saveSelectedItem();

				selectItem();
			}

		}

		private void BtnRemoveItem_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			// if (!removeItem()) operationFailed();

			removeItem();

			updateStatus(OpStatus.ES_CHOOSEITEM);
		}

		private void BtnCancelEditItem_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			SelectedIndex = -1;

			updateStatus(OpStatus.ES_CANCELEDITITEM);
		}


		private void BtnClear1_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			// disciplineMgr = new DataManager<DisciplinesDataSet>();
			createEmptyCollection();

			OnPropertyChange(nameof(Collection_status));

			updateStatus(OpStatus.ES_RESET);
			// updateStatus(OpStatus.ES_RESETFIELDS);
		}

		private void BtnShow1_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			TextBoxString = "";

			showDisciplines();
		}

		private void BtnTest1_OnClick(object sender, RoutedEventArgs e)
		{
			showBtnLocation();

			createEmptyCollection();

			addInitData();

			OnPropertyChange(nameof(Collection_status));

			updateStatus(OpStatus.ES_RESET);
			// updateStatus(OpStatus.ES_RESETFIELDS);
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}


		private void DisciplinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Collection_Changed = true;

			SaveColl_Enabled = true;
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
	}
}