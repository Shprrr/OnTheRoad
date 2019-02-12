using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyPropertyAttribute))]
public class ReadOnlyPropertyAttributeDrawer : PropertyDrawer
{
    private ReadOnlyPropertyAttribute _attributeValue = null;
    private ReadOnlyPropertyAttribute attributeValue
    {
        get
        {
            if (_attributeValue == null)
            {
                _attributeValue = (ReadOnlyPropertyAttribute)attribute;
            }
            return _attributeValue;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propertyInfoReal = fieldInfo.DeclaringType.GetProperty(attributeValue.MemberName);
        var value = propertyInfoReal.GetValue(GetParent(property));
        EditorGUI.LabelField(position, label.text, value.ToString());

        // Change field value to serialize the real value.
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
                break;
            case SerializedPropertyType.Integer:
                property.intValue = (int)value;
                break;
            case SerializedPropertyType.Boolean:
                property.boolValue = (bool)value;
                break;
            case SerializedPropertyType.Float:
                property.floatValue = (float)value;
                break;
            case SerializedPropertyType.String:
                property.stringValue = value.ToString();
                break;
            case SerializedPropertyType.Color:
                break;
            case SerializedPropertyType.ObjectReference:
                break;
            case SerializedPropertyType.LayerMask:
                break;
            case SerializedPropertyType.Enum:
                break;
            case SerializedPropertyType.Vector2:
                break;
            case SerializedPropertyType.Vector3:
                break;
            case SerializedPropertyType.Vector4:
                break;
            case SerializedPropertyType.Rect:
                break;
            case SerializedPropertyType.ArraySize:
                break;
            case SerializedPropertyType.Character:
                break;
            case SerializedPropertyType.AnimationCurve:
                break;
            case SerializedPropertyType.Bounds:
                break;
            case SerializedPropertyType.Gradient:
                break;
            case SerializedPropertyType.Quaternion:
                break;
            case SerializedPropertyType.ExposedReference:
                break;
            case SerializedPropertyType.FixedBufferSize:
                break;
            case SerializedPropertyType.Vector2Int:
                break;
            case SerializedPropertyType.Vector3Int:
                break;
            case SerializedPropertyType.RectInt:
                break;
            case SerializedPropertyType.BoundsInt:
                break;
        }

        property.serializedObject.ApplyModifiedProperties();
    }

    public object GetParent(SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements.Take(elements.Length - 1))
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }
        return obj;
    }

    public object GetValue(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();
        var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null)
        {
            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null)
                return null;
            return p.GetValue(source, null);
        }
        return f.GetValue(source);
    }

    public object GetValue(object source, string name, int index)
    {
        var enumerable = GetValue(source, name) as IEnumerable;
        var enm = enumerable.GetEnumerator();
        while (index-- >= 0)
            enm.MoveNext();
        return enm.Current;
    }
}