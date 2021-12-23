﻿using System;
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
        #region VariabelenETC
        // Declareren en initialiseren benodigde variabelen
        string juisteLetters;
        string fouteLetters;
        private static string geheimWoord;
        private int levens = 10;
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
        private int timerInput = 10;

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
        #endregion
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
        #region SingleOrMultiplayer
        // 'lblTweeSpelers_MouseDown' :
        // MouseDown event dat ervoor zorgt dat het spel kan gespeeld worden met twee spelers
        // Deze knop verschijnt op het startscherm
        // Als je op deze knop klikt dan activeer je de optie om met 2 spelers te starten door behulp van de .Visibility
        // De boodschap in 'txt.TextDisplay' is verschillend van singleplayer en twee spelers
        private void lblTweeSpelers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dockPanelTweeSpelers.Visibility = Visibility.Visible;
            dockPanelStart.Visibility = Visibility.Hidden;
            txtTextDisplay.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            btnVerberg.Visibility = Visibility.Visible;
            imgHighScore.Visibility = Visibility.Hidden;
            mnuTimer.IsEnabled = true;
            lblInputWoord.Visibility = Visibility.Visible;
            mnuHighscore.IsEnabled = false;

            txtTextDisplay.Text = "Start een nieuw spel door een woord te verbergen. Ook kan je de tijd tussen een beurt instellen";
        }
        // 'lblSinglePlayer_MouseDown' :
        // MouseDown event dat ervoor zorgt dat het spel alleen kan gespeeld worden
        // Deze knop verschijnt op het startscherm
        // Als je op deze knop klikt dan activeer je de optie singleplayer te starten door behulp van de .Visibility
        // De boodschap in 'txt.TextDisplay' is verschillend van singleplayer en twee spelers

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
            mnuTimer.IsEnabled = true;
            mnuHighscore.IsEnabled = false;





        }
        #endregion

        #region VerbergMethods
        /// <summary>
        /// Aanmaken van de Click-methode voor de button 'verbergen'
        /// try catch voor formatexception
        /// na de try catch :
        /// eerste if :
        /// STAP 1:
        /// Messagebox showen
        /// STAP 2 clearen van ingave
        /// ELSE :
        ///
        /// STAP1: Waarde van Input gebruiker in variabele steken
        /// STAP2: Button verbergen
        /// STAP3: Aanroepen methode PrintUsersOutPut
        /// STAP4: Clearing textbox voor de Input
        /// </summary>

        private void btnVerberg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                timerInput = Convert.ToInt32(txtTimerInstellen.Text);

            }
            catch (FormatException)
            {
                
                MessageBox.Show("Je hebt geen timer ingesteld");
            }
            if (timerInput <5 || timerInput > 20)
            {
                MessageBox.Show("Gelieve een timer in te stellen tussen 5 en 20 seconden!");
                txtTimerInstellen.Clear();

            }
            else
            {
                geheimWoord = txtInput.Text.ToLower();

                seconden = timerInput;

                VerbergClickVisibility();

                PrintUserOutPut();

                AanmakenMaskArray();

                txtInput.Clear();

                lblTimer.Text = seconden.ToString();
                timer.Start();
            }
                      
        }
        // MousDown event dat ervoor zorgt dat de speler kan beginnen spelen
        // Try catch voor FormatException te onderscheppen
        // visibilities visible of hidden maken
        // random woord genereren
        //PrintUserOutput ==> updaten van de levens en de foute en juiste letters
        // AanmakenMaskArray ==> printen van geheimwoord in andere tekens
        //Timer starten
        private void btnStartSinglePlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                timerInput = Convert.ToInt32(txtTimerInstellen.Text);

            }
            catch (FormatException)
            {

                MessageBox.Show("Je hebt geen timer ingesteld");
            }
            if (timerInput < 5 || timerInput > 20)
            {
                MessageBox.Show("Gelieve een timer in te stellen tussen 5 en 20 seconden!");
                txtTimerInstellen.Clear();

            }
            else
            {
                seconden = timerInput;
                txtInput.Visibility = Visibility.Visible;
                lblTimer.Visibility = Visibility.Visible;
                txtRandomWoord.Visibility = Visibility.Visible;
                btnStartSinglePlayer.Visibility = Visibility.Hidden;
                btnRaad.Visibility = Visibility.Visible;
                btnNieuwSpel.Visibility = Visibility.Visible;
                txtTimerInstellen.Visibility = Visibility.Hidden;
                lblTimerInstellen.Visibility = Visibility.Hidden;
                mnuTimer.IsEnabled = false;
                mnuHint.IsEnabled = true;


                NewRandomWord();
                PrintUserOutPut();

                AanmakenMaskArray();

                txtInput.Clear();


                lblTimer.Text = seconden.ToString();
                timer.Start();
            }

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
            txtTimerInstellen.Visibility = Visibility.Hidden;
            lblTimerInstellen.Visibility = Visibility.Hidden;
            lblInputWoord.Visibility = Visibility.Hidden;
            mnuTimer.IsEnabled = false;
        }
        #endregion

        #region TimerMethods
        //Methode  voor de timer in te stellen
        // TimerTick event aanmaken
        
        private void TimerAanmaken() 
        {
            timer.Interval = new TimeSpan(0, 0, 1); 
            timer.Tick += TimerTick;
            seconden = timerInput;
        }
        //TimerTick event 
        // Ervoor zorgen dat timer kan pauzeren
        private void TimerTick(object sender, EventArgs e)
        {
            lblTimer.Text = seconden.ToString();
            if (seconden == timerInput)
            {
                lblTimer.Text = timerInput.ToString();

            }
            if (seconden == -1)
            {
                txtTijdOp.Clear();
                lblTimer.Text = timerInput.ToString();
                timer.Stop();
                levens--;
                seconden = timerInput+1;
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
            if (seconden == timerInput || seconden == timerInput +1)
            {
                lblTimer.Text = timerInput.ToString();
                txtTijdOp.Visibility = Visibility.Hidden;
                txtTijdOp2.Visibility = Visibility.Hidden;
                dockPanelTweeSpelers.Background = Brushes.Transparent;
            }
            seconden--;

        }
        //methode om de seconden terug te resetten
        private void TimerReset() 
        {
            seconden = timerInput;
        }

        #endregion

        #region AanmakenMask
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
        #endregion

        #region ControleGuess
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
            seconden = timerInput + 1;
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
                        if (String.Join("", verborgenWoordArray) == String.Join("", woordArray))
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
                else if (txtInput.Text.Length == 0)
                {
                    FouteGok();
                    FouteLettersTonen();
                    PrintUserOutPut();
                }

            }
            else
            {
                YouLose();
                btnRaad.Visibility = Visibility.Hidden;
            }
            txtInput.Clear();
        }
        

        #endregion

        #region JuisteFouteLettersToevoegen
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
        #endregion

        #region GalgOpbouwen & PrintenPlayerLives and PrintenJuisteFouteLetters
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

        ///<summary>Methode om de users output te printen, en de galg indien ze fout gokken
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

        #endregion

        #region YouWinYouLoseMethods
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
        #endregion

        #region NaamWinnaarToevoegen
        private void lblNaamBevestigen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            naam = txtBevestigNaam.Text;
            winnaarLevens = levens;
            txtBevestigNaam.Visibility = Visibility.Hidden;
            lblNaamBevestigen.Visibility = Visibility.Hidden;
            lblText.Visibility = Visibility.Hidden;


            DateTimeNow();

        }
        private void DateTimeNow()
        {
            StringBuilder highscoreOutput = new StringBuilder();
            int levensVerloren = 10 - winnaarLevens;
            DateTime dag = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            string uren = dag.Hour.ToString();
            string minuten = dag.Minute.ToString();
            string seconden = dag.Second.ToString();

            txtHighScore.Text += highscoreOutput.AppendLine($"{naam} - {levensVerloren} ({uren}:{minuten}:{seconden})").ToString();
        }
        #endregion




        #region NieuwSpelMethods
        /// <summary>
        /// Aanmaken van de Click-methode voor de button 'Nieuw Spel'
        /// Veranderen van message naar de user toe en de andere knoppen visible maken
        /// </summary>

        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            ResetAllNewButton();
                     
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
            mnuTimer.IsEnabled = false;
            mnuHint.IsEnabled = false;
            mnuHighscore.IsEnabled = true;
        }
        #endregion


        #region MouseEnterBorderChanged
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
        #endregion

        #region MenuClickMethods
        // Afsluiten app
        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // elementen om timer in te stellen visible maken
        private void Timer_Click(object sender, RoutedEventArgs e)
        {
            txtTimerInstellen.Visibility = Visibility.Visible;
            lblTimerInstellen.Visibility = Visibility.Visible;
        }
        // nieuw spel starten 
        private void NieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            ResetAllNewButton();
        }
        //hint vragen
        // toont een random letter uit alfabet dat niet in het woord voorkomt
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
        // highscore scherm en image tonen
        private void HighScore_Click(object sender, RoutedEventArgs e)
        {    
            imgHighScore.Visibility = Visibility.Visible;
            txtHighScore.Visibility = Visibility.Visible;
        }
        #endregion
        
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

        // Random woord selecteren uit de array van woorden
        private void NewRandomWord()
        {
            Random random = new Random();
            indexRandomWoord = random.Next(0, galgjeWoorden.Length);
            geheimWoord = galgjeWoorden[indexRandomWoord];
        }
    }

}
