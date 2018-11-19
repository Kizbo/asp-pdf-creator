using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HTML2PDF.Controllers.PdfHelpers;
using HTML2PDF.Controllers.PdfHelpers.Elements;
using HTML2PDF.Models;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace HTML2PDF.Controllers
{
    public class DokumentController : Controller
    {

        private HTML2PDFContext db = new HTML2PDFContext();

        // GET: Dokument
        public ActionResult Index()
        {
            return View(db.Dokuments.ToList());
        }

        // GET: Dokument/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Dokument dokument = db.Dokuments.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            return View(dokument);
        }


        // GET: Dokument/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dokument/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,Adres,Naglowek,Tresc")] Dokument dokument)
        {
            if (ModelState.IsValid)
            {
                db.Dokuments.Add(dokument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dokument);
        }

        // GET: Dokument/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Dokument dokument = db.Dokuments.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            return View(dokument);
        }

        // POST: Dokument/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,Adres,Naglowek,Tresc")] Dokument dokument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dokument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dokument);
        }

        // GET: Dokument/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dokument dokument = db.Dokuments.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            return View(dokument);
        }

        // POST: Dokument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dokument dokument = db.Dokuments.Find(id);
            db.Dokuments.Remove(dokument);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public ActionResult Generuj(int? id)
        {
            PageHelper ph = new PageHelper();

        
              ph.Add(new PdfHelpers.Elements.Paragraph("Data wystawienia wniosku: 0000-00-00", XParagraphAlignment.Right));
             ph.Add(new PdfHelpers.Elements.Paragraph(25));
             ph.Add(new PdfHelpers.Elements.Paragraph("Imię i Nazwisko Pracownika: Jan Kowalski"));
             ph.Add(new PdfHelpers.Elements.Paragraph("Stanowisko: Operator Mopa"));
             ph.Add(new PdfHelpers.Elements.Paragraph(50));
             ph.Add(new PdfHelpers.Elements.Paragraph("WNIOSEK URLOPOWY", "Verdana", 12, XFontStyle.Bold, XParagraphAlignment.Center, XStringFormats.Default));
             ph.Add(new PdfHelpers.Elements.Paragraph("Proszę o udzielenie {typ urlopu} w okresie od dnia {data-start} do dnia {data-end} włącznie tj. {ilosc-dni} dni roboczych za rok {rok}"));
             ph.Add(new PdfHelpers.Elements.Paragraph(25));
             ph.Add(new PdfHelpers.Elements.Paragraph("Zastępstwo w czasie mojej nieobecności będzie pełnić: {zastępca}"));
             ph.Add(new PdfHelpers.Elements.Paragraph(25));
             ph.Add(new PdfHelpers.Elements.Paragraph("Wniosek został zaakceptowany w systemie przez: {akceptator}", "Verdana", 8, XFontStyle.Regular));
             ph.Add(new PdfHelpers.Elements.Paragraph(75));
             ph.Add(new PdfHelpers.Elements.Paragraph(".........................................", 270));
             ph.Add(new PdfHelpers.Elements.Paragraph(".........................................", false, 0, 0, 0, 270, null, 12, XFontStyle.Regular, XParagraphAlignment.Right));
             ph.Add(new PdfHelpers.Elements.Paragraph("Podpis wnioskodawcy", 270, "Verdana", 10, XFontStyle.Regular, XParagraphAlignment.Left));
             ph.Add(new PdfHelpers.Elements.Paragraph("Podpis zatwierdzającego", 270, "Verdana", 10, XFontStyle.Regular, XParagraphAlignment.Right));
             ph.Add(new PdfHelpers.Elements.Paragraph(100));
             ph.Add(new PdfHelpers.Elements.Paragraph("Wniosek wygenerowany z systemu NewHR24", XParagraphAlignment.Right));

             



            ph.CreateAndSendFile(Response); // Wysyłanie pliku do przeglądarki
            
            // /Kod do generowania pliku PDF

            return RedirectToAction("Index");

        }

    }

}
