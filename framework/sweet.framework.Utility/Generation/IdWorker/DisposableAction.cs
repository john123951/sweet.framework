using System;

namespace sweet.framework.Utility.Generattion
{
    public class DisposableAction : System.IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
            {
                throw new System.ArgumentNullException("action");
            }
            this._action = action;
        }

        public void Dispose()
        {
            this._action();
        }
    }
}