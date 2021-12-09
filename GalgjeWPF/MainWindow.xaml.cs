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
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace GalgjeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declareren en initialiseren benodigde variabelen
        string juisteLetters;
        string fouteLetters;
        private static string geheimWoord;
        private int levens = 10;
        string woordVerbergen;
        Image[] afbeeldingStukjes;
        string[] woordArray = new string[0];
        string[] verborgenWoordArray;
        int seconden;
        private DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            afbeeldingStukjes = Afbeeldingen();
            TimerAanmaken();            
        }

        

        ///<Summary>
        ///Aanmaken van de Click-methode voor de 'RAAD' button.
        ///STAP 1: Controleren of user out of quesses is of niet, NIET out of guesses dan:
        ///STAP 2: Controleren of ze een letter of een woord raden (.Length), Als het een letter is dan:
        ///STAP 3: Controleren of de geraden letter voorkomt in het geheime woord, ZO NIET:
        ///---> Foutief antwoord = -1 leven
        ///---> Out of guesses = LOST
        ///</Summary>
        private void btnRaad_Click(object sender, RoutedEventArgs e)
        {
            seconden = 11;
            dockPanel.Background = Brushes.Black;
            timer.Start();

            if (levens != 0)
            {
                if (txtInput.Text.Length == 1)
                {
                    if (geheimWoord.Contains(txtInput.Text.ToLower()))
                    {
                        for (int i = 0; i < woordArray.Length; i++)
                        {
                            if (txtInput.Text == woordArray[i])
                            {
                                verborgenWoordArray[i] = txtInput.Text;                               
                            }
                        }
                        JuisteLettersTonen();
                        txtWoord.Text = "";
                        PrintenMask();
                        PrintUserOutPut();
                        if (String.Join("",verborgenWoordArray) == String.Join("",woordArray))
                        {
                            YouWin();
                        }                                             
                    }                 
                    else
                    {
                        FouteGok();
                        FouteLettersTonen();
                        PrintUserOutPut();
                    }
                }
                else if (txtInput.Text.Length > 1)
                {
                    if (txtInput.Text == geheimWoord)
                    {
                        YouWin();
                    }
                    else
                    {
                        FouteGok();
                        PrintUserOutPut();
                    }
                }

            } 
            else 
            {
                YouLose();
                btnRaad.Visibility = Visibility.Hidden;
            }
            txtInput.Clear();
        }



        /// <summary>
        /// Aanmaken van de Click-methode voor de button 'Nieuw Spel'
        /// Veranderen van message naar de user toe en de andere knoppen visible maken
        /// </summary>

        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            ResetAllNewButton();
                     
        }

        /// <summary>
        /// Aanmaken van de Click-methode voor de button 'verbergen'
        /// STAP1: Waarde van Input gebruiker in variabele steken
        /// STAP2: Button verbergen
        /// STAP3: Aanroepen methode PrintUsersOutPut
        /// STAP4: Clearing textbox voor de Input
        /// </summary>

        private void btnVerberg_Click(object sender, RoutedEventArgs e)
        {
            geheimWoord = txtInput.Text.ToLower();

            VerbergClickVisibility();

            PrintUserOutPut();

            AanmakenMaskArray();
            
            txtInput.Clear();

            
            lblTimer.Text = seconden.ToString();
            timer.Start();
        }



        private void VerbergClickVisibility() 
        {
            btnRaad.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Hidden;
            btnNieuwSpel.Visibility = Visibility.Visible;
            txtWoord.Visibility = Visibility.Visible;
            lblTimer.Visibility = Visibility.Visible;
        }

        private void AanmakenMaskArray() 
        {
            woordArray = new string[geheimWoord.Length];
            verborgenWoordArray = new string[geheimWoord.Length];


            for (int i = 0; i < geheimWoord.Length; i++)
            {
                woordArray[i] = Convert.ToString(geheimWoord[i]);
            }
            for (int i = 0; i < geheimWoord.Length; i++)
            {
                verborgenWoordArray[i] = "_ ";
            }
            foreach (var item in verborgenWoordArray)
            {
                txtWoord.Text += item;
            }
        }
        private void PrintenMask()
        {
            foreach (var item in verborgenWoordArray)
            {
                txtWoord.Text += Convert.ToString(item);
            }
        }

        private void YouWin() 
        {
            txtTextDisplay.Text = $"Je hebt het woordje '{geheimWoord}' geraden!";
            btnRaad.Visibility = Visibility.Hidden;
            timer.Stop();
            lblTimer.Visibility = Visibility.Hidden;

        }
        private void YouLose() 
        {
            txtTextDisplay.Text = $"Oh spijtig, you lost\n:( Better luck next time!";
            timer.Stop();
            lblTimer.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
        }

        private void FouteGok() 
        {
            levens--;
            dockPanel.Background = Brushes.Red;
        }



        ///<summary>Methode om de users output te printen in de textbox, methode aangemaakt voor dubbele code te voorkomen</summary>///
        private void PrintUserOutPut()
        {
            var foutenAantal = 10 - levens;
            imageOutput.Children.Clear();
            for (int i = 0; i < foutenAantal; ++i)
            {
                imageOutput.Children.Add(afbeeldingStukjes[i]);
            }
            txtTextDisplay.Text = $"{levens} levens \n\r" +
                            $"Juiste letters: {juisteLetters}\n\r" +
                            $"Foute letters: {fouteLetters}";
        }
        private void ResetAllNewButton() 
        {
            juisteLetters = "";
            fouteLetters = "";
            levens = 10;
            txtWoord.Clear();
            imageOutput.Children.Clear();

            timer.Stop();
            TimerReset();

            txtTextDisplay.Text = "Geef een geheim woord in";

            btnVerberg.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            btnNieuwSpel.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
            lblTimer.Visibility = Visibility.Hidden;



        }
        private void JuisteLettersTonen() 
        {
            juisteLetters = juisteLetters + txtInput.Text + ",";

        }
        private void FouteLettersTonen() 
        {
            fouteLetters = fouteLetters + txtInput.Text + ",";
        }
        



        // Mouse enter and leave events

        private void btnAll_MouseEnter(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            
            lbl.BorderBrush = Brushes.Yellow;
        }

        private void btnAll_MouseLeave(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderBrush = Brushes.Crimson;
        }




        // galg opbouwen

        private static Image[] Afbeeldingen()
        {
            Debug.Assert(Directory.Exists("assets")); // als de map niet bestaat crasht het programma
            var arrayAfbeeldingen = new Image[10];
            int i = 0;
            foreach (var item in Directory.EnumerateFiles("assets"))
            {
                var path = new Uri(item, UriKind.Relative);
                var bitMap = new BitmapImage(path);

                var image = new Image();
                image.Source = bitMap;
                arrayAfbeeldingen[i++] = image;
            }
            return arrayAfbeeldingen;
        }


        //timer event
        private void TimerAanmaken() 
        {
            timer.Interval = new TimeSpan(0, 0, 1); // 0, 0, 1 = uur minuten seconden
            timer.Tick += TimerTick;
            seconden = 10;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            lblTimer.Text = seconden.ToString();
            if (seconden == 11)
            {
                lblTimer.Text = "10";

            }
            if (seconden == -1)
            {
                txtTijdOp.Clear();
                lblTimer.Text = "10";
                timer.Stop();
                levens--;
                seconden = 11;
                PrintUserOutPut();
                timer.Start();
                dockPanel.Background = Brushes.Black;

            }
            if (seconden == 0)
            {
                dockPanel.Background = Brushes.Red;
                txtTijdOp.Visibility = Visibility.Visible;
                txtTijdOp.Text = "Je tijd is op!";
                

            }
            if (levens == 0)
            {
                YouLose();   
            }
            if (seconden == 10 || seconden == 11)
            {
                txtTijdOp.Text = "";
                txtTijdOp.Visibility = Visibility.Hidden;
                dockPanel.Background = Brushes.Black;
            }
            seconden--;

        }
        private void TimerReset() 
        {
            seconden = 10;
        }

       
    }
}
