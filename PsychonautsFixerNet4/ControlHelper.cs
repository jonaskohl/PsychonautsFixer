using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace PsychonautsFixer
{
    public static class ControlHelper
    {
        public static void PaintBackground(Control instance, PaintEventArgs e, Rectangle rectangle, Color backColor, Point scrollOffset)
        {
            var method = typeof(Control)?.GetMethod(
                    "PaintBackground",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] {
                        typeof(PaintEventArgs),
                        typeof(Rectangle),
                        typeof(Color),
                        typeof(Point)
                    },
                    new ParameterModifier[] { }
                );
            if (method != null)
                method.Invoke(instance, new object[] { e, rectangle, backColor, scrollOffset });
        }
    }
}
