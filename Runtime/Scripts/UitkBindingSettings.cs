using UnityEngine;

namespace DA_Assets.ULB
{
    public class UitkBindingSettings : ScriptableObject
    {
        [SerializeField, Range(0, 4)] private int _maxDepth = 2;
        public int MaxDepth { get => _maxDepth; set => _maxDepth = Mathf.Max(0, value); }

        [SerializeField] private bool _showPrimitiveType = true;
        public bool ShowPrimitiveType { get => _showPrimitiveType; set => _showPrimitiveType = value; }

        [SerializeField] private bool _showObjectType = true;
        public bool ShowObjectType { get => _showObjectType; set => _showObjectType = value; }

        [SerializeField] private bool _showToString = true;
        public bool ShowToString { get => _showToString; set => _showToString = value; }

        [SerializeField] private bool _showObsolete = false;
        public bool ShowObsolete { get => _showObsolete; set => _showObsolete = value; }
    }
}
