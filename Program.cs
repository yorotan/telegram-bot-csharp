using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;

var options = new TelegramBotClientOptions(
    "9183954057:AAHPVvavNZFASsD22aTwlCr0MmaK0g34AP4",
    "https://floral-lake-09d5.egorylit.workers.dev"
);

var bot = new TelegramBotClient(options);
var me = await bot.GetMe();
Console.WriteLine($"бот @{me.Username} запущен.");

bot.OnMessage += OnMessage;
bot.OnError += OnError;
Console.ReadLine();

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;
    switch (msg.Text)
    {
        case "/start":
            var keyboard = new ReplyKeyboardMarkup(new[] {
                new KeyboardButton[] { "котик дня", "предсказание" },
                new KeyboardButton[] { "помощь" }
            })
            { ResizeKeyboard = true };
            await bot.SendMessage(msg.Chat, "привет! я бот-предсказатель. выбери, что хочешь:", replyMarkup: keyboard);
            break;
        case "/help":
        case "помощь":
            await bot.SendMessage(msg.Chat, "команды:\n/start\n/help\n/cat — котик дня\n/prediction — предсказание");
            break;
        case "/cat":
        case "котик дня":
            try
            {
                using var http = new HttpClient();
                var imageBytes = await http.GetByteArrayAsync($"https://cataas.com/cat?{Guid.NewGuid()}");
                using var stream = new MemoryStream(imageBytes);
                await bot.SendPhoto(msg.Chat, new InputFileStream(stream, "cat.jpg"), caption: "твой котик дня 🐱");
            }
            catch
            {
                await bot.SendMessage(msg.Chat, "котик убежал, попробуй ещё раз 🐾");
            }
            break;
        case "/prediction":
        case "предсказание":
            var predictions = new[]
            {
                "сегодня тебя ждёт неожиданная радость 🌟",
                "хороший день для новых начинаний 🚀",
                "будь внимателен к мелочам — они важны 💫",
                "улыбнись, и мир улыбнётся тебе 😊",
                "время отпустить прошлое и двигаться вперёд 🌿",
                "скоро ты получишь приятную новость 📩",
                "доверься интуиции — она не подведёт 🔮"
            };
            await bot.SendMessage(msg.Chat, $"🔮 {predictions[new Random().Next(predictions.Length)]}");
            break;
        default:
            await bot.SendMessage(msg.Chat, "я не понял. напиши /help.");
            break;
    }
}

async Task OnError(Exception e, HandleErrorSource s) => Console.WriteLine($"ошибка: {e.Message}");
