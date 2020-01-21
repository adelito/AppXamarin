using Mobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Milenio.Mobile.ViewModel
{
    public class RadioButtonViewModel : INotifyPropertyChanged
    {
        public RadioButtonViewModel()
        {
            MyList = new ObservableCollection<RadioButton>();
            FillData();
        }
        public IList<RadioButton> MyList { get; set; }
        private RadioButton _selectedItem;
        public RadioButton SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        void FillData()
        {
            for (int i = 0; i < 3; i++)
            {
                MyList.Add(new RadioButton { id = i, Name = "Option " + i });
            }
        }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
