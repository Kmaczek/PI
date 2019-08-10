using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.OldScheduler
{
    interface IConsoleCommander
    {
        void ListenToCommands(string command);
    }
}
