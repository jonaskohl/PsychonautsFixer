using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PsychonautsFixer
{
    public partial class LocatorForm : Form
    {
        private string installLocation = null;

        public LocatorForm()
        {
            InitializeComponent();
            locateButton.Hide();
        }

        private void SetProgressLabel(int current, int total)
        {
            SetProgressLabel($"Checking program entry {current + 1} of {total}\u2026");
        }

        private void SetProgressLabel(string text)
        {
            Invoke(new MethodInvoker(() =>
            {
                statusLabel.Text = text;
            }));
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var skipSearch = (ModifierKeys == Keys.Shift);

            DoSearch(skipSearch);
        }

        private void DoSearch(bool skipSearch)
        {
            stackedImagePlayer1.Size = new Size(64, 64);
            stackedImagePlayer1.Frames = 5;
            stackedImagePlayer1.Image = Properties.Resources.brains_bordered_withempty;
            stackedImagePlayer1.FrameDisplayTime = 100;
            stackedImagePlayer1.Direction = StackedImageDirection.LeftToRight;
            stackedImagePlayer1.AnimationEnabled = true;
            statusLabel.Text = "Retrieving installed programs\u2026";
            locateButton.Hide();
            searchButton.Hide();

            Task.Factory.StartNew(() => FindGame().Result).ContinueWith(task =>
            {
                installLocation = skipSearch ? null : task.Result;
                Invoke(new MethodInvoker(() =>
                {
                    if (installLocation == null)
                    {
                        stackedImagePlayer1.Frames = 1;
                        stackedImagePlayer1.Image = Properties.Resources.letter;
                        stackedImagePlayer1.AnimationEnabled = false;
                        stackedImagePlayer1.ResetAnimation();
                        stackedImagePlayer1.Invalidate();
                        if (skipSearch)
                            SetProgressLabel("You chose to locate the game's location manually by holding SHIFT when launching Psychonauts Fixer. Please press \"Locate\" to manually choose the location of Psychonauts or press \"Search\" to search for the game automatically.");
                        else
                            SetProgressLabel("The game's location could not be determined automatically. Please choose the path of your Psychonauts game manually.");
                        System.Media.SystemSounds.Asterisk.Play();
                        locateButton.Show();
                        if (skipSearch) searchButton.Show();
                        PerformLayout();
                        Focus();
                    }
                    else DialogResult = DialogResult.OK;
                }));
            });
        }

        Task<string> FindGame()
        {
            return Task.Factory.StartNew(() =>
            {
                string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    var subKeyNames = key.GetSubKeyNames();
                    for (var i = 0; i < subKeyNames.Length; ++i)
                    {
                        SetProgressLabel(i, subKeyNames.Length);
                        var subkeyName = subKeyNames[i];
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            var displayName = subkey?.GetValue("DisplayName") as string;
                            if (!"Psychonauts".Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
                                continue;
                            return subkey?.GetValue("InstallLocation") as string;
                        }
                    }
                }

                return null;
            });
        }

        public static string GetGameLocation()
        {
            using (var instance = new LocatorForm())
            {
                var result = instance.ShowDialog();
                if (result == DialogResult.OK)
                    return instance.installLocation;
                else
                    return null;
            }
        }

        private void locateButton_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog()
            {
                FileName = "Psychonauts.exe",
                Filter = "Psychonauts.exe|Psychonauts.exe|*.exe|*.exe|*.*|*.*"
            })
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    installLocation = Path.GetDirectoryName(openDialog.FileName);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }
    }
}
