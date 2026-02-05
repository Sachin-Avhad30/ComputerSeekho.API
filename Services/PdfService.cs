using ComputerSeekho.API.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ComputerSeekho.API.Services
{
    public class PdfService
    {
        public byte[] GenerateReceiptPdf(PaymentPdfDTO data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(content => ComposeContent(content, data));
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Generated on: ");
                        text.Span(DateTime.Now.ToString("dd-MM-yyyy HH:mm")).SemiBold();
                    });
                });
            });

            return document.GeneratePdf();
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("PAYMENT RECEIPT")
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(Colors.Blue.Medium);

                    column.Item().Text("Computer Seekho")
                        .FontSize(14)
                        .SemiBold();

                    column.Item().Text("Institute Management System")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);
                });

                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item().Text("PAID")
                        .FontSize(16)
                        .Bold()
                        .FontColor(Colors.Green.Medium);
                });
            });
        }

        void ComposeContent(IContainer container, PaymentPdfDTO data)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(10);

                // Student Information Section
                column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                    .PaddingBottom(5).Text("STUDENT INFORMATION")
                    .FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        AddInfoRow(col, "Student Name", data.StudentName);
                        AddInfoRow(col, "Mobile", data.StudentMobile);
                        AddInfoRow(col, "Email", data.StudentEmail);
                    });
                    row.RelativeItem().Column(col =>
                    {
                        AddInfoRow(col, "Address", data.StudentAddress);
                        AddInfoRow(col, "Course", data.CourseName);
                    });
                });

                column.Item().PaddingVertical(10);

                // Payment Information Section
                column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                    .PaddingBottom(5).Text("PAYMENT INFORMATION")
                    .FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        AddInfoRow(col, "Payment Type", data.PaymentType);
                        AddInfoRow(col, "Payment Date", data.PaymentDate.ToString("dd-MM-yyyy"));
                        AddInfoRow(col, "Amount", $"₹{data.Amount:N2}");
                    });
                    row.RelativeItem().Column(col =>
                    {
                        AddInfoRow(col, "Receipt Amount", $"₹{data.ReceiptAmount:N2}");
                        AddInfoRow(col, "Receipt Date", data.ReceiptDate.ToString("dd-MM-yyyy"));
                    });
                });

                column.Item().PaddingVertical(10);

                // Amount Highlight
                column.Item().Background(Colors.Green.Lighten4)
                    .Padding(15)
                    .AlignCenter()
                    .Column(col =>
                    {
                        col.Item().Text("AMOUNT PAID")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                        col.Item().Text($"₹{data.ReceiptAmount:N2}")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Green.Darken2);
                    });

                column.Item().PaddingVertical(10);

                // Thank You Note
                column.Item().PaddingTop(20)
                    .BorderTop(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .PaddingTop(10)
                    .AlignCenter()
                    .Text("Thank you for your payment!")
                    .FontSize(10)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);

                column.Item().AlignCenter()
                    .Text("This is a computer-generated receipt.")
                    .FontSize(8)
                    .FontColor(Colors.Grey.Medium);
            });
        }

        void AddInfoRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().Row(row =>
            {
                row.RelativeItem(2)
                    .Text(label + ":")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);

                row.RelativeItem(3)
                    .Text(value ?? "N/A")
                    .FontSize(10)
                    .SemiBold();
            });
        }
    }
}