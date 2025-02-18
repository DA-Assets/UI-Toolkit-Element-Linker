#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkVector4Field : UitkLinker<Vector4Field> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(Vector4FieldG))]
    public partial class Vector4FieldG : UnityEngine.UIElements.Vector4Field, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public Vector4FieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class Vector4FieldG : UnityEngine.UIElements.Vector4Field, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<Vector4FieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Vector4>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlFloatAttributeDescription m_XValue = new UxmlFloatAttributeDescription
            {
                name = "x"
            };

            private UxmlFloatAttributeDescription m_YValue = new UxmlFloatAttributeDescription
            {
                name = "y"
            };

            private UxmlFloatAttributeDescription m_ZValue = new UxmlFloatAttributeDescription
            {
                name = "z"
            };

            private UxmlFloatAttributeDescription m_WValue = new UxmlFloatAttributeDescription
            {
                name = "w"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                Vector4FieldG obj = (Vector4FieldG)ve;
                obj.SetValueWithoutNotify(new Vector4(m_XValue.GetValueFromBag(bag, cc), m_YValue.GetValueFromBag(bag, cc), m_ZValue.GetValueFromBag(bag, cc), m_WValue.GetValueFromBag(bag, cc)));

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif