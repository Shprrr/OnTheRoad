using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Characteristic))]
public class CharacteristicPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CreateDisplayOptions();

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var idRect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        //EditorGUI.PropertyField(idRect, property.FindPropertyRelative(nameof(Characteristic._id)), GUIContent.none);

        var idProperty = property.FindPropertyRelative(nameof(Characteristic._id));
        var indexValue = Array.FindIndex(displayOptions, o => o.tooltip == idProperty.stringValue);
        EditorGUI.BeginChangeCheck();
        indexValue = EditorGUI.Popup(idRect, GUIContent.none, indexValue, displayOptions);
        if(EditorGUI.EndChangeCheck())
            property.SetValue(CharacteristicFactory.Build(displayOptions[indexValue].tooltip));

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    private static GUIContent[] displayOptions;
    private void CreateDisplayOptions()
    {
        if (displayOptions != null) return;

        displayOptions = CharacteristicFactory.GetEveryIds().Select(i => new GUIContent(CharacteristicFactory.Build(i).Name, i)).ToArray();
    }
}
