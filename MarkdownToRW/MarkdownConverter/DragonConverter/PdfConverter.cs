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
                    PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Left = "My Amazing Title", Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                        FooterSettings = { FontSize = 9, Left = "My Amazing Title", Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    }
                }
            };

            Console.WriteLine("Saving PDF to " + ouputPath);

            byte[] pdf = converter.Convert(doc);
            
            using (FileStream stream = new FileStream(ouputPath, FileMode.Create))
            {
                stream.Write(pdf, 0, pdf.Length);
            }
        }

        private static void ConverterOnProgressChanged(object sender, ProgressChangedArgs progressChangedArgs)
        {
            OnProgressChangedEvent?.Invoke(progressChangedArgs.Description);
        }
    }
}
