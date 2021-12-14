using FASTER.core;
using System;
using System.Collections.Generic;
using System.IO;

namespace RecoverIssue
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
    }
}
