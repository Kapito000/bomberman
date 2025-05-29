using System;

namespace RemotePlugin.Remote {
    public interface IExceptionHandler {
        void HandleException(string message);
        void HandleException(Exception exception);
    }
}
