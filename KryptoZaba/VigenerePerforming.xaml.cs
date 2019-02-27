using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace KryptoZaba
{
    /// <summary>
    /// Interaction logic for VigenerePerforming.xaml
    /// </summary>
    public partial class VigenerePerforming : Window
    {
        private bool p_isFinished { get; set; } = false;

        private BackgroundWorker worker;
        private Panel resaultPanel;

        public VigenerePerforming(string cryptogram, string word, Panel resaultContainer)
        {
            InitializeComponent();
            string[] parametry = { cryptogram, word };

            resaultPanel = resaultContainer;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += workerPracuj;
            worker.ProgressChanged += workerPostep;
            worker.RunWorkerCompleted += workerZakonczone;

            worker.RunWorkerAsync(parametry);
        }

        private void workerPostep(object sender, ProgressChangedEventArgs e)
        {
            xe_TextBlock_Iteracja.Text = $"Bieżąca iteracja: {e.ProgressPercentage}";   
        }

        private void workerZakonczone(object sender, RunWorkerCompletedEventArgs e)
        {
            p_isFinished = true;
            Close();

            if(e.Cancelled == false)
            {
                List<string> wyniki = (List<string>)e.Result;

                foreach (string tekst in wyniki)
                {
                    TextBox t1 = new TextBox();
                    t1.Margin = new Thickness(5, 3, 5, 3);
                    t1.Text = tekst;
                    t1.AcceptsReturn = true;
                    resaultPanel.Children.Add(t1);
                }
                MessageBox.Show($"Kryptoanaliza zakończona. Znaleziono {wyniki.Count} możliwych rezultatów");
            }
                
        }

        private void workerPracuj(object sender, DoWorkEventArgs e)
        {
            string cryptogram = (e.Argument as string[])[0];
            string word = (e.Argument as string[])[1];
            List<string> wyniki = new List<string>();

            for (int i = 1; i < 4; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                CryptoFrog.VigenereResurce((sender as BackgroundWorker),i, new List<string>(), cryptogram, word, ref wyniki);
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
            }
            e.Result = wyniki;

        }

        private void em_Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(p_isFinished == false)
            { 
                MessageBox.Show("UWAGA!\n\nZamknięcie tego okna nie spowoduje zatrzymania procesu działającego w tle. Aby przerwać kryptoanalizę użyj przycisku anuluj w oknie \"Łamanie szyfru Vigener'a\"", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Hand);
                e.Cancel = true;
            }
        }
    }
}
