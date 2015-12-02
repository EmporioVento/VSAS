using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace labs.GraphCS
{
    [Serializable]
    public class node_view
    {
        private int topNum;
        [NonSerialized]
        private graphNodeCS _grNode;
        private graph_viewCS _graph_viewCS;
        private bool isValid;
        public string txt_id;
        public int id;
        public string txt_weight;
        private bool _move;
        private Point rel_pos;


        public event graph_viewCS.PointPositionChanged pointPositionChange;

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

        public graph_viewCS Graph
        {
            get { return _graph_viewCS; }
        }

        public graphNodeCS View
        {
            get { return _grNode; }
        }

        public node_view(graph_viewCS _graph_viewCS)
        {
            this._graph_viewCS = _graph_viewCS;
            _graph_viewCS.Tops().Add(this);
            _grNode = new graphNodeCS(this);
            _grNode.lblId_node.Content = _grNode.txtId_node.Text = (_graph_viewCS.maxTopID() + 1).ToString();
            id = int.Parse(_grNode.txtId_node.Text);
            _graph_viewCS.GRCanvas.Children.Add(_grNode);
            pointPositionChange += _graph_viewCS.OnPointPositionChanged;
            IsValid = false;
        }

        public bool Move
        {
            get { return _move; }
            set {_move = value; }
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
            Point p = Mouse.GetPosition(_graph_viewCS.GRCanvas);
            rel_pos = new Point(p.X - GRNode.Width / 2 + 2, p.Y - GRNode.Height / 4 + 3);
        }

        public graphNodeCS GRNode
        {
            get { return _grNode; }
        }

        public void CNTRPosition()
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, _graph_viewCS.GRCanvas.ActualWidth / 2); // w.ActualWidth / 2 - _grNode.Width / 2 + 2);
            Canvas.SetTop(_grNode, _graph_viewCS.GRCanvas.ActualHeight / 2); // w.ActualHeight / 2 - _grNode.Height + 3);
            rel_pos.X = _graph_viewCS.GRCanvas.ActualWidth / 2; // w.ActualWidth / 2 - _grNode.Width / 2;
            rel_pos.Y = _graph_viewCS.GRCanvas.ActualHeight / 2; // w.ActualHeight / 2 - _grNode.Height;
        }

        public void GivenPosition(double X, double Y, string name)
        {
            Window w = Window.GetWindow(GRNode);
            Canvas.SetLeft(_grNode, X);
            Canvas.SetTop(_grNode, Y);
            rel_pos.X = X;
            rel_pos.Y = Y;
            _grNode.txtId_node.Text = name;
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

        public int TopNum
        {
            get { return topNum; }
            set { topNum = value; }
        }

        public void Validate()
        {
            try
            {
                topNum = int.Parse(txt_id);
                if (!Graph.CheckTopNum(this))
                    IsValid = false;
                else
                    IsValid = true;
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }
    }
}