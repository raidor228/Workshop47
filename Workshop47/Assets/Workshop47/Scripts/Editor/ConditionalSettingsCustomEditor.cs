using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Melador.Utils;

namespace Melador.PlayerController.Editor
{
    public class ConditionalSettingsCustomEditor : UnityEditor.Editor
    {
        protected SerializedProperty selectedSettings;
        
        protected virtual void OnEnable()
        {
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            iterator.NextVisible(false);
            selectedSettings = iterator.Copy();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var property = serializedObject.GetIterator();
            
            if (property.NextVisible(true))
            {
                do
                {
                    if (property.name == "m_Script")
                    {
                        continue;
                    }

                    bool isConditional = IsConditionalSettingsGroup(property);
                    if (isConditional)
                    {
                        DrawConditionalSettingsGroup(property);
                    }
                    else
                    {
                        DrawSettingsGroup(property);
                    }
                } while (property.NextVisible(false));
            }

            DrawSelectedProperty();
            
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawSettingsGroup(SerializedProperty parentProperty)
        {
            EditorGUILayout.BeginHorizontal();

            var iterator = parentProperty.Copy();
            iterator.NextVisible(true);
            
            if (GUILayout.Button(parentProperty.displayName, GUILayout.Height(30)))
            {
                selectedSettings = parentProperty.Copy();
            }
            
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawConditionalSettingsGroup(SerializedProperty parentProperty)
        {
            EditorGUILayout.BeginHorizontal();

            var iterator = parentProperty.Copy();
            iterator.NextVisible(true);
            
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = iterator.boolValue ? Color.green : Color.red;
                    
            if (GUILayout.Button(parentProperty.displayName, style, GUILayout.Height(30)))
            {
                selectedSettings = parentProperty.Copy();
            }

            if (GUILayout.Button(iterator.boolValue ? "Turn Off" : "Turn On", 
                    GUILayout.Width(100), GUILayout.Height(30)))
            {
                iterator.boolValue = !iterator.boolValue;
            }
            
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawSelectedProperty()
        {
            EditorGUILayout.BeginVertical("box");

            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(selectedSettings.displayName, style);
            EditorGUILayout.Space(20);
            
            var iterator = selectedSettings.Copy();
            var end = iterator.GetEndProperty();
            iterator.NextVisible(true);
            while (!SerializedProperty.EqualContents(iterator, end))
            {
                if (iterator.name == GetBackingName("IsMechanicAllowed"))
                {
                    iterator.NextVisible(false);
                    continue;
                }
                
                var tooltip = GUIContentHelper.ContentWithTooltip(iterator);
                EditorGUILayout.PropertyField(iterator, tooltip, true);
                iterator.NextVisible(false);
            }
            
            EditorGUILayout.EndVertical();
        }
        
        protected bool IsConditionalSettingsGroup(SerializedProperty property)
        {
            var fieldType = GetFieldTypeFromProperty(property);
            if (fieldType == null)
            {
                return false;
            }

            while (fieldType != null)
            {
                if (Attribute.IsDefined(fieldType, typeof(ConditionalSettingsGroupAttribute)))
                {
                    return true;
                }

                fieldType = fieldType.BaseType;
            }

            return false;
        }

        private Type GetFieldTypeFromProperty(SerializedProperty property)
        {
            if (property == null)
                return null;

            var targetType = property.serializedObject.targetObject.GetType();
            var path = property.propertyPath.Replace(".Array.data[", "[");

            Type currentType = targetType;

            foreach (var element in path.Split('.'))
            {
                if (element.Contains("["))
                {
                    // Работа с массивами / списками
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var field = GetField(currentType, elementName);
                    if (field == null)
                        return null;

                    currentType = GetElementType(field.FieldType);
                }
                else
                {
                    var field = GetField(currentType, element);
                    if (field == null)
                        return null;

                    currentType = field.FieldType;
                }
            }

            return currentType;
        }

        private FieldInfo GetField(Type type, string fieldName)
        {
            while (type != null)
            {
                var field = type.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null)
                    return field;

                type = type.BaseType;
            }

            return null;
        }

        private Type GetElementType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericType)
                return type.GetGenericArguments()[0];

            return type;
        }
        
        protected string GetBackingName(string originalName)
        {
            return $"<{originalName}>k__BackingField";
        }
    }
}