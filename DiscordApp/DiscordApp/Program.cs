using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using DiscordApp;

class Program
{    
    static void Main(string[] args)
    {
        string jsonString = File.ReadAllText("appsettings.json");
        JObject jsonObject = JObject.Parse(jsonString);
        string token = jsonObject["Token"].ToString();

        DiscordBot bot = new DiscordBot(token);
        bot.RunBotAsync().GetAwaiter().GetResult();
    }
}
