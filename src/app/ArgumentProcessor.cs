﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.InteropServices;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using Metatool.Plugin;
using Metatool.Utils;
using Microsoft.Extensions.Logging;

namespace Metaseed.Metatool
{
    public class ArgumentProcessor
    {
        private readonly ILogger _logger;
        private string[] _args;

        public ArgumentProcessor(string[] args)
        {
            _args = args;
            if(args.Length == 0 || args[0] == "run")
                ConsoleExt.InitialConsole(true);
            App.InitServices();
            _logger = Services.Get<ILogger<ArgumentProcessor>>();
        }

        const string HelpOptionTemplate = "-? | -h | --help";

        public int ArgumentsProcess()
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name             = "metatool",
                Description      = "tools for Windows",
                ExtendedHelpText = "===Metaseed Metatool==="
            };
            app.HelpOption(inherited: true);

            app.OnExecute(() =>
            {
                // without command
                var application   = new App();
                var pluginManager = Services.GetOrCreate<PluginManager>();
                pluginManager.InitPlugins();
                application.RunApp();
            });

            app.Command("new", configCmd =>
            {
                configCmd.OnExecute(() =>
                {
                    Console.WriteLine("Please specify a subcommand");
                    configCmd.ShowHelp();
                    return 1;
                });

                configCmd.Command("script", c =>
                {
                    c.Description =
                        "Creates a sample script tool along with the files needed to launch and debug the script.";

                    var fileName = c.Argument("name",
                        "The name of the tool script to be created.").IsRequired(errorMessage: "please set the tool name \nusage: metatool new script <name>");
                    var cwd = c.Option("-dir |--directory <dir>",
                        "The directory to initialize the tool scripts. Defaults to current directory.",
                        CommandOptionType.SingleValue).Accepts(v => v.ExistingDirectory());
                    c.HelpOption(HelpOptionTemplate);
                    c.OnExecute(() =>
                    {
                        var scaffolder = new Scaffolder(_logger);
                        scaffolder.InitTemplate(fileName.Value, cwd.Value());
                    });
                });

                configCmd.Command("lib", c =>
                {
                    c.Description =
                        "Creates a sample lib(dll) tool along with the files needed to launch and debug the csharp project.";

                    var fileName = c.Argument("name",
                        "The name of the tool to be created.");
                    var cwd = c.Option("-dir |--directory <dir>",
                        "The directory to initialize the tool. Defaults to current directory.",
                        CommandOptionType.SingleValue).Accepts(v => v.ExistingDirectory());
                    c.HelpOption(HelpOptionTemplate);
                    c.OnExecute(() =>
                    {
                        var scaffolder = new Scaffolder(_logger);
                        scaffolder.InitTemplate(fileName.Value, cwd.Value(), true);
                    });
                });
            });

            app.Command("run", c =>
            {
                c.Description = "run the script or lib with metatool ";

                var fileName = c.Argument("path",
                    "The name of the tool script to be created.");
                fileName.Validators.Add(new FileNameValidator());

                c.HelpOption(HelpOptionTemplate);
                c.OnExecute(() =>
                {
                    var application = new App();
                    var fullPath = fileName.Value;
                    if (!File.Exists(fullPath))
                        fullPath = Path.Combine(Context.CurrentDirectory, fullPath);

                    if (fullPath.EndsWith(".dll"))
                    {
                        try
                        {
                            var pluginManager = Services.GetOrCreate<PluginManager>();
                            pluginManager.LoadDll(fullPath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                $"Error while loading tool {fullPath}! No tools loaded! Please fix it then restart!");
                        }
                    }
                    else if (fullPath.EndsWith(".csx"))
                    {
                        var assemblyName = Path.GetFileName(Path.GetDirectoryName(fullPath));
                        _logger.LogInformation($"Compile&Run: {fullPath}, {assemblyName}");
                        var pluginManager = Services.GetOrCreate<PluginManager>();
                        pluginManager.BuildReload(fullPath, assemblyName, false);
                    }
                    application.RunApp();
                });
            });

            // windows only 
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // on windows we have command to register .csx files to be executed by dotnet-script
                app.Command("register", c =>
                {
                    c.Description = "Register .csx file handler to enable running scripts directly";
                    c.HelpOption(HelpOptionTemplate);
                    c.OnExecute(() =>
                    {
                        var scaffolder = new Scaffolder(_logger);
                        scaffolder.RegisterFileHandler();
                    });
                });
            }


            return app.Execute(_args);
        }

        class FileNameValidator : IArgumentValidator
        {
            public ValidationResult GetValidationResult(CommandArgument argument, ValidationContext context)
            {
                var path = argument.Value;

                if (string.IsNullOrEmpty(path))
                    return new ValidationResult(
                        $"the 'run' command should have argument of a file end with '.csx' or '.dll'");

                if (!path.EndsWith(".dll") && !path.EndsWith(".csx"))
                    return new ValidationResult($"The value for {argument.Value} must be end with '.csx' or '.dll'");

                if (File.Exists(path))
                {
                    return ValidationResult.Success;
                }

                path = Path.Combine(Context.CurrentDirectory, path);
                if (File.Exists(path))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"Could not find {path}!");
            }
        }
    }
}