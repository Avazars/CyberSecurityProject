using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using Microsoft.Win32;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;

namespace CyberSecurityProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap bitStorage;
        private Bitmap encodedBitStorage; 
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncodeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Image.Equals(null) && EncodeText.Text.Length != 0)
            {
                SaveFileDialog sd = new SaveFileDialog()
                {
                    Filter = "Images|*.png;*.bmp;*.jpg"
                };
               
                encodedBitStorage = SteganographyHelper.embedText(EncodeText.Text, bitStorage);
                Image.Source = ToBitmapImage(encodedBitStorage);
            }
        }

        private void DecodeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!bitStorage.Equals(null))
            {
                DecodeText.Text = SteganographyHelper.extractText(bitStorage);
            }
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "OpenFile",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +  
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +  
                         "Portable Network Graphic (*.png)|*.png"
            };
            
            if (op.ShowDialog() == true)
            {
                BitmapImage temp = new BitmapImage(new Uri(op.FileName));
                Image.Source = temp;
                bitStorage = ImageToBitmap(temp);
                bitStorage = bitStorage.Clone(new Rectangle())
            }
            
        }

        private Bitmap ImageToBitmap(BitmapImage bitImg)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitImg));
                encoder.Save(outStream);
                Bitmap returnBitmap = new Bitmap(outStream);

                return returnBitmap;
                
            }
        }
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}