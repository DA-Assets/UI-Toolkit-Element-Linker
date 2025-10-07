using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class UitkLinkerBindingTests
    {
        private GameObject _componentCreatorGO;
        private ValueUpdater _valueUpdater;
        private UIDocument _uiDocument;

        [SetUp]
        public void Setup()
        {
            _componentCreatorGO = new GameObject(nameof(ComponentCreator));

            GameObject uiDocGO = new GameObject(nameof(UIDocument));
            _uiDocument = uiDocGO.AddComponent<UIDocument>();
            _uiDocument.visualTreeAsset = Resources.Load<VisualTreeAsset>("TestBindings");
            _uiDocument.panelSettings = Resources.Load<PanelSettings>("TestPanelSettings");

            GameObject providerGO = new GameObject(nameof(ValueUpdater));
            _valueUpdater = providerGO.AddComponent<ValueUpdater>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_componentCreatorGO);
            Object.Destroy(_valueUpdater.gameObject);
            Object.Destroy(_uiDocument.gameObject);
        }

        [UnityTest]
        public IEnumerator AllBindings_UpdateCorrectly_Over5Seconds()
        {
            ComponentCreator.Initialize(_uiDocument, _valueUpdater, _componentCreatorGO);

            const int snapshotCount = 5;
            const float snapshotInterval = 1.0f;

            for (int i = 0; i < snapshotCount; i++)
            {
                yield return new WaitForSeconds(snapshotInterval);

                BindingSnapshot expected = _valueUpdater.LastSnapshot;
                BindingSnapshot actual = ComponentCreator.CaptureActualSnapshot();

                string assertMessage = $"Snapshot #{i + 1} failed.";
                Assert.AreEqual(expected.DoubleValue, actual.DoubleValue, 0.001, $"{assertMessage} (DoubleValue)");
                Assert.AreEqual(expected.EnumValue, actual.EnumValue, $"{assertMessage} (EnumValue)");
                Assert.AreEqual(expected.FloatValue, actual.FloatValue, 0.001f, $"{assertMessage} (FloatValue)");
                Assert.AreEqual(expected.SpriteValue, actual.SpriteValue, $"{assertMessage} (SpriteValue)");
                Assert.AreEqual(expected.IntegerValue, actual.IntegerValue, $"{assertMessage} (IntegerValue)");
                Assert.AreEqual(expected.LongValue, actual.LongValue, $"{assertMessage} (LongValue)");
                Assert.AreEqual(expected.ProgressValue, actual.ProgressValue, 0.001f, $"{assertMessage} (ProgressValue)");
                Assert.AreEqual(expected.RadioValue, actual.RadioValue, $"{assertMessage} (RadioValue)");
                Assert.AreEqual(expected.SliderValue, actual.SliderValue, 0.001f, $"{assertMessage} (SliderValue)");
                Assert.AreEqual(expected.SliderIntValue, actual.SliderIntValue, $"{assertMessage} (SliderIntValue)");
                Assert.AreEqual(expected.TextFieldValue, actual.TextFieldValue, $"{assertMessage} (TextFieldValue)");
                Assert.AreEqual(expected.Vector2Value, actual.Vector2Value, $"{assertMessage} (Vector2Value)");
                Assert.AreEqual(expected.Vector2IntValue, actual.Vector2IntValue, $"{assertMessage} (Vector2IntValue)");
                Assert.AreEqual(expected.Vector3Value, actual.Vector3Value, $"{assertMessage} (Vector3Value)");
                Assert.AreEqual(expected.Vector3IntValue, actual.Vector3IntValue, $"{assertMessage} (Vector3IntValue)");
                Assert.AreEqual(expected.Vector4Value, actual.Vector4Value, $"{assertMessage} (Vector4Value)");
            }

            Assert.Pass("All 5 snapshots were successful.");
        }
    }
}