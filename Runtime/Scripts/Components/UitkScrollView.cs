#if UNITY_2022_1_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkScrollView : UitkLinker<ScrollView> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(ScrollViewG))]
    public partial class ScrollViewG : UnityEngine.UIElements.ScrollView, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public ScrollViewG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class ScrollViewG : UnityEngine.UIElements.ScrollView, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<ScrollViewG, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private static readonly float k_DefaultScrollDecelerationRate = 0.135f;
            private static readonly float k_DefaultElasticity = 0.1f;
            private static readonly long k_DefaultElasticAnimationInterval = 16L;

            private UxmlEnumAttributeDescription<ScrollViewMode> m_ScrollViewMode = new UxmlEnumAttributeDescription<ScrollViewMode>
            {
                name = "mode",
                defaultValue = ScrollViewMode.Vertical
            };

            private UxmlEnumAttributeDescription<NestedInteractionKind> m_NestedInteractionKind = new UxmlEnumAttributeDescription<NestedInteractionKind>
            {
                name = "nested-interaction-kind",
                defaultValue = NestedInteractionKind.Default
            };

            private UxmlBoolAttributeDescription m_ShowHorizontal = new UxmlBoolAttributeDescription
            {
                name = "show-horizontal-scroller"
            };

            private UxmlBoolAttributeDescription m_ShowVertical = new UxmlBoolAttributeDescription
            {
                name = "show-vertical-scroller"
            };

            private UxmlEnumAttributeDescription<ScrollerVisibility> m_HorizontalScrollerVisibility = new UxmlEnumAttributeDescription<ScrollerVisibility>
            {
                name = "horizontal-scroller-visibility"
            };

            private UxmlEnumAttributeDescription<ScrollerVisibility> m_VerticalScrollerVisibility = new UxmlEnumAttributeDescription<ScrollerVisibility>
            {
                name = "vertical-scroller-visibility"
            };

            private UxmlFloatAttributeDescription m_HorizontalPageSize = new UxmlFloatAttributeDescription
            {
                name = "horizontal-page-size",
                defaultValue = -1f
            };

            private UxmlFloatAttributeDescription m_VerticalPageSize = new UxmlFloatAttributeDescription
            {
                name = "vertical-page-size",
                defaultValue = -1f
            };

            private UxmlFloatAttributeDescription m_MouseWheelScrollSize = new UxmlFloatAttributeDescription
            {
                name = "mouse-wheel-scroll-size",
                defaultValue = 18f
            };

            private UxmlEnumAttributeDescription<TouchScrollBehavior> m_TouchScrollBehavior = new UxmlEnumAttributeDescription<TouchScrollBehavior>
            {
                name = "touch-scroll-type",
                defaultValue = TouchScrollBehavior.Clamped
            };

            private UxmlFloatAttributeDescription m_ScrollDecelerationRate = new UxmlFloatAttributeDescription
            {
                name = "scroll-deceleration-rate",
                defaultValue = k_DefaultScrollDecelerationRate
            };

            private UxmlFloatAttributeDescription m_Elasticity = new UxmlFloatAttributeDescription
            {
                name = "elasticity",
                defaultValue = k_DefaultElasticity
            };

            private UxmlLongAttributeDescription m_ElasticAnimationIntervalMs = new UxmlLongAttributeDescription
            {
                name = "elastic-animation-interval-ms",
                defaultValue = k_DefaultElasticAnimationInterval
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ScrollViewG obj = (ScrollViewG)ve;
                obj.mode = m_ScrollViewMode.GetValueFromBag(bag, cc);
                ScrollerVisibility value = ScrollerVisibility.Auto;
                if (m_HorizontalScrollerVisibility.TryGetValueFromBag(bag, cc, ref value))
                {
                    obj.horizontalScrollerVisibility = value;
                }
                else
                {
                    obj.showHorizontal = m_ShowHorizontal.GetValueFromBag(bag, cc);
                }

                ScrollerVisibility value2 = ScrollerVisibility.Auto;
                if (m_VerticalScrollerVisibility.TryGetValueFromBag(bag, cc, ref value2))
                {
                    obj.verticalScrollerVisibility = value2;
                }
                else
                {
                    obj.showVertical = m_ShowVertical.GetValueFromBag(bag, cc);
                }

                obj.nestedInteractionKind = m_NestedInteractionKind.GetValueFromBag(bag, cc);
                obj.horizontalPageSize = m_HorizontalPageSize.GetValueFromBag(bag, cc);
                obj.verticalPageSize = m_VerticalPageSize.GetValueFromBag(bag, cc);
                obj.mouseWheelScrollSize = m_MouseWheelScrollSize.GetValueFromBag(bag, cc);
                obj.scrollDecelerationRate = m_ScrollDecelerationRate.GetValueFromBag(bag, cc);
                obj.touchScrollBehavior = m_TouchScrollBehavior.GetValueFromBag(bag, cc);
                obj.elasticity = m_Elasticity.GetValueFromBag(bag, cc);
                obj.elasticAnimationIntervalMs = m_ElasticAnimationIntervalMs.GetValueFromBag(bag, cc);

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif