#if UNITY_2021_3_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkSliderInt : UitkLinker<SliderInt, int> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(SliderIntG))]
    public partial class SliderIntG : UnityEngine.UIElements.SliderInt, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public SliderIntG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class SliderIntG : UnityEngine.UIElements.SliderInt, IHaveGuid
    {
        public string guid { get; set; }

        private new class UxmlFactory : UxmlFactory<SliderIntG, UxmlTraits> { }

        private new class UxmlTraits : BaseSlider<int>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlIntAttributeDescription m_LowValue = new UxmlIntAttributeDescription
            {
                name = "low-value"
            };

            private UxmlIntAttributeDescription m_HighValue = new UxmlIntAttributeDescription
            {
                name = "high-value",
                defaultValue = 10
            };

            private UxmlIntAttributeDescription m_PageSize = new UxmlIntAttributeDescription
            {
                name = "page-size",
                defaultValue = 0
            };

            private UxmlBoolAttributeDescription m_ShowInputField = new UxmlBoolAttributeDescription
            {
                name = "show-input-field",
                defaultValue = false
            };

            private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
            {
                name = "direction",
                defaultValue = SliderDirection.Horizontal
            };

            private UxmlBoolAttributeDescription m_Inverted = new UxmlBoolAttributeDescription
            {
                name = "inverted",
                defaultValue = false
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                SliderIntG obj = (SliderIntG)ve;
                obj.lowValue = m_LowValue.GetValueFromBag(bag, cc);
                obj.highValue = m_HighValue.GetValueFromBag(bag, cc);
                obj.direction = m_Direction.GetValueFromBag(bag, cc);
                obj.pageSize = m_PageSize.GetValueFromBag(bag, cc);
                obj.showInputField = m_ShowInputField.GetValueFromBag(bag, cc);
#if UNITY_2021_3_OR_NEWER
                obj.inverted = m_Inverted.GetValueFromBag(bag, cc);
#endif
                base.Init(ve, bag, cc);

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif