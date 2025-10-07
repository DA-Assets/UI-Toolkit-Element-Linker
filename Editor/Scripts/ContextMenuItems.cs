using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public static class ContextMenuItems
    {
        private static bool _debug = false;

        private const string UI_BUILDER_TYPE_NAME = "Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule";
        private const string DOCUMENT_FIELD_NAME = "document";
        private const string ACTIVE_OPEN_UXML_FILE_FIELD_NAME = "activeOpenUXMLFile";
        private const string VISUAL_TREE_ASSET_FIELD_NAME = "visualTreeAsset";
        private const string SELECTION_FIELD_NAME = "m_Selection";

        [MenuItem("Tools/" + UitkConstants.Publisher + "/" + UitkConstants.Product + ": Copy element guid hierarchy", false, 10)]
        private static void CopyElementGuidHierarchy_OnClick()
        {
            CopyHierarchy(guid: true, includeIndex: false, includeName: true);
        }

        [MenuItem("Tools/" + UitkConstants.Publisher + "/" + UitkConstants.Product + ": Copy element name hierarchy", false, 11)]
        private static void CopyElementNameHierarchy_OnClick()
        {
            CopyHierarchy(guid: false, includeIndex: false, includeName: true);
        }

        [MenuItem("Tools/" + UitkConstants.Publisher + "/" + UitkConstants.Product + ": Copy element index hierarchy", false, 12)]
        private static void CopyElementIndexHierarchy_OnClick()
        {
            CopyHierarchy(guid: false, includeIndex: true, includeName: false);
        }

        [MenuItem("Tools/" + UitkConstants.Publisher + "/" + UitkConstants.Product + ": Copy element index + name hierarchy", false, 13)]
        private static void CopyElementIndexNameHierarchy_OnClick()
        {
            CopyHierarchy(guid: false, includeIndex: true, includeName: true);
        }

        //[MenuItem("Tools/" + UEL_Constants.Publisher + "/" + UEL_Constants.Product + ": Test Item", false, 14)]
        private static void TestItem_OnClick()
        {
            GetUIBuilderWindow(out var uiBuilderType, out var currentBuilderWindow);

            VisualElement selectedElement = GetSingleSelectedElement(uiBuilderType, currentBuilderWindow);

            if (selectedElement == null)
            {
                Debug.Log($"No element selected.");
                return;
            }
        }

        private static void CopyHierarchy(bool guid, bool includeIndex, bool includeName)
        {
            GetUIBuilderWindow(out var uiBuilderType, out var currentBuilderWindow);

            VisualElement selectedElement = GetSingleSelectedElement(uiBuilderType, currentBuilderWindow);

            if (selectedElement == null)
            {
                Debug.Log($"No element selected.");
                return;
            }

            string json = null;

            if (guid)
            {
                List<string> guids = new List<string>();

                var h = GetParentHierarchy(selectedElement);

                foreach (var item in h)
                {
                    if (item is IHaveGuid ihg)
                    {
                        guids.Add(ihg.guid);
                    }
                    else
                    {
                        guids.Add(null);
                    }
                }

                json = ElementIndexNameSerializator.ConvertGuidsToJson("_guids", guids.ToArray());

                Debug.Log($"The hierarchy of component '{(string.IsNullOrWhiteSpace(selectedElement.name) ? guids.Last() : selectedElement.name)}' is copied to the clipboard.");
            }
            else
            {
                ElementIndexName[] elementIndexNames = GetParentHierarchy(selectedElement).Select(x => new ElementIndexName
                {
                    Index = includeIndex ? x.parent.IndexOf(x) : UitkConstants.DEFAULT_INDEX,
                    Name = includeName ? x.name : null
                }).ToArray();

                json = ElementIndexNameSerializator.ConvertElementsToJson("_names", elementIndexNames);

#if UNITY_2021_3_OR_NEWER
                Debug.Log($"The hierarchy of component '{(string.IsNullOrWhiteSpace(selectedElement.name) ? selectedElement.GetType().ToString() : selectedElement.name)}' is copied to the clipboard.");
#else
                Debug.Log($"The hierarchy of component is copied to the clipboard.");
#endif
            }

            EditorGUIUtility.systemCopyBuffer = json;
        }

        //[MenuItem("Tools/" + UitkLinkerBase.Publisher + "/" + UitkLinkerBase.Product + " > Add GUID to selected elements")]
        private static void AddGuidToSelectedElements_OnClick()
        {
            AddGuidToSelectedElements();
        }

        //[MenuItem("Tools/" + UitkLinkerBase.Publisher + "/" + UitkLinkerBase.Product + " > Add GUID to all elements")]
        private static void AddGuidToAlldElements_OnClick()
        {
            AddGuidToAllElements();
        }

        private static void GetUIBuilderWindow(out Type uiBuilderType, out EditorWindow currentBuilderWindow)
        {
            currentBuilderWindow = null;
            uiBuilderType = Type.GetType(UI_BUILDER_TYPE_NAME);

            if (uiBuilderType == null)
            {
                Debug.LogError($"'{UI_BUILDER_TYPE_NAME}' type not found.");
                return;
            }

            currentBuilderWindow = EditorWindow.GetWindow(uiBuilderType);
            if (currentBuilderWindow == null)
            {
                Debug.LogError($"UI Builder window not found with '{uiBuilderType.Name}' type.");
                return;
            }
        }

        private static void AddGuidToAllElements()
        {
            GetUIBuilderWindow(out var uiBuilderType, out var currentBuilderWindow);

            List<VisualElement> selectionList = GetSelectedVisualElements(uiBuilderType, currentBuilderWindow);

            if (selectionList == null || selectionList.Count < 1)
            {
                Debug.LogError("No elements selected.");
                return;
            }

            string uxmlPath = GetUxmlPath(uiBuilderType, currentBuilderWindow);
            string uxmlContent = File.ReadAllText(uxmlPath);

            if (string.IsNullOrEmpty(uxmlContent))
            {
                Debug.LogError("Can't open UXML file.");
                return;
            }

            XElement root = XElement.Parse(uxmlContent);
            TraverseXElement(root);
            root.Save(uxmlPath);

            void TraverseXElement(XElement element)
            {
                AddGuidAttribute(element);

                foreach (XElement child in element.Elements())
                {
                    TraverseXElement(child);
                }
            }
        }

        private static void AddGuidToSelectedElements()
        {
            GetUIBuilderWindow(out var uiBuilderType, out var currentBuilderWindow);

            List<VisualElement> selectionList = GetSelectedVisualElements(uiBuilderType, currentBuilderWindow);

            if (selectionList == null || selectionList.Count < 1)
            {
                Debug.LogError("No elements selected.");
                return;
            }

            string uxmlPath = GetUxmlPath(uiBuilderType, currentBuilderWindow);
            string uxmlContent = File.ReadAllText(uxmlPath);

            if (string.IsNullOrEmpty(uxmlContent))
            {
                Debug.LogError("Can't open UXML file.");
                return;
            }

            XElement root = XElement.Parse(uxmlContent);

            foreach (VisualElement item in selectionList)
            {
                Debug.Log($"Selected: {item.name}");
                AddGuidAttribute(root, item);
            }

            root.Save(uxmlPath);

            Debug.Log("UXML file is updated.");

        }

        private static VisualElement GetSingleSelectedElement(Type uiBuilderType, EditorWindow currentBuilderWindow)
        {
            var l = GetSelectedVisualElements(uiBuilderType, currentBuilderWindow);

            if (l == null)
                return null;

            if (l.Count > 1)
            {
                Debug.Log($"must be 1 selected");
                return null;
            }

            return l.First();
        }

        private static List<VisualElement> GetSelectedVisualElements(Type uiBuilderType, EditorWindow currentBuilderWindow)
        {
            FieldInfo selectionField = uiBuilderType.GetField(SELECTION_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            if (selectionField == null)
            {
                Debug.LogError($"'{SELECTION_FIELD_NAME}' field not found in '{uiBuilderType.Name}' type.");
                return null;
            }

            object builderSelection = selectionField.GetValue(currentBuilderWindow);
            if (builderSelection == null)
            {
                Debug.LogError($"'{SELECTION_FIELD_NAME}' object is null.");
                return null;
            }

            FieldInfo selectionListField = selectionField.FieldType.GetField(SELECTION_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            if (selectionListField == null)
            {
                Debug.LogError($"'{SELECTION_FIELD_NAME}' not found in '{selectionField.FieldType.Name}' type.");
                return null;
            }

            List<VisualElement> selectionList = selectionListField.GetValue(builderSelection) as List<VisualElement>;

            if (selectionList == null || selectionList.Count < 1)
            {
                Debug.LogError("No elements selected.");
                return null;
            }

            return selectionList;
        }

        private static string GetUxmlPath(Type uiBuilderType, EditorWindow currentBuilderWindow)
        {
            PropertyInfo docProp = uiBuilderType.GetProperty(DOCUMENT_FIELD_NAME, BindingFlags.Public | BindingFlags.Instance);
            if (docProp == null)
            {
                Debug.LogError($"'{DOCUMENT_FIELD_NAME}' property not found in '{uiBuilderType.Name}' type.");
                return null;
            }

            object doc = docProp.GetValue(currentBuilderWindow);
            if (doc == null)
            {
                Debug.LogError($"'{DOCUMENT_FIELD_NAME}' object is null.");
                return null;
            }

            PropertyInfo activeOpenUXMLFileProp = docProp.PropertyType.GetProperty(ACTIVE_OPEN_UXML_FILE_FIELD_NAME, BindingFlags.Public | BindingFlags.Instance);
            if (activeOpenUXMLFileProp == null)
            {
                Debug.LogError($"'{ACTIVE_OPEN_UXML_FILE_FIELD_NAME}' not found in '{docProp.PropertyType.Name}'");
                return null;
            }

            object activeOpenUXMLFile = activeOpenUXMLFileProp.GetValue(doc);
            if (activeOpenUXMLFile == null)
            {
                Debug.LogError($"'{ACTIVE_OPEN_UXML_FILE_FIELD_NAME}' object is null.");
                return null;
            }

            PropertyInfo visualTreeAssetProp = activeOpenUXMLFileProp.PropertyType.GetProperty(VISUAL_TREE_ASSET_FIELD_NAME, BindingFlags.Public | BindingFlags.Instance);
            if (visualTreeAssetProp == null)
            {
                Debug.LogError($"'{VISUAL_TREE_ASSET_FIELD_NAME}' property not found in '{activeOpenUXMLFileProp.PropertyType.Name}' type.");
                return null;
            }

            VisualTreeAsset visualTreeAsset = visualTreeAssetProp.GetValue(activeOpenUXMLFile) as VisualTreeAsset;
            if (visualTreeAsset == null)
            {
                Debug.LogError($"'{VISUAL_TREE_ASSET_FIELD_NAME}' object is null.");
                return null;
            }

            string uxmlPath = AssetDatabase.GetAssetPath(visualTreeAsset);
            return uxmlPath;
        }

        private static void AddGuidAttribute(XElement root, VisualElement elementObj)
        {
            List<VisualElement> hierarchy = GetParentHierarchy(elementObj);

            XElement targetElement = FindElementByHierarchy(root, hierarchy.Select(x => x.name).ToList());

            if (targetElement == null)
            {
                Debug.LogError("Target element not found.");
                return;
            }

            AddGuidAttribute(targetElement);
        }

        private static void AddGuidAttribute(XElement element)
        {
            string guid = Guid.NewGuid().ToString();
            guid = guid.Replace("-", "");
            string attributeName = "guid";
            element.SetAttributeValue(attributeName, guid);
        }

        static string[] _endOfHierarchy = new string[] { "document", "shared-styles-and-document", "canvas", "viewport-surface", "viewport", "viewport-wrapper" };

        public static bool ValidateEndOfHierarchy(string[] endOf)
        {
            if (_endOfHierarchy.Length != endOf.Length)
            {
                throw new ArgumentException("Must be same length.");
            }

            for (int i = 0; i < _endOfHierarchy.Length; i++)
            {
                if (endOf[i] == null)
                {
                    return false;
                }

                if (_endOfHierarchy[i] != endOf[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static List<VisualElement> GetParentHierarchy(VisualElement selectedElement)
        {
            List<VisualElement> hierarchy = new List<VisualElement>();
            hierarchy.Add(selectedElement);

            VisualElement currentElement = selectedElement;

            while (currentElement.parent != null)
            {
                VisualElement parent = currentElement.parent;
                hierarchy.Add(parent);
                currentElement = parent;
            }

            TrimHierarchy(ref hierarchy, x => x.name);
            hierarchy.Reverse();
            return hierarchy;
        }

        private static void TrimHierarchy<T>(ref List<T> hierarchy, Func<T, string> propertySelector)
        {
            for (int i = 0; i < hierarchy.Count - (_endOfHierarchy.Length - 1); i++)
            {
                string[] endOfHierarchy = new string[_endOfHierarchy.Length];
                for (int j = 0; j < _endOfHierarchy.Length; j++)
                {
                    endOfHierarchy[j] = propertySelector(hierarchy[i + j]);
                }

                if (ValidateEndOfHierarchy(endOfHierarchy))
                {
                    hierarchy.RemoveRange(i, hierarchy.Count - i);
                    break;
                }
            }
        }

        private static XElement FindElementByHierarchy(XElement root, List<string> hierarchy)
        {
            string currentName = root.Attribute("name")?.Value ?? string.Empty;

            if (_debug)
                Debug.Log($"Searching in the element with attribute name='{currentName}' for {hierarchy[0]}");

            if (currentName == hierarchy[0])
            {
                if (_debug)
                    Debug.Log($"Match found: {currentName}");

                if (hierarchy.Count == 1)
                {
                    if (_debug)
                        Debug.Log("Final element found.");

                    return root;
                }
                else
                {
                    List<string> subHierarchy = hierarchy.GetRange(1, hierarchy.Count - 1);
                    foreach (XElement child in root.Elements())
                    {
                        if (_debug)
                            Debug.Log($"Moving to the child element with attribute name='{child.Attribute("name")?.Value}'");

                        XElement found = FindElementByHierarchy(child, subHierarchy);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }
            else
            {
                foreach (XElement child in root.Elements())
                {
                    if (_debug)
                        Debug.Log($"Moving to the child element with attribute name='{child.Attribute("name")?.Value}' in search of {hierarchy[0]}");

                    XElement found = FindElementByHierarchy(child, hierarchy);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            if (_debug)
                Debug.Log($"Element with attribute name='{hierarchy[0]}' not found in the current context {currentName}");

            return null;
        }
    }
}
