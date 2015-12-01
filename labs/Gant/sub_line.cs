using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using labs.Graph;
using labs.GraphCS;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace labs.Gant
{    
    public class sub_line
    {
        private Label label_horizontal;
        private Shape SubLineHorizontal;
        private DestinationAlgorithms _dest_alg;
        private horizontal_line _horiz_line;
        public int from_id;
        public int to_id;
        public double posY;

        public sub_line(DestinationAlgorithms _dest_alg, horizontal_line _horiz_line, double startY, int from_id, int to_id)
        {
            this._dest_alg = _dest_alg;
            this._horiz_line = _horiz_line;
            this.from_id = from_id;
            this.to_id = to_id;

            SubLineHorizontal = new Line();
            SubLineHorizontal.Stroke = Brushes.Blue;
            SubLineHorizontal.StrokeThickness = 0.2;
            ((Line)SubLineHorizontal).X1 = 50;
            ((Line)SubLineHorizontal).Y1 = startY;
            ((Line)SubLineHorizontal).X2 = 9950;
            ((Line)SubLineHorizontal).Y2 = startY;
            posY = startY;
            if (_horiz_line.SubLinesHorizontal == null)
                _horiz_line.SubLinesHorizontal = new List<sub_line>();

            _horiz_line.SubLinesHorizontal.Add(this);
            label_horizontal = new Label();
            label_horizontal.HorizontalContentAlignment = HorizontalAlignment.Center;
            label_horizontal.HorizontalContentAlignment = HorizontalAlignment.Center;
            label_horizontal.FontSize = 8;
            label_horizontal.Content = from_id + "-" + to_id;
            label_horizontal.Width = 30;

            _dest_alg.GantCanvas.Children.Add(SubLineHorizontal);
            _dest_alg.GantCanvas.Children.Add(label_horizontal);
            Canvas.SetZIndex(SubLineHorizontal, 0);
            Canvas.SetZIndex(label_horizontal, 2);

            Canvas.SetLeft(label_horizontal, 25);
            Canvas.SetTop(label_horizontal, startY - 12);           
        }
    }
}
