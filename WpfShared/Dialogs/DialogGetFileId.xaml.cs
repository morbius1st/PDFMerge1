using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AndyShared.Support;
using UtilityLibrary;
using static UtilityLibrary.CsUtilities;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;


namespace WpfShared.Dialogs
{
	/// <summary>
	/// Interaction logic for DialogGetFileId.xaml
	/// </summary>
	public partial class DialogGetFileId : Window, INotifyPropertyChanged
	{
	#region private fields

		private string fileId;

		private string notes = "Disallowed characters:" + INVALID_FILE_NAME_STRING
			+ "     Minimum 3 characters";

		private bool isValid;

		private string caption;

	#endregion

	#region ctor

		public DialogGetFileId(string title, string currentValue = null)
		{
			InitializeComponent();

			IsValid = false;

			Caption = title;

			if (!currentValue.IsVoid()) FileId = currentValue;
		}

	#endregion

	#region public properties

		public string FileId
		{
			get => fileId;

			set
			{
				fileId = value;
				OnPropertyChange();
			}
		}


		public string Notes => notes;

		public bool IsValid
		{
			get => isValid;
			private set
			{
				if (isValid == value) return;

				isValid = value;

				OnPropertyChange();
			}
		}



		public string Caption
		{
			get => caption;
			private set
			{
				caption = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

		private void DialogGetFileId_OnClosing(object sender, CancelEventArgs e)
		{
			this.DialogResult = isValid;
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
			isValid = false;

			this.Close();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void TbxFileId_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				if (isValid)
				{
					BtnDone.Focus();
				}
				else
				{
					BtnCancel.Focus();
				}
			}
		}

		private void TbxFileId_OnError(object sender, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
			{
				IsValid = false;
			}

			// else
			// {
			// 	IsValid = true;
			// }
		}


		private void TbxFileId_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox tb = (TextBox) sender;

			using (tb.DeclareChangeBlock())
			{
				foreach (TextChange c in e.Changes)
				{
					if (c.AddedLength == 0) continue;

					// tb.Select(c.Offset, c.AddedLength);

					string selected = tb.Text.Substring(c.Offset, c.AddedLength);

					if (!CsUtilities.ValidateStringChars(selected, INVALID_FILE_NAME_CHARACTERS))
					{
						SetValidationError(tb);

						return;
					}

					// if (tb.SelectedText.Contains(' '))
					// {
					// 	tb.SelectedText = tb.SelectedText.Replace(' ', '_');
					// }
					//
					// tb.Select(c.Offset + c.AddedLength, 0);
				}
			}

			ClearValidationError(tb);

			if (tb.Text.Length > 2)
			{
				IsValid = true;
			}
			else
			{
				IsValid = false;
			}
		}

		private void SetValidationError(TextBox tbx)
		{
			ValidationError err = new ValidationError(new ValidCharacterValidRule(),
				tbx.GetBindingExpression(TextBox.TextProperty));

			err.ErrorContent = "the text box value is not valid";

			Validation.MarkInvalid(tbx.GetBindingExpression(TextBox.TextProperty), err);
		}

		private void ClearValidationError(TextBox tbx)
		{
			Validation.ClearInvalid(tbx.GetBindingExpression(TextBox.TextProperty));
		}

	#endregion

	#region event handeling

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is DialogGetFileId";
		}

	#endregion
	}

	public class ValidCharacterValidRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string text = (string) value;

			if (!CsUtilities.ValidateStringChars(text, INVALID_FILE_NAME_CHARACTERS))
			{
				return new ValidationResult(false, "File Id includes invalid characters");
			}

			return ValidationResult.ValidResult;
		}
	}
}