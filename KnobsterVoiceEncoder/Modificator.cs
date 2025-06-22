using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace KnobsterVoiceEncoder
{
    public enum ModificatorCode
    {
        None = 0,
        Control = 1,
        Alt = 2,
        Shift = 3,
        ControlAlt = 4,
        ControlAltShift = 5,
        ControlShift = 6,
        AltShift = 7
    }

    public enum ModificatorKey
    {
        None = 48,
        Control = 49,
        Alt = 50,
        Shift = 51,
        ControlAlt = 52,
        ControlAltShift = 53,
        ControlShift = 54,
        AltShift = 55
    }


    public static class Modificator
    {
        public static byte Get(bool Control, bool Alt, bool Shift)
        {
            var result = ModificatorCode.None;

            if (!Control && Alt && Shift)
                return (byte)result;

            if (Control)
            {
                result = ModificatorCode.Control;

                if (Alt)
                {
                    result = ModificatorCode.ControlAlt;

                    if (Shift)
                        result = ModificatorCode.ControlAltShift;
                }
                else if (Shift)
                {
                    result = ModificatorCode.ControlShift;

                    if (Alt)
                        result = ModificatorCode.ControlAltShift;
                }
            }
            else if (Alt)
            {
                result = ModificatorCode.Alt;

                if (Control)
                {
                    result = ModificatorCode.ControlAlt;

                    if (Shift)
                        result = ModificatorCode.ControlAltShift;
                }
                else if (Shift)
                {
                    result = ModificatorCode.AltShift;

                    if (Control)
                        result = ModificatorCode.ControlAltShift;
                }
            }
            else if (Shift)
            {
                result = ModificatorCode.Shift;

                if (Control)
                {
                    result = ModificatorCode.ControlShift;

                    if (Alt)
                        result = ModificatorCode.ControlAltShift;
                }
                else if (Alt)
                {
                    result = ModificatorCode.AltShift;

                    if (Control)
                        result = ModificatorCode.ControlAltShift;
                }
            }


            return (byte)result;
        }

        public static bool CheckControl(byte Modificator)
        {
            return new byte[] { (int)ModificatorKey.Control, (int)ModificatorKey.ControlAlt, (int)ModificatorKey.ControlAltShift, (int)ModificatorKey.ControlShift }.Contains(Modificator);
        }

        public static bool CheckAlt(byte Modificator)
        {
            return new byte[] { (int)ModificatorKey.Alt, (int)ModificatorKey.ControlAlt, (int)ModificatorKey.ControlAltShift, (int)ModificatorKey.AltShift }.Contains(Modificator);
        }

        public static bool CheckShift(byte Modificator)
        {
            return new byte[] { (int)ModificatorKey.Shift, (int)ModificatorKey.ControlAltShift, (int)ModificatorKey.ControlShift, (int)ModificatorKey.AltShift }.Contains(Modificator);
        }
    }
}
