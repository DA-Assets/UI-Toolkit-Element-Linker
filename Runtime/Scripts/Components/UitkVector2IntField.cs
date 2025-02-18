#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkVector2IntField : UitkLinker<Vector2IntField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(Vector2IntFieldG))]
    public partial class Vector2IntFieldG : UnityEngine.UIElements.Vector2IntField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public Vector2IntFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class Vector2IntFieldG : UnityEngine.UIElements.Vector2IntField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<Vector2IntFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Vector2Int>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlIntAttributeDescription m_XValue = new UxmlIntAttributeDescription
            {
                name = "x"
            };

            private UxmlIntAttributeDescription m_YValue = new UxmlIntAttributeDescription
            {
                name = "y"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                Vector2IntFieldG obj = (Vector2IntFieldG)ve;
                obj.SetValueWithoutNotify(new Vector2Int(m_XValue.GetValueFromBag(bag, cc), m_YValue.GetValueFromBag(bag, cc)));

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif