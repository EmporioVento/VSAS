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
    public class horizontal_line
    {
        private Label label_horizontal;
        private Shape LineHorizontal;
        double sub_interval_horizontal;
        private DestinationAlgorithms _dest_alg;
        public List<sub_line> SubLinesHorizontal;
        public int id {get; set;}
        private List<int> to_id;
        public double startY;
        public double endX;


        public horizontal_line(DestinationAlgorithms _dest_alg, double startY, int count, int id, List<int> to_id)
        {
            this._dest_alg = _dest_alg;
            this.startY = startY;
            this.id = id;
            this.to_id = to_id;
            this.sub_interval_horizontal = (_dest_alg.interval_horizontal-20) / (count);
            endX = 50;
            LineHorizontal = new Line();
            LineHorizontal.Stroke = Brushes.Blue;
            LineHorizontal.StrokeThickness = 1;
            ((Line)LineHorizontal).X1 = 50;
            ((Line)LineHorizontal).Y1 = startY;
            ((Line)LineHorizontal).X2 = 9950;
            ((Line)LineHorizontal).Y2 = startY;

            _dest_alg.LinesHorizontal.Add(this);
            label_horizontal = new Label();
            label_horizontal.HorizontalContentAlignment = HorizontalAlignment.Center;
            label_horizontal.HorizontalContentAlignment = HorizontalAlignment.Center;
            label_horizontal.FontSize = 12;
            label_horizontal.Content = id.ToString();
            label_horizontal.Width = 30;
            label_horizontal.FontWeight = FontWeights.Bold;

            _dest_alg.GantCanvas.Children.Add(LineHorizontal);
            _dest_alg.GantCanvas.Children.Add(label_horizontal);
            Canvas.SetZIndex(LineHorizontal, 0);
            Canvas.SetZIndex(label_horizontal, 2);

            Canvas.SetLeft(label_horizontal, 25);
            Canvas.SetTop(label_horizontal, startY - 15);

            if (sub_interval_horizontal >= 20)
                startY -= sub_interval_horizontal;
            else
                startY -= 20;
            for (int i = 0; i < to_id.Count; i++)
            {                
                sub_line horiz_line = new sub_line(_dest_alg, this, startY, id, to_id[i]);
                startY -= sub_interval_horizontal;
            }
        }
    }
}
