using System;

// Use me like ProcessCheck.IsProcessOpen("explorer")
public static class ProcessCheck {
    public static bool IsProcessRunning(string name)
    {
        foreach (Process process in Process.GetProcesses())
        {
            if (process.ProcessName.Contains(name))
            return true;
        }
        return false;
    }
}        
        