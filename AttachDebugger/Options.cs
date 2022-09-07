using System;
using CommandLine;

namespace AttachDebugger
{
    public class Options
    {
        public static Options Default { get; } = Parser.Default.ParseArguments<Options>(Environment.GetCommandLineArgs()).Value;

        [Option("print", HelpText = "Print transport and engine options.")]
        public bool Print { get; set; }

        [Option('t', "transport", HelpText = "Choose connection type by id or name (default is the local machine).")]
        public string Transport { get; set; }

        [Option('e', "engine", HelpText = "Choose code type by id or name (default is the first option).")]
        public string Engine { get; set; }

        [Option('d', "destenation", HelpText = "Set connection target (default is the local machine).")]
        public string Destenation { get; set; }

        [Option('p', "pid", HelpText = "Choose process on the target mechine by process id.")]
        public int ProcessID { get; set; }

        [Option('n', "pname", HelpText = "Choose process on the target mechine by process name (if pid is set, it will search by pid first).")]
        public string ProcessName { get; set; }

        [Option("dte", HelpText = "Set DTE process ID.", Default = "VisualStudio.DTE.17.0")]
        public string DTE { get; set; } = "VisualStudio.DTE.17.0";
    }
}
