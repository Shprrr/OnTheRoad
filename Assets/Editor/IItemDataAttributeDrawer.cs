using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IItemDataAttribute))]
public class IItemDataAttributeDrawer : PropertyDrawer
{
    private IItemDataAttribute _attributeValue = null;
    private IItemDataAttribute attributeValue
    {
        get
        {
            if (_attributeValue == null)
            {
                _attributeValue = (IItemDataAttribute)attribute;
            }
            return _attributeValue;
        }
    }

    enum ItemDataType
    {
        NULL = 0,
        ItemUsable,
        Equipable,
        Weapon,
        Armor,
        Undefined = ~0
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var fieldInfoReal = fieldInfo.DeclaringType.GetField(attributeValue.MemberName);
        var realValue = fieldInfoReal.GetValue(property.serializedObject.targetObject) as IItemData;
        var realArray = fieldInfoReal.GetValue(property.serializedObject.targetObject) as IItemData[];

        if (fieldInfoReal.FieldType == typeof(IItemData))
            return GetPropertyHeightSingle(realValue);
        else
        {
            var height = EditorGUIUtility.singleLineHeight * 2;
            if (realArray != null)
                for (int i = 0; i < realArray.Length; i++)
                {
                    height += GetPropertyHeightSingle(realArray[i]);
                }

            return height;
        }
    }

    private float GetPropertyHeightSingle(IItemData realValue)
    {
        if (realValue != null && !realValue.foldout) return EditorGUIUtility.singleLineHeight;

        switch (GetItemDataType(realValue))
        {
            case ItemDataType.ItemUsable:
                var o = ScriptableObject.CreateInstance<ScriptableItemUsable>();
                o.TargetsPossible = ((ItemUsableData)realValue).TargetsPossible;
                var so = new SerializedObject(o);
                var sp = so.FindProperty(nameof(o.TargetsPossible));

                return EditorGUIUtility.singleLineHeight * 10 + EditorGUI.GetPropertyHeight(sp, true);
            case ItemDataType.Equipable:
                return EditorGUIUtility.singleLineHeight * 7;
            case ItemDataType.Weapon:
                return EditorGUIUtility.singleLineHeight * 7;
            case ItemDataType.Armor:
                return EditorGUIUtility.singleLineHeight * 7;
            case ItemDataType.NULL:
            case ItemDataType.Undefined:
            default:
                return EditorGUIUtility.singleLineHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var fieldInfoReal = fieldInfo.DeclaringType.GetField(attributeValue.MemberName);
        var realValue = fieldInfoReal.GetValue(property.serializedObject.targetObject) as IItemData;
        var realArray = fieldInfoReal.GetValue(property.serializedObject.targetObject) as IItemData[];

        if (fieldInfoReal.FieldType == typeof(IItemData))
            OnGUISingle(position, ObjectNames.NicifyVariableName(attributeValue.MemberName), ref realValue, () =>
            {
                fieldInfoReal.SetValue(property.serializedObject.targetObject, realValue);
                //property.stringValue = realValue?.Serialize();
                property.stringValue = realValue?.ToXML();
                property.serializedObject.ApplyModifiedProperties();
            });
        else
        {
            var pos = EditorGUI.IndentedRect(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight));
            EditorGUI.Foldout(pos, true, ObjectNames.NicifyVariableName(attributeValue.MemberName), true);

            EditorGUI.indentLevel++;
            pos.y += pos.height;
            pos = EditorGUI.IndentedRect(pos);
            EditorGUI.indentLevel--;

            if (realArray == null)
            {
                realArray = new IItemData[0];
                fieldInfoReal.SetValue(property.serializedObject.targetObject, realArray);
            }

            EditorGUI.BeginChangeCheck();
            int length = EditorGUI.IntField(pos, "Size", realArray.Length);
            if (EditorGUI.EndChangeCheck())
            {
                System.Array.Resize(ref realArray, length);
                fieldInfoReal.SetValue(property.serializedObject.targetObject, realArray);
                property.stringValue = realArray.ToXML();
                property.serializedObject.ApplyModifiedProperties();
            }

            var heightLast = pos.height;
            for (int i = 0; i < realArray.Length; i++)
            {
                pos.y += heightLast;
                OnGUISingle(pos, realArray[i]?.Id ?? "Element " + i, ref realArray[i], () =>
                {
                    fieldInfoReal.SetValue(property.serializedObject.targetObject, realArray);
                    property.stringValue = realArray.ToXML();
                    property.serializedObject.ApplyModifiedProperties();
                });
                heightLast = GetPropertyHeightSingle(realArray[i]);
            }
        }
    }

    private void OnGUISingle(Rect position, string label, ref IItemData realValue, System.Action actionSaveValue)
    {
        var pos = EditorGUI.IndentedRect(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight));
        if (realValue == null)
            pos = EditorGUI.PrefixLabel(pos, new GUIContent(label));
        else
        {
            EditorGUI.BeginChangeCheck();
            realValue.foldout = EditorGUI.Foldout(pos, realValue.foldout, label, true);
            if (EditorGUI.EndChangeCheck())
                actionSaveValue?.Invoke();
            if (!realValue.foldout) return;

            EditorGUI.indentLevel++;
            pos.y += pos.height;
            pos = EditorGUI.IndentedRect(pos);
            EditorGUI.indentLevel--;
        }

        // Choose a type.
        EditorGUI.BeginChangeCheck();
        System.Enum type = GetItemDataType(realValue);
        type = EditorGUI.EnumPopup(pos, type);
        if (EditorGUI.EndChangeCheck())
        {
            realValue = ChangeType((ItemDataType)type, realValue);
            actionSaveValue?.Invoke();
        }

        if (realValue == null) return;

        // Fields for properties from interface.
        pos.y += pos.height;
        EditorGUI.BeginChangeCheck();
        realValue.Id = EditorGUI.TextField(pos, "Id", realValue.Id);
        if (EditorGUI.EndChangeCheck())
            actionSaveValue?.Invoke();

        pos.y += pos.height;
        EditorGUI.BeginChangeCheck();
        realValue.Name = EditorGUI.TextField(pos, "Name", realValue.Name);
        if (EditorGUI.EndChangeCheck())
            actionSaveValue?.Invoke();

        pos.y += pos.height;
        EditorGUI.BeginChangeCheck();
        realValue.Description = EditorGUI.TextField(pos, "Description", realValue.Description);
        if (EditorGUI.EndChangeCheck())
            actionSaveValue?.Invoke();

        pos.y += pos.height;
        EditorGUI.BeginChangeCheck();
        realValue.Amount = EditorGUI.IntField(pos, "Amount", realValue.Amount);
        if (EditorGUI.EndChangeCheck())
            actionSaveValue?.Invoke();

        pos.y += pos.height;
        EditorGUI.BeginChangeCheck();
        realValue.Price = EditorGUI.IntField(pos, "Price", realValue.Price);
        if (EditorGUI.EndChangeCheck())
            actionSaveValue?.Invoke();

        if (realValue is ItemUsableData)
        {
            var itemUsable = (ItemUsableData)realValue;

            pos.y += pos.height;
            EditorGUI.BeginChangeCheck();
            itemUsable.AnimationName = EditorGUI.TextField(pos, ObjectNames.NicifyVariableName("AnimationName"), itemUsable.AnimationName);
            if (EditorGUI.EndChangeCheck())
                actionSaveValue?.Invoke();

            var o = ScriptableObject.CreateInstance<ScriptableItemUsable>();
            o.TargetsPossible = itemUsable.TargetsPossible;
            var so = new SerializedObject(o);
            var sp = so.FindProperty(nameof(o.TargetsPossible));
            pos.y += EditorGUIUtility.singleLineHeight;
            pos.height = EditorGUI.GetPropertyHeight(sp, true);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(pos, sp, true);
            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
                itemUsable.TargetsPossible = o.TargetsPossible;
                actionSaveValue?.Invoke();
            }

            pos.y += pos.height;
            pos.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(pos, "Effect", itemUsable.Effect?.ToString());

            pos.y += pos.height;
            EditorGUI.BeginChangeCheck();
            itemUsable.UsableOutsideBattle = EditorGUI.Toggle(pos, ObjectNames.NicifyVariableName("UsableOutsideBattle"), itemUsable.UsableOutsideBattle);
            if (EditorGUI.EndChangeCheck())
                actionSaveValue?.Invoke();
        }
    }

    class ScriptableItemUsable : ScriptableObject
    {
        public Cursor.eTargetType[] TargetsPossible;
    }

    private ItemDataType GetItemDataType(IItemData itemData)
    {
        if (itemData == null) return ItemDataType.NULL;

        var type = itemData.GetType();

        if (type == typeof(ItemUsableData)) return ItemDataType.ItemUsable;
        if (type == typeof(EquipableData)) return ItemDataType.Equipable;
        if (type == typeof(WeaponData)) return ItemDataType.Weapon;
        if (type == typeof(ArmorData)) return ItemDataType.Armor;
        return ItemDataType.Undefined;
    }

    private IItemData ChangeType(ItemDataType type, IItemData oldValue)
    {
        IItemData newValue;
        switch (type)
        {
            case ItemDataType.ItemUsable:
                newValue = new ItemUsableData();
                break;
            case ItemDataType.Equipable:
                newValue = new EquipableData();
                break;
            case ItemDataType.Weapon:
                newValue = new WeaponData();
                break;
            case ItemDataType.Armor:
                newValue = new ArmorData();
                break;
            case ItemDataType.Undefined:
            case ItemDataType.NULL:
            default:
                return null;
        }

        if (oldValue != null)
        {
            newValue.Id = oldValue.Id;
            newValue.Name = oldValue.Name;
            newValue.Description = oldValue.Description;
            newValue.Amount = oldValue.Amount;
            newValue.Price = oldValue.Price;
        }
        newValue.foldout = true;

        return newValue;
    }
}

static class Extension
{
    public static string ToXML<T>(this T o)
    {
        //var extraTypes = t.GetProperties()
        //    .Where(p => p.PropertyType.IsInterface)
        //    .Select(p => p.GetValue(o, null).GetType())
        //    .ToArray();
        var extraTypes = typeof(T).Assembly.GetTypes().Where(t => typeof(IItemData).IsAssignableFrom(t) || typeof(Effect).IsAssignableFrom(t)).ToArray();

        var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), extraTypes);
        var sw = new System.IO.StringWriter();
        var xw = new System.Xml.XmlTextWriter(sw);
        serializer.WriteObject(xw, o);
        return sw.ToString();
    }

    public static T FromXML<T>(this string str)
    {
        var extraTypes = typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t)).ToArray();

        var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), extraTypes);
        var sr = new System.IO.StringReader(str);
        var xr = new System.Xml.XmlTextReader(sr);
        var o = serializer.ReadObject(xr);
        return (T)o;
    }
}
