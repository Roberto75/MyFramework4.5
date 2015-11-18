using System;
using System.Collections.Generic;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.Shared.Helper
{
    public static class TokenHelper
    {
        private static TokenReplacer _tokenReplacer;

        public static TokenReplacer TokenReplacerWithPlaceHolders
        {
            get { return _tokenReplacer ?? (_tokenReplacer = CreateTokenReplacerWithPlaceHolders()); }
        }

        private static TokenReplacer CreateTokenReplacerWithPlaceHolders()
        {
            var tr = new TokenReplacer();

            if (!TranslationHelper.Instance.IsInitialized)
                return tr;

            tr.AddToken(new StringToken("Author", Environment.UserName));
            tr.AddToken(new StringToken("PrintJobAuthor", Environment.UserName));
            tr.AddToken(new StringToken("ClientComputer", Environment.MachineName));
            tr.AddToken(new StringToken("ComputerName", Environment.MachineName));
            tr.AddToken(new NumberToken("Counter", 1234));
            tr.AddToken(new DateToken("DateTime", DateTime.Now));
            tr.AddToken(new StringToken("InputFilename", TranslationHelper.Instance.GetTranslation("TokenPlaceHolders", "MyFileDocx", "MyFile.docx")));
            tr.AddToken(new StringToken("InputFilePath", @"C:\Temp"));
            tr.AddToken(new NumberToken("JobID", 1));
            tr.AddToken(new ListToken("OutputFilenames",
                new[] {TranslationHelper.Instance.GetTranslation("TokenPlaceHolders","OutputFilename", "OutputFilename.jpg")
                    , TranslationHelper.Instance.GetTranslation("TokenPlaceHolders","OutputFilename2", "OutputFilename2.jpg")
                    , TranslationHelper.Instance.GetTranslation("TokenPlaceHolders","OutputFilename3", "OutputFilename3.jpg")}));
            tr.AddToken(new StringToken("OutputFilePath", @"C:\Temp"));
            tr.AddToken(new StringToken("PrinterName", "PDFCreator"));
            tr.AddToken(new NumberToken("SessionID", 0));
            tr.AddToken(new StringToken("Title", TranslationHelper.Instance.GetTranslation("TokenPlaceHolders", "TitleFromSettings", "Title from Settings")));
            tr.AddToken(new StringToken("PrintJobName", TranslationHelper.Instance.GetTranslation("TokenPlaceHolders", "TitleFromPrintJob", "Title from Printjob")));
            tr.AddToken(new StringToken("Username", Environment.UserName));
            tr.AddToken(new EnvironmentToken());

            return tr;
        }

        public static List<string> GetTokenListWithFormatting()
        {
            var tokenList = new List<string>();
            tokenList.AddRange(TokenReplacerWithPlaceHolders.GetTokenNames());
            tokenList.Sort();   
            tokenList.Insert(tokenList.IndexOf("<DateTime>") + 1, "<DateTime:yyyyMMddHHmmss>");
            tokenList.Insert(tokenList.IndexOf("<Environment>") + 1, "<Environment:UserName>");
            
            return tokenList;
        }

        public static List<string> GetTokenListForAuthor()
        {
            var tokenList = GetTokenListWithFormatting();

            tokenList.Remove("<Author>");
            tokenList.Remove("<Title>");
            tokenList.Remove("<OutputFilenames>");
            tokenList.Remove("<InputFilename>");
            tokenList.Remove("<InputFilePath>");
            tokenList.Remove("<OutputFilePath>");

            return tokenList;
        }

        public static List<string> GetTokenListForTitle()
        {
            var tokenList = GetTokenListWithFormatting();

            tokenList.Remove("<Title>");
            tokenList.Remove("<OutputFilenames>");
            tokenList.Remove("<InputFilename>");
            tokenList.Remove("<InputFilePath>");
            tokenList.Remove("<OutputFilePath>");

            return tokenList;
        }

        public static List<string> GetTokenListForFilename()
        {
            var tokenList = GetTokenListWithFormatting();

            tokenList.Remove("<OutputFilenames>");
            tokenList.Remove("<InputFilePath>");
            tokenList.Remove("<OutputFilePath>");

            return tokenList;
        }
        
        public static List<string> GetTokenListForDirectory()
        {
            var tokenList = GetTokenListWithFormatting();
            tokenList.Remove("<OutputFilePath>");

            return tokenList;
        }

        public static List<string> GetTokenListForEmail()
        {
            var tokenList = GetTokenListWithFormatting();

            tokenList.Insert(tokenList.IndexOf("<OutputFilePath>") + 1, "<OutputFilenames:, >");
            tokenList.Insert(tokenList.IndexOf("<OutputFilePath>") + 2, "<OutputFilenames:\\r\\n>");
            tokenList.Remove("<OutputFilePath>");

            return tokenList;
        }
    }
}
