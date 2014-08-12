using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yujt.ToolBox.Common.Behaviors
{
    public class TextBoxBehavior
    {
        public static DependencyProperty OnLostFocusProperty 
            = DependencyProperty.RegisterAttached("OnLostFocus",
                                                  typeof(ICommand),
                                                  typeof(TextBoxBehavior),
                                                  new UIPropertyMetadata(OnLostFocus));
 
        public static void SetOnLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(OnLostFocusProperty, value);
        }
 
        /// <summary>
        /// PropertyChanged callback for OnDoubleClickProperty.  Hooks up an event handler with the 
        /// actual target.
        /// </summary>
        private static void OnLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TextBox;
            if (element == null)
            {
                throw new InvalidOperationException("This behavior can be attached to a TextBox item only.");
            }
 
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                element.LostFocus += OnPreviewLostFocus;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                element.LostFocus -= OnPreviewLostFocus;
            }
        }
 
        private static void OnPreviewLostFocus(object sender, RoutedEventArgs e)
        {
            var element = (UIElement)sender;
            var command = (ICommand)element.GetValue(OnLostFocusProperty);
            if (command != null)
            {
                command.Execute(e);
            }
        }
    }
}
