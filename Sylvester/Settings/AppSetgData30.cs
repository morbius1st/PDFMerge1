#region + Using Directives

using System.Runtime.Serialization;

#endregion


// projname: Sylvester.Settings
// itemname: AppSetgData30
// username: jeffs
// created:  3/15/2020 5:02:45 PM


namespace Sylvester.Settings
{

	public partial class AppSettingData30
	{
		// contains the strings for the saved folder
		// dialog box operations
		// "(dialog box operation)", "message" 
		[DataMember]
		public static string[,] SavedFolderOperationDesc =
			{
				{" (Administration)", ""},
				{" (Selecting a Current Folder)", "Select for the Current Folder"},
				{" (Selecting a Revision Folder)", "Select for the Revision Folder"}
			};


	}
}
