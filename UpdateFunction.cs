using FASTER.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoverIssue
{
    public class UpdateFunction : FunctionsBase<string, List<string>, UpdateInfo, List<string>, Empty>
    {
        private readonly Func<UpdateInfo, List<string>, List<string>> m_Updater;

        public UpdateFunction()
        {
            m_Updater = UpdateFilter;
        }

        public UpdateFunction(Func<UpdateInfo, List<string>, List<string>> updater)
        {
            m_Updater = updater;
        }

        #region RMW functions
        public override void InitialUpdater(ref string key, ref UpdateInfo input, ref List<string> value, ref List<string> output)
        {
            value = m_Updater(input, value);
        }

        public override void CopyUpdater(ref string key, ref UpdateInfo input, ref List<string> oldValue, ref List<string> newValue, ref List<string> output)
        {
            newValue = m_Updater(input, oldValue);
        }

        public override bool InPlaceUpdater(ref string key, ref UpdateInfo input, ref List<string> value, ref List<string> output)
        {
            value = m_Updater(input, value);
            return true;
        }
        #endregion

        private List<string> UpdateFilter(UpdateInfo updateInput, List<string> value)
        {
            var result = value == null ? new List<string>() : new List<string>(value);

            if (updateInput.IsRemoving)
            {
                result.Remove(updateInput.ValueItem);
            }
            else
            {
                result.Add(updateInput.ValueItem);
            }
            return result;
        }
    }
}
