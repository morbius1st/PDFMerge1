#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using AndyConfig.FilesSupport;

#endregion

// username: jeffs
// created:  6/21/2020 3:18:01 PM

namespace AndyConfig.ConfigMgr
{
	public class ConfigSeedInstalled : INotifyPropertyChanged
	{
	#region private fields

		private const string SEED_PATTERN = @"*.seed.xml";

	#endregion

	#region ctor

		public ConfigSeedInstalled() { }

	#endregion

	#region public properties

		public bool Initialized { get; set; }

		public string InstallFolder =>
	#if DEBUG
			@"B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\.sample";
	#else
			Assembly.GetExecutingAssembly().Location + @"\Seed Files";
	#endif

		public string InstallSeedFileFolder => InstallFolder + @"\Seed Files";

		public bool InstalledFolderExists => InstalledSeedFiles?.FolderExists ?? false;

		public bool InstalledSeedFilesExist => InstalledSeedFilesCount > 0;

		public FolderAndFileSupport InstalledSeedFiles { get; private set; }

		public int InstalledSeedFilesCount => InstalledSeedFiles?.Count ?? 0;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			Initialized = true;

			InstalledSeedFiles =
				new FolderAndFileSupport(InstallSeedFileFolder, SEED_PATTERN);

			InstalledSeedFiles.GetFiles();

			OnPropertyChange("Initialized");
			OnPropertyChange("InstalledFolderExists");
			OnPropertyChange("InstalledSeedFilesExist");
			OnPropertyChange("InstalledSeedFiles");
			OnPropertyChange("InstalledSeedFilesCount");

		}

	#endregion

	#region private methods

	#endregion

	#region event processing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ConfigSeedInstalled";
		}

	#endregion
	}
}