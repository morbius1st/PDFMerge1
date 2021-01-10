using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilityLibrary;
using WpfShared.Dialogs.DialogSupport;
using WpfShared.Windows.ResourceFiles.SkewedBtn;
using static UtilityLibrary.MessageUtilities;

namespace WpfShared.Windows.ResourceFiles.FolderRoute
{
	/// <summary>
	/// Interaction logic for FolderRoute.xaml
	/// </summary>
	public partial class FolderRoute : UserControl, INotifyPropertyChanged
	{
		private FilePath<FileNameSimple> path;

		private DockPanel dpContainer;
		private Path pArrow;
		private Style stTbx;
		private Style stSkb;
		private Style stArx;

		private int pathIdx = 0;
		private int nextIdx = 0;

		private bool hasDefaultDockPanel;

		private bool favoriteVisible = true;
		private bool historyVisible = true;
		private bool selectVisible = true;

		public FolderRoute()
		{
			InitializeComponent();
		}

	#region public properties

		public int SelectedIndex { get; private set; }

		public string SelectedFolder { get; private set; }

		public FilePath<FileNameSimple> Path
		{
			get => path;
			private set
			{
				path = value;
				OnPropertyChanged();
				RaisePathChangeEvent();
			}
		}

		public bool HasDefaultDockPanel
		{
			get => hasDefaultDockPanel;
			set
			{
				hasDefaultDockPanel = value;
				OnPropertyChanged();
			}
		}

		public bool FavoriteVisible
		{
			get => favoriteVisible;
			set
			{
				favoriteVisible = value;
				OnPropertyChanged();
			}
		}

		public bool HistoryVisible
		{
			get => historyVisible;
			set
			{
				historyVisible = value;
				OnPropertyChanged();
			}
		}

		public bool SelectVisible
		{
			get => selectVisible;
			set
			{
				selectVisible = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region public methods

		public void ClearPath()
		{
			if (!hasDefaultDockPanel) return;

			dpContainer.Children.Clear();

			pathIdx = 0;
		}

		public void SetPath(FilePath<FileNameSimple> newPath)
		{

			ClearPath();

			foreach (string folder in newPath.GetPathNames)
			{
				AddPathButton(folder, nextIdx++);
			}

			Path = newPath;
		}

		public void AddPathButton(string text, int idx)
		{
			if (!hasDefaultDockPanel) return;

			TextBlock tb = new TextBlock();
			tb.Text = text;
			tb.Style = stTbx;

			SkewedButton skb = new SkewedButton();
			skb.Style = stSkb;
			skb.Index = idx;
			skb.Icon = makeArrow();
			skb.TextBlk = tb;
			skb.Index = pathIdx++;
			skb.Click += ButtonBase_OnClick;

			dpContainer.Children.Add(skb);
		}

	#endregion

	#region private methods

		private Path makeArrow()
		{
			Path p = new Path();
			p.Style = stArx;
			return p;
		}

		// find by type and name or type and tag
		private T getChildByNameorTag<T>(DependencyObject dObj, string name, string tag = null)
			where T : DependencyObject
		{
			if (dObj == null || (name.IsVoid() && tag.IsVoid())) return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
			{
				var child = VisualTreeHelper.GetChild(dObj, i);

				if (child != null && child is FrameworkElement f)
				{
					f = child as FrameworkElement;

					if (!name.IsVoid() && !f.Name.IsVoid() && f.Name.Equals(name)) return (T) child;

					string t = (string) f.Tag;

					if (!t.IsVoid() && t.Equals(tag)) return (T) child;
				}

				child = getChildByNameorTag<T>(child, name, tag);

				if (child != null) return (T) child;
			}

			return null;
		}

	#endregion

	#region event processing

	#endregion


	#region event consuming

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			SkewedButton skb =  (SkewedButton) sender;

			if (skb.Index != 0 && skb.Index == path.Depth - 1) return;

			// int d = path.Depth;
			// int fc = path.FolderCount;
			// int x = skb.Index;
			// string p = path.FolderPath;


			SelectedIndex = skb.Index;

			SelectedFolder = skb.TextBlk.Text;

			if (path.Depth > 0)
			{
				// string newPath = Path.AssemblePath(SelectedIndex + 1);
				SetPath(
					new FilePath<FileNameSimple>(Path.AssemblePath(SelectedIndex + 1)));
			}
			else
			{
				RaiseSelectFolderEvent();
			}
		}

		private void Favorites_OnClick(object sender, RoutedEventArgs e) => RaiseFavoritesEvent();

		private void History_OnClick(object sender, RoutedEventArgs e) => RaiseHistoryEvent();

		private void Select_OnClick(object sender, RoutedEventArgs e) => RaiseSelectFolderEvent();

		private void FldrRoute_Loaded(object sender, RoutedEventArgs e)
		{
			dpContainer = getChildByNameorTag<DockPanel>(this, null, "DpFolderPath");

			if (dpContainer == null) return;

			HasDefaultDockPanel = true;

			pArrow = (Path) this.FindResource("Arrow");
			stTbx = (Style) this.FindResource("Tblk.Base");
			stSkb = (Style) this.FindResource("Skb.Base");
			stArx = (Style) this.FindResource("Path.Arrow");

			ClearPath();
			// listVisualTree(this, 0);
		}

	#endregion

	#region event consuming

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event publishing


		// event 1, a folder was selected
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

		public event FolderRoute.PathChangedEventHandler OnPathChange;

		protected virtual void RaisePathChangeEvent()
		{
			OnPathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, Path));
		}

		// event 2, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender, EventArgs e);

		public event FolderRoute.SelectFolderEventHandler OnSelectFolderRequested;

		protected virtual void RaiseSelectFolderEvent()
		{
			OnSelectFolderRequested?.Invoke(this, new EventArgs());
		}

		// event 3, favorites was selected
		public delegate void FavoritesEventHandler(object sender, EventArgs e);

		public event FolderRoute.FavoritesEventHandler OnFavoritesPressed;

		protected virtual void RaiseFavoritesEvent()
		{
			OnFavoritesPressed?.Invoke(this, new EventArgs());
		}

		// event 4, history was selected
		public delegate void HistoryEventHandler(object sender, EventArgs e);

		public event FolderRoute.HistoryEventHandler OnHistoryPressed;

		protected virtual void RaiseHistoryEvent()
		{
			OnHistoryPressed?.Invoke(this, new EventArgs());
		}

	#endregion


	#region dependent properties

	#region SkewAngle

		public static readonly DependencyProperty SkewAngleProperty = DependencyProperty.Register(
			"SkewAngle", typeof(double), typeof(FolderRoute), new PropertyMetadata(20.0));

		public double SkewAngle
		{
			get => (double) GetValue(SkewAngleProperty);
			set => SetValue(SkewAngleProperty, value);
		}

	#endregion

	#endregion


		// object vc = this.GetVisualChild(0);
		//
		// DockPanel dp1 = (DockPanel) this.FindName("DpFolderPath");
		// DockPanel dp2 = (DockPanel) this.FindName("DpControlButtons");
		// TextBlock tbx1 = (TextBlock) this.FindName("TblkInner");
		//
		// Debug.WriteLine("visual child| " + vc.GetType());
		// Debug.WriteLine("   dp1 child| " + dp1?.Name ?? "is null");
		// Debug.WriteLine("   dp2 child| " + dp2?.Name ?? "is null");
		// Debug.WriteLine("   tbx child| " + tbx1?.Name ?? "is null");
		//
		// // listVisualTree(this, 0);
		//
		//
		// SkewedButton skb1 = getChildByNameorTag<SkewedButton>(this, null, "Skb1");
		// SkewedButton skb2 = getChildByNameorTag<SkewedButton>(this, null, "SkbFolder");
		//
		// dp1 = getChildByNameorTag<DockPanel>(this, "DpFolderPath");
		// dp2 = getChildByNameorTag<DockPanel>(this, "DpControlButtons");
		// tbx1 = getChildByNameorTag<TextBlock>(this, null, "TblkInner");
		//
		//
		// Debug.WriteLine("  skb1| " + skb1?.Tag.ToString() ?? "is null");
		// Debug.WriteLine("  skb2| " + skb2?.Tag.ToString() ?? "is null");
		// Debug.WriteLine("   dp1| " + dp1?.Name ?? "is null");
		// Debug.WriteLine("   dp2| " + dp2?.Name ?? "is null");
		// Debug.WriteLine("  tbx1| " + tbx1?.Name ?? "is null");
		//
		//
		// // SkewedButton skz = (SkewedButton) this.FindResource("SkbInner");
		// // TextBlock tbz = (TextBlock) this.FindResource("TblkInner");
		// Path Px = (Path) this.FindResource("Arrow");
		//
		// Style StTbx = (Style) this.FindResource("Tblk.Base");
		// Style StSkb = (Style) this.FindResource("Skb.Base");
		//
		//
		// Debug.WriteLine("folder route click");


		private DependencyObject listVisualTree(DependencyObject dObj, int depth)
		{
			if (dObj == null) return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
			{
				var child = VisualTreeHelper.GetChild(dObj, i);

				if (child != null)
				{
					// Debug.Write("dep obj| " + depth + " :: " + i + "  | "
					// 	+ child.DependencyObjectType.Name);

					string objName = child.DependencyObjectType.Name;

					// string d = depth.ToString();
					// string idx = i.ToString();

					string seq = depth + " :: " + i;

					string tag = (string) child.GetValue(TagProperty);
					string name = (string) child.GetValue(NameProperty);

					// if (!name.IsVoid())
					// {
					// 	Debug.Write("  name| " + name);
					//
					// }
					//
					// if (tag != null)
					// {
					// 	Debug.Write("  tag| " + tag);
					// }
					// Debug.Write("\n");


					logMsgDbLn2("dep obj",
						new int[4] {10, 30, 15, 20},
						new string[8] {"", seq, "", objName, "", tag, "", name }
						);


					listVisualTree(child, ++depth);
				}
			}

			return null;
		}
	}
}