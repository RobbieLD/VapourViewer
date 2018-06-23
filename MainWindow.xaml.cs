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
using System.IO;
using Newtonsoft.Json;

namespace vv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double Scale = 1;
        private double MasterScale = 1;
        private bool ZoomingIn = false;
        private Settings Settings { get; set; }
        private string SettingsPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
            {
                SettingsPath = args[1];

                // check if the file exists, if not create it. 
                if (!File.Exists(SettingsPath))
                {
                    File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new Settings()));
                }

                // Read the settings
                Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));

                ApplySettings();
            }
            else
            {
                // No settings given so we throw a warning
                MessageBox.Show("No settings supplied so changes can't be saved, using system defaults", "No Settings", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplySettings()
        {
            // Load Image
            if (!string.IsNullOrEmpty(Settings.ImagePath))
            {
                imageHolder.Source = new BitmapImage(new Uri(Settings.ImagePath));
            }

            // Load Show Header
            if (Settings.ShowHeader)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                WindowStyle = WindowStyle.None;
            }

            // Load Scroll Bars
            if (Settings.ShowScrollbars)
            {
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            // Load Window Left
            Left = Settings.WindowLeft;
            Top = Settings.WindowTop;
            Width = Settings.WindowWidth;
            Height = Settings.WindowHeight;

            MasterScale = Settings.ZoomLevel == 0 ? 1 : Settings.ZoomLevel;

            // Load Zoom Level
            if (Settings.ZoomLevel != 0)
                imageHolder.Source = new TransformedBitmap(this.imageHolder.Source as BitmapSource, new ScaleTransform(MasterScale, MasterScale));

            if (Settings.ScrollTop > 0)
                scrollViewer.ScrollToVerticalOffset(Settings.ScrollTop);

            if (Settings.ScrollLeft > 0)
                scrollViewer.ScrollToHorizontalOffset(Settings.ScrollLeft);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (WindowStyle != WindowStyle.None)
            {
                WindowStyle = WindowStyle.None;
                Settings.ShowHeader = false;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                Settings.ShowHeader = true;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (scrollViewer.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                Settings.ShowScrollbars = false;
            }
            else
            {
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                Settings.ShowScrollbars = true;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void imageHolder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // record the window position for the settings
            Settings.WindowTop = Top;
            Settings.WindowLeft = Left;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            
            if (result.Value)
            {
                imageHolder.Source = new BitmapImage(new Uri(fileDialog.FileName));
                Settings.ImagePath = fileDialog.FileName;
            }

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (!ZoomingIn)
            {
                Scale = 1;
            }

            Scale += 0.1;
            MasterScale += 0.1;
            imageHolder.Source = new TransformedBitmap(this.imageHolder.Source as BitmapSource, new ScaleTransform(Scale, Scale));
            ZoomingIn = true;
            Settings.ZoomLevel = MasterScale;
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (ZoomingIn)
            {
                Scale = 1;
            }

            Scale += -0.1;
            MasterScale += -0.1;
            imageHolder.Source = new TransformedBitmap(this.imageHolder.Source as BitmapSource, new ScaleTransform(Scale, Scale));

            ZoomingIn = false;
            Settings.ZoomLevel = MasterScale;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Settings));
                MessageBox.Show("Settings Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Error Saving Settings: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.WindowHeight = Height;
            Settings.WindowWidth = Width;
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Settings.ScrollLeft = scrollViewer.HorizontalOffset;
            Settings.ScrollTop = scrollViewer.VerticalOffset;
        }
    }
}
