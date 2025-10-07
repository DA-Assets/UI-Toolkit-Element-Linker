using UnityEngine;
using UnityEngine.UIElements;

namespace DA_Assets.ULB
{
    public class ComponentCreator
    {
        const string DoubleGuid = "2abf6d5f4a2840f7873a9ef3245b62f1";
        const string EnumGuid = "7ebc4f4a7b2245c4a9df77af87b61a1a";
        const string FloatGuid = "c674d1b4d42d45018a4bfc651c01fbf6";
        const string ImageGuid = "14792e6be8744c8dbac8c484dd09cb3a";
        const string IntegerGuid = "0b21f1b223db497dbc8c20c6cd9ca520";
        const string LongGuid = "a03f5ce20daf4fffa44f14f7a64de795";
        const string ProgressGuid = "f7f38102afde4d86a1ff6bd739f8ed9d";
        const string RadioGuid = "2c7bb8fd9ea24313ad87750c124a1b63";
        const string SliderGuid = "88b8695b3d6f40d2b24ed8c3db8a5f95";
        const string SliderIntGuid = "ad0f5e2c9b2a4fe59bdf4b7a8249135c";
        const string TextGuid = "61f019adcc6c45818b46485f1b852d07";
        const string Vector2Guid = "5fc50b30f8224c0083aa7bb3cba5a4eb";
        const string Vector2IntGuid = "f8540368be404d1485d2bdfb1ef82b5c";
        const string Vector3Guid = "1a895f9fdf0a47b0a30734d3b90be671";
        const string Vector3IntGuid = "f4d8364073ae4dc7b37ed50d65ed90b0";
        const string Vector4Guid = "cad255502e8441399b8fba29cb96627b";
        private static GameObject _harnessGO;
        private static UitkDoubleField _doubleLinker;
        private static UitkEnumField _enumLinker;
        private static UitkFloatField _floatLinker;
        private static UitkImage _imageLinker;
        private static UitkIntegerField _integerLinker;
        private static UitkLongField _longLinker;
        private static UitkProgressBar _progressLinker;
        private static UitkRadioButton _radioLinker;
        private static UitkSlider _sliderLinker;
        private static UitkSliderInt _sliderIntLinker;
        private static UitkTextField _textLinker;
        private static UitkVector2Field _vector2Linker;
        private static UitkVector2IntField _vector2IntLinker;
        private static UitkVector3Field _vector3Linker;
        private static UitkVector3IntField _vector3IntLinker;
        private static UitkVector4Field _vector4Linker;

        public static void Initialize(UIDocument document, ValueUpdater provider, GameObject harnessGO)
        {
            _harnessGO = harnessGO;

            _doubleLinker = CreateLinker<UitkDoubleField>(document, provider, DoubleGuid, nameof(ValueUpdater.DoubleValue));
            _enumLinker = CreateLinker<UitkEnumField>(document, provider, EnumGuid, nameof(ValueUpdater.EnumValue));
            _floatLinker = CreateLinker<UitkFloatField>(document, provider, FloatGuid, nameof(ValueUpdater.FloatValue));
            _imageLinker = CreateLinker<UitkImage>(document, provider, ImageGuid, nameof(ValueUpdater.SpriteValue));
            _integerLinker = CreateLinker<UitkIntegerField>(document, provider, IntegerGuid, nameof(ValueUpdater.IntegerValue));
            _longLinker = CreateLinker<UitkLongField>(document, provider, LongGuid, nameof(ValueUpdater.LongValue));
            _progressLinker = CreateLinker<UitkProgressBar>(document, provider, ProgressGuid, nameof(ValueUpdater.ProgressValue));
            _radioLinker = CreateLinker<UitkRadioButton>(document, provider, RadioGuid, nameof(ValueUpdater.RadioValue));
            _sliderLinker = CreateLinker<UitkSlider>(document, provider, SliderGuid, nameof(ValueUpdater.SliderValue));
            _sliderIntLinker = CreateLinker<UitkSliderInt>(document, provider, SliderIntGuid, nameof(ValueUpdater.SliderIntValue));
            _textLinker = CreateLinker<UitkTextField>(document, provider, TextGuid, nameof(ValueUpdater.TextFieldValue));
            _vector2Linker = CreateLinker<UitkVector2Field>(document, provider, Vector2Guid, nameof(ValueUpdater.Vector2Value));
            _vector2IntLinker = CreateLinker<UitkVector2IntField>(document, provider, Vector2IntGuid, nameof(ValueUpdater.Vector2IntValue));
            _vector3Linker = CreateLinker<UitkVector3Field>(document, provider, Vector3Guid, nameof(ValueUpdater.Vector3Value));
            _vector3IntLinker = CreateLinker<UitkVector3IntField>(document, provider, Vector3IntGuid, nameof(ValueUpdater.Vector3IntValue));
            _vector4Linker = CreateLinker<UitkVector4Field>(document, provider, Vector4Guid, nameof(ValueUpdater.Vector4Value));
        }

        public static BindingSnapshot CaptureActualSnapshot()
        {
            return new BindingSnapshot
            {
                DoubleValue = _doubleLinker.Element.value,
                EnumValue = (BindingSampleEnum)_enumLinker.Element.value,
                FloatValue = _floatLinker.Element.value,
                SpriteValue = _imageLinker.Element.sprite,
                IntegerValue = _integerLinker.Element.value,
                LongValue = _longLinker.Element.value,
                ProgressValue = _progressLinker.Element.value,
                RadioValue = _radioLinker.Element.value,
                SliderValue = _sliderLinker.Element.value,
                SliderIntValue = _sliderIntLinker.Element.value,
                TextFieldValue = _textLinker.Element.value,
                Vector2Value = _vector2Linker.Element.value,
                Vector2IntValue = _vector2IntLinker.Element.value,
                Vector3Value = _vector3Linker.Element.value,
                Vector3IntValue = _vector3IntLinker.Element.value,
                Vector4Value = _vector4Linker.Element.value
            };
        }

        private static T CreateLinker<T>(UIDocument doc, ValueUpdater provider, string guid, string fieldName) where T : UitkLinkerBase
        {
            var go = new GameObject(typeof(T).Name);
            go.transform.SetParent(_harnessGO.transform);

            var linker = go.AddComponent<T>();

            linker.UIDocument = doc;
            linker.LinkingMode = UitkLinkingMode.Guid;
            linker.Guid = guid;

            if (linker is IUITKLinkerWithBinding binder)
            {
                binder.Initialize();
                binder.BindingRateMs = new Vector2Int(0, 0);
                binder.BindingEnabled = true;
                binder.Binding = new MemberBinding
                {
                    TargetObject = provider.gameObject,
                    Member = $"{nameof(ValueUpdater)}/{fieldName}"
                };
            }

            return linker;
        }
    }
}