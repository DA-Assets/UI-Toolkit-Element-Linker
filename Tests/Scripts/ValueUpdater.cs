using System;
using UnityEngine;

namespace DA_Assets.ULB
{
    public class ValueUpdater : MonoBehaviour
    {
        public double DoubleValue;
        public BindingSampleEnum EnumValue;
        public float FloatValue;
        public Sprite SpriteValue;
        public int IntegerValue;
        public long LongValue;
        public float ProgressValue;
        public bool RadioValue;
        public float SliderValue;
        public int SliderIntValue;
        public string TextFieldValue;
        public Vector2 Vector2Value;
        public Vector2Int Vector2IntValue;
        public Vector3 Vector3Value;
        public Vector3Int Vector3IntValue;
        public Vector4 Vector4Value;

        public BindingSnapshot LastSnapshot { get; private set; }

        private Texture2D _spriteTexture;
        private int _enumCount;
        private float _timer;
        private int _counter;
        private const float UpdateInterval = 1f;

        void Awake()
        {
            _enumCount = Enum.GetValues(typeof(BindingSampleEnum)).Length;
            UpdateValues();
            UpdateSnapshot();
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= UpdateInterval)
            {
                _timer -= UpdateInterval;
                UpdateValues();
                UpdateSnapshot();
            }
        }

        public BindingSnapshot GenerateSnapshot()
        {
            return new BindingSnapshot
            {
                DoubleValue = this.DoubleValue,
                EnumValue = this.EnumValue,
                FloatValue = this.FloatValue,
                SpriteValue = this.SpriteValue,
                IntegerValue = this.IntegerValue,
                LongValue = this.LongValue,
                ProgressValue = this.ProgressValue,
                RadioValue = this.RadioValue,
                SliderValue = this.SliderValue,
                SliderIntValue = this.SliderIntValue,
                TextFieldValue = this.TextFieldValue,
                Vector2Value = this.Vector2Value,
                Vector2IntValue = this.Vector2IntValue,
                Vector3Value = this.Vector3Value,
                Vector3IntValue = this.Vector3IntValue,
                Vector4Value = this.Vector4Value,
            };
        }

        private void UpdateSnapshot()
        {
            LastSnapshot = GenerateSnapshot();
        }

        void UpdateValues()
        {
            _counter++;
            DoubleValue = (_counter * 0.1d) % 100d;
            EnumValue = (BindingSampleEnum)((_counter) % _enumCount);
            FloatValue = (_counter * 0.1f) % 50f;

            EnsureSprite();
            Color color = Color.HSVToRGB((_counter * 0.05f) % 1.0f, 0.8f, 1f);
            PaintTexture(color);

            IntegerValue = _counter;
            LongValue = _counter * 1000L;
            ProgressValue = _counter % 101f;
            RadioValue = (_counter % 2 == 0);
            SliderValue = (_counter * 0.1f) % 10f;
            SliderIntValue = _counter % 21 - 10;
            TextFieldValue = $"Text {_counter}";
            Vector2Value = new Vector2(_counter, _counter * 2);
            Vector2IntValue = new Vector2Int(_counter, _counter * 2);
            Vector3Value = new Vector3(_counter, _counter * 2, _counter * 3);
            Vector3IntValue = new Vector3Int(_counter, _counter * 2, _counter * 3);
            Vector4Value = new Vector4(_counter, _counter * 2, _counter * 3, _counter * 4);
        }

        void EnsureSprite()
        {
            if (SpriteValue != null) return;
            _spriteTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            _spriteTexture.wrapMode = TextureWrapMode.Clamp;
            SpriteValue = Sprite.Create(_spriteTexture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f));
        }
        void PaintTexture(Color color)
        {
            if (_spriteTexture == null) return;
            Color[] pixels = new Color[_spriteTexture.width * _spriteTexture.height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            _spriteTexture.SetPixels(pixels);
            _spriteTexture.Apply();
        }
    }

    public enum BindingSampleEnum
    {
        First,
        Second,
        Third,
        Fourth
    }

    public struct BindingSnapshot
    {
        public double DoubleValue;
        public BindingSampleEnum EnumValue;
        public float FloatValue;
        public Sprite SpriteValue;
        public int IntegerValue;
        public long LongValue;
        public float ProgressValue;
        public bool RadioValue;
        public float SliderValue;
        public int SliderIntValue;
        public string TextFieldValue;
        public Vector2 Vector2Value;
        public Vector2Int Vector2IntValue;
        public Vector3 Vector3Value;
        public Vector3Int Vector3IntValue;
        public Vector4 Vector4Value;
    }
}