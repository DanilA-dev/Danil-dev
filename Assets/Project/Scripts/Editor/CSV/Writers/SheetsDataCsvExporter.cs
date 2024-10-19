using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using _Assets.Scripts.Utils.SheetsLoadable;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace SheetsData
{
    [CreateAssetMenu(menuName = "Editor/Sheets/Data Exporter")]
    public class SheetsDataCsvExporter : SerializedScriptableObject
    {
        [SerializeField] private string _path;
        [OdinSerialize] private List<ISheetsLoadable> _dataToExport = new();
        private string Path => _path + ".csv";


        [Button]
        public void ExportData()
        {
            if(_dataToExport.Count <= 0)
                return;

            if (File.Exists(Path))
            {
                File.Delete(Path);
                Debug.Log($"<color=orange> Rewrite data {Path} </color>");
            }
            
            var allHeaders = new HashSet<string>();

            foreach (var data in _dataToExport)
            {
                var ser = data.Serialize();
                foreach (var header in ser.Keys)
                    allHeaders.Add(header);
            }
            
            using (var writer = new StreamWriter(Path, false))
            {
                writer.WriteLine(string.Join(",", allHeaders));
    
                foreach (var data in _dataToExport)
                {
                    var ser = data.Serialize();
                    var values = allHeaders.Select(header =>
                    {
                        if (ser.TryGetValue(header, out var value))
                        {
                            if (value is IEnumerable<float> floatList)
                                return "\"" + string.Join(";",
                                           floatList.Select(f => f.ToString(CultureInfo.InvariantCulture))) +
                                       "\"";
                            else if (value is IEnumerable<double> doubleList)
                                return "\"" + string.Join(";",
                                    doubleList.Select(d => d.ToString(CultureInfo.InvariantCulture))) + "\"";
                            else if (value is IEnumerable<int> intList)
                                return "\"" + string.Join(";", intList) + "\"";
                            else if (value is IEnumerable<object> objectList)
                                return "\"" + string.Join(";", objectList.Select(o => o.ToString())) + "\"";
                            else
                            {
                                var stringValue = value?.ToString();
                                if (stringValue != null && stringValue.Contains(","))
                                {
                                    var replaceValue = stringValue.Replace(",", ".");
                                    return "\"" + replaceValue + "\"";
                                }
                                return stringValue;
                            }
                        }
                        else
                            return "";
                    });
                        writer.WriteLine(string.Join(",", values)); 
                }
            }
            Debug.Log($"<color=yellow> Data write to file path {Path} </color>");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
        }
    }
}