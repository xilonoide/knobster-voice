using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KnobsterVoiceEncoder.XML
{
    public class Importer
    {
        private string filename;
        private string profilename = null;

        public string ProfileName
        {
            get
            {
                if (profilename != null)
                {
                    return profilename;
                }
                else
                {
                    var xDoc = XDocument.Load(filename);
                    return xDoc.Element("Profile").Element("Name").Value;
                }
            }
        }
        public Importer(string Filename)
        {
            filename = Filename;
        }

        public BindingList<Command> GetBindings()
        {
            var xDoc = XDocument.Load(filename);

            var commands = xDoc.Element("Profile").Element("Commands").Elements("Command").Where(it => !it.Elements("CommandType").Where(ct => ct.Value == "1").Any())
              .Select(it =>
              {
                  var item = new Command
                  {
                      Category = it.Element("Category")?.Value,
                      Prefix = it.Element("CompositeGroup")?.Value,
                      Say = it.Element("CommandString")?.Value,
                      Token = it.Element("ActionSequence")?.Element("CommandAction")?.Element("Context")?.Value
                  };

                  var elementSay = it.Element("ActionSequence")?.Elements("CommandAction")?.Elements("ActionType").FirstOrDefault(a => a.Value == "Say")?.Parent;

                  if (elementSay != null)
                  {
                      item.ThenSay = elementSay.Element("Context")?.Value;
                      item.Volume = int.Parse(elementSay.Element("X").Value);
                      item.Pitch = int.Parse(elementSay.Element("Y").Value);
                  }

                  return item;
              }).ToList();

            return new BindingList<Command>(commands);
        }
    }
}
