using Jobs.OldScheduler.Jobs.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jobs.OldScheduler.Jobs
{
    public class PriceDetectiveParameters: ParametersParser
    {
        public Dictionary<string, Action<string>>  possibleParameters = new Dictionary<string, Action<string>>();
        public IEnumerable<int> ParsersToRun { get; set; }

        /// <summary>
        /// job pd -p=1,2,3
        /// </summary>
        /// <param name="stringParameters"></param>
        public PriceDetectiveParameters(IEnumerable<string> stringParameters)
        {
            possibleParameters.Add("-p", AssignParsersToRun);

            foreach (var stringParameter in stringParameters)
            {
                var inputParameter = GetParameter(stringParameter);
                possibleParameters[inputParameter.Name](inputParameter.Value);
            }
        }

        private void AssignParsersToRun(string parameterValue)
        {
            ParsersToRun = GetList<int>(parameterValue);
        }
    }
}
