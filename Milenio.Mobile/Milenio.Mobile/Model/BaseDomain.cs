using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mobile.Model
{
    public abstract class BaseDomain : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        protected  void OnPropertyChanged(string propertyName )
        {
            var handler = PropertyChanged;
            if(handler !=null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
