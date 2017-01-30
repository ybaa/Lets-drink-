using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class MainPage : Page
    {
        Day day = new Day();
        
        public MainPage()
        {
            this.InitializeComponent();
            setInitialValues();

        }

        private void setInitialValues() {
            try {
                typeOfBeverageComboBox.Items.Add("water");
                typeOfBeverageComboBox.Items.Add("tea");
                typeOfBeverageComboBox.Items.Add("juice");
                typeOfBeverageComboBox.Items.Add("coffee");


                string jsonString = File.ReadAllText("Assets/day.json");                
                day = JsonConvert.DeserializeObject<Day>(jsonString);
                if (checkIfItIsStillTheSameDay() == true) {
                    currentDrunkTextBlock.Text = day.currentDrunk.ToString();
                }
                else {
                    currentDrunkTextBlock.Text = "0";
                }
                goalTextBlock.Text = day.goal.ToString();

            }
            catch {
                throw new NotImplementedException();
            }
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
            if (date == day.date)
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

                    string data = JsonConvert.SerializeObject(day);
                    File.WriteAllText("Assets/day.json", data);
                }
                else {
                }
            }
            else {
            }
        }
    }
}
