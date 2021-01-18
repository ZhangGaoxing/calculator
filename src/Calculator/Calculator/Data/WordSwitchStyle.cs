using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Calculator.Data
{
    public class WordSwitchStyle : INotifyPropertyChanged
    {
        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }
        }

        private string foreground = "#FFFFFF";
        public string Foreground
        {
            get { return foreground; }
            set
            {
                Foreground = value;
                this.OnPropertyChanged("Foreground");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
