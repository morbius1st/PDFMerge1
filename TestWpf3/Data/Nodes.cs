#region + Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   11/29/2020 12:00:06 PM

namespace TestWpf2.Data
{
	public class Node : INotifyPropertyChanged
	{
		public static int masterNumber = 0;

		private ObservableCollection<Node> childNodes;

		private string name;
		private int number;
		private ExtendedData extData;

		protected int extCount;
		protected int extMergeCount;

		public Node(string name)
		{
			number = masterNumber++;
			this.name = name + number.ToString(" 000");
		}

		public ObservableCollection<Node> ChildNodes
		{
			get => childNodes;
			set
			{
				childNodes = value;
				OnPropertyChange();
			}
		}

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChange();
			}
		}

		public int Number
		{
			get => number;
		}

		public ExtendedData ExtData
		{
			get => extData;
			set
			{
				extData = value;
				OnPropertyChange();
			}
		}

		public int Count => childNodes?.Count ?? 0;

		public int ExtCount
		{
			get => extCount;
			set
			{
				extCount = value;
				OnPropertyChange();
			}
		}

		public int ExtMergeCount
		{
			get => extMergeCount;
			set
			{
				extMergeCount = value;
				OnPropertyChange();
			}
		}

		public int MergeCount => extData.MergeCount;

		public int exCount()
		{
			int count = childNodes?.Count ?? 0;

			if (count > 0)
			{
				foreach (Node child in childNodes)
				{
					count += child.exCount();
				}
			}

			ExtCount = count;

			return count;
		}

		public int exMergeCount()
		{
			int count = extData.MergeCount;

			if ((childNodes?.Count ?? 0) > 0)
			{
				foreach (Node child in childNodes)
				{
					count += child.exMergeCount();
				}
			}

			ExtMergeCount = count;

			return count;
		}

		public void ClrMergeItems()
		{
			extData.MergeInfo = new ObservableCollection<MergeData>();

			if ((childNodes?.Count ?? 0) == 0) return;

			foreach (Node child in childNodes)
			{
				child.ClrMergeItems();
			}
		}

		public void UpdateProperties()
		{
			OnPropertyChange("ExtCount");
			OnPropertyChange("ExtMergeCount");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "This is Node| " + name;
		}
	}

	public class BaseOfTree : Node
	{
		public BaseOfTree(string name) : base(name) { }

		public override string ToString()
		{
			return "This is Base of Tree Node| " + Name;
		}
	}
}