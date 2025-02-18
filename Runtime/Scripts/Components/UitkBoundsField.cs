#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.UEL
{
    public class UitkBoundsField : UitkLinker<BoundsField> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(BoundsFieldG))]
    public partial class BoundsFieldG : UnityEngine.UIElements.BoundsField, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public BoundsFieldG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class BoundsFieldG : UnityEngine.UIElements.BoundsField, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<BoundsFieldG, UxmlTraits> { }

        public new class UxmlTraits : BaseField<Bounds>.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            private UxmlFloatAttributeDescription m_CenterXValue = new UxmlFloatAttributeDescription
            {
                name = "cx"
            };

            private UxmlFloatAttributeDescription m_CenterYValue = new UxmlFloatAttributeDescription
            {
                name = "cy"
            };

            private UxmlFloatAttributeDescription m_CenterZValue = new UxmlFloatAttributeDescription
            {
                name = "cz"
            };

            private UxmlFloatAttributeDescription m_ExtentsXValue = new UxmlFloatAttributeDescription
            {
                name = "ex"
            };

            private UxmlFloatAttributeDescription m_ExtentsYValue = new UxmlFloatAttributeDescription
            {
                name = "ey"
            };

            private UxmlFloatAttributeDescription m_ExtentsZValue = new UxmlFloatAttributeDescription
            {
                name = "ez"
            };

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                BoundsFieldG boundsField = (BoundsFieldG)ve;
                boundsField.SetValueWithoutNotify(new Bounds(new Vector3(m_CenterXValue.GetValueFromBag(bag, cc), m_CenterYValue.GetValueFromBag(bag, cc), m_CenterZValue.GetValueFromBag(bag, cc)), new Vector3(m_ExtentsXValue.GetValueFromBag(bag, cc), m_ExtentsYValue.GetValueFromBag(bag, cc), m_ExtentsZValue.GetValueFromBag(bag, cc))));

                BoundsFieldG obj = ve as BoundsFieldG;
                GuidGenerator.GenerateGuid(m_Guid, obj, bag, cc);
            }
        }
    }
#endif
}
#endif