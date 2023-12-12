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
    /// <summary>
    /// 프로그램의 진입점
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        string jsonString = File.ReadAllText("appsettings.json");
        JObject jsonObject = JObject.Parse(jsonString);
        string token = jsonObject["Token"].ToString();

        DiscordBot bot = new DiscordBot(token);
        bot.BotMain().GetAwaiter().GetResult();   //봇의 진입점 실행
    }    
}
