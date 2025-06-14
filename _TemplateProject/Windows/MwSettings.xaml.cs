using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityLibrary;
using _TemplateProject.Windows.Support;

using static _TemplateProject.Windows.Support.MwSupport;

namespace _TemplateProject.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MwSettings : Window
	{
	#region fields

		private MwProcessManager pm;

	#endregion

	#region ctor

		public MwSettings()
		{
			InitializeComponent();

			init();
		}

		private void init()
		{
			DM.init(3);
			DM.DbxSetIdx(0, 0);
			DM.DbxSetDefaultWhere(0, ShowWhere.DEBUG);
			
			startMsg();

			DM.Start0();

			CsFlowDocManager.Instance.AddDescTextTb("<green>MainWindow initialized</green>");

			pm = new MwProcessManager();

			DM.End0();
		}

		private void startMsg()
		{
			string startInfo;

			Debug.WriteLine("*".Repeat(STARTMSGWIDTH));
			Debug.WriteLine($"{"*** start AndyLib ",-STARTTTLWIDTH}***");

			startInfo = $"*** {DateTime.Now.ToString()}";

			Debug.WriteLine($"{startInfo}{" ".Repeat(STARTMSGWIDTH - startInfo.Length - 4)} ***");
			Debug.WriteLine("*".Repeat(STARTMSGWIDTH));
		}

	#endregion

	#region public properties

		public string Messages { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(MainWindow)}";
		}

	#endregion

		private void Test1_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}