﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stylus.DataModel;

namespace Stylus.Literal
{
    // "xxx"@en
    internal class LangLiteralParser : LiteralParser
    {
        public override bool CanParse(string str)
        {
            // Exception: "@aaabbb"
            // return str.StartsWith("\"") && str.Contains("\"@");
            if (!str.StartsWith("\"") || str.EndsWith("\"")) // must startswith \", but must not endwswith \"
            {
                return false;
            }
            int index = str.IndexOf("\"@");
            if (index <= 0 || index >= str.Length - 2)
            {
                return false;
            }
            return true;
        }

        public override bool CanParse(Literal literal)
        {
            return literal.LiteralType == LiteralType.LANG;
        }

        public override bool CanParse(IndexEntry indexEntry)
        {
            return indexEntry.LiteralType == (int)LiteralType.LANG;
        }

        public override Literal FromIndexEntry(IndexEntry indexEntry, Func<int, string> prefixSuffix2str)
        {
            return new Literal
            {
                LiteralType = LiteralType.LANG,
                PrefixSuffix = prefixSuffix2str(indexEntry.PrefixSuffixId),
                Value = indexEntry.EntryKey.ToString()
            };
        }

        public override Literal ParseLiteral(string str)
        {
            Literal ret = new Literal
            {
                LiteralType = LiteralType.LANG
            };
            int idx = str.LastIndexOf("\"@");
            ret.Value = str.Substring(1, idx - 1);
            ret.PrefixSuffix = str.Substring(idx + 2);
            return ret;
        }

        public override IndexEntry ToIndexEntry(Literal literal, Func<string, int> str2prefixSuffix)
        {
            return new IndexEntry
            {
                LiteralType = (int)LiteralType.LANG,
                EntryKey = literal.Value,
                PrefixSuffixId = str2prefixSuffix(literal.PrefixSuffix)
            };
        }

        public override string ToString(Literal literal)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"");
            builder.Append(literal.Value);
            builder.Append("\"@");
            builder.Append(literal.PrefixSuffix);
            return builder.ToString();
        }
    }
}
