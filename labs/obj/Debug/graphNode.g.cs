﻿#pragma checksum "..\..\graphNode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "82AB4C9D052D08506E4B7C26FCAA3559"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace labs {
    
    
    /// <summary>
    /// graphNode
    /// </summary>
    public partial class graphNode : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\graphNode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal labs.graphNode node;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\graphNode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtId_node;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\graphNode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblId_node;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\graphNode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse point;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\graphNode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtWeight_node;
        
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
            System.Uri resourceLocater = new System.Uri("/labs;component/graphnode.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\graphNode.xaml"
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
            this.node = ((labs.graphNode)(target));
            
            #line 7 "..\..\graphNode.xaml"
            this.node.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.node_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 7 "..\..\graphNode.xaml"
            this.node.MouseEnter += new System.Windows.Input.MouseEventHandler(this.node_MouseEnter);
            
            #line default
            #line hidden
            
            #line 7 "..\..\graphNode.xaml"
            this.node.MouseLeave += new System.Windows.Input.MouseEventHandler(this.node_MouseLeave);
            
            #line default
            #line hidden
            
            #line 7 "..\..\graphNode.xaml"
            this.node.MouseMove += new System.Windows.Input.MouseEventHandler(this.node_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtId_node = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\graphNode.xaml"
            this.txtId_node.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtId_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lblId_node = ((System.Windows.Controls.Label)(target));
            
            #line 14 "..\..\graphNode.xaml"
            this.lblId_node.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.node_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 14 "..\..\graphNode.xaml"
            this.lblId_node.MouseEnter += new System.Windows.Input.MouseEventHandler(this.node_MouseEnter);
            
            #line default
            #line hidden
            
            #line 15 "..\..\graphNode.xaml"
            this.lblId_node.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.node_MouseRightButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.point = ((System.Windows.Shapes.Ellipse)(target));
            
            #line 17 "..\..\graphNode.xaml"
            this.point.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.node_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 17 "..\..\graphNode.xaml"
            this.point.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.node_MouseRightButtonUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtWeight_node = ((System.Windows.Controls.TextBox)(target));
            
            #line 20 "..\..\graphNode.xaml"
            this.txtWeight_node.KeyDown += new System.Windows.Input.KeyEventHandler(this.TxtBox_KeyDown);
            
            #line default
            #line hidden
            
            #line 21 "..\..\graphNode.xaml"
            this.txtWeight_node.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TxtBox_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

