using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Атрибут для блокирования модификации сериализуемых полей через инспектор
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
    // Для старой отрисовки через IMGUI
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }

    // Для новой отрисовки через UIElements
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var element = base.CreatePropertyGUI(property)
                     ?? new PropertyField(property);

        element.SetEnabled(false);
        return element;
    }
}
#endif