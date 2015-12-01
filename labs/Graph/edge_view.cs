using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace labs.Graph
{
    [Serializable]
    public class edge_view
    {
        [NonSerialized]
        private Shape Line;
        [NonSerialized]
        private Line LeftLine;
        [NonSerialized]
        private Line RightLine;
        [NonSerialized]
        private TextBox textBox1;
        [NonSerialized]
        private Brush opaqueBrush;
        [NonSerialized]
        private List<Shape> Lines;
        private int edge_weight;
        private graph_view _graph;
        private node_view from_node;
        private node_view to_node;

        Thickness th = new Thickness(1.1);
        Brush br;

        public edge_view(graph_view _graph, node_view from_node, node_view to_node, string weight)
        {
            this._graph = _graph;
            if (from_node != to_node)
            {
                Line = new Line(); 
                Line.MouseEnter += new MouseEventHandler(Line_MouseEnter);
                Line.MouseLeave += new MouseEventHandler(Line_MouseLeave);
                Line.MouseLeftButtonDown += new MouseButtonEventHandler(Line_MouseLeftButtonDown);
                Line.Stroke = Brushes.Black;
                Line.StrokeThickness = 1;
                ((Line)Line).X1 = 0;
                ((Line)Line).Y1 = 0;
                    LeftLine = new Line();
                    RightLine = new Line();
                    RightLine.Stroke = LeftLine.Stroke = Brushes.Black;
                    RightLine.StrokeThickness = LeftLine.StrokeThickness = 6;
                    _graph.GRCanvas.Children.Add(LeftLine);
                    _graph.GRCanvas.Children.Add(RightLine);
                if (Lines == null)
                {
                    Lines = new List<Shape>();
                    Lines.Add(Line);
                        Lines.Add(LeftLine);
                        Lines.Add(RightLine);
                }
                textBox1 = new TextBox();
                textBox1.Width = 50;
                br = new SolidColorBrush(Colors.Transparent);
                textBox1.BorderBrush = br;
                textBox1.VerticalContentAlignment = VerticalAlignment.Center;
                textBox1.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox1.FontSize = 14;
                TxtBox1_TextChanged(null, null);
                textBox1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(TxtBox1_PreviewMouseLeftButtonDown);
                textBox1.TextChanged += new TextChangedEventHandler(TxtBox1_TextChanged);
                textBox1.Text = weight;
                edge_weight = int.Parse(textBox1.Text);
                opaqueBrush = new SolidColorBrush(Colors.Black);
                opaqueBrush.Opacity = 0;
                textBox1.Background = opaqueBrush;
                _graph.GRCanvas.Children.Add(textBox1);
                _graph.GRCanvas.Children.Add(Line);
                Canvas.SetZIndex(Line, 0);
                Canvas.SetZIndex(textBox1, 2);
                this.from_node = from_node;
                this.to_node = to_node;
                to_node.pointPositionChange += new graph_view.PointPositionChanged(OnPointPositionChanged);
                from_node.pointPositionChange += new graph_view.PointPositionChanged(OnPointPositionChanged);
                OnPointPositionChanged(to_node);
                OnPointPositionChanged(from_node);
            }
            else
                MessageBox.Show("Граф задачі має бути ациклічним!");            
        }

        public bool isValid { get; set; }
        public int Weight
        {
            get { return edge_weight; }
            set { edge_weight = value; }
        }

        public TextBox TxtBox
        {
            get { return textBox1; }
        }

        public node_view From
        {
            get { return from_node; }
        }

        public node_view To
        {
            get { return to_node; }
        }

        public List<Shape> Edge
        {
            get
            {
                return Lines;
            }
        }

        private void TxtBox1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseEnter();
        }
        private void TxtBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                edge_weight = int.Parse(textBox1.Text);
                if (edge_weight >= 1000)
                    throw new Exception();
                if (edge_weight < 0)
                    throw new Exception();
                textBox1.Foreground = Brushes.Green;
                isValid = true;
            }
            catch (Exception ex)
            {
                textBox1.Foreground = Brushes.Green;
                isValid = false;
            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeave();
        }
        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            OnMouseEnter();
        }
        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_graph.IsEdgeDelete)
            {
                _graph.DeleteEdge(this);
                _graph.EndDeleteEdge();
            }
        }

        private void OnMouseEnter()
        {
            foreach (Shape shape in Lines)
                Canvas.SetZIndex(Line, 1);

            textBox1.IsReadOnly = false;
            textBox1.Background = Brushes.White;
        }
        private void OnMouseLeave()
        {
            Line.Stroke = Brushes.Black;           
                LeftLine.Stroke = RightLine.Stroke = Brushes.Black;
                foreach (Shape shape in Lines)
                Canvas.SetZIndex(Line, 0);

            textBox1.CaretBrush = textBox1.Background = opaqueBrush;
            textBox1.IsReadOnly = true;
        }

        public void OnPointPositionChanged(node_view top)
        {
            textBox1.Text = edge_weight.ToString();
                if (top == from_node)
                {
                    Canvas.SetLeft(Line, from_node.RELPos.X + from_node.GRNode.Width / 2);
                    Canvas.SetTop(Line, from_node.RELPos.Y + graphNode.Radius / 2);
                }
                ((Line)Line).X2 = to_node.RELPos.X - from_node.RELPos.X;
                ((Line)Line).Y2 = to_node.RELPos.Y - from_node.RELPos.Y;
                Canvas.SetLeft(textBox1, from_node.RELPos.X + from_node.GRNode.Width / 2 + ((Line)Line).X2 / 2 - textBox1.Width/2);
                Canvas.SetTop(textBox1, from_node.RELPos.Y + ((Line)Line).Y2 / 2 - textBox1.FontSize / 3);

                    double u_l = Math.Atan2(((Line)Line).X1 - ((Line)Line).X2, ((Line)Line).Y1 - ((Line)Line).Y2);
                   double u = Math.PI / 33;

                   LeftLine.StrokeThickness = 1;
                   RightLine.StrokeThickness = 1;
                   LeftLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                   LeftLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);
                   LeftLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l + 2 * u);
                   LeftLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l + 2 * u);

                   RightLine.X1 = ((Line)Line).X2 + 10 * Math.Sin(u_l);
                   RightLine.Y1 = ((Line)Line).Y2 + 10 * Math.Cos(u_l);
                   RightLine.X2 = ((Line)Line).X2 + 30 * Math.Sin(u_l - 2 * u);
                   RightLine.Y2 = ((Line)Line).Y2 + 30 * Math.Cos(u_l - 2 * u);

                   Canvas.SetLeft(LeftLine, from_node.RELPos.X + to_node.GRNode.Width / 2);
                   Canvas.SetTop(LeftLine, from_node.RELPos.Y + graphNode.Radius / 2);
                   Canvas.SetLeft(RightLine, from_node.RELPos.X + to_node.GRNode.Width / 2);
                   Canvas.SetTop(RightLine, from_node.RELPos.Y + graphNode.Radius / 2);
        }
    }
}
