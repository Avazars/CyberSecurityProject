using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
                
                Bitmap map = new Bitmap(op.FileName);
                Bitmap clone = map.Clone(new Rectangle(0, 0, map.Width, map.Height), PixelFormat.Format32bppArgb);
                Image.Source = ToBitmapImage(clone);
                bitStorage = clone;
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

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "All supported graphics|*.png|" +
                         "Portable Network Graphic (*.png)|*.png",
                Title = "SavePNG",
            };
            
            if (sfd.ShowDialog() == true)
            {
                string path = sfd.FileName;
                encodedBitStorage.Save(sfd.FileName);
            }
        }
    }
}