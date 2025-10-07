#if UNITY_2021_3_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkHelpBox : UitkLinker<HelpBox> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(HelpBoxG))]
    public partial class HelpBoxG : UnityEngine.UIElements.HelpBox, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public HelpBoxG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class HelpBoxG : UnityEngine.UIElements.HelpBox, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<HelpBoxG, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
            {
                name = "text"
            };

            private UxmlEnumAttributeDescription<HelpBoxMessageType> m_MessageType = new UxmlEnumAttributeDescription<HelpBoxMessageType>
            {
                name = "message-type",
                defaultValue = HelpBoxMessageType.None
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                HelpBoxG helpBox = ve as HelpBoxG;
                helpBox.text = m_Text.GetValueFromBag(bag, cc);
                helpBox.messageType = m_MessageType.GetValueFromBag(bag, cc);

                HelpBoxG obj = ve as HelpBoxG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif