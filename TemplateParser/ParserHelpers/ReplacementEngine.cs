using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateParser
{
    /// <summary>
    /// A simple engine of replacement template
    /// </summary>
    public sealed class ReplacementEngine : IReplacementEngine
    {
        private const char _LEFT_BRACKET = '[';
        private const char _RIGHT_BRACKET = ']';
        private const char _FORMAT_STRING_PREAMBLE = '"';

        private readonly Stack<LexerState> _lexerModes = new Stack<LexerState>();
        private readonly List<Token> _tokens = new List<Token>();
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
        private int _column;
        private LexerState _currentState;
        private int _line;
        private int _position;

        private int _savedColumn;
        private int _savedLine;
        private int _savedPosition;
        private string _curParent=String.Empty;

        private string _templateString;


        public ReplacementEngine()
        {
        }

        /// <summary>
        /// Get the list of tokens
        /// </summary>
        public List<Token> Tokens { get { return new List<Token>(this._tokens); } }

        #region Member of ITemplateEngine

        /// <summary>
        /// Introduce the methodology of state machine
        /// Read characters one by one, determine the status of the current characters
        /// then handle it accordingly
        /// </summary>
        /// <param name="templateString">Template string</param>
        public void Parser(string templateString)
        {
            if (string.IsNullOrEmpty(templateString)) return;

            this._line = 1;
            this._column = 1;
            this._position = 0;
            this._savedPosition = 0;
            this._currentState = LexerState.Text;
            this._tokens.Clear();
            this._lexerModes.Clear();

            this._templateString = templateString;

            int templateStringLength = this._templateString.Length;

            for (var index=0;index < templateStringLength; index++)
            {
                var curChar = templateString[index];
                this._position++;
                this._column++;

                switch (curChar)
                {
                    case _LEFT_BRACKET:
                        if (this._currentState == LexerState.Label) continue;

                        //Handle the current char in token mode
                        this._position--;
                        this._EnterMode(LexerState.Label);
                        this._position++;
                        this._tokens.Add(this._CreateToken(TokenType.LeftBracket, "[",""));
                        this._Save();
                        break;

                    case _RIGHT_BRACKET:
                        
                        //Activities when leaving a token
                        this._position--;                        
                        if (this._currentState == LexerState.FormatString) this._LeaveMode();
                        this._LeaveMode();
                        this._position++;
                        this._tokens.Add(this._CreateToken(TokenType.RightBracket, "]",""));
                        this._Save();
                        break;

                    case _FORMAT_STRING_PREAMBLE:
                        //Start a format string mode only when the current state is Label
                        if (this._currentState == LexerState.FormatString)
                        {
                            continue;
                        }                        

                        if(this._currentState == LexerState.Label)
                        {
                            this._position--;
                            this._EnterMode(LexerState.FormatString);
                            this._position++;

                            this._tokens.Add(this._CreateToken(TokenType.FormatStringPreamble, @"""",""));

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

       
        public string ProcessTemplate()
        {
            //throw new System.NotImplementedException();
            var result = new StringBuilder();
            for (var index = 0; index < this._tokens.Count; index++)
            {
                var token = this._tokens[index];
                switch (token.Kind)
                {
                    case TokenType.Token:
                        string value;

                        //Here the token flow is like{...,[Creationtime],",[Formating string],...}
                        //Hence the position of label is current position minus 2
                        if (index < this._tokens.Count - 2)
                        {
                            var nextToken = this._tokens[index + 2];
                            if (nextToken.Kind == TokenType.FormatString)
                            {
                                //Use IFormattable to identify the tokenis formattable
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
                    case TokenType.Text:
                        result.Append(token.Text);
                        break;
                }
            }
            return result.ToString();
        }

        
        #endregion
        private void _EnterMode(LexerState mode)
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

            this._lexerModes.Push(this._currentState);
            this._currentState = mode;

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

            this._currentState = this._lexerModes.Pop();
        }

        private Token _CreateToken(string text)
        {
            //throw new NotImplementedException();
            switch (this._currentState)
            {
                case LexerState.Label:
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
                    return this._CreateToken(TokenType.Token, tokenName,_curParent);

                case LexerState.FormatString:
                    string cleanedText = text.Trim().TrimEnd('"');
                    return this._CreateToken(TokenType.FormatString, cleanedText ,"");

                default:
                    return this._CreateToken(TokenType.Text, text, "");
            }
        }

        private Token _CreateToken(TokenType kind, string text, string parent)
        {
            //throw new NotImplementedException();
            if ((kind == TokenType.Token || kind == TokenType.FormatString) && string.IsNullOrEmpty(text))
                return null;
            return new Token(kind, text ?? String.Empty, this._savedLine, this._savedColumn, parent ?? String.Empty);
        }

        private void _Save()
        {
            this._savedLine = this._line;
            this._savedColumn = this._column;
            this._savedPosition = this._position;
        }


        #region Convert the datasource to the dictionary of subsititution keyworkds, whihch is the "_variables" 
        public void ConvertDataSourceToDict(object dataSource)
        {
            parseObj(dataSource, "");
        }

        /// <summary>
        /// Utilise relfection to find all the properties in the datasource
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentName"></param>
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