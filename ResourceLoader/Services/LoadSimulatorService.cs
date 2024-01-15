namespace ResourceLoader.Services;
using System.Diagnostics;

public class LoadSimulatorService
{
    private readonly Thread[] _cpuLoads = new Thread[Environment.ProcessorCount];
    private CancellationTokenSource _cancellationToken = new ();

    public void SetLoad(int percentage)
    {
        //cancel previous load if exists
        _cancellationToken.Cancel();
        
        _cancellationToken = new();
        for (int i = 0; i < _cpuLoads.Length; i++)
        {
            _cpuLoads[i] = new Thread(() =>
            {
                Stopwatch watch = new();
                watch.Start();
                while (!_cancellationToken.IsCancellationRequested)
                {
                    // Make the loop go on for "percentage" milliseconds then sleep the 
                    // remaining percentage milliseconds. So 40% utilization means work 40ms and sleep 60ms
                    if (watch.ElapsedMilliseconds > percentage)
                    {
                        Thread.Sleep(100 - percentage);
                        watch.Reset();
                        watch.Start();
                    }
                }
            });

            _cpuLoads[i].Start();
        }
    }

    public void ReleaseLoad()
    {
        _cancellationToken.Cancel();
    }
}


