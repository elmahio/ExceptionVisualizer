using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ExceptionVisualizer.Models
{
    [DataContract]
    public class ViewModel : INotifyPropertyChanged
    {
        [DataMember]
        public ObservableCollection<ExceptionViewModel> Exceptions { get; set; } = new ObservableCollection<ExceptionViewModel>();


        public event PropertyChangedEventHandler PropertyChanged;
        private ExceptionViewModel selectedItem;

        [DataMember]
        public ExceptionViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
    }
}
