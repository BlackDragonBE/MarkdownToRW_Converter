using System;
using System.Collections.Generic;
using System.Text;
using DinkToPdf;
using DinkToPdf.EventDefinitions;

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
                    Out = ouputPath,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Left = "My Amazing Title", Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    }
                }
            };

            converter.Convert(doc);
        }

        private static void ConverterOnProgressChanged(object sender, ProgressChangedArgs progressChangedArgs)
        {
            OnProgressChangedEvent?.Invoke(progressChangedArgs.Description);
        }
    }
}
