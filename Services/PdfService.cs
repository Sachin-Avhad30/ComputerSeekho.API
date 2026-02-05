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

                    page.Header().Element(content => ComposeHeader(content, data));
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

        void ComposeHeader(IContainer container, PaymentPdfDTO data)
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

                    column.Item().Text($"Receipt #{data.ReceiptId}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Darken1);

                    column.Item().Text(data.ReceiptDate.ToString("dd-MM-yyyy"))
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);
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
                        AddInfoRow(col, "Batch", data.BatchName);
                    });
                });

                column.Item().PaddingVertical(10);

                // Current Payment Section
                column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                    .PaddingBottom(5).Text("CURRENT PAYMENT")
                    .FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        AddInfoRow(col, "Payment Type", data.PaymentType);
                        AddInfoRow(col, "Payment Date", data.PaymentDate.ToString("dd-MM-yyyy"));
                        AddInfoRow(col, "Amount Paid", $"₹{data.ReceiptAmount:N2}");
                    });
                    row.RelativeItem().Column(col =>
                    {
                        if (!string.IsNullOrEmpty(data.TransactionReference))
                        {
                            AddInfoRow(col, "Transaction Ref", data.TransactionReference);
                        }
                    });
                });

                column.Item().PaddingVertical(10);

                // ✅ Payment Summary Section
                column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                    .PaddingBottom(5).Text("PAYMENT SUMMARY")
                    .FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Background(Colors.Grey.Lighten4)
                    .Padding(10)
                    .Column(col =>
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Total Course Fees:").FontSize(10);
                            r.RelativeItem().AlignRight().Text($"₹{data.TotalCourseFees:N2}").FontSize(10).SemiBold();
                        });
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Total Paid Till Now:").FontSize(10).FontColor(Colors.Green.Darken1);
                            r.RelativeItem().AlignRight().Text($"₹{data.TotalPaidTillNow:N2}").FontSize(10).SemiBold().FontColor(Colors.Green.Darken1);
                        });
                        col.Item().PaddingTop(5).BorderTop(1).BorderColor(Colors.Grey.Medium);
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Remaining Balance:").FontSize(11).Bold();
                            r.RelativeItem().AlignRight().Text($"₹{data.RemainingBalance:N2}").FontSize(11).Bold().FontColor(Colors.Red.Medium);
                        });
                    });

                column.Item().PaddingVertical(10);

                // ✅ Payment History Table
                if (data.AllPreviousPayments != null && data.AllPreviousPayments.Any())
                {
                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                        .PaddingBottom(5).Text("PAYMENT HISTORY")
                        .FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);  // #
                            columns.RelativeColumn(2);   // Date
                            columns.RelativeColumn(2);   // Payment Method
                            columns.RelativeColumn(1.5f);  // Amount
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Lighten3).Padding(5)
                                .Text("#").FontSize(10).SemiBold();
                            header.Cell().Background(Colors.Blue.Lighten3).Padding(5)
                                .Text("Date").FontSize(10).SemiBold();
                            header.Cell().Background(Colors.Blue.Lighten3).Padding(5)
                                .Text("Payment Method").FontSize(10).SemiBold();
                            header.Cell().Background(Colors.Blue.Lighten3).Padding(5)
                                .AlignRight().Text("Amount").FontSize(10).SemiBold();
                        });

                        // Rows
                        int index = 1;
                        foreach (var payment in data.AllPreviousPayments)
                        {
                            var bgColor = index % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;

                            table.Cell().Background(bgColor).Padding(5)
                                .Text(index.ToString()).FontSize(9);
                            table.Cell().Background(bgColor).Padding(5)
                                .Text(payment.PaymentDate.ToString("dd-MM-yyyy")).FontSize(9);
                            table.Cell().Background(bgColor).Padding(5)
                                .Text(payment.PaymentTypeDesc).FontSize(9);
                            table.Cell().Background(bgColor).Padding(5)
                                .AlignRight().Text($"₹{payment.PaymentAmount:N2}").FontSize(9).SemiBold()
                                .FontColor(Colors.Green.Darken1);

                            index++;
                        }

                        // Total Row
                        table.Cell().ColumnSpan(3).Background(Colors.Green.Lighten4).Padding(5)
                            .AlignRight().Text("TOTAL PAID:").FontSize(10).Bold();
                        table.Cell().Background(Colors.Green.Lighten4).Padding(5)
                            .AlignRight().Text($"₹{data.TotalPaidTillNow:N2}").FontSize(10).Bold()
                            .FontColor(Colors.Green.Darken2);
                    });
                }

                column.Item().PaddingVertical(10);

                // Current Payment Highlight
                column.Item().Background(Colors.Green.Lighten4)
                    .Padding(15)
                    .AlignCenter()
                    .Column(col =>
                    {
                        col.Item().Text("CURRENT PAYMENT AMOUNT")
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