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
        string ocFile_ = "oc.txt";               
        string imagePath_ = "";                  
        string selectedpath = "";                
        string absentPath = "";                 
        string attributePath = "";              
        string groundtruthPath = "";            
        string imagefoldername = "";               
        string labelPath = "";                  

        int zoom_ = 1;                           // scale

        //string currentJPGName_ = "";             

        
        
        
        bool isreset_ = false;
        int prelistBoxFileIndex_ = -1;
        //bool islistBoxFileIndexChanged_ = false;

        // Constructor
        public FormMain()
        {
            InitializeComponent();
        }

        // Main window loading
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
                MessageBox.Show("The sequence directory must exist and the picture file must be placed in the sequence/xx/ directory");
                return;// The directory structure is incorrect and exit directly
            }


            if (!Directory.Exists(imagePath_))
            {
                Directory.CreateDirectory(imagePath_);
            }

            LoadFiles();            // Load picture
            LoadGroundTruthFiles(); // load groundtruth.txt
            LoadLabelFiles();       // load label.txt

        }

        // Load all images in the image path
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

        // load groundtruth.txt
        private void LoadGroundTruthFiles()
        {
            var txt = groundtruthPath + imagefoldername;

            if (File.Exists(txt))   // If the file exists, return directly
            {
                return;
            }

            string context = "0,0,0,0,0,0,0,0";

            // Initialization files
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

        // load label.txt
        private void LoadLabelFiles()
        {
            var txt = labelPath + imagefoldername;

            if (File.Exists(txt))   // If the file exists, return directly
            {
                return;
            }

            //Initialization file
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


        // save groundtruth.txt
        void SaveGroundTruthFile()
        {
            var txt = groundtruthPath + imagefoldername;
            File.Delete(txt);
            var content = "";
            foreach (var item in listBoxLable.Items)    // Load information from the listBoxLabel control
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
                //Scaling             
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
                //Set the drawing quality of the canvas           
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(sourImage, new Rectangle((destWidth - width) / 2, (destHeight - height) / 2, width, height), 0, 0, sourImage.Width, sourImage.Height, GraphicsUnit.Pixel);
                g.Dispose();
                //Set compression quality     
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



        private int maxNum(int a, int b, int c, int d)
        {
            int m = a;
            if (m < b) {m = b;}
            if (m < c) {m = c;}
            if (m < d) {m = d;}
            return m;
        }
        private int minNum(int a, int b, int c, int d)
        {
            int m = a;
            if (m > b) {m = b;}
            if (m > c) {m = c;}
            if (m > d) {m = d;}
            return m;
        }
        // Triggered when the listBoxFiles control selects a file
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

                this.cFrameLabel.Text = "Current Frame:" + Convert.ToString(listBoxFiles.SelectedIndex + 1);

                // Refresh label information of listBoxLabel
                listBoxLable.Items.Clear();
                var txt = groundtruthPath + imagefoldername;
                if (File.Exists(txt))
                {
                    int cnt = 0;
                    foreach (var item in File.ReadAllLines(txt.Trim()))
                    {
                        var temp = item;
                        var cur = item.ToString().Split(',');
                        if (cnt++ == listBoxFiles.SelectedIndex && listBoxFiles.SelectedIndex != 0
                            && (Convert.ToInt32(cur[0]) == 0 && Convert.ToInt32(cur[1]) == 0 && Convert.ToInt32(cur[2]) == 0 && Convert.ToInt32(cur[3]) == 0))
                        {
                            var preitem = listBoxLable.Items[listBoxFiles.SelectedIndex - 1];
                            var it = preitem.ToString().Split(',');
                            if ((Convert.ToInt32(it[0]) != 0 || Convert.ToInt32(it[1]) != 0) && !checkBox6.Checked && !checkBox10.Checked)
                            {
                                listBoxLable.Items.Add(listBoxLable.Items[listBoxFiles.SelectedIndex - 1]);
                                temp = (string)listBoxLable.Items[listBoxFiles.SelectedIndex - 1];
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
                        if ((cnt-1) == listBoxFiles.SelectedIndex)
                        {
                            var loc = temp.ToString().Split(',');
                            var x1 = Convert.ToInt32(loc[0]);
                            var y1 = Convert.ToInt32(loc[1]);
                            var x2 = Convert.ToInt32(loc[2]);
                            var y2 = Convert.ToInt32(loc[3]);
                            var x3 = Convert.ToInt32(loc[4]);
                            var y3 = Convert.ToInt32(loc[5]);
                            var x4 = Convert.ToInt32(loc[6]);
                            var y4 = Convert.ToInt32(loc[7]);
                            int maxX = maxNum(x1, x2, x3, x4);
                            int maxY = maxNum(y1, y2, y3, y4);
                            int minX = minNum(x1, x2, x3, x4);
                            int minY = minNum(y1, y2, y3, y4);
                            this.xTextBox.Text = Convert.ToString(minX);
                            this.yTextBox.Text = Convert.ToString(minY);
                            this.widthTextBox.Text = Convert.ToString(maxX - minX);
                            this.heightTextBox.Text = Convert.ToString(maxY - minY);
                            this.x1TextBox.Text = loc[0];
                            this.y1TextBox.Text = loc[1];
                            this.x2TextBox.Text = loc[2];
                            this.y2TextBox.Text = loc[3];
                            this.x3TextBox.Text = loc[4];
                            this.y3TextBox.Text = loc[5];
                            this.x4TextBox.Text = loc[6];
                            this.y4TextBox.Text = loc[7];
                        }
                        
                    }

                    SaveGroundTruthFile();
                   

                }

                ClearSelect();  // Delete the previously selected redraw
                if (listBoxLable.Items.Count > listBoxFiles.SelectedIndex)
                {
                    listBoxLable.SelectedIndex = listBoxFiles.SelectedIndex;
                    listBoxLable_MouseClick(null, null);
                }

                LoadLabel(listBoxFiles.SelectedIndex);
            }
        }

        private Point RectStartPoint;
        private Point[] pointCorner = new Point[4];//Store the four vertices of the rectangle
        private Point[] Rect1 = new Point[4];
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(50, 72, 145, 220));

        private List<Point> OcPoints = new List<Point>();

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Picture frame
            
                RectStartPoint = e.Location;
                Invalidate();
            
        }

        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {


            if (e.Button != MouseButtons.Left)//Determine whether press the left button
                return;
                    
            Point tempEndPoint = e.Location; //Record the position and size of the box
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

                        e.Graphics.DrawPolygon(new Pen(Color.Red, 3), Rect1);//Repaint the color to red
            
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
                        MessageBox.Show("Only select from top left to bottom right");
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
                            MessageBox.Show("Need to mark in order of pictures!");
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
            dilog.Description = "Please select a folder";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
               selectedpath = dilog.SelectedPath;
               imagePath_ = dilog.SelectedPath + "\\";
               FormMain_Load(null, null);
            }
            this.listBoxFiles.SelectedIndex = 0;
            listBoxFiles_SelectedIndexChanged(null, null);
            if (this.listBoxFiles != null)
            {
                this.tFrameLabel.Text = "Total Frame:" + Convert.ToString(this.listBoxFiles.Items.Count);
                var tmp = imagePath_.Split('\\');
                var l = tmp.Length;
                if (l >= 2)
                {
                    var id = System.Text.RegularExpressions.Regex.Replace(tmp[l - 2], "[a-z]", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    this.nameLabel.Text = "Name:" + tmp[l - 2];
                    this.idLabel.Text = "ID:" + id;
                }
                //MessageBox.Show(tmp[tmp.Length-2]);
                if (l >= 4)
                {
                    if (tmp[l-4] == "multi-occlusion(211-240)")
                    {
                        this.typeLabel.Text = "Type:MOC";
                    }
                    else if(tmp[l-4] == "occlusion_different_team(151-180)")
                    {
                        this.typeLabel.Text = "Type:DOC";
                    }
                    else if(tmp[l-4] == "occlusion_same_team(121-150)")
                    {
                        this.typeLabel.Text = "Type:SOC";
                    }
                    else if(tmp[l-4] == "palyer-disappear(181-210)")
                    {
                        this.typeLabel.Text = "Type:OV";
                    }
                    else
                    {
                        this.typeLabel.Text = "Type:Undefind";
                    }
                }
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
                        items[0], items[1], items[2], items[3],items[4], items[5], items[6], items[7]), "Sample label information");
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


        // Handle some keyboard operations
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData != Keys.NumPad8 && keyData != Keys.NumPad5 && keyData != Keys.NumPad4 && keyData != Keys.NumPad6 && keyData != Keys.I && keyData != Keys.J && keyData != Keys.K && keyData != Keys.L
                && keyData != Keys.S && keyData != Keys.Z && keyData != Keys.X && keyData != Keys.C && keyData != Keys.V && keyData != Keys.Up && keyData != Keys.Down && keyData != Keys.Left && keyData != Keys.Right && keyData != Keys.Q && keyData != Keys.W)
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
                    double y02 = (y2 + y4) / 2;//Find the center of the rectangle and find two because it reduces the error caused by the rounding of decimals.

                    double moveSteps = 1;

                    switch (keyData)
                    {
                        case Keys.NumPad8:
                            { 
                                y1 -= moveSteps;
                                y2 -= moveSteps;
                                y3 -= moveSteps;
                                y4 -= moveSteps;
                            } 
                            break;     // up
                        case Keys.NumPad5:
                            { 
                                y1 += moveSteps;
                                y2 += moveSteps;
                                y3 += moveSteps;
                                y4 += moveSteps;
                            } 
                            break;     // down
                        case Keys.NumPad4: 
                            { 
                                x1 -= moveSteps;
                                x2 -= moveSteps;
                                x3 -= moveSteps;
                                x4 -= moveSteps;
                            } 
                            break;      // left 
                        case Keys.NumPad6: 
                            { 
                                x1 += moveSteps;
                                x2 += moveSteps;
                                x3 += moveSteps;
                                x4 += moveSteps;
                            } 
                            break;      // right
                        case Keys.I:
                            {
                                y1 -= moveSteps;
                                y2 -= moveSteps;
                                y3 -= moveSteps;
                                y4 -= moveSteps;
                            }
                            break;
                        case Keys.K:
                            {
                                y1 += moveSteps;
                                y2 += moveSteps;
                                y3 += moveSteps;
                                y4 += moveSteps;
                            }
                            break;
                        case Keys.J:
                            {
                                x1 -= moveSteps;
                                x2 -= moveSteps;
                                x3 -= moveSteps;
                                x4 -= moveSteps;
                            }
                            break;
                        case Keys.L:
                            {
                                x1 += moveSteps;
                                x2 += moveSteps;
                                x3 += moveSteps;
                                x4 += moveSteps;
                            }
                            break;
                        case Keys.S: 
                            { 
                                x4 = x4 - (x4 - x1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                y4 = y4 - (y4 - y1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                x3 = x3 - (x3 - x2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                                y3 = y3 - (y3 - y2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                            } 
                            break;// w
                        case Keys.X: 
                            {  
                                x4 = x4 + (x4 - x1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                y4 = y4 + (y4 - y1) / (Math.Sqrt(Math.Pow((x4 - x1), 2) + Math.Pow((y4 - y1), 2)));
                                x3 = x3 + (x3 - x2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                                y3 = y3 + (y3 - y2) / (Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2)));
                            } 
                            break;     // s
                        case Keys.Z: 
                            { 
                                x2 = x2 - (x2 - x1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                y2 = y2 - (y2 - y1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                x3 = x3 - (x3 - x4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                                y3 = y3 - (y3 - y4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                            } 
                            break;      // a 
                        case Keys.C: 
                            { 
                                x2 = x2 + (x2 - x1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                y2 = y2 + (y2 - y1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                x3 = x3 + (x3 - x4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                                y3 = y3 + (y3 - y4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                            } 
                            break;      // d
                        case Keys.V: 
                            { 
                                x2 = x2 + (x2 - x1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                y2 = y2 + (y2 - y1) / (Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                                x3 = x3 + (x3 - x4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                                y3 = y3 + (y3 - y4) / (Math.Sqrt(Math.Pow((x3 - x4), 2) + Math.Pow((y3 - y4), 2)));
                            } 
                            break;      // d
                        case Keys.Q:
                            {
                                double temp_x = x1;
                                double adjustAngle = 3;
                                x1 = (x1 - x0) * Math.Cos(-Math.PI / 180.0 * adjustAngle) - (y1 - y0) * Math.Sin(-Math.PI / 180.0 * adjustAngle) + x0;
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
                            break;
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
                            break;
                        //case Keys.Up:
                            //if (this.listBoxFiles.SelectedIndex > 0)
                            //{
                                //this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex - 1;
                            //}
                            //else
                            //{
                            //    this.listBoxFiles.SelectedIndex = 0;
                            //}
                            //break;
                        //case Keys.Down:
                            //if (this.listBoxFiles.SelectedIndex < this.listBoxFiles.Items.Count - 1)
                            //{
                            //    this.listBoxFiles.SelectedIndex = this.listBoxFiles.SelectedIndex + 1;
                            //}
                            //else
                            //{
                            //    this.listBoxFiles.SelectedIndex = this.listBoxFiles.Items.Count - 1;
                            //}
                            //break;
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

                    
                    x1 = Math.Round(x1);
                    y1 = Math.Round(y1);
                    x2 = Math.Round(x2);
                    y2 = Math.Round(y2);
                    x3 = Math.Round(x3);
                    y3 = Math.Round(y3);
                    x4 = Math.Round(x4);
                    y4 = Math.Round(y4);

                    // Critical value judgment
                    int widthTmp = pictureBox1.Image.Width;
                    int heightTmp = pictureBox1.Image.Height;

                    if (((x1 <= 0 || x2 <= 0 || x3 <= 0 || x4 <= 0 || y1 <= 0 || y2 <= 0 || y3 <= 0 || y4 <= 0 ) && !(x1 == 0 && x2 == 0))|| x1 >= widthTmp || x2 >= widthTmp || x3 >= widthTmp || x4 >= widthTmp || y1 >= heightTmp || y2 >= heightTmp || y3 >= heightTmp || y4 >= heightTmp)
                    {
                        MessageBox.Show("box out of bounds!");
                        return false;
                    }

                    if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Right || keyData == Keys.Left)
                    {
                        listBoxFiles_SelectedIndexChanged(null, null);
                        return base.ProcessCmdKey(ref msg, keyData);
                    }


                    if (listBoxLable.Items.Count > listBoxFiles.SelectedIndex)
                        {
                            listBoxLable.Items.RemoveAt(listBoxFiles.SelectedIndex);
                            listBoxLable.Items.Insert(listBoxFiles.SelectedIndex, String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", x1, y1, x2, y2, x3, y3, x4, y4));
                        }
                        
                    
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
            ClearSelect();  // Delete previously selected
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
        // Modify label by location
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
 
        //Modify the label file for the entire line
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
                    MessageBox.Show("Partial occlusion cannot be checked for complete occlusion or disappearance of the player");
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
                    MessageBox.Show("Check Partial Occlusion or Player Disappearance Cannot check Full Occlusion");
                    return;
                }
                clearBox_Click(null, null);// clear box
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
                    MessageBox.Show("Check Partial Blocking or Full Blocking Can not check the player disappear");
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

        // Load Label option
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
                //SaveLabelFile2(line);
                var status = line.Split(',');
                checkBox1.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox5.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
                checkBox7.CheckState = CheckState.Unchecked;
                checkBox8.CheckState = CheckState.Unchecked;
                checkBox9.CheckState = CheckState.Unchecked;
                checkBox10.CheckState = CheckState.Unchecked;
                if (status[0] == "1")
                {
                    checkBox1.CheckState = CheckState.Checked;
                }
                if (status[1] == "1")
                {
                    checkBox2.CheckState = CheckState.Checked;
                }
                if (status[2] == "1")
                {
                    checkBox3.CheckState = CheckState.Checked;
                }
                if (status[3] == "1")
                {
                    checkBox5.CheckState = CheckState.Checked;
                }
                if (status[4] == "1")
                {
                    checkBox6.CheckState = CheckState.Checked;
                }
                if (status[5] == "1")
                {
                    checkBox7.CheckState = CheckState.Checked;
                }
                if (status[6] == "1")
                {
                    checkBox8.CheckState = CheckState.Checked;
                }
                if (status[7] == "1")
                {
                    checkBox9.CheckState = CheckState.Checked;
                }
                if (status[8] == "1")
                {
                    checkBox10.CheckState = CheckState.Checked;
                }
 
            }
            else
            {
                MessageBox.Show("no data");
                //this.checkBox1.Select();
                //SaveLabelFile("0" , "1");//The target row has no data and the default is no occlusion
            }
        }

        
        private void reset_Click(object sender, EventArgs e)
        {
            isreset_ = true;
            listBoxFiles_SelectedIndexChanged(null, null);
        }

        // clear box
        private void clearBox_Click(object sender, EventArgs e)
        {
            // Refresh label information of listBoxLabel
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

        // Inherit the previous frame
        private void inheritBox_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedIndex == 0)
            {
                MessageBox.Show("Cannot be inherited without the previous frame");
                return;
            }
            // Refresh label information of listBoxLabel
            listBoxLable.Items.Clear();
            var txt = groundtruthPath + imagefoldername;
            if (File.Exists(txt))
            {
                int cnt = 0;
                var lastFrame = "0,0,0,0,0,0,0,0";
                foreach (var item in File.ReadAllLines(txt.Trim()))
                {
                    if (cnt++ == listBoxFiles.SelectedIndex)
                    {
                        listBoxLable.Items.Add(lastFrame);
                    }
                    else
                    {
                        listBoxLable.Items.Add(item);
                    } 
                    lastFrame = item;
                }
                SaveGroundTruthFile();
            }
            listBoxFiles_SelectedIndexChanged(null, null);
        }


        // 
        private void genFile_Click(object sender, EventArgs e)
        {
            var txt = labelPath + imagefoldername;
            if (labelPath == "" || imagefoldername == "" || !File.Exists(txt))
            {
                MessageBox.Show("Please select an image dataset first");
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
            if (listBoxFiles.Items.Count == 0) {return;}//这里处理一个没导入图片点击pre的异常
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


        // Some operations when the form is closed
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.checkBox2.Checked || this.checkBox3.Checked || this.radioButton4.Checked)
            {
                if (OcPoints.Count == 0)
                {
                    this.listBoxFiles.SelectedIndex = prelistBoxFileIndex_;
                    MessageBox.Show("When there is occlusion, you must trace points");
                    e.Cancel = true;
                }
            }
        }
    }
}
