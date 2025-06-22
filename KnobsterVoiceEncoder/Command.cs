using System;
using System.Net.NetworkInformation;

namespace KnobsterVoiceEncoder
{
    public class Command
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public string Say { get; set; }
        public string Category { get; set; }
        public string ThenSay { get; set; }
        public int Volume { get; set; }
        public int Pitch { get; set; }

        public bool Fullcommand
        {
            get { return string.IsNullOrEmpty(this.Prefix); }
        }

        public string DisplayName
        {
            get
            {
                var result = string.IsNullOrEmpty(Category) ? "" : $"{Category} - " ;

                if (!Fullcommand && !string.IsNullOrEmpty(Prefix))
                    result += $"{Prefix} - ";

                result += Say;

                if (!string.IsNullOrEmpty(ThenSay))
                    result += $" [{ThenSay}]";

                return result;
            }
        }
    }
}
