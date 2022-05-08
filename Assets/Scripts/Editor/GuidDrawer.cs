using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(GuidAttribute))]
    class GuidDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (string.IsNullOrWhiteSpace(property.stringValue))
            {
                property.stringValue = GUID.Generate().ToString();
            }

            EditorGUI.PropertyField(position, property, label);
        }
    }
}
