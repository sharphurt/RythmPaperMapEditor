using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RythmPaperMapEditor.Enums;
using RythmPaperMapEditor.Models;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class TrackGridElementHolder : Button
    {
        public event Action<Note> NoteAdded;
        
        public event Action<int> NoteRemoved;

        private int _index;

        private bool _containsNote;
        
        public TrackGridElementHolder(int index)
        {
            InitializeComponent();
            _index = index;
        }

        private void TrackGridElementHolder_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                if ((e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                    SetState((NoteType) e.Data.GetData("NoteType"));
                    e.Handled = true;
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
                var _element = (ItemCard) e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    SetState((NoteType) e.Data.GetData("NoteType"));
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void SetState(NoteType type)
        {
            if (_containsNote && type == NoteType.None)
            {
                NoteRemoved?.Invoke(_index);
                _containsNote = false;
            }
            else if (!_containsNote)
            {
                var note = new Note(type, NoteParameters.Random(), _index);
                NoteAdded?.Invoke(note);
                _containsNote = true;
            }
            
            SetVisualState(type);
        }

        private void SetVisualState(NoteType type)
        {
            if (type == NoteType.None)
            {
                EmptyIcon.Visibility = Visibility.Visible;
                ItemIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyIcon.Visibility = Visibility.Collapsed;
                ItemIcon.Visibility = Visibility.Visible;

                var icon = (BitmapImage)Application.Current.Resources[Data.Data.NoteIcons[type]];
                
                ItemIcon.Source = icon;
            }
        }

        private void TrackGridElementHolder_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetState(NoteType.None);
        }
    }
}