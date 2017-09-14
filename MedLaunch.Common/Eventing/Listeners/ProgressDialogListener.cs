using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common.IO.Compression;
using MedLaunch.Common.Eventing;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Common.Eventing.CustomEventArgs;

namespace MedLaunch.Common.Eventing.Listeners
{
    public class ProgressDialogListener
    {
        public ProgressDialogController Controller { get; set; }
        public SignatureType SigType { get; set; }

        public ProgressDialogListener(SignatureType sigType)
        {
            SigType = sigType;
        }

        public ProgressDialogListener(ProgressDialogController controller, SignatureType sigType)
        {
            if (controller != null)
                Controller = controller;

            SigType = sigType;
        }

        public void Subscribe(object a)
        {
            switch (SigType)
            {
                case SignatureType.Archive:
                    var obj = a as Archive;
                    obj.Message += new Archive.MessageHandler(MessageRevieved);
                    break;

            }
            //a.Message += new Archive.MessageHandler(MessageRevieved);
        }
        private void MessageRevieved(Archive a, ProgressDialogEventArgs e)
        {
            if (Controller == null)
            {
                Console.WriteLine(e.DialogText);
            }
            else
            {
                if (e.DialogText != null)
                    Controller.SetMessage(e.DialogText);
                if (e.DialogTitle != null)
                    Controller.SetTitle(e.DialogTitle);
            }
        }
    }
}
