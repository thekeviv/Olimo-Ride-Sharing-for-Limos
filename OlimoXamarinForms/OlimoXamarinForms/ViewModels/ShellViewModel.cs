using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OlimoXamarinForms.ViewModels
{
    class ShellViewModel : INotifyPropertyChanged
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                this.userName = value;
                OnPropertyChanged("UserName");
            }
        }
        public ShellViewModel()
        {
            this.UserName = App.UserName;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
