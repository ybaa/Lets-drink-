using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        Day day = new Day();

        public MainPage() {
            this.InitializeComponent();
            setInitialValues();

        }

        private void setInitialValues() {
            try {
                typeOfBeverageComboBox.Items.Add("water");
                typeOfBeverageComboBox.Items.Add("tea");
                typeOfBeverageComboBox.Items.Add("juice");
                typeOfBeverageComboBox.Items.Add("coffee");
                typeOfBeverageComboBox.SelectedIndex = 0;


                string jsonString = File.ReadAllText("Assets/day.json");
                day = JsonConvert.DeserializeObject<Day>(jsonString);
                if (checkIfItIsStillTheSameDay() == true) {
                    currentDrunkTextBlock.Text = day.currentDrunk.ToString();
                }
                else {
                    currentDrunkTextBlock.Text = "0";
                    day.date = DateTime.Now.ToString("dd-MM-yyyy");
                }
                goalTextBlock.Text = day.goal.ToString();

                day.currentDrunk = double.Parse(currentDrunkTextBlock.Text);

                sendDayToDatabase();

            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void sendDayToDatabase() {
            string data = JsonConvert.SerializeObject(day);
            File.WriteAllText("Assets/day.json", data);
        }

        private void firststButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(1);
            }
            catch {
                throw new NotImplementedException();
            }


        }

        private bool checkIfItIsStillTheSameDay() {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            if (date.Equals(day.date))
                return true;
            else
                return false;
        }

        private void secondButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(2);
            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void thirdButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(3);
            }
            catch {
                throw new NotImplementedException();
            }
        }

        private void fouthButton_Click(object sender, RoutedEventArgs e) {
            try {
                addToCurrentAfterClickButton(4);
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

            if (checkIfItIsStillTheSameDay() == true) {
                if (typeOfBeverageComboBox.SelectedItem != null) {
                    choosedButton = choosedButton.Replace("L", string.Empty);
                    //test.Text = t.ToString();
                    double currentTextFromButton = double.Parse(choosedButton);

                    double currentValue = Math.Round(day.currentDrunk + currentTextFromButton, 2);
                    currentDrunkTextBlock.Text = currentValue.ToString();
                    day.currentDrunk = Math.Round(double.Parse(currentDrunkTextBlock.Text), 2);

                    day.typeOfBeverage = typeOfBeverageComboBox.SelectedItem.ToString();

                    sendDayToDatabase();
                }
                else {
                }
            }
            else {
            }
        }

        private void SetGoalButton_Click(object sender, RoutedEventArgs e) {
            SetGoalButton.Visibility = Visibility.Collapsed;
            SetGoalPanel.Visibility = Visibility.Visible;
        }

        private void SetGaolAcceptButton_Click(object sender, RoutedEventArgs e) {
            SetGoalButton.Visibility = Visibility.Visible;
            SetGoalPanel.Visibility = Visibility.Collapsed;


            //if (SetGoalTextBox.Text != null) {

            //    if (SetGoalTextBox.Text.Contains(",")) {
            //       SetGoalTextBox.Text = SetGoalTextBox.Text.Replace(",", ".");
            //    }

            //    SetGoalTextBox.Text = Regex.Replace(SetGoalTextBox.Text, "[^0-9.]", "");        //remove all non-numeric signs


            //    day.goal = Math.Round(double.Parse(SetGoalTextBox.Text), 2);
            //    goalTextBlock.Text = day.goal.ToString();
            //    sendDayToDatabase();

            //    SetGoalTextBox.Text = "";
            //}
            changeValuesOfGoalOrCurrent(goalTextBlock, SetGoalTextBox);
        }

        void changeValuesOfGoalOrCurrent(TextBlock futureValue, TextBox typedValue) {

            if (typedValue.Text != null) {

                if (typedValue.Text.Contains(",")) {
                    typedValue.Text = typedValue.Text.Replace(",", ".");
                }

                typedValue.Text = Regex.Replace(typedValue.Text, "[^0-9.]", "");        //remove all non-numeric signs
              
                day.goal = Math.Round(double.Parse(typedValue.Text), 2);
                               
                futureValue.Text = day.goal.ToString();
                sendDayToDatabase();

                typedValue.Text = "";
            }
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
            if (myTextBox.Text != null) {
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

            changeValuesOfGoalOrCurrent(currentDrunkTextBlock, editAmountTillNowTextBox);
        }

        private void addNewBeverageButton_Click(object sender, RoutedEventArgs e) {
            addNewBeverageButton.Visibility = Visibility.Collapsed;
            addNewBeveragePanel.Visibility = Visibility.Visible;
        }

        private void addNewBeverageAcceptButton_Click(object sender, RoutedEventArgs e) {
            addNewBeverageButton.Visibility = Visibility.Visible;
            addNewBeveragePanel.Visibility = Visibility.Collapsed;
        }

       
    }
}
