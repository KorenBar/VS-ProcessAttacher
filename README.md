# Visual Studio Process Attacher
#### A simple tool for attaching a process to the Visual Studio debugger by a single command.
With this tool you can simply automate remote debugging by copying the output files to a remote machine (using scp/rsync or mapping a share drive) and then running this tool.

1. Run Visual Studio
2. Open a project.
3. Build and run your application on your computer or on *remote machine*.
3. Run AttachDebugger with process id/name and other arguments (run with flag --help for reading the options).
5. Done! The process is attached and you can debug it.

*NOTE: The process will be attached to the first instance of Visual Studio.*


### Command Line Options:
```
  --print              Print transport and engine options.

  -t, --transport      Choose connection type by id or name (default is the local machine).

  -e, --engine         Choose code type by id or name (default is the first option).

  -d, --destenation    Set connection target (default is the local machine).

  -p, --pid            Choose process on the target mechine by process id.

  -n, --pname          Choose process on the target mechine by process name (if pid is set, it will search by pid first).

  --dte                (Default: VisualStudio.DTE.17.0) Set DTE process ID.
```

## Post-Build Remote Debugging Automation (C# Application)
#### Upload Build Output to Remote Machine Using a Share Drive
1. Setup a Samba server on your remote machine.
2. Map a share drive on your debugging machine (let's say 'R' drive).
3. Add a new build plaform which will be selected for remote debugging (I've chose ARM64 for my Raspberry Pi 4).
4. Create a conditional PropertyGroup on the project file(.csproj) for changing the output path. 
``` xml
  <PropertyGroup Condition=" '$(Platform)' == 'ARM64' ">
	  <PlatformTarget>ARM64</PlatformTarget>
	  <BaseOutputPath>R:\home\pi\My\App</BaseOutputPath>
  </PropertyGroup>
```
#### Post-Build Script
5. Create a script and do the following things in it: (you can see my example post-bulid batch script)
    * Kill your app on the remote machine if already running.
    * Run your app on the remote machine using plink on a new cmd process (for not blocking the script).
    * Wait for the process to start on the remote machine (in parallel to plink).
    * Run AttachDebugger command. For my Raspberry Pi 4 through SSH the command will be:
    ``` cmd
    AttachDebugger -t SSH -e "Managed (.NET Core for Unix)" -d pi@192.168.1.7 --pname dotnet
    ```
5. And your script as a conditional post-build event on the project file. (use powershell start-process command for not blocking the bulid process)
``` xml
  <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition=" '$(Configuration)|$(Platform)' == 'Debug|ARM64' ">
    <Exec Command="powershell start-process PostBuildEvent.bat" />
  </Target>
```

TIP: Your application can wait for the debugger to attach by adding these lines of code:
``` C#
#if DEBUG
  Console.WriteLine("Waiting for debugger to attach");
  while (!System.Diagnostics.Debugger.IsAttached)
    Thread.Sleep(100);
#endif
```

### TODO:
- Pack (merge) that tool's assemblies to a single executable.

