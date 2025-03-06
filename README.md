# BookList
這是一個基於ConsoleApp的書籍清單!

## 環境需求
只需安裝Docker即可

### 安裝步驟
1. git clone https://github.com/Calvin088077/BookList.git
2. cd BookList/myList
3. docker-compose run -it mylist sh (PS:等待至出現 #，代表進入交互模式)

### 使用步驟
1. dotnet ef migrations add InitialCreate --project /src/myList/myList.csproj (PS:在專案中建立Migrations 的目錄)
2. dotnet ef database update --project /src/myList/myList.csproj (PS:移轉資料庫結構)
3. dotnet myList.dll