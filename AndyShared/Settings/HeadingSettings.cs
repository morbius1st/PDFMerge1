using System;
using System.Runtime.Serialization;
using UtilityLibrary;

namespace SettingsManager
{
	public partial class Heading
	{
		[DataMember(Order = 4)] public string Notes              = "created by v7.0";
		[DataMember(Order = 5)] public string DataClassVersion   = "";
		[DataMember(Order = 6)] public string Description        = "";
	}
}
