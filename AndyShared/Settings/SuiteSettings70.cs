﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AndyShared.ConfigSupport;


// projname: SettingsManagerV40
// itemname: AppSettingInfo70
// username: jeffs
// Created:      -- ()

namespace SettingsManager
{
#region info class

	[DataContract(Name = "SuiteSettingInfoInfo")]
	public class SuiteSettingInfo70<T> : SuiteSettingInfoBase<T>
		where T : new ()
	{
		[DataMember]
		public override string DataClassVersion => "0.7.su";

		public override string Description => "Suite setting file for Andy";
		public override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
	}

#endregion

#region suite data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract]
	public class SuiteSettingData70
	{
		[DataMember(Order = 1)]
		public string SiteRootPath { get; set; }

//			= @"D:\Users\Jeff\OneDrive\Prior Folders\Office Stuff\CAD\Copy Y Drive & Office Standards\AppData" ;

		// [DataMember(Order = 1)]
		// public List<ConfigFileSetting> LocalSeedFiles { get; set; }

//			= new List<ConfigFileSetting>()
//			{
//				new ConfigFileSetting("UnNamed", "No User", 
//					@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample", 
//					@"SheetCategories.xml")
//			};
	}

#endregion
}