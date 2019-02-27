using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace KryptoZaba
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        //  v_      - variables
        //  p_      - property
        //  sm_     - standard methods
        //  xe_     - XAML elements
        //  em_     - event methods
        //  cm_     - command method      

        //==================================================================================================================
        //============================================== VARIABLES =========================================================
        //==================================================================================================================

        /// <summary>
        /// Provides a list of avaible ciphers.
        /// </summary>
        private enum Ciphers
        {
            Caesar,
            Vigenere
        }

        //==================================================================================================================
        //=============================================== PROPERTY =========================================================
        //==================================================================================================================
        
        private Ciphers p_currentCipher { get; set; }
        private Ciphers p_currentBreakCipher { get; set; }

        //==================================================================================================================
        //======================================= CONSTRUCTOR / DESTRUCTOR =================================================
        //==================================================================================================================

        public MainWindow()
        {
            InitializeComponent();
            xe_GalleryCipher_Galery.SelectedItem = FindResource("Cez").ToString();
            
        }

        //==================================================================================================================
        //============================================= STD METHODS ========================================================
        //==================================================================================================================

        private bool sm_CheckForOperation()
        {
            if (xe_TextBox_Input.Text.Length == 0)
            {
                MessageBox.Show("Nie podano danych wejściowych.", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (xe_KeyCipher_TextBox.Text.Length == 0)
            {
                MessageBox.Show("Nie podano klucza.","Błąd",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return false;
            }
            //else
            return true;
        }

        private bool sm_CheckForCryptoAnalysis()
        {
            if (xe_CryptoAnalysis_WordInput.Text != "" && xe_CryptoAnalisis_TextBox.Text.Length > 3)
                return true;
            else return false;
        }

        //==================================================================================================================
        //============================================= EVENT METHODS ======================================================
        //==================================================================================================================

        private void em_Szyfruj_OnClick(object sender, RoutedEventArgs e)
        {
            if (sm_CheckForOperation())
            {
                xe_TextBox_Output.FontStyle = FontStyles.Normal;

                if (p_currentCipher == Ciphers.Caesar)
                {
                    string plainText = xe_TextBox_Input.Text;
                    string key = xe_KeyCipher_TextBox.Text;
                    xe_TextBox_Output.Text = CryptoFrog.CaesarEncrypt(plainText, key);
                }
                else if(p_currentCipher == Ciphers.Vigenere)
                {
                    string plainText = xe_TextBox_Input.Text;
                    string key = xe_KeyCipher_TextBox.Text;
                    xe_TextBox_Output.Text = CryptoFrog.VigenereEncrypt(plainText, key);
                }
            }
        }

        private void em_Deszyfruj_OnClick(object sender, RoutedEventArgs e)
        {
            if (sm_CheckForOperation())
            {
                xe_TextBox_Output.FontStyle = FontStyles.Normal;

                if (p_currentCipher == Ciphers.Caesar)
                {
                    string cryptogram = xe_TextBox_Input.Text;
                    string key = xe_KeyCipher_TextBox.Text;
                    xe_TextBox_Output.Text = CryptoFrog.CaesarDecrypt(cryptogram, key); 
                }
                else if(p_currentCipher == Ciphers.Vigenere)
                {
                    string cryptogram = xe_TextBox_Input.Text;
                    string key = xe_KeyCipher_TextBox.Text;
                    xe_TextBox_Output.Text = CryptoFrog.VigenereDecrypt(cryptogram, key);
                }
            }
        }

        private void em_SwitchButton_OnClick(object sender, RoutedEventArgs e)
        {
            xe_TextBox_Input.Text = xe_TextBox_Output.Text;
            xe_TextBox_Output.FontStyle = FontStyles.Italic;
            xe_TextBox_Output.Text = FindResource("Wynik").ToString();
        }

        private void em_Clear_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            xe_TextBox_Output.Text = FindResource("Wynik").ToString();
            xe_TextBox_Output.FontStyle = FontStyles.Italic;
        }

        private void em_GaleryCipher_OnChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (xe_GalleryCipher.SelectionBoxItem.ToString() == FindResource("Cez").ToString())
                p_currentCipher = Ciphers.Caesar;
            else if (xe_GalleryCipher.SelectionBoxItem.ToString() == FindResource("PoliAlf").ToString())
                p_currentCipher = Ciphers.Vigenere;

            //mistrzowskie rozwiazanie:
            em_KeyCipherTextBox_OnChange(null, null);
            //fuck yeah!


        }

        private void em_KeyCipherTextBox_OnChange(object sender, TextChangedEventArgs e)
        {
            if (p_currentCipher == Ciphers.Caesar)
            {
                int realNumber = CryptoFrog.TranslateKey(xe_KeyCipher_TextBox.Text);
                int shortNumber = realNumber % CryptoFrog.alphabet.Length;
                xe_KeyCipher_TextBox.ToolTip = $"Real value: {realNumber}\nShort value: {shortNumber}";
            }
            else if(p_currentCipher == Ciphers.Vigenere)
            {
                string tooltiptext = "";
                foreach (char znak in xe_KeyCipher_TextBox.Text)
                {
                    tooltiptext += "[" + CryptoFrog.TranslateKey(znak.ToString()).ToString() + "] ";
                }
                xe_KeyCipher_TextBox.ToolTip = tooltiptext;

            }
            else
                xe_KeyCipher_TextBox.ToolTip = "?";
        }

        private void em_Ribbon_OnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (xe_Ribbon.SelectedIndex == 0)
            {
                xe_StandardGrid.Visibility = Visibility.Visible;
                xe_CryptoanalysysGrid.Visibility = Visibility.Collapsed;
            }    
            else if (xe_Ribbon.SelectedIndex == 1)
            {
                xe_StandardGrid.Visibility = Visibility.Collapsed;
                xe_CryptoanalysysGrid.Visibility = Visibility.Visible;
            }
                
        }


        //==================================================================================================================
        //============================================= UNCATEGORIZED ======================================================
        //==================================================================================================================

        private void cm_Save_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if(xe_TextBox_Output != null)
            { 
                if (xe_TextBox_Output.Text != FindResource("Wynik").ToString())
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }

        private void cm_Save(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Zapisz kryptogram",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "Szyfrogram Krypto Zaby (*.cfro)|*.cfro|Wszystkie pliki (*.*)|*.*",
                DefaultExt = "cfro"
            };

            if(dialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dialog.FileName, xe_TextBox_Output.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Krytyczny wyjątek", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void cm_Open_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cm_Open(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Otwórz kryptogtam",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "Szyfrogram Krypto Zaby (*.cfro)|*.cfro|Wszystkie pliki (*.*)|*.*",
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    if (xe_Ribbon.SelectedIndex == 0)
                        xe_TextBox_Input.Text = File.ReadAllText(dialog.FileName);
                    else if (xe_Ribbon.SelectedIndex == 1)
                        xe_CryptoAnalisis_TextBox.Text = File.ReadAllText(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Krytyczny wyjątek", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void em_Cryptoanalisis_OnClick(object sender, RoutedEventArgs e)
        {
            List<string> wyniki = new List<string>();

            if (sm_CheckForCryptoAnalysis())
            {

                if (p_currentBreakCipher == Ciphers.Caesar)
                {
                    wyniki = CryptoFrog.BreakCaesar(xe_CryptoAnalisis_TextBox.Text, xe_CryptoAnalysis_WordInput.Text);
                    foreach (string tekst in wyniki)
                    {
                        TextBox t1 = new TextBox();
                        t1.Margin = new Thickness(5, 3, 5, 3);
                        t1.Text = tekst;
                        t1.AcceptsReturn = true;
                        xe_CyrptoAnalysis_StackPanel_wyniki.Children.Add(t1);
                    }
                    MessageBox.Show($"Kryptoanaliza zakończona. Znaleziono {wyniki.Count} możliwych rezultatów");
                }
                else if(p_currentBreakCipher == Ciphers.Vigenere)
                {
                    MessageBoxResult rezultat;

                    rezultat = MessageBox.Show("Uwaga!\n\nZamierzasz wykonać operację łamania szyfru Vigener'a. Ta operacja w znacznym stopniu obciąza procesor. Przed kontynuowaniem upewnij się, że twój procesor jest wystarczająco mocny oraz, że podczas tej operacji ma zapewnione odpowiednie chłodzenie i jest podłączony do ładowarki.\n\nKontynuować?", "UWAGA!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (rezultat == MessageBoxResult.Yes)
                    {
                        VigenerePerforming vigenere = new VigenerePerforming(xe_CryptoAnalisis_TextBox.Text, xe_CryptoAnalysis_WordInput.Text, xe_CyrptoAnalysis_StackPanel_wyniki);
                        vigenere.ShowDialog();
                    }
                    else
                        return;
                }
                else
                {
                    MessageBox.Show("Żaden rodzaj szyfru nie został wybrany.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                   
            }
            else
            {
                MessageBox.Show("Nie można wykonać operacji. Upewnij się, że wprowadziłeś szyfrogram oraz, że podałeś szukane słowo (conajmniej 3 litery).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void em_CryptoAnalisis_GalleryCipher_OnChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (xe_CryptoAnalysis_GalleryCipher.SelectionBoxItem.ToString() == FindResource("Cez").ToString())
                p_currentBreakCipher = Ciphers.Caesar;
            else if (xe_CryptoAnalysis_GalleryCipher.SelectionBoxItem.ToString() == FindResource("PoliAlf").ToString())
                p_currentBreakCipher = Ciphers.Vigenere;
        }

        private void em_CryptoAnalysis_Clear_OnClick(object sender, RoutedEventArgs e)
        {
            xe_CyrptoAnalysis_StackPanel_wyniki.Children.Clear();
        }
    }
}
