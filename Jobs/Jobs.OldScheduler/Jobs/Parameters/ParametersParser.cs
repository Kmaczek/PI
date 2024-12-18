using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs.OldScheduler.Jobs.Parameters
{
    public class ParametersParser
    {
        protected InputParameter GetParameter(string paramString)
        {
            var splitParamString = paramString.Split('=');

            return new InputParameter()
            {
                Name = splitParamString[0],
                Value = splitParamString[1]
            };
        }

        protected IEnumerable<T> GetList<T>(string parameter)
        {
            return parameter.Split(',').Select(id => (T)Convert.ChangeType(id, typeof(T)));
        }
    }
}
