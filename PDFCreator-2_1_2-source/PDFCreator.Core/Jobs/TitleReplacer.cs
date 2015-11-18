using System;
using System.Collections.Generic;
using pdfforge.PDFCreator.Core.Settings;

namespace pdfforge.PDFCreator.Core.Jobs
{
    /// <summary>
    /// Replaces occurances within a string in a given order. This is used to remove unwanted parts from titles in the JobInfos
    /// </summary>
    public class TitleReplacer
    {
        private readonly List<TitleReplacement> _replacements = new List<TitleReplacement>();

        /// <summary>
        /// Replace the title string with the replacements
        /// </summary>
        /// <param name="originalTitle">The original title where replacements should be applied</param>
        /// <returns>The title with replacements</returns>
        public string Replace(string originalTitle)
        {
            if (originalTitle == null)
                throw new ArgumentException("originalTitle");
            
            var title = originalTitle;

            foreach (var titleReplacement in _replacements)
            {
                title = title.Replace(titleReplacement.Search, titleReplacement.Replace);
            }

            return title;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="replaceWith"></param>
        public void AddReplacement(string searchString, string replaceWith)
        {
            AddReplacement(new TitleReplacement(searchString, replaceWith));
        }

        public void AddReplacement(TitleReplacement titleReplacement)
        {
            _replacements.Add(titleReplacement);
        }

        public void AddReplacements(IEnumerable<TitleReplacement> replacements)
        {
            foreach (var titleReplacement in replacements)
            {
                AddReplacement(titleReplacement);
            }
        }
    }
}