#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   11/29/2020 12:02:18 PM

namespace TestWpf2.Data
{
	public class ExtendedData : INotifyPropertyChanged
	{
		public static int masterExtNumber;

		private ObservableCollection<MergeData> mergeInfo;

		private string extName;
		private int extNumber;
		private int lockIdx = 0;

		public ExtendedData(string name)
		{
			extNumber = masterExtNumber++;
			extName = name + extNumber.ToString(" 000");
		}

		public int LockIdx
		{
			get => lockIdx;
			set
			{
				lockIdx = value;
				OnPropertyChange();
			}
		}

		public ObservableCollection<MergeData> MergeInfo
		{
			get => mergeInfo;
			set
			{
				mergeInfo = value;
				OnPropertyChange();
				OnPropertyChange("MergeCount");
			}
		}

		public string ExtName
		{
			get => extName;
			set
			{
				extName = value;
				OnPropertyChange();
			}
		}

		public int ExtNumber
		{
			get => extNumber;
		}

		public int MergeCount => mergeInfo?.Count ?? 0;

		public void UpdateProperties()
		{
			OnPropertyChange("MergeCount");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "This is ExtendedData| " + extName;
		}
	}
}
