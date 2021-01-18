using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Calculator.Data
{
    public class SystemSwitchStyle : INotifyPropertyChanged
    {
        private bool isEnabled= false;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }
        }

        private string background = "#FFFFFF";
        public string Background
        {
            get { return background; }
            set
            {
                background = value;
                this.OnPropertyChanged("Background");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
