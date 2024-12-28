using ProjectWorks.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectWorks.KeyboardExtention
{
    public static class KeyBoardsExample
    {
        internal static ReplyKeyboardMarkup GetKeyboard(ReadersModel currentUser)=> currentUser.IsAdmin switch
        {
            true => GetKeyboardAdmin(),
            false => GetKeyboardUser()
        };
        internal static ReplyKeyboardMarkup GetKeyboardAdmin() =>
            new ReplyKeyboardMarkup(new[]
            { 
                new[] 
                {
                    KeyboardButton.WithRequestUser("Выбрать контакт",new KeyboardButtonRequestUser(){UserIsBot = false }),
                    new KeyboardButton("Перечень книг у меня"), 
                    new KeyboardButton("Детали о книге")
                },
                [
                    new KeyboardButton("Взять книгу"),
                    new KeyboardButton("Вернуть книгу"),
                    new KeyboardButton("Продлить книгу")
                ]
            }){ ResizeKeyboard = true };
        internal static ReplyKeyboardMarkup GetKeyboardUser() =>
            new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Перечень книг у меня"),
                    new KeyboardButton("Взять книгу")
                },
                [
                    new KeyboardButton("Вернуть книгу"),
                    new KeyboardButton("Продлить книгу"),
                    new KeyboardButton("Детали о книге")
                ]
            }){ ResizeKeyboard = true };
    }
}
