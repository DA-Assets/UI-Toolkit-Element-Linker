using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkMinMaxSlider : UitkLinker<MinMaxSlider> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(MinMaxSliderG))]
    public partial class MinMaxSliderG : UnityEngine.UIElements.MinMaxSlider, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public MinMaxSliderG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class MinMaxSliderG : UnityEngine.UIElements.MinMaxSlider, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<MinMaxSliderG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Vector2>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlFloatAttributeDescription m_MinValue = new UxmlFloatAttributeDescription
            {
                name = "min-value",
                defaultValue = 0f
            };

            private UxmlFloatAttributeDescription m_MaxValue = new UxmlFloatAttributeDescription
            {
                name = "max-value",
                defaultValue = 10f
            };

            private UxmlFloatAttributeDescription m_LowLimit = new UxmlFloatAttributeDescription
            {
                name = "low-limit",
                defaultValue = float.MinValue
            };

            private UxmlFloatAttributeDescription m_HighLimit = new UxmlFloatAttributeDescription
            {
                name = "high-limit",
                defaultValue = float.MaxValue
            };

            public UxmlTraits()
            {
                m_PickingMode.defaultValue = PickingMode.Ignore;
            }

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                MinMaxSliderG minMaxSlider = (MinMaxSliderG)ve;
                minMaxSlider.lowLimit = m_LowLimit.GetValueFromBag(bag, cc);
                minMaxSlider.highLimit = m_HighLimit.GetValueFromBag(bag, cc);
                Vector2 value = new Vector2(m_MinValue.GetValueFromBag(bag, cc), m_MaxValue.GetValueFromBag(bag, cc));
                minMaxSlider.value = value;

                MinMaxSliderG obj = ve as MinMaxSliderG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}