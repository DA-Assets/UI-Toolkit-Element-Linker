using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    [CustomEditor(typeof(UitkLinker<>), true), CanEditMultipleObjects]
    public class UitkLinkerEditor : Editor
    {
        UitkLinkerBase monoBeh;

        Type targetType = null;

        void OnEnable()
        {
            targetType = target.GetType();
            monoBeh = (UitkLinkerBase)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            IMGUIContainer imguiContainer = new IMGUIContainer(() =>
            {
                OnInspectorLegacy(root);
            });

            imguiContainer.style.display = DisplayStyle.Flex;
            root.Add(imguiContainer);

            return root;
        }

        public void OnInspectorLegacy(VisualElement root)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_uiDocument"));
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_linkingMode"));
            EditorGUILayout.Space(5);

            SerializedProperty linkingMode = serializedObject.FindProperty("_linkingMode");
            switch ((UitkLinkingMode)linkingMode.enumValueIndex)
            {
                case UitkLinkingMode.Name:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
                    break;
                case UitkLinkingMode.IndexNames:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_names"));
                    break;
                case UitkLinkingMode.Guid:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_guid"));
                    break;
                case UitkLinkingMode.Guids:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_guids"));
                    break;
            }

#if UNITY_2021_3_OR_NEWER
            if (target.GetType() == typeof(UitkButton))
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_onClick"));
            }
#endif

            SerializedProperty isDebug = serializedObject.FindProperty("_debug");
            if (isDebug.boolValue)
            {
                GUILayout.Space(15);
                base.OnInspectorGUI();
            }
            else
            {
                EditorGUILayout.PropertyField(isDebug);
            }

            serializedObject.ApplyModifiedProperties();
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
}