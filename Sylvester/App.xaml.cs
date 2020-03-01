using System.Windows;

namespace Sylvester
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static string Icon_FolderProject01 { get; } = "folder01";
		public static string Icon_FolderProject02 { get; } = "folder02";
		public static string Icon_FolderProject03 { get; } = "folder03";
		public static string Icon_FolderProject04 { get; } = "folder04";

		public static string Icon_FolderPair01 { get; } = "pair01";
		public static string Icon_FolderPair02 { get; } = "pair02";
		public static string Icon_FolderPair03 { get; } = "pair03";
		public static string Icon_FolderPair04 { get; } = "pair04";
		public static string Icon_FolderPair05 { get; } = "pair05";

		public static string[] Icon_FolderProjects { get; }  = new []
		{
			Icon_FolderProject01,
			Icon_FolderProject02,
			Icon_FolderProject03,
			Icon_FolderProject04
		};

		public static string[] Icon_FolderPairs { get; }  = new []
		{
			Icon_FolderPair01,
			Icon_FolderPair02,
			Icon_FolderPair03,
			Icon_FolderPair04,
			Icon_FolderPair05
		};
	}

	public class Info
	{
		
	}
}
