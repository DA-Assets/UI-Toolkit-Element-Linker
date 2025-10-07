using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    [CustomEditor(typeof(UitkLinkerBase), true), CanEditMultipleObjects]
    public class UitkLinkerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create the root VisualElement
            var root = new VisualElement();

            // Bind the serializedObject to automatically update properties
            root.Bind(serializedObject);

            // Standard fields that are always visible
            var uiDocumentField = new PropertyField(serializedObject.FindProperty("_uiDocument"));
            var linkingModeField = new PropertyField(serializedObject.FindProperty("_linkingMode"));

            root.Add(uiDocumentField);
            root.Add(linkingModeField);

            #region Linking Mode Fields
            // Create a container for the fields that depend on the linking mode
            var linkingFieldsContainer = new VisualElement
            {
                name = "linking-fields-container",
                style = { marginTop = 5 }
            };

            var nameField = new PropertyField(serializedObject.FindProperty("_name"), "Name");
            var namesField = new PropertyField(serializedObject.FindProperty("_names"), "Names");
            var guidField = new PropertyField(serializedObject.FindProperty("_guid"), "Guid");
            var guidsField = new PropertyField(serializedObject.FindProperty("_guids"), "Guids");

            linkingFieldsContainer.Add(nameField);
            linkingFieldsContainer.Add(namesField);
            linkingFieldsContainer.Add(guidField);
            linkingFieldsContainer.Add(guidsField);
            root.Add(linkingFieldsContainer);

            // Function to update visibility based on linking mode
            void UpdateLinkingFields(UitkLinkingMode mode)
            {
                nameField.style.display = (mode == UitkLinkingMode.Name) ? DisplayStyle.Flex : DisplayStyle.None;
                namesField.style.display = (mode == UitkLinkingMode.IndexNames) ? DisplayStyle.Flex : DisplayStyle.None;
                guidField.style.display = (mode == UitkLinkingMode.Guid) ? DisplayStyle.Flex : DisplayStyle.None;
                guidsField.style.display = (mode == UitkLinkingMode.Guids) ? DisplayStyle.Flex : DisplayStyle.None;
            }

            // Register a callback to update fields when the enum value changes
            linkingModeField.RegisterValueChangeCallback(evt =>
            {
                UpdateLinkingFields((UitkLinkingMode)evt.changedProperty.enumValueIndex);
            });

            // Set the initial state
            UpdateLinkingFields((UitkLinkingMode)serializedObject.FindProperty("_linkingMode").enumValueIndex);
            #endregion

            #region Binding Fields
            // Check if the target component supports binding
            if (IsBinder(target.GetType()))
            {
                var bindingEnabledToggle = new PropertyField(serializedObject.FindProperty("_bindingEnabled"));
                bindingEnabledToggle.style.marginTop = 10;

                var bindingDetailsContainer = new VisualElement
                {
                    name = "binding-details-container",
                    style = { marginLeft = 10, marginTop = 5 } // Indent binding details
                };

                var bindingRateField = new PropertyField(serializedObject.FindProperty("_bindingRateMs"));
                var bindingSettingsField = new PropertyField(serializedObject.FindProperty("_bindingSettings"));
                var bindingField = new PropertyField(serializedObject.FindProperty("_binding"));

                bindingDetailsContainer.Add(bindingRateField);
                bindingDetailsContainer.Add(bindingSettingsField);
                bindingDetailsContainer.Add(bindingField);

                root.Add(bindingEnabledToggle);
                root.Add(bindingDetailsContainer);

                // Function to update visibility of binding details
                void UpdateBindingFields(bool isEnabled)
                {
                    bindingDetailsContainer.style.display = isEnabled ? DisplayStyle.Flex : DisplayStyle.None;
                }

                // Register callback for the toggle
                bindingEnabledToggle.RegisterValueChangeCallback(evt =>
                {
                    UpdateBindingFields(evt.changedProperty.boolValue);
                });

                // Set initial state for binding details
                UpdateBindingFields(serializedObject.FindProperty("_bindingEnabled").boolValue);
            }
            #endregion

            // Add the debug field at the end
            var debugField = new PropertyField(serializedObject.FindProperty("_debug"));
            debugField.style.marginTop = 10;
            root.Add(debugField);

            return root;
        }

        /// <summary>
        /// Checks if the specified type inherits from the generic UitkLinker<,> for data binding.
        /// </summary>
        private bool IsBinder(Type type)
        {
            if (type == null || type == typeof(object))
            {
                return false;
            }

            Type currentType = type;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(UitkLinker<,>))
                {
                    return true;
                }
                currentType = currentType.BaseType;
            }

            return false;
        }
    }

    [CustomPropertyDrawer(typeof(ElementIndexName))]
    public class ElementIndexNameDrawer : PropertyDrawer
    {
        static readonly ElementIndexName def = default;

        const int padding = 2;
        const float spacing = 10;
        const float indexFieldWidth = 30;
        const float labelWidth = 50;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUIUtility.labelWidth = labelWidth;

            string indexName = nameof(def.Index);
            string nameName = nameof(def.Name);

            Rect indexRect = new Rect(position.x, position.y, indexFieldWidth + labelWidth, position.height);
            EditorGUI.PropertyField(indexRect, property.FindPropertyRelative(indexName), new GUIContent(indexName));

            float nameFieldWidth = position.width - indexFieldWidth - labelWidth - padding - spacing;

            Rect nameRect = new Rect(position.x + indexFieldWidth + labelWidth + padding + spacing, position.y, nameFieldWidth, position.height);
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative(nameName), new GUIContent(nameName));

            EditorGUIUtility.labelWidth = 0;
            EditorGUI.indentLevel = indent;

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
    }
}