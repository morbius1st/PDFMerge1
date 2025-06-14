using System.Windows.Documents;
using System.Windows.Forms.ComponentModel.Com2Interop;
using UtilityLibrary;


// user name: jeffs
// created:   5/4/2025 11:38:46 PM


/*switchboard descriptors*/

/* this is a template
public static List<string> Menu00 = new ()
{
	"<section>", "<paragraph foreground='red' padding='0'>",


	"</paragraph>","</section>"
};
*/


namespace _TemplateProject.Menus
{
	// this is a "complete" (but example only) menu system

	// this is the menu manager for this menu system
	public class SbPdfPrimeManager : AMenuManager
	{
		public const string MENU_NAME = "PdfPrimeMgr";

		private static SbPdfPrimeManager instance;

		static SbPdfPrimeManager()
		{
			if (instance == null) instance = new SbPdfPrimeManager();
		}

		// hold / organize the various menus
		private SbPdfPrimeManager()
		{
			configMenus();
		}

		public static SbPdfPrimeManager Instance => instance;

		private void configMenus()
		{
			Menus = new Dictionary<string, AMenu>();

			// add the menu to the dictionary + convert the descriptor
			// init the jump list
			AddMenu(Menus, new SbPdfPrime());
			AddMenu(Menus, new SbPdfSelect());
			AddMenu(Menus, new SbPdfRead());
			AddMenu(Menus, new SbPdfScan());
			AddMenu(Menus, new SbPdfBtnTest());
		}

		// this is the primary menu - this includes
		// menu options that directly run functions and
		// menu options that lead to other menus
		internal class SbPdfPrime : AMenu
		{
			public const string MENU_NAME = "PdfPrime";  // const is used throughout

			public SbPdfPrime()
			{
				Name = MENU_NAME;     // menu name
				MaxOptionChars = 2;   // max number of characters for menu options
				IgnoreCase = true;    // require correct case for menu option entry
				MenuOptions = new (); // list if valid menu options
				UIeOptions = new ();  // list of valid UIe options
			}

			/* properties */

			public override Block Menu { get; internal set; } = null;

		#region menu descriptors

			public override List<string> MenuDescriptors => new ()
			{
				"<section>",
				"<paragraph foreground='red' padding='0'><indent spaces='4'/>",
				"<background color='60, 60, 60'>╭───── • <fontsize fontsize='18'><gray>Primary</gray></fontsize> • <repeat text='─' tocolumn='54'/>╮</background><linebreak/>",
				"<linebreak/>",
				"╭───── • <gray>Initialize</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='I1'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Initialize 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='I2'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Initialize 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='I3'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Initialize 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='I4'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Initialize 1</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Reset</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='R1'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Reset Full 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='R2'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Reset Data Manager 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='R3'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Reset the sheet file 1</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Data to Process</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				$"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SD' result='{SbPdfSelect.MENU_NAME}'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Select Data</foreground>  ❯ ❯❯ <dimgray>[to select a data set]</dimgray><linebreak/>",
				$"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='RD' result='{SbPdfRead.MENU_NAME}'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Read Data File</foreground>  ❯ ❯❯ <dimgray>[to read data file]</dimgray><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Operations</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				$"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SS' result='{SbPdfScan.MENU_NAME}'/></foreground></bold>  │  ▪ <foreground color='lawngreen'>Scan Menu</foreground>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				// buttons just to validate the concept

				"╭───── • <gray>Buttons</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				"<fontsize fontsize='16'>│ ",
				$"<button result='{SbPdfBtnTest.MENU_NAME}' name='Alpha' content='Alpha' height='18' width='40' horizontalcontentalignment='Left' fontsize='10' margin='0' padding='0' verticalalignment='Bottom'/>",
				" │</fontsize>  ▪ <foreground color='lawngreen'>Press Me 1</foreground>  ❯ ❯❯<linebreak/>",
				"<fontsize fontsize='16'>│ ",
				"<button name='Beta' content='Beta' height='18' width='40' horizontalcontentalignment='Center' fontsize='10' margin='0'  padding='0' verticalalignment='Bottom'/>",
				" │</fontsize>  ▪ <foreground color='lawngreen'>Press Me 2</foreground>  ❯ ❯❯<linebreak/>",
				"<fontsize fontsize='16'>│ ",
				"<button name='Delta' content='Delta' height='18' width='40' horizontalcontentalignment='Right' fontsize='10' margin='0'  padding='0' verticalalignment='Bottom'/>",
				" │</fontsize>  ▪ <foreground color='lawngreen'>Press Me 3</foreground>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",


				"╭───── • <gray>Completion Options</gray> • <repeat text='─' tocolumn='55'/>╮<linebreak/>",
				$"│  <bold><magenta><menuitem menuitemoption='X' result='{AMenu.MENU_EXIT_NAME}'/></magenta></bold>   │  ▪ <magenta>Exit</magenta>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				"<indent/></paragraph>",
				"</section>"
			};

		#endregion
		}

		public class SbPdfRead : AMenu
		{
			public const string MENU_NAME = "PdfRead";

			public SbPdfRead()
			{
				Name         = MENU_NAME;
				MaxOptionChars = 3;
				MenuOptions = new ();
				UIeOptions = new ();
			}

			public override Block Menu { get; internal set; } = null;

		#region menu descriptors

			public override List<string> MenuDescriptors => new ()
			{
				"<section>",
				"<paragraph foreground='red' padding='0'>",
				"╭───── • <gray>Read Data</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='RD1'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Read Data 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='RD2'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Read Data 2</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Completion Options</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				$"│  <bold><magenta><menuitem menuitemoption='X' result='{AMenu.MENU_EXIT_NAME}'/></magenta></bold>   │  ▪ <magenta>Exit</magenta>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				"</paragraph>",
				"</section>"
			};

		#endregion
		}

		public class SbPdfSelect : AMenu
		{
			public const string MENU_NAME = "PdfSelect";

			public SbPdfSelect()
			{
				Name           = MENU_NAME;
				MaxOptionChars = 3;
				MenuOptions = new ();
				UIeOptions = new ();
			}

			public override Block Menu { get; internal set; } = null;

		#region menu descriptors

			public override List<string> MenuDescriptors => new ()
			{
				"<section>",
				"<paragraph foreground='red' padding='0'>",
				"╭───── • <gray>Select Data</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SE1'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Select Data 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SE2'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Select Data 2</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SE3'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Select Data 3</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Completion Options</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				$"│  <bold><magenta><menuitem menuitemoption='X' result='{AMenu.MENU_EXIT_NAME}'/></magenta></bold>   │  ▪ <magenta>Exit</magenta>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				"</paragraph>",
				"</section>"
			};

		#endregion
		}

		public class SbPdfScan : AMenu
		{
			public const string MENU_NAME = "PdfScan";

			public SbPdfScan()
			{
				Name         = MENU_NAME;
				MaxOptionChars = 3;
				MenuOptions = new ();
				UIeOptions = new ();
			}

			public override Block Menu { get; internal set; } = null;

		#region menu descriptors

			public override List<string> MenuDescriptors => new ()
			{
				"<section>",
				"<paragraph foreground='red' padding='0'>",
				"╭───── • <gray>Scan Data</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SC1'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Scan PDF 1</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='SC2'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Scan PDF 2</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Completion Options</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				$"│  <bold><magenta><menuitem menuitemoption='X' result='{AMenu.MENU_EXIT_NAME}'/></magenta></bold>   │  ▪ <magenta>Exit</magenta>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				"</paragraph>",
				"</section>"
			};

		#endregion
		}

		public class SbPdfBtnTest : AMenu
		{
			public const string MENU_NAME = "PdfBtnTest";

			public SbPdfBtnTest()
			{
				Name         = MENU_NAME;
				MaxOptionChars = 1;
				MenuOptions = new ();
				UIeOptions = new ();
			}

			public override Block Menu { get; internal set; } = null;

		#region menu descriptors

			public override List<string> MenuDescriptors => new ()
			{
				"<section>",
				"<paragraph foreground='red' padding='0'>",
				"╭───── • <gray>Scan Data</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='A'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Option A</foreground><linebreak/>",
				"│  <bold><foreground color='lawngreen'><menuitem menuitemoption='B'/></foreground></bold>   │  ▪ <foreground color='lawngreen'>Option B</foreground><linebreak/>",
				"<linebreak/>",

				"╭───── • <gray>Completion Options</gray> • <repeat text='─' tocolumn='45'/>╮<linebreak/>",
				$"│  <bold><magenta><menuitem menuitemoption='X' result='{AMenu.MENU_EXIT_NAME}'/></magenta></bold>   │  ▪ <magenta>Exit</magenta>  ❯ ❯❯<linebreak/>",
				"<linebreak/>",

				"</paragraph>",
				"</section>"
			};

		#endregion
		}
	}
}