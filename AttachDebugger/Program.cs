using System;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;
using System.Runtime.InteropServices;
using CommandLine;

namespace AttachDebugger
{
    public static class Program
    {
        static Debugger5 GetDebugger(Options opts)
        { // get the running DTE
            try
            {
                DTE2 dte = (DTE2)Marshal.GetActiveObject(opts.DTE); // NOTE that will get the first!
                return (Debugger5)dte.Debugger;
            }
            catch (Exception ex)
            {
                throw new DebugException(ErrorCode.NoDebugger, "Failed to get debugger", ex);
            }
        }

        static void DebugProcess(Debugger5 debugger, Options opts)
        {
            Transport transport = null;
            Engine engine = null;
            Process2 process = null;

            if (opts.ProcessID <= 0 && string.IsNullOrEmpty(opts.ProcessName))
                throw new DebugException(ErrorCode.MissingArgument, "No process chosen");

            // choose transport and engine
            try
            {
                if (!string.IsNullOrEmpty(opts.Transport))
                {
                    transport = debugger.Transports.Item(t => t.ID == opts.Transport || t.Name == opts.Transport);
                    if (transport == null) 
                        throw new DebugException(ErrorCode.TransportNotFound, "Transport does not exist");

                    if (!string.IsNullOrEmpty(opts.Engine))
                    {
                        engine = transport.Engines.Item(e => e.ID == opts.Engine || e.Name == opts.Engine);
                        if (engine == null) 
                            throw new DebugException(ErrorCode.EngineNotFound, "Engine does not exist");
                    }
                    else foreach (Engine e in transport.Engines)
                        { // get first
                            engine = e;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw new DebugException("Failed to choose transport or engine", ex);
            }

            // find process to debug
            try
            {
                Processes processes;
                if (transport != null && engine != null)
                    processes = debugger.GetProcesses(transport, opts.Destenation ?? Environment.MachineName);
                else processes = debugger.LocalProcesses; // no transport or engine selected - default is local machine

                if (opts.ProcessID > 0) // by process id
                    process = processes.Item(p => p.ProcessID == opts.ProcessID);

                if (process == null && !string.IsNullOrEmpty(opts.ProcessName)) // by process name
                    process = processes.Item(p => p.Name == opts.ProcessName);

                if (process == null) throw new DebugException(ErrorCode.ProcessNotFound, $"Process was not found");
            }
            catch (Exception ex)
            {
                throw new DebugException("Failed to get process to debug", ex);
            }

            // attach to process
            try
            {
                process.Attach2(engine);
            }
            catch (Exception ex)
            {
                throw new DebugException(ErrorCode.AttachFailure, "Attach failed", ex);
            }
        }

        static void PrintEnginesOptions(Debugger5 debugger)
        {
            Console.WriteLine("Transports:");
            foreach (Transport t in debugger.Transports)
            {
                Console.WriteLine();
                Console.WriteLine($"  ID: {t.ID}, Name: \"{t.Name}\"");
                if (t.Engines.Count > 0)
                {
                    Console.WriteLine("    Engines:");
                    foreach (Engine e in t.Engines)
                        Console.WriteLine($"      ID: {e.ID}, Name: \"{e.Name}\"");
                }
                else
                    Console.WriteLine("    No Engines");
            }
        }

        public static int Main()
        {
            var opts = Options.Default;

            if (opts == null) return 0; // probably guide printed

            try
            {
                var debugger = GetDebugger(opts);

                if (opts.Print)
                {
                    PrintEnginesOptions(debugger);
                    return 0;
                }

                DebugProcess(debugger, opts);

                Console.WriteLine("Attached Successfully");
                return 0;
            }
            catch (Exception ex)
            {
                ErrorCode errCode = (ex as DebugException)?.ErrorCode ?? ErrorCode.General;

                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += ": " + ex.InnerException.Message;

                Console.Error.WriteLine($"[Error {(int)errCode}] " + msg);

                return (int)errCode;
            }
        }
    }
}
