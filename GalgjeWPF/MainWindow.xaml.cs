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
        string hintLetter;
        int seconden;
        int winnaarLevens;
        string naam;
        private DispatcherTimer timer = new DispatcherTimer();
        private static Color red = new Color();
        private static int indexRandomWoord;
        private bool askedForHint;
        private static string[] letters = new string[]
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "q",
            "y",
            "z"
        };
        private string[] galgjeWoorden = new string[]
        {
            "grafeem",
            "tjiftjaf",
            "maquette",
            "kitsch",
            "pochet",
            "convocaat",
            "jakkeren",
            "collaps",
            "zuivel",
            "cesium",
            "voyant",
            "spitten",
            "pancake",
            "gietlepel",
            "karwats",
            "dehydreren",
            "viswijf",
            "flater",
            "cretonne",
            "sennhut",
            "tichel",
            "wijten",
            "cadeau",
            "trotyl",
            "chopper",
            "pielen",
            "vigeren",
            "vrijuit",
            "dimorf",
            "kolchoz",
            "janhen",
            "plexus",
            "borium",
            "ontweien",
            "quiche",
            "ijverig",
            "mecenaat",
            "falset",
            "telexen",
            "hieruit",
            "femelaar",
            "cohesie",
            "exogeen",
            "plebejer",
            "opbouw",
            "zodiak",
            "volder",
            "vrezen",
            "convex",
            "verzenden",
            "ijstijd",
            "fetisj",
            "gerekt",
            "necrose",
            "conclaaf",
            "clipper",
            "poppetjes",
            "looikuip",
            "hinten",
            "inbreng",
            "arbitraal",
            "dewijl",
            "kapzaag",
            "welletjes",
            "bissen",
            "catgut",
            "oxymoron",
            "heerschaar",
            "ureter",
            "kijkbuis",
            "dryade",
            "grofweg",
            "laudanum",
            "excitatie",
            "revolte",
            "heugel",
            "geroerd",
            "hierbij",
            "glazig",
            "pussen",
            "liquide",
            "aquarium",
            "formol",
            "kwelder",
            "zwager",
            "vuldop",
            "halfaap",
            "hansop",
            "windvaan",
            "bewogen",
            "vulstuk",
            "efemeer",
            "decisief",
            "omslag",
            "prairie",
            "schuit",
            "weivlies",
            "ontzeggen",
            "schijn",
            "sousafoon"
        };
        public MainWindow()
        {
            InitializeComponent();
            afbeeldingStukjes = Afbeeldingen();
            TimerAanmaken();
            red.R = 181;
            red.G = 20;
            red.B = 36;
            red.A = 90;
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
            dockPanelTweeSpelers.Background = Brushes.Transparent;
            timer.Start();
            imageOutput.Visibility = Visibility.Visible;
         
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
                        txtRandomWoord.Text = "";
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


        //<summary>
        // Aanmaken methodie die Visibility changed als je op de knop drukt
        //</summary>
        private void VerbergClickVisibility() 
        {
            btnRaad.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Hidden;
            btnNieuwSpel.Visibility = Visibility.Visible;
            txtWoord.Visibility = Visibility.Visible;
            lblTimer.Visibility = Visibility.Visible;
            mnuTimer.Visibility = Visibility.Hidden;
        }

        //<summary>
        // Methode voor het aanmaken van 2 arrays, en het printen van de mask. 
        // 1) elke index van woordArray vullen met elke letter van het geheimwoord & 
        // 2) verborgenwoordArray is het mask van het woord
        //</summary>
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
                txtRandomWoord.Text += item;
            }
        }
        // Methode die het mask aanpast
        private void PrintenMask()
        {
            foreach (var item in verborgenWoordArray)
            {
                txtWoord.Text += Convert.ToString(item);
                txtRandomWoord.Text += Convert.ToString(item);
            }
        }

        // Methode die staat voor de uitkomst 'Victory'
        // Message dat de speler het juist heeft geraden
        // Timer stopt
        // Visibility functionaliteiten
        private void YouWin() 
        {
            txtTextDisplay.Text = $"Je hebt het woordje '{geheimWoord}' geraden!";
            timer.Stop();
            lblTimer.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
            txtInput.Visibility = Visibility.Hidden;
            imageOutput.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;

            if (askedForHint == false)
            {
                lblNaamBevestigen.Visibility = Visibility.Visible;
                lblText.Visibility = Visibility.Visible;
                txtBevestigNaam.Visibility = Visibility.Visible;
            }
            
        }
        // Methode die staat voor de uitkomst 'You lose'
        // Message dat de speler het juist heeft geraden
        // Timer stopt
        // Visibility functionaliteiten
        private void YouLose() 
        {
            txtTextDisplay.Text = $"Oh spijtig, you lost\n:( Better luck next time!";
            timer.Stop();
            lblTimer.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
            txtInput.Visibility = Visibility.Hidden;
            imageOutput.Visibility = Visibility.Hidden;
            txtRandomWoord.Visibility = Visibility.Hidden;
        }

        // Methode die een leven aftrekt bij foute gok en de achtergrond veranderd naar rood
        private void FouteGok() 
        {
            levens--;
            dockPanelTweeSpelers.Background = new SolidColorBrush(red);
                      
        }
        // Methode voor message als tijd op is
        private void ToSlow() 
        {
            txtTijdOp.Text = "Too Slow";
            txtTijdOp2.Text = "B*TCH";
        }



        ///<summary>Methode om de users output te printen in, en de galg indien ze fout gokken
        /// </summary>///
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
        // methode die alle code bevat om een nieuw spel te starten
        // alles resetten naar nieuwe waardes
        // visibility functionaliteit aanpassen als er op de knop word gedrukt
        private void ResetAllNewButton() 
        {
            juisteLetters = "";
            fouteLetters = "";
            levens = 10;
            txtWoord.Clear();
            txtRandomWoord.Clear();
            imageOutput.Children.Clear();

            timer.Stop();
            TimerReset();

            txtTextDisplay.Text = "Geef een geheim woord in";
            dockPanelTweeSpelers.Visibility = Visibility.Hidden;
            dockPanelStart.Visibility = Visibility.Visible;
            btnNieuwSpel.Visibility = Visibility.Hidden;
            txtWoord.Visibility = Visibility.Hidden;
            lblTimer.Visibility = Visibility.Hidden;
            imageOutput.Visibility = Visibility.Visible;
            btnRaad.Visibility = Visibility.Hidden;
            txtRandomWoord.Visibility = Visibility.Hidden;
            mnuTimer.Visibility = Visibility.Visible;
            btnStartSinglePlayer.Visibility = Visibility.Hidden;
            






        }

        // Methode voor printen van juiste letters
        private void JuisteLettersTonen() 
        {
            juisteLetters = juisteLetters + txtInput.Text + ",";

        }

        //Methode voor printen van foute letters
        private void FouteLettersTonen() 
        {
            fouteLetters = fouteLetters + txtInput.Text + ",";
        }
        



        // Mouse Enter & Leave events om border van kleur te veranderen van alle knoppen

        private void btnAll_MouseEnter(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            
            lbl.BorderBrush = Brushes.SaddleBrown;
        }

        private void btnAll_MouseLeave(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderBrush = Brushes.Black;
        }




        // Methode om galg opbouwen

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


        //Methode  voor de timer in te stellen
        private void TimerAanmaken() 
        {
            timer.Interval = new TimeSpan(0, 0, 1); 
            timer.Tick += TimerTick;
            seconden = 10;
        }
        //TimerTick event 
        // Eerste if : als de timer op 11 komt dan display '10', zo lijkt het dat de timer wacht
        // Tweede if : als de timer op -1 komt stopt die met tellen en displayed 10 voor de timer te pauzeren
        // Derde if : Als timer op 0 komt dan verander de background en komt er een Message dat de tijd op is
        // Vierde if : Als de timer op 10 kom of 11 kmot dan word de message verwijderd 
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
                dockPanelTweeSpelers.Background = Brushes.Transparent;

            }
            if (seconden == 0)
            {
                dockPanelTweeSpelers.Background = new SolidColorBrush(red);
                txtTijdOp.Visibility = Visibility.Visible;
                txtTijdOp2.Visibility = Visibility.Visible;

                ToSlow();        

            }
            if (levens == 0)
            {
                YouLose();   
            }
            if (seconden == 10 || seconden == 11)
            {
                txtTijdOp.Visibility = Visibility.Hidden;
                txtTijdOp2.Visibility = Visibility.Hidden;
                dockPanelTweeSpelers.Background = Brushes.Transparent;
            }
            seconden--;

        }
        //methode om de seconden terug te resetten
        private void TimerReset() 
        {
            seconden = 10;
        }
        private void DateTimeNow() 
        {
            DateTime dag = new DateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            string moment =  dag.ToString();
        }
        

        private void lblTweeSpelers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dockPanelTweeSpelers.Visibility = Visibility.Visible;
            dockPanelStart.Visibility = Visibility.Hidden;
            txtTextDisplay.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Visible;
            imgHighScore.Visibility = Visibility.Hidden;

            txtTextDisplay.Text = "Start een nieuw spel door een woord te verbergen. Ook kan je de tijd tussen een beurt instellen";
        }

        private void lblSinglePlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTextDisplay.Text = "Start een nieuw spel door op de 'Start' button te drukken. Ook kan je de tijd tussen een beurt instellen";
            dockPanelStart.Visibility = Visibility.Hidden;
            dockPanelTweeSpelers.Visibility = Visibility.Visible;
            txtTextDisplay.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Hidden;
            btnStartSinglePlayer.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Hidden;
            imgHighScore.Visibility = Visibility.Hidden;



        }

        private void Timer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lblNaamBevestigen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            naam = txtBevestigNaam.Text;
            winnaarLevens = levens;
            txtBevestigNaam.Visibility = Visibility.Hidden;
            lblNaamBevestigen.Visibility = Visibility.Hidden;
            lblText.Visibility = Visibility.Hidden;

            StringBuilder highscoreOutput = new StringBuilder();
            int levensVerloren = 10 - winnaarLevens;
            DateTime dag = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            string uren = dag.Hour.ToString();
            string minuten = dag.Minute.ToString();
            string seconden = dag.Second.ToString();

            txtHighScore.Text += highscoreOutput.AppendLine($"{naam} - {levensVerloren} ({uren}:{minuten}:{seconden})").ToString();


        }
        // controle button voor de single player
        private void btnStartSinglePlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtInput.Visibility = Visibility.Visible;
            lblTimer.Visibility = Visibility.Visible;
            txtRandomWoord.Visibility = Visibility.Visible;
            btnStartSinglePlayer.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Visible;
            btnNieuwSpel.Visibility = Visibility.Visible;
            mnuTimer.Visibility = Visibility.Hidden;
            

            NewRandomWord();
            PrintUserOutPut();

            AanmakenMaskArray();

            txtInput.Clear();


            lblTimer.Text = seconden.ToString();
            timer.Start();
        }

        private void NewRandomWord() 
        {
            Random random = new Random();
            indexRandomWoord = random.Next(0,galgjeWoorden.Length);
            geheimWoord = galgjeWoorden[indexRandomWoord];
        }

        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            ResetAllNewButton();
        }

        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            askedForHint = true;
            

            if (!String.IsNullOrWhiteSpace(geheimWoord))
            {

                askedForHint = true;

                List<char> geheimWoordList = geheimWoord.ToList();
                List<char> alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToList();
                List<char> overgeblevenLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToList();

                for (int i = 0; i < geheimWoordList.Count; i++)
                {
                    for (int y = 0; y < alfabet.Count; y++)
                    {
                        if (geheimWoordList[i] == alfabet[y])
                        {
                            var chr = alfabet[y];
                            overgeblevenLetters.Remove(chr);
                        }
                    }
                }
                Random random = new Random();
                hintLetter = overgeblevenLetters[random.Next(0, overgeblevenLetters.Count)].ToString();
                fouteLetters += hintLetter;
                MessageBox.Show($"De letter die ZEKER NIET voorkomt in het geheimwoord is: {hintLetter} ", "HINT");
            }

        }
       
        private void HighScore_Click(object sender, RoutedEventArgs e)
        {    
            imgHighScore.Visibility = Visibility.Visible;
            txtHighScore.Visibility = Visibility.Visible;
        }
    }

}
