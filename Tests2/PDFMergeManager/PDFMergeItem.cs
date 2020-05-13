// Solution:     PDFMerge1
// Project:       PDFMerge1
// File:             MergeItem.cs
// Created:      -- ()

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tests2.FileListManager;

namespace Tests2.PDFMergeManager
{
	public enum TreeNodeType
	{
		INVALID,
		BRANCH,
		LEAF
	}

	public class PDFMergeItem : INotifyPropertyChanged
	{
		private enum SelectedState
		{
			UNDEFINED = 100,
			UNCHECK = 0,
			CHECKED = 2,
			PARTIAL = 10,
			CHK_WAS_PARTIAL = 11,
			CHK_WAS_UNCHECK = 1,
			UNCHK_WAS_PARTIAL = -11,
			UNCHK_WAS_SELECTED = -2
		}


		private string bookmarkTitle;
		private TreeNodeType treeNodeType;
		private int pageNumber;
		private int depth;
		private bool? isSelected;

#pragma warning disable CS0414 // The field 'PDFMergeItem.currentState' is assigned but its value is never used
		private SelectedState currentState = SelectedState.UNDEFINED;
#pragma warning restore CS0414 // The field 'PDFMergeItem.currentState' is assigned but its value is never used

		private FileItem fileItem;
		private ObservableCollection<KeyValuePair<string, PDFMergeItem>> mergeItems;

		public string BookmarkTitle
		{
			get => bookmarkTitle;
			set
			{
				bookmarkTitle = value;
				OnPropertyChange();
			}
		}

		public TreeNodeType TreeNodeType
		{
			get => treeNodeType;
			set
			{
				treeNodeType = value;
				OnPropertyChange();
			}
		}

		public int PageNumber
		{
			get => pageNumber;
			set
			{
				pageNumber = value;
				OnPropertyChange();
			}
		}

		public int Depth
		{
			get => depth;
			set
			{
				depth = value;
				OnPropertyChange();
			}
		}

		public bool? IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;
				OnPropertyChange();
			}
		}

		public FileItem FileItem
		{
			get => fileItem;
			set
			{
				fileItem = value;
				OnPropertyChange();
			}
		}

		public ObservableCollection<KeyValuePair<string, PDFMergeItem>> MergeItems
		{
			get => mergeItems;
			private set
			{
				mergeItems = value;
				OnPropertyChange();
			}
		}

		public PDFMergeItem(
			string bookmarkTitle,
			TreeNodeType treeNodeType,
			int pageNumber,
			int depth,
			FileItem fileItem
			)
		{
			this.bookmarkTitle = bookmarkTitle;
			this.treeNodeType = treeNodeType;
			this.pageNumber = pageNumber;
			this.depth = depth;
			this.fileItem = fileItem;
			this.MergeItems = null;
		}

		internal bool hasChildren => MergeItems.Count > 0;

		internal int count => MergeItems.Count;

		public void AddMergeItems()
		{
			MergeItems = new ObservableCollection<KeyValuePair<string, PDFMergeItem>>();
		}

//		public void AddPropChg(PropertyChangedEventHandler e)
//		{
//			e += E;
//		}

		public void E(object sender, PropertyChangedEventArgs e)
		{
			Debug.WriteLine("got event");
		}

//
//		private SelectedState SwitchSelectedState(int operation)
//		{
//			if (operation == 1)
//			{
//				switch (isSelected)
//				{
//				case true:
//					currentState = SelectedState.CHECKED
//					break;
//				case false:
//					currentState = SelectedState.CHK_WAS_UNCHECK;
//					break;
//				case null:
//					currentState = SelectedState.CHK_WAS_PARTIAL;
//					break;
//				}
//
//				IsSelected = true;
//
//			}
//			else if (operation == 2)
//			{
//				switch (current)
//				{
//				case SelectedState.CHK_WAS_PARTIAL:
//					return SelectedState.UNCHK_WAS_PARTIAL;
//				case SelectedState.CHECKED:
//				case SelectedState.PARTIAL:
//					return SelectedState.UNCHK_WAS_SELECTED;
//				case SelectedState.CHK_WAS_UNCHECK:
//					return SelectedState.UNCHECK;
//				}
//
//				return current;
//			}
//			else
//			{
//				// operation 3
//				switch (current)
//				{
//				case SelectedState.UNCHK_WAS_PARTIAL:
//					return SelectedState.PARTIAL;
//				case SelectedState.UNCHK_WAS_SELECTED:
//					return SelectedState.UNCHECK;
//				case SelectedState.UNCHECK:
//					return SelectedState.UNCHECK;
//				}
//			}
//
//			return current;
//		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}