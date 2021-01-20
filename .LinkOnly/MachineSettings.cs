using System.Runtime.Serialization;

// Solution:     SettingsManager
// Project:       SettingsManagerV70
// File:             MachineSettings.cs
// Created:      -- ()

namespace SettingsManager
{
	#region info class

	[DataContract(Name = "MachSettings", Namespace = "")]
	internal class MachSettingInfo<T> : MachSettingInfoBase<T>
		where T : new()
	{
		public MachSettingInfo()
		{
			DataClassVersion = "7.0m";
			Description = "machine setting file for SettingsManagerV70";
		}

#pragma warning disable CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
		internal override void UpgradeFromPrior(SettingInfoBase<T> prior) { }
#pragma warning restore CS0436 // The type 'SettingInfoBase<TData>' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs' conflicts with the imported type 'SettingInfoBase<TData>' in 'WpfShared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\SettingManager\v7.2\SettingsMgr.cs'.
	}

	#endregion

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	internal class MachSettingData
	{
		[DataMember(Order = 1)]
		public string LastClassificationFileId { get; set; } = "PdfSample 1";
	}

	#endregion
}