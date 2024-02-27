using Grpc.Core;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using Shared;
using Shared.Requests;
internal class Program
{
    private static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5004");


        //    var authenticationClient = new Authentication.AuthenticationClient(channel);
        var authenticationClient = channel.CreateGrpcService<IAuthenticationService>();

        var authenticationResponse =await authenticationClient.Authenticate(new AuthenticationRequest
        {
            UserName = "admin",
            Password = "admin"
        });


        if (authenticationResponse.AccessToken.Length >= 1)
        {
            Console.WriteLine("usuario logado com sucesso");
            var headers = new Metadata { { "Authorization", $"Bearer {authenticationResponse.AccessToken}" } };

            var client = channel.CreateGrpcService<IGreeterService>();
            var reply = await client.SayHello(new HelloRequest { Name = "lindinho da mamae" }, headers);
            Console.WriteLine(reply.Message);
        }
        else
        {
            Console.WriteLine("usuario nao autorizado");
            Console.ReadLine(); return;
        }
        await channel.ShutdownAsync();
        Console.WriteLine("GFDGs");
        Console.ReadLine();
    }




}