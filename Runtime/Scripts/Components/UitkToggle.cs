using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkToggle : UitkLinker<Toggle> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(ToggleG))]
    public partial class ToggleG : UnityEngine.UIElements.Toggle, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public ToggleG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class ToggleG : UnityEngine.UIElements.Toggle, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<ToggleG, UxmlTraits> { }

        public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
            {
                name = "text"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ToggleG obj = ve as ToggleG;
                obj.text = m_Text.GetValueFromBag(bag, cc);

                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}