using AlbionMarketPriceDiff.Util;
using AlbionMarketPriceDiff.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlbionMarketPriceDiff
{
    public partial class MainWindow : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); // not allowed text

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModelBuilder.Build();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
        
        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!textBox.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    textBox.Focus();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            (DataContext as MainViewModel)?.Dispose();
            base.OnClosing(e);
        }
    }
}
