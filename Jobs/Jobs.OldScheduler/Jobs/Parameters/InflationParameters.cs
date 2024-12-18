using System;
using System.Collections.Generic;

namespace Jobs.OldScheduler.Jobs.Parameters
{
    public class InflationParameters : ParametersParser
    {
        public Dictionary<string, Action<string>> possibleParameters = new Dictionary<string, Action<string>>();
        public IEnumerable<DateTime> DatesToRun { get; set; }

        /// <summary>
        /// job pd -d=yyyy-MM-dd
        /// </summary>
        /// <param name="stringParameters"></param>
        public InflationParameters(IEnumerable<string> stringParameters)
        {
            possibleParameters.Add("-d", AssignParsersToRun);

            foreach (var stringParameter in stringParameters)
            {
                var inputParameter = GetParameter(stringParameter);
                possibleParameters[inputParameter.Name](inputParameter.Value);
            }
        }

        private void AssignParsersToRun(string parameterValue)
        {
            DatesToRun = GetList<DateTime>(parameterValue);
        }
    }
}
