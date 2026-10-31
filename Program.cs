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
                new KeyboardButton[] { "привет", "помощь" },
                new KeyboardButton[] { "факт о кошке" }
            })
            { ResizeKeyboard = true };
            await bot.SendMessage(msg.Chat, "здравствуйте! выберите действие на клавиатуре ниже.", replyMarkup: keyboard);
            break;
        case "/help":
        case "помощь":
            await bot.SendMessage(msg.Chat, "команды:\n/start\n/help\n/cat");
            break;
        case "/cat":
        case "факт о кошке":
            var facts = new[]
            {
                "кошки спят около 70% своей жизни.",
                "у кошек потеют только подушечки лап.",
                "кошки не чувствуют сладкого вкуса.",
                "мурлыканье кошки может снижать стресс у человека.",
                "у кошек 32 мышцы в каждом ухе.",
                "кошки видят в темноте в 6 раз лучше человека."
            };
            var random = new Random();
            var fact = facts[random.Next(facts.Length)];
            await bot.SendMessage(msg.Chat, $"🐱 {fact}");
            break;
        case "привет":
            await bot.SendMessage(msg.Chat, $"привет, {msg.From?.FirstName}! 👋");
            break;
        default:
            await bot.SendMessage(msg.Chat, $"ты написал: {msg.Text}");
            break;
    }
}

async Task OnError(Exception e, HandleErrorSource s) => Console.WriteLine($"ошибка: {e.Message}");