# External Libraries

PDFCreator uses different external assemblies that are not part of our source code. Some of the are required to compile PDFCreator, some are only required to run the Unit tests. This is a list of these libraries and where you can find them:

## Required

 - NLog\NLog.dll: Used for logging [http://nlog-project.org/](http://nlog-project.org/)
 - iTextSharp.dll: PDF postprocessing [http://sourceforge.net/projects/itextsharp/](http://sourceforge.net/projects/itextsharp/)
 - ftlib.dll: Provides FTP upload [http://ftplib.codeplex.com/](http://ftplib.codeplex.com/)
 - SystemWrapper\SystemWrapper.dll: Provides interface abstraction for System classes to allowing mocks and testing [http://systemwrapper.codeplex.com/](http://systemwrapper.codeplex.com/)

## Tests
 - LibTiff\BitMiracle.LibTiff.NET.dll: performs checks on the TIFF output [http://bitmiracle.com/libtiff/](http://bitmiracle.com/libtiff/)
 - NUnit\nunit.framework.dll: Unit testing framework [http://www.nunit.org/](http://www.nunit.org/)
 - RhinoMocks\RhinoMocks.dll: .Net mocking library [http://hibernatingrhinos.com/oss/rhino-mocks](http://hibernatingrhinos.com/oss/rhino-mocks)
 - ImapX.dll: IMAP connector library [https://imapx.codeplex.com/](https://imapx.codeplex.com/)
 - PdfFileAnalyzerLibrary.dll: Parser for PDF files [http://www.codeproject.com/Articles/450254/PDF-File-Analyzer-With-Csharp-Parsing-Classes-Vers](http://www.codeproject.com/Articles/450254/PDF-File-Analyzer-With-Csharp-Parsing-Classes-Vers)