using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace TFLabelTool
{
    public partial class FormMain : Form
    {
        string ocFile_ = "oc.txt";               // 球员中心坐标文件
        string imagePath_ = "";                  // 标注图像路径
        string selectedpath = "";                //文件选择时的路径
        string absentPath = "";                 // absent标注文件所在位置
        string attributePath = "";              // attribute标注文件所在位置
        string groundtruthPath = "";            // groundtruth8标框文件所在位置
        string imagefoldername = "";               //存图片的那个文件夹的名字
        string labelPath = "";                  //存放标记信息的位置

        int zoom_ = 1;                           // 缩放比例  

        //string currentJPGName_ = "";             // 当前图片名称

        
        
        
        bool isreset_ = false;
        int prelistBoxFileIndex_ = -1;
        //bool islistBoxFileIndexChanged_ = false;

        // 构造函数
        public FormMain()
        {
            InitializeComponent();
        }

        // 主窗口加载
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (imagePath_ == "")
            {
                imagePath_ = String.Format("{0}\\{1}\\", Application.StartupPath, "image");
            }

            var items = selectedpath.Split('\\');
            int len = items.Length;
            if (len >= 3 && items[len - 2] == "sequences")//string.Compare(items[len - 2], "sequence") == 0
            {
                imagefoldername = items[len - 1] + ".txt";
                absentPath = "";
                attributePath = "";
                groundtruthPath = "";
                labelPath = "";
                for (int i = 0; i < len - 2 ; i++)
                {
                    absentPath = absentPath + items[i] + "\\";
                    attributePath = attributePath + items[i] + "\\";
                    groundtruthPath = groundtruthPath + items[i] + "\\";
                    labelPath = labelPath + items[i] + "\\";
                }
                absentPath += "anno\\absent\\";
                attributePath += "anno\\attribute\\";
                groundtruthPath += "anno\\groundtruth8\\";
                labelPath += "anno\\label\\";
                if (!Directory.Exists(absentPath))
                {
                    Directory.CreateDirectory(absentPath);
                }
                if (!Directory.Exists(attributePath))
                {
                    Directory.CreateDirectory(attributePath);
                }
                if (!Directory.Exists(groundtruthPath))
                {
                    Directory.CreateDirectory(groundtruthPath);
                }
                if (!Directory.Exists(labelPath))
                {
                    Directory.CreateDirectory(labelPath);
                }
            }
            else
            {
                MessageBox.Show("sequence目录必须有并且图片文件必须放在sequence/xx/目录下");
                return;// 目录结构不正确直接退出
            }


            if (!Directory.Exists(imagePath_))
            {
                Directory.CreateDirectory(imagePath_);
            }

            LoadFiles();            // 加载图片
            LoadGroundTruthFiles(); // 加载groundtruth.txt
            LoadLabelFiles();       // 加载label.txt

        }

        // 加载image路径下的所有图像
        void LoadFiles()
        {
            listBoxFiles.Items.Clear();
            var dir = new DirectoryInfo(imagePath_);
            foreach (var file in dir.GetFiles())
            {
                if (file.Extension == ".jpg")
                    listBoxFiles.Items.Add(file.FullName);
            }
        }

        // 加载groundtruth.txt
        private void LoadGroundTruthFiles()
        {
            var txt = groundtruthPath + imagefoldername;

            if (File.Exists(txt))   // 如果文件存在，直接返回
            {
                return;
            }

            string context = "0,0,0,0,0,0,0,0";

            // 初始化文件
            FileStream _file = new FileStream(@txt, FileMode.Create, FileAccess.ReadWrite);
            using (StreamWriter writer1 = new StreamWriter(_file))
            {
                for (int i = 0; i < listBoxFiles.Items.Count; ++i)
                {
                    writer1.WriteLine(context);
                }
                writer1.Flush();
                writer1.Close();

                _file.Close();
            }
        }

        // 加载label.txt
        private void LoadLabelFiles()
        {
            var txt = labelPath + imagefoldername;

            if (File.Exists(txt))   // 如果文件存在，直接返回
            {
                return;
            }

            // 初始化文件
            FileStream _file = new FileStream(@txt, FileMode.Create, FileAccess.ReadWrite);
            using (StreamWriter writer1 = new StreamWriter(_file))
            {
                for (int i = 0; i < listBoxFiles.Items.Count; ++i)
                {
                    writer1.WriteLine("0,0,0,0,0,0,0,0,0");
                }
                writer1.Flush();
                writer1.Close();

                _file.Close();
            }
        }


        // 保存groundtruth.txt
        void SaveGroundTruthFile()
        {
            var txt = groundtruthPath + imagefoldername;
            File.Delete(txt);
            var content = "";
            foreach (var item in listBoxLable.Items)    // 从listBoxLabel控件中加载信息
            {
                content += item + "\n";
            }
            File.AppendAllText(txt, content.Trim());
        }

        private Bitmap ZoomImage(Bitmap bitmap, int destHeight, int destWidth)
        {
            try
            {
                System.Drawing.Image sourImage = bitmap;
                int width = 0, height = 0;
                //按比例缩放             
                int sourWidth = sourImage.Width;
                int sourHeight = sourImage.Height;
                if (sourHeight > destHeight || sourWidth > destWidth)
                {
                    if ((sourWidth * destHeight) > (sourHeight * destWidth))
                    {
                        width = destWidth;
                        height = (destWidth * sourHeight) / sourWidth;
                    }
                    else
                    {
                        height = destHeight;
                        width = (sourWidth * destHeight) / sourHeight;
                    }
                }
                else
                {
                    width = sourWidth;
                    height = sourHeight;
                }
                Bitmap destBitmap = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(destBitmap);
                g.Clear(Color.Transparent);
                //设置画布的描绘质量           
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(sourImage, new Rectangle((destWidth - width) / 2, (destHeight - height) / 2, width, height), 0, 0, sourImage.Width, sourImage.Height, GraphicsUnit.Pixel);
                g.Dispose();
                //设置压缩质量       
                System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                sourImage.Dispose();
                return destBitmap;
            }
            catch
            {
                return bitmap;
            }
        }



        // listBoxFiles控件选择文件时触发
        private void listBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (isreset_)
            {
                zoom_ = 1;
                prelistBoxFileIndex_ = this.listBoxFiles.SelectedIndex;
            }
            
            isreset_ = false;
            
           
            
            label7.Text = "";

            if (listBoxFiles.SelectedItem != null)
            {
                var jpgPath = listBoxFiles.SelectedItem.ToString();
                pictureBox1.ImageLocation = jpgPath;
                pictureBox1.Load(jpgPath);

                pictureBox1.Width = pictureBox1.Image.Width * zoom_;
                pictureBox1.Height = pictureBox1.Image.Height * zoom_;


                // 刷新listBoxLabel 标注信息
                listBoxLable.Items.Clear();
                var txt = groundtruthPath + imagefoldername;
                if (File.Exists(txt))
                {
                    int cnt = 0;
                    foreach (var item in File.ReadAllLines(txt.Trim()))
                    {
                        var cur = item.ToString().Split(',');
                        if (cnt++ == listBoxFiles.SelectedIndex && listBoxFiles.SelectedIndex != 0
                            && (Convert.ToInt32(cur[0]) == 0 && Convert.ToInt32(cur[1]) == 0 && Convert.ToInt32(cur[2]) == 0 && Convert.ToInt32(cur[3]) == 0))
                        {
                            var preitem = listBoxLable.Items[listBoxFiles.SelectedIndex - 1];
                            var it = preitem.ToString().Split(',');
                            if ((Convert.ToInt32(it[0]) != 0 || Convert.ToInt32(it[1]) != 0) && !checkBox6.Checked && !checkBox10.Checked)
                            {
                                listBoxLable.Items.Add(listBoxLable.Items[listBoxFiles.SelectedIndex - 1]);
                            }
                            else
                            {
                                listBoxLable.Items.Add(item);
                            }
                        }
                        else
                        {
                            listBoxLable.Items.Add(item);
                        } 
                    }

                    SaveGroundTruthFile();
                   

                }

                ClearSelect();  // 删除之前选中的 重新画图
                if (listBoxLable.Items.Count > listBoxFiles.SelectedIndex)
                {
                    listBoxLable.SelectedIndex = listBoxFiles.SelectedIndex;
                    listBoxLable_MouseClick(null, null);
                }

                LoadLabel(listBoxFiles.SelectedIndex);
            }
        }

        private Point RectStartPoint;
        private Point[] pointCorner = new Point[4];//存储矩形的四个顶点
        private Point[] Rect1 = new Point[4];
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(50, 72, 145, 220));

        private List<Point> OcPoints = new List<Point>();

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // 画框
            
                RectStartPoint = e.Location;
                Invalidate();
            
        }

        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {


            if (e.Button != MouseButtons.Left)//判断是否按下左键
                return;
                    
            Point tempEndPoint = e.Location; //记录框的位置和大小
            Rect1[0].X = RectStartPoint.X;
            Rect1[0].Y = RectStartPoint.Y;
            Rect1[1].X = tempEndPoint.X;
            Rect1[1].Y = RectStartPoint.Y;
            Rect1[2].X = tempEndPoint.X;
            Rect1[2].Y = tempEndPoint.Y;
            Rect1[3].X = RectStartPoint.X;
            Rect1[3].Y = tempEndPoint.Y;

            pictureBox1.Invalidate();
            
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                
                if ((Rect1[0] != null && Rect1[0].X > 0 && Rect1[0].Y > 0))
                {
                    if (Rect1[0] != null && Rect1[0].X > 0 && Rect1[0].Y > 0)
                    {

                        e.Graphics.DrawPolygon(new Pen(Color.Red, 3), Rect1);//重新绘制颜色为红色
            
                    }
                }              
            }
        }


        void ClearSelect()
        {
            for (int i = 0; i < 4; i++)
            {
                Rect1[i].Y = 0;
                Rect1[i].X = 0;
            }
            
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
                if (e.Button == MouseButtons.Left)
                {
                    if (RectStartPoint == e.Location)
                    {
                        return;
                    }

                    if (RectStartPoint.X > e.Location.X || RectStartPoint.Y > e.Location.Y)
                    {
                        MessageBox.Show("只能从左上向右下选");
                        ClearSelect();
                        return;
                    }

                    var height = e.Location.Y - RectStartPoint.Y;
                    var width = e.Location.X - RectStartPoint.X;
                    pointCorner[0] = new Point(RectStartPoint.X, RectStartPoint.Y);
                    pointCorner[1] = new Point(e.Location.X, RectStartPoint.Y);
                    pointCorner[2] = new Point(e.Location.X, e.Location.Y);
                    pointCorner[3] = new Point(RectStartPoint.X, e.Location.Y);
                    if (listBoxLable.Items.Count > 0)
                    {
                        if (listBoxLable.Items.Count < listBoxFiles.SelectedIndex)
                        {
                            MessageBox.Show("需要按图片顺序进行标注！");
                            ClearSelect();
                            return;
                        }
                        if (listBoxLable.Items.Count > listBoxFiles.SelectedIndex)
                            listBoxLable.Items.RemoveAt(listBoxFiles.SelectedIndex);
                        // TODO
                        if (listBoxLable.Items.Count >= listBoxFiles.SelectedIndex)
                            listBoxLable.Items.Insert(listBoxFiles.SelectedIndex, String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", pointCorner[0].X / zoom_, pointCorner[0].Y / zoom_, pointCorner[1].X / zoom_, pointCorner[1].Y / zoom_,pointCorner[2].X / zoom_, pointCorner[2].Y / zoom_,pointCorner[3].X / zoom_, pointCorner[3].Y / zoom_));
                    }

                    SaveGroundTruthFile();
              
                    listBoxFiles_SelectedIndexChanged(null, null);
                }
            
        }



        private void listBoxFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listBoxFiles.SelectedItems != null)
                {
                    if (listBoxFiles.SelectedItems.Count == 1)
                    {
                        FileInfo fi = new FileInfo(listBoxFiles.SelectedItem.ToString());
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false;
                        }
                        File.Delete(listBoxFiles.SelectedItem.ToString());
                        File.Delete(listBoxFiles.SelectedItem.ToString().Replace(".jpg", ".txt"));
                        listBoxLable.Items.Clear();
                        int i = listBoxFiles.SelectedIndex;
                        listBoxFiles.Items.RemoveAt(listBoxFiles.SelectedIndex);
                        if (i < listBoxFiles.Items.Count)
                        {
                            listBoxFiles.SelectedIndex = i;
                        }
                    }
                    else
                    if (listBoxFiles.SelectedItems.Count == 0)
                    {
                        return;
                    }
                    else
                    {

                        foreach (var item in listBoxFiles.SelectedItems)
                        {
                            FileInfo fi = new FileInfo(item.ToString());
                            if (fi.IsReadOnly)
                            {
                                fi.IsReadOnly = false;
                            }
                            File.Delete(item.ToString());
                            File.Delete(item.ToString().Replace(".jpg", ".txt"));
                            listBoxLable.Items.Clear();
                        }
                        listBoxFiles.Items.Clear();

                    }

                }
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
               selectedpath = dilog.SelectedPath;
               imagePath_ = dilog.SelectedPath + "\\";
               FormMain_Load(null, null);
            }
            

        }

        void ImportFiles(string path)
        {
            if (Directory.Exists(path))
            {
                var importDir = new DirectoryInfo(path);
                foreach (var file in importDir.GetFiles())
                {
                    if (file.Extension == ".jpg")
                    {
                        string desFilePath = imagePath_ + file.Name;
                        if (!File.Exists(desFilePath))
                        {
                            var image = Bitmap.FromFile(file.FullName);
                            var f = ZoomImage(new Bitmap(image), (int)numericUpDownImportHeight.Value, (int)numericUpDownImportHeight.Value);
                            f.Save(imagePath_ + file.Name, System.Drawing.Imaging.ImageFormat.Jpeg);
                            image.Dispose();

                            listBoxFiles.Items.Add(imagePath_ + file.Name);
                        }

                    }
                }
            }

        }

        private void listBoxLable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxLable.SelectedIndex != -1)
            {
                var line = listBoxLable.SelectedItem.ToString();
                if (line != "")
                {
                    var items = line.Split(',');
                    MessageBox.Show(String.Format("x1={0},y1={1}\nx2={2},y2={3}\nx3={4},y3={5}\nx4={6},y4={7}\n",
                        items[0], items[1], items[2], items[3],items[4], items[5], items[6], items[7]), "样本标注信息");
                }
            }
        }

        private void listBoxLable_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxLable.SelectedIndex != -1)
            {
                var line = listBoxLable.SelectedItem.ToString();
                if (line != "")
                {
                    var items = line.Split(',');
                    var x1 = Convert.ToDouble(items[0]);
                    var y1 = Convert.ToDouble(items[1]);
                    var x2 = Convert.ToDouble(items[2]);
                    var y2 = Convert.ToDouble(items[3]);
                    var x3 = Convert.ToDouble(items[4]);
                    var y3 = Convert.ToDouble(items[5]);
                    var x4 = Convert.ToDouble(items[6]);
                    var y4 = Convert.ToDouble(items[7]);



                    Rect1[0].X = (int)x1 * zoom_;
                    Rect1[0].Y = (int)y1 * zoom_;
                    Rect1[1].X = (int)x2 * zoom_;
                    Rect1[1].Y = (int)y2 * zoom_;
                    Rect1[2].X = (int)x3 * zoom_;
                    Rect1[2].Y = (int)y3 * zoom_;
                    Rect1[3].X = (int)x4 * zoom_;
                    Rect1[3].Y = (int)y4 * zoom_;

                    pictureBox1.Invalidate();

                }
            }
        }

       
        private void buttonOpenImageFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", imagePath_);
        }


        // 处理一些键盘操作
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData != Keys.NumPad8 && keyData != Keys.NumPad5 && keyData != Keys.NumPad4 && keyData != Keys.NumPad6
                && keyData != Keys.S && keyData != Keys.Z && keyData != Keys.X && keyData != Keys.C && keyData != Keys.Up && keyData != Keys.Down && keyData != Keys.Left && keyData != Keys.Right && keyData != Keys.Q && keyData != Keys.W)
            {
                return base.ProcessCmdKey(ref msg, keyData); ;
            }

            if (listBoxLable.SelectedIndex != -1)
            {
                var line = listBoxLable.SelectedItem.ToString();
                if (line != "")
                {
                    var items = line.Split(',');



                    double x1 = Convert.ToDouble(items[0]);
                    double y1 = Convert.ToDouble(items[1]);
                    double x2 = Convert.ToDouble(items[2]);
                    double y2 = Convert.ToDouble(items[3]);
                    double x3 = Convert.ToDouble(items[4]);
                    double y3 = Convert.ToDouble(items[5]);
                    double x4 = Convert.ToDouble(items[6]);
                    double y4 = Convert.ToDouble(items[7]);
                    double x0 = (x1 + x3) / 2;
                    double y0 = (y1 + y3) / 2;
                    double x02 = (x2 + x4) / 2;
                    double y02 = (y2 + y4) / 2;//求矩形中心，求两个是因为减少因小数取整导致的误差。

                    double moveSteps = 2;

                    switch (keyData)
                    {
                        case Keys.NumPad8:
                            { 
                                y1 -= moveSteps;
                                y2 -= moveSteps;
                                y3 -= moveSteps;
                                y4 -= moveSteps;
                            } 
                            break;     // 上
                        case Keys.NumPad5:
                            { 
                                y1 += moveSteps;
                                y2 += moveSteps;
                                y3 += moveSteps;
                                y4 += moveSteps;
                            } 
                            break;     // 下
                        case Keys.NumPad4: 
                            { 
                                x1 -= moveSteps;
                                x2 -= moveSteps;
                                x3 -= moveSteps;
                                x4 -= moveSteps;
                            } 
                            break;      // 左 
                        case Keys.NumPad6: 
                            { 
                                x1 += moveSteps;
                                x2 += moveSteps;
                                x3 += moveSteps;
                                x4 += moveSteps;
                            } 
                            break;      // 右
                        case Keys.S: 
                            { 
                                x4 = x4 - 2 * (x4 - x1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));//两倍扩大
                                y4 = y4 - 2 * (y4 - y1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                x3 = x3 - 2 * (x3 - x2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                                y3 = y3 - 2 * (y3 - y2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                            } 
                            break;// w
                        case Keys.X: 
                            {  
                                x4 = x4 + 2 * (x4 - x1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));//两倍扩大
                                y4 = y4 + 2 * (y4 - y1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                x3 = x3 + 2 * (x3 - x2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                                y3 = y3 + 2 * (y3 - y2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                            } 
                            break;     // s
                        case Keys.Z: 
                            { 
                                x2 = x2 - 2 * (x2 - x1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));//两倍扩大
                                y2 = y2 - 2 * (y2 - y1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                x3 = x3 - 2 * (x3 - x4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                                y3 = y3 - 2 * (y3 - y4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                            } 
                            break;      // a 
                        case Keys.C: 
                            { 
                                x2 = x2 + 2 * (x2 - x1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));//两倍扩大
                                y2 = y2 + 2 * (y2 - y1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                x3 = x3 + 2 * (x3 - x4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                                y3 = y3 + 2 * (y3 - y4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                            } 
                            break;      // d
                        case Keys.Q:
                            {
                                double temp_x = x1;
                                double adjustAngle = 3;
                                x1 = (x1 - x0) * Math.Cos(-Math.PI / 180.0 * adjustAngle) - (y1 - y0) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + x0; // 10表示每次调整10°
                                y1 = (temp_x - x0) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + (y1 - y0) * Math.Cos(-Math.PI / 180.0 * adjustAngle) + y0; //
                                temp_x = x2;
                                x2 = (x2 - x02) * Math.Cos(-Math.PI / 180.0 * adjustAngle) - (y2 - y02) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + x02; //
                                y2 = (temp_x - x02) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + (y2 - y02) * Math.Cos(-Math.PI / 180.0 * adjustAngle) + y02; //
                                temp_x = x3;
                                x3 = (x3 - x0) * Math.Cos(-Math.PI / 180.0 * adjustAngle) - (y3 - y0) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + x0; //
                                y3 = (temp_x - x0) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + (y3 - y0) * Math.Cos(-Math.PI / 180.0 * adjustAngle) + y0; //
                                temp_x = x4;
                                x4 = (x4 - x02) * Math.Cos(-Math.PI / 180.0 * adjustAngle) - (y4 - y02) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + x02; //
                                y4 = (temp_x - x02) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + (y4 - y02) * Math.Cos(-Math.PI / 180.0 * adjustAngle) + y02; //
                            }
                            break;//逆时针旋转
                        case Keys.W:
                            {
                                double temp_x = x1;
                                double adjustAngle = 3;
                                x1 = (x1 - x0) * Math.Cos(Math.PI / 180.0 * adjustAngle) - (y1 - y0) * Math.Sin(Math.PI / 180.0 * adjustAngle) + x0; // 10表示每次调整10°
                                y1 = (temp_x - x0) * Math.Sin(Math.PI / 180.0 * adjustAngle) + (y1 - y0) * Math.Cos(Math.PI / 180.0 * adjustAngle) + y0; //
                                temp_x = x2;
                                x2 = (x2 - x02) * Math.Cos(Math.PI / 180.0 * adjustAngle) - (y2 - y02) * Math.Sin(Math.PI / 180.0 * adjustAngle) + x02; //
                                y2 = (temp_x - x02) * Math.Sin(Math.PI / 180.0 * adjustAngle) + (y2 - y02) * Math.Cos(Math.PI / 180.0 * adjustAngle) + y02; //
                                temp_x = x3;
                                x3 = (x3 - x0) * Math.Cos(Math.PI / 180.0 * adjustAngle) - (y3 - y0) * Math.Sin(Math.PI / 180.0 * adjustAngle) + x0; //
                                y3 = (temp_x - x0) * Math.Sin(Math.PI / 180.0 * adjustAngle) + (y3 - y0) * Math.Cos(Math.PI / 180.0 * adjustAngle) + y0; //
                                temp_x = x4;
                                x4 = (x4 - x02) * Math.Cos(Math.PI / 180.0 * adjustAngle) - (y4 - y02) * Math.Sin(Math.PI / 180.0 * adjustAngle) + x02; //
                                y4 = (temp_x - x02) * Math.Sin(Math.PI / 180.0 * adjustAngle) + (y4 - y02) * Math.Cos(Math.PI / 180.0 * adjustAngle) + y02; //
                            }
                            break;//顺时针旋转
                        case Keys.Up:
                            if (this.listBoxFiles.SelectedIndex > 0)
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex - 1;
                            }
                            else
                            {
                                this.listBoxFiles.SelectedIndex = 0;
                            }
                            break;
                        case Keys.Down:
                            if (this.listBoxFiles.SelectedIndex < this.listBoxFiles.Items.Count - 1)
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex + 1;
                            }
                            else
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.Items.Count - 1;
                            }
                            break;
                        case Keys.Left:
                            if (this.listBoxFiles.SelectedIndex > 0)
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex - 1;
                            }
                            else
                            {
                                this.listBoxFiles.SelectedIndex = 0;
                            }
                            break;
                        case Keys.Right:
                            if (this.listBoxFiles.SelectedIndex < this.listBoxFiles.Items.Count - 1)
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex + 1;
                            }
                            else
                            {
                                this.listBoxFiles.SelectedIndex = this.listBoxFiles.Items.Count - 1;
                            }
                            break;
                    }

                    // 如果有小数就取整
                    x1 = Math.Round(x1);
                    y1 = Math.Round(y1);
                    x2 = Math.Round(x2);
                    y2 = Math.Round(y2);
                    x3 = Math.Round(x3);
                    y3 = Math.Round(y3);
                    x4 = Math.Round(x4);
                    y4 = Math.Round(y4);

                    // 临界值判断
                    int widthTmp = pictureBox1.Image.Width;
                    int heightTmp = pictureBox1.Image.Height;

                    if (((x1 <= 0 || x2 <= 0 || x3 <= 0 || x4 <= 0 || y1 <= 0 || y2 <= 0 || y3 <= 0 || y4 <= 0 ) && !(x1 == 0 && x2 == 0))|| x1 >= widthTmp || x2 >= widthTmp || x3 >= widthTmp || x4 >= widthTmp || y1 >= heightTmp || y2 >= heightTmp || y3 >= heightTmp || y4 >= heightTmp)
                    {
                        MessageBox.Show("标框出界！");
                        return false;
                    }

                    if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                    {
                        listBoxFiles_SelectedIndexChanged(null, null);
                        return base.ProcessCmdKey(ref msg, keyData); ;
                    }

                    if (listBoxLable.Items.Count > listBoxFiles.SelectedIndex)
                        listBoxLable.Items.RemoveAt(listBoxFiles.SelectedIndex);
                    listBoxLable.Items.Insert(listBoxFiles.SelectedIndex, String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", x1, y1, x2, y2, x3, y3, x4, y4));
                    SaveGroundTruthFile();
                    
                  
                    listBoxFiles_SelectedIndexChanged(null, null);
                }
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // 
        private void scaleRect()
        {
            if (listBoxFiles.SelectedIndex != -1)
            {
                this.listBoxLable.SelectedIndex = this.listBoxFiles.SelectedIndex;
                var line = listBoxLable.SelectedItem.ToString();
                if (line != "")
                {
                    var items = line.Split(',');

                    var x1 = Convert.ToDouble(items[0]);
                    var y1 = Convert.ToDouble(items[1]);
                    var x2 = Convert.ToDouble(items[2]);
                    var y2 = Convert.ToDouble(items[3]);
                    var x3 = Convert.ToDouble(items[4]);
                    var y3 = Convert.ToDouble(items[5]);
                    var x4 = Convert.ToDouble(items[6]);
                    var y4 = Convert.ToDouble(items[7]);

                    Rect1[0].X = (int)x1 * zoom_;
                    Rect1[0].Y = (int)y1 * zoom_;
                    Rect1[1].X = (int)x2 * zoom_;
                    Rect1[1].Y = (int)y2 * zoom_;
                    Rect1[2].X = (int)x3 * zoom_;
                    Rect1[2].Y = (int)y3 * zoom_;
                    Rect1[3].X = (int)x4 * zoom_;
                    Rect1[3].Y = (int)y4 * zoom_;

                    pictureBox1.Invalidate();

                }
            }
        }

        private void enlarge_Click(object sender, EventArgs e)
        {
            zoom_ *= 2;
            pictureBox1.Width = pictureBox1.Width * 2;
            pictureBox1.Height = pictureBox1.Height * 2;
            ClearSelect();  // 删除之前选中的   
            scaleRect();
        }

        private void reduce_Click(object sender, EventArgs e)
        {
            if (zoom_ > 1)
            {
                pictureBox1.Width = pictureBox1.Width / 2;
                pictureBox1.Height = pictureBox1.Height / 2;
                zoom_ /= 2;
                scaleRect();  
            }
            if (zoom_ < 1)
            {
                zoom_ = 1;
                scaleRect();
            }
        }
        // 按位置修改label
        void SaveLabelFile(string s,string status)
        {
            var txt = labelPath + imagefoldername;
            if (!File.Exists(txt))
            {
                File.Create(txt);
            }
            int counter = 0;
            string context = "";
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(txt);
            while ((line = file.ReadLine()) != null)
            {
                if (counter == listBoxFiles.SelectedIndex)
                {
                    var temps = line.Trim().Split(',');
                    int len = temps.Length;
                    string res = "";
                    temps[Convert.ToInt32(s)] = status;
                    for (int i = 0; i < len; i++)
                    {
                        if (i == len - 1)
                            res += temps[i];
                        else
                            res = res +temps[i] + ",";
                    }
                    context += res + "\r\n";
                }
                else
                    context += line + "\r\n";
                counter++;
            }

            while (counter++ < listBoxFiles.Items.Count)
            {
                context += "0,0,0,0,0,0,0,0,0\r\n";
            }

            file.Close();
            if (context == "")
            {
                File.WriteAllText(txt, s);
            }
            else
            {
                File.WriteAllText(txt, context);
            }

        }
 
        //整行修改label文件
        void SaveLabelFile2(string s)
        {
            var txt = labelPath + imagefoldername;
            if (!File.Exists(txt))
            {
                File.Create(txt);
            }
            int counter = 0;
            string context = "";
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(txt);
            while ((line = file.ReadLine()) != null)
            {
                //System.Console.WriteLine(line);
                if (counter == listBoxFiles.SelectedIndex)
                {
                    context += s + "\r\n";
                }
                else
                    context += line + "\r\n";
                counter++;
            }

            while (counter++ < listBoxFiles.Items.Count)
            {
                context += "0,0,0,0,0,0,0,0,0\r\n";
            }

            file.Close();
            if (context == "")
            {
                File.WriteAllText(txt, s);
            }
            else
            {
                File.WriteAllText(txt, context);
            }

        }





        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox1.Checked){
                SaveLabelFile("0", "1");
            }else{
                SaveLabelFile("0", "0");
            }
            genFile_Click(null, null);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox2.Checked){
                SaveLabelFile("1", "1");
            }else{
                SaveLabelFile("1", "0");
            }
            genFile_Click(null, null);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox3.Checked){
                SaveLabelFile("2", "1");
            }else{
                SaveLabelFile("2", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            
            if (this.checkBox5.Checked){
                if (checkBox6.Checked || checkBox10.Checked)
                {
                    checkBox5.CheckState = CheckState.Unchecked;
                    MessageBox.Show("完全遮挡或者球员消失不能勾选部分遮挡");
                    return;
                }
                SaveLabelFile("3", "1");
            }else{
                SaveLabelFile("3", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox6.Checked){
                if(checkBox5.Checked || checkBox10.Checked)
                {
                    checkBox6.CheckState = CheckState.Unchecked;
                    MessageBox.Show("勾选部分遮挡或者球员消失不能勾选完全遮挡");
                    return;
                }
                clearBox_Click(null, null);// 清除框选
                SaveLabelFile("4", "1");
            }else{
                SaveLabelFile("4", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox7.Checked){
                SaveLabelFile("5", "1");
            }else{
                SaveLabelFile("5", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox8.Checked){
                SaveLabelFile("6", "1");
            }else{
                SaveLabelFile("6", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox9.Checked){
                SaveLabelFile("7", "1");
            }else{
                SaveLabelFile("7", "0");
            }
            genFile_Click(null, null);
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.checkBox10.Checked){
                if (checkBox5.Checked || checkBox6.Checked)
                {
                    checkBox10.CheckState = CheckState.Unchecked;
                    MessageBox.Show("勾选部分遮挡或完全遮挡不能勾选球员消失");
                    return;
                }
                clearBox_Click(null, null);
                SaveLabelFile("8", "1");
            }else{
                SaveLabelFile("8", "0");
            }
            genFile_Click(null, null);
        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return;
            }
            if (this.radioButton4.Checked)
            {
                SaveLabelFile("3","1");
            }
        }

        // 加载Label选项
        private void LoadLabel(int index)
        {
            var txt = labelPath + imagefoldername;
            if (!File.Exists(txt))
            {
                File.Create(txt);
            }

            string line = "";
            int counter = 0;
            System.IO.StreamReader file =
               new System.IO.StreamReader(txt);
            while ((line = file.ReadLine()) != null)
            {
                if (counter == index)
                {
                    break;
                }
                counter++;
            }

            file.Close();
            if (line != null && line != "")
            {
                SaveLabelFile2(line);
                var status = line.Split(',');
                if (status[0] == "0")
                {
                    checkBox1.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox1.CheckState = CheckState.Checked;
                }
                if (status[1] == "0")
                {
                    checkBox2.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox2.CheckState = CheckState.Checked;
                }
                if (status[2] == "0")
                {
                    checkBox3.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox3.CheckState = CheckState.Checked;
                }
                if (status[3] == "0")
                {
                    checkBox5.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox5.CheckState = CheckState.Checked;
                }
                if (status[4] == "0")
                {
                    checkBox6.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox6.CheckState = CheckState.Checked;
                }
                if (status[5] == "0")
                {
                    checkBox7.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox7.CheckState = CheckState.Checked;
                }
                if (status[6] == "0")
                {
                    checkBox8.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox8.CheckState = CheckState.Checked;
                }
                if (status[7] == "0")
                {
                    checkBox9.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox9.CheckState = CheckState.Checked;
                }
                if (status[8] == "0")
                {
                    checkBox10.CheckState = CheckState.Unchecked;
                }
                else
                {
                    checkBox10.CheckState = CheckState.Checked;
                }
 
            }
            else
            {
                this.checkBox1.Select();
                SaveLabelFile("0" , "1");//目标行没有数据默认为无遮挡
            }
        }

        
        private void reset_Click(object sender, EventArgs e)
        {
            isreset_ = true;
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void clearBox_Click(object sender, EventArgs e)
        {
            // 刷新listBoxLabel 标注信息
            listBoxLable.Items.Clear();
            var txt = groundtruthPath + imagefoldername;
            if (File.Exists(txt))
            {
                int cnt = 0;
                foreach (var item in File.ReadAllLines(txt.Trim()))
                {
                    if (cnt++ == listBoxFiles.SelectedIndex)
                    {
                        listBoxLable.Items.Add("0,0,0,0,0,0,0,0");
                    }
                    else
                    {
                        listBoxLable.Items.Add(item);
                    } 
                }
    
                SaveGroundTruthFile();
               
    
            }
            ClearSelect();
        }

        // 
        private void genFile_Click(object sender, EventArgs e)
        {
            var txt = labelPath + imagefoldername;
            if (labelPath == "" || imagefoldername == "" || !File.Exists(txt))
            {
                MessageBox.Show("请先选择图像数据集");
                return;
            }
            string line;
            string absent = "";
            int[] NumArr = new int[8]{0,0,0,0,0,0,0,0};
            string attribute = "";
            System.IO.StreamReader file = new System.IO.StreamReader(txt);
            while ((line = file.ReadLine()) != null)
            {
                var items = line.Trim().Split(',');
                absent = absent + items[4] + "," + items[8] + "\r\n";
                for (int i = 1; i < 9; i++)
                {
                    if (items[i] == "1")
                        NumArr[i - 1]++;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                if (NumArr[i] != 0)
                {
                    if (i != 7)
                        attribute += "1,";
                    else
                        attribute += "1";
                }
                else
                {
                    if (i != 7)
                        attribute += "0,";
                    else
                        attribute += "0";
                }

            }
            file.Close();
            string absentfile = absentPath + imagefoldername;
            string attributefile = attributePath + imagefoldername;
            if (File.Exists(absentfile))
                File.Delete(absentfile);
            if (File.Exists(attributefile))
                File.Delete(attributefile);
            File.AppendAllText(absentfile, absent);
            File.AppendAllText(attributefile, attribute);
        }



        private void AdjustX_ValueChanged(object sender, EventArgs e)
        {
            
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void AdjustY_ValueChanged(object sender, EventArgs e)
        {
            
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void AdjustHeight_ValueChanged(object sender, EventArgs e)
        {
            
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void AdjustWidth_ValueChanged(object sender, EventArgs e)
        {
            
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void preListBoxFile_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedIndex > 0 && listBoxFiles.SelectedIndex < listBoxFiles.Items.Count)
            {
                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex - 1;
            }
            else
            {
                this.listBoxFiles.SelectedIndex = 0;
            }
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        private void nextListBoxFile_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedIndex >= 0 && listBoxFiles.SelectedIndex < listBoxFiles.Items.Count-1)
            {
                this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex + 1;
            }
            else
            {
                this.listBoxFiles.SelectedIndex = listBoxFiles.Items.Count-1;
            }
            listBoxFiles_SelectedIndexChanged(null, null);
        }


        // 表格关闭情况下的一些操作
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.checkBox2.Checked || this.checkBox3.Checked || this.radioButton4.Checked)
            {
                if (OcPoints.Count == 0)
                {
                    this.listBoxFiles.SelectedIndex = prelistBoxFileIndex_;
                    MessageBox.Show("存在遮挡的情况下必须描点");
                    e.Cancel = true;
                }
            }
        }
    }
}
