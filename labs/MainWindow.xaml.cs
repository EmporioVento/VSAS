using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using labs.GraphCS;
using labs.Gant;
using System.Windows.Markup;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace labs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
     [Serializable]
    public partial class MainWindow : Window
    {
        private graph_view _grView;
        private DestinationAlgorithms _destin_alg;
        private graph_viewCS _grViewCS;
        
        public MainWindow()
        {
            InitializeComponent();
            _grView = new graph_view(canvas, true);
            _grViewCS = new graph_viewCS(canvasCS, false);
            
            PanelsHidden();            
        }

        private void Grid_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            double x = grid1.ActualWidth;
            double y = grid1.ActualHeight;
        }

        private void mainWnd_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Point m = Mouse.GetPosition(canvasGant);
            buflab.Content = "x = " + m.X.ToString() + "\ty = " + m.Y.ToString();
        }
         
        private void mainWnd_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double x = mainWnd.ActualWidth;
            double y = mainWnd.ActualHeight;
            grid1.Width = x;
            grid1.Height = y;
            
        }

        private void mainWnd_StateChanged(object sender, EventArgs e)
        {
            double x = mainWnd.ActualWidth;
            double y = mainWnd.ActualHeight;
            grid1.Width = x;
            grid1.Height = y;
            x = grid1.Width;
            y = grid1.Height;
            x = grid1.ActualWidth;
            y = grid1.ActualHeight;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (tabTask.IsSelected)
            {
                gridTaskCS.Visibility = Visibility.Visible;
                gridGant.Visibility = Visibility.Hidden;
                Connected.Content = "Перевірка ациклічності";
                gridOnlyTask.Visibility = Visibility.Visible;
            }
            else if (tabCS.IsSelected)
            {
                gridTaskCS.Visibility = Visibility.Visible;
                gridGant.Visibility = Visibility.Hidden;
                Connected.Content = "Перевірка на зв'язнісь";
                gridOnlyTask.Visibility = Visibility.Hidden;
            }
            else if (tabGant.IsSelected)
            {
                gridTaskCS.Visibility = Visibility.Hidden;
                gridGant.Visibility = Visibility.Visible;
            }
        }

        private void _Click(object sender, RoutedEventArgs e)
        {
            if (tabTask.IsSelected)
                _grView.GraphIsAcyclic(true);
            else
                _grViewCS.IsConnect(true);
        }

        private void Algorithm_Click(object sender, RoutedEventArgs e)
        {
            _grView.Algorithm_2(false, true);
            _grView.Algorithm_3(false, true);
            _grView.Algorithm_12(false, true);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            Button_Click_3(null, null);
            _grView.GenerateGraph(int.Parse(NodeCount.Text), int.Parse(MinWeight.Text), int.Parse(MaxWeight.Text), double.Parse(Coherence.Text));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            KillAll();
            if (tabTask.IsSelected)
                _grView.AddTop(true);
            else _grViewCS.AddTop(true);
        }

        private void btnAddR_Click(object sender, RoutedEventArgs e)
        {
            KillAll();
            if (tabTask.IsSelected)
                _grView.StartAddEdge();
            else _grViewCS.StartAddEdge();
        }

        private void btnDeleteNode_Click(object sender, RoutedEventArgs e)
        {
            KillAll();
            if (tabTask.IsSelected)
                _grView.StartDeleteNode();
            else _grViewCS.StartDeleteNode();
        }

        private void btnDeleteEdge_Click(object sender, RoutedEventArgs e)
        {
            KillAll();
            if (tabTask.IsSelected)
                _grView.StartDeleteEdge();
            else _grViewCS.StartDeleteEdge();
        }

        private void KillAll()
        {
            if (tabTask.IsSelected)
            {
                if (_grView.IsEdgeAdd)
                    _grView.EndAddEdge();
                if (_grView.IsEdgeDelete)
                    _grView.EndDeleteEdge();
                if (_grView.IsNodeDelete)
                    _grView.EndDeleteNode();
            }
            
            else
            {
                if (_grViewCS.IsEdgeAdd)
                    _grViewCS.EndAddEdge();
                if (_grViewCS.IsEdgeDelete)
                    _grViewCS.EndDeleteEdge();
                if (_grViewCS.IsNodeDelete)
                    _grViewCS.EndDeleteNode();
            }            
        }

        private void Button_Click_1(object sender, EventArgs e)
        {
            if (tabTask.IsSelected)
                _grView.SaveAs();
            else
                _grViewCS.SaveAs();
        }

        private void Button_Click_2(object sender, EventArgs e)
        {
            string filePath = "";
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Text Files(*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
                filePath = dialog.FileName;// SafeFileName;
            
            try
            {
                if (tabTask.IsSelected)
                    _grView.LoadGraph(_grView.OpenGraphFrom(filePath));
                else
                    _grViewCS.LoadGraph(_grViewCS.OpenGraphFrom(filePath));
            }
            catch (Exception ex) { System.Windows.MessageBox.Show("Невірний формат файлу"); }    
        }
         
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (tabTask.IsSelected)
                _grView.ClearAll();
            else
                _grViewCS.ClearAll();
        }

        public void PanelsHidden()
        {
            ModelPanel.Visibility = Visibility.Hidden;
        }

        public void GridsHidden()
        {
            gridOnlyTask.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            PanelsHidden();
            tabTask.IsSelected = true;
            Connected.Content = "Перевірка ациклічності";
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            PanelsHidden();
            ModelPanel.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            mainWnd.Close();
        }

        private void Button_Click_Gant(object sender, EventArgs e)
        {
            int algorithm = -1, destination_algorithm = -1;
            if (radBtnAlg2.IsChecked == true)
                algorithm = 2;
            else if (radBtnAlg3.IsChecked == true)
                algorithm = 3;
            else if (radBtnAlg12.IsChecked == true)
                algorithm = 12;

            if (radBtnDestAlg4.IsChecked == true)
                destination_algorithm = 4;
            else if (radBtnDestAlg5.IsChecked == true)
                destination_algorithm = 5;
            else if (radBtnDestAlg6.IsChecked == true)
                destination_algorithm = 6;

            _destin_alg = new DestinationAlgorithms(canvasGant, _grView, _grViewCS);
            if (algorithm != -1 && destination_algorithm != -1)
                _destin_alg.test(algorithm, destination_algorithm, true);
            else
                MessageBox.Show("Для побудови діаграми Ганта необхідно обрати алгоритми");
        }
    }
}