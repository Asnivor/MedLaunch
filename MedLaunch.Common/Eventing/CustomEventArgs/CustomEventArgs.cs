using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common.Eventing.CustomEventArgs
{
    public class ProgressDialogEventArgs : EventArgs
    {
        public int IntArg { get; set; }
        public int CurrentValue { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public string DialogTitle { get; set; }
        public string DialogText { get; set; }
    }

    public class ExampleEventArgs : EventArgs
    {
        public int IntArg { get; set; }
    }
}
