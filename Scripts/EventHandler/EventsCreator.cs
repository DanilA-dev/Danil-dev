#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace D_dev.Scripts.Runtime
{
    [CreateAssetMenu(menuName = "D-Dev/EventCreator")]
    public class EventsCreator : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _eventsContainerPath;
        [SerializeField] private TextAsset _structureEventContainerPlaceHolder;
        [Space]
        [SerializeField] private string _eventName;

        #endregion

        #region Editor

         [Button]
        private void CreateEvent()
        {
            var split = _eventsContainerPath.Split('/');
            split[^1] = "EventContainer.cs";
            var fullPath = Path.Combine(_eventsContainerPath, split[^1]);
            
            if (!string.IsNullOrEmpty(_eventName))
            {
                if(_eventName.Contains(" "))
                    _eventName = _eventName.Replace(" ", "");
                
                string filePath = fullPath;
                List<string> lines = new List<string>();
        
                if (File.Exists(filePath))
                    lines.AddRange(File.ReadAllLines(filePath));
                else
                    lines.AddRange(_structureEventContainerPlaceHolder.text.Split(new[] { "\r\n", "\n", "\r"}, System.StringSplitOptions.None));
        
                int enumDeclarationIndex = lines.FindIndex(line => line.Contains("public enum"));
                int openingBraceIndex = lines.FindIndex(enumDeclarationIndex, line => line.Contains("{"));
                int closingBraceIndex = lines.FindIndex(openingBraceIndex, line => line.Contains("}"));
        
                List<string> existingEvents = new List<string>();
                for (int i = openingBraceIndex + 1; i < closingBraceIndex; i++)
                {
                    var line = lines[i].Trim().TrimEnd(',');
                    if (!string.IsNullOrEmpty(line) && line != "{")
                    {
                        var eventName = line.Split('=')[0].Trim();
                        existingEvents.Add(eventName);
                    }
                }
        
                if (!existingEvents.Contains(_eventName))
                    existingEvents.Add(_eventName);
        
                if (File.Exists(filePath))
                    File.Delete(filePath);
        
                lines.Clear();
                lines.AddRange(_structureEventContainerPlaceHolder.text.Split(new[] { "\r\n", "\n", "\r"}, System.StringSplitOptions.None));
        
                enumDeclarationIndex = lines.FindIndex(line => line.Contains("public enum"));
                openingBraceIndex = lines.FindIndex(enumDeclarationIndex, line => line.Contains("{"));
                closingBraceIndex = lines.FindIndex(openingBraceIndex, line => line.Contains("}"));
        
                for (int i = openingBraceIndex + 1; i < closingBraceIndex; i++)
                    lines.RemoveAt(openingBraceIndex + 1);
        
                int insertIndex = openingBraceIndex + 1;
                for (int i = 0; i < existingEvents.Count; i++)
                    lines.Insert(insertIndex + i, $"       {existingEvents[i]} = {i},");
        
                if (existingEvents.Count > 0)
                {
                    int lastIndex = insertIndex + existingEvents.Count - 1;
                    lines[lastIndex] = lines[lastIndex].TrimEnd(',');
                }
        
                File.WriteAllLines(filePath, lines);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                _eventName = string.Empty;
            }
        }

        #endregion
    }
}
#endif

