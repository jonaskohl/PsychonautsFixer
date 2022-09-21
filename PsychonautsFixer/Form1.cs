using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace PsychonautsFixer
{
    public partial class Form1 : Form
    {
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_CONTEXTHELP = 0xf180;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        Icon folderIcon, helpIcon, pcIcon, userIcon;

        string installLocation = "";
        string? profile = null;

        string? displayIniLocation, audioIniLocation, profileIniLocation;
        IniFile? displayIniFile, audioIniFile, profileIniFile;

        bool _hasUnsavedChanges = false;
        bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set
            {
                _hasUnsavedChanges = value;
                RefreshApplyButton();
            }
        }

        private void RefreshApplyButton()
        {
            applyButton.Enabled = _hasUnsavedChanges;
        }

        public Form1()
        {
            InitializeComponent();

            RefreshApplyButton();

            versionLabel.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version}%n{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright}".Replace("%n", Environment.NewLine);

            folderIcon = SysIcons.GetSystemIcon(SysIcons.SHSTOCKICONID.SIID_FOLDEROPEN, SysIcons.IconSize.Small);
            helpIcon = SysIcons.GetSystemIcon(SysIcons.SHSTOCKICONID.SIID_HELP, SysIcons.IconSize.Small);
            pcIcon = SysIcons.GetSystemIcon(SysIcons.SHSTOCKICONID.SIID_DESKTOPPC, SysIcons.IconSize.Small);
            userIcon = SysIcons.GetSystemIcon(SysIcons.SHSTOCKICONID.SIID_USERS, SysIcons.IconSize.Small);

            Shown += Form1_Shown;
            FormClosing += Form1_FormClosing;
            launchButton.HandleCreated += LaunchButton_SetImage;
            browseButton.HandleCreated += BrowseButton_SetImage;
            helpButton.HandleCreated += HelpButton_SetImage;
            getFromScreenButton.HandleCreated += GetFromScreenButton_SetImage;
            profileButton.HandleCreated += ProfileButton_SetImage;
        }

        private void ProfileButton_SetImage(object? sender, EventArgs e)
        {
            profileButton.SetWin32Icon(userIcon);
        }

        private void GetFromScreenButton_SetImage(object? sender, EventArgs e)
        {
            getFromScreenButton.SetWin32Icon(pcIcon);
        }

        private void HelpButton_SetImage(object? sender, EventArgs e)
        {
            helpButton.SetWin32Icon(helpIcon);
        }

        private void LaunchButton_SetImage(object? sender, EventArgs e)
        {
            launchButton.SetWin32Icon(Properties.Resources.PSYCHO);
        }

        private void BrowseButton_SetImage(object? sender, EventArgs e)
        {
            browseButton.SetWin32Icon(folderIcon);
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = !WindowAskClose();
            }
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            var location =
                args.Any(a => a.StartsWith("/location:")) ?
                args.First(a => a.StartsWith("/location:")).Substring(10) :
                LocatorForm.GetGameLocation();

            if (location == null)
            {
                Close();
                return;
            }
            else installLocation = location;

            try
            {
                var fn = Path.Combine(location, "PSYFIX.TMP");
                File.Create(fn).Close();
                File.Delete(fn);
            }
            catch (UnauthorizedAccessException)
            {
                if (SelfElevate(new[] { $"/location:{location}" }))
                {
                    Close();
                    return;
                }
            }

            displayIniLocation = Path.Combine(installLocation, "DisplaySettings.ini");

            if (File.Exists(displayIniLocation))
                displayIniFile = IniFile.Load(displayIniLocation);
            else
                displayIniFile = new();

            if (displayIniFile.ContainsSection("DisplaySettings"))
            {
                var displaySection = displayIniFile.GetSection("DisplaySettings");

                if (displaySection != null)
                {
                    string widthString = resWidth.ToString(), heightString = resHeight.ToString();

                    if (displaySection.ContainsKey("ScreenWidth"))
                        widthString = displaySection.Get("ScreenWidth");

                    if (displaySection.ContainsKey("ScreenHeight"))
                        heightString = displaySection.Get("ScreenHeight");

                    if (int.TryParse(widthString, out int parsedWidth))
                        resWidth.Value = parsedWidth;

                    if (int.TryParse(heightString, out int parsedHeight))
                        resHeight.Value = parsedHeight;

                    checkFullscreen.Checked = "true".Equals(displaySection.TryGet("FullScreen", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkFSAA.Checked = "true".Equals(displaySection.TryGet("FSAA", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkFSFX.Checked = "true".Equals(displaySection.TryGet("FSFX", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkVSync.Checked = "true".Equals(displaySection.TryGet("VSync", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkAdvancedShading.Checked = "true".Equals(displaySection.TryGet("AdvancedShading", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkShadows.Checked = "true".Equals(displaySection.TryGet("Shadows", "false"), StringComparison.InvariantCultureIgnoreCase);

                    var gammaString = displaySection.TryGet("GammaCorrection", "1");

                    if (decimal.TryParse(gammaString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedGamma))
                    {
                        trackGamma.Value = (int)(parsedGamma * 1e6m);
                        spinnerGamma.Value = parsedGamma;
                    }
                }
            }

            audioIniLocation = Path.Combine(installLocation, "AudioSettings.ini");

            if (File.Exists(audioIniLocation))
                audioIniFile = IniFile.Load(audioIniLocation);
            else
                audioIniFile = new();

            if (audioIniFile.ContainsSection("AudioSettings"))
            {
                var audioSection = audioIniFile.GetSection("AudioSettings");

                if (audioSection != null)
                {
                    checkSubtitles.Checked = "true".Equals(audioSection.TryGet("ShowSubtitles", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkHWAudio.Checked = "true".Equals(audioSection.TryGet("HardwareAccel", "false"), StringComparison.InvariantCultureIgnoreCase);
                    checkEAX.Checked = "true".Equals(audioSection.TryGet("UseEAX", "false"), StringComparison.InvariantCultureIgnoreCase);

                    var volMasterString = audioSection.TryGet("MasterVolume", "1");

                    if (decimal.TryParse(volMasterString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedVolMaster))
                    {
                        trackVolMaster.Value = (int)(parsedVolMaster * 1e6m);
                        spinnerVolMaster.Value = parsedVolMaster;
                    }

                    var volFXString = audioSection.TryGet("FXVolume", "1");

                    if (decimal.TryParse(volFXString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedVolFX))
                    {
                        trackVolFX.Value = (int)(parsedVolFX * 1e6m);
                        spinnerVolFX.Value = parsedVolFX;
                    }

                    var volMusicString = audioSection.TryGet("MusicVolume", "1");

                    if (decimal.TryParse(volMusicString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedVolMusic))
                    {
                        trackVolMusic.Value = (int)(parsedVolMusic * 1e6m);
                        spinnerVolMusic.Value = parsedVolMusic;
                    }

                    var volVoicesString = audioSection.TryGet("VoiceVolume", "1");

                    if (decimal.TryParse(volVoicesString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedVolVoices))
                    {
                        trackVolVoices.Value = (int)(parsedVolVoices * 1e6m);
                        spinnerVolVoices.Value = parsedVolVoices;
                    }
                }
            }

            var cutsceneFolder = Path.Combine(installLocation, "WorkResource", "cutscenes", "prerendered");
            checkDFLogo.Enabled = File.Exists(Path.Combine(cutsceneFolder, "DFLogo.bik")) || File.Exists(Path.Combine(cutsceneFolder, "DFLogo.bik.DISABLED"));
            checkMajescoLogo.Enabled = File.Exists(Path.Combine(cutsceneFolder, "MajescoLogo.bik")) || File.Exists(Path.Combine(cutsceneFolder, "MajescoLogo.bik.DISABLED"));
            checkDFLogo.Checked = !File.Exists(Path.Combine(cutsceneFolder, "DFLogo.bik"));
            checkMajescoLogo.Checked = !File.Exists(Path.Combine(cutsceneFolder, "MajescoLogo.bik"));

            HasUnsavedChanges = false;

            if (!PromptProfileSelect())
            {
                Close();
            }

            HasUnsavedChanges = false;
        }

        private bool PromptProfileSelect()
        {
            profile = ProfileSelectorForm.GetProfile2(installLocation);

            if (profile is null)
            {
                return false;
            }
            else if (profile == "")
            {
                checkRumble.Enabled = false;
                checkRumble.Checked = false;
            }
            else
            {
                checkRumble.Enabled = true;
            }

            if (profile != "")
            {
                var profileDir = Path.Combine(installLocation, "profiles", profile);
                var profileFiles = Directory.GetFiles(profileDir, "*.ini");
                if (profileFiles.Length > 0)
                {
                    profileIniLocation = Path.Combine(profileDir, profileFiles[0]);

                    if (File.Exists(profileIniLocation))
                        profileIniFile = IniFile.Load(profileIniLocation);
                    else
                        profileIniFile = new();

                    if (profileIniFile.ContainsSection("Global"))
                    {
                        var globalSection = profileIniFile.GetSection("Global");

                        if (globalSection != null)
                        {
                            checkRumble.Checked = "true".Equals(globalSection.TryGet("EnableRumble", "false"), StringComparison.InvariantCultureIgnoreCase);
                        }
                    }
                }
            }

            return true;
        }

        private void SelfElevate()
        {
            SelfElevate(new string[0]);
        }

        private static bool IsElevated()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private bool SelfElevate(string[] args)
        {
            if (IsElevated())
                return false;

            var process = Process.GetCurrentProcess();
            if (process is null)
                return false;

            var currentStartInfo = new ProcessStartInfo();
            currentStartInfo.FileName = Application.ExecutablePath;
            currentStartInfo.WorkingDirectory = Environment.CurrentDirectory;
            currentStartInfo.UseShellExecute = true;
            currentStartInfo.Verb = "runas";
            foreach (var arg in args)
                currentStartInfo.ArgumentList.Add(arg);

            Application.Exit();
            Process.Start(currentStartInfo);
            return true;
        }

        private void getFromScreenButton_Click(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);
            resWidth.Value = screen.Bounds.Width;
            resHeight.Value = screen.Bounds.Height;
            HasUnsavedChanges = true;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            WindowApply();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            WindowClose();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            WindowOk();
        }

        private void WindowApply()
        {
            // Apply video settings to ini
            displayIniFile.Set("DisplaySettings", "ScreenWidth", resWidth.Value.ToString());
            displayIniFile.Set("DisplaySettings", "ScreenHeight", resHeight.Value.ToString());
            displayIniFile.Set("DisplaySettings", "FullScreen", checkFullscreen.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "VSync", checkVSync.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "FSAA", checkFSAA.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "FSFX", checkFSFX.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "AdvancedShading", checkAdvancedShading.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "Shadows", checkShadows.Checked ? "true" : "false");
            displayIniFile.Set("DisplaySettings", "GammaCorrection", spinnerGamma.Value.ToString("0.000000", CultureInfo.InvariantCulture));
            File.WriteAllText(displayIniLocation, displayIniFile.ToString());

            // Apply QOL settings
            var cutsceneFolder = Path.Combine(installLocation, "WorkResource", "cutscenes", "prerendered");
            if (checkDFLogo.Checked)
                TryRename(cutsceneFolder, "DFLogo.bik", "DFLogo.bik.DISABLED");
            else
                TryRename(cutsceneFolder, "DFLogo.bik.DISABLED", "DFLogo.bik");

            if (checkMajescoLogo.Checked)
                TryRename(cutsceneFolder, "MajescoLogo.bik", "MajescoLogo.bik.DISABLED");
            else
                TryRename(cutsceneFolder, "MajescoLogo.bik.DISABLED", "MajescoLogo.bik");

            if (profile != "")
            {
                profileIniFile.Set("Global", "EnableRumble", checkRumble.Checked ? "true" : "false");
                File.WriteAllText(profileIniLocation, profileIniFile.ToString());
            }

            // Apply audio settings to ini
            audioIniFile.Set("AudioSettings", "MasterVolume", spinnerVolMaster.Value.ToString("0.000000", CultureInfo.InvariantCulture));
            audioIniFile.Set("AudioSettings", "FXVolume", spinnerVolFX.Value.ToString("0.000000", CultureInfo.InvariantCulture));
            audioIniFile.Set("AudioSettings", "MusicVolume", spinnerVolMusic.Value.ToString("0.000000", CultureInfo.InvariantCulture));
            audioIniFile.Set("AudioSettings", "VoiceVolume", spinnerVolVoices.Value.ToString("0.000000", CultureInfo.InvariantCulture));
            audioIniFile.Set("AudioSettings", "ShowSubtitles", checkSubtitles.Checked ? "true" : "false");
            audioIniFile.Set("AudioSettings", "HardwareAccel", checkHWAudio.Checked ? "true" : "false");
            audioIniFile.Set("AudioSettings", "UseEAX", checkEAX.Checked ? "true" : "false");
            File.WriteAllText(audioIniLocation, audioIniFile.ToString());

            HasUnsavedChanges = false;
        }

        private void TryRename(string folder, string fileFrom, string fileTo)
        {
            var pathFrom = Path.Combine(folder, fileFrom);
            var pathTo = Path.Combine(folder, fileTo);

            if (File.Exists(pathFrom))
                File.Move(pathFrom, pathTo);
        }

        private void WindowOk()
        {
            if (HasUnsavedChanges)
                WindowApply();
            WindowClose();
        }

        private void WindowClose()
        {
            Close();
        }

        private bool WindowAskClose()
        {
            if (!HasUnsavedChanges) return true;

            var res = TaskDialog.ShowDialog(new TaskDialogPage()
            {
                Buttons = new TaskDialogButtonCollection()
                {
                    new TaskDialogButton("Save"),
                    new TaskDialogButton("Discard"),
                    new TaskDialogButton("Cancel")
                },
                Icon = TaskDialogIcon.Warning,
                Caption = "Psychonauts Fixer",
                Text = "There are unsaved changes. Do you want to save them?"
            });

            if (res.Text == "Save")
            {
                WindowApply();
            }

            return res.Text != "Cancel";
        }

        private void AnyCheckChanged(object? sender, EventArgs e)
        {
            HasUnsavedChanges = true;
        }

        private void trackGamma_Scroll(object sender, EventArgs e)
        {
            spinnerGamma.ValueChanged -= spinnerGamma_ValueChanged;
            spinnerGamma.Value = trackGamma.Value / 1e6m;
            spinnerGamma.ValueChanged += spinnerGamma_ValueChanged;
            HasUnsavedChanges = true;
        }

        private void spinnerGamma_ValueChanged(object? sender, EventArgs e)
        {
            trackGamma.Value = (int)(spinnerGamma.Value * 1e6m);
            HasUnsavedChanges = true;
        }
        private void trackVolMaster_Scroll(object sender, EventArgs e)
        {
            spinnerVolMaster.ValueChanged -= spinnerVolMaster_ValueChanged;
            spinnerVolMaster.Value = trackVolMaster.Value / 1e6m;
            spinnerVolMaster.ValueChanged += spinnerVolMaster_ValueChanged;
            HasUnsavedChanges = true;
        }

        private void spinnerVolMaster_ValueChanged(object sender, EventArgs e)
        {
            trackVolMaster.Value = (int)(spinnerVolMaster.Value * 1e6m);
            HasUnsavedChanges = true;
        }

        private void trackVolMusic_Scroll(object sender, EventArgs e)
        {
            spinnerVolMusic.ValueChanged -= spinnerVolMusic_ValueChanged;
            spinnerVolMusic.Value = trackVolMusic.Value / 1e6m;
            spinnerVolMusic.ValueChanged += spinnerVolMusic_ValueChanged;
            HasUnsavedChanges = true;
        }

        private void spinnerVolMusic_ValueChanged(object sender, EventArgs e)
        {
            trackVolMusic.Value = (int)(spinnerVolMusic.Value * 1e6m);
            HasUnsavedChanges = true;
        }

        private void trackVolFX_Scroll(object sender, EventArgs e)
        {
            spinnerVolFX.ValueChanged -= spinnerVolFX_ValueChanged;
            spinnerVolFX.Value = trackVolFX.Value / 1e6m;
            spinnerVolFX.ValueChanged += spinnerVolFX_ValueChanged;
            HasUnsavedChanges = true;
        }

        private void spinnerVolFX_ValueChanged(object sender, EventArgs e)
        {
            trackVolFX.Value = (int)(spinnerVolFX.Value * 1e6m);
            HasUnsavedChanges = true;
        }

        private void trackVolVoices_Scroll(object sender, EventArgs e)
        {
            spinnerVolVoices.ValueChanged -= spinnerVolVoices_ValueChanged;
            spinnerVolVoices.Value = trackVolVoices.Value / 1e6m;
            spinnerVolVoices.ValueChanged += spinnerVolVoices_ValueChanged;
            HasUnsavedChanges = true;
        }

        private void spinnerVolVoices_ValueChanged(object sender, EventArgs e)
        {
            trackVolVoices.Value = (int)(spinnerVolVoices.Value * 1e6m);
            HasUnsavedChanges = true;
        }

        private void AnyValueChanged(object sender, EventArgs e)
        {
            HasUnsavedChanges = true;
        }

        private void launchButton_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges)
            {
                var btnSave = new TaskDialogButton("Save and start");
                var btnDiscard = new TaskDialogButton("Start without saving");
                var btnCancel = new TaskDialogButton("Don't start");
                var res = TaskDialog.ShowDialog(new TaskDialogPage()
                {
                    Buttons = new TaskDialogButtonCollection()
                {
                    btnSave,
                    btnDiscard,
                    btnCancel
                },
                    Icon = TaskDialogIcon.Warning,
                    Caption = "Psychonauts Fixer",
                    Text = "There are unsaved changes. Do you want to save them before you start the game?"
                });

                if (res == btnSave)
                {
                    WindowApply();
                }
                else if (res == btnCancel)
                {
                    return;
                }
            }

            var psi = new ProcessStartInfo()
            {
                FileName = "Psychonauts.exe",
                WorkingDirectory = installLocation,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void profileButton_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges)
            {
                var btnSave = new TaskDialogButton("Save and switch");
                var btnDiscard = new TaskDialogButton("Switch without saving");
                var btnCancel = new TaskDialogButton("Don't switch");
                var res = TaskDialog.ShowDialog(new TaskDialogPage()
                {
                    Buttons = new TaskDialogButtonCollection()
                {
                    btnSave,
                    btnDiscard,
                    btnCancel
                },
                    Icon = TaskDialogIcon.Warning,
                    Caption = "Psychonauts Fixer",
                    Text = "There are unsaved changes. Do you want to save them before you switch the profile?"
                });

                if (res == btnSave)
                {
                    WindowApply();
                }
                else if (res == btnCancel)
                {
                    return;
                }
            }

            PromptProfileSelect();
            HasUnsavedChanges = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "https://jonaskohl.de/software/psyfix/");
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = installLocation,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            TriggerHelp();
        }

        private void TriggerHelp()
        {
            SendMessage(this.Handle, WM_SYSCOMMAND, (IntPtr)SC_CONTEXTHELP, IntPtr.Zero);
        }
    }
}
