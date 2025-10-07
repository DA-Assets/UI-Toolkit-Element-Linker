#if UNITY_2021_3_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkGroupBox : UitkLinker<GroupBox> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(GroupBoxG))]
    public partial class GroupBoxG : UnityEngine.UIElements.GroupBox, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public GroupBoxG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class GroupBoxG : UnityEngine.UIElements.GroupBox, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<GroupBoxG, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
            {
                name = "text"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((GroupBoxG)ve).text = m_Text.GetValueFromBag(bag, cc);

                GroupBoxG obj = ve as GroupBoxG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif