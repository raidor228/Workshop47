using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Melador.PlayerController.Editor
{
    public static class GUIContentHelper
    {
        public static GUIContent ContentWithTooltip(SerializedProperty prop, string displayName = null)
        {
            string label = displayName ?? ObjectNames.NicifyVariableName(prop.name);
            string tooltip = GetTooltipFromProperty(prop);
            return new GUIContent(label, tooltip);
        }

        private static string GetTooltipFromProperty(SerializedProperty prop)
        {
            object currentObject = prop.serializedObject.targetObject;
            Type currentType = currentObject.GetType();

            string[] parts = prop.propertyPath.Split('.');

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];

                string fieldName = part;
                if (part.StartsWith("<") && part.Contains(">"))
                {
                    int start = 1;
                    int end = part.IndexOf(">");
                    fieldName = part.Substring(start, end - start);
                }

                PropertyInfo property = currentType.GetProperty(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (property == null)
                {
                    return string.Empty;
                }

                if (i == parts.Length - 1)
                {
                    var attr = property.GetCustomAttribute<TooltipAttribute>();
                    return attr?.tooltip ?? string.Empty;
                }

                currentObject = property.GetValue(currentObject);
                if (currentObject == null)
                {
                    return string.Empty;
                }

                currentType = currentObject.GetType();
            }

            return string.Empty;
        }
    }
}