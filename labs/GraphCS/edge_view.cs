using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace labs.GraphCS
{
    [Serializable]
    public class edge_view
    {
        private Shape Line;
        private List<Shape> Lines;
        private bool isLine;
        private graph_viewCS _graph;
        private node_view from_node;
        private node_view to_node;

        Thickness th = new Thickness(1.1);

        public edge_view(graph_viewCS _graph, node_view from_node, node_view to_node)
        {
            this._graph = _graph;
            if (from_node != to_node)
                isLine = true;
            else
                isLine = false;
            if (isLine)
                Line = new Line();
            else
                Line = new Ellipse();

            Line.MouseLeftButtonDown += new MouseButtonEventHandler(Line_MouseLeftButtonDown);
            Line.Stroke = Brushes.Black;
            Line.StrokeThickness = 1;
            if (isLine)
            {
                ((Line)Line).X1 = 0;
                ((Line)Line).Y1 = 0;              
            }
            else
            {
                ((Ellipse)Line).Width = 50;
                ((Ellipse)Line).Height = 50;
            }
            if (Lines == null)
            {
                Lines = new List<Shape>();
                Lines.Add(Line);
            }
            _graph.GRCanvas.Children.Add(Line);
            Canvas.SetZIndex(Line, 0);
            this.from_node = from_node;
            this.to_node = to_node;
            to_node.pointPositionChange += new graph_viewCS.PointPositionChanged(OnPointPositionChanged);
            from_node.pointPositionChange += new graph_viewCS.PointPositionChanged(OnPointPositionChanged);
            OnPointPositionChanged(to_node);
            if (isLine)
                OnPointPositionChanged(from_node);
        }

        public bool isValid { get; set; }

        public node_view From
        {
            get { return from_node; }
        }

        public node_view To
        {
            get { return to_node; }
        }

        public bool IsLine
        {
            get { return isLine; }
        }

        public List<Shape> Edge
        {
            get { return Lines; }
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_graph.IsEdgeDelete)
            {
                _graph.DeleteEdge(this);
                _graph.EndDeleteEdge();
            }
        }

        public void OnPointPositionChanged(node_view top)
        {
            if (isLine)
            {
                if (top == from_node)
                {
                    Canvas.SetLeft(Line, from_node.RELPos.X + from_node.GRNode.Width / 2);
                    Canvas.SetTop(Line, from_node.RELPos.Y + graphNode.Radius / 2);
                }
                ((Line)Line).X2 = to_node.RELPos.X - from_node.RELPos.X;
                ((Line)Line).Y2 = to_node.RELPos.Y - from_node.RELPos.Y;
            }
            else
            {
                Canvas.SetLeft(Line, from_node.RELPos.X);
                Canvas.SetTop(Line, from_node.RELPos.Y - 35);
            }
        }
    }
}