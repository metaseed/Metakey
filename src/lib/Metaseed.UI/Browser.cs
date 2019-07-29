﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace Metaseed.UI
{
    public class Browser
    {
        public static string DefaultPath
        {
            get
            {
                string      name   = string.Empty;
                RegistryKey regKey = null;

                try
                {
                    var regDefault = Registry.CurrentUser.OpenSubKey(
                        "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.htm\\UserChoice", false);
                    var stringDefault = regDefault.GetValue("ProgId");

                    regKey = Registry.ClassesRoot.OpenSubKey(stringDefault         + "\\shell\\open\\command", false);
                    name   = regKey.GetValue(null).ToString().ToLower().Replace("" + (char) 34, "");

                    if (!name.EndsWith("exe"))
                        name = name.Substring(0, name.LastIndexOf(".exe") + 4);
                }
                catch (Exception ex)
                {
                    name =
                        $"ERROR: An exception of type: {ex.GetType()} occurred in method: {ex.TargetSite} in the following module: {typeof(Browser)}";
                }
                finally
                {
                    regKey?.Close();
                }

                return name;
            
            }
        }
    }
}

