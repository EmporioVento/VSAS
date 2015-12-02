using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using labs.Graph;

namespace labs
{
    /// <summary>
    /// Interaction logic for graphNode.xaml
    /// </summary>
    public partial class graphNode : UserControl
    {
        public graphNode(node_view parentView)
        {
            InitializeComponent();
            this.parentView = parentView;
            Canvas.SetZIndex(this, 2);
            this.point.Width = this.point.Height = NodeRadius;
            opaqueBrush = new SolidColorBrush(Colors.Black);
            opaqueBrush.Opacity = 0;
            Canvas.SetZIndex(txtWeight_node, 2);
            txtWeight_node.Background = opaqueBrush;
        }

        private static int NodeRadius = 25;
        private node_view parentView;
        private Brush opaqueBrush;

        public static int Radius
        {
            get { return NodeRadius; }
        }

        private void node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parentView.Move = false;
        }
        
        private void node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //isMove = true;
            if (parentView.Graph.IsEdgeAdd)
            {
                if (parentView.Graph.FirstTop == null)
                    parentView.Graph.FirstTop = parentView;
                else
                {
                    parentView.Graph.AddEdge(parentView.Graph.FirstTop, parentView);
                    parentView.Graph.EndAddEdge();
                }
            }
            else if (parentView.Graph.IsNodeDelete)
            {
                parentView.Graph.DeleteNode(parentView);
                parentView.Graph.EndDeleteNode();
            }
            else
                parentView.Move = true;
        }
        private void node_MouseEnter(object sender, MouseEventArgs e)
        {
            txtId_node.IsReadOnly = true;
            txtWeight_node.IsReadOnly = false;
        }
        private void node_MouseLeave(object sender, MouseEventArgs e)
        {
            parentView.OnMouseLeave();
        }
        private void node_MouseMove(object sender, MouseEventArgs e)
        {
            parentView.OnMouseMove();
        }

        private void node_MouseRightButtonUp(object sender, RoutedEventArgs e)
        {
            txtId_node.Visibility = Visibility.Visible;
            txtId_node.Text = parentView.txt_id = lblId_node.Content.ToString();
            lblId_node.Visibility = Visibility.Hidden;
            txtId_node.IsReadOnly = false;
            txtId_node.CaretBrush = Brushes.Black;
            txtId_node.Background = Brushes.White;
            txtId_node.Foreground = System.Windows.Media.Brushes.Black;
        }       

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lblId_node.Content = parentView.txt_id = txtId_node.Text;
                parentView.id = int.Parse(txtId_node.Text);
                parentView.Validate();
                txtId_node.IsReadOnly = true;
                txtId_node.Visibility = Visibility.Hidden;
                lblId_node.Visibility = Visibility.Visible;
                parentView.Graph.ValidateTopNumbers();
            }
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                parentView.txt_weight = txtWeight_node.Text;
                parentView.weight = int.Parse(txtWeight_node.Text);
                txtWeight_node.IsReadOnly = true;
                txtWeight_node.CaretBrush = opaqueBrush;
                txtWeight_node.Background = opaqueBrush;                
                Canvas.SetZIndex(this, 2);
            }
        }
        private void TxtBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            parentView.txt_weight = txtWeight_node.Text;
            txtWeight_node.IsReadOnly = false;
            txtWeight_node.CaretBrush = Brushes.Black;
            txtWeight_node.Background = Brushes.White;
            Canvas.SetZIndex(this, 4);
        }
    }
}