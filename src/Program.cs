using System;

// custom libs
using MangaSee123;
public class Program
{  // TODO: write cloner class when well rested
    public static void Main(string[] args)
    {
        string tunnel = "http://127.0.0.1:80";
        string authkey = "tb_is_johns_son";

        MangaSee123Client client = new MangaSee123Client(tunnel, authkey);
        string page = client.GetPage("Naruto", "1", "1");

        Console.WriteLine(page);
    }
}
