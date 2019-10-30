using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStream
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Demo1Async();


            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static async Task Demo1Async()
        {
            var sensorDevice = new SensorDevice();

            await foreach (var x in sensorDevice.GetSensorData1())
            {
                Console.WriteLine($"{x.Value1} {x.Value2}");
            }
        }

        private static async Task Demo2Async()
        {
            var sensorDevice = new SensorDevice();

            IAsyncEnumerable<SensorData> en = sensorDevice.GetSensorData1();
            IAsyncEnumerator<SensorData> enumerator = en.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var sensorData = enumerator.Current;
                Console.WriteLine($"{sensorData.Value1} {sensorData.Value2}");
            }
            await enumerator.DisposeAsync();
        }

        private static async Task Demo3Async()
        {
            try
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(5000);
                var sensorDevice = new SensorDevice();

                await foreach (var x in sensorDevice.GetSensorData2(cts.Token))
                {
                    Console.WriteLine($"{x.Value1} {x.Value2}");
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
