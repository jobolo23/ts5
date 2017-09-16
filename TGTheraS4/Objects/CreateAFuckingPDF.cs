using System.Diagnostics;
using System.Linq;
using System.Windows;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace TheraS5.Objects
{
    internal class CreateAFuckingPDF
    {
        public XFont font = new XFont("Courier New", 10, XFontStyle.Regular);
        public PageOrientation pageO = PageOrientation.Portrait;
        public string pdfTitle = "";

        public string text = "";
        public string truenamebro = "";

        /*
        public void createThisShit(string fuckingtext, string truenamebro)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //XPdfFontOptions options = new XPdfFontOption(PdfFontEncoding.Unicode,PdfFontEmbedding.Always);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.Italic);

            // Draw the text
            gfx.DrawString(fuckingtext, font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height),
            XStringFormats.TopLeft);

            // Save the document...
            string filename = truenamebro + ".pdf";
            document.Save(filename);
            // ...and start a viewer.
            // Process.Start(filename);
        }
         */

        public bool show { get; set; }

        public void createThisShit(string text, string truenamebro)
        {
            var lines = text.Split('\n');

            var document = new PdfDocument();
            var seite = "";
            var seitenn = lines.Count() % 70;
            var seitennn = lines.Count() / 70;
            var seiten = 1;
            if (seitenn > 0)
            {
                seiten = seitennn + 1;
            }
            else
            {
                seiten = seitennn;
            }
            if (seiten < 1)
            {
                seiten = 1;
            }
            var seiten_count = 1;
            for (var i = 0; i < lines.Count(); i++)
            {
                seite += lines[i] + '\n';
                if (i != 0 && i % 70 == 0)
                {
                    var page = document.AddPage();
                    page.Orientation = pageO;
                    var gfx = XGraphics.FromPdfPage(page);
                    //XFont font = new XFont("Courier New", 10, XFontStyle.Regular);
                    var tf = new XTextFormatter(gfx);
                    var tf2 = new XTextFormatter(gfx);
                    var rect2 = new XRect(5, 5, page.Width - 10, page.Height - 10);
                    gfx.DrawRectangle(XBrushes.White, rect2);

                    tf2.DrawString("     Thera", font, XBrushes.DarkBlue, rect2, XStringFormats.TopLeft);
                    tf2.DrawString("          S4", font, XBrushes.DarkRed, rect2, XStringFormats.TopLeft);

                    if (pageO == PageOrientation.Portrait)
                    {
                        if (pdfTitle != "")
                        {
                            tf2.DrawString(
                                fillSpace(12, " ") + " - " + pdfTitle +
                                fillSpace(87 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                                "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                                XStringFormats.TopLeft);
                        }
                        else
                        {
                            tf2.DrawString(
                                fillSpace(102 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                                seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                        }
                    }
                    else
                    {
                        if (pdfTitle != "")
                        {
                            tf2.DrawString(
                                fillSpace(12, " ") + " - " + pdfTitle +
                                fillSpace(124 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                                "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                                XStringFormats.TopLeft);
                        }
                        else
                        {
                            tf2.DrawString(
                                fillSpace(139 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                                seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                        }
                    }
                    var rect = new XRect(30, 30, page.Width - 60, page.Height - 60);
                    gfx.DrawRectangle(XBrushes.White, rect);

                    //tf.Alignment = ParagraphAlignment.Left;
                    tf.DrawString(seite, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                    seite = "";
                    seiten_count++;
                }
            }

            if (seite != "")
            {
                var page = document.AddPage();
                page.Orientation = pageO;
                var gfx = XGraphics.FromPdfPage(page);
                //XFont font = new XFont("Courier New", 10, XFontStyle.Regular);
                var tf = new XTextFormatter(gfx);
                var tf2 = new XTextFormatter(gfx);
                var rect2 = new XRect(5, 5, page.Width - 10, page.Height - 10);
                gfx.DrawRectangle(XBrushes.White, rect2);

                tf2.DrawString("     Thera", font, XBrushes.DarkBlue, rect2, XStringFormats.TopLeft);
                tf2.DrawString("          S4", font, XBrushes.DarkRed, rect2, XStringFormats.TopLeft);

                if (pageO == PageOrientation.Portrait)
                {
                    if (pdfTitle != "")
                    {
                        tf2.DrawString(
                            fillSpace(12, " ") + " - " + pdfTitle +
                            fillSpace(87 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                            "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        tf2.DrawString(
                            fillSpace(102 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                            seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                    }
                }
                else
                {
                    if (pdfTitle != "")
                    {
                        tf2.DrawString(
                            fillSpace(12, " ") + " - " + pdfTitle +
                            fillSpace(124 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                            "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        tf2.DrawString(
                            fillSpace(139 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                            seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                    }
                }
                var rect = new XRect(30, 30, page.Width - 60, page.Height - 60);
                gfx.DrawRectangle(XBrushes.White, rect);
                //tf.Alignment = ParagraphAlignment.Left;
                tf.DrawString(seite, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            // Save the document...
            document.Save(truenamebro);
            // ...and start a viewer.

            if (MessageBox.Show("Abrechnung öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
            {
                Process.Start(truenamebro);
            }
        }

        public void createThisShit2()
        {
            var lines = text.Split('\n');

            var document = new PdfDocument();

            var seite = "";
            var seitenn = lines.Count() % 69;
            var seitennn = lines.Count() / 69;
            var seiten = 1;
            if (seitenn > 0)
            {
                seiten = seitennn + 1;
            }
            else
            {
                seiten = seitennn;
            }
            if (seiten < 1)
            {
                seiten = 1;
            }
            var seiten_count = 1;
            for (var i = 0; i < lines.Count(); i++)
            {
                seite += lines[i] + '\n';
                if (i != 0 && i % 69 == 0)
                {
                    var page = document.AddPage();
                    page.Orientation = pageO;
                    var gfx = XGraphics.FromPdfPage(page);
                    //XFont font = new XFont("Courier New", 10, XFontStyle.Regular);
                    var tf = new XTextFormatter(gfx);


                    var tf2 = new XTextFormatter(gfx);
                    var rect2 = new XRect(5, 5, page.Width - 10, page.Height - 10);
                    gfx.DrawRectangle(XBrushes.White, rect2);

                    tf2.DrawString("     Thera", font, XBrushes.DarkBlue, rect2, XStringFormats.TopLeft);
                    tf2.DrawString("          S4", font, XBrushes.DarkRed, rect2, XStringFormats.TopLeft);

                    if (pageO == PageOrientation.Portrait)
                    {
                        if (pdfTitle != "")
                        {
                            tf2.DrawString(
                                fillSpace(12, " ") + " - " + pdfTitle +
                                fillSpace(81 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                                "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                                XStringFormats.TopLeft);
                        }
                        else
                        {
                            tf2.DrawString(
                                fillSpace(96 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                                seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                        }
                    }
                    else
                    {
                        if (pdfTitle != "")
                        {
                            tf2.DrawString(
                                fillSpace(12, " ") + " - " + pdfTitle +
                                fillSpace(118 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                                "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                                XStringFormats.TopLeft);
                        }
                        else
                        {
                            tf2.DrawString(
                                fillSpace(132 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                                seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                        }
                    }
                    var rect = new XRect(30, 30, page.Width - 60, page.Height - 60);
                    gfx.DrawRectangle(XBrushes.White, rect);

                    //tf.Alignment = ParagraphAlignment.Left;
                    tf.DrawString(seite, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                    var xim = XImage.FromFile(@"logo.tg");
                    gfx.DrawImage(xim, 10, 10, 25, 20);
                    seite = "";
                    seiten_count++;
                }
            }

            if (seite != "")
            {
                var page = document.AddPage();
                page.Orientation = pageO;
                var gfx = XGraphics.FromPdfPage(page);

                var tf = new XTextFormatter(gfx);


                var tf2 = new XTextFormatter(gfx);
                var rect2 = new XRect(5, 5, page.Width - 10, page.Height - 10);
                gfx.DrawRectangle(XBrushes.White, rect2);

                tf2.DrawString("     Thera", font, XBrushes.DarkBlue, rect2, XStringFormats.TopLeft);
                tf2.DrawString("          S4", font, XBrushes.DarkRed, rect2, XStringFormats.TopLeft);

                if (pageO == PageOrientation.Portrait)
                {
                    if (pdfTitle != "")
                    {
                        tf2.DrawString(
                            fillSpace(12, " ") + " - " + pdfTitle +
                            fillSpace(81 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                            "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        tf2.DrawString(
                            fillSpace(96 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                            seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                    }
                }
                else
                {
                    if (pdfTitle != "")
                    {
                        tf2.DrawString(
                            fillSpace(12, " ") + " - " + pdfTitle +
                            fillSpace(118 - pdfTitle.Length - ("Seite " + seiten + " von " + seiten).Length, " ") +
                            "Seite " + seiten_count + " von " + seiten, font, XBrushes.Gray, rect2,
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        tf2.DrawString(
                            fillSpace(132 - ("Seite " + seiten + " von " + seiten).Length, " ") + "Seite " +
                            seiten_count + " von " + seiten, font, XBrushes.Gray, rect2, XStringFormats.TopLeft);
                    }
                }
                var rect = new XRect(30, 30, page.Width - 60, page.Height - 60);
                gfx.DrawRectangle(XBrushes.White, rect);
                //tf.Alignment = ParagraphAlignment.Left;
                tf.DrawString(seite, font, XBrushes.Black, rect, XStringFormats.TopLeft);


                var xim = XImage.FromFile(@"logo.tg");
                gfx.DrawImage(xim, 10, 10, 25, 20);
            }

            // Save the document...
            document.Save(truenamebro);
            // ...and start a viewer.

            if (show)
            {
                if (MessageBox.Show("Abrechnung öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    Process.Start(truenamebro);
                }
            }
        }

        private string fillSpace(int len, string s)
        {
            var ret = "";
            for (var i = 0; i < len; i++)
            {
                ret += s;
            }
            return ret;
        }
    }
}