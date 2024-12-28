using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

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
                HResult = e.HResult,
                HelpLink = e.HelpLink,
                Source = e.Source,
                StackTrace = e.StackTrace,
                TargetSite = e.TargetSite?.ToString(),
                Data = new List<KeyValuePair<string, string>>(e
                    .Data
                    .Keys
                    .Cast<object>()
                    .Where(k => !string.IsNullOrWhiteSpace(k.ToString()))
                    .Select(i => new KeyValuePair<string, string>(i.ToString(), Value(e.Data, i)))),
                Properties = e
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.CanRead)
                    .Where(p => p.PropertyType.IsSubclassOf(typeof(ValueType)) || p.PropertyType.Equals(typeof(string)))
                    .Where(p => p.Name != "Message" && p.Name != "Source" && p.Name != "HResult" && p.Name != "HelpLink" && p.Name != "TargetSite" && p.Name != "StackTrace" && p.GetValue(e) != null)
                    .Select(p => new KeyValuePair<string, string>(p.Name, p.GetValue(e)?.ToString()))
                    .ToList()
            };
            var stackTrace = new EnhancedStackTrace(e);
            if (stackTrace.FrameCount > 0)
            {
                model.Demystified = stackTrace.ToString();
            }

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
