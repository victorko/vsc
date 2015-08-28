using System;

namespace Vsc.Core
{
    public class NotifyMicroActionEventArgs : EventArgs
    {
        internal NotifyMicroActionEventArgs(string actionDescription)
        {
            this.ActionDescription = actionDescription;
        }

        public string ActionDescription { get; private set; }
    }
}