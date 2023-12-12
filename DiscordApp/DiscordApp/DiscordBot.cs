using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel.Design;

namespace DiscordApp;

public class DiscordBot
{
    DiscordSocketClient _client; //봇 클라이언트
    CommandService _commands;    //명령어 수신 클라이언트
    private readonly string _token;
    public DiscordBot(string token)
    {
        _token = token;
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        // It is recommended to Dispose of a client when you are finished
        // using it, at the end of your app's lifetime.
        _client = new DiscordSocketClient(config);       
    }

    /// <summary>
    /// 봇의 진입점, 봇의 거의 모든 작업이 비동기로 작동되기 때문에 비동기 함수로 생성해야 함
    /// </summary>
    /// <returns></returns>
    public async Task BotMain()
    {
        _commands = new CommandService(new CommandServiceConfig()        //명령어 수신 클라이언트 초기화
        {
            LogLevel = LogSeverity.Verbose                              //봇의 로그 레벨 설정
        });

        //로그 수신 시 로그 출력 함수에서 출력되도록 설정
        _client.Log += OnClientLogReceived;
        _client.MessageReceived += MessageReceivedAsync;         //봇이 메시지를 수신할 때 처리하도록 설정
        //_client.Ready += ReadyAsync;
        //_client.InteractionCreated += InteractionCreatedAsync;
        _commands.Log += OnClientLogReceived;

        await _client.LoginAsync(TokenType.Bot, _token); //봇의 토큰을 사용해 서버에 로그인
        await _client.StartAsync();                         //봇이 이벤트를 수신하기 시작


        await Task.Delay(-1);   //봇이 종료되지 않도록 블로킹
    }

    private async Task MessageReceivedAsync(SocketMessage arg)
    {
        //수신한 메시지가 사용자가 보낸 게 아닐 때 취소
        var message = arg as SocketUserMessage;
        if (message == null || message.Author.IsBot) return;

        Console.WriteLine($"Received message: {arg.Content}"); // 수신된 메시지의 원문 출력

        var context = new SocketCommandContext(_client, message);                    //수신된 메시지에 대한 컨텍스트 생성   

        await context.Channel.SendMessageAsync("명령어 수신됨 - " + message.Content); //수신된 명령어를 다시 보낸다.
    }


    /// <summary>
    /// 봇의 로그를 출력하는 함수
    /// </summary>
    /// <param name="msg">봇의 클라이언트에서 수신된 로그</param>
    /// <returns></returns>
    private Task OnClientLogReceived(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());  //로그 출력
        return Task.CompletedTask;
    }
}
