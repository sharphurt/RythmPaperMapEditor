using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class FloatNumericTextBox : TextBox
    {
        private readonly Regex _regex = new Regex("[^0-9.]+");

        public FloatNumericTextBox()
        {
            InitializeComponent();
        }

        private void FloatNumericTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse((sender as FloatNumericTextBox).Text + e.Text, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var _);
        }
    }
}