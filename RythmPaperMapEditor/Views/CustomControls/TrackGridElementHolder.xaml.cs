using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RythmPaperMapEditor.Enums;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class TrackGridElementHolder : Button
    {
        public TrackGridElementHolder()
        {
            InitializeComponent();
        }

        private void TrackGridElementHolder_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void TrackGridElementHolder_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                var _panel = (TrackGridElementHolder)sender;
                var _element = (ItemCard)e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    SetState(_element.Type);
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void SetState(ItemType item)
        {
            if (item == ItemType.None)
            {
                EmptyIcon.Visibility = Visibility.Visible;
                ItemIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyIcon.Visibility = Visibility.Collapsed;
                ItemIcon.Visibility = Visibility.Visible;
                ItemIcon.Source = (BitmapImage)Application.Current.Resources[item.ToString()];
            }
        }

        private void TrackGridElementHolder_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetState(ItemType.None);
        }
    }
}