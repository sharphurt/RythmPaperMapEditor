﻿#pragma checksum "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A3F6EB387A79018D2FEF672FD87C89EF1B891E1299356EE89966239324AA1B08"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using RythmPaperMapEditor.Views.CustomControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RythmPaperMapEditor.Views.CustomControls {
    
    
    /// <summary>
    /// TrackGridElementHolder
    /// </summary>
    public partial class TrackGridElementHolder : System.Windows.Controls.Button, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle EmptyIcon;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ItemIcon;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RythmPaperMapEditor;component/views/customcontrols/trackgridelementholder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
            ((RythmPaperMapEditor.Views.CustomControls.TrackGridElementHolder)(target)).Drop += new System.Windows.DragEventHandler(this.TrackGridElementHolder_OnDrop);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
            ((RythmPaperMapEditor.Views.CustomControls.TrackGridElementHolder)(target)).DragEnter += new System.Windows.DragEventHandler(this.TrackGridElementHolder_OnDragOver);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
            ((RythmPaperMapEditor.Views.CustomControls.TrackGridElementHolder)(target)).DragOver += new System.Windows.DragEventHandler(this.TrackGridElementHolder_OnDragOver);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\Views\CustomControls\TrackGridElementHolder.xaml"
            ((RythmPaperMapEditor.Views.CustomControls.TrackGridElementHolder)(target)).MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.TrackGridElementHolder_OnMouseRightButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.EmptyIcon = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.ItemIcon = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

