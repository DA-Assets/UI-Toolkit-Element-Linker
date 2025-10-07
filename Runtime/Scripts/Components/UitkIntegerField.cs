#if UNITY_2022_1_OR_NEWER
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkIntegerField : UitkLinker<IntegerField, int> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(IntegerFieldG))]
    public partial class IntegerFieldG : UnityEngine.UIElements.IntegerField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public IntegerFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class IntegerFieldG : UnityEngine.UIElements.IntegerField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<IntegerFieldG, UxmlTraits> { }

        public new class UxmlTraits : TextValueFieldTraits<int, UxmlIntAttributeDescription>
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                IntegerFieldG obj = ve as IntegerFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif