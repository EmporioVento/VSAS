using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using labs.Graph;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace labs.Gant
{
    public class work_view
    {
        private DestinationAlgorithms _dest_alg;
        public Label label1;
        public Shape Rectangle;

        public int rect_id;
        public Point position;
        public double rect_height, rect_width;
        public double end_x, end_y;
        public int proc_id;

        public work_view(DestinationAlgorithms _dest_alg, string top_id, double top_weight, double from_x, double from_y, int proc_id)
        {
            this._dest_alg = _dest_alg;

            Rectangle = new Rectangle();
            Rectangle.Fill = Brushes.White;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.StrokeThickness = 2;
            rect_height = ((Rectangle)Rectangle).Height = 20;
            rect_width = ((Rectangle)Rectangle).Width = top_weight;

            end_x = from_x + rect_width;
            end_y = from_y + rect_height;
            this.proc_id = proc_id;
            _dest_alg.LinesHorizontal.Find(x => x.id.Equals(proc_id)).endX = end_x;
            _dest_alg.WorkList.Add(this);
            
            label1 = new Label();
            label1.Width = top_weight;
            label1.VerticalContentAlignment = VerticalAlignment.Center;
            label1.HorizontalContentAlignment = HorizontalAlignment.Center;
            label1.FontSize = 12;
            label1.FontWeight = FontWeights.Bold;
            label1.Content = top_id;
            rect_id = int.Parse(top_id);

            _dest_alg.GantCanvas.Children.Add(label1);
            _dest_alg.GantCanvas.Children.Add(Rectangle);
            Canvas.SetZIndex(Rectangle, 0);
            Canvas.SetZIndex(label1, 2);

            position = new Point(from_x, from_y);

            Canvas.SetLeft(Rectangle, from_x);
            Canvas.SetTop(Rectangle, from_y);
            Canvas.SetLeft(label1, from_x);
            Canvas.SetTop(label1, from_y-4);
        }
    }
}