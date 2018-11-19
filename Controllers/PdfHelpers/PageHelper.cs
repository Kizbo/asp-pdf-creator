using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Web;
using HTML2PDF.Controllers.PdfHelpers.Elements;

namespace HTML2PDF.Controllers.PdfHelpers
{

    /// <summary>
    /// Klasa zajmuje się zarządzaniem stronami pliku PDF.
    /// </summary>

    public class PageHelper
    {

        PdfPage page;
        PdfDocument document;
        XGraphics gfx;
        PdfHelper ph;
        XPoint margins;

        public PageHelper(double marginX = 20, double marginY = 20, string tabName = "Dokument")
        {

            margins.X = marginX;
            margins.Y = marginY;

            document = new PdfDocument();
            document.Info.Title = tabName;
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);

            ph = new PdfHelper(gfx, page, margins.X, margins.Y);

        }

        List<IPdfElement> _wholeQueue = new List<IPdfElement>();

        public void Add(IPdfElement element) => _wholeQueue.Add(element);

        private void CheckAndCreatePages(bool noQueueClear = false)
        {

            XFont _font;
            double maxHeight = 0, lastx = margins.X, pageHeight = 0, width = 0, wordWidth = 0, calcHeight = 0, lineWidth = 0, lastLineWidth = 2;
            bool newLine = false;
            string _text = "", lineWord = "", wholeWord = "", tempWord = "";

            foreach (IPdfElement item in _wholeQueue)
            {

                if (item.GetType() != typeof(Paragraph))
                {

                    item.CalculateSize(gfx, lastx, margins);

                    if (item.IsFixed) { ph.Add(item); }
                    else
                    {

                        if (lastx + item.Width < page.Width - margins.X && item.Height < (page.Height - (margins.Y * 2) - pageHeight)) // W lastx już mamy margines, więc liczymy dodatkowo tylko prawy.
                        {

                            if ((item.Height + 1) > maxHeight) { maxHeight = item.Height + 1; }
                            lastx += item.Width + 1;
                            ph.Add(item);
                            newLine = false;

                        }
                        else
                        {

                            //pageHeight += maxHeight;
                            calcHeight += maxHeight;
                            lastx = margins.X + item.Width + 1;
                            maxHeight = item.Height + 1;
                            newLine = true;
                            lastLineWidth = 2;

                        }

                    }

                    if (newLine)
                    {

                        lastLineWidth = 2;

                        if ((pageHeight + maxHeight) < (page.Height - (margins.Y * 2)))
                        {

                            ph.Add(item);
                            lastLineWidth = 2;

                        }
                        else
                        {

                            ph.CreatePage();
                            pageHeight = 0;
                            lastLineWidth = 2;
                            lastx = margins.X + item.Width + 1;
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            ph = new PdfHelper(gfx, page, margins.X, margins.Y);
                            ph.Add(item);

                        }

                        newLine = false;

                    }

                }
                else
                {

                    _font = item.FontStyle;
                    _text = item.Text;

                    if (item.Width <= 0)
                    {

                        width = gfx.PageSize.Width - (margins.X * 2);
                        lastx = margins.X;

                    }
                    else
                    {

                        width = item.Width;

                    }

                    //if(width + lastx + prevItem.width > page.Width - margins.X)
                    //{

                    calcHeight += maxHeight;
                    maxHeight = 0;

                    //}

                    calcHeight += pageHeight;

                    for (int i = 0; i <= _text.Length; i++)
                    {

                        if (i < _text.Length)
                        {

                            while (true)
                            {

                                if (_text[i] == ' ') { tempWord += _text[i]; break; }
                                else if (i >= (_text.Length - 1)) { if (i <= _text.Length) { tempWord += _text[i]; } break; }
                                else { tempWord += _text[i]; i++; }

                            }

                        }

                        wordWidth = gfx.MeasureString(" " + tempWord, _font).Width;
                        lineWidth = gfx.MeasureString(lineWord, _font).Width;

                        if (lineWidth + wordWidth >= width)
                        {

                            if (calcHeight + _font.GetHeight(gfx) >= page.Height - (margins.Y * 3))
                            {

                                lastLineWidth = 2;
                                ph.Add(new Paragraph(wholeWord, false, 0, 0, 0, 0, _font.Name, _font.Size, _font.Style, item.Align, item.Format));
                                ph.CreatePage();
                                lastx = margins.X;
                                page = document.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                ph = new PdfHelper(gfx, page, margins.X, margins.Y);
                                wholeWord = "";
                                calcHeight = 0;

                            }
                            else
                            {

                                lastLineWidth = 2;
                                calcHeight += _font.GetHeight(gfx);
                                wholeWord += lineWord;
                                lineWord = tempWord;
                                tempWord = "";

                            }

                        }
                        else
                        {

                            lineWord += tempWord;
                            tempWord = "";

                        }

                    }

                    wholeWord += lineWord;
                    lineWord = "";

                    if (wholeWord != "")
                    {

                        ph.Add(new Paragraph(wholeWord, false, 0, 0, item.Height, item.Width, _font.Name, _font.Size, _font.Style, item.Align, item.Format));

                        if (lastx + width >= page.Width - margins.X)
                        {

                            if (maxHeight >= _font.GetHeight(gfx) && maxHeight >= item.Height) { calcHeight += maxHeight; }
                            else if (item.Height >= _font.GetHeight(gfx)) { calcHeight += item.Height; }
                            else { calcHeight += _font.GetHeight(gfx); }
                            lastLineWidth = 2;

                        }

                        if (width != page.Width - margins.X * 2)
                        {

                            if (width + lastLineWidth >= page.Width - margins.X * 2)
                            {

                                if (maxHeight >= _font.GetHeight(gfx) && maxHeight >= item.Height) { calcHeight += maxHeight; }
                                else if (item.Height >= _font.GetHeight(gfx)) { calcHeight += item.Height; }
                                else { calcHeight += _font.GetHeight(gfx); }
                                wordWidth = 0;
                                lineWidth = 0;
                                lastLineWidth = 2;


                            }
                            else
                            {

                                lastLineWidth += width + 1;

                            }

                        }

                        wholeWord = "";
                        wordWidth = 0;

                    }

                }

            }

            ph.CreatePage();
            if (!noQueueClear) { _wholeQueue.Clear(); }

        }

        public void CreateAndSendFile(HttpResponseBase Response, string FileName = "Dokument")
        {

            CheckAndCreatePages(); // !important

            if (document.PageCount > 0)
            {

                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", stream.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".pdf");
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                stream.Close();
                Response.End();

            }

        }

    }

}