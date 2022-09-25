using System;
using System.Windows.Forms;

namespace PsychonautsFixer
{
    public class ImageOnlyButton : Button
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (Environment.OSVersion.Version.Major >= 6)
                    cp.Style |= 0x40;
                return cp;
            }
        }
    }
}
