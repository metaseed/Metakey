﻿using System.Diagnostics;
using System.Linq;

namespace Metaseed.MetaKeyboard
{
    public class ProcessEx
    {
        static string BuildCmd(string cmd, params string[] args)
        {
            var a = string.Join(" ", args.ToList().Select(arg => arg.Any(char.IsWhiteSpace)? $"\"{arg}\"": arg));
            return $"\"\"{cmd}\" {a}\"";
        }
        // Process.Start would keep the process parent/child structure, if the parent exit, the child would exit.
        // so we use cmd to make a workaround
        // https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
        // https: //ss64.com/nt/cmd.html
        public static void Run(string cmd, params string[] args)
        {
           
            var proc = new Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd",
                    Arguments = "/c " + BuildCmd(cmd,args),
                    RedirectStandardOutput = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                }
            };
            proc.Start();
        }

        /// <summary>
        /// Process.Start would keep the process parent/child structure, if the parent exit, the child would exit.
        /// so we use cmd to make a workaround
        ///
        /// this could run *.lnk
        /// </summary>
        /// <param name="cmd"></param>
        public static void Start(string cmd)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName        = "explorer.exe",
                    Arguments       = cmd,
                    CreateNoWindow  = true,
                    UseShellExecute = false,
                    WindowStyle     = ProcessWindowStyle.Hidden
                }
            };
            proc.Start();
        }
    }
}