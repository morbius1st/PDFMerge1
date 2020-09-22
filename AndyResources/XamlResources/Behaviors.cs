#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

#endregion

// username: jeffs
// created:  6/7/2020 6:19:51 AM

namespace AndySharedResources.XamlResources
{
	public class TextBoxBehavior
	{
		public static bool GetSelectAllTextOnFocus(TextBox textBox)
		{
			return (bool) textBox.GetValue(SelectAllTextOnFocusProperty);
		}

		public static void SetSelectAllTextOnFocus(TextBox textBox, bool value)
		{
			textBox.SetValue(SelectAllTextOnFocusProperty, value);
		}

		public static readonly DependencyProperty SelectAllTextOnFocusProperty =
			DependencyProperty.RegisterAttached(
				"SelectAllTextOnFocus",
				typeof (bool),
#pragma warning disable CS0436 // The type 'TextBoxBehavior' in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs' conflicts with the imported type 'TextBoxBehavior' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs'.
				typeof (TextBoxBehavior),
#pragma warning restore CS0436 // The type 'TextBoxBehavior' in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs' conflicts with the imported type 'TextBoxBehavior' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs'.
				new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

		private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = d as TextBox;
			if (textBox == null) return;

			if (e.NewValue is bool == false) return;

			if ((bool) e.NewValue)
			{
				textBox.GotFocus += SelectAll;
				textBox.PreviewMouseDown += IgnoreMouseButton;
			}
			else
			{
				textBox.GotFocus -= SelectAll;
				textBox.PreviewMouseDown -= IgnoreMouseButton;
			}
		}

		private static void SelectAll(object sender, RoutedEventArgs e)
		{
			var textBox = e.OriginalSource as TextBox;
			if (textBox == null) return;
			textBox.SelectAll();
		}

		private static void IgnoreMouseButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox == null || (!textBox.IsReadOnly && textBox.IsKeyboardFocusWithin)) return;

			e.Handled = true;
			textBox.Focus();
		}
	}

	public static class InputBindingsManager
{

    public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = DependencyProperty.RegisterAttached(
#pragma warning disable CS0436 // The type 'InputBindingsManager' in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs' conflicts with the imported type 'InputBindingsManager' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs'.
            "UpdatePropertySourceWhenEnterPressed", typeof(DependencyProperty), typeof(InputBindingsManager), new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));
#pragma warning restore CS0436 // The type 'InputBindingsManager' in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs' conflicts with the imported type 'InputBindingsManager' in 'WpfShared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in 'B:\Programming\VisualStudioProjects\PDFMerge1\ClassifierEditor\..\AndyResources\XamlResources\Behaviors.cs'.

    static InputBindingsManager()
    {

    }

    public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
    {
        dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);
    }

    public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed(DependencyObject dp)
    {
        return (DependencyProperty)dp.GetValue(UpdatePropertySourceWhenEnterPressedProperty);
    }

    private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        UIElement element = dp as UIElement;

        if (element == null)
        {
            return;
        }

        if (e.OldValue != null)
        {
            element.PreviewKeyDown -= HandlePreviewKeyDown;
        }

        if (e.NewValue != null)
        {
            element.PreviewKeyDown += new KeyEventHandler(HandlePreviewKeyDown);
        }
    }

    static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            DoUpdateSource(e.Source);
        }
    }

    static void DoUpdateSource(object source)
    {
        DependencyProperty property =
            GetUpdatePropertySourceWhenEnterPressed(source as DependencyObject);

        if (property == null)
        {
            return;
        }

        UIElement elt = source as UIElement;

        if (elt == null)
        {
            return;
        }

        BindingExpression binding = BindingOperations.GetBindingExpression(elt, property);

        if (binding != null)
        {
            binding.UpdateSource();
        }
    }
}


}
