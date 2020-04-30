# LoopTask - Demonstrates Tasks & Thread Pool with C# & DotNetCore

When you run the program, it creates a new task every 1000mS. Each task executes for 2500mS. After a few seconds, type Control-C to exit, but any tasks still executing will continue till they complete.

This program demonstrates the use of 'Task.Run', 'Task.Delay', 'Task.WaitAll' and Async/Await.

Sample output:
```
K:\$CODE\C-SHARP\$DotNetCore\LoopTask\bin\Debug\netcoreapp3.1>LoopTask.exe
[2020-04-30 18:04:27.082][1] ConsoleCtrlHandler is set...
[2020-04-30 18:04:27.112][4 bg tp] Enter - 1
[2020-04-30 18:04:28.124][5 bg tp] Enter - 2
[2020-04-30 18:04:29.128][6 bg tp] Enter - 3
[2020-04-30 18:04:29.613][4 bg tp] >Exit - 1 after 2500mS
[2020-04-30 18:04:30.139][7 bg tp] Enter - 4
[2020-04-30 18:04:30.468][8 bg] ::: Exit Event: CTRL_C_EVENT
[2020-04-30 18:04:30.626][5 bg tp] >Exit - 2 after 2501mS
[2020-04-30 18:04:31.146][5 bg tp] Loop has exited
Task 1 - RanToCompletion
Task 2 - RanToCompletion
Task 3 - Running
Task 4 - Running
[2020-04-30 18:04:31.638][6 bg tp] >Exit - 3 after 2509mS
[2020-04-30 18:04:32.639][7 bg tp] >Exit - 4 after 2499mS
Task 1 - RanToCompletion
Task 2 - RanToCompletion
Task 3 - RanToCompletion
Task 4 - RanToCompletion
[2020-04-30 18:04:32.644][5 bg tp] Exiting...
```
In the above, after the timestamp, in square brackets, it shows:
* ManagedThreadId
* 'bg' for background threads
* 'tp' for thread pool threads

