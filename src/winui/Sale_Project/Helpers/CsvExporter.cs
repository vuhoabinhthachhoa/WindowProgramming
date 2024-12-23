using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using Sale_Project.Core.Models.Invoices;
using System.Collections.Generic;
using System.IO;

namespace Sale_Project.Helpers;
public class CsvExporter
{
    public static void ExportInvoicesToCsv(List<Invoice> invoices, string filePath)
    {
        if (invoices == null || invoices.Count == 0)
        {
            throw new ArgumentException("The invoices list is empty.", nameof(invoices));
        }

        try
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Invoices");

                // Add the header row
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Employee";
                worksheet.Cells[1, 3].Value = "CreatedDate";
                worksheet.Cells[1, 4].Value = "TotalAmount";
                worksheet.Cells[1, 5].Value = "RealAmount";

                // Add data rows
                for (int i = 0; i < invoices.Count; i++)
                {
                    var invoice = invoices[i];
                    worksheet.Cells[i + 2, 1].Value = invoice.Id;
                    worksheet.Cells[i + 2, 2].Value = invoice.Employee?.Name ?? ""; // Assuming Employee has a Name property
                    worksheet.Cells[i + 2, 3].Value = invoice.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    worksheet.Cells[i + 2, 4].Value = invoice.TotalAmount;
                    worksheet.Cells[i + 2, 5].Value = invoice.RealAmount;
                }

                // Save the Excel file
                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new InvalidOperationException("An error occurred while exporting invoices to CSV.", ex);
        }
    }
}
