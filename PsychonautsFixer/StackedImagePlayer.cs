using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace PsychonautsFixer
{
    public class StackedImagePlayer : Control
    {
        public Image? Image { get; set; } = null;
        public int Frames { get; set; } = 4;
        public StackedImageDirection Direction { get; set; } = StackedImageDirection.LeftToRight;
        public int FrameDisplayTime
        {
            get
            {
                return animationTimer.Interval;
            }
            set
            {
                animationTimer.Interval = value;
            }
        }
        public bool AnimationEnabled
        {
            get
            {
                return animationTimer.Enabled;
            }
            set
            {
                animationTimer.Enabled = value;
            }
        }

        private Timer animationTimer;
        private int currentFrame = 0;
        private bool designMode;

        public StackedImagePlayer()
        {
            designMode = DesignMode || IsAncestorSiteInDesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            DoubleBuffered = true;

            animationTimer = new Timer()
            {
                Interval = 100,
                Enabled = false //!designMode
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
            currentFrame = (currentFrame + 1) % Frames;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //TextRenderer.DrawText(e.Graphics, designMode ? "DESIGN" : "RUNTIME", Font, Point.Empty, ForeColor);

            if (Image == null) return;
            e.Graphics.DrawImage(Image, ClientRectangle, new Rectangle(
                Direction == StackedImageDirection.LeftToRight ? (Width * currentFrame) : 0,
                Direction == StackedImageDirection.TopToBottom ? (Height * currentFrame) : 0,
                Width,
                Height
            ), GraphicsUnit.Pixel);
        }

        public void ResetAnimation()
        {
            currentFrame = 0;
        }
    }

    public enum StackedImageDirection
    {
        LeftToRight,
        TopToBottom
    }
}
