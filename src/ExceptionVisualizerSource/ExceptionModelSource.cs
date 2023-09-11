using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace ExceptionVisualizerSource
{
    public class ExceptionModelSource : VisualizerObjectSource
    {
        /// <inheritdoc/>
        public override void GetData(object target, Stream outgoingData)
        {
            if (target is Exception ex)
            {
                var result = Convert(ex);
                SerializeAsJson(outgoingData, result);
            }
        }

        private static string Value(IDictionary data, object key)
        {
            var value = data[key];
            if (value == null) return string.Empty;
            return value.ToString();
        }

        private static ExceptionModel Convert(Exception e)
        {
            var model = new ExceptionModel
            {
                Message = e.Message,
                Type = e.GetType().FullName,
                HResult = ""+e.HResult,
                HelpLink = e.HelpLink,
                Source = e.Source,
                StackTrace = e.StackTrace,
                TargetSite = e.TargetSite?.ToString(),
            };
            model.Data = new List<KeyValuePair<string, string>>(e
                    .Data
                    .Keys
                    .Cast<object>()
                    .Where(k => !string.IsNullOrWhiteSpace(k.ToString()))
                    .Select(i => new KeyValuePair<string, string>(i.ToString(), Value(e.Data, i))));

            if (e is AggregateException aggregateException && aggregateException.InnerExceptions?.Count> 0)
            {
                foreach (var inner in aggregateException.InnerExceptions)
                {
                    model.InnerExceptions.Add(Convert(inner));
                }
            }
            else if (e.InnerException != null)
            {
                model.InnerExceptions.Add(Convert(e.InnerException));
            }

            return model;
        }
    }
}
