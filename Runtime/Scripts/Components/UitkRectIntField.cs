#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkRectIntField : UitkLinker<RectIntField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(RectIntFieldG))]
    public partial class RectIntFieldG : UnityEngine.UIElements.RectIntField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public RectIntFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class RectIntFieldG : UnityEngine.UIElements.RectIntField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<RectIntFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<RectInt>.UxmlTraits
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

            private UxmlIntAttributeDescription m_WValue = new UxmlIntAttributeDescription
            {
                name = "w"
            };

            private UxmlIntAttributeDescription m_HValue = new UxmlIntAttributeDescription
            {
                name = "h"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                RectIntFieldG obj = (RectIntFieldG)ve;
                obj.SetValueWithoutNotify(new RectInt(m_XValue.GetValueFromBag(bag, cc), m_YValue.GetValueFromBag(bag, cc), m_WValue.GetValueFromBag(bag, cc), m_HValue.GetValueFromBag(bag, cc)));

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif