using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RythmPaperMapEditor.Enums;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class ItemCard : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ItemCard), new PropertyMetadata(null));

        public ItemType Type
        {
            get { return (ItemType)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register("Type", typeof(ItemType), typeof(ItemCard),
                new PropertyMetadata(SetIcon));

        public ItemCard()
        {
            InitializeComponent();
        }

        private static void SetIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemCard)d).Icon.Source = (BitmapImage) Application.Current.Resources[e.NewValue.ToString()];
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Text", Text);
                data.SetData("Type", Type);
                data.SetData("Object", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }

            e.Handled = true;
        }
    }
}