using Microsoft.VisualBasic;
using ProjectWorks.KeyboardExtention;
using ProjectWorks.Models;
using ProjectWorks.Repository;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectWorks
{
    public class Worker(ILogger<Worker> logger,
        IAuthorshipRepository authorshipRepository,
        IBooksRepository booksRepository,
        IAuthorsRepository authorsRepository,
        IBooksReaderRepository booksreaderRepository,
        IReadersRepository readersRepository) : BackgroundService
    {
        private const string _botKey = "8048679564:AAElbMlfx4b-iY-yep1ptVkV9I2iLmL4pQg";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bot = new TelegramBotClient(_botKey);
            bot.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                },
                stoppingToken);
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await BotOnMessageReceived(botClient, update, token);
                        break;
                    case UpdateType.CallbackQuery:                        
                        break;
                }
                Task.CompletedTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            bool isIssue = true; 
            var message = update.Message;
            if (message?.Text != null)
            {
                Console.WriteLine($" Worker.cs line 87  ChatId = {message.Text}");
                var user = update.Message?.From;
                
                var currentUser = await CheckReader(user, token);
                if (currentUser is null)
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "��� ��� � ������ ������������� ����������");
                    return;
                }
                currentUser = await UpdateReaderModel(currentUser, user);
                switch (message.Text)
                {                    
                    case "/start":
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� �������", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                        break;
                    case "����� �����":
                        await IssueBook(botClient, update, currentUser,true);
                        break;
                    case "�������� �����":
                        await IssueBook(botClient, update, currentUser,false);
                        break;
                    case "������� �����":
                        isIssue = false;
                        await ListBooksReaderId( botClient, update, isIssue, currentUser);
                        break;
                    case "�������� ���� � ����":
                        isIssue = true;
                        await ListBooksReaderId( botClient, update, isIssue, currentUser);
                        break;
                    case "������ � �����":
                        var books = (await booksRepository.GetBooks()).OrderBy(b => b.Id);
                        var text = "";
                        foreach (var item in books)
                        {
                            text += $"{item.Id}. {item.Name}\n";
                        }
                        text += $"������� ����� ����� ����� ���� �������������(_)";
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id,text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                        break;
                    default:
                        if (message.Text.StartsWith('#'))
                            await TakeBook(botClient, update, currentUser);
                        else if (message.Text.StartsWith('$'))
                            await ReturnedBook( botClient, update,currentUser);
                        else if (message.Text.StartsWith('_'))
                            await InfoOfBook(botClient, update, currentUser);
                        else if (message.Text.StartsWith('&'))
                            await ExtendBook( botClient, update,currentUser);
                        else
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� ������! ��������� ����", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                        }
                        break;
                }
            }
            else if (message?.Type is MessageType.UserShared)
            {
                var userId = message.UserShared.UserId;
                await AddReader(readersRepository,  userId);
                return;
            }
        }
        private async Task InfoOfBook(ITelegramBotClient botClient, Update update, ReadersModel currentUser)
        {
            try
            {
                int id = Convert.ToInt16(update.Message.Text.Substring(1));
                var book = await booksRepository.GetBookDetailed(id);
                var text = book switch
                {
                    null => "����� � ����� ������� ��� � ����������!!\n ��������� ����",
                    _ => book.OutputRecordingBooks()
                };
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }
            catch (Exception ex)
            { 
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� ������! ��������� ����", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }
        }
        private async Task ReturnedBook(ITelegramBotClient botClient, Update update, ReadersModel currentUser)
        {
            try
            {
                int id = Convert.ToInt16(update.Message.Text.Substring(1));

                var book = await booksRepository.GetBook(id);

                var returnBook = await readersRepository.GetReaderById(currentUser.Id);

                var text = "";
                if (book == null)
                {
                    text = "����� � ����� ������� ��� � ����������!!\n ��������� ����";
                    if (currentUser is not null)
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                }
                else if (book.Status == false)
                {
                    text = "��������� ����� ������ �� ������! \n ������� ����� ����� ����� ���� $\n ������� � ��� �� �����!";
                    if (currentUser is not null)
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                }
                else
                {
                    var booksreaders = await booksreaderRepository.GetBooksReaderByFilter(returnBook.Id);
                    var entity = booksreaders.Where(b => b.BookId.Equals(id)).FirstOrDefault();
                    if (entity == null)
                    {
                        text = "����� � ����� ������� ��� �� ������!!\n ��������� ����";
                        if (currentUser is not null)
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                    }
                    else
                    {
                        //��� �������� ����� � ������� �������� � ������ ������ ������ ����� �� false � ������ ����������� ���� ��������
                        var updateEntity = new BooksReaderModel()
                        {
                            ReaderId = entity.ReaderId,
                            BookId = id,
                            IsReadered = false,
                            DateOfIssue = entity.DateOfIssue,
                            ReturnDate = DateTime.Now,
                            Id = entity.Id
                        };
                        await booksreaderRepository.UpdateBooksReader(updateEntity);
                        //� ������� ����� ������ ������ �� false
                        var updateBook = new BooksModel()
                        {
                            Id = book.Id,
                            Name = book.Name,
                            Status = false,
                        };
                        await booksRepository.UpdateBook(updateBook);
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� �������", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                    }
                }
            }
            catch
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� ������! ��������� ����", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }
        }
        private  async Task AddReader(IReadersRepository readersRepository, long userId)
        {
            var reader = new ReadersModel()
            {
                Id = userId,
                Name = string.Empty,
                IsAdmin = false,
                IsUsed = false
            };
            await readersRepository.InsertReader(reader);
        }
        private async Task ListBooksReaderId(ITelegramBotClient botClient, Update update, bool isIssue, ReadersModel currentUser)
        {
            var booksreaders = await booksreaderRepository.GetBooksReaderByFilter(currentUser.Id);

            var text = "";

            foreach (var item in booksreaders)
            {
                var book = await booksRepository.GetBook(item.BookId);
                text += $"{book.Id}. {book.Name}\n";
            }

            if (isIssue)
            {
                text += $"�������� ���� �� ������";
            }
            else
            {
                text += $"�������� ����� ���� $ ����� ������������ �����";
            }
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
        }
        private  async Task TakeBook(ITelegramBotClient botClient, Update update, ReadersModel currentUser)
        {
            int id;
            try
            {
                var text = string.Empty;
                id = Convert.ToInt16(update.Message.Text.Substring(1));
                var book = await booksRepository.GetBook(id);

                if (book == null)
                {
                    text = "����� � ����� ������� ��� � ����������!!\n ��������� ����";
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                }
                else if (book.Status == true)
                {
                    text = "��������� ����� ������!\n �������� ������!";
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                }
                else
                {
                    Console.WriteLine(id);
                    var booksreader = new BooksReaderModel()
                    {
                        ReaderId = currentUser.Id,
                        BookId = id,
                        IsReadered = true,
                        DateOfIssue = DateTime.Now,
                        ReturnDate = DateTime.Now.AddDays(5)
                    };

                    await booksreaderRepository.InsertBooksReader(booksreader);

                    var currentBook = await booksRepository.GetBook(id);

                    var newBook = new BooksModel();
                    {
                        newBook.Id = currentBook.Id;
                        newBook.Status = true;
                        newBook.Name = currentBook.Name;
                    }
                    await booksRepository.UpdateBook(newBook);

                    text = "�����  " + newBook.Name + " ��� ������.\n ���� �������� " + booksreader.ReturnDate.Date.ToShortDateString() + "\n�������� �������!";
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
                }
            }
            catch (FormatException exceptionFormat)
            { 
                 await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� ������! ��������� ����", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }          
        }
        private async Task ExtendBook(ITelegramBotClient botClient, Update update, ReadersModel currentUser)
        {
            int id;
            try
            {
                var text = string.Empty;
                id = Convert.ToInt16(update.Message.Text.Substring(1));
                var book = await booksRepository.GetBook(id);

                if (book == null)
                {
                    text = "����� � ����� ������� ��� � ����������!!\n ��������� ����";
                }
                else if (book.Status == true)
                {
                    var booksreader = await booksreaderRepository.GetBooksReaderByFilter(currentUser.Id);
                    var bookRelation = booksreader.Where(x => x.BookId.Equals(id)).FirstOrDefault();
                    if (bookRelation is not null)
                    {
                        var updatebookRelation = new BooksReaderModel()
                        {
                            Id = bookRelation.Id,
                            BookId = bookRelation.BookId,
                            ReaderId = bookRelation.ReaderId,
                            IsReadered = bookRelation.IsReadered,
                            DateOfIssue = bookRelation.DateOfIssue,
                            ReturnDate = bookRelation.ReturnDate.AddDays(5)
                        };
                        await booksreaderRepository.UpdateBooksReader(updatebookRelation);
                        text = "���� �������� ����� " + book.Name + "\n�������� ��: " + updatebookRelation.ReturnDate.Date.ToShortDateString() + "\n�������� �������!";
                    }
                }
                else
                {
                    text = "����� " + book.Name + "\n��� �" + book.Id + "�� � ��� �� �����\n" + "������� � ����� ����� ��������� (&)";
                }
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }
            catch (FormatException exceptionFormat)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "�������� ������! ��������� ����", replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
            }
        }
        async Task IssueBook(ITelegramBotClient botClient, Update update, ReadersModel currentUser, bool isPr)
        {
            var text = "������ ���� ��� ������\n\n";

            if (isPr == true)
            {
                var books = await booksRepository.GetBooks();

                foreach (var book in books)
                {
                    if (book.Status != true) { text += $"{book.Id}. {book.Name}\n"; }
                }
                text += $"��� ������ ����� �������� ����� ������� (#) ����� ����� �� ������";
            }
            else
            {
                var booksreaders = await booksreaderRepository.GetBooksReaderByFilter(currentUser.Id);
                foreach (var item in booksreaders)
                {
                    var book = await booksRepository.GetBook(item.BookId);
                    text += $"{book.Id}. {book.Name}\n";
                }
                text += $"��� ��������� ����� �������� ����� ��������� (&) ����� ����� �� ������";
            }
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyBoardsExample.GetKeyboard(currentUser));
        }
        async Task<ReadersModel?> CheckReader(User user, CancellationToken token)
        {
            var currentUser = await readersRepository.GetReaderById(user.Id);
            if (currentUser is null)
            {
                var users = await readersRepository.GetReaders();
                //�������� ������� � ������� �������� ���� �� ������ ������������
                if (users.Count == 0)
                {
                    //������������� ���, ��������� ������ � ������� � ��������� Admin
                    currentUser = new ReadersModel()
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        IsAdmin = true,
                        IsUsed = true
                    };
                    // ���������� ������ � ������� readers ��
                    await readersRepository.InsertReader(currentUser);
                }
            }
            return currentUser;
        }
        async Task<ReadersModel> UpdateReaderModel( ReadersModel currentUser, User user )
        {
            if (currentUser.IsAdmin == false && currentUser.IsUsed == false)
            {
                var updateReader = new ReadersModel()
                {
                    Id = currentUser.Id,
                    IsAdmin = false,
                    IsUsed = true,
                    Name = user.FirstName + " " + user.LastName
                };
                await readersRepository.UpdateReader(updateReader);
                return updateReader;
            }
            else
                return currentUser;
        }
    }
}
