using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateParser
{
    public sealed class TemplateEngine : IReplacementEngine
    {
        private const char _LEFT_BACKET = '[';
        private const char _RIGHT_BACKET = ']';
        private const char _FORMAT_STRING_PREAMBLE = '"';

        private readonly Stack<LexerMode> _lexerModes = new Stack<LexerMode>();
        private readonly List<Token> _tokens = new List<Token>();
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
        private int _column;
        private LexerMode _currentMode;
        private int _line;
        private int _position;

        private int _savedColumn;
        private int _savedLine;
        private int _savedPosition;
        private string _curParent=String.Empty;

        private string _templateString;


        public TemplateEngine()
        {
        }

        public List<Token> Tokens { get { return new List<Token>(this._tokens); } }

        #region Member of ITemplateEngine
        public void Parser(string templateString)
        {
            if (string.IsNullOrEmpty(templateString)) return;

            this._line = 1;
            this._column = 1;
            this._position = 0;
            this._savedPosition = 0;
            this._currentMode = LexerMode.Text;
            this._tokens.Clear();
            this._lexerModes.Clear();

            this._templateString = templateString;

            int templateStringLength = this._templateString.Length;

            for (var index=0;index < templateStringLength; index++)
            {
                var cur = templateString[index];
                this._position++;
                this._column++;

                switch (cur)
                {
                    case _LEFT_BACKET:
                        if (this._currentMode == LexerMode.Label) continue;

                        //Handle the token
                        this._position--;
                        this._EnterMode(LexerMode.Label);
                        this._position++;
                        this._tokens.Add(this._CreateToken(TokenKind.LeftBracket, "[",""));
                        this._Save();
                        break;

                    case _RIGHT_BACKET:
                        this._position--;

                        //Handle the end of a token
                        if (this._currentMode == LexerMode.FormatString) this._LeaveMode();
                        this._LeaveMode();
                        this._position++;
                        this._tokens.Add(this._CreateToken(TokenKind.RightBracket, "]",""));
                        this._Save();
                        break;

                    case _FORMAT_STRING_PREAMBLE:
                        if (this._currentMode == LexerMode.FormatString)
                        {
                            continue;
                        }                        

                        if(this._currentMode == LexerMode.Label)
                        {
                            this._position--;
                            this._EnterMode(LexerMode.FormatString);
                            this._position++;

                            this._tokens.Add(this._CreateToken(TokenKind.FormatStringPreamble, @"""",""));

                            this._Save();
                        }
                        break;
                    case '\n':
                        this._line++;
                        break;

                }
            }

            if (this._position <= 0) return;
            var text = this._templateString.Substring(this._savedPosition, this._position - this._savedPosition);
            var token = this._CreateToken(text);
            this._tokens.Add(token);
            this._Save();

        }

        public void SetValue(string key, object value)
        {
            this._variables[key] = value;

        }

        
        public string Process()
        {
            //throw new System.NotImplementedException();
            var result = new StringBuilder();
            for (var index = 0; index < this._tokens.Count; index++)
            {
                var token = this._tokens[index];
                switch (token.Kind)
                {
                    case TokenKind.Label:
                        string value;
                        if (index < this._tokens.Count - 2)
                        {
                            var nextToken = this._tokens[index + 2];
                            if (nextToken.Kind == TokenKind.FormatString)
                            {
                                var obj = this._variables[token.Text] as IFormattable;
                                value = obj == null ? this._variables[token.Text].ToString() : obj.ToString(nextToken.Text, null);

                            }
                            else
                            {
                                //value = this._variables[token.Text].ToString();
                                value = this._variables.ContainsKey(token.Text)? this._variables[token.Text].ToString() : "";

                            }
                        }
                        else
                        {
                            value = this._variables[token.Text].ToString();
                        }
                        result.Append(value);
                        break;
                    case TokenKind.Text:
                        result.Append(token.Text);
                        break;
                }
            }
            return result.ToString();
        }

        
        #endregion
        private void _EnterMode(LexerMode mode)
        {
            //throw new NotImplementedException();
            if (this._position > 0)
            {
                var text = this._templateString.Substring(this._savedPosition, this._position - this._savedPosition);
                var token = this._CreateToken(text);
            
                if (token != null)
                {
                    this._tokens.Add(token);
                    this._Save();
                }
            }

            this._lexerModes.Push(this._currentMode);
            this._currentMode = mode;

        }

        private void _LeaveMode()
        {
            //throw new NotImplementedException();
            if (this._position > 0)
            {
                var text = this._templateString.Substring(this._savedPosition, this._position - this._savedPosition);
                var token = this._CreateToken(text);

                if (token != null)
                {
                    this._tokens.Add(token);
                    this._Save();
                }
            }

            this._currentMode = this._lexerModes.Pop();
        }

        private Token _CreateToken(string text)
        {
            //throw new NotImplementedException();
            switch (this._currentMode)
            {
                case LexerMode.Label:
                    if (text.StartsWith("with"))
                    {
                        string tempText = text.Split(' ')[1].Trim();
                        this._curParent = this._curParent==String.Empty? tempText : this._curParent+"."+tempText;
                    }else if (text.StartsWith("/with"))
                    {
                        //string tempText = _curParent;
                        var tempArr = _curParent.Split('.');
                        var newArr = tempArr.Take(tempArr.Length - 1);
                        _curParent = tempArr.Length==1? String.Empty:String.Join(".",newArr);
                    }
                    string tokenName = _curParent == String.Empty ? text.Trim() : _curParent + "." + text;
                    return this._CreateToken(TokenKind.Label, tokenName,_curParent);

                case LexerMode.FormatString:
                    string cleanedText = text.Trim().TrimEnd('"');
                    return this._CreateToken(TokenKind.FormatString, cleanedText ,"");

                default:
                    return this._CreateToken(TokenKind.Text, text, "");
            }
        }

        private Token _CreateToken(TokenKind kind, string text, string parent)
        {
            //throw new NotImplementedException();
            if ((kind == TokenKind.Label || kind == TokenKind.FormatString) && string.IsNullOrEmpty(text))
                return null;
            return new Token(kind, text ?? String.Empty, this._savedLine, this._savedColumn, parent ?? String.Empty);
        }

        private void _Save()
        {
            this._savedLine = this._line;
            this._savedColumn = this._column;
            this._savedPosition = this._position;
        }


        #region Convert the datasource to the dictionary of "_variables" 
        public void ConvertDataSourceToDict(object dataSource)
        {
            parseObj(dataSource, "");
        }

        private void parseObj(object obj, string parentName)
        {

            Type objType = obj.GetType();
            foreach (var prop in objType.GetProperties())
            {
                string name = parentName == "" ? prop.Name : parentName + "." + prop.Name;

                if (prop.GetValue(obj, null).GetType().ToString().Contains("AnonymousType"))
                {
                    parseObj(prop.GetValue(obj, null), name);
                    continue;
                }
                _variables[name] = prop.GetValue(obj, null);
                Console.WriteLine(name);
                Console.WriteLine(prop.GetValue(obj, null));
                Console.WriteLine("====");
            }

        }
        #endregion

    }
}