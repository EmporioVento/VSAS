using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using labs.Graph;

using System.Windows.Markup;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace labs.Graph
{
    [Serializable]
    public class node_view
    {
        [NonSerialized]
        private graphNode _grNode;
        private graph_view _graph_view;
        private bool isValid;
        public string txt_id;
        public string txt_weight;
        private bool _move;
        private Point rel_pos;
        public int id, weight;


        public event graph_view.PointPositionChanged pointPositionChange;

        public Point RELPos
        {
            get
            {
                return rel_pos;
            }
            set
            {
                rel_pos = value;
            }
        }

        public graph_view Graph
        {
            get { return _graph_view; }
        }

        public graphNode View
        {
            get { return _grNode; }
        }

        public node_view(graph_view _graph_view)
        {
            this._graph_view = _graph_view;
            _graph_view.TopList.Add(this);
            _grNode = new graphNode(this);
            _grNode.txtWeight_node.Text = "5";
            _grNode.lblId_node.Content = _grNode.txtId_node.Text = (_graph_view.maxTopID() + 1).ToString();
            id = int.Parse(_grNode.txtId_node.Text);
            weight = int.Parse(_grNode.txtWeight_node.Text);
            _graph_view.GRCanvas.Children.Add(_grNode);
            pointPositionChange += _graph_view.OnPointPositionChanged;
            IsValid = false;
        }

        public bool Move
        {
            get
            {
                return _move;
            }
            set
            {
                _move = value;
            }
        }

        public void OnMouseLeave()
        {
            if (_move)
                pointPositionChange(this);
        }

        public void OnMouseMove()
        {
            if (_move)
                pointPositionChange(this);
        }

        public void UpdPos()
        {
            Point p = Mouse.GetPosition(_graph_view.GRCanvas);
            rel_pos = new Point(p.X - GRNode.Width / 2 + 2, p.Y - GRNode.Height / 4 + 3);
        }

        public graphNode GRNode
        {
            get { return _grNode; }
        }

        public void CNTRPosition()
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, _graph_view.GRCanvas.ActualWidth / 2); // w.ActualWidth / 2 - _grNode.Width / 2 + 2);
            Canvas.SetTop(_grNode, _graph_view.GRCanvas.ActualHeight / 2); // w.ActualHeight / 2 - _grNode.Height + 3);
            rel_pos.X = _graph_view.GRCanvas.ActualWidth / 2; // w.ActualWidth / 2 - _grNode.Width / 2;
            rel_pos.Y = _graph_view.GRCanvas.ActualHeight / 2; // w.ActualHeight / 2 - _grNode.Height;
        }

        public void GivenPosition(double X, double Y, string weight)
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, X);
            Canvas.SetTop(_grNode, Y);
            rel_pos.X = X;
            rel_pos.Y = Y;
            _grNode.txtWeight_node.Text = weight;
            this.weight = int.Parse(weight);
      //      _grNode.txtId_node.Text = name;
        }

        public void GivenPosition(double X, double Y, string id, string weight)
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, X);
            Canvas.SetTop(_grNode, Y);
            rel_pos.X = X;
            rel_pos.Y = Y;
            _grNode.txtWeight_node.Text = weight;
            this.weight = int.Parse(weight);
            _grNode.txtId_node.Text = id;
        }

        public void GivenPosition(double X, double Y)
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, X);
            Canvas.SetTop(_grNode, Y);
            rel_pos.X = X;
            rel_pos.Y = Y;
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                if (isValid)
                    _grNode.txtId_node.Foreground = System.Windows.Media.Brushes.Black;
                else
                    _grNode.txtId_node.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        public void Validate()
        {
            try
            {

            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }
    }
}