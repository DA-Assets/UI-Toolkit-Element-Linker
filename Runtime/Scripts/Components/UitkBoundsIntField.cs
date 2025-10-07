#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkBoundsIntField : UitkLinker<BoundsIntField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(BoundsIntFieldG))]
    public partial class BoundsIntFieldG : UnityEngine.UIElements.BoundsIntField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public BoundsIntFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class BoundsIntFieldG : UnityEngine.UIElements.BoundsIntField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<BoundsIntFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<BoundsInt>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlIntAttributeDescription m_PositionXValue = new UxmlIntAttributeDescription
            {
                name = "px"
            };

            private UxmlIntAttributeDescription m_PositionYValue = new UxmlIntAttributeDescription
            {
                name = "py"
            };

            private UxmlIntAttributeDescription m_PositionZValue = new UxmlIntAttributeDescription
            {
                name = "pz"
            };

            private UxmlIntAttributeDescription m_SizeXValue = new UxmlIntAttributeDescription
            {
                name = "sx"
            };

            private UxmlIntAttributeDescription m_SizeYValue = new UxmlIntAttributeDescription
            {
                name = "sy"
            };

            private UxmlIntAttributeDescription m_SizeZValue = new UxmlIntAttributeDescription
            {
                name = "sz"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                BoundsIntFieldG boundsIntField = (BoundsIntFieldG)ve;
                boundsIntField.SetValueWithoutNotify(new BoundsInt(new Vector3Int(m_PositionXValue.GetValueFromBag(bag, cc), m_PositionYValue.GetValueFromBag(bag, cc), m_PositionZValue.GetValueFromBag(bag, cc)), new Vector3Int(m_SizeXValue.GetValueFromBag(bag, cc), m_SizeYValue.GetValueFromBag(bag, cc), m_SizeZValue.GetValueFromBag(bag, cc))));

                BoundsIntFieldG obj = ve as BoundsIntFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif