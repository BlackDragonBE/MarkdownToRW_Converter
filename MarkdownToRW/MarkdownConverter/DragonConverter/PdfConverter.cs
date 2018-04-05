using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DinkToPdf;
using DinkToPdf.EventDefinitions;
using DragonMarkdown.Utility;

namespace DragonMarkdown.DragonConverter
{
    public static class PdfConverter
    {
        public static event Action<string> OnProgressChangedEvent;

        public static void ConvertToPdf(string html, string ouputPath)
        {
            var converter = new BasicConverter(new PdfTools());

            converter.ProgressChanged += ConverterOnProgressChanged;
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings(15,15,15,15),
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        LoadSettings = new LoadSettings(){JSDelay = 1000, StopSlowScript = false},
                        WebSettings = { DefaultEncoding = "utf-8" ,EnableIntelligentShrinking = true, PrintMediaType = true},
                        HeaderSettings = { FontSize = 9, Left = "Title", Right = "[section]", Line = false, Spacing = 5},
                        FooterSettings = { FontSize = 9, Left = "raywenderlich.com", Right = "[page]", Line = false, Spacing = 5 },
                    }
                }
            };

            Console.WriteLine("Saving PDF to " + ouputPath);
            byte[] pdf = converter.Convert(doc);
            
            using (FileStream stream = new FileStream(ouputPath, FileMode.Create))
            {
                stream.Write(pdf, 0, pdf.Length);
            }

            OnProgressChangedEvent = null;
        }

        private static void ConverterOnProgressChanged(object sender, ProgressChangedArgs progressChangedArgs)
        {
            OnProgressChangedEvent?.Invoke(progressChangedArgs.Description);
        }
    }
}
