using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychonautsFixer
{
    public class IniFile
    {
        private List<IniSection> sections;

        public string[] SectionNames => sections.Select(s => s.Name).ToArray();

        public IniFile()
        {
            sections = new List<IniSection>();
        }

        public static IniFile Parse(string contents)
        {
            var ini = new IniFile();

            var lines = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(l => !l.StartsWith(";") && !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim());

            string currentSectionName = null; // Null indicates global section
            foreach (var ln in lines)
            {
                if (ln.StartsWith("[") && ln.EndsWith("]"))
                {
                    currentSectionName = ln.Substring(1, ln.Length - 2);
                }
                else
                {
                    (string key, string value, IList<string> _) = ln.Split(new char[] { '=' }, 2);
                    ini.Set(currentSectionName, key, value);
                }
            }

            return ini;
        }

        public static IniFile Load(string fileName)
        {
            return Parse(File.ReadAllText(fileName));
        }

        public string Get(string key)
        {
            return Get(null, key);
        }

        public string Get(string section, string key)
        {
            return sections.Where(s => s.Name == section).FirstOrDefault()?.Get(key);
        }

        public void Set(string section, string key, string value)
        {
            if (!sections.Any(s => s.Name == section))
                sections.Add(new IniSection() { Name = section });

            sections.Where(s => s.Name == section).FirstOrDefault()?.Set(key, value);
        }

        public IniSection GetSection(string section)
        {
            return sections.FirstOrDefault(s => s.Name == section);
        }

        public bool ContainsSection(string section)
        {
            return sections.Any(s => s.Name == section);
        }

        public void Remove(string section, string key)
        {
            sections.Where(s => s.Name == section).FirstOrDefault()?.Remove(key);
        }

        public void RemoveSection(string section)
        {
            sections.RemoveAll(s => s.Name == section);
        }

        public override string ToString()
        {
            var globalSection = sections.FirstOrDefault(s => s.Name == null);
            var namedSections = sections.Where(s => s.Name != null);

            var sb = new StringBuilder();
            if (globalSection != null)
                sb.AppendLine(globalSection.ToString());

            foreach (var section in namedSections)
            {
                sb.AppendLine($"[{section.Name}]");
                sb.AppendLine(section.ToString());
            }

            return sb.ToString();
        }
    }

    public class IniSection
    {
        private Dictionary<string, string> entries;
        public string Name { get; set; }

        public IniSection()
        {
            entries = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries.Select(e => $"{e.Key}={e.Value}"));
        }

        public void Set(string key, string value)
        {
            entries[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return entries.ContainsKey(key);
        }

        public string Get(string key)
        {
            return entries[key];
        }

        public string TryGet(string key, string fallbackValue = null)
        {
            return ContainsKey(key) ? Get(key) : fallbackValue;
        }

        public bool Remove(string key)
        {
            return entries.Remove(key);
        }

        public string this[string key]
        {
            get => Get(key);
            set => Set(key, value);
        }
    }
}
