using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using _Assets.Scripts.Utils.SheetsLoadable;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace SheetsData
{
    [CreateAssetMenu(menuName = "Editor/Sheets/Data Importer")]
    public class SheetsDataUrlImporter : SerializedScriptableObject
    {
        private enum ImportType
        {
            URL,
            File
        }

        [SerializeField] private ImportType _importType;
        [ShowIf(nameof(_importType), ImportType.URL)]
        [SerializeField] private string _url;
        [ShowIf(nameof(_importType), ImportType.File)]
        [SerializeField] private TextAsset _textFile;
        [OdinSerialize] private List<ISheetsLoadable> _dataToImport = new();

        [Button]
        public void ImportData()
        {
            if (_dataToImport.Count <= 0)
            {
                Debug.Log("No data to load...");
                return;
            }
            if(_importType == ImportType.File)
                ImportFromFile();
            else
                ImportFromURL();
        }

        private async void ImportFromURL()
        {
            using (HttpClient client = new HttpClient())
            {
                if (_url == string.Empty)
                {
                    client.CancelPendingRequests();
                    return;

                }
                string csvData = await client.GetStringAsync(_url);
                HandleCsvData(csvData);
            }
        }

        private void ImportFromFile()
        {
            if (_textFile == null)
            {
                Debug.LogError("File is null");
                return;
            }
            
            HandleCsvData(_textFile.text);
        }

        private void HandleCsvData(string dataPath, Action onEmptyPath = null)
        {
             var lines = dataPath.Split('\n');

             if (lines.Length <= 0)
             {
                 onEmptyPath?.Invoke();
                 return;
             }
               
             var headers = lines[0].Split(',').Select(h => h.Trim()).ToList();
                
             for (int i = 1; i < lines.Length; i++)
             {
                 var values = lines[i].Split(',');
                 var data = new Dictionary<string, object>();

                 for (int j = 0; j < headers.Count; j++)
                 {
                     string header = headers[j];
                     string value = values.Length > j? values[j] : string.Empty;

                        
                     if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatResult))
                         data[header] = floatResult;
                     else if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intResult))
                         data[header] = intResult;
                     else if (double.TryParse(value,NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleResult))
                         data[header] = doubleResult;
                     else if (!string.IsNullOrEmpty(value))
                         data[header] = value; 
                     else
                         data[header] = null; 
                 }

                 var loadableData = GetData(data);
                 if (loadableData == null)
                     continue;
                    
                 loadableData.Deserialize(data);
                 EditorApplication.delayCall += () =>
                 {
                     EditorUtility.SetDirty(loadableData as ScriptableObject);
                     AssetDatabase.SaveAssets();
                     AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                     UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                     string importRoot = _importType == ImportType.File
                         ? $"File"
                         : $"URL - {_url}";
                     Debug.Log($"<color=green> Data {loadableData} is loaded from {importRoot} </color>");   
                 };
             }
        }
        
        private ISheetsLoadable GetData(Dictionary<string, object> data)
        {
            if (data.TryGetValue("Name", out var value) && value is string dataName)
                return _dataToImport.FirstOrDefault(a => a.Name == dataName);

            return null; 
        }
    }
}