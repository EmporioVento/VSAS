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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace labs.GraphCS
{[Serializable]
    public class graph_viewCS// : property_base
    {

        private Canvas canvas;
        public List<edge_view> edgeList = new List<edge_view>();
        public List<node_view> TopList = new List<node_view>();
        private bool isGraphTask;
        private bool orient;
        private bool edgeAdd; 
        private bool edgeDelete;
        private bool nodeDelete;
        public node_view FirstTop;
        public delegate void PointPositionChanged(node_view top);

        public graph_viewCS(Canvas canvas, bool orient)
        {
            this.canvas = canvas;
            this.orient = orient;
        }

        public List<node_view> Tops()
        {
            return TopList;
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
        public bool CheckTopNum(node_view top)
        {
            foreach (node_view model in TopList)
                if (top.TopNum == model.TopNum && model != top)
                    return false;
            return true;
        }       
    
        public void AddTop(bool center)
        {
            node_view top = new node_view(this);            
            
            if (center)
                top.CNTRPosition();
            else
                OnPointPositionChanged(top);
        }

        public void AddTop(double X, double Y, string id)
        {
            node_view top = new node_view(this);
            top.GivenPosition(X, Y, id);
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
            }
        }
        public void ValidateTopNumbers()
        {
            foreach (node_view top in TopList)
                top.Validate();
        }

        public string ShowTop()
        {
            string str = "";
            for (int i = 0; i < TopList.Count; i++)
                str += "x=" + TopList[i].RELPos.X + "y=" + TopList[i].RELPos.Y + "\n";
            return str;
        }

        public bool isTask
        {
            get { return isGraphTask; }
            set { isGraphTask = value; }
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
                edge_view line = new edge_view(this, from_node, to_node);
                edgeList.Add(line);
                FirstTop = null;
                EndAddEdge();
            }
        }

        public void AddEdge(int index_from, int index_to)
        {
            bool isExist = false;
            node_view from_node = TopList[index_from];
            node_view to_node = TopList[index_to];
            foreach (edge_view line0 in edgeList)
                if ((line0.From == from_node && line0.To == to_node) || (line0.To == from_node && line0.From == to_node))
                {
                    MessageBox.Show("Дуга вже існує");
                    isExist = true;
                }
            if (!isExist)
            {
                edge_view line = new edge_view(this, from_node, to_node);
                edgeList.Add(line);
                FirstTop = null;
                EndAddEdge();
            }
        }

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

        // зв'язність графа
        public bool IsConnect(bool is_mess_show)
        {
            int n = TopList.Count;
            int[] status = new int[n];
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    matrix[i, j] = 0;
                status[i] = 0;
            }

            foreach (edge_view line in edgeList)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (TopList[i].id == line.From.id)
                            if (TopList[j].id == line.To.id)
                            {
                                matrix[i, j] = 1;
                                matrix[j, i] = 1;
                            }

            int curr = 0;
            status[curr] = 1;      // вершина переглянута
            System.Collections.Queue och = new System.Collections.Queue();
            och.Enqueue(curr);
 
            while (och.Count != 0)
            {
                curr = Convert.ToInt32(och.Dequeue());
                for (int i = 0; i < n; i++)
                    if (matrix[curr, i] != 0 && status[i] == 0)
                    {
                        status[i] = 1;          // відвідали вершину
                        och.Enqueue(i);
                    }
            }
            if (endSeek(status) == true)
            {
                if (is_mess_show)
                    MessageBox.Show("Граф зв'язний");
                return true;
            }
            else
                MessageBox.Show("Граф не зв'язний");
            return false;
        }

        static bool endSeek(int[] pstatus)
        {
            bool flag = true;
            for (int i = 0; i < pstatus.Length; i++)
                if (pstatus[i] == 0)
                    flag = false;
            return flag;
        }

        public int maxTopID()
        {
            int temp = 0;
            foreach (node_view top in TopList)
                if (top.id > temp)
                    temp = top.id;
            return temp;
        }

        public string ShowNodesEdges()
        {
            string str = "";
            for (int i = 0; i < TopList.Count; i++)
                str += "node" + i + "[" + TopList[i].id + "](" + TopList[i].RELPos.X + ";" + TopList[i].RELPos.Y + ")\n";
            str += "\n";

            for (int i = 0; i < edgeList.Count; i++)
                str += "edge" + i + "(" + TopList.IndexOf(edgeList[i].From) + ";" + TopList.IndexOf(edgeList[i].To) + ")\n";

            return str;
        } 
    
        public void SaveAs()
        {
            if (IsConnect(true))
            {
                string fileText = ShowNodesEdges();
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "GraphCS";
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
                            AddTop(Convert.ToDouble(split[2]), Convert.ToDouble(split[3]), split[1]);
                        if (split[j].Contains("edge"))
                            AddEdge(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
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
    }
}