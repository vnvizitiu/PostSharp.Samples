using System;
using System.Windows;
using PostSharp.Patterns.Threading;

namespace PostSharp.Samples.Threading.ThreadDispatching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.EnableControls(false);
            this.DoStuff();
        }

        [Background]
        private void DoStuff()
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 1000000; j++)
                {
                    Math.Sin(random.NextDouble());
                }
                this.SetProgress(i);

            }

            this.EnableControls(true);

            
        }

        [Dispatched(true)]
        private void SetProgress(int progress)
        {
            this.progressBar.Value = progress;
        }

        [Dispatched(true)]
        private void EnableControls(bool enabled)
        {

            this.startButton.IsEnabled = enabled;
        }
    }
}
