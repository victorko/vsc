using System;
using System.Collections.Generic;
//using System.Diagnostics.Contracts;
using Vsc.Core;

namespace Vsc.Assembler
{
    public static class Assembler
    {
        public static double[] Compile(string code)
        {
            //Contract.Requires(!string.IsNullOrEmpty(code));

            var lines = code.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var words = new List<string>();
            foreach (var line in lines)
            {
                var text = line;

                var commentStart = text.IndexOf("//");
                if (commentStart >= 0)
                    text = text.Substring(0, commentStart);

                var tokens = text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                words.AddRange(tokens);
            }

            var labels = new Dictionary<string, double>();
            var offset = 0;
            foreach (var word in words)
            {
                if (word.EndsWith(":"))
                {
                    var labelKey = word.Substring(0, word.Length - 1).ToUpperInvariant();
                    labels.Add(labelKey, offset);
                }
                else
                {
                    offset++;
                }
            }

            var result = new double[offset];
            var index = 0;
            foreach (var word in words)
            {
                if (word.EndsWith(":"))
                {
                    continue;
                }

                if (word.StartsWith("#"))
                {
                    var labelKey = word.Substring(1).ToUpperInvariant();
                    result[index] = labels[labelKey];
                }
                else
                {
                    //OpCode command;
                    //if (!Enum.TryParse(word, true, out command))
                    //    throw new Exception("Unknown command");
                    var command = (OpCode)Enum.Parse(typeof(OpCode), word, true);
                    result[index] = (double)command;

                }
                index++;
            }

            return result;
        }
    }
}
