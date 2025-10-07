using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkFoldout : UitkLinker<Foldout> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(FoldoutG))]
    public partial class FoldoutG : UnityEngine.UIElements.Foldout, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public FoldoutG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class FoldoutG : UnityEngine.UIElements.Foldout, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<FoldoutG, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
            {
                name = "text"
            };

            private UxmlBoolAttributeDescription m_Value = new UxmlBoolAttributeDescription
            {
                name = "value",
                defaultValue = true
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is FoldoutG foldout)
                {
                    foldout.text = m_Text.GetValueFromBag(bag, cc);
                    foldout.SetValueWithoutNotify(m_Value.GetValueFromBag(bag, cc));

                    FoldoutG obj = ve as FoldoutG;
                    GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
                }
            }
        }
    }
#endif
}
