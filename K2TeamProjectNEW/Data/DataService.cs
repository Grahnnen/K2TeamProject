using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2TeamProjectNEW.Data
{
    public class DataService
    {
        public CodeFirstContext CodeFirst { get; }
        public DatabaseFirstContext DatabaseFirst { get; }

        public DataService(CodeFirstContext cf, DatabaseFirstContext df)
        {
            CodeFirst = cf;
            DatabaseFirst = df;
        }
    }
}
