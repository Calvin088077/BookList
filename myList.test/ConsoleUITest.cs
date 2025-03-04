using Moq;
using myList.Models;
using NUnit.Framework;




namespace myList.test
{
    public class ConsoleUITests
    {
        private ConsoleUI _consoleUI;
        private Mock<IController<book>> _mockBookController;
        private StringReader _stringReader;
        private StringWriter _stringWriter;

        [SetUp]
        public void Setup()
        {
           _mockBookController = new Mock<IController<book>>();
           _consoleUI = new ConsoleUI(_mockBookController.Object);
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetIn(new StringReader(string.Empty));  // 清理 Console.SetIn
            _stringWriter.Dispose();  // 釋放 StringWriter
        }

        [Test]
        public async Task RunAsync_ShallShowCorrectResponse_WithStringOne()
        {
            _stringReader = new StringReader("1\n富爸爸窮爸爸\n是\n測試用\n5\n");
            Console.SetIn(_stringReader);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("書名", result);
            NUnit.Framework.Legacy.StringAssert.Contains("[是][否]看完", result);
            NUnit.Framework.Legacy.StringAssert.Contains("備註", result);
            NUnit.Framework.Legacy.StringAssert.Contains("新增成功!!", result);
        }

        [Test]
        public async Task RunAsync_ShallShowHaveInfoResponse_WithStringTwo()
        {
            _stringReader = new StringReader("2\n5\n");
            Console.SetIn(_stringReader);
            var books = new List<book>
            {
                new book { id = 1, book_name = "富爸爸窮爸爸", is_read = true, notes = "測試用" },
                new book { id = 2, book_name = "Hello World", is_read = false, notes = "" }
            };
            _mockBookController.Setup(mock => mock.GetAllAsync()).ReturnsAsync(books);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("ID: 1", result);
            NUnit.Framework.Legacy.StringAssert.Contains("書名: 富爸爸窮爸爸", result);
            NUnit.Framework.Legacy.StringAssert.Contains("是否看完: 是", result);
            NUnit.Framework.Legacy.StringAssert.Contains("備註: 測試用", result);
            NUnit.Framework.Legacy.StringAssert.Contains("ID: 2", result);
            NUnit.Framework.Legacy.StringAssert.Contains("書名: Hello World", result);
            NUnit.Framework.Legacy.StringAssert.Contains("是否看完: 否", result);
            NUnit.Framework.Legacy.StringAssert.Contains("備註: ", result);
            NUnit.Framework.Legacy.StringAssert.Contains("查詢成功!!", result);
        }

        [Test]
        public async Task RunAsync_ShallShowNoInfoResponse_WithStringTwo()
        {
            _stringReader = new StringReader("2\n5\n");
            Console.SetIn(_stringReader);
            var books = new List<book>
            {
            };
            _mockBookController.Setup(mock => mock.GetAllAsync()).ReturnsAsync(books);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("目前沒有資料。", result);
            NUnit.Framework.Legacy.StringAssert.Contains("查詢成功!!", result);
        }

        [Test]
        public async Task RunAsync_ShallShowRightIdResponse_WithStringThree()
        {
            _stringReader = new StringReader("3\n1\n哈利波特\n否\n奇幻小說\n5\n");
            Console.SetIn(_stringReader);
            var updateBook = new book { id = 1, book_name = "富爸爸窮爸爸", is_read = true, notes = "測試用" };
            _mockBookController.Setup(mock => mock.GetByIdAsync(1)).ReturnsAsync(updateBook);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("新書名", result);
            NUnit.Framework.Legacy.StringAssert.Contains("[是][否]看完", result);
            NUnit.Framework.Legacy.StringAssert.Contains("新備註", result);
            NUnit.Framework.Legacy.StringAssert.Contains("書籍更新成功!!", result);
        }

        [Test]
        public async Task RunAsync_ShallShowUnknowIdResponse_WithStringThree()
        {
            _stringReader = new StringReader("3\n2\n5\n");
            Console.SetIn(_stringReader);
            var updateBook = new book { id = 1, book_name = "富爸爸窮爸爸", is_read = true, notes = "測試用" };
            _mockBookController.Setup(mock => mock.GetByIdAsync(1)).ReturnsAsync(updateBook);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("未找到該本書!!", result);
           
        }

        [Test]
        public async Task RunAsync_ShallShowRightIdResponse_WithStringFour()
        {
            _stringReader = new StringReader("4\n1\n5\n");
            Console.SetIn(_stringReader);
            var deleteBook = new book { id = 1, book_name = "富爸爸窮爸爸", is_read = true, notes = "測試用" };
            _mockBookController.Setup(mock => mock.GetByIdAsync(1)).ReturnsAsync(deleteBook);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("書籍刪除成功!!", result);
        }

        [Test]
        public async Task RunAsync_ShallShowWrongIdResponse_WithStringFour()
        {
            _stringReader = new StringReader("4\n2\n5\n");
            Console.SetIn(_stringReader);
            var deleteBook = new book { id = 1, book_name = "富爸爸窮爸爸", is_read = true, notes = "測試用" };
            _mockBookController.Setup(mock => mock.GetByIdAsync(1)).ReturnsAsync(deleteBook);
            await _consoleUI.RunAsync();
            string result = _stringWriter.ToString();
            NUnit.Framework.Legacy.StringAssert.Contains("未找到該本書!!", result);
        }
    }
}
