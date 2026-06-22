using System.Runtime.Serialization;
using System.Windows;

namespace ExceptionVisualizer.Models
{
    [DataContract]
    public class StackFrameViewModel
    {
        // For non-frame lines (exception header, blank lines, etc.)
        [DataMember]
        public string RawText { get; set; } = string.Empty;

        [DataMember]
        public Visibility RawTextVisibility { get; set; } = Visibility.Visible;

        // Parsed frame parts
        [DataMember]
        public Visibility ParsedVisibility { get; set; } = Visibility.Collapsed;

        [DataMember]
        public string Prefix { get; set; } = string.Empty;

        [DataMember]
        public string Namespace { get; set; } = string.Empty;

        [DataMember]
        public string ClassName { get; set; } = string.Empty;

        [DataMember]
        public string MethodDot { get; set; } = string.Empty;

        [DataMember]
        public string MethodName { get; set; } = string.Empty;

        [DataMember]
        public string Parameters { get; set; } = string.Empty;

        // File/line parts (shown when source info is available)
        [DataMember]
        public string InPart { get; set; } = string.Empty;

        [DataMember]
        public string FilePath { get; set; } = string.Empty;

        [DataMember]
        public string FileLine { get; set; } = string.Empty;

        [DataMember]
        public string FileNavigationParam { get; set; } = string.Empty;

        [DataMember]
        public Visibility FileLinkVisibility { get; set; } = Visibility.Collapsed;
    }
}
