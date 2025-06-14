
using SettingReaderClassifierEditor;


namespace SettingReaderMgr
{
	internal class Program
	{

		private static ClassifierEditorSettings? classifierEditor;

		static void Main(string[] args)
		{

			classifierEditor = new ClassifierEditorSettings();

			Console.WriteLine("Hello, World!");

			Console.WriteLine("Classifier Editor");
			Console.WriteLine($"LastClassificationFileId | {classifierEditor.LastClassificationFileId}");


			Console.Write("Waiting ... |  ");
			Console.ReadKey();

			
		}
	}
}
