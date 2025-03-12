using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
// using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AndyShared.ClassificationDataSupport.TreeSupport;
using AndyShared.FileSupport.FileNameSheetPDF;

using JetBrains.Annotations;
using Test4.SheetMgr;
using Test4.Windows;
using System.Runtime.Serialization;
using AndyShared.ClassificationDataSupport.SheetSupport;
using UtilityLibrary;


namespace Test4
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged, IWin
	{
		// private static MainWindow mw;

		private MainWinSupport ms;
		private SheetPdfManager sm;

		private string tvIndexPath = "1,1";

		private ObservableCollection<TreeNode> nodes;

		private TreeViewItem priorTvi;
		private ListBoxItem priorLbi;

		private TreeViewItem priorTviAlt;
		private ListBoxItem priorLbiAlt;



		// private ListBoxItem lixi;
		// private TreeNode node2;


		private TreeNode node = null;
		private SheetCategory shtCat = null;
		private int compOpIdx = 0;
		private ComparisonOperation compOp;
		private ComparisonOperation priorCompOp;

		private TreeNode nodeCopy;
		// private SheetCategory shtCatCopy;
		// private ComparisonOperation compOpCopy;

		private ComparisonOperation compOpSelected;

		private string tvNamePath;
		private string messageBox2;


		private static string messageBox;

		public MainWindow()
		{
			InitializeComponent();

			DM.Iw = (IWin) this;

			init();
		}

		public MainWinSupport MainWinSupport => ms;

		public BaseOfTree TreeBase => MainWinSupport?.ClassificationFile?.TreeBase ?? null;

		public string MessageBox
		{
			get => messageBox;
			set
			{
				if (value == messageBox) return;
				messageBox = value;
				OnPropertyChanged();
			}
		}

		public string MessageBox2
		{
			get => messageBox2;
			set
			{
				if (value == messageBox2) return;
				messageBox2 = value;
				OnPropertyChanged();
			}
		}

		public string TvNamePath
		{
			get => tvNamePath;
			set
			{
				if (value == tvNamePath) return;
				tvNamePath = value;
				OnPropertyChanged();
			}
		}

		// public string TvIndexPath
		// {
		// 	get => tvIndexPath;
		// 	set
		// 	{
		// 		if (value == tvIndexPath) return;
		// 		tvIndexPath = value;
		// 		OnPropertyChanged();
		// 	}
		// }

		// public int CompOpIdx
		// {
		// 	get => compOpIdx;
		// 	set
		// 	{
		// 		if (value == compOpIdx) return;
		// 		compOpIdx = value;
		// 		OnPropertyChanged();
		// 	}
		// }

		public TreeNode Node
		{
			get => node;
			set
			{
				node = value;
				OnPropertyChanged();

				DebugMsgLine("tree node selected");
			}
		}

		public SheetCategory ShtCat
		{
			get => shtCat;
			set
			{
				shtCat = value;
				OnPropertyChanged();
			}
		}

		public ComparisonOperation CompOp
		{
			get => compOp;
			set
			{
				if (value == null || Equals(value, compOp)) return;
				compOp = value;
				OnPropertyChanged();

				DebugMsgLine("compOp selected");

				// if (IsInitialized) selectCompOp();
			}
		}


		public TreeNode NodeCopy
		{
			get => nodeCopy;
			set
			{
				nodeCopy = value;
				OnPropertyChanged();
			}
		}

		// public SheetCategory ShtCatCopy
		// {
		// 	get => shtCatCopy;
		// 	set
		// 	{
		// 		shtCatCopy = value;
		// 		OnPropertyChanged();
		// 	}
		// }

		// public ComparisonOperation CompOpCopy
		// {
		// 	get => compOpCopy;
		// 	set
		// 	{
		// 		if (value == null || Equals(value, compOpCopy)) return;
		// 		compOpCopy = value;
		// 		OnPropertyChanged();
		// 	}
		// }


		// selected items

		// public routines

		private void init()
		{
			bool result;

			ms = new MainWinSupport(this);
			sm = SheetPdfManager.Instance;

			// if (ms.GetClassifFile("PdfSample 1"))
			if (ms.GetClassifFile("Pdf Test 2"))
			{
				Debug.WriteLine("get classification file worked");

				nodes = ms.ClassificationFile.TreeBase.Children;
			}
			else
			{
				Debug.WriteLine("get classification file failed");
			}

			OnPropertyChanged(nameof(MainWinSupport));

			// if (ms.ParseSamples())
			// {
			// 	Debug.WriteLine("parse samples worked");
			// }
			// else
			// {
			// 	Debug.WriteLine("parse samples failed");
			// }
		}

		public void DebugMsg(string msg)
		{
			MessageBox += msg;
		}

		public void DebugMsgLine(string msg)
		{
			MessageBox += msg + "\n";
		}

		// private

		private bool update()
		{
			if (!nodeCopy.Item.ChildCompOpModified
				&&
				!nodeCopy.Item.ShtCatModified
				) return false;

			node.Item.Merge(nodeCopy.Item);

			return true;
		}

		private void deSelect()
		{
			if (node == null) return;

			node.IsNodeSelected = false;
			// tv_parent.Focus();

			node = null;
			// shtCat = null;

			// if (compOp != null)
			// {
			// 	compOp.IsSelected = false;
			// 	compOp = null;
			// 	OnPropertyChanged(nameof(CompOp));
			// }

			OnPropertyChanged(nameof(Node));

			if (priorLbi!=null) priorLbi.IsSelected = false;
			if (priorTvi!=null) priorTvi.IsSelected = false;

			nodeCopy = null;
			OnPropertyChanged(nameof(NodeCopy));

			// shtCatCopy = null;
			//
			// if (compOpCopy != null)
			// {
			// 	compOpCopy = null;
			// 	OnPropertyChanged(nameof(CompOpCopy));
			// }
			//
			// OnPropertyChanged(nameof(ShtCatCopy));

		}

		// event consuming

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		// public static event PropertyChangedEventHandler PropertyChanged_S;
		//
		// [DebuggerStepThrough]
		// [NotifyPropertyChangedInvocator]
		// private static void OnPropertyChanged_S([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged_S?.Invoke(MainWindow.mw, new PropertyChangedEventArgs(memberName));
		// }

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnClassifyTest_OnClick(object sender, RoutedEventArgs e)
		{
			// if (ms.FilterSamples("PdfSample 4"))
			if (ms.FilterSamples(ms.ClassificationFile.SampleFileName))
			{
				Debug.WriteLine("filter worked");
				OnPropertyChanged(nameof(ms.ClassificationFile));
			}
			else
			{
				Debug.WriteLine("filter failed");
				return;
			}
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			int idx0 = 3;
			int idx1 = 3;
			int idx2 = 0;
			int idxs = 0;

			BaseOfTree b = ms.ClassificationFile.TreeBase;
			ObservableCollection<TreeNode> c0 = b.Children;
			ObservableCollection<TreeNode> c1 = c0[idx0].Children;
			ObservableCollection<TreeNode> c2 = c1[idx1].Children;

			SheetCategory s = c2[idx2].Item;
			ObservableCollection<ComparisonOperation> o = s.CompareOps;
			ComparisonOperation op = o[idxs];
			FileNameSheetIdentifiers.ShtNumComps2 nd = op.CompNameData;
			LogicalComparisonOp lcop = op.LogicalComparisonOpCode;
			ValueComparisonOp vcop = op.ValueComparisonOpCode;
			LogicalCompOpDef lcp = op.LogicalCompOpDef;
			ValueCompOpDef Vcp = op.ValueCompOpDef;

			TreeNode na = c0[0];
			SheetCategory sa = na.Item;
			ComparisonOperation ca;
			if (sa.CompareOps.Count>0) ca = sa.CompareOps[0];

			if (nodeCopy != null)
			{
				TreeNode nx = nodeCopy;
				nx.ID = "nx01";
				
				SheetCategory sx = nx.Item;
				sx.ID = "sx01";

				ComparisonOperation cx = sx.CompareOps[0];
				cx.ID = "cx01";

			}

			int A = 1 + 1;


			// n.TreeNodeChildItemModified = true;
			// n.TreeNodeChildItemModified = false;
			// n.TreeNodeChildItemModified = true;
			// n.TreeNodeChildItemModified = false;

			// n.ModifyTreeNode = true;
			// n.ModifyTreeNode = false;
			// n.ModifyTreeNode = true;
			// n.ModifyTreeNode = false;


			// b.GetType().GetFields()[0].GetValue(null);
			// b.GetType().GetProperties()[0].GetValue(null);

		}


		// causes the nested listbox to transfer wheel movements to the parent treeview
		private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				MouseWheelEventArgs eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
				eventArg.RoutedEvent = UIElement.MouseWheelEvent;
				eventArg.Source = sender;
				var parent = ((Control)sender).Parent as UIElement;
				parent.RaiseEvent(eventArg);
			}
		}

		private void tv1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.OldValue != null) ((TreeNode) e.OldValue).IsNodeSelected = false;
			if (e.NewValue != null)
			{
				node = (TreeNode) e.NewValue;
				node.IsNodeSelected = true;

				// shtCat = node.Item;

				OnPropertyChanged(nameof(Node));
				// OnPropertyChanged(nameof(ShtCat));

				TvNamePath = ms.GetNodePath(node);

				nodeCopy = (TreeNode) ((TreeNode) e.NewValue).Clone(0);
				// nodeCopy.IsNodeSelected = true;
				// nodeCopy.Parent = null;

				// shtCatCopy = nodeCopy.Item;
				// compOpCopy.ChangeParentNoMod(shtCatCopy);

				OnPropertyChanged(nameof(NodeCopy));
				// OnPropertyChanged(nameof(ShtCatCopy));

			}
		}

		/*
		private void Lbi_OnSelected2(object sender, RoutedEventArgs e)
		{
			ListBoxItem lbi = sender as ListBoxItem;

			if (lbi == null) return;

			if (priorLbi != null)
			{
				priorLbi.IsSelected = false;

				priorCompOp.IsSelected = false;
			}

			compOp = lbi.Content as ComparisonOperation;
			
			compOp.IsSelected = true;

			priorCompOp = compOp;
			priorLbi = lbi;

			OnPropertyChanged(nameof(CompOp));

			// compOpCopy = (ComparisonOperation) compOp.Clone();
			// OnPropertyChanged(nameof(CompOpCopy));

			TreeViewItem tvi = lbi.Tag as TreeViewItem ?? null;

			if (tvi != null)
			{
				tvi.IsSelected = true;
				priorTvi = tvi;
			}
		}
		*/

		private void Lbi_OnSelected(object sender, RoutedEventArgs e)
		{
			ListBoxItem lbi = sender as ListBoxItem;

			if (lbi == null) return;

			if (priorLbi != null) priorLbi.IsSelected = false;
			priorLbi = lbi;

			TreeViewItem tvi = lbi.Tag as TreeViewItem ?? null;

			if (tvi == null) return;

			if (priorTvi != null) priorTvi.IsSelected = false;
			priorTvi = tvi;

			tvi.IsSelected = true;
		}

		private void Lbi_OnSelected_Alt(object sender, RoutedEventArgs e)
		{
			ListBoxItem lbi = sender as ListBoxItem;

			if (lbi == null) return;

			if (priorLbiAlt != null) priorLbiAlt.IsSelected = false;
			
			priorLbiAlt = lbi;

			// lbi.IsSelected = true;
		}

		private void BtnCheckClone_OnClick(object sender, RoutedEventArgs e)
		{

			shtCat = TreeBase.Children[0].Item.Clone() as SheetCategory;

			node = new TreeNode(null, shtCat, false);

			compOp = shtCat.CompareOps[0];

			TvNamePath = "cloned";

			OnPropertyChanged(nameof(Node));
			OnPropertyChanged(nameof(ShtCat));
			OnPropertyChanged(nameof(CompOp));

		}

		private void BtnDeselect_OnClick(object sender, RoutedEventArgs e)
		{
			deSelect();
		}

		private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
		{
			if (!update()) return;

			deSelect();
		}

		private void BtnAddCompOp_OnClick(object sender, RoutedEventArgs e)
		{
			nodeCopy.Item.AddPrelimCompOp();
		}

		private void BtnDelSelCompOp_OnClick(object sender, RoutedEventArgs e)
		{
			nodeCopy.Item.DeleteSelectedCompOp();
		}

		private void BtnClrNodeMod_OnClick(object sender, RoutedEventArgs e)
		{
			TreeBase.NotifyChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_NODE_MODIFICATION);
		}

		private void BtnClrItemMod_OnClick(object sender, RoutedEventArgs e)
		{
			TreeBase.NotifyChangeFromParent(INTERNODE_MESSAGES.IM_CLEAR_ITEM_MODIFICATION);
		}

	}
}

//
// 		private void BtnCheckClone_OnClick(object sender, RoutedEventArgs e)
// 		{
// 			string name;
// 			MemberTypes mTyp;
// 			string mType;
//
// 			string fType;
// 			string fTypeNum;
//
// 			// int mTypeNumVal;
// 			string mTypeNum;
// 			string value;
//
// 			FieldInfo[] fis = node.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
// 			PropertyInfo[] pis = node.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
// 			MemberInfo[] mis1 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);
// 			MemberInfo[] mis2 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);
// 			MemberInfo[] mis5 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty| BindingFlags.Instance);
// 			MemberInfo[] mis6 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty| BindingFlags.Instance);
// 			MemberInfo[] mis3 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.GetField);
// 			MemberInfo[] mis4 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//
// 			MemberInfo[] mis7 = node.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
//
// 			/*
// 			foreach (MemberInfo mi in mis7)
// 			{
// 				mTyp = mi.MemberType;
// 				// mTypeNumVal = (int) mi.MemberType;
// 				if (mTyp != MemberTypes.Property && mTyp != MemberTypes.Field) continue;
//
// 				name = mi.Name;
// 				mType = mi.MemberType.ToString();
// 				mTypeNum = ((int) mi.MemberType).ToString();
// 				MessageBox2 += $"{name,-40} {mType,-16} {mTypeNum,-8}\n";
// 			}
// 			*/
//
// 			foreach (FieldInfo fi in fis)
// 			{
// 				name = fi.Name;
// 				mType = fi.MemberType.ToString();
// 				mTypeNum = ((int) fi.MemberType).ToString();
//
// 				fType = fi.FieldType.ToString();
//
// 				value = fi.GetValue(node)?.ToString() ?? "is null";
//
// 				MessageBox2 += $"{name,-40} {mType,-16} {mTypeNum,-8} {value,-16} {fType}\n";
// 			}
//
// 			MessageBox2 += "***********";
// 			
// 			foreach (PropertyInfo fi in pis)
// 			{
// 				name = fi.Name;
// 				mType = fi.MemberType.ToString();
// 				mTypeNum = ((int) fi.MemberType).ToString();
//
// 				fType = fi.PropertyType.ToString();
//
// 				value = fi.GetValue(node)?.ToString() ?? "is null";
//
// 				MessageBox2 += $"{name,-40} {mType,-16} {mTypeNum,-8} {value,-16} {fType}\n";
// 			}

