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
using System.Diagnostics;
using System.Threading;

namespace labs.Gant
{
    public class DestinationAlgorithms
    {
        private graph_view _graph_view;
        public graph_viewCS _graph_viewCS;
        private Label label_vert;
        private work_view _w_view;
        public List<work_view> WorkList = new List<work_view>();
        public List<sub_work_view> SubWorkList = new List<sub_work_view>();

        Dictionary<int, List<int>> neighbors_nodes;
        Dictionary<KeyValuePair<int, int>, int> edge_weight;
        private Shape LineVertical;
        private List<Shape> LinesVert;
        public List<horizontal_line> LinesHorizontal = new List<horizontal_line>();
        public double interval_vertical, interval_horizontal;
        Dictionary<int, int> node_countEdges = new Dictionary<int, int>();
        public List<labs.Graph.node_view> TopListNew = new List<labs.Graph.node_view>();
        public List<labs.GraphCS.node_view> TopListCSNew = new List<labs.GraphCS.node_view>();
        bool[] status;
        private const int INFINITY = 10000;
        private Dictionary<int, int> distances_list;
        private Dictionary<int, List<int>> neighbors_nodes666;
        List<int> node_min_list = new List<int>();
        int[,] matrixCS;
        int countCS, Tkr;
        public List<sub_work_view> SubWorkTestList = new List<sub_work_view>();
        Stopwatch stopWatch = new Stopwatch();

        private Canvas canvas;

        public double Keap, Kp, Ke, Time = 0;
        public string time;

        public DestinationAlgorithms(Canvas canvas, graph_view _graph_view, graph_viewCS _graph_viewCS)
        {
            this.canvas = canvas;
            this._graph_view = _graph_view;
            this._graph_viewCS = _graph_viewCS;
            countCS = _graph_viewCS.TopList.Count;
            matrixCS = new int[countCS, countCS];
            stopWatch.Start();
        }

        public Canvas GantCanvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public void AddVerticalLines(double startX, int count, double interval)
        {
            this.interval_vertical = interval;
            for (int i = 0; i < count; i++)
            {
                LineVertical = new Line();
                LineVertical.Stroke = Brushes.Blue;
                LineVertical.StrokeThickness = 0.5;
                ((Line)LineVertical).X1 = startX;
                ((Line)LineVertical).Y1 = 30;
                ((Line)LineVertical).X2 = startX;
                ((Line)LineVertical).Y2 = 600;
                if (LinesVert == null)
                {
                    LinesVert = new List<Shape>();

                }
                LinesVert.Add(LineVertical);

                label_vert = new Label();
                label_vert.VerticalContentAlignment = VerticalAlignment.Center;
                label_vert.HorizontalContentAlignment = HorizontalAlignment.Left;
                label_vert.FontSize = 12;
                label_vert.Content = i.ToString();
                label_vert.Width = 30;

                canvas.Children.Add(LineVertical);
                canvas.Children.Add(label_vert);
                Canvas.SetZIndex(LineVertical, 0);
                Canvas.SetZIndex(label_vert, 2);

                Canvas.SetLeft(label_vert, startX - 10);
                Canvas.SetTop(label_vert, 600);
                startX += interval;
            }
        }

        public void AddHorizontalLines(int count)
        {
            this.interval_horizontal = 570 / count;
            double startY = 30 + interval_horizontal;

            for (int i = 0; i < count; i++)
            {
                List<int> to_id = new List<int>();
                foreach (labs.GraphCS.edge_view edge in _graph_viewCS.edgeList)
                {
                    if (edge.From.id == _graph_viewCS.TopList[i].id)
                        to_id.Add(edge.To.id);
                    else if (edge.To.id == _graph_viewCS.TopList[i].id)
                        to_id.Add(edge.From.id);
                }

                horizontal_line horiz_line = new horizontal_line(this, startY, count, _graph_viewCS.TopList[i].id, to_id);
                startY += interval_horizontal;
            }
        }

        public string test(int algorithm, int destination_algorithm, bool isMessShow)
        {
            if (!_graph_view.GraphIsAcyclic(false) || !_graph_viewCS.IsConnect(false))
            {
                MessageBox.Show("Неможливо побудувати діаграму Ганта");
                return "";
            }
            ClearAll();

            AddVerticalLines(50, 397, 25);
            AddHorizontalLines(_graph_viewCS.TopList.Count);

            Label lbl_Alg = new Label();
            if (algorithm == 2)
                lbl_Alg.Content = _graph_view.Algorithm_2(false, false);
            else if (algorithm == 3)
                lbl_Alg.Content = _graph_view.Algorithm_3(false, false);
            else if (algorithm == 12)
                lbl_Alg.Content = _graph_view.Algorithm_12(false, false);

            canvas.Children.Add(lbl_Alg);
            Canvas.SetLeft(lbl_Alg, 20);

            status = new bool[_graph_view.TopList.Count];
            for (int i = 0; i < _graph_view.TopList.Count; i++)
            {
                TopListNew.Add(_graph_view.TopList[_graph_view.idListAlgorithm[i]]);
                status[i] = false;
            }

            List<int> Tkri = new List<int>();

            for (int i = 0; i < _graph_view.TopList.Count; i++)
            {
                _graph_view.NodeIsAcyclic(i, 2, false);
                Tkri.Add(_graph_view.CriticalPath(i, 3));
            }

            Tkr = Tkri.Max();

            Planning(destination_algorithm);
            string str = T_End();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = String.Format("{0:00}год, {1:00}хв, {2:00}с, {3:00}мс", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Time = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + Convert.ToDouble(ts.Milliseconds) / 1000;
            str += "\nЧас роботи алгоритму:\n" + time;
            if (isMessShow)
                MessageBox.Show(str + "\n" + Time, "Алгоритм " + algorithm + " Призначення " + destination_algorithm);

            string str0 = "Алгоритм " + algorithm + "\tПризначення " + destination_algorithm + "\n" + str +
                "\n\n*********************************************************\n\n";

            return str0;
        }

        public void CSQueue()
        {
            int n = _graph_viewCS.TopList.Count;
            int[] count_edges = new int[n];

            for (int i = 0; i < n; i++)
            {
                count_edges[i] = 0;
                foreach (labs.GraphCS.edge_view edge in _graph_viewCS.edgeList)
                    if (edge.From.id == _graph_viewCS.TopList[i].id || edge.To.id == _graph_viewCS.TopList[i].id)
                        count_edges[i]++;
                node_countEdges.Add(_graph_viewCS.TopList[i].id, count_edges[i]);
            }

            node_countEdges = node_countEdges.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var n_c in node_countEdges)
                TopListCSNew.Add(_graph_viewCS.TopList.Find(x => x.id.Equals(n_c.Key)));

        }

        public void Planning(int destination_algorithm)
        {
            CSQueue();
            FillDependentNodes();
            MatrixCS();

            for (int i = 0, j = 0; j < TopListNew.Count && i < TopListCSNew.Count; j++)
                if (neighbors_nodes[TopListNew[j].id].Count == 0)
                {
                    double startY = LinesHorizontal.Find(x => x.id.Equals(TopListCSNew[i].id)).startY;

                    _w_view = new work_view(this, TopListNew[j].id.ToString(),
                        interval_vertical * (int)TopListNew[j].weight, 50,
                        startY - 20, TopListCSNew[i].id);
                    status[j] = true;
                    i++;
                }


            for (int j = 0; j < TopListNew.Count; j++)
                if (!status[j])
                {
                    List<int> depend = neighbors_nodes[TopListNew[j].id];
                    int count_depend_done = 0;
                    for (int k = 0; k < depend.Count; k++)
                    {
                        int index = 0;
                        for (index = 0; index < TopListNew.Count; index++)
                            if (TopListNew[index].id == depend[k])
                                break;
                        if (status[index])
                            count_depend_done++;
                    }
                    if (count_depend_done == depend.Count)
                    {
                        List<work_view> dependWorkList = new List<work_view>();
                        for (int k = 0; k < depend.Count; k++)
                            dependWorkList.Add(WorkList.Find(x => x.rect_id.Equals(depend[k])));

                        int need_proc_id = -1;
                        if (destination_algorithm == 4 || destination_algorithm == 5)
                            need_proc_id = FindNeedProcAlg4(j, depend, dependWorkList);
                        else if (destination_algorithm == 6)
                            need_proc_id = FindNeedProcAlg6(j, dependWorkList);
                        DrawSubWorks(destination_algorithm, j, need_proc_id, dependWorkList);
                        DrawWork(j, need_proc_id, dependWorkList);

                        for (int k = 0; k < j; k++)
                            if (!status[k])
                            {
                                j = k - 1;
                                break;
                            }
                    }
                }
        }

        public int FindNeedProcAlg4(int j, List<int> depend, List<work_view> dependWorkList)
        {
            int min_vp = 10000;

            Dictionary<int, List<labs.GraphCS.node_view>> _dict_proc_mb = new Dictionary<int, List<labs.GraphCS.node_view>>();
            Dictionary<double, List<labs.GraphCS.node_view>> _dict_proc_mb_without = new Dictionary<double, List<labs.GraphCS.node_view>>();
            List<labs.GraphCS.node_view> _proc_mb;
            double minEnd = LinesHorizontal[0].endX;

            if (dependWorkList.Count == 0)
            {
                for (int i = 0; i < TopListCSNew.Count; i++)
                {
                    minEnd = DrawWork(j, TopListCSNew[i].id, dependWorkList);
                    try
                    {
                        _dict_proc_mb_without[minEnd].Add(TopListCSNew[i]);
                    }
                    catch (Exception ex)
                    {
                        _dict_proc_mb_without[minEnd] = new List<GraphCS.node_view>();
                        _dict_proc_mb_without[minEnd].Add(TopListCSNew[i]);
                    }
                    canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).Rectangle);
                    canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).label1);
                    WorkList.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)));
                }
                _dict_proc_mb_without = _dict_proc_mb_without.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                _proc_mb = _dict_proc_mb_without.First().Value;
            }
            else
            {
                for (int i = 0; i < TopListCSNew.Count; i++)
                {
                    int min = 0;
                    for (int k = 0; k < dependWorkList.Count; k++)
                        min += edge_weight[new KeyValuePair<int, int>(dependWorkList[k].rect_id, TopListNew[j].id)] *
                             Deijkstra(TopListCSNew[i].id, dependWorkList[k].proc_id);

                    if (min <= min_vp)
                    {
                        min_vp = min;
                        try
                        {
                            _dict_proc_mb[min_vp].Add(TopListCSNew[i]);
                        }
                        catch (Exception ex)
                        {
                            _dict_proc_mb[min_vp] = new List<GraphCS.node_view>();
                            _dict_proc_mb[min_vp].Add(TopListCSNew[i]);
                        }
                    }
                }
                _dict_proc_mb = _dict_proc_mb.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                _proc_mb = _dict_proc_mb.First().Value;

                Dictionary<double, List<labs.GraphCS.node_view>> _dict_proc_end_mb = new Dictionary<double, List<labs.GraphCS.node_view>>();

                if (_proc_mb.Count > 1)
                {
                    for (int i = 0; i < _proc_mb.Count; i++)
                    {
                        SubWorkTestList.Clear();
                        DrawSubWorks(4, j, _proc_mb[i].id, dependWorkList);
                        minEnd = DrawWork(j, _proc_mb[i].id, dependWorkList);
                        try
                        {
                            _dict_proc_end_mb[minEnd].Add(_proc_mb[i]);
                        }
                        catch (Exception ex)
                        {
                            _dict_proc_end_mb[minEnd] = new List<GraphCS.node_view>();
                            _dict_proc_end_mb[minEnd].Add(_proc_mb[i]);
                        }
                        canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).Rectangle);
                        canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).label1);
                        WorkList.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)));
                        for (int k = 0; k < SubWorkTestList.Count; k++)
                        {
                            canvas.Children.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)).Rectangle);
                            canvas.Children.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)).label1);
                            SubWorkList.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)));
                        }
                    }
                    _dict_proc_end_mb = _dict_proc_end_mb.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    _proc_mb = _dict_proc_end_mb.First().Value;
                }
            }

            List<labs.GraphCS.node_view> _proc_mb2 = new List<GraphCS.node_view>();
            int max_links = 0;
            _proc_mb2.Add(_proc_mb[0]);
            if (_proc_mb.Count > 1)
                for (int i = 0; i < _proc_mb.Count; i++)
                {
                    if (node_countEdges[_proc_mb[i].id] > max_links)
                        max_links = node_countEdges[_proc_mb[i].id];
                }

            for (int i = 1; i < _proc_mb.Count; i++)
                if (node_countEdges[_proc_mb[i].id] == max_links)
                    _proc_mb2.Add(_proc_mb[i]);

            int need_proc_id = _proc_mb2[0].id;

            if (_proc_mb2.Count > 1)
                for (int i = 0; i < _proc_mb2.Count; i++)
                    for (int m = 0; m < dependWorkList.Count; m++)
                        if (_proc_mb2[i].id == dependWorkList[m].rect_id)
                            need_proc_id = _proc_mb2[i].id;

            return need_proc_id;
        }

        public int FindNeedProcAlg6(int j, List<work_view> dependWorkList)
        {
            Dictionary<double, List<labs.GraphCS.node_view>> _dict_proc_end_mb = new Dictionary<double, List<labs.GraphCS.node_view>>();
            List<labs.GraphCS.node_view> _proc_mb;
            double minEnd;// = LinesHorizontal[0].endX;

            for (int i = 0; i < TopListCSNew.Count; i++)
            {
                SubWorkTestList.Clear();
                DrawSubWorks(6, j, TopListCSNew[i].id, dependWorkList);
                minEnd = DrawWork(j, TopListCSNew[i].id, dependWorkList);
                try
                {
                    _dict_proc_end_mb[minEnd].Add(TopListCSNew[i]);
                }
                catch (Exception ex)
                {
                    _dict_proc_end_mb[minEnd] = new List<GraphCS.node_view>();
                    _dict_proc_end_mb[minEnd].Add(TopListCSNew[i]);
                }
                canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).Rectangle);
                canvas.Children.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)).label1);
                WorkList.Remove(WorkList.Find(x => x.rect_id.Equals(TopListNew[j].id)));
                for (int k = 0; k < SubWorkTestList.Count; k++)
                {
                    canvas.Children.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)).Rectangle);
                    canvas.Children.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)).label1);
                    SubWorkList.Remove(SubWorkList.Find(x => x.sub_work_id.Equals(SubWorkTestList[k].sub_work_id)));
                }
            }
            _dict_proc_end_mb = _dict_proc_end_mb.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            _proc_mb = _dict_proc_end_mb.First().Value;
            List<labs.GraphCS.node_view> _proc_mb2 = new List<GraphCS.node_view>();
            int max_links = 0;

            for (int i = 0; i < _proc_mb.Count; i++)
            {
                if (node_countEdges[_proc_mb[i].id] > max_links)
                    max_links = node_countEdges[_proc_mb[i].id];
            }

            for (int i = 0; i < _proc_mb.Count; i++)
                if (node_countEdges[_proc_mb[i].id] == max_links)
                    _proc_mb2.Add(_proc_mb[i]);

            int need_proc_id = _proc_mb2[0].id;

            if (_proc_mb2.Count > 1)
                for (int i = 0; i < _proc_mb2.Count; i++)
                    for (int m = 0; m < dependWorkList.Count; m++)
                        if (_proc_mb2[i].id == dependWorkList[m].rect_id)
                            need_proc_id = _proc_mb2[i].id;

            return need_proc_id;
        }

        public double get_start(List<sub_line> list_sub_duplex, List<sub_work_view> _sub_work_new,
            Dictionary<sub_work_view, double> _dict_sub_work_link, List<sub_work_view> _sub_work_other_links,
            double start_sub_point_next, double weight)
        {
            for (int m = 0; m < SubWorkList.Count; m++)
                for (int u = 0; u < list_sub_duplex.Count; u++)
                    if (SubWorkList[m].end_y == list_sub_duplex[u].posY)
                        try
                        {
                            _dict_sub_work_link.Add(SubWorkList[m], SubWorkList[m].end_x);
                        }
                        catch (Exception ex) { }

            _dict_sub_work_link = _dict_sub_work_link.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var n_c in _dict_sub_work_link)
                _sub_work_other_links.Add(n_c.Key);
            List<sub_work_view> _sub_work_test_links = _sub_work_other_links;

            for (int lol = 0; lol < SubWorkList.Count; lol++)
                for (int m = 0; m < _sub_work_new.Count; m++)
                {
                    double other_sub_start_x = _sub_work_new[m].end_x - _sub_work_new[m].rect_width;

                    if ((start_sub_point_next < other_sub_start_x && (start_sub_point_next + weight) > other_sub_start_x) ||
                        (start_sub_point_next < _sub_work_new[m].end_x && (start_sub_point_next + weight) > other_sub_start_x) ||
                        start_sub_point_next == other_sub_start_x || (start_sub_point_next + weight == _sub_work_new[m].end_x))
                        start_sub_point_next = _sub_work_new[m].end_x;

                    start_sub_point_next = ShiftLinks(_sub_work_other_links, start_sub_point_next, weight);
                }
            if (_sub_work_new.Count == 0)
                start_sub_point_next = ShiftLinks(_sub_work_other_links, start_sub_point_next, weight);
            return start_sub_point_next;
        }

        public void DrawSubWorks(int alg, int j, int need_proc_id, List<work_view> dependWorkList)
        {
            double start_sub_point = 0;
            List<sub_line> list_sub_duplex = new List<sub_line>();
            sub_line _sub_line_duplex = LinesHorizontal[0].SubLinesHorizontal[0];
            horizontal_line horiz_line = LinesHorizontal[0];
            sub_line _sub_line = LinesHorizontal[0].SubLinesHorizontal[0];
            if (alg == 4)
                for (int i = 0; i < dependWorkList.Count; i++)
                    if (dependWorkList[i].end_x > start_sub_point)
                        start_sub_point = dependWorkList[i].end_x;

            for (int k = 0; k < dependWorkList.Count; k++)
            {
                list_sub_duplex.Clear();
                if (alg == 6 || alg == 5)
                    start_sub_point = dependWorkList[k].end_x;

                if (dependWorkList[k].proc_id != need_proc_id)
                {
                    NodeMinList(dependWorkList[k].proc_id, need_proc_id);
                    double start_sub_point_next = start_sub_point;
                    for (int i = 0; i < node_min_list.Count - 1; i++)
                    {
                        if (node_min_list.Count <= 2)
                        {
                            horiz_line = LinesHorizontal.Find(x => x.id.Equals(dependWorkList[k].proc_id));
                            _sub_line = horiz_line.SubLinesHorizontal.Find(x => x.to_id.Equals(need_proc_id));
                        }
                        else
                        {
                            horiz_line = LinesHorizontal.Find(x => x.id.Equals(node_min_list[i]));
                            _sub_line = horiz_line.SubLinesHorizontal.Find(x => x.to_id.Equals(node_min_list[i + 1]));
                        }

                        double weight = edge_weight[new KeyValuePair<int, int>(dependWorkList[k].rect_id, TopListNew[j].id)] *
                                        interval_vertical;

                        for (int m = 0; m < LinesHorizontal.Count; m++)
                            if (LinesHorizontal[m] != horiz_line)
                                for (int tt = 0; tt < horiz_line.SubLinesHorizontal.Count; tt++)
                                    for (int u = 0; u < LinesHorizontal[m].SubLinesHorizontal.Count; u++)
                                        if ((LinesHorizontal[m].SubLinesHorizontal[u].to_id == horiz_line.SubLinesHorizontal[tt].to_id ||
                                            LinesHorizontal[m].SubLinesHorizontal[u].from_id == horiz_line.SubLinesHorizontal[tt].to_id))// &&
                                            list_sub_duplex.Add(LinesHorizontal[m].SubLinesHorizontal[u]);

                        List<sub_work_view> _sub_work_new = new List<sub_work_view>();
                        Dictionary<sub_work_view, double> _dict_sub_work = new Dictionary<sub_work_view, double>();

                        for (int m = 0; m < SubWorkList.Count; m++)
                            if (SubWorkList[m].end_y == _sub_line.posY)
                                _dict_sub_work.Add(SubWorkList[m], SubWorkList[m].end_x);

                        _dict_sub_work = _dict_sub_work.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                        foreach (var n_c in _dict_sub_work)
                            _sub_work_new.Add(n_c.Key);

                        List<sub_work_view> _sub_work_other_links = new List<sub_work_view>();
                        Dictionary<sub_work_view, double> _dict_sub_work_link = new Dictionary<sub_work_view, double>();

                        for (int s = 0; s < horiz_line.SubLinesHorizontal.Count; s++)
                            if (horiz_line.SubLinesHorizontal[s].to_id != _sub_line.to_id)
                                for (int l = 0; l < SubWorkList.Count; l++)
                                    if (SubWorkList[l].end_y == horiz_line.SubLinesHorizontal[s].posY)
                                        _dict_sub_work_link.Add(SubWorkList[l], SubWorkList[l].end_x);

                        get_start(list_sub_duplex, _sub_work_new, _dict_sub_work_link, _sub_work_other_links,
                            start_sub_point_next, weight);

                        sub_work_view _sub_work = new sub_work_view(this, dependWorkList[k].rect_id, TopListNew[j].id,
                            weight, start_sub_point_next, _sub_line.posY - 10, maxSubID() + 1);

                        start_sub_point_next += weight;
                    }
                }
            }
        }

        public double ShiftLinks(List<sub_work_view> _sub_work_other_links, double start_sub_point_next, double weight)
        {
            int count_nakladka = 0;
            List<sub_work_view> _sub_work_test_links = _sub_work_other_links;
            List<sub_work_view> _list = new List<sub_work_view>();
            int nakladka_in_others = 0;
            for (int c = 0; c < _sub_work_other_links.Count; c++)
            {
                double vertical_sub_start_x = _sub_work_other_links[c].end_x - _sub_work_other_links[c].rect_width;

                if ((start_sub_point_next < vertical_sub_start_x && (start_sub_point_next + weight) > vertical_sub_start_x) ||
                    (start_sub_point_next < _sub_work_other_links[c].end_x && (start_sub_point_next + weight) > vertical_sub_start_x) ||
                    start_sub_point_next == vertical_sub_start_x || (start_sub_point_next + weight == _sub_work_other_links[c].end_x))
                {
                    _list.Add(_sub_work_other_links[c]);
                    count_nakladka++;
                }
                for (int s = 0; s < _list.Count; s++)
                {
                    int c_n = 0;
                    for (int ss = 0; ss < _list.Count; ss++)
                        if (s != ss)
                            if ((_list[s].start_x < _list[ss].start_x && _list[s].end_x > _list[ss].start_x) ||
                                (_list[s].start_x < _list[ss].end_x && _list[s].end_x > _list[ss].start_x) ||
                                _list[s].start_x == _list[ss].start_x || (_list[s].end_x == _list[ss].end_x))
                            {
                                c_n++;
                                nakladka_in_others++;

                            }
                    if (c_n < count_nakladka - 1)
                    {
                        int index = _sub_work_test_links.IndexOf(_list[s]);
                        _sub_work_test_links.Remove(_list[s]);

                        _list.Remove(_list[s]);
                        s--; c = index;
                        nakladka_in_others -= c_n;
                    }
                }
                if (nakladka_in_others >= (Math.Pow(count_nakladka, 2) - count_nakladka))
                {
                    start_sub_point_next = _sub_work_test_links.First().end_x;
                    _list.Remove(_sub_work_test_links.First());
                    _sub_work_test_links.Remove(_sub_work_test_links.First());
                    count_nakladka = _list.Count;
                    c--;
                    nakladka_in_others = 0;
                }
            }
            return start_sub_point_next;
        }

        public double DrawWork(int j, int need_proc_id, List<work_view> dependWorkList)
        {
            double start = 50;
            for (int o = 0; o < dependWorkList.Count; o++)
                foreach (sub_work_view sub_work in SubWorkList)
                    if (sub_work.to_id == TopListNew[j].id && sub_work.end_x > start)
                        start = sub_work.end_x;

            if (start == 50 && dependWorkList.Count > 0)
                start = dependWorkList[0].end_x;

            List<work_view> _work_new = new List<work_view>();
            double weight_node = interval_vertical * (int)TopListNew[j].weight;

            Dictionary<work_view, double> _dict_work = new Dictionary<work_view, double>();

            for (int m = 0; m < WorkList.Count; m++)
                if (WorkList[m].end_y == LinesHorizontal.Find(x => x.id.Equals(need_proc_id)).startY)
                    if (WorkList[m].end_x > start)
                        _dict_work.Add(WorkList[m], WorkList[m].end_x);

            _dict_work = _dict_work.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var n_c in _dict_work)
                _work_new.Add(n_c.Key);

            if (_work_new.Count > 0)
                for (int m = 0; m < _work_new.Count; m++)
                {
                    double other_start_x = _work_new[m].end_x - _work_new[m].rect_width;

                    if ((start < other_start_x && (start + weight_node) >
                        other_start_x) || (start < _work_new[m].end_x &&
                        (start + weight_node) > other_start_x) || start == other_start_x)
                        start = _work_new[m].end_x;
                }

            _w_view = new work_view(this, TopListNew[j].id.ToString(), weight_node, start,
                LinesHorizontal.Find(x => x.id.Equals(need_proc_id)).startY - 20, need_proc_id);
            status[j] = true;

            return _w_view.end_x;
        }

        public string T_End()
        {
            double max = 0;
            for (int i = 0; i < WorkList.Count; i++)
                if (WorkList[i].end_x > max)
                    max = WorkList[i].end_x;

            double T = (max - 50) / interval_vertical;
            int sum_node_weight = 0;

            for (int i = 0; i < TopListNew.Count; i++)
                sum_node_weight += TopListNew[i].weight;

            Kp = sum_node_weight / T;
            Ke = Kp / TopListCSNew.Count;
            Keap = Tkr / T;

            return "Tкрг = " + Tkr + "\nT = " + T + "\nKпр = " + Kp.ToString("N2") + "\nКе = " + Ke.ToString("N2") + "\nКеап = "
                + Keap.ToString("N2") + "\n";
        }

        private void FillDependentNodes()
        {
            neighbors_nodes = new Dictionary<int, List<int>>();
            edge_weight = new Dictionary<KeyValuePair<int, int>, int>();

            foreach (labs.Graph.node_view top in TopListNew)
                neighbors_nodes[top.id] = new List<int>();
            foreach (labs.Graph.edge_view line in _graph_view.edgeList)
            {
                neighbors_nodes[line.To.id].Add(line.From.id);
                edge_weight[new KeyValuePair<int, int>(line.From.id, line.To.id)] = (int)line.Weight;
            }
        }

        public void NodeMinList(int id_from, int id_to)
        {
            node_min_list.Clear();
            int index_from = -1, index_to = -1;
            for (int i = 0; i < countCS; i++)
                if (TopListCSNew[i].id == id_from)
                    index_from = i;
                else if (TopListCSNew[i].id == id_to)
                    index_to = i;

            int idd = NodesMin(index_from, index_to);
            while (idd != index_from)
                idd = NodesMin(index_from, idd);

            node_min_list.Add(id_to);
        }

        public int NodesMin(int index_from, int index_to)
        {
            int[] status = new int[countCS];
            for (int i = 0; i < countCS; i++)
                status[i] = 0;

            int curr = index_from;
            status[curr] = 1;      // вершина переглянута
            System.Collections.Queue och = new System.Collections.Queue();
            och.Enqueue(curr);

            while (och.Count != 0)
            {
                curr = Convert.ToInt32(och.Dequeue());
                for (int i = 0; i < countCS; i++)
                    if (matrixCS[curr, i] != 0 && status[i] == 0)
                    {
                        status[i] = 1;          // відвідали вершину
                        och.Enqueue(i);
                        if (i == index_to)
                        {
                            node_min_list.Insert(0, TopListCSNew[curr].id);
                            return curr;
                        }
                    }
            }
            return curr;
        }

        private int Deijkstra(int from_id, int to_id)
        {
            neighbors_nodes666 = new Dictionary<int, List<int>>();
            foreach (labs.GraphCS.node_view top in _graph_viewCS.TopList)
                neighbors_nodes666[top.id] = new List<int>();
            foreach (labs.GraphCS.edge_view line in _graph_viewCS.edgeList)
            {
                neighbors_nodes666[line.From.id].Add(line.To.id);
                neighbors_nodes666[line.To.id].Add(line.From.id);
            }

            distances_list = new Dictionary<int, int>();
            List<int> went = new List<int>();
            foreach (int top in neighbors_nodes666.Keys)
                if (top != from_id)
                    distances_list[top] = INFINITY;
                else
                    distances_list[top] = 0;

            int curTop = from_id;
            int pathSum;
            int index;
            List<int> list;
            int minDistance;
            while (went.Count != TopListCSNew.Count)
            {
                foreach (int neighbor in neighbors_nodes666[curTop])
                {
                    pathSum = distances_list[curTop] + 1;
                    if (pathSum < distances_list[neighbor])
                        distances_list[neighbor] = pathSum;
                }
                went.Add(curTop);
                index = went.Count - 1;
                do
                {
                    if (curTop == INFINITY)
                        curTop = went[--index];
                    list = neighbors_nodes666[curTop];
                    curTop = minDistance = INFINITY;
                    for (int i = 0; i < list.Count; ++i)
                        if (!went.Contains(list[i]) && distances_list[list[i]] < minDistance)
                        {
                            minDistance = distances_list[list[i]];
                            curTop = list[i];
                        }
                }
                while (curTop == INFINITY && index != 0);
                if (index == -1 || curTop == INFINITY)
                    break;
            }
            return distances_list[to_id];
        }

        public void MatrixCS()
        {
            for (int i = 0; i < countCS; i++)
                for (int j = 0; j < countCS; j++)
                    matrixCS[i, j] = 0;

            foreach (labs.GraphCS.edge_view line in _graph_viewCS.edgeList)
                for (int i = 0; i < countCS; i++)
                    for (int j = 0; j < countCS; j++)
                        if (TopListCSNew[i].id == line.From.id)
                            if (TopListCSNew[j].id == line.To.id)
                            {
                                matrixCS[i, j] = 1;
                                matrixCS[j, i] = 1;
                            }
        }

        public int maxSubID()
        {
            int temp = 0;
            foreach (sub_work_view top in SubWorkList)
                if (top.sub_work_id > temp)
                    temp = top.sub_work_id;
            return temp;
        }

        public void ClearAll()
        {
            try
            {
                node_countEdges.Clear();
                canvas.Children.Clear();
                LinesHorizontal.Clear();
                LinesVert.Clear();
                WorkList.Clear();
                SubWorkList.Clear();
                //              TopListCSNew.Clear();
                //              TopListNew.Clear();
            }
            catch (Exception ex) { }
        }
    }
}