namespace PsychonautsFixer;
public class ImageOnlyButton : Button
{
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.Style |= 0x40;
            return cp;
        }
    }
}
