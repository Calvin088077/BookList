using myList.Models;

public class ConsoleUI
{
    private readonly IController<book> _bookController;

    public ConsoleUI(IController<book> bookController)
    {
        _bookController = bookController;
    }

    public async Task RunAsync()
    {
        bool closeTheConsole = false;

        do
        {
            var userInputChooseFunction = ShowMenuAndGetChoice();

            switch (userInputChooseFunction)
            {
                case "1":
                    await AddBookAsync();
                    break;
                case "2":
                    await ShowBooksAsync();
                    break;
                case "3":
                    await UpdateBookAsync();
                    break;
                case "4":
                    await DeleteBookAsync();
                    break;
                case "5":
                    closeTheConsole = true;
                    break;
                default:
                    Console.WriteLine("\n請輸入正確的選項！\n");
                    break;
            }
        } while (!closeTheConsole);
    }

    private string ShowMenuAndGetChoice()
    {
        Console.WriteLine("請輸入對應功能的數字:\n" +
                          "[1] 新增\n" +
                          "[2] 查看\n" +
                          "[3] 更新\n" +
                          "[4] 刪除\n" +
                          "[5] 關閉程式");
        return Console.ReadLine();
    }

    private async Task AddBookAsync()
    {
        Console.WriteLine("書名");
        var bookName = Console.ReadLine();

        bool boolIsRead = GetReadStatus();

        Console.WriteLine("備註");
        var bookNotes = Console.ReadLine();

        var newBook = new book { book_name = bookName, is_read = boolIsRead, notes = bookNotes };
        await _bookController.AddAsync(newBook);

        Console.WriteLine("\n新增成功!!\n");
    }

    private bool GetReadStatus()
    {
        while (true)
        {
            Console.WriteLine("[是][否]看完");
            var stringIsRead = Console.ReadLine();

            if (stringIsRead == "是") return true;
            if (stringIsRead == "否") return false;

            Console.WriteLine("\n無效輸入，請重新選擇！\n");
        }
    }

    private async Task ShowBooksAsync()
    {
        var books = await _bookController.GetAllAsync();

        if (books.Count == 0)
        {
            Console.WriteLine("\n目前沒有資料。\n");
        }
        else
        {
            foreach (var book in books)
            {
                Console.WriteLine($"\nID: {book.id}" +
                                  $"\n書名: {book.book_name}" +
                                  $"\n是否看完: {(book.is_read ? "是" : "否")}" +
                                  $"\n備註: {book.notes}");
            }
        }

        Console.WriteLine("\n查詢成功!!\n");
    }

    private async Task UpdateBookAsync()
    {
        int userId = GetUserId();

        var updateBook = await _bookController.GetByIdAsync(userId);
        if (updateBook == null)
        {
            Console.WriteLine("\n未找到該本書!!\n");
            return;
        }

        Console.WriteLine("新書名");
        string newBookName = Console.ReadLine();

        bool boolIsRead = GetReadStatus();

        Console.WriteLine("新備註");
        string newNotes = Console.ReadLine();

        updateBook.book_name = newBookName;
        updateBook.is_read = boolIsRead;
        updateBook.notes = newNotes;

        await _bookController.UpdateAsync(userId, updateBook);
        Console.WriteLine("\n書籍更新成功!!\n");
    }

    private async Task DeleteBookAsync()
    {
        int userId = GetUserId();

        var deleteBook = await _bookController.GetByIdAsync(userId);
        if (deleteBook == null)
        {
            Console.WriteLine("\n未找到該本書!!\n");
            return;
        }

        await _bookController.DeleteAsync(userId);
        Console.WriteLine("\n書籍刪除成功!!\n");
    }

    private int GetUserId()
    {
        int userId;
        while (true)
        {
            Console.WriteLine("請輸入ID");
            var userInputId = Console.ReadLine();

            if (int.TryParse(userInputId, out userId))
            {
                return userId;
            }
            else
            {
                Console.WriteLine("\n輸入的不是有效數字\n");
            }
        }
    }
}
