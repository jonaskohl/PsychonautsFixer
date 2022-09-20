using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychonautsFixer
{
    public class HeaderLabel : Label
    {
        private Color _foreColor = Color.FromArgb(255, 0, 51, 153);
        private Color _lineColor = Color.FromArgb(255, 178, 193, 224);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                base.ForeColor = _foreColor;
            }
        }

        public HeaderLabel()
        {
            //base.Font = _font;
            base.ForeColor = _foreColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlHelper.PaintBackground(this, e, ClientRectangle, BackColor, Point.Empty);

            var tff = TextFormatFlags.Left;
            if (!UseMnemonic)
                tff |= TextFormatFlags.NoPrefix;

            var tsize = TextRenderer.MeasureText(Text, Font, Size, tff);
            var y = Height / 2;
            using (var p = new Pen(_lineColor, 1f))
                e.Graphics.DrawLine(p, tsize.Width + 4, y, Width, y);
            TextRenderer.DrawText(e.Graphics, Text, Font, Point.Empty, ForeColor, tff);
        }
    }
}
