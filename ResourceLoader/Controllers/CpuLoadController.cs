using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MathNet.Numerics.LinearAlgebra;
using ResourceLoader.Services;

namespace ResourceLoader.Controllers;

public class CpuLoadController : ControllerBase
{

    [HttpGet("load/cpu")]
    public IActionResult LoadCpu(
        [FromQuery] int threadsNumber,
        [FromQuery] int iterationsCount,
        [FromQuery] int restPeriod)
    {
        var threads = new List<Thread>();

        var timer = Stopwatch.StartNew();
        for (int i = 0; i < threadsNumber; i++)
        {
            var thread = new Thread(() =>
            {
                for (var j = 0; j < iterationsCount; j++)
                {
                    var matrixA = Matrix<double>.Build.Random(100, 100); 
                    var matrixB = Matrix<double>.Build.Random(100, 100); 

                    var resultMatrix = matrixA * matrixB;
                    Thread.Sleep(restPeriod);
                }
            });
            thread.Start();
            threads.Add(thread);
        }

        // Wait for all threads to complete
        foreach (var thread in threads)
        {
            thread.Join();
        }

        var elapsed = timer.Elapsed;
        Console.WriteLine($"Elapsed time {elapsed}");

        return Ok(elapsed.Milliseconds);
    }

    
    [HttpGet("load/cpu/percentage")]
    public IActionResult LoadCpu(
        [FromServices] LoadSimulatorService loadSimulator,
        [FromQuery] int percentage)
    {
        loadSimulator.SetLoad(percentage);

        return Ok();
    }
    
    [HttpGet("load/cpu/percentage/stop")]
    public IActionResult StopCpuLoad( [FromServices] LoadSimulatorService loadSimulator)
    {
        loadSimulator.ReleaseLoad();

        return Ok();
    }

    [HttpGet("load/memory")]
    public IActionResult LoadMemory(
        [FromQuery] int targetBytes,
        [FromQuery] int bytesStep,
        [FromQuery] int delayInMs)
    {
        var allocatedBytes = 0;
        var buffers = new List<byte[]>();
        var random = new Random();
        while (allocatedBytes <= targetBytes)
        {
            var stepBuffer = new byte[bytesStep];
            random.NextBytes(stepBuffer);
            buffers.Add(stepBuffer);
            allocatedBytes = buffers.Sum(buffer => buffer.Length);
            Console.WriteLine($"Allocated {(double)allocatedBytes/1000000} MB");
            Thread.Sleep(delayInMs);
        }

        return Ok();
    }
}