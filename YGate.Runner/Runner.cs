using System;

namespace YGate.Security
{
    public static class Runner
    {

        public static Action<string> ExceptionReportDelegate { get; set; }

        public static void TaskRun(Task runn)
        {
            try
            {
                runn.Start();
            }
            catch (Exception ex)
            {
                ExceptionReportDelegate.DynamicInvoke(ex);
            }
        }

        public static void ActionRun(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                ExceptionReportDelegate.DynamicInvoke(ex);
            }
        }
    }
}
