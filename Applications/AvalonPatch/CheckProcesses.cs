using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace AvalonPatch
{
    // Class to check if a process is active
    class CheckProcesses
    {
        private string _pName;
        public CheckProcesses(string pName)
        {
            this._pName = pName;
        }

        public bool running
        {
            get
            {
                Process[] processes = Process.GetProcessesByName(_pName);
                if (processes.Length == 0)
                    return false;
                return true;
            }
        }

        public bool kill()
        {
            foreach (Process actProcess in Process.GetProcesses())
            {
                if (actProcess.ProcessName.ToLower() == _pName)
                    actProcess.Kill();
            }
            return true;
        }
    }
}
