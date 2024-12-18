using System.Collections.Concurrent;

namespace Assessment5
{
    public class Assessment5
    {
        private static BlockingCollection<string> messageQueue = new BlockingCollection<string>(5);

        public static void RunPubSub()
        {
            var cts = new CancellationTokenSource();

            var publisherTask = Task.Run(() => SimulatePublisher(cts.Token));

            var subscriberTask1 = Task.Run(() => SimulateSubscriber("Subscriber 1", cts.Token));
            var subscriberTask2 = Task.Run(() => SimulateSubscriber("Subscriber 2", cts.Token));
            
            Task.WaitAll(publisherTask, subscriberTask1, subscriberTask2);

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] All messages processed.");
            Console.ReadLine();
        }

        private static void SimulatePublisher(CancellationToken ct)
        {
            var random = new Random();
            var messageCount = 0;

            while (!ct.IsCancellationRequested && messageCount < 10)
            {
                var message = $"Message {++messageCount}";
                messageQueue.Add(message, ct);
                Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Published: {message}");

                var delay = random.Next(100, 501);
                Thread.Sleep(delay);
            }

            messageQueue.CompleteAdding();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Publisher: No more messages.");
        }

        private static void SimulateSubscriber(string subscriberName, CancellationToken ct)
        {
            try
            {
                foreach (var message in messageQueue.GetConsumingEnumerable(ct))
                {
                    Console.WriteLine($"[{DateTime.Now.TimeOfDay}] {subscriberName} received: {message}");
                    //simulate delay
                    Thread.Sleep(1000);
                }

                Console.WriteLine($"[{DateTime.Now.TimeOfDay}] {subscriberName}: No more messages to process. Exiting.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"[{DateTime.Now.TimeOfDay}] {subscriberName}: Operation canceled. Exiting.");
            }
        }
    }
}
