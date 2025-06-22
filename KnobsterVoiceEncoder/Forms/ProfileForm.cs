using KnobsterVoiceEncoder.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KnobsterVoiceEncoder
{
    public partial class ProfileForm : Form
    {
        BindingList<Command> commands = new BindingList<Command>();
        string filename = null;
        bool edited = false;
        bool editedCommand = false;
        string windowTitle = "KnobsterVoice Encoder - XMAS - ";

        ScanForm scan = new ScanForm();

        public ProfileForm()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            windowTitle += $"v.{ver.Major}.{ver.Minor}.{ver.Build}";

            InitializeComponent();

#if DEBUG
            linkDonation.Visible = false;
#else
            btnCopy.Visible = false;
            btnPaste.Visible = false;
            txtEncode.Visible = false;
            txtDecode.Visible = false;
            button1.Visible = false;
            btnDecode.Visible = false;
#endif
        }

        void ProfileForm_Load(object sender, EventArgs e)
        {
            PushKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); PushKey.ValueMember = "KeyValue"; PushKey.DisplayMember = "DisplayName";
            ReleaseKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); ReleaseKey.ValueMember = "KeyValue"; ReleaseKey.DisplayMember = "DisplayName";
            InnerCCWKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); InnerCCWKey.ValueMember = "KeyValue"; InnerCCWKey.DisplayMember = "DisplayName";
            InnerCWKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); InnerCWKey.ValueMember = "KeyValue"; InnerCWKey.DisplayMember = "DisplayName";
            OuterCCWKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); OuterCCWKey.ValueMember = "KeyValue"; OuterCCWKey.DisplayMember = "DisplayName";
            OuterCWKey.DataSource = new Keys().List.Where(it => it.ShowInCombo).ToList(); OuterCWKey.ValueMember = "KeyValue"; OuterCWKey.DisplayMember = "DisplayName";

            CommandsList.DataSource = commands;
            CommandsList.DisplayMember = "DisplayName";
            CommandsList.ValueMember = "Token";

            this.Text = windowTitle;
        }

        void ProfileForm_Close(object sender, FormClosingEventArgs e)
        {
            var msg = edited ? "Changes dont saved" : "";

            e.Cancel = edited && MessageBox.Show($"{msg}\n\nAre you sure?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No;
        }

        public void SetEdited(bool edited)
        {
            this.edited = edited;

            this.Text = $"{windowTitle} - {(string.IsNullOrEmpty(filename) ? "New file" : Path.GetFileName(filename))}{(edited ? "*" : "")}";
        }

        public void SetEditedCommand(bool editedCommand)
        {
            this.editedCommand = editedCommand;
            this.btnUpdateCommand.Enabled = editedCommand;
        }

        public string Encode()
        {
            string result = "";

            // Push / Relase
            result += Modificator.Get(PushModificatorControlLeft.Checked, PushModificatorAltLeft.Checked, PushModificatorShiftLeft.Checked);
            result += Modificator.Get(PushModificatorControlRight.Checked, PushModificatorAltRight.Checked, PushModificatorShiftRight.Checked);
            result += (char)((Key)PushKey.SelectedItem)?.KeyValue;

            result += Modificator.Get(ReleaseModificatorControlLeft.Checked, ReleaseModificatorAltLeft.Checked, ReleaseModificatorShiftLeft.Checked);
            result += Modificator.Get(ReleaseModificatorControlRight.Checked, ReleaseModificatorAltRight.Checked, ReleaseModificatorShiftRight.Checked);
            result += (char)((Key)ReleaseKey.SelectedItem)?.KeyValue;

            // Inner 
            result += Modificator.Get(InnerCCWModificatorControlLeft.Checked, InnerCCWModificatorAltLeft.Checked, InnerCCWModificatorShiftLeft.Checked);
            result += Modificator.Get(InnerCCWModificatorControlRight.Checked, InnerCCWModificatorAltRight.Checked, InnerCCWModificatorShiftRight.Checked);
            result += (char)((Key)InnerCCWKey.SelectedItem)?.KeyValue;

            result += Modificator.Get(InnerCWModificatorControlLeft.Checked, InnerCWModificatorAltLeft.Checked, InnerCWModificatorShiftLeft.Checked);
            result += Modificator.Get(InnerCWModificatorControlRight.Checked, InnerCWModificatorAltRight.Checked, InnerCWModificatorShiftRight.Checked);
            result += (char)((Key)InnerCWKey.SelectedItem)?.KeyValue;

            result += InnerSpeed.Value != 0 ? (char)InnerSpeed.Value : (char)InnerSpeed.Minimum;

            // Outer
            result += Modificator.Get(OuterCCWModificatorControlLeft.Checked, OuterCCWModificatorAltLeft.Checked, OuterCCWModificatorShiftLeft.Checked);
            result += Modificator.Get(OuterCCWModificatorControlRight.Checked, OuterCCWModificatorAltRight.Checked, OuterCCWModificatorShiftRight.Checked);
            result += (char)((Key)OuterCCWKey.SelectedItem)?.KeyValue;

            result += Modificator.Get(OuterCWModificatorControlLeft.Checked, OuterCWModificatorAltLeft.Checked, OuterCWModificatorShiftLeft.Checked);
            result += Modificator.Get(OuterCWModificatorControlRight.Checked, OuterCWModificatorAltRight.Checked, OuterCWModificatorShiftRight.Checked);
            result += (char)((Key)OuterCWKey.SelectedItem)?.KeyValue;

            result += OuterSpeed.Value != 0 ? (char)OuterSpeed.Value : (char)OuterSpeed.Minimum;

            var bytes = Encoding.Unicode.GetBytes(result);

            return Crockbase32.Encode(bytes);
        }

        void Decode(string Token)
        {
            byte[] decoded = null;
            
            try
            {
                decoded = Crockbase32.Decode(Token);
            }
            catch (Exception)
            {
                return;
            }

            if (decoded.Length != 40)
            {
                txtDecode.Text = "Invalid code";
                return;
            }

            // Push & Release
            PushModificatorControlLeft.Checked = Modificator.CheckControl(decoded[0]);
            PushModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[0]);
            PushModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[0]);
            PushModificatorControlRight.Checked = Modificator.CheckControl(decoded[2]);
            PushModificatorAltRight.Checked = Modificator.CheckAlt(decoded[2]);
            PushModificatorShiftRight.Checked = Modificator.CheckShift(decoded[2]);
            PushKey.SelectedIndex = PushKey.Items.Cast<Key>().ToList().IndexOf((PushKey.Items.Cast<Key>().First(it => it.KeyValue == BitConverter.ToInt16(decoded, 4))));

            ReleaseModificatorControlLeft.Checked = Modificator.CheckControl(decoded[6]);
            ReleaseModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[6]);
            ReleaseModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[6]);
            ReleaseModificatorControlRight.Checked = Modificator.CheckControl(decoded[8]);
            ReleaseModificatorAltRight.Checked = Modificator.CheckAlt(decoded[8]);
            ReleaseModificatorShiftRight.Checked = Modificator.CheckShift(decoded[8]);
            ReleaseKey.SelectedIndex = ReleaseKey.Items.Cast<Key>().ToList().IndexOf((ReleaseKey.Items.Cast<Key>().First(it => it.KeyValue == BitConverter.ToInt16(decoded, 10))));

            // Inner 
            InnerCCWModificatorControlLeft.Checked = Modificator.CheckControl(decoded[12]);
            InnerCCWModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[12]);
            InnerCCWModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[12]);
            InnerCCWModificatorControlRight.Checked = Modificator.CheckControl(decoded[14]);
            InnerCCWModificatorAltRight.Checked = Modificator.CheckAlt(decoded[14]);
            InnerCCWModificatorShiftRight.Checked = Modificator.CheckShift(decoded[14]);
            InnerCCWKey.SelectedIndex = InnerCCWKey.Items.Cast<Key>().ToList().IndexOf((InnerCCWKey.Items.Cast<Key>().First(it => it.KeyValue == BitConverter.ToInt16(decoded, 16))));

            InnerCWModificatorControlLeft.Checked = Modificator.CheckControl(decoded[18]);
            InnerCWModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[18]);
            InnerCWModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[18]);
            InnerCWModificatorControlRight.Checked = Modificator.CheckControl(decoded[20]);
            InnerCWModificatorAltRight.Checked = Modificator.CheckAlt(decoded[20]);
            InnerCWModificatorShiftRight.Checked = Modificator.CheckShift(decoded[20]);
            InnerCWKey.SelectedIndex = InnerCWKey.Items.Cast<Key>().ToList().IndexOf((InnerCWKey.Items.Cast<Key>().First(it => it.KeyValue == BitConverter.ToInt16(decoded, 22))));

            InnerSpeed.Value = BitConverter.ToInt16(decoded, 24);

            // Outer 
            OuterCCWModificatorControlLeft.Checked = Modificator.CheckControl(decoded[26]);
            OuterCCWModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[26]);
            OuterCCWModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[26]);
            OuterCCWModificatorControlRight.Checked = Modificator.CheckControl(decoded[28]);
            OuterCCWModificatorAltRight.Checked = Modificator.CheckAlt(decoded[28]);
            OuterCCWModificatorShiftRight.Checked = Modificator.CheckShift(decoded[28]);
            OuterCCWKey.SelectedIndex = OuterCCWKey.Items.Cast<Key>().ToList().IndexOf((OuterCCWKey.Items.Cast<Key>().First(it => it.KeyValue == decoded[30])));

            OuterCWModificatorControlLeft.Checked = Modificator.CheckControl(decoded[32]);
            OuterCWModificatorAltLeft.Checked = Modificator.CheckAlt(decoded[32]);
            OuterCWModificatorShiftLeft.Checked = Modificator.CheckShift(decoded[32]);
            OuterCWModificatorControlRight.Checked = Modificator.CheckControl(decoded[34]);
            OuterCWModificatorAltRight.Checked = Modificator.CheckAlt(decoded[34]);
            OuterCWModificatorShiftRight.Checked = Modificator.CheckShift(decoded[34]);
            OuterCWKey.SelectedIndex = OuterCWKey.Items.Cast<Key>().ToList().IndexOf((OuterCWKey.Items.Cast<Key>().First(it => it.KeyValue == decoded[36])));

            OuterSpeed.Value = BitConverter.ToInt16(decoded, 38);
        }

        public void btnEncode_Click(object sender, EventArgs e)
        {
            txtEncode.Text = Encode();
        }

        void btnDecode_Click(object sender, EventArgs e)
        {
            Decode(txtDecode.Text);
        }

        void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEncode.Text))
                    System.Windows.Forms.Clipboard.Clear();
                else
                    System.Windows.Forms.Clipboard.SetText(txtEncode.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't copy to clipboard. Don't press so quickly the Copy button or, if you are running KnobsterVoice, please close it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                txtDecode.Text = Clipboard.GetText(TextDataFormat.Text);
                Decode(txtDecode.Text);
            }
        }

        void txtDecode_TextChanged(object sender, EventArgs e)
        {
            txtDecode.ForeColor = Color.Black;
        }

        void CommandsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommandsList.SelectedItem == null)
                return;

            Decode(((Command)CommandsList.SelectedItem).Token);
            txtEncode.Text = Encode();
            txtCategory.Text = ((Command)CommandsList.SelectedItem).Category;
            txtPrefix.Text = ((Command)CommandsList.SelectedItem).Prefix;
            txtSay.Text = ((Command)CommandsList.SelectedItem).Say;
            txtThenSay.Text = ((Command)CommandsList.SelectedItem).ThenSay;
            trackVolume.Value = ((Command)CommandsList.SelectedItem).Volume;
            trackPitch.Value = ((Command)CommandsList.SelectedItem).Pitch;

            SetEditedCommand(false);
        }

        void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
			new AboutForm().ShowDialog();
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = edited ? "Changes dont saved\n\n" : "";
            msg += "Are you sure?";

            if (!edited || edited && MessageBox.Show(msg, "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SetEdited(false);
                Application.Exit();
            }
        }

        void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!commands.Any())
            {
                MessageBox.Show("Empty command list\n\nI can't save an empty profile", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (filename == null)
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                    return;
                }

                if (string.IsNullOrEmpty(txtName.Text) && MessageBox.Show("Empty profile name\n\nAre you sure?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    return;

                var xDoc = new Exporter().GetXML(txtName.Text, commands);

                xDoc.Save(filename);
                SetEdited(false);
            }
        }

        void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!commands.Any())
            {
                MessageBox.Show("I can't save an empty profile\n\nEmpty command list", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(txtName.Text) && MessageBox.Show("Empty profile name\n\nAre you sure?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    return;

                var saveDialog = new SaveFileDialog { Filter = "VoiceAttack profile files (*.vap)|*.vap|All files (*.*)|*.*", FileName = string.IsNullOrEmpty(txtName.Text) ? "New profile.vap" : $"{txtName.Text}.vap" };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var xDoc = new Exporter().GetXML(txtName.Text, commands);

                    xDoc.Save(saveDialog.FileName);
                    filename = saveDialog.FileName;
                    SetEdited(false);
                }
            }
        }

        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!edited || (edited && MessageBox.Show("I will discard changes\n\nAre you sure?", "Open", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK))
            {
                var dialog = new OpenFileDialog { Filter = "VoiceAttack profile files (*.vap)|*.vap|All files (*.*)|*.*" };

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var importer = new Importer(dialog.FileName);
                var importedCommands = importer.GetBindings();

                commands.Clear();
                foreach (var command in importedCommands) { commands.Add(command); }

                CommandsList.SelectedIndex = 0;
                CommandsList_SelectedIndexChanged(null, null);

                txtCategory.Items.Clear();
                foreach (var category in importedCommands.Where(it => !string.IsNullOrEmpty(it.Category)).Select(it => it.Category).Distinct()) { txtCategory.Items.Add(category); }

                txtPrefix.Items.Clear();
                foreach (var prefix in importedCommands.Where(it => !string.IsNullOrEmpty(it.Prefix)).Select(it => it.Prefix).Distinct()) { txtPrefix.Items.Add(prefix); }

                txtEncode.Text = Encode();
                txtName.Text = importer.ProfileName;
                filename = dialog.FileName;

                SetEdited(false);
            }
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            if (CommandsList.SelectedIndex != -1 && MessageBox.Show("I will remove this command\n\nAre you sure?", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                commands.RemoveAt(CommandsList.SelectedIndex);

                txtCategory.Items.Clear();
                txtCategory.Items.AddRange(CommandsList.Items.Cast<Command>().Select(it => it.Category).Where(it => !string.IsNullOrEmpty(it)).Distinct().ToArray()); ;

                txtPrefix.Items.Clear();
                txtPrefix.Items.AddRange(CommandsList.Items.Cast<Command>().Select(it => it.Prefix).Where(it => !string.IsNullOrEmpty(it)).ToArray());
                txtPrefix.Items.AddRange(CommandsList.Items.Cast<Command>().Select(it => it.Prefix).Where(it => !string.IsNullOrEmpty(it)).Distinct().ToArray());

                SetEdited(true);
            }
        }

        void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (CommandsList.Items.Count > 0 && MessageBox.Show("I will remove ALL commands\n\nAre you sure?", "Remove all", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                commands.ToList().ForEach(command => commands.Remove(command));
                txtCategory.Items.Clear();
                txtPrefix.Items.Clear();
                SetEdited(true);
            }
        }

        void btnNewCommand_Click(object sender, EventArgs e)
        {
            if (editedCommand)
            {
                if (MessageBox.Show("I will reset all keys, checkboxes and sliders\n\nAre you sure?", "New command", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
					txtCategory.Text = null;
					txtPrefix.Text = null;
					trackVolume.Value = trackVolume.Maximum;
					trackPitch.Value = 0;

					ResetButtons();
                    SetEditedCommand(false);
                }
            }
            else
            {
				txtCategory.Text = null;
				txtPrefix.Text = null;
				trackVolume.Value = trackVolume.Maximum;
				trackPitch.Value = 0;

				ResetButtons();
                SetEditedCommand(false);
            }
        }

        void UpdateCommand()
        {
            var token = Encode();

            commands[CommandsList.SelectedIndex] = new Command
            {
                Prefix = txtPrefix.Text,
                Category = txtCategory.Text,
                Say = txtSay.Text,
                Token = token,
                ThenSay = txtThenSay.Text,
                Volume = trackVolume.Value,
                Pitch = trackPitch.Value
            };

            txtCategory.Items.Clear();
            txtCategory.Items.AddRange(CommandsList.Items.Cast<Command>().Select(it => it.Category).Where(it => !string.IsNullOrEmpty(it)).Distinct().ToArray()); ;

            txtPrefix.Items.Clear();
            txtPrefix.Items.AddRange(CommandsList.Items.Cast<Command>().Select(it => it.Prefix).Where(it => !string.IsNullOrEmpty(it)).Distinct().ToArray());

            txtEncode.Text = token;

            SetEdited(true);
            SetEditedCommand(false);
        }

        void SaveCommand()
        {
            if (string.IsNullOrEmpty(txtSay.Text))
            {
                MessageBox.Show("'When I say' is empty", "Save command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var token = Encode();

                var newCommand = new Command
                {
                    Prefix = txtPrefix.Text,
                    Category = txtCategory.Text,
                    Say = txtSay.Text,
                    Token = token,
                    ThenSay = txtThenSay.Text,
                    Volume = trackVolume.Value,
                    Pitch = trackPitch.Value
                };

                if (commands.Select(it => it.DisplayName).Contains(newCommand.DisplayName))
                {
                    MessageBox.Show("Can't add existing command", "New command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    commands.Add(newCommand);

                    if (!txtCategory.Items.Cast<string>().Any(it => it == txtCategory.Text))
                        txtCategory.Items.Add(txtCategory.Text);

                    if (!txtPrefix.Items.Cast<string>().Any(it => it == txtPrefix.Text))
                        txtPrefix.Items.Add(txtPrefix.Text);

                    txtEncode.Text = token;
                    txtSay.Text = null;

                    ResetButtons();
                    SetEdited(true);
                    SetEditedCommand(false);
                }
            }
        }

        void btnUpdateCommand_Click(object sender, EventArgs e)
        {
            if (editedCommand && CommandsList.SelectedIndex != -1)
                UpdateCommand();
            else
                SaveCommand();
        }

        public void ResetButtons()
        {
            CommandsList.SelectedIndex = -1;

            // Push
            PushModificatorControlLeft.Checked = false;
            PushModificatorAltLeft.Checked = false;
            PushModificatorShiftLeft.Checked = false;
            PushKey.SelectedIndex = 0;

            // Release
            ReleaseModificatorControlLeft.Checked = false;
            ReleaseModificatorControlLeft.Checked = false;
            ReleaseModificatorControlLeft.Checked = false;
            ReleaseKey.SelectedIndex = 0;

            // Inner CCW
            InnerCCWModificatorControlLeft.Checked = false;
            InnerCCWModificatorAltLeft.Checked = false;
            InnerCCWModificatorShiftLeft.Checked = false;
            InnerCCWKey.SelectedIndex = 0;

            // Inner CW
            InnerCWModificatorControlLeft.Checked = false;
            InnerCWModificatorAltLeft.Checked = false;
            InnerCWModificatorShiftLeft.Checked = false;
            InnerCWKey.SelectedIndex = 0;

            InnerSpeed.Value = 1;

            // Outer CCW
            OuterCCWModificatorControlLeft.Checked = false;
            OuterCCWModificatorAltLeft.Checked = false;
            OuterCCWModificatorShiftLeft.Checked = false;
            OuterCCWKey.SelectedIndex = 0;

            // Outer CW
            OuterCWModificatorControlLeft.Checked = false;
            OuterCWModificatorAltLeft.Checked = false;
            OuterCWModificatorShiftLeft.Checked = false;
            OuterCWKey.SelectedIndex = 0;

            OuterSpeed.Value = 1;

            txtSay.Text = null;
            txtThenSay.Text = null;
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!edited || (edited && MessageBox.Show("I will close this profile to create a new one\n\nAre you sure?", "New profile", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK))
            {
                ResetButtons();
                filename = null;
                txtName.Text = null;
                txtSay.Text = null;
                txtPrefix.Text = null;
                txtPrefix.Items.Clear();
                txtCategory.Text = null;
                txtCategory.Items.Clear();
                txtThenSay.Text = null;
                trackVolume.Value = trackVolume.Maximum;
                trackPitch.Value = 0;
                commands.Clear();
                SetEdited(false);
            }
        }

        void Edited(object sender, EventArgs e)
        {
            SetEdited(true);
        }

        void EditedCommand(object sender, EventArgs e)
        {
            //if (CommandsList.SelectedIndex != -1)
            SetEditedCommand(true);
        }

        void txtSay_Enter(object sender, EventArgs e)
        {
            txtSay.SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scan.Start("Push button", this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            scan.Start("Release button", this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scan.Start("Inner Counter ClockWise", this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            scan.Start("Inner Clock Wise", this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            scan.Start("Outer Counter ClockWise", this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            scan.Start("Outer ClockWise", this);
        }

        private void linkDonation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/xmasmusicsoft");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open donations link");
            }
        }
    }
}
