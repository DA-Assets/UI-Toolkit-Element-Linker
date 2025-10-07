using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DA_Assets.ULB
{
    [Serializable]
    public struct MemberBinding
    {
        [SerializeField] public GameObject TargetObject;
        [SerializeField] public string Member;

        private Component _cachedComponent;
        private List<MemberWrapper> _MemberChain;

        private struct MemberWrapper
        {
            public PropertyInfo PropertyInfo;
            public FieldInfo FieldInfo;
            public bool IsStatic;

            public MemberWrapper(PropertyInfo propertyInfo, FieldInfo fieldInfo, bool isStatic)
            {
                PropertyInfo = propertyInfo;
                FieldInfo = fieldInfo;
                IsStatic = isStatic;
            }
        }

        public void SetFieldPropInfo()
        {
            string methodPath = $"{typeof(MemberBinding).Name}.{nameof(SetFieldPropInfo)}";

            if (string.IsNullOrWhiteSpace(Member))
            {
                Debug.LogError($"{methodPath} | Property or field path is empty.");
                return;
            }

            string[] MemberPathParts = Member.Split(UitkConstants.FIELD_SEPARATOR);

            if (MemberPathParts.Length < 1)
                return;

            string componentName = MemberPathParts[0];

            if (string.IsNullOrWhiteSpace(componentName))
            {
                Debug.LogError($"{methodPath} | Component name is empty.");
                return;
            }

            _cachedComponent = TargetObject.GetComponent(componentName);

            if (_cachedComponent == null)
            {
                Debug.LogError($"{methodPath} | Component '{componentName}' not found on GameObject '{TargetObject.name}'");
                return;
            }

            Type currentType = _cachedComponent.GetType();
            _MemberChain = new List<MemberWrapper>();

            for (int i = 1; i < MemberPathParts.Length; i++)
            {
                string MemberName = MemberPathParts[i];

                FieldInfo fieldInfo = currentType.GetField(MemberName, UitkConstants.BINDING_FLAGS);

                if (fieldInfo != null)
                {
                    bool isStatic = fieldInfo.IsStatic;
                    _MemberChain.Add(new MemberWrapper(null, fieldInfo, isStatic));
                    currentType = fieldInfo.FieldType;
                }
                else
                {
                    PropertyInfo propertyInfo = currentType.GetProperty(MemberName, UitkConstants.BINDING_FLAGS);

                    if (propertyInfo != null && propertyInfo.CanRead)
                    {
                        MethodInfo getMethod = propertyInfo.GetGetMethod(true);
                        bool isStatic = getMethod.IsStatic;
                        _MemberChain.Add(new MemberWrapper(propertyInfo, null, isStatic));
                        currentType = propertyInfo.PropertyType;
                    }
                    else
                    {
                        Debug.LogError($"{methodPath} | Field or property '{MemberName}' not found on type '{currentType.Name}'");
                        break;
                    }
                }
            }
        }

        public object GetValue()
        {
            string methodPath = $"{typeof(MemberBinding).Name}.{nameof(GetValue)}";

            if (_cachedComponent == null || _MemberChain == null)
            {
                Debug.LogError($"{methodPath} | Cached data is null. Ensure that SetFieldInfo() is called before GetValue().");
                return null;
            }

            object currentObject = null;

            for (int i = 0; i < _MemberChain.Count; i++)
            {
                MemberWrapper memberWrapper = _MemberChain[i];
                bool isStatic = memberWrapper.IsStatic;

                if (i == 0)
                {
                    currentObject = isStatic ? null : _cachedComponent;
                }

                if (memberWrapper.FieldInfo != null)
                {
                    currentObject = memberWrapper.FieldInfo.GetValue(isStatic ? null : currentObject);
                }
                else if (memberWrapper.PropertyInfo != null)
                {
                    currentObject = memberWrapper.PropertyInfo.GetValue(isStatic ? null : currentObject);
                }

                if (currentObject == null && i < _MemberChain.Count - 1)
                {
                    break;
                }
            }

            return currentObject;
        }
    }
}