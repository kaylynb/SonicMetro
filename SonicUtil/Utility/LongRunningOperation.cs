using System;
using System.Threading.Tasks;

namespace SonicUtil.Utility
{
    public class LongRunningOperation : BindableBase
    {
        public enum OperationState
        {
            Initial,
            Running,
            Done,
            Error
        }

        public LongRunningOperation()
        {
            State = OperationState.Initial;
        }

        private OperationState _state;
        public OperationState State
        {
            get { return _state; }
            set
            {
                SetProperty(ref _state, value);
// ReSharper disable ExplicitCallerInfoArgument
                OnPropertyChanged("IsRunning");
// ReSharper restore ExplicitCallerInfoArgument
            }
        }

        public bool IsRunning { get { return State == OperationState.Running; } }

        public async Task<T> RunAsync<T>(Func<Task<T>> func, Action<Exception> errorFunc)
        {
            ThrowIf.Null(func, "func");
            ThrowIf.Null(errorFunc, "errorFunc");

            State = OperationState.Running;

            var res = default(T);

            try
            {
                res = await func();

                State = OperationState.Done;
            }
            catch (Exception e)
            {
                State = OperationState.Error;
                errorFunc(e);
            }

            return res;
        }

        public async Task RunAsync(Func<Task> func, Action<Exception> errorFunc)
        {
            ThrowIf.Null(func, "func");
            ThrowIf.Null(errorFunc, "errorFunc");

            State = OperationState.Running;

            try
            {
                await func();

                State = OperationState.Done;
            }
            catch (Exception e)
            {
                State = OperationState.Error;
                errorFunc(e);
            }
        }
    }
}
