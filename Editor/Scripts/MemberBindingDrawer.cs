using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DA_Assets.ULB
{
    [CustomPropertyDrawer(typeof(MemberBinding))]
    public class MemberBindingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty rootProperty, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, rootProperty);

            UnityEngine.Object targetOwner = rootProperty.serializedObject.targetObject;
            FieldInfo settingsFieldInfo = targetOwner.GetType().GetField("_bindingSettings", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            UitkBindingSettings bindingSettings = settingsFieldInfo?.GetValue(targetOwner) as UitkBindingSettings;

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float padding = 3f;

            // Set targetObject field to take 1/3 of the width
            float targetObjectWidth = position.width / 3;

            // Target object field on the left
            Rect targetObjectRect = new Rect(
                position.x,
                position.y,
                targetObjectWidth - padding,
                singleLineHeight);

            SerializedProperty targetObjectProp = rootProperty.FindPropertyRelative(nameof(MemberBinding.TargetObject));

            EditorGUI.PropertyField(targetObjectRect, targetObjectProp, GUIContent.none);

            Rect fieldRect = new Rect(
                position.x + targetObjectWidth + padding,
                position.y,
                position.width - targetObjectWidth - 2 * padding,
                singleLineHeight);

            // Field selection button on the right
            if (targetObjectProp.objectReferenceValue != null)
            {
                GameObject targetObject = targetObjectProp.objectReferenceValue as GameObject;

                if (targetObject != null)
                {
                    SerializedProperty selectedFieldProp = rootProperty.FindPropertyRelative(nameof(MemberBinding.Member));

                    string buttonLabel = string.IsNullOrEmpty(selectedFieldProp.stringValue) ? UitkConstants.NO_FIELD_PROP : selectedFieldProp.stringValue;
                    bool fieldExists = PropertyOrFieldExists(targetObject, selectedFieldProp.stringValue);

                    if (!fieldExists && !string.IsNullOrEmpty(selectedFieldProp.stringValue))
                    {
                        buttonLabel = string.Format(UitkConstants.MISSING_FIELD_PROP, selectedFieldProp.stringValue);
                    }

                    if (GUI.Button(fieldRect, buttonLabel, EditorStyles.popup))
                    {
                        OpenGenericMenu_OnClick(selectedFieldProp, rootProperty, targetObject, fieldRect, bindingSettings);
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(fieldRect, " Assign a GameObject with a MonoBehaviour script.");
            }

            EditorGUI.EndProperty();
        }

        private void OpenGenericMenu_OnClick(
            SerializedProperty selectedFieldProp,
            SerializedProperty rootProperty,
            GameObject targetObject,
            Rect fieldRect,
            UitkBindingSettings settings)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent(UitkConstants.NO_FIELD_PROP), string.IsNullOrEmpty(selectedFieldProp.stringValue), () =>
            {
                selectedFieldProp.stringValue = string.Empty;
                rootProperty.serializedObject.ApplyModifiedProperties();
            });

            Component[] components = targetObject.GetComponents<Component>();

            foreach (Component component in components)
            {
                List<PathSegment> initialPath = new List<PathSegment>
                {
                    new PathSegment(component.GetType().Name, component.GetType())
                };

                AddComponentMembersToMenu(menu, component.GetType(), initialPath, selectedFieldProp, rootProperty, 2, settings);
            }

            menu.DropDown(fieldRect);
        }

        private void AddComponentMembersToMenu(
            GenericMenu menu,
            Type type,
            List<PathSegment> parentPath,
            SerializedProperty selectedFieldProp,
            SerializedProperty rootProperty,
            int depth,
            UitkBindingSettings settings)
        {
            IEnumerable<MemberInfo> members = GetPropertiesOrFields(type, UitkConstants.BINDING_FLAGS);
            members = members.OrderBy(m => m.Name);

            int maxDepth = settings != null ? settings.MaxDepth : UitkConstants.RECOMENDED_DEPTH;
            bool showToString = settings != null ? settings.ShowToString : UitkConstants.SHOW_TOSTRING;
            bool showObsolete = settings != null ? settings.ShowObsolete : UitkConstants.SHOW_OBSOLETE;
            bool showPrimitiveType = settings != null ? settings.ShowPrimitiveType : UitkConstants.SHOW_PRIMITIVE_TYPE;
            bool showObjectType = settings != null ? settings.ShowObjectType : UitkConstants.SHOW_OBJECT_TYPE;

            bool isLastSegment = IsPrimitive(type) || depth > maxDepth;
            bool toString = showToString && IsSupportFormating(type);

            string currentPathString = string.Join(UitkConstants.FIELD_SEPARATOR.ToString(), parentPath.Select(p => p.FieldName));
            string currentLabel = string.Join(UitkConstants.FIELD_SEPARATOR.ToString(), parentPath.Select(p => GetSegmentName(p, showPrimitiveType, showObjectType)));

            if (!isLastSegment && showToString && IsSupportFormating(type))
            {
                string newLabel = $"{currentLabel} [ToString]";
                menu.AddItem(new GUIContent(newLabel), IsOn(selectedFieldProp, currentPathString), () =>
                {
                    SetPath(selectedFieldProp, rootProperty, currentPathString);
                });
            }

            if (isLastSegment)
            {
                menu.AddItem(new GUIContent(currentLabel), IsOn(selectedFieldProp, currentPathString), () =>
                {
                    SetPath(selectedFieldProp, rootProperty, currentPathString);
                });

                return;
            }

            foreach (MemberInfo member in members)
            {
                if (!member.GetCanRead())
                {
                    continue;
                }

                if (showObsolete)
                {
                    //Only hide Obsolete(isError: true) fields, as it is impossible to get values from them.
                    if (IsObsoleteError(member))
                    {
                        continue;
                    }
                }
                else
                {
                    //Hide all Obsolete fields.
                    if (HasObsoleteAttribute(member))
                    {
                        continue;
                    }
                }

                Type memberType = GetMemberType(member);

                List<PathSegment> newPath = new List<PathSegment>(parentPath)
                {
                    new PathSegment(member.Name, memberType)
                };

                AddComponentMembersToMenu(menu, memberType, newPath, selectedFieldProp, rootProperty, depth + 1, settings);
            }
        }

        /// <summary>
        /// Checks if an item in the GenericMenu is marked.
        /// </summary>
        /// <returns>True if paths match, otherwise false.</returns>
        private bool IsOn(SerializedProperty selectedFieldProp, string path)
        {
            return selectedFieldProp.stringValue == path;
        }

        /// <summary>
        /// Stores and serializes the path to the property or field to which the binding should be applied.
        /// </summary>
        private void SetPath(SerializedProperty selectedFieldProp, SerializedProperty rootProperty, string path)
        {
            selectedFieldProp.stringValue = path;
            rootProperty.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Check whether the field or property still exists.
        /// This is necessary in case the field has been deleted or renamed.
        /// </summary>
        private bool PropertyOrFieldExists(GameObject targetObject, string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                return false;

            string[] parts = memberName.Split(UitkConstants.FIELD_SEPARATOR);
            if (parts.Length < 1)
                return false;

            Component currentComponent = targetObject.GetComponent(parts[0]);
            if (currentComponent == null)
                return false;

            if (parts.Length == 1)
                return true;

            Type currentType = currentComponent.GetType();

            for (int i = 1; i < parts.Length; i++)
            {
                string memberPart = parts[i];

                FieldInfo fieldInfo = currentType.GetField(memberPart, UitkConstants.BINDING_FLAGS);
                if (fieldInfo != null && !IsObsoleteError(fieldInfo))
                {
                    currentType = fieldInfo.FieldType;
                    continue;
                }

                PropertyInfo propertyInfo = currentType.GetProperty(memberPart, UitkConstants.BINDING_FLAGS);
                if (propertyInfo != null && propertyInfo.CanRead && !IsObsoleteError(propertyInfo))
                {
                    currentType = propertyInfo.PropertyType;
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the member has the <see cref="ObsoleteAttribute"/>.
        /// </summary>
        /// <returns>Returns true if the Obsolete attribute is found; otherwise, false.</returns>
        private bool HasObsoleteAttribute(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                return false;

            return memberInfo.GetCustomAttribute<ObsoleteAttribute>() != null;
        }

        /// <summary>
        /// Checks whether the member has the <see cref="ObsoleteAttribute"/> with the IsError parameter set to true.
        /// </summary>
        /// <returns>Returns true if the Obsolete attribute with IsError=true is found; otherwise, false.</returns>
        private bool IsObsoleteError(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                return false;

            ObsoleteAttribute obsoleteAttribute = memberInfo.GetCustomAttribute<ObsoleteAttribute>();
            return obsoleteAttribute != null && obsoleteAttribute.IsError;
        }

        /// <summary>
        /// Determines if the given <see cref="Type"/> is a primitive type.
        /// <para><see href="https://stackoverflow.com/a/2442544"/></para>
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a primitive, string, or decimal; otherwise, false.</returns>
        private static bool IsPrimitive(Type type)
        {
            if (type == null)
                return false;

            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        /// <summary>
        /// <para>Examples: <see href="https://docs.unity3d.com/ScriptReference/30_search.html?q=ToString"/></para>
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if Unity supports auto-formatting for the given <see cref="Type"/>; otherwise, false.</returns>
        private static bool IsSupportFormating(Type type)
        {
            return _autoFormatedTypes.Contains(type);
        }

        private string GetSegmentName(PathSegment ps, bool showPrimitiveType, bool showObjectType)
        {
            string label;

            if (IsPrimitive(ps.FieldType) && showPrimitiveType)
            {
                label = $"{ps.FieldName} [{GetTypeName(ps.FieldType)}]";
            }
            else if (!IsPrimitive(ps.FieldType) && (ps.FieldType.IsClass || ps.FieldType.IsValueType) && showObjectType)
            {
                label = $"{ps.FieldName} [{ps.FieldType.Name}]";
            }
            else
            {
                label = ps.FieldName;
            }

            string GetTypeName(Type type)
            {
                if (type == typeof(float))
                {
                    return "float";
                }
                else if (type == typeof(bool))
                {
                    return "bool";
                }
                else if (type == typeof(int))
                {
                    return "int";
                }
                else
                {
                    return type.Name.ToLowerInvariant();
                }
            }

            return label;
        }

        /// <summary>
        /// Used for <see cref="IsSupportFormating"/>
        /// </summary>
        private static readonly HashSet<System.Type> _autoFormatedTypes = new HashSet<System.Type>
        {
            typeof(Ray),
            typeof(Rect),
            typeof(Color),
            typeof(Ray2D),
            typeof(Bounds),
            typeof(Color32),
            typeof(Hash128),
            typeof(RectInt),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(BoundsInt),
            typeof(Matrix4x4),
            typeof(Quaternion),
            typeof(RectOffset),
            typeof(Resolution),
            typeof(Vector2Int),
            typeof(Vector3Int),
        };

        /// <summary>
        /// Stores information about the path segment for the GenericMenu.  
        /// <para>Example: {<see cref="PathSegment"/>}/{<see cref="PathSegment"/>}/{<see cref="PathSegment"/>}</para>
        /// </summary>
        struct PathSegment
        {
            private string _fieldName;
            private Type _fieldType;

            public string FieldName => _fieldName;
            public Type FieldType => _fieldType;

            public PathSegment(string fieldName, Type fieldType)
            {
                _fieldName = fieldName;
                _fieldType = fieldType;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }


        private static IEnumerable<MemberInfo> GetPropertiesOrFields(
            Type t,
            BindingFlags bf = BindingFlags.Public | BindingFlags.Instance) =>
                t.GetMembers(bf)
                .Where(mi => mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property);

        private static Type GetMemberType(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo mfi:
                    return mfi.FieldType;
                case PropertyInfo mpi:
                    return mpi.PropertyType;
                case EventInfo mei:
                    return mei.EventHandlerType;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", nameof(member));
            }
        }
    }
}