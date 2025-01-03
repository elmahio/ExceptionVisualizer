﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExceptionVisualizerSource
{
    [DataContract]
    public class ExceptionModel
    {
        [DataMember]
        public string Message { get; internal set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public string Demystified { get; set; }

        [DataMember]
        public List<KeyValuePair<string, string>> Data { get; set; } = [];

        [DataMember]
        public List<KeyValuePair<string, string>> Properties { get; set; } = [];

        [DataMember]
        public List<ExceptionModel> InnerExceptions { get; set; } = [];

        [DataMember]
        public string Source { get; internal set; }

        [DataMember]
        public string TargetSite { get; internal set; }

        [DataMember]
        public int HResult { get; internal set; }

        [DataMember]
        public string HelpLink { get; internal set; }
    }
}
