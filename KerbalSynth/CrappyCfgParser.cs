using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSynth
{
    public static class CrappyCfgParser
    {
        public class CfgNode
        {
            public string name;
            public List<CfgValue> values = new List<CfgValue>();
            public List<CfgNode> nodes = new List<CfgNode>();
            public CfgNode(string name)
            {
                this.name = name;
            }
            public int ValueCount(string valueName)
            {
                valueName = valueName.ToLower();
                int res = 0;
                foreach (var value in values)
                {
                    if (value.name == valueName) res++;
                }
                return res;
            }
            public int NodeCount(string nodeName)
            {
                nodeName = nodeName.ToLower();
                int res = 0;
                foreach (var node in nodes)
                {
                    if (node.name == nodeName) res++;
                }
                return res;
            }
            public IEnumerator<CfgValue> EnumerateValue(string valueName)
            {
                valueName = valueName.ToLower();
                foreach (var value in values)
                {
                    if (value.name == valueName) yield return value;
                }
            }
            public IEnumerator<CfgNode> EnumerateNode(string nodeName)
            {
                nodeName = nodeName.ToLower();
                foreach (var node in nodes)
                {
                    if (node.name == nodeName) yield return node;
                }
            }
        }

        public class CfgValue
        {
            public string name;
            public string value;
            public CfgValue(string name, string value)
            {
                this.name = name; this.value = value;
            }
        }

        public static CfgNode ParseNode(string textContent, string name)
        {
            var res = new CfgNode(name);
            var state = ParserState.WaitingName;
            var bufferName = "";
            var bufferValue = "";
            for (int i = 0; i < textContent.Length; i++)
            {
                switch (state)
                {
                    case ParserState.WaitingName:
                        if (Utils.isSomething(textContent[i]))
                        {
                            i--;
                            state = ParserState.Name;
                        }
                        break;
                    case ParserState.Name:
                        if (Utils.isSomething(textContent[i]) && !"{=".Contains(textContent[i])) 
                        {
                            bufferName += textContent[i];
                        }
                        else
                        {
                            i--;
                            state = ParserState.WaitingOperator;
                        }
                        break;
                    case ParserState.WaitingOperator:
                        if (textContent[i] == '=')
                        {
                            state = ParserState.WaitingValue;
                        }
                        else if (textContent[i] == '{')
                        {
                            int rightBracketIndex = Utils.SearchBracketPair(textContent, i);
                            //recursion
                            CfgNode subNode = ParseNode(textContent.Substring(i + 1, rightBracketIndex - i - 1), bufferName.ToLower());
                            res.nodes.Add(subNode);
                            i = rightBracketIndex;
                            bufferName = "";
                            state = ParserState.WaitingName;
                        }
                        else if (Utils.isSomething(textContent[i]))
                        {
                            //裂开
                            throw new Exception("Syntax Exception : invalid operator " + textContent[i]);
                        }
                        break;
                    case ParserState.WaitingValue:
                        if (Utils.isSomething(textContent[i]))
                        {
                            i--;
                            state = ParserState.Value;
                        }
                        break;
                    case ParserState.Value:
                        if (!Utils.isLine(textContent[i]))
                        {
                            bufferValue += textContent[i];
                        }
                        else
                        {
                            res.values.Add(new CfgValue(bufferName.ToLower(), bufferValue));
                            bufferName = "";
                            bufferValue = "";
                            state = ParserState.WaitingName;
                        }
                        break;

                }
                if (i + 2 < textContent.Length && textContent[i + 1] == '/' && textContent[i + 2] == '/')
                {
                    i++;
                    while (i < textContent.Length && !Utils.isLine(textContent[i]))
                    {
                        i++;
                    }
                }
            }
            return res;
        }

        public enum ParserState
        {
            WaitingName,
            Name,
            WaitingOperator,
            WaitingValue,
            Value,
        }
    }
}
