﻿using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace Metaseed.MetaPlugin
{
    public abstract class PluginBase : IMetaPlugin
    {
        private readonly ILogger _logger;

        protected PluginBase(ILogger logger)
        {
            _logger                  =  logger;
        }

        private void OnPluginUnloadingRequested(AssemblyLoadContext obj)
        {
            this.OnUnloading();
        }

        public virtual bool Init()
        {
            _logger.LogInformation($"{this.GetType().Name} loaded.");
            return true;
        }

        public virtual void OnUnloading()
        {
            _logger.LogInformation($"{this.GetType().Name} start unloading, make sure resources get released here");
        }
    }
}