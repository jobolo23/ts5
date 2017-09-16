using System;
using System.Windows;



namespace TheraS5
{
    /// <summary>
    /// Interaktionslogik für loading.xaml
    /// </summary>
    public partial class loading : Window
    {
        public loading()
        {
            InitializeComponent();
            //pb_load.IsIndeterminate = true;
        }

        
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Parallel.Invoke(Drehen);
            

        }

        public void RotateWheelEvent(object sender, EventArgs args)
        {
            
        }

        private void Drehen()
        {
           


            /*
        
            DoubleAnimation da = new DoubleAnimation
                (360, 0, new Duration(TimeSpan.FromSeconds(3)));
            RotateTransform rt = new RotateTransform();
            imgSanduhr.RenderTransform = rt;
            imgSanduhr.RenderTransformOrigin = new Point(0.5, 0.5);
            da.RepeatBehavior = RepeatBehavior.Forever;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        

            */





        }
        

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
