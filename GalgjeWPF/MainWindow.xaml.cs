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
        public MainWindow()
        {
            InitializeComponent();
            afbeeldingStukjes = Afbeeldingen();
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
            if (levens != 1) 
            {
                if (txtInput.Text.Length == 1)
                {
                    if (geheimWoord.Contains(txtInput.Text.ToLower()))
                    {
                        juisteLetters = juisteLetters + txtInput.Text + ",";
                        int indexGuess = geheimWoord.IndexOf(txtInput.Text.ToLower());
                        verborgenWoordArray[indexGuess] = txtInput.Text;
                        txtWoord.Text = "";
                        foreach (var item in verborgenWoordArray)
                        {
                            txtWoord.Text += Convert.ToString(item);
                        }
                        PrintUserOutPut();
                    }
                    else
                    {
                        levens--; 
                        fouteLetters = fouteLetters + txtInput.Text + ",";
                        PrintUserOutPut();
                    }


                }
                else if (txtInput.Text.Length > 1)
                {
                    if (txtInput.Text == geheimWoord)
                    {
                        txtTextDisplay.Text = $"Je hebt het woordje '{geheimWoord}' geraden!";
                        btnRaad.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        levens--;
                        PrintUserOutPut();
                    }
                }

                txtInput.Clear();
            }
            else
            {
                txtTextDisplay.Text = $"Oh spijtig, you lost\n:( Better luck next time!";
                btnRaad.Visibility = Visibility.Hidden;
            }
        }
             
            

        /// <summary>
        /// Aanmaken van de Click-methode voor de button 'Nieuw Spel'
        /// Veranderen van message naar de user toe en de andere knoppen visible maken
        /// </summary>
        
        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            txtTextDisplay.Text = "Geef een geheim woord in";
            btnVerberg.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            btnNieuwSpel.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
            levens = 10;
            juisteLetters = "";
            fouteLetters = "";
            txtWoord.Clear();

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
           
            btnRaad.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Hidden;
            btnNieuwSpel.Visibility = Visibility.Visible;

            PrintUserOutPut();
            txtWoord.Visibility = Visibility.Visible;


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

            txtInput.Clear();
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

        private void btnRaad_MouseEnter(object sender, MouseEventArgs e)
        {
            
            
        }

        private void btnRaad_MouseLeave(object sender, MouseEventArgs e)
        {
            btnRaad.BorderBrush = Brushes.Transparent;
        }

        private void btnNieuwSpel_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNieuwSpel.BorderBrush = Brushes.Crimson;
        }

        private void btnNieuwSpel_MouseLeave(object sender, MouseEventArgs e)
        {
            btnNieuwSpel.BorderBrush = Brushes.Transparent;

        }

        private void btnVerberg_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnVerberg_MouseLeave(object sender, MouseEventArgs e)
        {

        }

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
            MessageBox.Show($"{i}");
            return arrayAfbeeldingen;
        }
    }
}
