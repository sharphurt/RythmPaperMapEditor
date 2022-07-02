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

        public TrackGridElementHolder(int index, Note note)
        {
            InitializeComponent();

            if (note.Time != index)
            {
                MessageBox.Show($"Error! Note loading error. Problems with note {note.Time}");
                return;
            }

            _containsNote = true;
            _index = note.Time;
            SetState(note);
        }

        private void TrackGridElementHolder_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                if ((e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;

                    var note = new Note((NoteType)e.Data.GetData("NoteType"), NoteParameters.Random(), _index);
                    SetState(note);

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
                var _element = (ItemCard)e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    var note = new Note((NoteType)e.Data.GetData("NoteType"), NoteParameters.Random(), _index);

                    SetState(note);
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void SetState(Note note)
        {
            if (_containsNote && note.Type == NoteType.None)
            {
                NoteRemoved?.Invoke(_index);
                _containsNote = false;
            }
            else if (!_containsNote)
            {
                NoteAdded?.Invoke(note);
                _containsNote = true;
            }

            SetVisualState(note.Type);
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
            SetState(new Note(NoteType.None, NoteParameters.Random(), 0));
        }
    }
}