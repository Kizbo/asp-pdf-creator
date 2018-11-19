using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace HTML2PDF.Controllers.PdfHelpers
{

    /// <summary>
    /// Interfejs zapewnia komunikację między obiektami, w tym przekazywanie koordynatów.
    /// </summary>

    public interface IPdfElement
    {

        bool IsFixed { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        double Lastx { get; set; }

        // Kod dla PageHelper'a
        string Text { get; } 
        XFont FontStyle { get; }
        XParagraphAlignment Align { get; }
        XStringFormat Format { get; }

        void CalculateSize(XGraphics gfx, double lastx, XPoint margins);
        void Render(PdfPage page, XGraphics gfx, XPoint? coords);

    }

}