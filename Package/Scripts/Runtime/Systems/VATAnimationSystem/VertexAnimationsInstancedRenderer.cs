using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace D_Dev.VATAnimationSystem
{
    public class VertexAnimationsInstancedRenderer : MonoBehaviour
    {
        #region Classes

        public class VATInstance
        {
            public Matrix4x4 Matrix;
            public VertexAnimationClipInfo Clip;
            public float Time;
            public bool Playing;
        }

        #endregion

        #region Fields

        [SerializeField] private VertexAnimationData _data;
        [SerializeField] private Material _material;
        [SerializeField] private int _layer;
        [SerializeField] private ShadowCastingMode _shadowCasting = ShadowCastingMode.Off;
        [SerializeField] private bool _receiveShadows;

        private readonly List<VATInstance> _vatInstances = new();
        private Matrix4x4[] _matrixBuffer = new Matrix4x4[1023];
        private float[] _frameBuffer = new float[1023];
        private MaterialPropertyBlock _props;

        private static readonly int CurrentFrameId = Shader.PropertyToID("_CurrentFrame");
        private static readonly int TotalFramesId = Shader.PropertyToID("_TotalFrames");

        private const int BatchSize = 1023;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _props = new MaterialPropertyBlock();

            if (_data != null)
                _material.SetFloat(TotalFramesId, _data.TotalFrames);
        }

        private void Update()
        {
            if (_vatInstances.Count == 0)
                return;

            float dt = Time.deltaTime;

            for (int i = 0; i < _vatInstances.Count; i++)
            {
                var unit = _vatInstances[i];
                if (!unit.Playing || unit.Clip == null)
                    continue;

                unit.Time += dt;
                if (unit.Time >= unit.Clip.Duration)
                    unit.Time %= unit.Clip.Duration;
            }

            DrawAll();
        }

        #endregion

        #region Public

        public VATInstance AddUnit(Matrix4x4 matrix, string clipName = null)
        {
            var unit = new VATInstance
            {
                Matrix = matrix,
                Playing = true
            };

            if (!string.IsNullOrEmpty(clipName) && _data != null)
                _data.TryGetClip(clipName, out unit.Clip);

            _vatInstances.Add(unit);
            return unit;
        }

        public void RemoveUnit(VATInstance unit)
        {
            _vatInstances.Remove(unit);
        }

        public void Clear()
        {
            _vatInstances.Clear();
        }

        public int UnitCount => _vatInstances.Count;

        #endregion

        #region Private

        private void DrawAll()
        {
            int total = _vatInstances.Count;
            int offset = 0;

            while (offset < total)
            {
                int count = Mathf.Min(BatchSize, total - offset);

                for (int i = 0; i < count; i++)
                {
                    var unit = _vatInstances[offset + i];
                    _matrixBuffer[i] = unit.Matrix;
                    _frameBuffer[i] = CalculateFrame(unit);
                }

                _props.SetFloatArray(CurrentFrameId, _frameBuffer);

                Graphics.DrawMeshInstanced(
                    _data.BaseMesh,
                    0,
                    _material,
                    _matrixBuffer,
                    count,
                    _props,
                    _shadowCasting,
                    _receiveShadows,
                    _layer
                );

                offset += count;
            }
        }

        private float CalculateFrame(VATInstance unit)
        {
            if (unit.Clip == null) return 0f;
            float t = unit.Clip.Duration > 0f ? unit.Time / unit.Clip.Duration : 0f;
            return unit.Clip.StartFrame + t * (unit.Clip.FrameCount - 1);
        }

        #endregion
    }
}
