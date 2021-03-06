﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Metatool.Input
{
   

    /// <summary>
    /// await TaskExt
    // .FromEvent<WebBrowserDocumentCompletedEventArgs>()
    // .WithHandlerConversion(handler => new WebBrowserDocumentCompletedEventHandler(handler))
    // .Start(
    // handler => this.webBrowser.DocumentCompleted += handler,
    // () => this.webBrowser.Navigate(@"about:blank"),
    // handler => this.webBrowser.DocumentCompleted -= handler,
    // CancellationToken.None);
    /// </summary>
    public static class TaskExt
    {
        public static EAPTask<TEventArgs, EventHandler<TEventArgs>> FromEvent<TEventArgs>(Action<TEventArgs> action=null)
        {
            var tcs     = new TaskCompletionSource<TEventArgs>();
            var handler = new EventHandler<TEventArgs>((s, e) =>
            {
                action?.Invoke(e);
                tcs.TrySetResult(e);
            });
            return new EAPTask<TEventArgs, EventHandler<TEventArgs>>(tcs, handler);
        }
    }


    public sealed class EAPTask<TEventArgs, TEventHandler>
        where TEventHandler : class
    {
        private readonly TaskCompletionSource<TEventArgs> _completionSource;
        private readonly TEventHandler                    _eventHandler;

        public EAPTask(
            TaskCompletionSource<TEventArgs> completionSource,
            TEventHandler eventHandler)
        {
            _completionSource = completionSource;
            _eventHandler     = eventHandler;
        }

        public EAPTask<TEventArgs, TOtherEventHandler> HandlerConversion<TOtherEventHandler>(
            Converter<TEventHandler, TOtherEventHandler> converter)
            where TOtherEventHandler : class
        {
            return new EAPTask<TEventArgs, TOtherEventHandler>(
                _completionSource, converter(_eventHandler));
        }

        public async Task<TEventArgs> Start(
            Action<TEventHandler> subscribe,
            Action<TEventHandler> unsubscribe,
            CancellationToken cancellationToken,
            Action trigger = null
            )
        {
            subscribe(_eventHandler);
            try
            {
                using (cancellationToken.Register(() => _completionSource.SetCanceled()))
                {
                    trigger?.Invoke();
                    return await _completionSource.Task;
                }
            }
            finally
            {
                unsubscribe(_eventHandler);
            }
        }
    }
}
