using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

class Program
{
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;

    private static string _token;

    static void Main(string[] args)
    {
        string jsonString = File.ReadAllText("appsettings.json");
        JObject jsonObject = JObject.Parse(jsonString);
        _token = jsonObject["Token"].ToString();

        new Program().RunBotAsync().GetAwaiter().GetResult();
    }

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient();
        _commands = new CommandService();

        _client.Log += Log;

        await RegisterCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, _token);

        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public async Task RegisterCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (message.Author.IsBot) return;

        int argPos = 0;
        if (message.HasStringPrefix("!", ref argPos))
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
        }
    }
}
