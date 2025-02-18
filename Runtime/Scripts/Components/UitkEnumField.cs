#if UNITY_2022_1_OR_NEWER
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkEnumField : UitkLinker<EnumField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(EnumFieldG))]
    public partial class EnumFieldG : UnityEngine.UIElements.EnumField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public EnumFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class EnumFieldG : UnityEngine.UIElements.EnumField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<EnumFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Enum>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            internal static readonly UxmlTypeAttributeDescription<Enum> type = new UxmlTypeAttributeDescription<Enum>
            {
                name = "type"
            };

            internal static readonly UxmlStringAttributeDescription value = new UxmlStringAttributeDescription
            {
                name = "value"
            };

            internal static readonly UxmlBoolAttributeDescription includeObsoleteValues = new UxmlBoolAttributeDescription
            {
                name = "include-obsolete-values",
                defaultValue = false
            };

            private UxmlTypeAttributeDescription<Enum> m_Type = type;

            private UxmlStringAttributeDescription m_Value = value;

            private UxmlBoolAttributeDescription m_IncludeObsoleteValues = includeObsoleteValues;

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ExtractValue(bag, cc, out var resEnumType, out var resEnumValue, out var resIncludeObsoleteValues))
                {
                    EnumFieldG enumField = (EnumFieldG)ve;
                    enumField.Init(resEnumValue, resIncludeObsoleteValues);
                }
                else if (null != resEnumType)
                {
                    EnumFieldG enumField2 = (EnumFieldG)ve;

                    FieldInfo mEnumTypeField = typeof(UnityEngine.UIElements.EnumField).GetField("m_EnumType", BindingFlags.NonPublic | BindingFlags.Instance);
                    mEnumTypeField.SetValue(enumField2, resEnumType);

                    object mEnumTypeValue = mEnumTypeField.GetValue(enumField2);

                    if (mEnumTypeValue != null)
                    {
                        MethodInfo populateDataFromTypeMethod = typeof(UnityEngine.UIElements.EnumField).GetMethod("PopulateDataFromType", BindingFlags.NonPublic | BindingFlags.Instance);
                        populateDataFromTypeMethod.Invoke(enumField2, new object[] { mEnumTypeValue });
                    }

                    enumField2.value = null;
                }
                else
                {
                    EnumFieldG enumField3 = (EnumFieldG)ve;

                    FieldInfo mEnumTypeField = typeof(UnityEngine.UIElements.EnumField).GetField("m_EnumType", BindingFlags.NonPublic | BindingFlags.Instance);
                    mEnumTypeField.SetValue(enumField3, null);

                    enumField3.value = null;
                }

                EnumFieldG obj = ve as EnumFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }

            private static bool ExtractValue(IUxmlAttributes bag, CreationContext cc, out Type resEnumType, out Enum resEnumValue, out bool resIncludeObsoleteValues)
            {
                resIncludeObsoleteValues = false;
                resEnumValue = null;
                resEnumType = type.GetValueFromBag(bag, cc);
                if (resEnumType == null)
                {
                    return false;
                }

                string text = null;
                object result = null;
                if (value.TryGetValueFromBag(bag, cc, ref text) && !Enum.TryParse(resEnumType, text, ignoreCase: false, out result))
                {
                    Debug.LogErrorFormat("EnumField: Could not parse value of '{0}', because it isn't defined in the {1} enum.", text, resEnumType.FullName);
                    return false;
                }

                resEnumValue = ((text != null && result != null) ? ((Enum)result) : ((Enum)Enum.ToObject(resEnumType, 0)));
                resIncludeObsoleteValues = includeObsoleteValues.GetValueFromBag(bag, cc);
                return true;
            }
        }
    }
#endif
}
#endif