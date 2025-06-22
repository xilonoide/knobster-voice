using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KnobsterVoiceEncoder
{
    public partial class ScanForm : Form
    {
        //[DllImport("user32.dll")]
        //static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        //[DllImport("user32.dll")]
        //static extern uint MapVirtualKey(uint uCode, uint uMapType);
        //var scanCode = MapVirtualKey((uint)e.KeyCode, 0);

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const byte VK_NUMLOCK = 0x90;
        private const uint KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_KEYDOWN = 0x0;

        bool isLControlPressed = false;
        string ID_Control;
        ProfileForm Profile;

        Keys k = new Keys();
        List<Key> pressedKeys = new List<Key>();
        List<int> modificatorKeyValues = new List<int> {
            (int)System.Windows.Forms.Keys.LControlKey,
            (int)System.Windows.Forms.Keys.LMenu,
            (int)System.Windows.Forms.Keys.LShiftKey,
            (int)System.Windows.Forms.Keys.RControlKey,
            (int)System.Windows.Forms.Keys.RMenu,
            (int)System.Windows.Forms.Keys.RShiftKey
        };

        public ScanForm()
        {
            InitializeComponent();
        }

        public void Start(string ID_Control, ProfileForm Profile)
        {
            this.ID_Control = ID_Control;
            this.Profile = Profile;

            this.Text = $"Scanning keys for {ID_Control}";

            Clear();

            if (!Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock))
            {
                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            }

            this.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void SetControl()
        {
            var normalKeyPressedValue = pressedKeys.Where(it => !modificatorKeyValues.Contains(it.KeyValue)).FirstOrDefault()?.KeyValue ?? k.List.First().KeyValue;

            switch (ID_Control)
            {

                case "Push button":
                    Profile.PushModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.PushModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.PushModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.PushModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.PushModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.PushModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.PushKey.SelectedIndex = Profile.PushKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
                case "Release button":
                    Profile.ReleaseModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.ReleaseModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.ReleaseModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.ReleaseModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.ReleaseModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.ReleaseModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.ReleaseKey.SelectedIndex = Profile.ReleaseKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
                case "Inner Counter ClockWise":
                    Profile.InnerCCWModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.InnerCCWModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.InnerCCWModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.InnerCCWModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.InnerCCWModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.InnerCCWModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.InnerCCWKey.SelectedIndex = Profile.InnerCCWKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
                case "Inner Clock Wise":
                    Profile.InnerCWModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.InnerCWModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.InnerCWModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.InnerCWModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.InnerCWModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.InnerCWModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.InnerCWKey.SelectedIndex = Profile.InnerCWKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
                case "Outer Counter ClockWise":
                    Profile.OuterCCWModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.OuterCCWModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.OuterCCWModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.OuterCCWModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.OuterCCWModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.OuterCCWModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.OuterCCWKey.SelectedIndex = Profile.OuterCCWKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
                case "Outer ClockWise":
                    Profile.OuterCWModificatorControlLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey);
                    Profile.OuterCWModificatorAltLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu);
                    Profile.OuterCWModificatorShiftLeft.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey);
                    Profile.OuterCWModificatorControlRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey);
                    Profile.OuterCWModificatorAltRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu);
                    Profile.OuterCWModificatorShiftRight.Checked = pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey);
                    Profile.OuterCWKey.SelectedIndex = Profile.OuterCWKey.FindStringExact(k.List.First(it => it.KeyValue == normalKeyPressedValue).DisplayName);
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetControl();
            Profile.btnEncode_Click(null, null);
            Profile.SetEdited(true);
            this.Close();
        }

        private void Clear()
        {
            pressedKeys.Clear();
            writeKeys();
            lblLeftControl.Font = new Font("Arial", 12, FontStyle.Regular);
            lblLeftAlt.Font = new Font("Arial", 12, FontStyle.Regular);
            lblLeftShift.Font = new Font("Arial", 12, FontStyle.Regular);
            lblRightControl.Font = new Font("Arial", 12, FontStyle.Regular);
            lblRightAlt.Font = new Font("Arial", 12, FontStyle.Regular);
            lblRightShift.Font = new Font("Arial", 12, FontStyle.Regular);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void writeKeys()
        {
            txtKeys.Text = "";

            foreach (var key in pressedKeys.Select(it => it).ToList())
            {
                if (modificatorKeyValues.Contains(key.KeyValue))
                {
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.LControlKey)
                        lblLeftControl.Font = new Font("Arial", 12, FontStyle.Bold);
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.LMenu)
                        lblLeftAlt.Font = new Font("Arial", 12, FontStyle.Bold);
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.LShiftKey)
                        lblLeftShift.Font = new Font("Arial", 12, FontStyle.Bold);
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.RControlKey)
                        lblRightControl.Font = new Font("Arial", 12, FontStyle.Bold);
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.RMenu)
                        lblRightAlt.Font = new Font("Arial", 12, FontStyle.Bold);
                    if (key.KeyValue == (int)System.Windows.Forms.Keys.RShiftKey)
                        lblRightShift.Font = new Font("Arial", 12, FontStyle.Bold);
                }
                else
                {
                    txtKeys.Text = key.DisplayName;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, System.Windows.Forms.Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            const int VK_SHIFT = 0x10;
            const int VK_CONTROL = 0x11;
            const int VK_MENU = 0x12;

            if ((msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN) && ((int)msg.WParam == VK_CONTROL || (int)msg.WParam == VK_SHIFT || (int)msg.WParam == VK_MENU))
            {
                KeyEventArgs e = new KeyEventArgs(System.Windows.Forms.Keys.None);

                switch ((OemScanCode)(((int)msg.LParam >> 16) & 0x1FF))
                {
                    case OemScanCode.LControl:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.LControlKey);
                        break;
                    case OemScanCode.RControl:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.RControlKey);
                        break;
                    case OemScanCode.LShift:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.LShiftKey);
                        break;
                    case OemScanCode.RShift:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.RShiftKey);
                        break;
                    case OemScanCode.LAlternate:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.LMenu);
                        break;
                    case OemScanCode.RAlternate:
                        e = new KeyEventArgs(System.Windows.Forms.Keys.RMenu);
                        break;
                    default:
                        if ((int)msg.WParam == VK_SHIFT)
                            e = new KeyEventArgs(System.Windows.Forms.Keys.ShiftKey);
                        else if ((int)msg.WParam == VK_CONTROL)
                            e = new KeyEventArgs(System.Windows.Forms.Keys.ControlKey);
                        break;
                }

                if (e.KeyData != System.Windows.Forms.Keys.None)
                {
                    if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
                        OnKeyDown(e);
                    else
                        OnKeyUp(e);

                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ScanForm_KeyDown(object sender, KeyEventArgs e)
        {
            var pressedKey = k.List.FirstOrDefault(it => it.KeyValue == (int)e.KeyCode);

            if (pressedKey.KeyValue == (int)System.Windows.Forms.Keys.RMenu && pressedKeys.LastOrDefault()?.KeyValue == (int)System.Windows.Forms.Keys.LControlKey)
            {
                pressedKeys.RemoveAt(pressedKeys.Count - 1);
                lblLeftControl.Font = new Font("Arial", 12, FontStyle.Regular);
            }

            int modificators = 0;

            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LControlKey))
                modificators++;
            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LMenu))
                modificators++;
            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.LShiftKey))
                modificators++;
            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RControlKey))
                modificators++;
            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RMenu))
                modificators++;
            if (pressedKeys.Select(it => it.KeyValue).Contains((int)System.Windows.Forms.Keys.RShiftKey))
                modificators++;

            if (pressedKey != null && pressedKeys.Count <= 6 && modificators <= 6 && !pressedKeys.Select(it => it.KeyValue).Contains((int)e.KeyValue))
            {
                if (!modificatorKeyValues.Contains((int)e.KeyValue))
                {
                    if (k.List.Where(it => it.ShowInCombo).Select(it => it.KeyValue).Contains(e.KeyValue))
                    {
                        var oldKeyValue = pressedKeys.FirstOrDefault(it => !modificatorKeyValues.Contains(it.KeyValue))?.KeyValue;

                        if (oldKeyValue != null)
                            pressedKeys.Remove(pressedKeys.First(it => it.KeyValue == oldKeyValue));

                        pressedKeys.Add(pressedKey);
                        writeKeys();
                    }
                }
                else
                {
                    pressedKeys.Add(pressedKey);
                    writeKeys();
                }
            }
        }
    }
}


