using System;
using System.Collections.Generic;
using System.Text;

namespace Dneedy.Specflow.Synchotron.Testing
{
    public class Logger : ISynchotronLog
    {
        public List<string> Lines = new List<string>();

        public void Debug(string line)
        {
            Lines.Add(line);
        }
    }
}
