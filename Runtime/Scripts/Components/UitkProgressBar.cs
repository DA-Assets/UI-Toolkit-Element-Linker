#if UNITY_2021_3_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkProgressBar : UitkLinker<ProgressBar> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(ProgressBarG))]
    public partial class ProgressBarG : UnityEngine.UIElements.ProgressBar, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public ProgressBarG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class ProgressBarG : UnityEngine.UIElements.ProgressBar, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<ProgressBarG, UxmlTraits> { }

        public new class UxmlTraits : UnityEngine.UIElements.VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ProgressBarG obj = ve as ProgressBarG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif