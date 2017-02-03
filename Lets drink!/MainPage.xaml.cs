using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lets_drink_
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
         private Day day = new Day();
         private Statistics stats = new Statistics();
         private List<Statistics> week = new List<Statistics>();
        

        public MainPage() {
            this.InitializeComponent();
            
            setInitialValues();





            // readStatistics();

            //***************************************************************************
            //week.Clear();
            //string data = JsonConvert.SerializeObject(week);
            //File.WriteAllText("Assets/week.json", data);

            //stats.favouriteBeverage = "none";
            //stats.currentSeries = 0;
            //stats.totalAmount = 0;



            //data = JsonConvert.SerializeObject(stats);
            //File.WriteAllText("Assets/statistics.json", data);


            //week.Add(stats);

            //data = JsonConvert.SerializeObject(week);
            //File.WriteAllText("Assets/week.json", data);
            //***************************************************************************
            showWeeklyStatistics();


        }

        

        private void readStatistics() {
            string jsonString = File.ReadAllText("Assets/statistics.json");
            stats = JsonConvert.DeserializeObject<Statistics>(jsonString);
            jsonString = File.ReadAllText("Assets/week.json");
            week = JsonConvert.DeserializeObject<List<Statistics>>(jsonString);
 
            //nie wiem jak dostac sie do tych warunkow, sprawdzenie daty nie dziala w tym wypadku tak jak trzeba
            if (checkIfItIsStillTheSameDay()==0) {
               // titleTextBlock.Text = "ten sam";
            }
            else {
                if (week.Count < 7) {
                    week.Add(stats);
                   // titleTextBlock.Text = "mniej niz 7";
                }
                else {
                    week.RemoveAt(0);
                    week.Add(stats);
                   // titleTextBlock.Text = "wiecej niz 7";
                }
                sendStatisticsToDatebase();
                sendWeekToDatebase();
            }
            longestSeriesTextBlock.Text = stats.currentSeries.ToString();
           // titleTextBlock.Text = week.Count.ToString();
            

           

        }


        private void readAllJsons() {
            string jsonString = File.ReadAllText("Assets/statistics.json");
            stats = JsonConvert.DeserializeObject<Statistics>(jsonString);

            jsonString = File.ReadAllText("Assets/week.json");
            week = JsonConvert.DeserializeObject<List<Statistics>>(jsonString);

            jsonString = File.ReadAllText("Assets/day.json");
            day = JsonConvert.DeserializeObject<Day>(jsonString);
        }

        private void setInitialValues() {
            try {
                typeOfBeverageComboBox.Items.Add("water");
                typeOfBeverageComboBox.Items.Add("tea");
                typeOfBeverageComboBox.Items.Add("juice");
                typeOfBeverageComboBox.Items.Add("coffee");
                typeOfBeverageComboBox.SelectedIndex = 0;
                //***************************************************************************
                //day.date = DateTime.Now.ToString("dd-MM-yyyy");
                //sendDayToDatabase();
                //***************************************************************************
                readAllJsons();

                stats.totalAmount = 0;
                sendStatisticsToDatebase();
                sendWeekToDatebase();

                

                if (checkIfItIsStillTheSameDay() == 0) {
                    currentDrunkTextBlock.Text = day.currentDrunk.ToString();
                }
                else {
                    if(week.Count < 7) {
                        week.Add(stats);
                    }
                    else {
                        week.RemoveAt(0);
                        week.Add(stats);
                    }


                        if (checkIfItIsStillTheSameDay() > 1) {
                        stats.currentSeries = 0;
                        stats.totalAmount = 0;
                        for (int i = checkIfItIsStillTheSameDay(); i == 1; i--) {
                            if (week.Count < 7) {
                                week.Add(stats);
                            }
                            else {
                                week.RemoveAt(0);
                                week.Add(stats);
                            }
                        }

                        stats.currentSeries = 0;
                        sendStatisticsToDatebase();
                        sendWeekToDatebase();
                    }
                    currentDrunkTextBlock.Text = "0";
                    day.date = DateTime.Now.ToString("dd-MM-yyyy");
                }
                goalTextBlock.Text = day.goal.ToString();

                day.currentDrunk = double.Parse(currentDrunkTextBlock.Text);
               
                sendDayToDatabase();

                longestSeriesTextBlock.Text = stats.currentSeries.ToString();

                titleTextBlock.Text = week.Count.ToString();

            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void sendDayToDatabase() {
            string data = JsonConvert.SerializeObject(day);
            File.WriteAllText("Assets/day.json", data);
        }

        private void sendStatisticsToDatebase() {
            string data = JsonConvert.SerializeObject(stats);
            File.WriteAllText("Assets/statistics.json", data);
        }

        private void sendWeekToDatebase() {
            week.RemoveAt(week.Count - 1);
            week.Add(stats);
            string data = JsonConvert.SerializeObject(week);
             File.WriteAllText("Assets/week.json", data);
        }


        private int checkIfItIsStillTheSameDay() {
            string date = DateTime.Now.ToString("dd-MM-yyyy");

            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(day.date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            TimeSpan ts = dt - dt2;
            int difference = ts.Days;
            
            if(difference != 0) {
                day.goalIsAchievedToday = false;
                sendDayToDatabase();
            }
            return difference;


            //if (date.Equals(day.date))
            //    return true;
            //else {
            //    day.goalIsAchievedToday = false;
            //    sendDayToDatabase();
            //    return false;
            //}
        }


        private void firststButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(1);

                if (checkIfGoalIsAchieved()) {
                    if (day.goalIsAchievedToday == false) {
                        incrementCurrentSeries();
                    }                     
                }
            }
            catch {
                throw new NotImplementedException();
            }


        }

        private void incrementCurrentSeries() {
            stats.currentSeries++;
            longestSeriesTextBlock.Text = stats.currentSeries.ToString();

            sendStatisticsToDatebase();
            sendWeekToDatebase();
            day.goalIsAchievedToday = true;
            sendDayToDatabase();
        }

        
        private void secondButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(2);

                if (checkIfGoalIsAchieved()) {
                    if (day.goalIsAchievedToday == false) {
                        incrementCurrentSeries();
                    }
                }
            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void thirdButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(3);

                if (checkIfGoalIsAchieved()) {
                    if (day.goalIsAchievedToday == false) {
                        incrementCurrentSeries();
                    }
                }
            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void fouthButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(4);

                if (checkIfGoalIsAchieved()) {
                    if (day.goalIsAchievedToday == false) {
                        incrementCurrentSeries();
                    }
                }
            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void addToCurrentAfterClickButton(int button) {
            string choosedButton = "L";
            switch (button) {
                case 1:
                    choosedButton = firststButton.Content.ToString();
                    break;
                case 2:
                    choosedButton = secondButton.Content.ToString();
                    break;
                case 3:
                    choosedButton = thirdButton.Content.ToString();
                    break;
                case 4:
                    choosedButton = fouthButton.Content.ToString();
                    break;

                default:
                    break;
            }

            if (checkIfItIsStillTheSameDay() == 0) {
                if (typeOfBeverageComboBox.SelectedItem != null) {
                    choosedButton = choosedButton.Replace("L", string.Empty);
                    //test.Text = t.ToString();
                    double currentTextFromButton = double.Parse(choosedButton);

                    double currentValue = Math.Round(day.currentDrunk + currentTextFromButton, 2);
                    currentDrunkTextBlock.Text = currentValue.ToString();
                    day.currentDrunk = Math.Round(double.Parse(currentDrunkTextBlock.Text), 2);

                    //day.typeOfBeverage = typeOfBeverageComboBox.SelectedItem.ToString();

                    sendDayToDatabase();

                    stats.totalAmount = day.currentDrunk;
                    sendStatisticsToDatebase();
                    sendWeekToDatebase();
                    showWeeklyStatistics();

                }
                else {
                }
            }
            else {
            }
        }

        private void showWeeklyStatistics() {
            weeklyTotalTextBlock.Text = "0";
            double total = double.Parse(weeklyTotalTextBlock.Text);           
            foreach (Statistics s in week) {
                total += s.totalAmount;
            }
            weeklyTotalTextBlock.Text = total.ToString() + "L";
        }

        private void SetGoalButton_Click(object sender, RoutedEventArgs e) {
            SetGoalButton.Visibility = Visibility.Collapsed;
            SetGoalPanel.Visibility = Visibility.Visible;
        }

        private void SetGaolAcceptButton_Click(object sender, RoutedEventArgs e) {
            SetGoalButton.Visibility = Visibility.Visible;
            SetGoalPanel.Visibility = Visibility.Collapsed;


            day.goal = changeValuesOfGoalOrCurrent(goalTextBlock, SetGoalTextBox, day.goal);
            sendDayToDatabase();
        }

        private double changeValuesOfGoalOrCurrent(TextBlock futureValue, TextBox typedValue, double variable) {

            if (typedValue.Text != null) {

                if (typedValue.Text.Contains(",")) {
                    typedValue.Text = typedValue.Text.Replace(",", ".");
                }

                typedValue.Text = Regex.Replace(typedValue.Text, "[^0-9.]", "");        //remove all non-numeric signs
              
                variable = Math.Round(double.Parse(typedValue.Text), 2);

                futureValue.Text = variable.ToString();
                //sendDayToDatabase();

                typedValue.Text = "";

            }
            return variable;
        }




        private void setContentOfFirstButton_Click(object sender, RoutedEventArgs e) {
            setContentOfFirstButton.Visibility = Visibility.Collapsed;
            SetFirstButtonPanel.Visibility = Visibility.Visible;
        }
        private void SetFirstButtonAcceptButton_Click(object sender, RoutedEventArgs e) {
            setContentOfFirstButton.Visibility = Visibility.Visible;
            SetFirstButtonPanel.Visibility = Visibility.Collapsed;

            changeTextOnButtonWithCapacity(setFirstButtonTextBox, firststButton);

        }
     

        private void setContentOfSecondButton_Click(object sender, RoutedEventArgs e) {
            setContentOfSecondButton.Visibility = Visibility.Collapsed;
            SetSecondButtonPanel.Visibility = Visibility.Visible;
        }

        private void SetSecondButtonAcceptButton_Click(object sender, RoutedEventArgs e) {
            setContentOfSecondButton.Visibility = Visibility.Visible;
            SetSecondButtonPanel.Visibility = Visibility.Collapsed;

            changeTextOnButtonWithCapacity(setSecondButtonTextBox, secondButton);
        }

        private void setContentOfThirdButton_Click(object sender, RoutedEventArgs e) {
            setContentOfThirdButton.Visibility = Visibility.Collapsed;
            SetThirdButtonPanel.Visibility = Visibility.Visible;
        }

        private void SetThirdButtonAcceptButton_Click(object sender, RoutedEventArgs e) {
            setContentOfThirdButton.Visibility = Visibility.Visible;
            SetThirdButtonPanel.Visibility = Visibility.Collapsed;

            changeTextOnButtonWithCapacity(setThirdButtonTextBox, thirdButton);
        }

        private void setContentOfFourthButton_Click(object sender, RoutedEventArgs e) {
            setContentOfFourthButton.Visibility = Visibility.Collapsed;
            SetFourthButtonPanel.Visibility = Visibility.Visible;
        }

        private void SetFourthButtonAcceptButton_Click(object sender, RoutedEventArgs e) {
            setContentOfFourthButton.Visibility = Visibility.Visible;
            SetFourthButtonPanel.Visibility = Visibility.Collapsed;

            changeTextOnButtonWithCapacity(setFourthButtonTextBox, fouthButton);
        }


        void changeTextOnButtonWithCapacity(TextBox myTextBox, Button btn) {
            if (!myTextBox.Text.Equals(string.Empty)) {
                string newContent = myTextBox.Text;
                if (newContent.Contains(",")) {
                    newContent = newContent.Replace(",", ".");
                }

                newContent = Regex.Replace(newContent, "[^0-9.]", "");
                newContent += "L";

                btn.Content = newContent;
                myTextBox.Text = "";
            }
        }


        private void editAmountTillNowButton_Click(object sender, RoutedEventArgs e) {
            editAmountTillNowButton.Visibility = Visibility.Collapsed;
            editAmountTillNowPanel.Visibility = Visibility.Visible;
        }

        private void editAmountTillNowAcceptButton_Click(object sender, RoutedEventArgs e) {
            editAmountTillNowButton.Visibility = Visibility.Visible;
            editAmountTillNowPanel.Visibility = Visibility.Collapsed;

            day.currentDrunk = changeValuesOfGoalOrCurrent(currentDrunkTextBlock, editAmountTillNowTextBox, day.currentDrunk);
            sendDayToDatabase();
        }

        private void addNewBeverageButton_Click(object sender, RoutedEventArgs e) {
            addNewBeverageButton.Visibility = Visibility.Collapsed;
            addNewBeveragePanel.Visibility = Visibility.Visible;
        }

        private void addNewBeverageAcceptButton_Click(object sender, RoutedEventArgs e) {
            addNewBeverageButton.Visibility = Visibility.Visible;
            addNewBeveragePanel.Visibility = Visibility.Collapsed;

            if(AddNewBeverageTextBox.Text != null) {
                typeOfBeverageComboBox.Items.Add(AddNewBeverageTextBox.Text);
            }
        }

        private bool checkIfGoalIsAchieved() {
            if (day.currentDrunk >= day.goal)
                return true;
            else
                return false;
        }
       
    }
}
