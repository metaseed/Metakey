﻿using System;
using System.Linq;
using Metatool.Utils;
using Microsoft.CodeAnalysis.Scripting;

namespace Metaseed.Metatool
{
    public partial class App
    {
        private const string DebugFlagShort = "-d";
        private const string DebugFlagLong  = "--debug";
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                return new ArgumentProcessor(args).ArgumentsProcess();
            }
            catch (Exception e)
            {
                if (e is AggregateException aggregateEx)
                {
                    e = aggregateEx.Flatten().InnerException;
                }

                if (e is CompilationErrorException)
                {
                    // no need to write out anything as the upstream services will report that
                    return 0x1;
                }

                // Be verbose (stack trace) in debug mode otherwise brief
                var error = args.Any(arg => arg    == DebugFlagShort
                                            || arg == DebugFlagLong)
                    ? e.ToString()
                    : e.GetBaseException().Message;
                Console.Error.WriteLine(error);
                return 0x1;
            }
        }
    }
}