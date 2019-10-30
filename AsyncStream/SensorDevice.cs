using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStream
{
    public class SensorDevice
    {
        public async IAsyncEnumerable<SensorData> GetSensorData1()
        {
            var r = new Random();
            while (true)
            {
                await Task.Delay(r.Next(300));
                yield return new SensorData(r.Next(100), r.Next(100));
            }
        }

        public async IAsyncEnumerable<SensorData> GetSensorData2(CancellationToken cancellationToken = default)
        {
            bool cancel = false;
            using var registration = cancellationToken.Register(() => cancel = true);
            var r = new Random();
            while (!cancel)
            {
                await Task.Delay(r.Next(500));
                yield return new SensorData(r.Next(100), r.Next(100));
            }
            Console.WriteLine("cancel requested");
            throw new OperationCanceledException(cancellationToken);
        }
    }
}
