using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MastermindNewLayout_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variabelen worden globaal geinitieerd
        string color1, color2, color3, color4;
        string chosenColor1, chosenColor2, chosenColor3, chosenColor4;
        int attempts = 0;
        int score = 100;
        int row = 0;
        int column;
        int n = 1;
        string answer;
        int chosenColor1Index = 0;
        int chosenColor2Index = 0;
        int chosenColor3Index = 0;
        int chosenColor4Index = 0;
        double maxAttempts = 10;
        List<string> playerNames = new List<string>();
        List<Ellipse> colorEllipses = new List<Ellipse>();
        List<Ellipse> choiseEllispes = new List<Ellipse>();
        List<Label> colorLabels = new List<Label>();
        StringBuilder highScoresList = new StringBuilder();
        List<RowDefinition> attemptRows = new List<RowDefinition>();
        string[] highScores = new string[15];
        public MainWindow()
        {
            InitializeComponent();
            CodeRandomizer(out color1, out color2, out color3, out color4);
            correctCodeTextBox.Text = $"{color1}, {color2}, {color3}, {color4}";
        }
        
        private void GameReset()
        {
            controlButton.IsEnabled = true;

            attemptRows.Clear();
            attemptGrid.RowDefinitions.Clear();
            CodeRandomizer(out color1, out color2, out color3, out color4);
            correctCodeTextBox.Text = $"{color1}, {color2}, {color3}, {color4}";

            RemoveAttemptEllipse();
            ResetChoiseEllipse();

            attempts = 0;
            score = 100;
            row = 0;
            n = 1;
            this.Title = $"Pogingen: 0";
            scoreTextBox.Text = "100";
            timerTextBox.Text = "0";
        }

        private void CodeRandomizer(out string color1, out string color2, out string color3, out string color4)
        {
            //Declareren van een random en een list
            Random color = new Random();
            List<string> colorList = new List<string>();
            string randomColor = "null";

            //For loop die 4 keer loopt om 4 kleuren in de list te zetten
            for (int colorPosition = 1; colorPosition <= 4; colorPosition++)
            {
                int colorNumber = (color.Next(1, 7));
                switch (colorNumber)
                {
                    case 1:
                        randomColor = "red";
                        break;
                    case 2:
                        randomColor = "orange";
                        break;
                    case 3:
                        randomColor = "yellow";
                        break;
                    case 4:
                        randomColor = "green";
                        break;
                    case 5:
                        randomColor = "blue";
                        break;
                    case 6:
                        randomColor = "white";
                        break;
                }

                colorList.Add(randomColor);
            }

            color1 = colorList[0];
            color2 = colorList[1];
            color3 = colorList[2];
            color4 = colorList[3];
        }

        //kijkt waneer de F12+CTRL keys ingedrukt worden
        private void Mastermind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12 && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                ToggleDebug();
            }
        }

        //Maakt debug vissible of invissible
        private void ToggleDebug()
        {
            if (correctCodeTextBox.Visibility == Visibility.Visible)
            {
                correctCodeTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                correctCodeTextBox.Visibility = Visibility.Visible;
            }
        }

        //Decclareren van startTimer en initiazeren van timer
        DateTime startTime = DateTime.Now;
        DispatcherTimer timer = new DispatcherTimer();

        //lijst met gradientBrushes
        GradientStop red = new GradientStop(Colors.Red, 0.5);
        GradientStop darkRed = new GradientStop(Colors.DarkRed, 1);
        GradientStop whiteSheen = new GradientStop(Colors.White, 0.0);
        GradientStop orange = new GradientStop(Colors.Orange, 0.5);
        GradientStop darkOrange = new GradientStop(Colors.DarkOrange, 1);
        GradientStop yellow = new GradientStop(Colors.Yellow, 0.5);
        GradientStop darkYellow = new GradientStop(Colors.DarkGoldenrod, 1);
        GradientStop green = new GradientStop(Colors.Green, 0.5);
        GradientStop darkGreen = new GradientStop(Colors.DarkGreen, 1);
        GradientStop blue = new GradientStop(Colors.Blue, 0.5);
        GradientStop darkBlue = new GradientStop(Colors.DarkBlue, 1);
        GradientStop white = new GradientStop(Colors.White, 0.5);
        GradientStop lightGray = new GradientStop(Colors.LightGray, 1);
        GradientStop gray = new GradientStop(Colors.Gray, 0.5);
        GradientStop black = new GradientStop(Colors.Black, 1);

        private void ColorChange(object sender, MouseButtonEventArgs e)
        {
            var ellipseColor = new RadialGradientBrush
            {
                GradientOrigin = new Point(0.3, 0.3),
                Center = new Point(0.5, 0.5)
            };

            ellipseColor.GradientStops.Clear();
            if (sender == ellipse1)
            {

                chosenColor1Index++;
                switch (chosenColor1Index)
                {
                    case 1:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(red);
                        ellipseColor.GradientStops.Add(darkRed);
                        chosenColor1 = "red";
                        break;
                    case 2:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(orange);
                        ellipseColor.GradientStops.Add(darkOrange);
                        chosenColor1 = "orange";
                        break;
                    case 3:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(yellow);
                        ellipseColor.GradientStops.Add(darkYellow);
                        chosenColor1 = "yellow";
                        break;
                    case 4:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(green);
                        ellipseColor.GradientStops.Add(darkGreen);
                        chosenColor1 = "green";
                        break;
                    case 5:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(blue);
                        ellipseColor.GradientStops.Add(darkBlue);
                        chosenColor1 = "blue";
                        break;
                    case 6:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(white);
                        ellipseColor.GradientStops.Add(lightGray);
                        chosenColor1 = "white";
                        chosenColor1Index = 0;
                        break;
                }

                ellipse1.Fill = ellipseColor;
            }
            else if (sender == ellipse2)
            {

                chosenColor2Index++;
                switch (chosenColor2Index)
                {
                    case 1:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(red);
                        ellipseColor.GradientStops.Add(darkRed);
                        chosenColor2 = "red";
                        break;
                    case 2:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(orange);
                        ellipseColor.GradientStops.Add(darkOrange);
                        chosenColor2 = "orange";
                        break;
                    case 3:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(yellow);
                        ellipseColor.GradientStops.Add(darkYellow);
                        chosenColor2 = "yellow";
                        break;
                    case 4:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(green);
                        ellipseColor.GradientStops.Add(darkGreen);
                        chosenColor2 = "green";
                        break;
                    case 5:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(blue);
                        ellipseColor.GradientStops.Add(darkBlue);
                        chosenColor2 = "blue";
                        break;
                    case 6:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(white);
                        ellipseColor.GradientStops.Add(lightGray);
                        chosenColor2 = "white";
                        chosenColor2Index = 0;
                        break;
                }

                ellipse2.Fill = ellipseColor;
            }
            else if (sender == ellipse3)
            {

                chosenColor3Index++;
                switch (chosenColor3Index)
                {
                    case 1:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(red);
                        ellipseColor.GradientStops.Add(darkRed);
                        chosenColor3 = "red";
                        break;
                    case 2:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(orange);
                        ellipseColor.GradientStops.Add(darkOrange);
                        chosenColor3 = "orange";
                        break;
                    case 3:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(yellow);
                        ellipseColor.GradientStops.Add(darkYellow);
                        chosenColor3 = "yellow";
                        break;
                    case 4:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(green);
                        ellipseColor.GradientStops.Add(darkGreen);
                        chosenColor3 = "green";
                        break;
                    case 5:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(blue);
                        ellipseColor.GradientStops.Add(darkBlue);
                        chosenColor3 = "blue";
                        break;
                    case 6:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(white);
                        ellipseColor.GradientStops.Add(lightGray);
                        chosenColor3 = "white";
                        chosenColor3Index = 0;
                        break;
                }

                ellipse3.Fill = ellipseColor;
            }
            else if (sender == ellipse4)
            {

                chosenColor4Index++;
                switch (chosenColor4Index)
                {
                    case 1:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(red);
                        ellipseColor.GradientStops.Add(darkRed);
                        chosenColor4 = "red";
                        break;
                    case 2:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(orange);
                        ellipseColor.GradientStops.Add(darkOrange);
                        chosenColor4 = "orange";
                        break;
                    case 3:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(yellow);
                        ellipseColor.GradientStops.Add(darkYellow);
                        chosenColor4 = "yellow";
                        break;
                    case 4:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(green);
                        ellipseColor.GradientStops.Add(darkGreen);
                        chosenColor4 = "green";
                        break;
                    case 5:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(blue);
                        ellipseColor.GradientStops.Add(darkBlue);
                        chosenColor4 = "blue";
                        break;
                    case 6:
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(white);
                        ellipseColor.GradientStops.Add(lightGray);
                        chosenColor4 = "white";
                        chosenColor4Index = 0;
                        break;
                }

                ellipse4.Fill = ellipseColor;
            }
        }

        private void controlButton_Click(object sender, RoutedEventArgs e)
        {
            Start_Countdown();
            if(attempts < maxAttempts) 
            {
                attempts++;
            }


            List<string> chosenColors = new List<string> { chosenColor1, chosenColor2, chosenColor3, chosenColor4 }; 
            List<string> correctColors = new List<string> { color1, color2, color3, color4 };

            this.Title = $"Poging: {attempts}";



            ShowAttempt(chosenColors, correctColors);
            ChangeScore(chosenColors, correctColors);
            WinOrLose(chosenColors, correctColors);

        }

        //Deze methode start de countdown met een interval van 1
        private void Start_Countdown()
        {
            timerTextBox.Text = "00";
            timer.Interval = TimeSpan.FromSeconds(1);
            //aanmaken van de Timer_Tick methode
            timer.Tick += Timer_Tick;
            timer.Start();

            startTime = DateTime.Now;
        }

        //De timer tickt iedere seconden en na 10 seconder voert die de methode Stop_Countdown uit
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan interval = DateTime.Now - startTime;

            timerTextBox.Text = interval.ToString("ss");
            string tien = "10";
            if (interval.ToString("ss") == tien)
            {
                Stop_Countdown();
            }
        }

        //Deze methode stops de countdown en laat de speler weten dat hij zijn beurt verloren heeft
        private void Stop_Countdown()
        {
            timer.Stop();
            MessageBox.Show("Beurt verloren");
            score -= 8;
        }
        private void ShowAttempt(List<string> chosenColors, List<string> correctColors)
        {
            column = 0;

            for (int i = 0; i < 4; i++)
            {
                Ellipse colorEllipse = new Ellipse
                {
                    Width = 25,
                    Height = 25
                };

                RadialGradientBrush ellipseColor = new RadialGradientBrush
                {
                    GradientOrigin = new Point(0.3, 0.3),
                    Center = new Point(0.5, 0.5)
                };

                ellipseColor.GradientStops.Clear();

                switch (chosenColors[i])
                {
                    case "red":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(red);
                        ellipseColor.GradientStops.Add(darkRed);
                        break;
                    case "orange":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(orange);
                        ellipseColor.GradientStops.Add(darkOrange);
                        break;
                    case "yellow":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(yellow);
                        ellipseColor.GradientStops.Add(darkYellow);
                        break;
                    case "green":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(green);
                        ellipseColor.GradientStops.Add(darkGreen);
                        break;
                    case "blue":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(blue);
                        ellipseColor.GradientStops.Add(darkBlue);
                        break;
                    case "white":
                        ellipseColor.GradientStops.Add(whiteSheen);
                        ellipseColor.GradientStops.Add(white);
                        ellipseColor.GradientStops.Add(black);
                        break;
                }

                colorEllipse.Fill = ellipseColor;

                AddRow();
                Grid.SetRow(colorEllipse, row);
                Grid.SetColumn(colorEllipse, column);
                attemptGrid.Children.Add(colorEllipse);
                colorEllipses.Add(colorEllipse);
                column++;


                if (chosenColors[i] == correctColors[i])
                {
                    colorEllipse.StrokeThickness = 3;
                    colorEllipse.Stroke = new SolidColorBrush(Colors.DarkRed);
                }
                else if (correctColors.Contains(chosenColors[i]))
                {
                    colorEllipse.StrokeThickness = 3;
                    colorEllipse.Stroke = new SolidColorBrush(Colors.Wheat);
                }
                else
                {
                    colorEllipse.StrokeThickness = 3;
                    colorEllipse.Stroke = new SolidColorBrush(Colors.Transparent);
                }
            }
            row++;
        }

        private void ChangeScore(List<string> chosenColors, List<string> correctColors)
        {
            for (int i = 0; i < 4; i++)
            {
                if (correctColors[i] == chosenColors[i])
                {
                    score -= 0;
                }
                else if (correctColors.Contains(chosenColors[i]))
                {
                    score--;
                }
                else
                {
                    score -= 2;
                }
            }

            scoreTextBox.Text = score.ToString();
        }

        int i = 0;

        private void WinOrLose(List<string> chosenColors, List<string> correctColors)
        {
            if (chosenColors.SequenceEqual(correctColors))
            {
                timer.Stop();
                controlButton.IsEnabled = false;
                MessageBoxResult antwoord = MessageBox.Show($"Score: {score}\nAantal pogingen: {attempts}!\nWilt u noch eens proberen?", "Gewonnen!");
                highScores[i] = $"{answer} - {attempts} pogingen - {score}/100";
                highScoresList.AppendLine(highScores[i]);
                i++;
            }
            else if (attempts >= maxAttempts)
            {
                timer.Stop();
                controlButton.IsEnabled = false;
                MessageBoxResult antwoord = MessageBox.Show("Helaas u pogingen zijn op.\nWilt u noch eens proberen?", "Verloren");
                highScores[i] = $"{answer} - {attempts} pogingen - {score}/100";
                highScoresList.AppendLine(highScores[i]);
                i++;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult antwoord = MessageBox.Show("Bent u zeker dat u de aplicatie wilt afsluiten?", "Afsluiten", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (antwoord == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void maxAttemptSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            maxAttempts = maxAttemptSlider.Value;
        }

        private void RemoveAttemptEllipse()
        {
            foreach(var ellipse in colorEllipses)
            {
                attemptGrid.Children.Remove(ellipse);
            }

            colorEllipses.Clear();
        }

        private void ResetChoiseEllipse()
        {
            RadialGradientBrush ellipseColor = new RadialGradientBrush
            {
                GradientOrigin = new Point(0.3, 0.3),
                Center = new Point(0.5, 0.5)
            };

            ellipseColor.GradientStops.Add(whiteSheen);
            ellipseColor.GradientStops.Add(gray);
            ellipseColor.GradientStops.Add(black);

            ellipse1.Fill = ellipseColor;
            ellipse2.Fill = ellipseColor;
            ellipse3.Fill = ellipseColor;
            ellipse4.Fill = ellipseColor;
        }

        private void closeMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void newGameMenu_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
            GameReset();
        }


        private void StartGame()
        {
            answer = Interaction.InputBox("Geef u naam: ", $"Speler {n} naam.");
            if (string.IsNullOrEmpty(answer))
            {
                MessageBox.Show("Geef alstublief u naam in!", "Leeg antwoord!");
                answer = Interaction.InputBox("Geef u naam: ", "Speler naam.");
            }
            if (answer.Length > 0)
            {
                n++;
                playerNames.Add(answer);
                MessageBoxResult anotherPlayerMsgBox = MessageBox.Show("Wilt u nog een speler toevoegen?", $" speler nummer: {n}", MessageBoxButton.YesNo);
                if(anotherPlayerMsgBox == MessageBoxResult.Yes)
                {
                    StartGame();
                }
            }

        }

        private void highScoreMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{highScoresList}", "Highscores");
        }

        private void AddRow()
        {
            RowDefinition attemptRow = new RowDefinition
            {
                Height = new GridLength(30)
            };
            attemptGrid.RowDefinitions.Add(attemptRow);
            attemptRows.Add(attemptRow);
        }

    }
}