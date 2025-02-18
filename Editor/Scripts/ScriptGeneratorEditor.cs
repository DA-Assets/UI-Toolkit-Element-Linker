using UnityEngine;
using UnityEditor;
using System.IO;

namespace DA_Assets.UEL
{
    public class ScriptGeneratorEditor : EditorWindow
    {
        private string[] classNames = new string[] { "BoundsField",
"BoundsIntField",
"Box",
"Button",

"DoubleField",
"DropdownField",
"EnumField",

"FloatField",
"Foldout",

"GroupBox",
"Hash128Field",
"HelpBox",
"IMGUIContainer",
"Image",

"IntegerField",
"Label",

"ListView",
"LongField",

"MinMaxSlider",
"MultiColumnListView",
"MultiColumnTreeView",

"PopupWindow",
"ProgressBar",

"RadioButton",
"RadioButtonGroup",
"RectField",
"RectIntField",
"RepeatButton",
"ScrollView",
"Scroller",
"Slider",
"SliderInt",

"TextElement",
"TextField",
"TemplateContainer",
"Toggle",

"TreeView",
"TwoPaneSplitView",
"UnsignedIntegerField",
"UnsignedLongField",
"Vector2Field",
"Vector2IntField",
"Vector3Field",
"Vector3IntField",
"Vector4Field",

"VisualElement" };

        //[MenuItem("Tools/Generate UI Toolkit Scripts")]
        private static void ShowWindow()
        {
            var window = GetWindow<ScriptGeneratorEditor>();
            window.titleContent = new GUIContent("Script Generator");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Generate Scripts"))
            {
                GenerateScripts();
            }
        }

        private void GenerateScripts()
        {
            string folderPath = Path.Combine("Assets", "GeneratedScripts");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var className in classNames)
            {
                string scriptContent = $@"using UnityEngine.UIElements;

namespace DA_Assets.UitkElementLinker
{{
    public class {className} : UnityEngine.UIElements.{className}, IHaveGuid
    {{
        public string Guid {{ get; set; }}

        public new class UxmlFactory : UxmlFactory<VisualElement, UxmlTraits> {{ }}

        public new class UxmlTraits : UnityEngine.UIElements.VisualElement.UxmlTraits
        {{
            UxmlStringAttributeDescription m_Guid = new UxmlStringAttributeDescription {{ name = nameof(Guid) }};

            public override void Init(UnityEngine.UIElements.{className} ve, IUxmlAttributes bag, CreationContext cc)
            {{
                base.Init(ve, bag, cc);

                {className} obj = ve as {className};
                obj.Guid = GuidGenerator.GenerateGuid();
            }}
        }}
    }}

    public class Uitk{className} : UitkLinker<Test.{className}>
    {{
        public UnityEngine.UIElements.{className} {className} => _element;
    }}
}}
";

                File.WriteAllText(Path.Combine(folderPath, $"Uitk{className}.cs"), scriptContent);
            }

            AssetDatabase.Refresh();
            Debug.Log("Scripts generated successfully.");
        }
    }
}