using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System.IO;

namespace IntegrationCore.Helper
{
    public class CompressPdf
    {
        private GhostscriptVersionInfo _gs_verssion_info;
        private string _inputFile;
        private string _outputFile;

        public string LIB
        {
            get
            {
                return @"C:\Program Files\gs\gs9.53.3\bin;" +
                       @"C:\Program Files\gs\gs9.53.3\lib;"
                        + @"C:\Program Files\gs\gs9.53.3\Resource\Font";
            }
        }

        public string DLL
        {
            get
            {
                return @"C:\Program Files\gs\gs9.53.3\bin\gsdll64.dll";
            }
        }

        public CompressPdf(string nameFile, string newNameFile)
        {
            this._inputFile = nameFile;
            this._outputFile = newNameFile;
            this._gs_verssion_info = new GhostscriptVersionInfo(new System.Version("9.53.3"), DLL, LIB, GhostscriptLicense.GPL);
        }

        public void ProcessFiles()
        {
            // check if gs9 installed in server
            string gsPathInstalled = @"C:\Program Files\gs\gs9.53.3";
            if (Directory.Exists(gsPathInstalled))
            {
                CompressDocument();
            }
            else
            {
                Logger.AddToLogFile("CompressPdf." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "GostScript isn't installed in server");
            }
        }

        private void CompressDocument()
        {
            try
            {
                List<string> gsArgs = new List<string>();

                gsArgs.Add("-empty");
                gsArgs.Add("-dSAFER");
                gsArgs.Add("-dBATCH");
                gsArgs.Add("-dNOPAUSE");
                gsArgs.Add("-dNOPROMPT");

                gsArgs.Add("-sDEVICE=pdfwrite");
                gsArgs.Add("-dCompatibilityLevel=1.4");
                // dPDFSETTINGS is basically our commpression option
                //  - /screen selects low-resolution output similar to the Acrobat Distiller "Screen Optimized" setting.
                //  - /ebook selects medium-resolution output similar to the Acrobat Distiller "eBook" setting.
                //  - /printer selects output similar to the Acrobat Distiller "Print Optimized" setting.
                //  - /prepress selects output similar to Acrobat Distiller "Prepress Optimized" setting.
                //  - /default selects output intended to be useful across a wide variety of uses, possibly at the expense of a larger output file.
                // gsArgs.Add("-dPDFSETTINGS=/screen");
                gsArgs.Add("-dPDFSETTINGS=/ebook");

                gsArgs.Add("-sOutputFile=" + _outputFile + "");
                gsArgs.Add("-f");
                gsArgs.Add(_inputFile);

                using (GhostscriptProcessor processor = new GhostscriptProcessor(_gs_verssion_info, true))
                    processor.StartProcessing(gsArgs.ToArray(), null);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
