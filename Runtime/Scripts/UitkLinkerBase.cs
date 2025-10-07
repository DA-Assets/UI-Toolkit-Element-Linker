using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public abstract class UitkLinkerBase : MonoBehaviour
    {
        [SerializeField] protected UitkLinkingMode _linkingMode = UitkLinkingMode.IndexNames;
        public UitkLinkingMode LinkingMode { get => _linkingMode; set => _linkingMode = value; }

        [SerializeField] protected string _guid;
        public string Guid { get => _guid; set => _guid = value; }

        [SerializeField] protected string[] _guids = new string[] { };
        public string[] Guids { get => _guids; set => _guids = value; }

        [SerializeField] protected string _name;
        public string Name { get => _name; set => _name = value; }

        [SerializeField] protected ElementIndexName[] _names = new ElementIndexName[] { };
        public ElementIndexName[] Names { get => _names; set => _names = value; }

        [SerializeField]
        protected
#if UNITY_2021_3_OR_NEWER
            UIDocument
#else
            GameObject
#endif
            _uiDocument;

        public
#if UNITY_2021_3_OR_NEWER
            UIDocument
#else
            GameObject
#endif
            UIDocument
        { get => _uiDocument; set => _uiDocument = value; }

        [SerializeField] protected bool _debug;
        public bool IsDebug { get => _debug; set => _debug = value; }

        public GameObject GameObject => gameObject;

        public virtual void LinkElement()
        {

        }

        public virtual void OnElementLinked()
        {

        }

        public virtual void OnValidate()
        {
            if (_linkingMode == UitkLinkingMode.IndexNames)
            {
                for (int i = 0; i < _names.Length; i++)
                {
                    ElementIndexName ein = _names[i];

                    if (ein.Index == 0 && string.IsNullOrEmpty(ein.Name) && !ein.Init)
                    {
                        ein.Init = true;
                        ein.Index = UitkConstants.DEFAULT_INDEX;
                    }

                    _names[i] = ein;
                }
            }
        }

        public static VisualElement FindGuidRecursive(VisualElement root, string guid)
        {
            if (root == null)
                return null;

            System.Collections.Generic.IEnumerable<VisualElement> childs = root.Children();

            if (childs == null)
                return null;

            foreach (VisualElement child in childs)
            {
                if (child is IHaveGuid customElement && customElement.guid == guid)
                {
                    return child;
                }

                VisualElement found = FindGuidRecursive(child, guid);

                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        public static VisualElement FindComponentByHierarchy(
            VisualElement root,
            ElementIndexName[] hierarchy,
            Func<VisualElement, string> propertySelector,
            int depth = 0,
            bool debug = false,
            GameObject gameObject = null)
        {
            if (depth == hierarchy.Length)
                return root;

            if (root == null)
                return null;

            ElementIndexName ein = hierarchy[depth];

            VisualElement[] children = root.Children().ToArray();

            for (int i = 0; i < children.Length; i++)
            {
                VisualElement child = children[i];

                int newDepth = depth + 1;

                if (ein.Index == UitkConstants.DEFAULT_INDEX)
                {
                    if (debug)
                        Debug.Log($"{gameObject?.name} | 1 | {ein.Name}");

                    if (propertySelector(child) == ein.Name)
                    {
                        return FindComponentByHierarchy(child, hierarchy, propertySelector, newDepth, debug, gameObject);
                    }
                }
                else if (ein.Index == i)
                {
                    if (debug)
                        Debug.Log($"{gameObject?.name} | 2 | {ein.Name}");

                    return FindComponentByHierarchy(child, hierarchy, propertySelector, newDepth, debug, gameObject);
                }
            }

            return null;
        }

        public static VisualElement FindComponentByGuidHierarchy(
            VisualElement root,
            string[] guids,
            Func<IHaveGuid, string> propertySelector,
            int depth = 0,
            bool debug = false,
            GameObject gameObject = null)
        {
            if (depth == guids.Length)
                return root;

            if (root == null)
                return null;

            string guid = guids[depth];

            VisualElement[] children = root.Children().ToArray();

            for (int i = 0; i < children.Length; i++)
            {
                VisualElement child = children[i];

                int newDepth = depth + 1;

                if (child is IHaveGuid myElement)
                {
                    if (propertySelector(myElement) == guid)
                    {
                        if (debug)
                            Debug.Log($"{gameObject?.name} | 3 | {guid}");

                        return FindComponentByGuidHierarchy(child, guids, propertySelector, newDepth, debug, gameObject);
                    }
                }
            }

            return null;
        }
    }
}