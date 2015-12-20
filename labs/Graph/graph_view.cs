using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Text;
using labs.Graph;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics; 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace labs.Graph
{
    [Serializable]

    [TestClass]
    public class Test_Graph
    {
        static Canvas canvas = new Canvas();
        graph_view _grView = new graph_view(canvas, false);

        // 3 Юнит Теста, определять по названию.
        [TestMethod]
        public void Test_isAcyclic()
        {
            int count = 10;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0.5;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);

            int ac = 0;
            if (_grView.GraphIsAcyclic(false))
                ac = 1;
            Assert.AreEqual(ac, 0, 0.5, "NO Acyclic"); // И так пишет ошибку;  0.5 - расхождения значения
        }

        [TestMethod]
        public void Test_ZeroinCount()
        {
            int count = 0;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void Test_ZeroinCoherence()
        {
            int count = 10;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);

            Assert.AreEqual(1, coherence);
        }

        // 3 Стресс/Лоад теста - условия выполнения: нахождения условия до Стресс теста, до 3мин
        [TestMethod, Timeout (180000)]
        public void Test_SearchTime10000Counts()
        {
            int count = 10000;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0.5;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);
            // Нет. 500+ Mb
        }

        [TestMethod, Timeout(180000)]
        public void Test_750Count()
        {
            int count = 750;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0.5;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);
            // Nine. 29.8-32 Mb
        }

        [TestMethod, Timeout(180000)]
        public void Test_500Count()
        {
            int count = 500;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0.5;
            _grView.GenerateGraph(count, min_weight, max_weight, coherence);
            // Прошел за 1 мин. 27 МБ. Так показало:(
        }

        [TestMethod]
        public void TestGraphS() // slava test
        {
            int count = 10;
            int min_weight = 5;
            int max_weight = 10;
            double coherence = 0.9;

            _grView.GenerateGraph(count, min_weight, max_weight, coherence);

            _grView.ClearAll();

            int count_children = 100;
            if (_grView.TopList.Count == 0 && _grView.edgeList.Count == 0 && _grView.canvas.Children.Count == 0)
                count_children = 0;

            Assert.AreEqual(count_children, 1, 1, "No Clear");
            // Slava Test1
        }


        public int sum_nodes()
        {
            int sum_nodes = 0;
            for (int i = 0; i < _grView.TopList.Count; i++)
                sum_nodes += _grView.TopList[i].weight;
            return sum_nodes;
        }

        public int sum_edges()
        {
            int sum_edges = 0;
            for (int i = 0; i < _grView.edgeList.Count; i++)
                sum_edges += _grView.edgeList[i].Weight;
            return sum_edges;
        }
    }

    public class graph_view// : property_base
    {

        public Canvas canvas;
        public List<edge_view> edgeList = new List<edge_view>();
        public List<node_view> TopList = new List<node_view>();
        public List<int> idListAlgorithm = new List<int>();
        private bool orient;
        private bool edgeAdd; 
        private bool edgeDelete;
        private bool nodeDelete;
        
        public node_view FirstTop;
        public delegate void PointPositionChanged(node_view top);

        public graph_view(Canvas canvas, bool orient)
        {
            this.canvas = canvas;
            this.orient = orient;
        }

        public Canvas GRCanvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public bool IsEdgeAdd
        {
            get { return edgeAdd; }
        }
        public bool IsEdgeDelete
        {
            get { return edgeDelete; }
        }
    
        public void AddTop(bool center)
        {
            node_view top = new node_view(this);          
            if (center)
                top.CNTRPosition();
            else
                OnPointPositionChanged(top);
        }

        public void AddTop(double X, double Y, string name)
        {
            node_view top = new node_view(this);
            top.GivenPosition(X, Y, name);
        }

        public void AddTop(double X, double Y, string id, string weight)
        {
            node_view top = new node_view(this);
            top.GivenPosition(X, Y, id, weight);
        }
        
        public void OnPointPositionChanged(node_view top)
        {
            top.UpdPos();
            Canvas.SetLeft(top.GRNode, top.RELPos.X);
            Canvas.SetTop(top.GRNode, top.RELPos.Y);
        }
        public void DeleteNode(node_view top)
        {
            if (TopList.Contains(top))
            {
                for (int i = edgeList.Count-1; i >=0; i--)
                    if (edgeList[i].From == top || edgeList[i].To == top)
                        DeleteEdge(edgeList[i]);
                TopList.Remove(top);
                canvas.Children.Remove(top.View);
            }
        }
        public void DeleteEdge(edge_view line)
        {
            if (edgeList.Contains(line))
            {
                edgeList.Remove(line);
                foreach (Shape shape in line.Edge)
                    canvas.Children.Remove(shape);
                canvas.Children.Remove(line.TxtBox);
            }
        }
        public void ValidateTopNumbers()
        {
            foreach (node_view top in TopList)
                top.Validate();
        }

        public void SaveAs()
        {
            if (GraphIsAcyclic(true))
            {
                string fileText = ShowNodesEdges();
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "GraphTask";
                dialog.Filter = "Text Files(*.txt)|*.txt";
                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllText(dialog.FileName, fileText);
                    SaveCanvasToFile(dialog.FileName + ".bmp");
                }
            }
            else
                MessageBox.Show("Збереження неможливе");
        }

        public void SaveCanvasToFile(string filename)
        {
            Canvas cnvs = canvas;
            Transform transform = cnvs.LayoutTransform;
            cnvs.LayoutTransform = null;
            Size size = new Size(1165, 662);
            cnvs.Measure(size);
            cnvs.Arrange(new Rect(size));
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                80d,
                80d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(cnvs);

            using (FileStream outStream = new FileStream(filename, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
        }

        public string OpenGraphFrom(string filename)
        {
            int counter = 0;
            string line, str = "";
            if (filename != "")
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    str += line + "\n";
                    counter++;
                }
                file.Close();
            }
            return str;
        }

        public string ShowNodesEdges()
        {
            string str = "";
            for (int i = 0; i < TopList.Count; i++)
                str += "node" + i + "[" + TopList[i].id + "][" + TopList[i].weight + "](" + TopList[i].RELPos.X + ";" + TopList[i].RELPos.Y + ")\n";
            str += "\n";

            for (int i = 0; i < edgeList.Count; i++)
                str += "edge" + i + "[" + edgeList[i].TxtBox.Text + "](" + TopList.IndexOf(edgeList[i].From) + ";" + TopList.IndexOf(edgeList[i].To) + ")\n";

            return str;
        }   

        public void LoadGraph(string str)
        {
            if (str != "")
            {
                ClearAll();
                string[] substrings = Regex.Split(str, "(\n)");
                string[] split = new string[substrings.Length];
                for (int i = 0; i < substrings.Length; i++)
                {
                    split = substrings[i].Split(new Char[] { '[', ']', '(', ')', ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < split.Length; j++)
                    {
                        if (split[j].Contains("node"))
                            AddTop(Convert.ToDouble(split[3]), Convert.ToDouble(split[4]), split[1], split[2]);
                        if (split[j].Contains("edge"))
                            AddEdge(Convert.ToInt32(split[2]), Convert.ToInt32(split[3]), split[1], false);
                    }
                }
            }
        }

        public void ClearAll()
        {
            TopList.Clear();
            edgeList.Clear();
            canvas.Children.Clear();
        }
            
        public bool IsNodeDelete
        {
            get { return nodeDelete; }
        }
        
        public void AddEdge(node_view from_node, node_view to_node)
        {
            bool isExist = false;
            foreach (edge_view line0 in edgeList)
                if ((line0.From == from_node && line0.To == to_node) || (line0.To == from_node && line0.From == to_node))
                {
                    MessageBox.Show("Дуга вже існує");
                    isExist = true;
                }
            if (!isExist)
            {
                edge_view line = new edge_view(this, from_node, to_node, "1");
                edgeList.Add(line);
                FirstTop = null;
                EndAddEdge();
            }
        }

        public void AddEdge(int index_from, int index_to, string weight, bool generate)
        {
            node_view from_node = TopList[index_from];
            node_view to_node = TopList[index_to];
            bool isExist = false;
            foreach (edge_view line0 in edgeList)
            {
                if ((line0.From == from_node && line0.To == to_node) || (line0.To == from_node && line0.From == to_node))
                {
                    if (!generate)
                        MessageBox.Show("Дуга вже існує");
                        
                    else
                        line0.Weight = line0.Weight + int.Parse(weight);
                    isExist = true;
                }
            }
            if (!isExist)
            {
                if (weight == "0")
                    weight = "1";
                edge_view line = new edge_view(this, from_node, to_node, weight);
                edgeList.Add(line);
                FirstTop = null;
                EndAddEdge();
            }
        }


        // node-edge  add-delete
        public void StartAddEdge()
        {
            edgeAdd = true;
        }
        public void EndAddEdge()
        {
            edgeAdd = false;
            FirstTop = null;
        }
        public void StartDeleteEdge()
        {
            edgeDelete = true;
        }
        public void EndDeleteEdge()
        {
            edgeDelete = false;
        }
        public void StartDeleteNode()
        {
            nodeDelete = true;
        }
        public void EndDeleteNode()
        {
            nodeDelete = false;
        }

        public int maxTopID()
        {
            int temp = 0;
            foreach (node_view top in TopList)
                if (top.id > temp)
                    temp = top.id;
            return temp;
        }

        public int[,] Matrix(int algorithm)
        {
            int n = TopList.Count;
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = 0;

            if (algorithm == 2)
                foreach (edge_view line in edgeList)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (TopList[i].id == line.To.id)
                                if (TopList[j].id == line.From.id)
                                    matrix[i, j] = 1;

            if (algorithm == 3)
                foreach (edge_view line in edgeList)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (TopList[i].id == line.From.id)
                                if (TopList[j].id == line.To.id)
                                    matrix[i, j] = 1;
            return matrix;
        }

        List<int> all_neighbors_nodes = new List<int>();

        public bool GraphIsAcyclic(bool is_mess_show)
        {
            for (int i = 0; i < TopList.Count; i++)
                if (!NodeIsAcyclic(i, 3, true))
                    return false;
            if (is_mess_show)
                MessageBox.Show("Граф ациклічний");
            return true;
        }

        // ациклічність графа
        public bool NodeIsAcyclic(int currNode, int algorithm, bool is_mess_show)
        {
            int n = TopList.Count;
            int[] status = new int[n];
            int[,] matrix = Matrix(algorithm);
            for (int i = 0; i < n; i++)
                status[i] = 0;

            int curr = currNode;
            System.Collections.Queue och = new System.Collections.Queue();
            och.Enqueue(curr);
            all_neighbors_nodes.Clear();

            while (och.Count != 0)
            {
                curr = Convert.ToInt32(och.Dequeue());
                all_neighbors_nodes.Add(curr);
                for (int i = 0; i < n; i++)
                    if (matrix[curr, i] != 0 &&  status[i] == 0)
                    {
                        status[i] = 1;          // відвідали вершину
                        och.Enqueue(i);
                        if (och.Contains(currNode))
                        {
                            if (is_mess_show)
                                MessageBox.Show("Цикл в " + TopList[currNode].id + " вершині");
                            return false;
                        }
                    }
            }
            return true; 
        }

        public int[,] MatrixOnlyNeighbors(List<int> neighbors_nodes, int algorithm)
        {
            int n = TopList.Count;
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = 0;

            if (algorithm == 2)
                foreach (edge_view line in edgeList)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (TopList[i].id == line.To.id)
                                if (TopList[j].id == line.From.id)
                                    matrix[i, j] = TopList[j].weight;

            if (algorithm == 3)
                foreach (edge_view line in edgeList)
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (TopList[i].id == line.From.id)
                                if (TopList[j].id == line.To.id)
                                    matrix[i, j] = TopList[j].weight;

            for (int i = 0; i < n; i++)
            {
                if (!neighbors_nodes.Contains(i))
                    for (int j = 0; j < n; j++)
                        matrix[i, j] = 0;
                else if (i == neighbors_nodes.First() && algorithm == 3)
                    for (int j = 0; j < n; j++)
                        if (matrix[i, j] != 0)
                            matrix[i, j] += TopList[i].weight;
            }

            return matrix;
        }

        public int CriticalPath(int currNode, int algorithm)
        {
            int n = TopList.Count;
            NodeIsAcyclic(currNode, algorithm, false);
            if (all_neighbors_nodes.Count == 1)
            {
                if (algorithm == 2)
                    return 0;
                else if (algorithm == 3)
                    return TopList[currNode].weight;
            }

            int[,] matrix = MatrixOnlyNeighbors(all_neighbors_nodes, algorithm);

            List<int> tk = new List<int>();
            int max;

            for (int i = 0; i < n; i++)
                tk.Add(0);

            List<int> ii = new List<int>();
            List<int> jj = new List<int>();
            List<int> dij = new List<int>();

            for (int j = 0; j < n; j++)
                for (int i = 0; i < n; i++)
                    if (matrix[i, j] != 0)
                    {
                        ii.Add(i);
                        jj.Add(j);
                        dij.Add(matrix[i, j]);
                    }

            for (int i = 0; i < n; i++)
                for (int k = 0; k < ii.Count; k++)
                {
                    max = tk[ii[k]] + dij[k];
                    if (tk[jj[k]] < max)
                        tk[jj[k]] = max;
                }

            return tk.Max();                
        }

        public string Algorithm_2(bool is_mess_acyclic_show, bool is_mess_queue_show)
        {
            idListAlgorithm.Clear();
            string str2 = "[Алгоритм №2] Вершина (Різниця між Тпс і Трс):  ";
            if (GraphIsAcyclic(is_mess_acyclic_show))
            {
                string str = "Проміжкові дані дані\n";
                int n = TopList.Count;

                List<int> Tkri = new List<int>();

                for (int i = 0; i < TopList.Count; i++)
                {
                    NodeIsAcyclic(i, 2, false);
                    Tkri.Add(CriticalPath(i, 3));
                }

                int Tkr = Tkri.Max();
                str += "Tкрг = " + Tkr;

                Dictionary<int, int> node_alg2 = new Dictionary<int, int>();

                int difference = 0;
                int Trs, Tps;
                for (int i = 0; i < TopList.Count; i++)
                {
                    Trs = CriticalPath(i, 2);
                    Tps = Tkr - Tkri[i];
                    difference = Tps - Trs;
                    node_alg2.Add(i, difference);
                    str += "\ni = " + TopList[i].id + ";\tТкр[i] = Tпс[i] (" + Tkr + " - " + Tkri[i] + ") - Трс[i] (" + Trs + ") = " + difference;
                }

                node_alg2 = node_alg2.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                str += "\n\nРЕЗУЛЬТАТ\nВершина (Різниця між Тпс і Трс)\n";
                foreach (var n_c in node_alg2)
                {
                    idListAlgorithm.Add(n_c.Key);
                    str += TopList[n_c.Key].id + " (" + n_c.Value + ")" + "\n";
                    str2 += TopList[n_c.Key].id + " (" + n_c.Value + ")" + ",  ";
                }
                if (is_mess_queue_show)
                    MessageBox.Show(str, "Алгоритм 2");
            }
            else
                MessageBox.Show("Алгоритм неможливо застосувати, якщо граф має цикли");

            return str2;
        }

        public string Algorithm_3(bool is_mess_acyclic_show, bool is_mess_queue_show)
        {
            idListAlgorithm.Clear();
            string str2 = "[Алгоритм №3] Вершина (Ткр. вершини):  ";
            if (GraphIsAcyclic(is_mess_acyclic_show))
            {
                string str = "Вершина (Ткр. вершини)\n";
                Dictionary<int, int> node_CriticalPath = new Dictionary<int, int>();

                for (int i = 0; i < TopList.Count; i++)
                    node_CriticalPath.Add(i, CriticalPath(i, 3));

                node_CriticalPath = node_CriticalPath.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var n_c in node_CriticalPath)
                {
                    idListAlgorithm.Add(n_c.Key);
                    str += TopList[n_c.Key].id + " (" + n_c.Value + ")" + "\n";
                    str2 += TopList[n_c.Key].id + " (" + n_c.Value + ")" + ",  ";
                }
                if (is_mess_queue_show)
                    MessageBox.Show(str, "Алгоритм 3");
            }
            else
                MessageBox.Show("Алгоритм неможливо застосувати, якщо граф має цикли");

            return str2;
        }

        public string Algorithm_12(bool is_mess_acyclic_show, bool is_mess_queue_show)
        {
            idListAlgorithm.Clear();
            string str2 = "[Алгоритм №12] Вершина (Кількість вихідних дуг):  ";
            if (GraphIsAcyclic(is_mess_acyclic_show))
            {
                int n = TopList.Count;
                int[] count_edges = new int[n];
                int[,] matrix = new int[n, 2];
                matrix = Matrix(3);
                Dictionary<int, int> node_countEdges = new Dictionary<int, int>();
                List<int> list_count = new List<int>();

                for (int i = 0; i < n; i++)
                {
                    count_edges[i] = 0;
                    for (int j = 0; j < n; j++)
                        if (matrix[i, j] != 0)
                            count_edges[i]++;
                    node_countEdges.Add(i, count_edges[i]);
                }

                string str = "Вершина (Кількість вихідних дуг)\n";
                
                node_countEdges = node_countEdges.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (var n_c in node_countEdges)
                {
                    idListAlgorithm.Add(n_c.Key);
                    str += TopList[n_c.Key].id + " (" + n_c.Value + ")" + "\n";
                    str2 += TopList[n_c.Key].id + " (" + n_c.Value + ")" + ",  ";
                }
                if (is_mess_queue_show)
                    MessageBox.Show(str, "Алгоритм 12");
            }
            else
                MessageBox.Show("Алгоритм неможливо застосувати, якщо граф має цикли");
            
            return str2;
        }

        public void GenerateGraph(int count, int min_weight, int max_weight, double coherence)
        {
            int sum_NodeWeights = 0;
            int sum_EdgeWeights;
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
                AddTop(rnd.Next(50, 650), rnd.Next(50, 500), rnd.Next(min_weight, max_weight).ToString());

            foreach (node_view node in TopList)
                sum_NodeWeights += node.weight;

            sum_EdgeWeights = Convert.ToInt32(Convert.ToDouble(sum_NodeWeights) / coherence) - sum_NodeWeights;

            do
            {
                int from = rnd.Next(count);
                int to = rnd.Next(count);
                int weight = Convert.ToInt32(sum_EdgeWeights / rnd.Next(5, 10));
                if (from != to)
                {
                    if (weight == 0)
                        weight++;
                    if (sum_EdgeWeights - weight > 0)
                    {
                        AddEdge(from, to, weight.ToString(), true);
                        sum_EdgeWeights -= weight;
                    }
                    else if (sum_EdgeWeights - weight <= 0)
                    {
                        AddEdge(from, to, (sum_EdgeWeights).ToString(), true);
                        sum_EdgeWeights = 0;
                    }
                }
            } while (sum_EdgeWeights != 0);

            for (int i = 0; i < TopList.Count; i++)
                while (!NodeIsAcyclic(i, 3, false))
                {
                    for (int j = edgeList.Count - 1; j >= 0; j--)
                        if (edgeList[j].To.id == TopList[i].id)
                        {                            
                            int weight = edgeList[j].Weight;
                            DeleteEdge(edgeList[j]);
                            edge_view e_w = edgeList[rnd.Next(edgeList.Count)];

                            e_w.Weight += weight;

                            e_w.OnPointPositionChanged(e_w.To);
                            e_w.OnPointPositionChanged(e_w.From);
                        }
                }

        }
    }
}