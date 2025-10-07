using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkVisualElement : UitkLinker<VisualElement> { }

#if UNITY_6000_0_OR_NEWER
    [UxmlElement(nameof(VisualElementG))]
    public partial class VisualElementG : UnityEngine.UIElements.VisualElement, IHaveGuid
    {
        [UxmlAttribute]
        public string guid { get; set; }

        public VisualElementG()
        {
            guid = GuidGenerator.GenerateGuid(guid);
        }
    }
#else
    public class VisualElementG : UnityEngine.UIElements.VisualElement, IHaveGuid
    {
        public string guid { get; set; }

        public new class UxmlFactory : UxmlFactory<VisualElementG, UxmlTraits> { }

        public new class UxmlTraits : UnityEngine.UIElements.UxmlTraits
        {
            UxmlStringAttributeDescription m_Guid = GuidGenerator.GetGuidField();

            protected UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
            {
                name = "name"
            };

            private UxmlStringAttributeDescription m_ViewDataKey = new UxmlStringAttributeDescription
            {
                name = "view-data-key"
            };

            protected UxmlEnumAttributeDescription<PickingMode> m_PickingMode = new UxmlEnumAttributeDescription<PickingMode>
            {
                name = "picking-mode",
                obsoleteNames = new string[1] { "pickingMode" }
            };

            private UxmlStringAttributeDescription m_Tooltip = new UxmlStringAttributeDescription
            {
                name = "tooltip"
            };

            private UxmlEnumAttributeDescription<UsageHints> m_UsageHints = new UxmlEnumAttributeDescription<UsageHints>
            {
                name = "usage-hints"
            };

            private UxmlIntAttributeDescription m_TabIndex = new UxmlIntAttributeDescription
            {
                name = "tabindex",
                defaultValue = 0
            };

            private UxmlStringAttributeDescription m_Class = new UxmlStringAttributeDescription
            {
                name = "class"
            };

            private UxmlStringAttributeDescription m_ContentContainer = new UxmlStringAttributeDescription
            {
                name = "content-container",
                obsoleteNames = new string[1] { "contentContainer" }
            };

            private UxmlStringAttributeDescription m_Style = new UxmlStringAttributeDescription
            {
                name = "style"
            };

            protected UxmlIntAttributeDescription focusIndex { get; set; } = new UxmlIntAttributeDescription
            {
                name = null,
                obsoleteNames = new string[2] { "focus-index", "focusIndex" },
                defaultValue = -1
            };

            protected UxmlBoolAttributeDescription focusable { get; set; } = new UxmlBoolAttributeDescription
            {
                name = "focusable",
                defaultValue = false
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get
                {
                    yield return new UxmlChildElementDescription(typeof(VisualElement));
                }
            }

            public override void Init(UnityEngine.UIElements.VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (ve == null)
                {
                    throw new ArgumentNullException("ve");
                }

                ve.name = m_Name.GetValueFromBag(bag, cc);
                ve.viewDataKey = m_ViewDataKey.GetValueFromBag(bag, cc);
                ve.pickingMode = m_PickingMode.GetValueFromBag(bag, cc);
                ve.usageHints = m_UsageHints.GetValueFromBag(bag, cc);
                ve.tooltip = m_Tooltip.GetValueFromBag(bag, cc);
                int value = 0;
                if (focusIndex.TryGetValueFromBag(bag, cc, ref value))
                {
                    ve.tabIndex = ((value >= 0) ? value : 0);
                    ve.focusable = value >= 0;
                }

                ve.tabIndex = m_TabIndex.GetValueFromBag(bag, cc);
                ve.focusable = focusable.GetValueFromBag(bag, cc);

                VisualElementG ate = (VisualElementG)ve;

                GuidGenerator.GenerateGuid(m_Guid, ate, bag, cc);
            }
        }
    }
#endif
}