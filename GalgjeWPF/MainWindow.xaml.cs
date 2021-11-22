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
        string geheimWoord;
        private int levens = 10;
        public MainWindow()
        {
            InitializeComponent();
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
            int lengteWoord = txtInput.Text.Length; 
            if (levens != 1) 
            {
                if (lengteWoord == 1)
                {
                    if (geheimWoord.Contains(txtInput.Text))
                    {
                        juisteLetters = juisteLetters + txtInput.Text + ","; 
                        PrintUserOutPut(); 
                    }
                    else
                    {
                        levens--; 
                        fouteLetters = fouteLetters + txtInput.Text + ",";
                        PrintUserOutPut();
                    }


                }
                else if (lengteWoord > 1)
                {
                    if (txtInput.Text == geheimWoord)
                    {
                        txtTextDisplay.Text = $"Je hebt het\nwoordje '{geheimWoord}' geraden!";
                    }
                    else
                    {
                        levens--;
                        
                    }
                }

                txtInput.Clear();
            }
            else
            {
                txtTextDisplay.Text = $"Oh spijtig, you lost :(\nBetter luck next time!";
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
            geheimWoord = txtInput.Text;

            btnRaad.Visibility = Visibility.Visible;

            btnVerberg.Visibility = Visibility.Hidden;
            btnNieuwSpel.Visibility = Visibility.Visible;
            PrintUserOutPut();
            txtInput.Clear();
        }

        ///<summary>Methode om de users output te printen in de textbox, methode aangemaakt voor dubbele code te voorkomen</summary>///
        private void PrintUserOutPut() 
        {
            txtTextDisplay.Text = $"{levens} levens \n" +
                            $"Juiste letters: {juisteLetters}\n" +
                            $"Foute letters: {fouteLetters}";
        }
    }
}
