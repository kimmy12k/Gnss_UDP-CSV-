using System;
using System.Collections.Generic;
using System.IO;

namespace UDPMode
{
    internal class IniParser
    {
        private readonly Dictionary<string, Dictionary<string, string>> _sections
            = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        private IniParser() { }

        public static IniParser Load(string filePath)
        {
            var parser = new IniParser();
            if (!File.Exists(filePath)) return parser;

            string currentSection = "";
            foreach (string rawLine in File.ReadAllLines(filePath))
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2).Trim();
                    if (!parser._sections.ContainsKey(currentSection))
                        parser._sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    continue;
                }

                int eqIndex = line.IndexOf('=');
                if (eqIndex < 0) continue;

                string key = line.Substring(0, eqIndex).Trim();
                string value = line.Substring(eqIndex + 1).Trim();

                if (!parser._sections.ContainsKey(currentSection))
                    parser._sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                parser._sections[currentSection][key] = value;
            }
            return parser;
        }

        public string Get(string section, string key, string defaultValue = "")
        {
            if (_sections.TryGetValue(section, out var dict))
                if (dict.TryGetValue(key, out string value))
                    return value;
            return defaultValue;
        }

        public void Set(string section, string key, string value)
        {
            if (!_sections.ContainsKey(section))
                _sections[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _sections[section][key] = value;
        }

        public void Save(string filePath)
        {
            var lines = new List<string>();
            foreach (var section in _sections)
            {
                lines.Add($"[{section.Key}]");
                foreach (var kvp in section.Value)
                    lines.Add($"{kvp.Key}={kvp.Value}");
                lines.Add("");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
