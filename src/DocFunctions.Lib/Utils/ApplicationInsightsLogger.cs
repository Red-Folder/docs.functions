using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using DocFunctions.Lib.Utils;

namespace DocFunctions.Lib.Utils
{
    public class ApplicationInsightsLogger : ILogger
    {
        private TelemetryClient _client = new TelemetryClient();

        public ApplicationInsightsLogger(string key)
        {
            TelemetryConfiguration.Active.InstrumentationKey = key;
        }

        public object StartOperation(string operationName)
        {
            var state = _client.StartOperation<RequestTelemetry>(operationName);
            return state;
        }

        public void EndOperation(object state)
        {
            if (state is IOperationHolder<RequestTelemetry>)
            {
                _client.StopOperation(state as IOperationHolder<RequestTelemetry>);
            }
        }

        public void Info(string message)
        {
            _client.TrackTrace(message, SeverityLevel.Information);
        }
    }
}
