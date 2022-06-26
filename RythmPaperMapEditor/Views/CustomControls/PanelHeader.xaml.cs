using System.Windows;
using System.Windows.Controls;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class PanelHeader : UserControl
    {
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(PanelHeader),
                new PropertyMetadata(null));
        
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PanelHeader), new PropertyMetadata(null));

        public PanelHeader()
        {
            InitializeComponent();
        }
    }
}