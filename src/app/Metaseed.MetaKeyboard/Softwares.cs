﻿using System;
using Metaseed.Input;
using Metaseed.UI;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Metaseed.MetaKeyboard
{
    public class Utilities
    {
        public Utilities()
        {
            Keys.C.With(Keys.CapsLock).Hit("Metaseed.OpenCodeEditor", "Open &Code Editor", async e =>
            {
                var info = new ProcessStartInfo(Config.Current.Tools.Code) {UseShellExecute = true};

                if (!Window.IsExplorerOrOpenSaveDialog)
                {
                    Process.Start(info);
                    return;
                }

                var paths = await Explorer.GetSelectedPath(Window.CurrentWindowHandle);

                if (paths.Length == 0)
                {
                    Process.Start(info);
                    return;
                }

                foreach (var path in paths)
                {
                    info.Arguments = path;
                    Process.Start(info);
                }
            }, null, true);

            Keys.D.With(Keys.CapsLock).MapOnHit(Keys.D.With(Keys.LMenu).With(Keys.ShiftKey));

            Keys.F.With(Keys.CapsLock).Down("Metaseed.Find", "&Find With Everything", async e =>
            {
                e.Handled = true;
                var shiftDown = e.KeyboardState.IsDown(Keys.ShiftKey);

                var c = Window.CurrentWindowClass;
                if ("CabinetWClass" != c)
                {
                    Utils.Run(shiftDown
                        ? $"{Config.Current.Tools.EveryThing} -newwindow"
                        : $"{Config.Current.Tools.EveryThing} -toggle-window");
                    return;
                }

                var path = await Explorer.Path(Window.CurrentWindowHandle);
                Utils.Run(shiftDown
                    ? $"{Config.Current.Tools.EveryThing} -path {path} -newwindow"
                    : $"{Config.Current.Tools.EveryThing} -path {path} -toggle-window");
            });

            Keys.T.With(Keys.CapsLock).Down("Metaseed.Terminal", "Open &Terminal", async e =>
            {
                e.Handled = true;
                var shiftDown = e.KeyboardState.IsDown(Keys.ShiftKey);

                var c = Window.CurrentWindowClass;
                if ("CabinetWClass" != c)
                {
                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    Utils.Run(shiftDown
                        ? $"{Config.Current.Tools.Cmd}  /single /start \"{folderPath}\""
                        : $"{Config.Current.Tools.Cmd}  /start \"{folderPath}\"");
                    return;
                }

                var path = await Explorer.Path(Window.CurrentWindowHandle);
                Utils.Run(shiftDown
                    ? $"{Config.Current.Tools.Cmd} /start \"{path}\""
                    : $"{Config.Current.Tools.Cmd} /single /start \"{path}\"");
            });
            Keys.W.With(Keys.CapsLock).Down("Metaseed.WebSearch", "&Web Search", e =>
            {
                var altDown = e.KeyboardState.IsDown(Keys.Menu);
                new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = true,
                        FileName = altDown
                            ? Config.Current.Tools.SearchEngineSecondary
                            : Config.Current.Tools.SearchEngine
                    }
                }.Start();
            });

            var softwareTrigger = Keys.Space.With(Keys.CapsLock).Handled();

            softwareTrigger.Then(Keys.R).Down("Metaseed.ScreenRuler", "Start Screen &Ruler",
                e =>
                {
                    e.Handled = true;
                    Utils.Run(Config.Current.Tools.Ruler);
                });

            softwareTrigger.Then(Keys.T).Down("Metaseed.TaskExplorer", "Start &Task Explorer ",
                e =>
                {
                    e.Handled = true;
                    Utils.Run(Config.Current.Tools.ProcessExplorer);
                });

            softwareTrigger.Then(Keys.G).Down("Metaseed.GifRecord", "Start &Gif Record ",
                e =>
                {
                    e.Handled = true;
                    Utils.Run(Config.Current.Tools.GifTool);
                });

            softwareTrigger.Then(Keys.N).Down("Metaseed.NodePad", "Start &Notepad ", e =>
            {
                e.Handled = true;
                var exeName = "Notepad";
                var notePad = Process.GetProcessesByName(exeName);

                var hWnd = notePad.FirstOrDefault(p => p.MainWindowTitle == "Untitled - Notepad")?.MainWindowHandle;
                if (hWnd != null)
                {
                    Window.Show(hWnd.Value);
                    return;
                }

                Utils.Run("Notepad");
            });

            softwareTrigger.Then(Keys.V).Down("Metaseed.VisualStudio", "Start &VisualStudio ", async e =>
            {
                if (!Window.IsExplorerOrOpenSaveDialog) return;

                e.Handled = true;

                var path = await Explorer.Path(Window.CurrentWindowHandle);
                if (string.IsNullOrEmpty(path))
                {
                    Process.Start(new ProcessStartInfo(Config.Current.Tools.VisualStudio)
                        {UseShellExecute = true});
                    return;
                }

                Directory.CreateDirectory(path).EnumerateFiles("*.sln").Select(f => f.FullName).AsParallel().ForAll(s =>
                {
                    Process.Start(new ProcessStartInfo(Config.Current.Tools.VisualStudio)
                        {UseShellExecute = true, Arguments = s, WorkingDirectory = path});
                });
            });
        }
    }
}