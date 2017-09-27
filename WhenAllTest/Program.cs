using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WhenAllTest
{
    public class PartyStatus
    {
        public int InvitesSent { get; set; }
        public decimal FoodCost { get; set; }
        public bool IsHouseClean { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("I'm planning a party!");
            var partyStatus = new PartyStatus();

            var timer = Stopwatch.StartNew();

            partyStatus.InvitesSent = await SendInvites();
            partyStatus.FoodCost = await OrderFood();
            partyStatus.IsHouseClean = await CleanHouse();

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");

            Console.WriteLine("Now I'm planning one with helpers working in parallel!");
            timer = Stopwatch.StartNew();

            var sendInvites = SendInvites();
            var orderFood = OrderFood();
            var cleanHouse = CleanHouse();

            await Task.WhenAll(sendInvites, orderFood, cleanHouse);
            partyStatus.InvitesSent = await sendInvites;
            partyStatus.FoodCost = await orderFood;
            partyStatus.IsHouseClean = await cleanHouse;

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");

        }

        public static async Task<int> SendInvites()
        {
            await Task.Delay(2000);

            return 100;
        }
        public static async Task<decimal> OrderFood()
        {
            await Task.Delay(2000);

            return 123.23m;
        }
        public static async Task<bool> CleanHouse()
        {
            await Task.Delay(2000);

            return true;
        }
    }
}
