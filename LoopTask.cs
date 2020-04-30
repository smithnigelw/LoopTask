using System;
using System.Runtime.InteropServices;
using System.Threading;  // For 'CancellationToken'
using System.Threading.Tasks;  // For 'Tasks'
using System.Collections.Generic;  // For 'List'
using System.Linq;
using System.Diagnostics;  // For 'Stopwatch'

public class Program
{
  #region unmanaged
  [DllImport("Kernel32")]
  private static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

  //delegate type to be used of the handler routine
  private delegate bool HandlerRoutine(CtrlTypes CtrlType);

  // control messages
  private enum CtrlTypes
  {
    CTRL_C_EVENT = 0,
    CTRL_BREAK_EVENT,
    CTRL_CLOSE_EVENT,
    CTRL_LOGOFF_EVENT = 5,
    CTRL_SHUTDOWN_EVENT
  }
  #endregion

  private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
  {
    Log("::: Exit Event: " + Enum.GetName(typeof(CtrlTypes), ctrlType));
    bRunLoop = false;
    //tokenSource.Cancel();
    return true;
  }

  private void Log(String sText)
  {
    Thread curThread = Thread.CurrentThread;
    Int32 nThread = curThread.ManagedThreadId;
    String sThreadInfo = $"{nThread}";
    sThreadInfo += (curThread.IsBackground ? " bg" : null);
    sThreadInfo += (curThread.IsThreadPoolThread ? " tp" : null);
    String sNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    String sMsg = $"[{sNow}][{sThreadInfo}] {sText}".Trim();
    Console.WriteLine(sMsg);
  }
  private void ListAllTasks()
  {
    foreach (Task t in tasks)
    {
      Console.WriteLine($"Task {t.Id} - {t.Status}");
    }
  }

  private void DoMyTask(Int32 nJ)
  {
    Log($"Enter - {nJ}");
    Stopwatch sw = Stopwatch.StartNew();
    Task.Delay(2500).Wait();
    sw.Stop();
    Log($">Exit - {nJ} after {sw.ElapsedMilliseconds}mS");
  }

  private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
  private readonly List<Task> tasks = new List<Task>();
  private Boolean bRunLoop;

  private async Task LoopTasks(Int32 interval, CancellationToken cancellationToken)
  {
    Int32 nJ = 1;
    bRunLoop = true;
    while (bRunLoop)
    {
      try
      {
        //await Task.Factory.StartNew(() => Log(String.Empty), TaskCreationOptions.LongRunning);
        Task myJob = Task.Run(() => DoMyTask(nJ));
        tasks.Add(myJob);
        await Task.Delay(interval, cancellationToken);
        nJ++;
      }
      catch (OperationCanceledException ex)
      {
        Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {ex.Message}");
      }
    }
    Log("Loop has exited");
    ListAllTasks();
    Task.WaitAll(tasks.Where(t => !t.IsCompleted).ToArray());
    ListAllTasks();
  }

  static public async Task  Main(string[] args)
  {
    var myProg = new Program();
    //## Setup Console Break Handler
    HandlerRoutine hr = new HandlerRoutine(myProg.ConsoleCtrlCheck);
    GC.KeepAlive(hr);
    SetConsoleCtrlHandler(hr, true);
    myProg.Log("ConsoleCtrlHandler is set...");

    const Int32 nDelayMs = 1000;
    await myProg.LoopTasks(nDelayMs, myProg.tokenSource.Token);

    myProg.Log("Exiting...");
    Environment.Exit(0);
  }
}