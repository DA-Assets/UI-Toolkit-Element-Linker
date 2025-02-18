using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkScroller : UitkLinker<Scroller> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(ScrollerG))]
    public partial class ScrollerG : UnityEngine.UIElements.Scroller, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public ScrollerG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class ScrollerG : UnityEngine.UIElements.Scroller, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<ScrollerG, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
            {
                name = "low-value",
                obsoleteNames = new string[1] { "lowValue" }
            };

            private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
            {
                name = "high-value",
                obsoleteNames = new string[1] { "highValue" }
            };

            private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
            {
                name = "direction",
                defaultValue = SliderDirection.Vertical
            };

            private UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription
            {
                name = "value"
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get
                {
                    yield break;
                }
            }

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ScrollerG obj = (ScrollerG)ve;
                obj.slider.lowValue = m_LowValue.GetValueFromBag(bag, cc);
                obj.slider.highValue = m_HighValue.GetValueFromBag(bag, cc);
                obj.direction = m_Direction.GetValueFromBag(bag, cc);
                obj.value = m_Value.GetValueFromBag(bag, cc);

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
