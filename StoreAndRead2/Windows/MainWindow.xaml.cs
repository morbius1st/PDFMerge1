using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using StoreAndRead.Annotations;
using StoreAndRead2.FavHistoryAdmin;
using StoreAndRead2.FhTests;
using StoreAndRead2.TestClasses;

/*
status:
basic classes made
basic save and read works

methods
Add works

next
verify other methods
- remove, insert, etc.

add methods
** adjust sample data sample lists to have more entries and
 * include job number as a part of the list to allow
 * sample entries with different job numbers
>> find via job number - provide list
>> provide views for WPF list display
- view in job number order
- view in display name order
>> revise entry's values
- actually, add new and remove old
>> remove all by job number


*/

namespace StoreAndRead.Windows
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private List<BaseClass> baseClasses = new List<BaseClass>();

		private string message;

		private FhManager fhMgr;


		public MainWindow()
		{
			InitializeComponent();

			Process();
		}

		public string Message
		{
			get => message;
			set
			{
				if (value == message) return;
				message = value;
				OnPropertyChanged();
			}
		}

		private void Process()
		{
			// TestDerivedClasses();

			TestFhManager();
		}

		private void writeline(string msg)
		{
			write(msg + "\n");
		}

		private void write(string msg)
		{
			Message += msg;
		}


		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			BaseClass.bc.RaiseCommonFunctionEvent();
		}
		
		// private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
		// {
		// 	BaseClass.bc.RaiseCommonFunctionEvent();
		// }

		private void TestDerivedClasses()
		{
			BaseClass.bc.Init();

			for (int i = 0; i < 3; i++)
			{
				baseClasses.Add(new Derived1("D1-"+i));
			}

			for (int i = 0; i < 3; i++)
			{
				baseClasses.Add(new Derived2("D2-"+i));
			}
		}

		private void TestFhManager()
		{
			writeline("Starting TestFhManager test");

			fhMgr = FhManager.Instance;

			writeline("file path| " + fhMgr.Path.SettingFilePath);

			// FhSampleData.Instance.TestMakeSampleData1();

			FhSampleData.Instance.TestMakeSampleData2();

			fhMgr.Save();
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


	}
}
