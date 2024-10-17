namespace SimpleTestClient;

internal static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Testing real time communication...");
        await HttpClientTesting.StartClient();
        //await SignalRClientTesting.StartClient();
    }
}