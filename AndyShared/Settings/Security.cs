#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SettingsManager;
using UtilityLibrary;

#endregion


// projname: $projectname$
// itemname: Security
// username: jeffs
// created:  10/20/2024 3:02:04 PM

namespace AndyShared.Settings
{
	public class Security : INotifyPropertyChanged
	{
	#region private fields

		private static readonly Lazy<Security> instance =
			new Lazy<Security>(() => new Security());

		private bool isAdministrator;

	#endregion

	#region ctor

		private Security() { init();}

	#endregion

	#region public properties

		public static Security Instance => instance.Value;

		public string CurrentUser => CsUtilities.UserName;

		public bool UserIsAdministrator
		{
			get => isAdministrator;

			set
			{
				if (value == isAdministrator) return;
				isAdministrator = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void init()
		{
			SiteSettings.Admin.Read();

			foreach (string adminUser in SiteSettings.Data.AdminUsers)
			{
				if (adminUser.Equals(CurrentUser))
				{
					UserIsAdministrator = true;
					break;
				}
			}

			// todo remove - for testing only
			// UserIsAdministrator = false;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(Security)}";
		}

	#endregion
	}
}