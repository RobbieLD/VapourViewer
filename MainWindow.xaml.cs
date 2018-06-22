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

namespace vv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double Scale = 1;
        private bool ZoomingIn = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle != WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.None;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.scrollViewer.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                this.scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
            else
            {
                this.scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            
            if (result.Value)
            {
                this.imageHolder.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (!ZoomingIn)
            {
                Scale = 1;
            }

            Scale += 0.1;
            this.imageHolder.Source = new TransformedBitmap(this.imageHolder.Source as BitmapSource, new ScaleTransform(Scale, Scale));
            ZoomingIn = true;
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (ZoomingIn)
            {
                Scale = 1;
            }

            Scale += -0.1;
            this.imageHolder.Source = new TransformedBitmap(this.imageHolder.Source as BitmapSource, new ScaleTransform(Scale, Scale));

            ZoomingIn = false;
        }
    }
}
