using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Image_App_2
{
    
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Basic image loading app \n Shizoor \n 2021");
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "%APPDATA%";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    filePath = openFileDialog.FileName;

                    //pictureBox1.ImageLocation = filePath;  One way of doing it
                    pictureBox1.Image = Bitmap.FromFile(filePath);   /// another
                    
                  
                }
            }
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Rectangle bitmapshape = new System.Drawing.Rectangle();
            int x, y;
            //Int32 pixelcolour;
            float r, g, b;
            Int32 pixel;

            bitmapshape.X = 0;
            bitmapshape.Y = 0;


            //Scrape the image into an array if it's not null
            if (pictureBox1.Image != null)
            {
                Bitmap image1 = new Bitmap(pictureBox1.Image);
                Bitmap image2 = new Bitmap(pictureBox1.Image);

                bitmapshape.Width = image1.Width;
                bitmapshape.Height = image1.Height;

                byte[,,] pixelarray = new byte[image1.Width, image1.Height, 3];   //red, green, blue
                byte[,,] pixelarray2 = new byte[image1.Width, image1.Height, 3];   //buffer for applying an effect
                label1.Text = ("Blur called");
                for (x = 0; x < image1.Width; x++)
                {
                    for (y = 0; y < image1.Height; y++)
                    {
                        pixel = image1.GetPixel(x, y).ToArgb();
                        pixelarray[x, y, 0] = (byte)((pixel & 0xFF0000) >> 16);
                        pixelarray[x, y, 1] = (byte)((pixel & 0xFF00) >> 8);
                        pixelarray[x, y, 2] = (byte)((pixel & 0xFF));
                    }
                }

                //Blur the image

                for (x = 1; x < (image1.Width - 1); x++)
                {
                    for (y = 1; y < image1.Height - 1; y++)
                    {
                        r = (pixelarray[x, y, 0] + pixelarray[x - 1, y, 0] + pixelarray[x + 1, y, 0] + pixelarray[x, y - 1, 0] + pixelarray[x, y + 1, 0]) / 5;
                        g = (pixelarray[x, y, 1] + pixelarray[x - 1, y, 1] + pixelarray[x + 1, y, 1] + pixelarray[x, y - 1, 1] + pixelarray[x, y + 1, 1]) / 5;
                        b = (pixelarray[x, y, 2] + pixelarray[x - 1, y, 2] + pixelarray[x + 1, y, 2] + pixelarray[x, y - 1, 2] + pixelarray[x, y + 1, 2]) / 5;

                        pixelarray2[x, y, 0] = (byte)r;
                        pixelarray2[x, y, 1] = (byte)g;
                        pixelarray2[x, y, 2] = (byte)b;

                    }
                }

                for (x = 0; x < image1.Width; x++)
                {
                    for (y = 0; y < image1.Height; y++)
                    {
                        //pixelcolour = pixelarray2[x, y, 0] + pixelarray2[x, y, 1] >> 8 + pixelarray2[x, y, 2] >> 16;
                        image2.SetPixel(x, y, Color.FromArgb(255, pixelarray2[x, y, 0], pixelarray2[x, y, 1], pixelarray2[x, y, 2]));
                        //image2.SetPixel(x, y, Color.FromArgb(pixelcolour));
                        //textBox1.Text = ("Chewing pixel" + x + "," + y);
                    }
                }

                image1 = image2.Clone(bitmapshape, System.Drawing.Imaging.PixelFormat.DontCare);   // meow

                pictureBox1.Image = image1;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a picture first.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                dialog.Filter= "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|png image|*.png";
                dialog.Title = "Save the image";
                dialog.ShowDialog();

                bmp.Save(dialog.FileName);
                label1.Text = "Image saved";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a picture first.");
            }
        }
    }
}
