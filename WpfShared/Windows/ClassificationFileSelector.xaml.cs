using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using AndyShared.ConfigMgrShared;
using AndyShared.ConfigSupport;

namespace WpfShared.Windows
{
	/// <summary>
	/// Interaction logic for ClassificationFileSelector.xaml
	/// </summary>
	public partial class ClassificationFileSelector : Window, INotifyPropertyChanged
	{
	#region private fields

		private ConfigClassificationFiles cfgClsFiles = null;

		private ConfigFileClassificationUser selected;

		private ICollectionView view;

	#endregion

	#region ctor

		public ClassificationFileSelector()
		{
			InitializeComponent();

			initialize();
		}

	#endregion

	#region public properties

		public ConfigClassificationFiles CfgClsFiles => cfgClsFiles;

		public ICollectionView View => view;

		public ConfigFileClassificationUser Selected
		{
			get => selected;
			set
			{
				selected = value;

				OnPropertyChange();
			}
		}

		public string UserName => Environment.UserName;

		public string ListViewTitle => UserName + "'s Classification Files";

	#endregion

	#region private properties



	#endregion

	#region public methods



	#endregion

	#region private methods

		private void initialize()
		{
			cfgClsFiles = ConfigClassificationFiles.Instance;

			cfgClsFiles.Initialize();

			initializeView();
		}

		private void initializeView()
		{
			view = CollectionViewSource.GetDefaultView(cfgClsFiles.UserClassificationFiles);

			view.Filter = new Predicate<object>(MatchUser);

			OnPropertyChange("View");

		}

		private bool MatchUser(object usr)
		{
			ConfigFileClassificationUser user = usr as ConfigFileClassificationUser;

			return user.UserName.Equals(UserName);


		}

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
			return "this is ClassificationFileSelector";
		}

	#endregion

	}
}
