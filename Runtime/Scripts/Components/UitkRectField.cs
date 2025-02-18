#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkRectField : UitkLinker<RectField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(RectFieldG))]
    public partial class RectFieldG : UnityEngine.UIElements.RectField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public RectFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class RectFieldG : UnityEngine.UIElements.RectField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<RectFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Rect>.UxmlTraits
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

            private UxmlFloatAttributeDescription m_WValue = new UxmlFloatAttributeDescription
            {
                name = "w"
            };

            private UxmlFloatAttributeDescription m_HValue = new UxmlFloatAttributeDescription
            {
                name = "h"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                RectFieldG obj = (RectFieldG)ve;
                obj.SetValueWithoutNotify(new Rect(m_XValue.GetValueFromBag(bag, cc), m_YValue.GetValueFromBag(bag, cc), m_WValue.GetValueFromBag(bag, cc), m_HValue.GetValueFromBag(bag, cc)));

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif