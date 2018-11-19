using HTML2PDF.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HTML2PDF.Controllers.PdfHelpers
{

    /// <summary>
    /// Klasa zajmuje się generowaniem strony w pliku PDF.
    /// </summary>

    public class PdfHelper
    {

        PdfPage _page;
        XGraphics _gfx;
        XPoint _margins;

        List<IPdfElement> _queue = new List<IPdfElement>();

        public PdfHelper(XGraphics gfx, PdfPage page, double? marginX = null, double? marginY = null)
        {

            if (marginX != null) { _margins.X = (double)marginX; }
            else { _margins.X = 0; }

            if (marginY != null) { _margins.Y = (double)marginY; }
            else { _margins.Y = 0; }

            _page = page;
            _gfx = gfx;

        }

        public void Add(IPdfElement element) => _queue.Add(element);

        public void CreatePage(bool noQueueClear = false)
        {

            double maxHeight = 0, lastx = _margins.X;

            foreach (IPdfElement item in _queue)
            {

                item.CalculateSize(_gfx, lastx, _margins);

                if (item.IsFixed) { item.Render(_page, _gfx, null); }
                else
                {

                    int PreviousIndex = _queue.IndexOf(item) - 1;
                    if (PreviousIndex > -1)
                    {

                        IPdfElement prevItem = _queue.ElementAt(PreviousIndex);
                        lastx = prevItem.X + prevItem.Width + 1;

                        if (lastx + item.Width < _page.Width - _margins.X) // W lastx już mamy margines, więc liczymy dodatkowo tylko prawy.
                        {

                            item.Render(_page, _gfx, new XPoint(lastx, prevItem.Y));
                            if (item.Height + 1 > maxHeight) { maxHeight = item.Height + 1; }

                        }
                        else
                        {

                            lastx = _margins.X;
                            item.Render(_page, _gfx, new XPoint(lastx, prevItem.Y + maxHeight));
                            maxHeight = item.Height + 1;

                        }

                    }
                    else
                    {

                        item.Render(_page, _gfx, _margins);
                        maxHeight = item.Height + 1;

                    }

                }

            }

            if (!noQueueClear) { _queue.Clear(); }

        }

        public void SendFileToWeb(HttpResponseBase Response, PdfDocument document, string FileName)
        {

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