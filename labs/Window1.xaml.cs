using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using labs.Graph;
using labs.GraphCS;
using labs.Gant;
using System.IO;
using Word = Microsoft.Office.Interop.Word;


namespace labs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private graph_view _grView;
        private DestinationAlgorithms _destin_alg;
        private graph_viewCS _grViewCS;
        double propuskChannels;
        double proizvodProcessors;
        bool isDuplex;
        int countLinks;
        Canvas canvas;

        public Window1(graph_view _grView, graph_viewCS _grViewCS, DestinationAlgorithms _destin_alg, double propuskChannels,
            double proizvodProcessors, bool isDuplex, int countLinks, Canvas canvas)
        {
            InitializeComponent();
            this.proizvodProcessors = proizvodProcessors;
            this.propuskChannels = propuskChannels;
            this.isDuplex = isDuplex;
            this.countLinks = countLinks;
            this.canvas = canvas;
            this._grView = _grView;
            this._grViewCS = _grViewCS;
            this._destin_alg = _destin_alg;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Dictionary<double, List<double>> dict_Kp = new Dictionary<double, List<double>>();
            Dictionary<double, List<double>> dict_Ke = new Dictionary<double, List<double>>();
            Dictionary<double, List<double>> dict_Keap = new Dictionary<double, List<double>>();
            Dictionary<double, List<double>> dict_Time = new Dictionary<double, List<double>>();
            int int_minCountTops = int.Parse(minCountTops.Text);
            int int_maxCountTops = int.Parse(maxCountTops.Text);
            int int_stepCountTops = int.Parse(stepCountTops.Text);
            int int_minConnected = Convert.ToInt32(double.Parse(minConnected.Text) * 100);
            int int_maxConnected = Convert.ToInt32(double.Parse(maxConnected.Text) * 100);
            int int_stepConnected = Convert.ToInt32(double.Parse(stepConnected.Text) * 100);
            int int_minWeightTops = int.Parse(minWeightTops.Text);
            int int_maxWeightTops = int.Parse(maxWeightTops.Text);
            int int_countGraphs = int.Parse(countGraphs.Text);
            int k = 0;
            string str = "";


            for (int i = int_minConnected; i <= int_maxConnected; i += int_stepConnected)
            {
                double coherence = Convert.ToDouble(i) / 100;
                dict_Kp[coherence] = new List<double>();
                dict_Ke[coherence] = new List<double>();
                dict_Keap[coherence] = new List<double>();
                dict_Time[coherence] = new List<double>();
                k = 0;
                for (int j = int_minCountTops; k < int_countGraphs; j += int_stepCountTops, k++)
                {
                    _grView.ClearAll();
                    if (j > int_maxCountTops)
                        j = int_minCountTops;

                    _grView.GenerateGraph(j, int_minWeightTops, int_maxWeightTops, coherence);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    //     str += "Зв'язність " + coherence + "\n" +_destin_alg.test(2, 4, false);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(2, 4, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(2, 5, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(2, 6, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(3, 4, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(3, 5, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(3, 6, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(12, 4, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(12, 5, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);

                    _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, isDuplex, countLinks);
                    str += "Зв'язність " + coherence + "\n" + _destin_alg.test(12, 6, false);
                    dict_Kp[coherence].Add(_destin_alg.Kp);
                    dict_Ke[coherence].Add(_destin_alg.Ke);
                    dict_Time[coherence].Add(_destin_alg.Time);
                    dict_Keap[coherence].Add(_destin_alg.Keap);
                }
            }

            FileStream fs = new FileStream("test.txt", FileMode.Create);
            StreamWriter w = new StreamWriter(fs, Encoding.UTF8);
            w.WriteLine(str);
            w.Flush();
            w.Close();
            fs.Close();
            SaveWord(dict_Kp, dict_Ke, dict_Keap, dict_Time);

            MessageBox.Show("Результати записані");
            this.Close();
        }

        private string uploads_dir2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Статистика");

        private void SaveWord(Dictionary<double, List<double>> dict_Kp, Dictionary<double, List<double>> dict_Ke,
            Dictionary<double, List<double>> dict_Keap, Dictionary<double, List<double>> dict_Time)
        {
            object fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Статистика\\Результат (шаблон).docx";
            Word._Application oWord = new Word.Application();
            Word._Document oDoc = oWord.Documents.Open(fileName);
            oWord.Visible = true;
            object oMissing = System.Reflection.Missing.Value;

            try
            {
                Word.Table oTable = oDoc.Tables[1];
                oTable.Rows.Add();
                Koeff(dict_Kp, oTable);

                oTable = oDoc.Tables[2];
                oTable.Rows.Add();
                Koeff(dict_Ke, oTable);

                oTable = oDoc.Tables[3];
                oTable.Rows.Add();
                Koeff(dict_Keap, oTable);

                oTable = oDoc.Tables[4];
                oTable.Rows.Add();
                Koeff(dict_Time, oTable);

                object fileName2 = uploads_dir2 + "//Результат.docx";
                object o = false;
                oDoc.SaveAs(ref fileName2,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref o, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }

            finally
            {
                object doNotSaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                oWord.Quit(ref doNotSaveChanges, ref oMissing, ref oMissing);
            }
        }

        public void Koeff(Dictionary<double, List<double>> dict, Word.Table oTable)
        {
            int i = 2;
            foreach (var coh in dict)
            {
                int m = 0;
                double alg_2_4 = 0;
                double alg_2_5 = 0;
                double alg_2_6 = 0;
                double alg_3_4 = 0;
                double alg_3_5 = 0;
                double alg_3_6 = 0;
                double alg_12_4 = 0;
                double alg_12_5 = 0;
                double alg_12_6 = 0;
                List<double> list_Keap = coh.Value;
                for (int j = 0; j < list_Keap.Count; j += 9)
                {
                    alg_2_4 += list_Keap[j];
                    alg_2_5 += list_Keap[j + 1];
                    alg_2_6 += list_Keap[j + 2];
                    alg_3_4 += list_Keap[j + 3];
                    alg_3_5 += list_Keap[j + 4];
                    alg_3_6 += list_Keap[j + 5];
                    alg_12_4 += list_Keap[j + 6];
                    alg_12_5 += list_Keap[j + 7];
                    alg_12_6 += list_Keap[j + 8];
                    m++;
                }

                alg_2_4 = alg_2_4 / m;
                alg_2_5 = alg_2_5 / m;
                alg_2_6 = alg_2_6 / m;
                alg_3_4 = alg_2_4 / m;
                alg_3_5 = alg_2_5 / m;
                alg_3_6 = alg_2_6 / m;
                alg_12_4 = alg_2_4 / m;
                alg_12_5 = alg_2_5 / m;
                alg_12_6 = alg_2_6 / m;

                oTable.Rows.Add();
                oTable.Cell(i, 1).Range.Text = (i - 1).ToString();
                oTable.Cell(i, 2).Range.Text = coh.Key.ToString();


                oTable.Cell(i, 3).Range.Text = alg_2_4.ToString("N2");
                oTable.Cell(i, 4).Range.Text = alg_2_5.ToString("N2");
                oTable.Cell(i, 5).Range.Text = alg_2_6.ToString("N2");
                oTable.Cell(i, 6).Range.Text = alg_3_4.ToString("N2");
                oTable.Cell(i, 7).Range.Text = alg_3_5.ToString("N2");
                oTable.Cell(i, 8).Range.Text = alg_3_6.ToString("N2");
                oTable.Cell(i, 9).Range.Text = alg_12_4.ToString("N2");
                oTable.Cell(i, 10).Range.Text = alg_12_5.ToString("N2");
                oTable.Cell(i, 11).Range.Text = alg_12_6.ToString("N2");
                oTable.Cell(i, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oTable.Cell(i, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                i++;
            }
            try
            {
                oTable.Rows[i + 1].Delete();
            }
            catch (Exception ex) { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Dictionary<double, List<double>> dict_KpDuplex = new Dictionary<double, List<double>>();
            //      Dictionary<int, List<double>> dict_KpLink = new Dictionary<int, List<double>>();
            Dictionary<double, List<double>> dict_TimeDuplex = new Dictionary<double, List<double>>();
            //        Dictionary<int, List<double>> dict_TimeLink = new Dictionary<int, List<double>>();
            int int_minCountTops = int.Parse(minCountTops.Text);
            int int_maxCountTops = int.Parse(maxCountTops.Text);
            int int_stepCountTops = int.Parse(stepCountTops.Text);
            int int_minConnected = Convert.ToInt32(double.Parse(minConnected.Text) * 100);
            int int_maxConnected = Convert.ToInt32(double.Parse(maxConnected.Text) * 100);
            int int_stepConnected = Convert.ToInt32(double.Parse(stepConnected.Text) * 100);
            int int_minWeightTops = int.Parse(minWeightTops.Text);
            int int_maxWeightTops = int.Parse(maxWeightTops.Text);
            int int_countGraphs = int.Parse(countGraphs.Text);
            int int_minCountLinks = int.Parse(minCountLinks.Text);
            int int_maxCountLinks = int.Parse(maxCountLinks.Text);


            Dictionary<double, Dictionary<int, List<double>>> dict_Kp = new Dictionary<double, Dictionary<int, List<double>>>();
            Dictionary<double, Dictionary<int, List<double>>> dict_Time = new Dictionary<double, Dictionary<int, List<double>>>();

            int k = 0;
            string str = "";


                 for (int i = int_minConnected; i <= int_maxConnected; i += int_stepConnected)
                 {
                     double coherence = Convert.ToDouble(i) / 100;
                     dict_KpDuplex[coherence] = new List<double>();
                     dict_TimeDuplex[coherence] = new List<double>();
                     k = 0;
                     for (int j = int_minCountTops; k < int_countGraphs; j += int_stepCountTops, k++)
                     {
                         _grView.ClearAll();
                         if (j > int_maxCountTops)
                             j = int_minCountTops;

                         _grView.GenerateGraph(j, int_minWeightTops, int_maxWeightTops, coherence);

                         _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, true, countLinks);
                         str += "Д Зв'язність " + coherence + "\n" + _destin_alg.test(2, 5, false);
                         dict_KpDuplex[coherence].Add(_destin_alg.Kp);
                         dict_TimeDuplex[coherence].Add(_destin_alg.Time);

                         _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, false, countLinks);
                         str += "Н Зв'язність " + coherence + "\n" + _destin_alg.test(2, 5, false);
                         dict_KpDuplex[coherence].Add(_destin_alg.Kp);
                         dict_TimeDuplex[coherence].Add(_destin_alg.Time);

                         _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, true, countLinks);
                         str += "Д Зв'язність " + coherence + "\n" + _destin_alg.test(2, 6, false);
                         dict_KpDuplex[coherence].Add(_destin_alg.Kp);
                         dict_TimeDuplex[coherence].Add(_destin_alg.Time);

                         _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, false, countLinks);
                         str += "Н Зв'язність " + coherence + "\n" + _destin_alg.test(2, 6, false);
                         dict_KpDuplex[coherence].Add(_destin_alg.Kp);
                         dict_TimeDuplex[coherence].Add(_destin_alg.Time);
                     }
                 }

       /*     string str2 = "";
            int l = 0;
            for (int i = int_minConnected; i <= int_maxConnected; i += int_stepConnected)
            {
                double coherence = Convert.ToDouble(i) / 100;
                dict_Kp[coherence] = new Dictionary<int, List<double>>();
                dict_Time[coherence] = new Dictionary<int, List<double>>();
                k = 0;

                Dictionary<int, List<double>> dict_newKp = new Dictionary<int, List<double>>();
                Dictionary<int, List<double>> dict_newTime = new Dictionary<int, List<double>>();
                for (l = int_minCountLinks; l <= int_maxCountLinks; l++)
                {
                    dict_newKp[l] = new List<double>();
                    dict_newTime[l] = new List<double>();
                }

                for (int j = int_minCountTops; k < int_countGraphs; j += int_stepCountTops, k++)
                {

                    _grView.ClearAll();
                    if (j > int_maxCountTops)
                        j = int_minCountTops;

                    _grView.GenerateGraph(j, int_minWeightTops, int_maxWeightTops, coherence);

                    for (l = int_minCountLinks; l <= int_maxCountLinks; l++)
                    {
                        _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, true, l);
                        str2 += "Зв'язність " + coherence + "\tlinks " + l + "\n" + _destin_alg.test(2, 5, false);
                        dict_newKp[l].Add(_destin_alg.Kp);
                        dict_newTime[l].Add(_destin_alg.Time);

                        _destin_alg = new DestinationAlgorithms(canvas, _grView, _grViewCS, propuskChannels, proizvodProcessors, true, l);
                        str2 += "Зв'язність " + coherence + "\tlinks " + l + "\n" + _destin_alg.test(2, 6, false);
                        dict_newKp[l].Add(_destin_alg.Kp);
                        dict_newTime[l].Add(_destin_alg.Time);
                    }
                }
    //            MessageBox.Show("");
                dict_Kp[coherence] = dict_newKp;
                dict_Time[coherence] = dict_newTime;
                
            }
            */

            FileStream fs = new FileStream("test.txt", FileMode.Create);
            StreamWriter w = new StreamWriter(fs, Encoding.UTF8);
            w.WriteLine(str);
            w.Flush();
            w.Close();
            fs.Close();

       /*     fs = new FileStream("test2.txt", FileMode.Create);
            w = new StreamWriter(fs, Encoding.UTF8);
            w.WriteLine(str2);
            w.Flush();
            w.Close();
            fs.Close();

            */

            SaveWord(dict_KpDuplex, dict_TimeDuplex, dict_Kp, dict_Time);

            MessageBox.Show("Результати записані");
        }

        private void SaveWord(Dictionary<double, List<double>> dict_KpDuplex, Dictionary<double, List<double>> dict_TimeDuplex,
            Dictionary<double, Dictionary<int, List<double>>> dict_Kp, Dictionary<double, Dictionary<int, List<double>>> dict_Time)
        {
            object fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Статистика\\Результат (шаблон - улюблені).docx";
            Word._Application oWord = new Word.Application();
            Word._Document oDoc = oWord.Documents.Open(fileName);
            oWord.Visible = true;
            object oMissing = System.Reflection.Missing.Value;

            try
            {
                Word.Table oTable = oDoc.Tables[1];
                oTable.Rows.Add();
                KoeffDuplex(dict_KpDuplex, oTable);

                oTable = oDoc.Tables[2];
                oTable.Rows.Add();
                KoeffDuplex(dict_TimeDuplex, oTable);

         /*       int k = 0;
                for (int l = 0; k < dict_Kp.Keys.Count; l += 2, k++)
                {
                    Dictionary<int, List<double>> dict_KpLink = new Dictionary<int, List<double>>();
                    Dictionary<int, List<double>> dict_TimeLink = new Dictionary<int, List<double>>();

                    dict_Kp.TryGetValue(dict_Kp.Keys.ElementAt(k), out dict_KpLink);
                    dict_Time.TryGetValue(dict_Time.Keys.ElementAt(k), out dict_TimeLink);

                    oTable = oDoc.Tables[l + 3];
                    oTable.Rows.Add();
                    KoeffLink(dict_KpLink, dict_Kp.Keys.ElementAt(k), oTable);

                    oTable = oDoc.Tables[l + 4];
                    oTable.Rows.Add();
                    KoeffLink(dict_TimeLink, dict_Kp.Keys.ElementAt(k), oTable);
                }*/

                object fileName2 = uploads_dir2 + "//Результат улюблені.docx";
                object o = false;
                oDoc.SaveAs(ref fileName2,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref o, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }

            finally
            {
                object doNotSaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                oWord.Quit(ref doNotSaveChanges, ref oMissing, ref oMissing);
            }
        }

        public void KoeffDuplex(Dictionary<double, List<double>> dict, Word.Table oTable)
        {
            int i = 2;

            for (int k = 0; k < dict.Keys.Count; k++)
            {
                int m = 0;
                double alg_2_5d = 0;
                double alg_2_5 = 0;
                double alg_2_6d = 0;
                double alg_2_6 = 0;
                List<double> list_Keap;
                dict.TryGetValue(dict.Keys.ElementAt(k), out list_Keap);
                for (int j = 0; j < list_Keap.Count; j += 4)
                {
                    alg_2_5d += list_Keap[j];
                    alg_2_5 += list_Keap[j + 1];
                    alg_2_6d += list_Keap[j + 2];
                    alg_2_6 += list_Keap[j + 3];
                    m++;
                }

                alg_2_5d = alg_2_5d / m;
                alg_2_5 = alg_2_5 / m;
                alg_2_6d = alg_2_6d / m;
                alg_2_6 = alg_2_6 / m;

                oTable.Rows.Add();
                oTable.Cell(i, 1).Range.Text = dict.Keys.ElementAt(k).ToString();
                oTable.Cell(i, 2).Range.Text = alg_2_5d.ToString("N2");
                oTable.Cell(i, 3).Range.Text = alg_2_5.ToString("N2");
                oTable.Cell(i, 4).Range.Text = alg_2_6d.ToString("N2");
                oTable.Cell(i, 5).Range.Text = alg_2_6.ToString("N2");
                oTable.Cell(i, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oTable.Cell(i, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                i++;
            }
            oTable.Rows[i].Delete();
        }

        public void KoeffLink(Dictionary<int, List<double>> dict, double coherence, Word.Table oTable)
        {
            int i = 2;

            for (int k = 0; k < dict.Keys.Count; k++)
            {
                int m = 0;
                double alg_2_5 = 0;
                double alg_2_6 = 0;
                List<double> list_Keap;
                dict.TryGetValue(dict.Keys.ElementAt(k), out list_Keap);
                for (int j = 0; j < list_Keap.Count; j += 2)
                {
                    alg_2_5 += list_Keap[j];
                    alg_2_6 += list_Keap[j + 1];
                    m++;
                }

                alg_2_5 = alg_2_5 / m;
                alg_2_6 = alg_2_6 / m;

                oTable.Rows.Add();
                oTable.Cell(i, 1).Range.Text = coherence.ToString();
                oTable.Cell(i, 2).Range.Text = dict.Keys.ElementAt(k).ToString();
                oTable.Cell(i, 3).Range.Text = alg_2_5.ToString("N2");
                oTable.Cell(i, 4).Range.Text = alg_2_6.ToString("N2");
                oTable.Cell(i, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oTable.Cell(i, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                i++;
            }
            oTable.Rows[i].Delete();
        }
    }
}