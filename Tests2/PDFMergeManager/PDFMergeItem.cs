// Solution:     PDFMerge1
// Project:       PDFMerge1
// File:             MergeItem.cs
// Created:      -- ()

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		private string bookmarkTitle;
		private TreeNodeType treeNodeType;
		private int pageNumber;
		private int depth;
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}