using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace LibraryManagement
{
    class Book
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int SerialNo { get; set; }
        public bool IsSold { get; set; }

        public Book(string name, double price, int serialNo)
        {
            Name = name;
            Price = price;
            SerialNo = serialNo;
            IsSold = false;
        }
    }

    class Program
    {
        static void Main()
        {
            ArrayList bookLibrary = new ArrayList();
            ArrayList soldBooks = new ArrayList();

            while (true)
            {
                Console.WriteLine("\n Library Menu : ");
                Console.WriteLine("1. Add Books");
                Console.WriteLine("2. Display all Books");
                Console.WriteLine("3. Update Books");
                Console.WriteLine("4. Delete Books");
                Console.WriteLine("5. Calculate all book Price");
                Console.WriteLine("6. Sell Books");
                Console.WriteLine("7. Display Sold Books");
                Console.WriteLine("8. Exit");
                Console.WriteLine("Enter the Number to perform the Task: ");

                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        AddBook(bookLibrary);
                        break;
                    case 2:
                        DisplayBooks(bookLibrary);
                        break;
                    case 3:
                        UpdateBook(bookLibrary);
                        break;
                    case 4:
                        DeleteBook(bookLibrary);
                        break;
                    case 5:
                        CalculateTotalPrice(bookLibrary);
                        break;
                    case 6:
                        SellBooks(bookLibrary, soldBooks);
                        break;
                    case 7:
                        DisplaySoldBooks(soldBooks);
                        break;
                    case 8:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddBook(ArrayList bookLibrary)
        {
            string continueAdding = "yes";

            do
            {
                Console.Write("Enter Book Name: Enter name or Enter END ");
                string name = Console.ReadLine().ToLower();

                if (name.Equals("end"))
                {
                    continueAdding = "no";
                }
                else
                {
                    Console.Write("Enter Book Price: ");
                    double price = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter Book Serial No: ");
                    int serialNo = Convert.ToInt32(Console.ReadLine());

                    Book newBook = new Book(name, price, serialNo);
                    bookLibrary.Add(newBook);

                    Console.WriteLine("Book added successfully.");
                }

            }
            while (continueAdding == "yes");
        }

        static void DisplayBooks(ArrayList bookLibrary)
        {
            if (bookLibrary.Count == 0)
            {
                Console.WriteLine("No books in the library.");
                return;
            }

            Console.WriteLine("\nBooks in the Library:");
            foreach (Book book in bookLibrary)
            {
                Console.WriteLine($"Serial No: {book.SerialNo}, Name: {book.Name}, Price: {book.Price}");
            }
        }

        static void UpdateBook(ArrayList bookLibrary)
        {
            Console.Write("Enter Serial No of the book to update: ");
            int serialNo = Convert.ToInt32(Console.ReadLine());

            foreach (Book book in bookLibrary)
            {
                if (book.SerialNo == serialNo)
                {
                    bool updateValue = true;
                    while (updateValue)
                    {
                        Console.WriteLine("Choose the update value --> ");
                        Console.WriteLine("1. Update book Name ");
                        Console.WriteLine("2. Update book Price ");
                        Console.WriteLine("3. Update book SerialNo ");
                        Console.WriteLine("4. Update Complete Exit() ");
                        Console.WriteLine("\nWhich value do you want to update");

                        int option = Convert.ToInt32(Console.ReadLine());

                        switch (option)
                        {
                            case 1:
                                Console.Write("Enter New Book Name: ");
                                book.Name = Console.ReadLine();
                                Console.WriteLine("Book name updated successfully.");
                                break;
                            case 2:
                                Console.Write("Enter New Book Price: ");
                                book.Price = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("Book price updated successfully.");
                                break;
                            case 3:
                                Console.Write("Enter New Book SerialNo: ");
                                book.SerialNo = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Book SerialNo updated successfully.");
                                break;
                            case 4:
                                updateValue = false;
                                Console.WriteLine("Finished updating.");
                                break;
                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }
                    }
                    return; // Exit the method after updating the book
                }
            }
            Console.WriteLine("Book not found.");
        }

        static void DeleteBook(ArrayList bookLibrary)
        {
            Console.Write("Enter Serial No of the book to delete: ");
            int serialNo = Convert.ToInt32(Console.ReadLine());

            foreach (Book book in bookLibrary)
            {
                if (book.SerialNo == serialNo)
                {
                    bookLibrary.Remove(book);
                    Console.WriteLine("Book deleted successfully.");

                    return;
                }
            }
            Console.WriteLine("Book not found.");
        }

        static void CalculateTotalPrice(ArrayList bookLibrary)
        {
            double totalPrice = 0;

            foreach (Book book in bookLibrary)
            {
                totalPrice += book.Price;
            }

            Console.WriteLine($"Total Price of All Books: {totalPrice}");
        }

        static void SellBooks(ArrayList bookLibrary, ArrayList soldBooks)
        {
            ArrayList booksToSell = new ArrayList();
            double totalBill = 0;

            while (true)
            {
                Console.Write("Enter Serial No of the book to sell (or 'done' to finish): ");
                string input = Console.ReadLine();

                if (input.ToLower() == "done")
                    break;

                int serialNo = Convert.ToInt32(input);

                Book bookToSell = null;
                foreach (Book book in bookLibrary)
                {
                    if (book.SerialNo == serialNo && !book.IsSold)
                    {
                        bookToSell = book;
                        break;
                    }
                }

                if (bookToSell != null)
                {
                    bookToSell.IsSold = true;
                    booksToSell.Add(bookToSell);
                    soldBooks.Add(bookToSell);
                    totalBill += bookToSell.Price;
                    Console.WriteLine($"Added {bookToSell.Name} to the sale.");
                }
                else
                {
                    Console.WriteLine("Book not found or already sold.");
                }
            }

            if (booksToSell.Count > 0)
            {
                Console.WriteLine($"\nTotal Bill: ${totalBill}");
                GenerateBillPDF(booksToSell, totalBill);
            }
            else
            {
                Console.WriteLine("No books were sold.");
            }
        }

        static void DisplaySoldBooks(ArrayList soldBooks)
        {
            if (soldBooks.Count == 0)
            {
                Console.WriteLine("No books have been sold yet.");
                return;
            }

            Console.WriteLine("\nSold Books:");
            foreach (Book book in soldBooks)
            {
                Console.WriteLine($"Serial No: {book.SerialNo}, Name: {book.Name}, Price: {book.Price}");
            }
        }

        static void GenerateBillPDF(ArrayList soldBooks, double totalBill)
        {
            string fileName = $"Bill_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            Document document = new Document();

            try
            {
                string pt = "../../../" + fileName;
                PdfWriter.GetInstance(document, new FileStream(pt, FileMode.Create));
                document.Open();

                document.Add(new Paragraph("Book Sale Bill"));
                document.Add(new Paragraph($"Date: {DateTime.Now}"));
                document.Add(new Paragraph("--------------------------------------"));

                foreach (Book book in soldBooks)
                {
                    document.Add(new Paragraph($"Book: {book.Name}, Price: ${book.Price}"));
                }

                document.Add(new Paragraph("--------------------------------------"));
                document.Add(new Paragraph($"Total Bill: ${totalBill}"));

                Console.WriteLine($"Bill PDF generated: {fileName}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while generating the PDF: {e.Message}");
            }
            finally
            {
                document.Close();
            }
        }
    }
}