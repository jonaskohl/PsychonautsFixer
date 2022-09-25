using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PsychonautsFixer
{
    public partial class UnsavedChangesForm : Form
    {
        public UnsavedChangesForm()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(IWin32Window owner, string message, string title)
        {
            using (var win = new UnsavedChangesForm())
            {
                win.Text = title;
                win.label1.Text = message;
                return win.ShowDialog(owner);
            }
        }
    }
}
