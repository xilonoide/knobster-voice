using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KnobsterVoiceEncoder.XML
{
    public class Exporter
    {
        public Exporter()
        {
        }

        public XDocument GetXML(string ProfileName, BindingList<Command> Commands)
        {
            var xDoc = new XDocument();

            // header
            var xProfile = HeaderXML(ProfileName);

            // commands
            var xCommands = new XElement("Commands");

            foreach (var command in Commands.Where(it => it.Fullcommand))
            {
                xCommands.Add(SayToXML(command));
            }

            foreach (var group in Commands.Where(it => !it.Fullcommand).GroupBy(it => it.Prefix))
            {
                xCommands.Add(PrefixToXML(group.Key, group.First().Category));

                foreach (var command in group)
                {
                    xCommands.Add(SayToXML(command));
                }
            }

            xProfile.Add(xCommands);

            xDoc.Add(xProfile);

            return xDoc;
        }

        XElement HeaderXML(string ProfileName)
        {
            var xProfile = new XElement("Profile", new XAttribute(XNamespace.Xmlns + "xsi", XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")), new XAttribute(XNamespace.Xmlns + "xsd", XNamespace.Get("http://www.w3.org/2001/XMLSchema")));
            xProfile.Add(new XElement("HasMB", "false"));
            xProfile.Add(new XElement("Id", Guid.NewGuid().ToString()));
            xProfile.Add(new XElement("Name", ProfileName));

            return xProfile;
        }

        XElement PrefixToXML(string Prefix, string Category)
        {
            var xCommand = new XElement("Command");

            xCommand.Add(new XElement("Referrer", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
            xCommand.Add(new XElement("ExecType", 0));
            xCommand.Add(new XElement("Confidence", 0));
            xCommand.Add(new XElement("PrefixActionCount", 0));
            xCommand.Add(new XElement("IsDynamicallyCreated", false));
            xCommand.Add(new XElement("TargetProcessSet", false));
            xCommand.Add(new XElement("TargetProcessType", 0));
            xCommand.Add(new XElement("TargetProcessLevel", 0));
            xCommand.Add(new XElement("CompareType", 0));
            xCommand.Add(new XElement("ExecFromWildcard", false));
            xCommand.Add(new XElement("IsSubCommand", false));
            xCommand.Add(new XElement("IsOverride", false));
            xCommand.Add(new XElement("BaseId", Guid.NewGuid().ToString()));
            xCommand.Add(new XElement("OriginId", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("SessionEnabled", true));
            xCommand.Add(new XElement("DoubleTapInvoked", false));
            xCommand.Add(new XElement("SingleTapDelayedInvoked", false));
            xCommand.Add(new XElement("LongTapInvoked", false));
            xCommand.Add(new XElement("ShortTapDelayedInvoked", false));
            xCommand.Add(new XElement("Id", Guid.NewGuid().ToString()));
            xCommand.Add(new XElement("CommandString", Prefix));
            xCommand.Add(new XElement("Async", false));
            xCommand.Add(new XElement("Enabled", true));

            if (!string.IsNullOrEmpty(Category))
                xCommand.Add(new XElement("Category", Category));

            xCommand.Add(new XElement("UseShortcut", false));
            xCommand.Add(new XElement("keyValue", 0));
            xCommand.Add(new XElement("keyShift", 0));
            xCommand.Add(new XElement("keyAlt", 0));
            xCommand.Add(new XElement("keyCtrl", 0));
            xCommand.Add(new XElement("keyWin", 0));
            xCommand.Add(new XElement("keyPassthru", true));
            xCommand.Add(new XElement("UseSpokenPhrase", true));
            xCommand.Add(new XElement("onlyKeyUp", false));
            xCommand.Add(new XElement("RepeatNumber", 2));
            xCommand.Add(new XElement("RepeatType", 0));
            xCommand.Add(new XElement("CommandType", 1));
            xCommand.Add(new XElement($"CompositeGroup", Prefix));
            xCommand.Add(new XElement("SourceProfile", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("UseConfidence", false));
            xCommand.Add(new XElement("minimumConfidenceLevel", 0));
            xCommand.Add(new XElement("UseJoystick", false));
            xCommand.Add(new XElement("joystickNumber", 0));
            xCommand.Add(new XElement("joystickButton", 0));
            xCommand.Add(new XElement("joystickNumber2", 0));
            xCommand.Add(new XElement("joystickButton2", 0));
            xCommand.Add(new XElement("joystickUp", false));
            xCommand.Add(new XElement("KeepRepeating", false));
            xCommand.Add(new XElement("UseProcessOverride", false));
            xCommand.Add(new XElement("ProcessOverrideActiveWindow", false));
            xCommand.Add(new XElement("LostFocusStop", false));
            xCommand.Add(new XElement("PauseLostFocus", false));
            xCommand.Add(new XElement("LostFocusBackCompat", false));
            xCommand.Add(new XElement("UseMouse", false));
            xCommand.Add(new XElement("Mouse1", false));
            xCommand.Add(new XElement("Mouse2", false));
            xCommand.Add(new XElement("Mouse3", false));
            xCommand.Add(new XElement("Mouse4", false));
            xCommand.Add(new XElement("Mouse5", false));
            xCommand.Add(new XElement("Mouse6", false));
            xCommand.Add(new XElement("Mouse7", false));
            xCommand.Add(new XElement("Mouse8", false));
            xCommand.Add(new XElement("Mouse9", false));
            xCommand.Add(new XElement("MouseUpOnly", false));
            xCommand.Add(new XElement("MousePassThru", true));
            xCommand.Add(new XElement("joystickExclusive", false));
            xCommand.Add(new XElement("lastEditedAction", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("UseProfileProcessOverride", false));
            xCommand.Add(new XElement("ProfileProcessOverrideActiveWindow", false));
            xCommand.Add(new XElement("RepeatIfKeysDown", false));
            xCommand.Add(new XElement("RepeatIfMouseDown", false));
            xCommand.Add(new XElement("RepeatIfJoystickDown", false));
            xCommand.Add(new XElement("AH", 0));
            xCommand.Add(new XElement("CL", 0));
            xCommand.Add(new XElement("HasMB", false));
            xCommand.Add(new XElement("UseVariableHotkey", false));
            xCommand.Add(new XElement("CLE", 0));
            xCommand.Add(new XElement("EX1", false));
            xCommand.Add(new XElement("EX2", false));
            xCommand.Add(new XElement("InternalId", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
            xCommand.Add(new XElement("HasInput", false));
            xCommand.Add(new XElement("HotkeyDoubleTapLevel", 0));
            xCommand.Add(new XElement("MouseDoubleTapLevel", 0));
            xCommand.Add(new XElement("JoystickDoubleTapLevel", 0));
            xCommand.Add(new XElement("HotkeyLongTapLevel", 0));
            xCommand.Add(new XElement("MouseLongTapLevel", 0));
            xCommand.Add(new XElement("JoystickLongTapLevel", 0));
            xCommand.Add(new XElement("AlwaysExec", false));

            return xCommand;
        }

        XElement SayToXML(Command Command)
        {
            var xCommand = new XElement("Command");

            xCommand.Add(new XElement("Referrer", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
            xCommand.Add(new XElement("ExecType", 0));
            xCommand.Add(new XElement("Confidence", 0));
            xCommand.Add(new XElement("PrefixActionCount", 0));
            xCommand.Add(new XElement("IsDynamicallyCreated", false));
            xCommand.Add(new XElement("TargetProcessSet", false));
            xCommand.Add(new XElement("TargetProcessType", 0));
            xCommand.Add(new XElement("TargetProcessLevel", 0));
            xCommand.Add(new XElement("CompareType", 0));
            xCommand.Add(new XElement("ExecFromWildcard", false));
            xCommand.Add(new XElement("IsSubCommand", false));
            xCommand.Add(new XElement("IsOverride", false));
            xCommand.Add(new XElement("BaseId", Guid.NewGuid().ToString()));
            xCommand.Add(new XElement("OriginId", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("SessionEnabled", true));
            xCommand.Add(new XElement("DoubleTapInvoked", false));
            xCommand.Add(new XElement("SingleTapDelayedInvoked", false));
            xCommand.Add(new XElement("LongTapInvoked", false));
            xCommand.Add(new XElement("ShortTapDelayedInvoked", false));
            xCommand.Add(new XElement("Id", Guid.NewGuid().ToString()));
            xCommand.Add(new XElement("CommandString", Command.Say));

            var xActionSequence = new XElement("ActionSequence");
            var xCommandAction = new XElement("CommandAction");
            xCommandAction.Add(new XElement("_caption", $"Set Windows clipboard to '{Command.Token}'"));
            xCommandAction.Add(new XElement("PairingSet", false));
            xCommandAction.Add(new XElement("PairingSetElse", false));
            xCommandAction.Add(new XElement("Ordinal", 0));
            xCommandAction.Add(new XElement("ConditionMet", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
            xCommandAction.Add(new XElement("IndentLevel", 0));
            xCommandAction.Add(new XElement("ConditionSkip", false));
            xCommandAction.Add(new XElement("IsSuffixAction", false));
            xCommandAction.Add(new XElement("DecimalTransient1", 0));
            xCommandAction.Add(new XElement("Caption", $"Set Windows clipboard to '{Command.Token}'"));
            xCommandAction.Add(new XElement("Id", Guid.NewGuid().ToString()));
            xCommandAction.Add(new XElement("ActionType", "SetClipboard"));
            xCommandAction.Add(new XElement("Duration", 0));
            xCommandAction.Add(new XElement("Delay", 0));
            xCommandAction.Add(new XElement("KeyCodes", null));
            xCommandAction.Add(new XElement("Context", Command.Token));
            xCommandAction.Add(new XElement("X", 0));
            xCommandAction.Add(new XElement("Y", 0));
            xCommandAction.Add(new XElement("Z", 0));
            xCommandAction.Add(new XElement("InputMode", 0));
            xCommandAction.Add(new XElement("ConditionPairing", 0));
            xCommandAction.Add(new XElement("ConditionGroup", 0));
            xCommandAction.Add(new XElement("ConditionStartOperator", 0));
            xCommandAction.Add(new XElement("ConditionStartValue", 0));
            xCommandAction.Add(new XElement("ConditionStartValueType", 0));
            xCommandAction.Add(new XElement("ConditionStartType", 0));
            xCommandAction.Add(new XElement("DecimalContext1", 0));
            xCommandAction.Add(new XElement("DecimalContext2", 0));
            xCommandAction.Add(new XElement("DateContext1", "0001-01-01T00:00:00"));
            xCommandAction.Add(new XElement("DateContext2", "0001-01-01T00:00:00"));
            xCommandAction.Add(new XElement("Disabled", false));
            xCommandAction.Add(new XElement("RandomSounds", null));
            xActionSequence.Add(xCommandAction);

            if (!string.IsNullOrEmpty(Command.ThenSay))
            {
                xCommandAction = new XElement("CommandAction");
                xCommandAction.Add(new XElement("_caption", Command.ThenSay));
                xCommandAction.Add(new XElement("PairingSet", false));
                xCommandAction.Add(new XElement("PairingSetElse", false));
                xCommandAction.Add(new XElement("Ordinal", 1));
                xCommandAction.Add(new XElement("ConditionMet", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
                xCommandAction.Add(new XElement("IndentLevel", 0));
                xCommandAction.Add(new XElement("ConditionSkip", false));
                xCommandAction.Add(new XElement("IsSuffixAction", false));
                xCommandAction.Add(new XElement("DecimalTransient1", 0));
                xCommandAction.Add(new XElement("Caption", $"Say, '{Command.ThenSay}'"));
                xCommandAction.Add(new XElement("Id", Guid.NewGuid().ToString()));
                xCommandAction.Add(new XElement("ActionType", "Say"));
                xCommandAction.Add(new XElement("Duration", 0));
                xCommandAction.Add(new XElement("Delay", 0));
                xCommandAction.Add(new XElement("KeyCodes", null));
                xCommandAction.Add(new XElement("Context", Command.ThenSay));
                xCommandAction.Add(new XElement("Context3", "00000000-0000-0000-0000-000000000000"));
                xCommandAction.Add(new XElement("Context4", "Default"));
                xCommandAction.Add(new XElement("X", Command.Volume));
                xCommandAction.Add(new XElement("Y", Command.Pitch));
                xCommandAction.Add(new XElement("Z", 0));
                xCommandAction.Add(new XElement("InputMode", 0));
                xCommandAction.Add(new XElement("ConditionPairing", 0));
                xCommandAction.Add(new XElement("ConditionGroup", 0));
                xCommandAction.Add(new XElement("ConditionStartOperator", 0));
                xCommandAction.Add(new XElement("ConditionStartValue", 0));
                xCommandAction.Add(new XElement("ConditionStartValueType", 0));
                xCommandAction.Add(new XElement("ConditionStartType", 0));
                xCommandAction.Add(new XElement("DecimalContext1", 0));
                xCommandAction.Add(new XElement("DecimalContext2", 0));
                xCommandAction.Add(new XElement("DateContext1", "0001-01-01T00:00:00"));
                xCommandAction.Add(new XElement("DateContext2", "0001-01-01T00:00:00"));
                xCommandAction.Add(new XElement("Disabled", false));
                xCommandAction.Add(new XElement("RandomSounds", null));
                xActionSequence.Add(xCommandAction);
            }

            xCommand.Add(xActionSequence);

            xCommand.Add(new XElement("Async", false));
            xCommand.Add(new XElement("Enabled", true));
            
            if (!string.IsNullOrEmpty(Command.Category))
                xCommand.Add(new XElement("Category", Command.Category));

            xCommand.Add(new XElement("UseShortcut", false));
            xCommand.Add(new XElement("keyValue", 0));
            xCommand.Add(new XElement("keyShift", 0));
            xCommand.Add(new XElement("keyAlt", 0));
            xCommand.Add(new XElement("keyCtrl", 0));
            xCommand.Add(new XElement("keyWin", 0));
            xCommand.Add(new XElement("keyPassthru", true));
            xCommand.Add(new XElement("UseSpokenPhrase", true));
            xCommand.Add(new XElement("onlyKeyUp", false));
            xCommand.Add(new XElement("RepeatNumber", 2));
            xCommand.Add(new XElement("RepeatType", 0));

            if (Command.Fullcommand)
            {
                xCommand.Add(new XElement("CommandType", 0));
            }
            else
            {
                xCommand.Add(new XElement("CommandType", 2));
                xCommand.Add(new XElement($"CompositeGroup", Command.Prefix));
            }

            xCommand.Add(new XElement("SourceProfile", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("UseConfidence", false));
            xCommand.Add(new XElement("minimumConfidenceLevel", 0));
            xCommand.Add(new XElement("UseJoystick", false));
            xCommand.Add(new XElement("joystickNumber", 0));
            xCommand.Add(new XElement("joystickButton", 0));
            xCommand.Add(new XElement("joystickNumber2", 0));
            xCommand.Add(new XElement("joystickButton2", 0));
            xCommand.Add(new XElement("joystickUp", false));
            xCommand.Add(new XElement("KeepRepeating", false));
            xCommand.Add(new XElement("UseProcessOverride", false));
            xCommand.Add(new XElement("ProcessOverrideActiveWindow", false));
            xCommand.Add(new XElement("LostFocusStop", false));
            xCommand.Add(new XElement("PauseLostFocus", false));
            xCommand.Add(new XElement("LostFocusBackCompat", false));
            xCommand.Add(new XElement("UseMouse", false));
            xCommand.Add(new XElement("Mouse1", false));
            xCommand.Add(new XElement("Mouse2", false));
            xCommand.Add(new XElement("Mouse3", false));
            xCommand.Add(new XElement("Mouse4", false));
            xCommand.Add(new XElement("Mouse5", false));
            xCommand.Add(new XElement("Mouse6", false));
            xCommand.Add(new XElement("Mouse7", false));
            xCommand.Add(new XElement("Mouse8", false));
            xCommand.Add(new XElement("Mouse9", false));
            xCommand.Add(new XElement("MouseUpOnly", false));
            xCommand.Add(new XElement("MousePassThru", true));
            xCommand.Add(new XElement("joystickExclusive", false));
            xCommand.Add(new XElement("lastEditedAction", "00000000-0000-0000-0000-000000000000"));
            xCommand.Add(new XElement("UseProfileProcessOverride", false));
            xCommand.Add(new XElement("ProfileProcessOverrideActiveWindow", false));
            xCommand.Add(new XElement("RepeatIfKeysDown", false));
            xCommand.Add(new XElement("RepeatIfMouseDown", false));
            xCommand.Add(new XElement("RepeatIfJoystickDown", false));
            xCommand.Add(new XElement("AH", 0));
            xCommand.Add(new XElement("CL", 0));
            xCommand.Add(new XElement("HasMB", false));
            xCommand.Add(new XElement("UseVariableHotkey", false));
            xCommand.Add(new XElement("CLE", 0));
            xCommand.Add(new XElement("EX1", false));
            xCommand.Add(new XElement("EX2", false));
            xCommand.Add(new XElement("InternalId", new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "nil", true)));
            xCommand.Add(new XElement("HasInput", false));
            xCommand.Add(new XElement("HotkeyDoubleTapLevel", 0));
            xCommand.Add(new XElement("MouseDoubleTapLevel", 0));
            xCommand.Add(new XElement("JoystickDoubleTapLevel", 0));
            xCommand.Add(new XElement("HotkeyLongTapLevel", 0));
            xCommand.Add(new XElement("MouseLongTapLevel", 0));
            xCommand.Add(new XElement("JoystickLongTapLevel", 0));
            xCommand.Add(new XElement("AlwaysExec", false));

            return xCommand;
        }
    }
}
