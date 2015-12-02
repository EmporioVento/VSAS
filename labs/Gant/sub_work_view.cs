using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace labs.Gant
{
    public class sub_work_view
    {
        private DestinationAlgorithms _dest_alg;
        public Label label1;
        public Shape Rectangle;

        public int to_id;
        public double rect_height, rect_width;
        public double end_x, end_y, start_x;
        public int sub_work_id;
        public string lbl;

        public sub_work_view(DestinationAlgorithms _dest_alg, int from_id, int to_id, double top_weight, double from_x, double from_y, int sub_work_id)
        {
            this._dest_alg = _dest_alg;
            
            Rectangle = new Rectangle();
            Rectangle.Fill = Brushes.White;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.StrokeThickness = 1;
            rect_height = ((Rectangle)Rectangle).Height = 10;
            rect_width = ((Rectangle)Rectangle).Width = top_weight;
            this.to_id = to_id;
            start_x = from_x;
            end_x = from_x + rect_width;
            end_y = from_y + rect_height;
            this.sub_work_id = sub_work_id;
      //      this.sub_line_id = sub_line_id;
            _dest_alg.SubWorkList.Add(this);
            _dest_alg.SubWorkTestList.Add(this);
            
            label1 = new Label();
  //          label1.Width = top_weight;
            label1.VerticalContentAlignment = VerticalAlignment.Center;
            label1.HorizontalContentAlignment = HorizontalAlignment.Center;
            label1.FontSize = 10;
            label1.Content = lbl = from_id + "-" + to_id;

            _dest_alg.GantCanvas.Children.Add(label1);
            _dest_alg.GantCanvas.Children.Add(Rectangle);
            Canvas.SetZIndex(Rectangle, 0);
            Canvas.SetZIndex(label1, 2);

            Canvas.SetLeft(Rectangle, from_x);
            Canvas.SetTop(Rectangle, from_y);
            Canvas.SetLeft(label1, from_x-2);
            Canvas.SetTop(label1, from_y-8);
        }
    }
}
