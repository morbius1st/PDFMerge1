﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SettingManager;
using Tests2.FileListManager;
using Tests2.OutlineManager;

// projname: SettingsManagerV30
// itemname: UserSettingInfo22
// username: jeffs
// created:  12/23/2018 1:14:35 PM


namespace Tests2.Settings
{
	public static class UserSettings
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<UserSettingInfo30> Admin { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettingInfo30 Info { get; private set; }
		public static UserSettingData30 Data { get; private set; }

		// initialize and create the setting objects
		static UserSettings()
		{
			Admin = new SettingsMgr<UserSettingInfo30>(ResetData);
			Info = Admin.Info;
			Data = Info.Data;
		}

		public static void ResetData()
		{
			// this makes sure the above static class points
			// to the current data structure
			Info  = Admin.Info;
			Data  = Info.Data;
		}
	}

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	
	[DataContract(Name = "UserSettingData30")]
	public class UserSettingData30
	{
//		public Route GetInitFolder()
//		{
//			return new Route(PriorFolder);
//		}
//		public void SetInitFolder(Route priorFolder)
//		{
//			PriorFolder = priorFolder.FullRoute;
//		}

		[DataMember]
		public string PriorFolder = @"C:\2099-999 Sample Project\Publish\Bulletins";

		[DataMember]
		public bool AllowOverwriteOutputFile = true;

		[DataMember]
		public List<OutlineItem> OutlineItems = new List<OutlineItem>()
//		{
//			new OutlineItem(  "072 Architectural - Plans",
//				"Architectural Plan Sheets", "A A2", "07002"),
//			new OutlineItem(  "070 Architectural",
//				"All Architectural Sheets", "A A", "070"),
//			new OutlineItem(  "072 Architectural - Floor Plans",
//				"Architectural Plan Sheets", "A A2.1", "0700201")
//		}
			;


//		// added with version 2.1
//		[DataMember]
//		public int UnCategorizedValue2 = 2000;
//
//		// added with version 2.2
//		[DataMember]
//		public int UnCategorizedValue3 = 3000;

//		[DataMember]
//		public GeneralValues GeneralValues = new GeneralValues();
//
//		[DataMember]
//		public Window1 MainWindow { get; set; } = new Window1();
//
//		[DataMember(Name = "DictionaryTest3")]
//		public CustDict<string, TestStruct> TestDictionary3 =
//			new CustDict<string, TestStruct>()
//			{
//				{"one", new TestStruct(1, 2, 3)},
//				{"two", new TestStruct(1, 2, 3)},
//				{"three", new TestStruct(1, 2, 3)}
//			};
	}

//	// sample sub-class of dictionary to provide names to elements
//	[CollectionDataContract(Name = "CustomDict", KeyName = "key", ValueName = "data", ItemName = "row")]
//	public class CustDict<T1, T2> : Dictionary<T1, T2>
//	{
//	}
//
//	// sample struct / data
//	public struct TestStruct
//	{
//		[DataMember(Name = "line1")]
//		public int IntA;
//
//		[DataMember(Name = "line2")]
//		public int IntB;
//
//		[DataMember(Name = "line3")]
//		public int IntC;
//
//		public TestStruct(int a,
//			int b,
//			int c)
//		{
//			IntA = a;
//			IntB = b;
//			IntC = c;
//		}
//	}

	[DataContract(Name = "UserSettingInfo30")]
	public class UserSettingInfo30 : UserSettingBase
	{
		[DataMember]
		public UserSettingData30 Data = new UserSettingData30();

		public override string ClassVersion => "0.0";

		public override void UpgradeFromPrior(SettingBase prior)
		{
			// no implementation - first invocation


//			UserSettingInfo21 p = (UserSettingInfo21) prior;
//
//			Header.Notes =
//				p.Header.Notes + " :: updated to v" + ClassVersion;
//
//			Data.UnCategorizedValue2 = p.Data.UnCategorizedValue2;
//			Data.UnCategorizedValue  = p.Data.UnCategorizedValue;
//			Data.GeneralValues.TestB = p.Data.GeneralValues.TestB;
//			Data.GeneralValues.TestD = p.Data.GeneralValues.TestD;
//			Data.GeneralValues.TestI = p.Data.GeneralValues.TestI;
//			Data.GeneralValues.TestS = p.Data.GeneralValues.TestS;
//			Data.MainWindow.Height   = p.Data.MainWindow.Height;
//			Data.MainWindow.Width    = p.Data.MainWindow.Width;
//
//			for (int i = 0; i < Data.GeneralValues.TestIs.Length; i++)
//			{
//				Data.GeneralValues.TestIs[i] =
//					p.Data.GeneralValues.TestIs[i];
//			}
//
//			p.Data.GeneralValues.TestSs.CopyTo(Data.GeneralValues.TestSs, 0);
//
//			foreach (KeyValuePair<string, TestStruct> kvp in p.Data.TestDictionary3)
//			{
//				if (Data.TestDictionary3.ContainsKey(kvp.Key))
//				{
//					Data.TestDictionary3[kvp.Key] =
//						p.Data.TestDictionary3[kvp.Key];
//				}
//			}
		}
	}

//	public class GeneralValues
//	{
//		public int      TestI  = 0;
//		public bool     TestB  = false;
//		public double   TestD  = 0.0;
//		public string   TestS  = "this is a test";
//		public int[]    TestIs = new[] { 20, 30 };
//		public string[] TestSs = new[] { "user 1", "user 2", "user 3" };
//	}
//
//	// sample class / data
//	public class Window1
//	{
//		public int Height = 50;
//		public int Width  = 100;
//	}
}
