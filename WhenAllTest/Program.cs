using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WhenAllTest
{
    class Program
    {
        static bool ThrowExceptions = false;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Not required in C# 7.1.
        /// See: https://ardalis.com/better-performance-from-async-operations
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task MainAsync(string[] args)
        {
            await PlanPartySequentially(); // ~6000ms
            await PlanPartyConcurrently(); // ~2000ms
            await PlanPartyConcurrentlyFewerAwaits(); // ~2000ms
            await PlanPartyConcurrentlyWithoutWhenAll(); // ~2000ms


            //// add error handling
            //try
            //{
            //    await PlanPartySequentially(); // ~6000ms
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            //try
            //{
            //    await PlanPartyConcurrently(); // ~2000ms
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            //try
            //{
            //    await PlanPartyConcurrentlyFewerAwaits(); // ~2000ms
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            //try
            //{
            //    await PlanPartyConcurrentlyWithoutWhenAll(); // ~2000ms
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}



        }

        private static async Task PlanPartyConcurrentlyWithoutWhenAll()
        {
            Console.WriteLine("Now I'm planning one with helpers working concurrently (no WhenAll)!");
            var partyStatus = new PartyStatus();
            var timer = Stopwatch.StartNew();

            // kicks off all 3 tasks
            var sendInvites = SendInvites();
            var orderFood = OrderFood();
            var cleanHouse = CleanHouse();

            // waits for all 3 to complete
            partyStatus.InvitesSent = await sendInvites;
            partyStatus.FoodCost = await orderFood;
            partyStatus.IsHouseClean = await cleanHouse;

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        private static async Task PlanPartyConcurrentlyFewerAwaits()
        {
            Console.WriteLine("Now I'm planning one with helpers working concurrently (again)!");
            var partyStatus = new PartyStatus();
            var timer = Stopwatch.StartNew();

            var sendInvites = SendInvites();
            var orderFood = OrderFood();
            var cleanHouse = CleanHouse();

            await Task.WhenAll(sendInvites, orderFood, cleanHouse);
            partyStatus.InvitesSent = sendInvites.Result;
            partyStatus.FoodCost = orderFood.Result;
            partyStatus.IsHouseClean = cleanHouse.Result;

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        private static async Task PlanPartyConcurrently()
        {
            Console.WriteLine("Now I'm planning one with helpers working concurrently!");
            var partyStatus = new PartyStatus();

            var timer = Stopwatch.StartNew();

            var sendInvites = SendInvites();
            var orderFood = OrderFood();
            var cleanHouse = CleanHouse();

            await Task.WhenAll(sendInvites, orderFood, cleanHouse);
            partyStatus.InvitesSent = await sendInvites;
            partyStatus.FoodCost = await orderFood;
            partyStatus.IsHouseClean = await cleanHouse;

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        private static async Task PlanPartySequentially()
        {
            Console.WriteLine("I'm planning a party!");
            var partyStatus = new PartyStatus();

            var timer = Stopwatch.StartNew();

            partyStatus.InvitesSent = await SendInvites();
            partyStatus.FoodCost = await OrderFood();
            partyStatus.IsHouseClean = await CleanHouse();

            Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}ms");
        }

        public static async Task<int> SendInvites()
        {
            await Task.Delay(2000);
            if (Program.ThrowExceptions) throw new Exception("Boom. Nobody is coming.");

            return 100;
        }
        public static async Task<decimal> OrderFood()
        {
            await Task.Delay(2000);

            if(Program.ThrowExceptions) throw new Exception("Boom. No food for you.");

            return 123.23m;
        }
        public static async Task<bool> CleanHouse()
        {
            await Task.Delay(2000);
            if (Program.ThrowExceptions) throw new Exception("Boom. You have small children. Forget having a clean house.");

            return true;
        }
    }
}
