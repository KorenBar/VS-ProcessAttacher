# Visual Studio Process Attacher
#### A simple tool for attaching a process to the Visual Studio debugger by a single command.
With this tool you can simply automate remote debugging by copying the output files to a remote machine (using scp/rsync or mapping a share drive) and then running this tool.

1. Run Visual Studio
2. Open a project.
3. Build and manually run your application on your computer or on *remote machine*.
3. Run AttachDebugger with process id/name and other arguments (run with flag --help for reading the options).
5. Done! The process is attached and you can debug it.

*NOTE: The process will be attached to the first instance of Visual Studio.*


### Command Line Options:

-  --print => Print transport and engine options.

-  -t, --transport => Choose connection type by id or name (default is the local machine).

-  -e, --engine => Choose code type by id or name (default is the first option).

-  -d, --destenation => Set connection target (default is the local machine).

-  -p, --pid => Choose process on the target mechine by process id.

-  -n, --pname => Choose process on the target mechine by process name (if pid is set, it will search by pid first).

-  --dte => (Default: VisualStudio.DTE.17.0) Set DTE process ID.


### TODO:
- Pack (merge) that tool's assemblies to a single executable.

