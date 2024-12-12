

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using System.Reflection;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Layout.Element;
using Sale_Project.Core.Models.Invoices;
using System.Collections.Generic;
using System.IO;
using iText.Layout;
using Sale_Project.Contracts.Services;

public class PdfExporter
{
    private readonly IDialogService _dialogService;
    public PdfExporter(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public void ExportInvoiceToPdf(Invoice invoice, string filePath)
    {
        try
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a new page to the document
            PdfPage page = document.Pages.Add();

            // Add the invoice title
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 24);
            PdfTextElement title = new PdfTextElement("ClothingStoreManager", font);
            title.Draw(page, new PointF(10, 10));

            // Add the invoice details
            font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfTextElement invoiceId = new PdfTextElement($"Invoice ID: {invoice.Id}", font);
            invoiceId.Draw(page, new PointF(10, 50));

            PdfTextElement employee = new PdfTextElement($"Employee: {invoice.Employee.Name}", font);
            employee.Draw(page, new PointF(10, 70));

            PdfTextElement createdDate = new PdfTextElement($"Created Date: {invoice.CreatedDate.ToString("yyyy-MM-dd")}", font);
            createdDate.Draw(page, new PointF(10, 90));

            PdfTextElement totalAmount = new PdfTextElement($"Total Amount: {invoice.TotalAmount:C}", font);
            totalAmount.Draw(page, new PointF(10, 110));

            PdfTextElement realAmount = new PdfTextElement($"Real Amount: {invoice.RealAmount:C}", font);
            realAmount.Draw(page, new PointF(10, 130));

            // Add the invoice details table
            PdfGrid grid = new PdfGrid();
            grid.Columns.Add(6);

            // Add the header row
            PdfGridRow header = grid.Headers.Add(1)[0];
            header.Cells[0].Value = "Product";
            header.Cells[1].Value = "Size";
            header.Cells[2].Value = "Unit Price";
            header.Cells[3].Value = "Discount Percent (%)";
            header.Cells[4].Value = "Quantity";
            header.Cells[5].Value = "Total Price";

            // Center align header cells
            foreach (PdfGridCell cell in header.Cells)
            {
                cell.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            }

            // Add the data rows
            foreach (var detail in invoice.InvoiceDetails)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = detail.Product.Name;
                row.Cells[1].Value = detail.Product.Size;
                row.Cells[2].Value = detail.SellingPrice.ToString("C");
                row.Cells[3].Value = detail.DiscountPercent.ToString();
                row.Cells[4].Value = detail.Quantity.ToString();
                row.Cells[5].Value = (detail.SellingPrice * detail.Quantity * (1 - detail.DiscountPercent / 100)).ToString("C");

                // Center align data cells
                foreach (PdfGridCell cell in row.Cells)
                {
                    cell.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                }
            }

            grid.Draw(page, new PointF(10, 150));

            // Save the PDF document
            using (FileStream outputStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                document.Save(outputStream);
                document.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            _dialogService.ShowErrorAsync("Error", "An error occurred while exporting the invoice to PDF");
        }
    }
}


