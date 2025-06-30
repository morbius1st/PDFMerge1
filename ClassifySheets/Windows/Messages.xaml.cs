using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using JetBrains.Annotations;
using UtilityLibrary;

namespace ClassifySheets.Windows
{
	/// <summary>
	/// Interaction logic for Messages.xaml
	/// </summary>
	public partial class Messages : Window, INotifyPropertyChanged, IWin
	{
		private string message;

		public Messages()
		{
			InitializeComponent();

			init();
		}

		private void init()
		{
			DM.init(5);

			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DBG_TBX);
			
			DM.DbxSetIdx(1, 0);
			DM.DbxSetDefaultWhere(1, ShowWhere.DBG_TBX);

			startMsg();
		}

		private void startMsg()
		{
			DM.DbxMsg    (1, "\n");
			DM.DbxMsgLine(1, "*".Repeat(50));
			DM.DbxMsgLine(1, $"***  {nameof(MainWinClassifySheets),-42}***");
			DM.DbxMsgLine(1, $"***  {DateTime.Now,-42}***");
			DM.DbxMsgLine(1, "*".Repeat(50));
			DM.DbxMsg    (1, "\n");
		}


		public string Message
		{
			get => message;
			set
			{
				if (value == message) return;
				message = value;
				OnPropertyChanged();
			}
		}

		public void DebugMsgLine(string msg)
		{
			Message += msg + "\n";
		}

		public void DebugMsg(string msg)
		{
			Message += msg;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}
