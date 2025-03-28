﻿#region using
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

#endregion

// username: jeffs
// created:  6/7/2020 6:19:51 AM

namespace AndyResources.XamlResources
{
	public class TextBoxBehavior
	{
		public static bool GetSelectAllTextOnFocus(TextBox textBox)
		{
			return (bool)textBox.GetValue(SelectAllTextOnFocusProperty);
		}

		public static void SetSelectAllTextOnFocus(TextBox textBox, bool value)
		{
			textBox.SetValue(SelectAllTextOnFocusProperty, value);
		}

		public static readonly DependencyProperty SelectAllTextOnFocusProperty =
			DependencyProperty.RegisterAttached(
				"SelectAllTextOnFocus",
				typeof(bool),
				typeof(TextBoxBehavior),
				new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

		private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = d as TextBox;
			if (textBox == null) return;

			if (e.NewValue is bool == false) return;

			if ((bool)e.NewValue)
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
			"UpdatePropertySourceWhenEnterPressed", typeof(DependencyProperty), typeof(InputBindingsManager), new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));

		static InputBindingsManager()
		{

		}

		public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
		{


			dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);

		}

		public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed(DependencyObject dp)
		{

			if (dp is TextBox) ((TextBox)dp).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

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
