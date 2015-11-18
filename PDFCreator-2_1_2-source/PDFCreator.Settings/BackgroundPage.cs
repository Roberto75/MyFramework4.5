using System;
using System.Collections.Generic;
using System.Text;
using pdfforge.DataStorage;
using pdfforge.DataStorage.Storage;
// Custom Code starts here
// START_CUSTOM_SECTION:INCLUDES
using pdfforge.PDFCreator.Core.Settings.Enums;
// END_CUSTOM_SECTION:INCLUDES
// Custom Code ends here. Do not edit below


// ! This file is generated automatically.
// ! Do not edit it outside the sections for custom code.
// ! These changes will be deleted during the next generation run

namespace pdfforge.PDFCreator.Core.Settings
{
	/// <summary>
	/// Adds a page background to the resulting document
	/// </summary>
	public class BackgroundPage {
		
		/// <summary>
		/// Enables the BackgroundPage action
		/// </summary>
		public bool Enabled { get; set; }
		
		/// <summary>
		/// Filename of the PDF that will be used as background
		/// </summary>
		public string File { get; set; }
		
		/// <summary>
		/// If true, the background will be placed on the attachment as well
		/// </summary>
		public bool OnAttachment { get; set; }
		
		/// <summary>
		/// If true, the background will be placed on the cover as well
		/// </summary>
		public bool OnCover { get; set; }
		
		/// <summary>
		/// Defines the way the background document is repeated. Valid values are: NoRepetition, RepeatAllPages, RepeatLastPage
		/// </summary>
		public BackgroundRepetition Repetition { get; set; }
		
		
		private void Init() {
			Enabled = false;
			File = "";
			OnAttachment = false;
			OnCover = false;
			Repetition = BackgroundRepetition.RepeatLastPage;
		}
		
		public BackgroundPage()
		{
			Init();
		}
		
		public void ReadValues(Data data, string path)
		{
			try { Enabled = bool.Parse(data.GetValue(@"" + path + @"Enabled")); } catch { Enabled = false;}
			try { File = Data.UnescapeString(data.GetValue(@"" + path + @"File")); } catch { File = "";}
			try { OnAttachment = bool.Parse(data.GetValue(@"" + path + @"OnAttachment")); } catch { OnAttachment = false;}
			try { OnCover = bool.Parse(data.GetValue(@"" + path + @"OnCover")); } catch { OnCover = false;}
			try { Repetition = (BackgroundRepetition) Enum.Parse(typeof(BackgroundRepetition), data.GetValue(@"" + path + @"Repetition")); } catch { Repetition = BackgroundRepetition.RepeatLastPage;}
		}
		
		public void StoreValues(Data data, string path)
		{
			data.SetValue(@"" + path + @"Enabled", Enabled.ToString());
			data.SetValue(@"" + path + @"File", Data.EscapeString(File));
			data.SetValue(@"" + path + @"OnAttachment", OnAttachment.ToString());
			data.SetValue(@"" + path + @"OnCover", OnCover.ToString());
			data.SetValue(@"" + path + @"Repetition", Repetition.ToString());
		}
		
		public BackgroundPage Copy()
		{
			BackgroundPage copy = new BackgroundPage();
			
			copy.Enabled = Enabled;
			copy.File = File;
			copy.OnAttachment = OnAttachment;
			copy.OnCover = OnCover;
			copy.Repetition = Repetition;
			
			return copy;
		}
		
		public override bool Equals(object o)
		{
			if (!(o is BackgroundPage)) return false;
			BackgroundPage v = o as BackgroundPage;
			
			if (!Enabled.Equals(v.Enabled)) return false;
			if (!File.Equals(v.File)) return false;
			if (!OnAttachment.Equals(v.OnAttachment)) return false;
			if (!OnCover.Equals(v.OnCover)) return false;
			if (!Repetition.Equals(v.Repetition)) return false;
			
			return true;
		}
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.AppendLine("Enabled=" + Enabled.ToString());
			sb.AppendLine("File=" + File.ToString());
			sb.AppendLine("OnAttachment=" + OnAttachment.ToString());
			sb.AppendLine("OnCover=" + OnCover.ToString());
			sb.AppendLine("Repetition=" + Repetition.ToString());
			
			return sb.ToString();
		}
		
		public override int GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
		
// Custom Code starts here
// START_CUSTOM_SECTION:GENERAL
// END_CUSTOM_SECTION:GENERAL
// Custom Code ends here. Do not edit below
		
	}
}
