using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateParser
{
    public class ParseTokenHelper
    {
        

        private const char _LABEL_OPEN_CHAR = '[';
        private const char _LABEL_CLLOSE_CHAR = ']';

        /// <summary>
        /// The collections of the tokens
        /// Such as: [[Hello], [Contact.FirstName],  [Contact.LastName]]
        /// </summary>
        private readonly List<string> _tokens = new List<string>();

        /// <summary>
        /// A tempory storage
        /// </summary>
        private readonly StringBuilder _temp = new StringBuilder();

        /// <summary>
        /// Current status
        /// </summary>
        private ParseMode _currentState;


        /// <summary>
        /// Previous status
        /// </summary>
        private ParseMode _lastState;

        /// <summary>
        /// When enter a token
        /// </summary>
        /// <param name="mode"><see cref="ParseMode">Status enumb</param>
        private void _EnterMode(ParseMode mode)
        {
            if (this._temp.Length > 0)
            {
                this._tokens.Add(this._temp.ToString());
                this._temp.Clear();

            }

            this._lastState = this._currentState;
            this._currentState = mode;
        }

        private void _LeaveMode()
        {
            if (this._temp.Length > 0)
            {
                this._tokens.Add(this._temp.ToString());
                this._temp.Clear();

            }
            this._currentState = this._lastState;
        }
        
        public List<string> ParseTemplate(string template)
        {
            if (String.IsNullOrEmpty(template)) return null;
            var templateLength = template.Length;

            for (var index = 0; index < templateLength; index++)
            {
                var cur = template[index];
                switch (cur)
                {
                    case char.MinValue:
                        break;
                    case _LABEL_OPEN_CHAR:
                        this._EnterMode(ParseMode.EnterLabel);
                        this._temp.Append(cur);
                        break;

                    case _LABEL_CLLOSE_CHAR:
                        this._temp.Append(cur);
                        this._LeaveMode();
                        break;

                    default:
                        this._temp.Append(cur);
                        break;                      
                }
            }

            //if (this._temp.Length <= 0) return null;
            
            this._tokens.Add(this._temp.ToString());
            this._tokens.Remove(this._tokens[this._tokens.Count - 1]);
                        
            this._temp.Clear();
            return this._tokens;
                
        }



    }
}
