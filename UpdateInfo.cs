using System;
using System.Collections.Generic;
using System.Text;

namespace RecoverIssue
{
    public class UpdateInfo
    {
        public string Key { get; set; }
        public string ValueItem { get; set; }
        public bool IsRemoving { get; set; }

        public UpdateInfo(string key, string valueItem, bool isRemoving)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (valueItem == null) throw new ArgumentNullException(nameof(valueItem));

            Key = key;
            ValueItem = valueItem;
            IsRemoving = isRemoving;
        }
    }
}
