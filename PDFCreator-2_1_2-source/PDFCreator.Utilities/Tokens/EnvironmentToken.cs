using System;

namespace pdfforge.PDFCreator.Utilities.Tokens
{
    public class EnvironmentToken : IToken
    {
        private readonly string _name;

        public EnvironmentToken()
        {
            _name = "Environment";
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Token Name</param>
        public EnvironmentToken(string name)
        {
            _name = name;
        }

        /// <summary>
        ///     Returns value of Token
        /// </summary>
        /// <returns>Value of Token as string</returns>
        public string GetValue()
        {
            return "";
        }

        /// <summary>
        ///     Returns Value of Token in given C#-format
        /// </summary>
        /// <param name="formatString">C#-format String</param>
        /// <returns>Formated Value as string</returns>
        public string GetValueWithFormat(string formatString)
        {
            try
            {
                string s = Environment.GetEnvironmentVariable(formatString);
                if (s == null)
                    return "";
                return s;
            }
            catch (ArgumentNullException)
            {
                return "";
            }
        }

        /// <summary>
        ///     Returns Name of Token
        /// </summary>
        /// <returns>Name of Token as String</returns>
        public string GetName()
        {
            return _name;
        }
    }
}