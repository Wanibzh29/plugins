﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Nikse.SubtitleEdit.PluginLogic.Logic
{
    public static class Utilities
    {
        internal static readonly char[] NewLineChars = Environment.NewLine.ToCharArray();

        #region String Extensions

        public static bool StartsWith(this string s, char c)
        {
            return s.Length > 0 && s[0] == c;
        }

        public static bool StartsWith(this StringBuilder sb, char c)
        {
            return sb.Length > 0 && sb[0] == c;
        }

        public static bool EndsWith(this string s, char c)
        {
            return s.Length > 0 && s[s.Length - 1] == c;
        }

        public static bool EndsWith(this StringBuilder sb, char c)
        {
            return sb.Length > 0 && sb[sb.Length - 1] == c;
        }

        public static bool Contains(this string source, char value)
        {
            return source.IndexOf(value) >= 0;
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        public static string[] SplitToLines(this string s)
        {
            return s.Replace(Environment.NewLine, "\n").Replace('\r', '\n').Split('\n');
        }

        #endregion

        internal static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static bool IsInteger(string s)
        {
            int i;
            if (int.TryParse(s, out i))
                return true;
            return false;
        }

        public static string RemoveHtmlTags(string s, bool alsoSsa)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            int idx;
            if (alsoSsa)
            {
                const string SSATAg = "{\\";
                idx = s.IndexOf(SSATAg, StringComparison.Ordinal);
                while (idx >= 0)
                {
                    var endIdx = s.IndexOf('>', idx + 2);
                    if (endIdx < idx)
                        break;
                    s = s.Remove(idx, endIdx - idx + 1);
                    idx = s.IndexOf(SSATAg, idx, StringComparison.Ordinal);
                }
            }
            if (!s.Contains("<"))
                return s;
            s = Regex.Replace(s, "(?i)</?[iіbu]>", string.Empty);

            s = Regex.Replace(s, "</?font>", string.Empty, RegexOptions.IgnoreCase);
            idx = s.IndexOf("<font", StringComparison.Ordinal);
            while (idx >= 0)
            {
                while (idx >= 0)
                {
                    var endIdx = s.IndexOf('>', idx + 5);
                    if (endIdx < idx)
                        break;
                    s = s.Remove(idx, endIdx - idx + 1);
                    idx = s.IndexOf("<font", idx, StringComparison.Ordinal);
                }
            }
            return s.Trim();
        }

        public static int GetNumberOfLines(string text)
        {
            if (text == null)
                return 0;
            var lines = 1;
            var idx = text.IndexOf('\n');
            while (idx >= 0)
            {
                lines++;
                idx = text.IndexOf('\n', idx + 1);
            }
            return lines;
        }

        public static string GetWordListFileName()
        {
            // "%userprofile%Desktop\SubtitleEdit\Plugins\SeLinesUnbreaker.xml"
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (path.StartsWith("file:\\", StringComparison.Ordinal))
                path = path.Remove(0, 6);
            path = Path.Combine(path, "Plugins");
            if (!Directory.Exists(path))
                path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Subtitle Edit"), "Plugins");
            return Path.Combine(path, "WordList.xml");
        }
    }
}