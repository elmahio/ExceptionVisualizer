using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExceptionVisualizerSource
{
    [DataContract]
    public class ExceptionModel
    {
        public string Id { get; set; } = $"i{Guid.NewGuid().ToString().Replace("-", "")}";

        [DataMember]
        public string Message { get; internal set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public List<KeyValuePair<string, string>> Data { get; set; } = new List<KeyValuePair<string, string>>();

        [DataMember]
        public List<ExceptionModel> InnerExceptions { get; set; } = new List<ExceptionModel>();

        [DataMember]
        public string Source { get; internal set; }
        [DataMember]
        public string TargetSite { get; internal set; }

        [DataMember]
        public string HResult { get; internal set; }

        [DataMember]
        public string HelpLink { get; internal set; }
    }
}
