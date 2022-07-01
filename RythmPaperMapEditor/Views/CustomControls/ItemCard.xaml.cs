using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RythmPaperMapEditor.Enums;
using RythmPaperMapEditor.Models;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class ItemCard : UserControl
    {
        public string Text { get; }

        public NoteType NoteType { get; }

        public ItemCard(NoteType noteType)
        {
            InitializeComponent();

            NoteType = noteType;
            Text = noteType.ToString();

            Icon.Source = (BitmapImage)Application.Current.Resources[Data.Data.NoteIcons[noteType]];
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Text", Text);
                data.SetData("NoteType", NoteType);
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