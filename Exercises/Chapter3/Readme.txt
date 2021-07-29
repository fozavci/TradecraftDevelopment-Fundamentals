Petaq Capabilities is taken from Petaq as Gadgets
* CheckWin() // Checking the platform whether Windows or not
* ExecSharpAssembly() // Executes the given .NET assembly bytecode with parameters
* ExecSharpCode() // Compile and execute .NET source code with parameters
* Exec() // Process execution with parameters
* ExecPowershellAutomation() // Execute PowerShell scripts in System.Management.Automation
* ExecShellcode() // Executes shellcode using CreateProcess and QueueUserAPC with PPID spooing with explorer as parent


To compile the capabilities, you need System.Management.Automation reference for Powershell

mcs /r:System.Management.Automation.dll /target:library /out:Petaq-CapabilitiesClass.dll Petaq-CapabilitiesClass.cs
