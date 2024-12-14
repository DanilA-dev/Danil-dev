using System.Collections.Generic;

namespace _Assets.Scripts.Utils.SheetsLoadable
{
    public interface ISheetsLoadable
    {
        public string Name { get; }
        public Dictionary<string, object> Serialize();
        public void Deserialize(Dictionary<string, object> data);
    }
}