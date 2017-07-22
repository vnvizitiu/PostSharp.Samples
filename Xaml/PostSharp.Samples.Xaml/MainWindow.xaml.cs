﻿using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Threading;
using PostSharp.Patterns.Xaml;

namespace PostSharp.Samples.Xaml
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [NotifyPropertyChanged]
    public partial class MainWindow : Window
    {
        private Recorder recorder;
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
            // We need to have a local reference for [NotifyPropertyChanged] to work.
            this.recorder = RecordingServices.DefaultRecorder;


            InitializeComponent();

            // Register our custom operation formatter.
            RecordingServices.OperationFormatter = new MyOperationFormatter(RecordingServices.OperationFormatter);

            // Create initial data.
            var customerViewModel = new CustomerViewModel { Customer = customer };

            customerViewModel.Customer.PrincipalAddress = customerViewModel.Customer.Addresses[0];

            // Clear the initialization steps from the recorder.
            this.recorder.Clear();

            DataContext = customerViewModel;
        }

        [Command]
        public ICommand SaveCommand { get; private set; }
        
        private void ExecuteSave()
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