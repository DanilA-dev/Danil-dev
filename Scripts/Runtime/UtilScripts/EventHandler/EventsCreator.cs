#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using D_dev.Scripts.EventHandler;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace D_dev.Scripts.Runtime
{
    [CreateAssetMenu(menuName = "D-Dev/EventCreator")]
    public class EventsCreator : ScriptableObject
    {
        #region Fields
        
        [SerializeField] private string _eventName;

        #endregion

        #region Editor

         [Button]
        private void CreateEvent()
        {
            var guids = AssetDatabase.FindAssets("CustomEventType");
            if (guids.Length == 0)
            {
                Debug.LogError("[CustomEventType] file not found");
                return;
            }
            var fullPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (!string.IsNullOrEmpty(_eventName))
            {
                if(_eventName.Contains(" "))
                    _eventName = _eventName.Replace(" ", "");
                
                string filePath = fullPath;
                List<string> lines = new List<string>();
        
                lines.AddRange(File.ReadAllLines(filePath));
                
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
                else
                    Debug.Log($"[CustomEventType] Event - [{_eventName}] already exists");
        
                lines.Clear();
                lines.AddRange(File.ReadAllLines(filePath));
        
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

