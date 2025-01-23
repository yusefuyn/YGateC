using System;
using System.Buffers;
using YGate.Entities;

namespace YGate.Operation.Runner
{
    public static class Runner<T>
    {
        public async static Task<T> RunAsync(OperationResult<T> result, Task<Func<object>> action)
        {

            try
            {
                action.Start();
                result.Result = EnumOperationResult.Success;
                return result.Obj;
            }
            catch (Exception ex)
            {
                result.LongDescription += " Error :" + ex.Message.ToString();
                SaveLog(ex.ToString());
                result.Result = EnumOperationResult.Error;
                return result.Obj;
            }
        }

        public static OperationResult<T> Run(ref OperationResult<T> result, Action action)
        {
            try
            {
                action.Invoke();
                result.Result = EnumOperationResult.Success;
                return result;
            }
            catch (Exception ex)
            {
                result.LongDescription += " Error :" + ex.Message.ToString();
                SaveLog(ex.ToString());
                result.Result = EnumOperationResult.Error;
                return result;
            }
        }
        public static async Task<OperationResult<T>> RunAsync(OperationResult<T> result, Func<Task<object>> action)
        {
            try
            {
                await action.Invoke();
                result.Result = EnumOperationResult.Success;
                return result;
            }
            catch (Exception ex)
            {
                result.LongDescription += " Error :" + ex.Message.ToString();
                SaveLog(ex.ToString());
                result.Result = EnumOperationResult.Error;
                return result;
            }
        }


        public static void SaveLog(string log) {
            YGate.IO.Operations.File.SaveLog($"{DateTime.Now.ToString()} : {log.ToString()}");
        }
    }
}
