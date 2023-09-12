using System.Runtime.Serialization;

namespace ExceptionVisualizer.Models
{
    [DataContract]
    public class DataViewModel
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
