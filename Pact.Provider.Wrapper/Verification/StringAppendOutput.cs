using System.Collections.Generic;
using PactNet.Infrastructure.Outputters;

namespace Pact.Provider.Wrapper.Verification
{
    public class StringAppendOutput:IOutput
    {
        public string Log { get; private set; } = "";

        public void WriteLine(string line)
        {
            Log += line + "\n";
        }
    }
}