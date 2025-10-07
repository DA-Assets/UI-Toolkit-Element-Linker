using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkLinker<TElement> : UitkLinkerBase, IHaveElement<TElement> where TElement : VisualElement
    {
        protected TElement _element;
        public TElement Element => _element;
        public TElement E => _element;

        public virtual void Start()
        {
            LinkElement();
        }

        public override void LinkElement()
        {
            string goName = this == null ? "null" : gameObject == null ? "null" : gameObject.name;
            string targetObjectStr = GetTargetObjectStr();

            if (_uiDocument == null)
            {
                throw new NullReferenceException($"Can't find UIDocument for '{targetObjectStr}'.\nGameObject name: {goName}");
            }

            VisualElement root =
#if UNITY_2021_3_OR_NEWER
                _uiDocument.rootVisualElement;
#else  
                null;
#endif

            VisualElement elem = null;

            switch (_linkingMode)
            {
                case UitkLinkingMode.Name:
                    {
                        elem = root.Query(_name);
                    }
                    break;
                case UitkLinkingMode.IndexNames:
                    {
                        elem = FindComponentByHierarchy(
                            root,
                            _names,
                            ve => ve.name,
                            debug: _debug,
                            gameObject: gameObject);
                    }
                    break;
                case UitkLinkingMode.Guid:
                    {
                        elem = FindGuidRecursive(root, _guid);
                    }
                    break;
                case UitkLinkingMode.Guids:
                    {
                        elem = FindComponentByGuidHierarchy(
                            root,
                            _guids,
                            gh => gh.guid,
                            debug: _debug,
                            gameObject: gameObject);
                    }
                    break;
                default:
                    {
                        Debug.LogError($"{nameof(UitkLinkingMode)} for '{goName}' component is not specified.");
                    }
                    break;
            }

            if (elem == null)
            {
                Debug.LogError($"Can't find '{targetObjectStr}' element.\nGameObject name: {goName}");
                return;
            }

            if (elem is TElement e)
            {
                _element = e;

            }
            else
            {
                Debug.LogError($"Can't cast types: {elem.GetType()} | {typeof(TElement)}.\nGameObject name: {goName}");
            }

            if (_debug && _element != null)
            {
                string elementName = (string.IsNullOrEmpty(_element.name)
                    && (_element is IHaveGuid))
                    ? (_element as IHaveGuid).guid
                    : _element.name;

                Debug.Log($"Element found: {elementName}.\nGameObject name: {goName}");
            }

            OnElementLinked();
        }

        private string GetTargetObjectStr()
        {
            string str = null;

            switch (_linkingMode)
            {
                case UitkLinkingMode.Name:
                    {
                        str = _name;
                    }
                    break;
                case UitkLinkingMode.IndexNames:
                    {
                        if (_names.Length > 0)
                        {
                            str = _names.Last().Name;
                        }
                    }
                    break;
                case UitkLinkingMode.Guid:
                    {
                        str = _guid;
                    }
                    break;
                case UitkLinkingMode.Guids:
                    {
                        if (_guids.Length > 0)
                        {
                            str = _guids.Last();
                        }
                    }
                    break;
            }

            if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }

            return str;
        }
    }
}
