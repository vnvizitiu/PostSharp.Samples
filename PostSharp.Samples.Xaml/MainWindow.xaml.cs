using System.IO;
using System.Text;
using System.Windows;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Threading;

namespace PostSharp.Samples.Xaml
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerModel customer = new CustomerModel
        {
            FirstName = "Jan",
            LastName = "Novak",
            Addresses = new AdvisableCollection<AddressModel>
            {
                new AddressModel
                {
                    Line1 = "Saldova 1G",
                    Town = "Prague"
                },
                new AddressModel
                {
                    Line1 = "Tyrsova 25",
                    Town = "Brno"
                },
                new AddressModel
                {
                    Line1 = "Pivorarka 154",
                    Town = "Pilsen"
                }
            }
        };

        public MainWindow()
        {
            InitializeComponent();

            // Register our custom operation formatter.
            RecordingServices.OperationFormatter = new MyOperationFormatter(RecordingServices.OperationFormatter);

            // Create initial data.
            var customerViewModel = new CustomerViewModel { Customer = customer };

            customerViewModel.Customer.PrincipalAddress = customerViewModel.Customer.Addresses[0];

            // Clear the initialization steps from the recorder.
            RecordingServices.DefaultRecorder.Clear();

            DataContext = customerViewModel;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.SaveFileDialog();
            
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                Save(openFileDialog.FileName);
            }


        }

        [Background]
        [DisableUI]
        private void Save(string path)
        {
            customer.Save(path);
        }
    }
}