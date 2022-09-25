using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PsychonautsFixer
{
    public partial class ProfileSelectorForm : Form
    {
        string selectedProfile = "";
        string installLocation = "";

        public ProfileSelectorForm()
        {
            InitializeComponent();
        }

        private static string[] GetProfileNames(string installLocation)
        {
            var profilesFolder = Path.Combine(installLocation, "profiles");
            return new[] { 1, 2, 3 }
                .Select(i =>
                {
                    var profile = $"profile {i}";
                    var folder = Path.Combine(profilesFolder, profile);
                    if (!Directory.Exists(folder))
                        return null;
                    var files = Directory.GetFiles(folder);
                    var prFile = files.FirstOrDefault(f => {
                        var b = Path.GetFileName(f);
                        return b.StartsWith($"{profile}- ", StringComparison.InvariantCultureIgnoreCase) && !b.EndsWith(".ini", StringComparison.InvariantCultureIgnoreCase);
                    });
                    if (prFile == null)
                        return null;
                    var profileBytes = File.ReadAllBytes(prFile);
                    var profileNameBytes = profileBytes.Skip(8).Take(8).Where(b => b != 0).ToArray();
                    return Encoding.ASCII.GetString(profileNameBytes);
                }).ToArray();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var profileNames = GetProfileNames(installLocation);

            if (!string.IsNullOrEmpty(profileNames[0]))
                buttonProfile1.Text += Environment.NewLine + profileNames[0];
            else
            {
                buttonProfile1.Enabled = false;
                buttonProfile1.Text += Environment.NewLine + "(Empty)";
            }

            if (!string.IsNullOrEmpty(profileNames[1]))
                buttonProfile2.Text += Environment.NewLine + profileNames[1];
            else
            {
                buttonProfile2.Enabled = false;
                buttonProfile2.Text += Environment.NewLine + "(Empty)";
            }

            if (!string.IsNullOrEmpty(profileNames[2]))
                buttonProfile3.Text += Environment.NewLine + profileNames[2];
            else
            {
                buttonProfile3.Enabled = false;
                buttonProfile3.Text += Environment.NewLine + "(Empty)";
            }
        }

        public static string GetProfile(string installLocation)
        {
            using (var f = new ProfileSelectorForm())
            {
                f.installLocation = installLocation;
                if (f.ShowDialog() == DialogResult.OK)
                    return f.selectedProfile;
                return null;
            }
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            selectedProfile = (sender as Button)?.Tag.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
