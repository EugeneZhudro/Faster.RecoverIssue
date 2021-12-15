using FASTER.core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RecoverIssue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {

                var grantsLogPath = Path.Combine("Storage", "hlog.log");
                var grantsObjLogPath = Path.Combine("Storage", "hlog.obj.log");

                var log = FASTER.core.Devices.CreateLogDevice(grantsLogPath);
                var objLog = FASTER.core.Devices.CreateLogDevice(grantsObjLogPath);

                var store = new FasterKV<string, List<string>>(
                        1L << 20,
                        new LogSettings { LogDevice = log, ObjectLogDevice = objLog },
                        new CheckpointSettings { CheckpointDir = "Storage", RemoveOutdated = true, CheckPointType = CheckpointType.FoldOver });

               store.Recover();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"UnhandledException: {e.ExceptionObject.ToString()}");
        }


        private static async Task<bool> TrySave(FasterKV<string, List<string>> store, List<UpdateInfo> updateData)
        {
            var updateFailed = false;
            using (var session = store.NewSession(new UpdateFunction()))
            {
                foreach (var updInfo in updateData)
                {
                    var status = session.RMW(updInfo.Key, updInfo);
                     if (status == Status.ERROR)
                    {
                        updateFailed = true;
                        break;
                    }
                }
                await session.CompletePendingAsync();
            }

            await store.TakeHybridLogCheckpointAsync(CheckpointType.FoldOver);
            return !updateFailed;
        }
    }
}
