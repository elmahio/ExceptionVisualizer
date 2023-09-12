using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ExceptionVisualizer.Models
{
    [DataContract]
    public class ExceptionViewModel : INotifyPropertyChanged
    {
        [DataMember]
        public string Message { get; internal set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public ObservableCollection<DataViewModel> Data { get; set; } = new ObservableCollection<DataViewModel>();

        [DataMember]
        public ObservableCollection<ExceptionViewModel> InnerExceptions { get; set; } = new ObservableCollection<ExceptionViewModel>();

        [DataMember]
        public string Source { get; internal set; }

        [DataMember]
        public string TargetSite { get; internal set; }

        [DataMember]
        public string HResult { get; internal set; }

        [DataMember]
        public string HelpLink { get; internal set; }

        private bool isSelected;

        [DataMember]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
