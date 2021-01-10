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

		private bool hasDefaultDockPanel;
		private bool isLoaded;

		private string whoClicked;

		public FolderRoute()
		{
			InitializeComponent();

			// Debug.WriteLine("folder route started");
		}

	#region public properties

		public int SelectedIndex { get; private set; }
		public string SelectedFolder { get; private set; }

		public FilePath<FileNameSimple> Path
		{
			get => path;
			set
			{
				path = value;
				setPath(value);
				// RaisePathChangeEvent();
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

		public bool IsFolderRouteLoaded
		{
			get => isLoaded;
			set
			{
				isLoaded = value;
				OnPropertyChanged();
			}
		}

		public string WhoClicked
		{
			get => whoClicked;
			set
			{
				whoClicked = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region public methods

		private void clearSkewedButtons() { }

	#endregion

	#region private methods

		public void ClearPath()
		{
			if (!hasDefaultDockPanel) return;

			dpContainer.Children.Clear();

			pathIdx = 0;
		}


		public void AddPathButton(string text)
		{
			if (!hasDefaultDockPanel) return;

			TextBlock tb = new TextBlock();
			tb.Text = text;
			tb.Style = stTbx;

			SkewedButton skb = new SkewedButton();
			skb.Style = stSkb;
			skb.Icon = MakeArrow();
			skb.TextBlk = tb;
			skb.Index = pathIdx++;
			skb.Click += ButtonBase_OnClick;

			dpContainer.Children.Add(skb);
		}

		private Path MakeArrow()
		{
			Path p = new Path();
			p.Style = stArx;
			return p;
		}


		private void setPath(FilePath<FileNameSimple> newPath) { }

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

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			SkewedButton skb =  (SkewedButton) sender;
			WhoClicked = skb.TextBlk.Text + " :: " + skb.Index;




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
		}


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


	#endregion


	#region event consuming

		private void FldrRoute_Loaded(object sender, RoutedEventArgs e)
		{
			IsFolderRouteLoaded = true;

			dpContainer = getChildByNameorTag<DockPanel>(this, "DpFolderPath");

			if (dpContainer == null) return;

			HasDefaultDockPanel = true;

			pArrow = (Path) this.FindResource("Arrow");
			stTbx = (Style) this.FindResource("Tblk.Base");
			stSkb = (Style) this.FindResource("Skb.Base");
			stArx = (Style) this.FindResource("Path.Arrow");


			// listVisualTree(this, 0);
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		// event 1, a folder was selected
		public delegate void PathChangedEventHandler(object sender, PathChangeArgs e);

		public event FolderRoute.PathChangedEventHandler PathChange;

		protected virtual void RaisePathChangeEvent()
		{
			PathChange?.Invoke(this, new PathChangeArgs(SelectedIndex, SelectedFolder, Path));
		}

		// event 2, the select folder button was pressed
		public delegate void SelectFolderEventHandler(object sender, EventArgs e);

		public event FolderRoute.SelectFolderEventHandler SelectFolder;

		protected virtual void RaiseSelectFolderEvent()
		{
			SelectFolder?.Invoke(this, new EventArgs());
		}

		// event 3, favorites was selected
		public delegate void FavoritesEventHandler(object sender, EventArgs e);

		public event FolderRoute.FavoritesEventHandler Favorites;

		protected virtual void RaiseFavoritesEvent()
		{
			Favorites?.Invoke(this, new EventArgs());
		}

		// event 4, history was selected
		public delegate void HistoryEventHandler(object sender, EventArgs e);

		public event FolderRoute.HistoryEventHandler History;

		protected virtual void RaiseHistoryEvent()
		{
			History?.Invoke(this, new EventArgs());
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
	}
}