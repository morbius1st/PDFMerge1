using System;
using System.Runtime.Serialization;
using UtilityLibrary;

namespace SettingsManager
{
	[DataContract(Namespace =  N_SPACE)]
	public class Heading
	{
		public static string ClassVersionName = nameof(DataClassVersion);
		public static string SystemVersionName = nameof(SystemVersion);
		public static string SuiteName = "Andy";

		public const string N_SPACE = "";

		public Heading(string dataClassVersion) => DataClassVersion = dataClassVersion;

		[DataMember(Order = 1)] public string SaveDateTime       = DateTime.Now.ToString("yyyy-MM-dd - HH:mm zzz");
#pragma warning disable CS0436 // The type 'CsUtilities' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilities.cs' conflicts with the imported type 'CsUtilities' in 'StoreAndRead, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilities.cs'.
		[DataMember(Order = 2)] public string AssemblyVersion    = CsUtilities.AssemblyVersion;
#pragma warning restore CS0436 // The type 'CsUtilities' in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilities.cs' conflicts with the imported type 'CsUtilities' in 'StoreAndRead, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'D:\Users\Jeff\Documents\Programming\VisualStudioProjects\UtilityLibrary\UtilityLibrary\CsUtilities.cs'.
		[DataMember(Order = 3)] public string SystemVersion      = "7.0";
		[DataMember(Order = 4)] public string DataClassVersion;
		[DataMember(Order = 5)] public string Notes              = "created by v7.0";
		[DataMember(Order = 6)] public string Description;
	}
}
