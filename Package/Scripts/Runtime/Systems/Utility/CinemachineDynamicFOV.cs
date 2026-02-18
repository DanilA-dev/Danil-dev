using Cinemachine;
using System.Collections;
using D_Dev.PolymorphicValueSystem;
using UnityEngine;

namespace D_Dev.Utility
{
    public class CinemachineDynamicFOV : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _minFovValue;
        [SerializeField] private float _maxFovValue;
        [SerializeReference] private PolymorphicValue<float> _fovTransitionDuration = new FloatConstantValue();
        [SerializeField] private CinemachineVirtualCamera _cm;

        private Coroutine _fovCoroutine;

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            if(_fovCoroutine != null)
                StopCoroutine(_fovCoroutine);
        }

        #endregion
        
        #region Public

        public void SetFOV(float targetFOV, float duration)
        {
            if (_cm == null)
                return;

            var clampedFOV = Mathf.Clamp(targetFOV, _minFovValue, _maxFovValue);

            if (_fovCoroutine != null)
                StopCoroutine(_fovCoroutine);

            _fovCoroutine = StartCoroutine(FOVTransitionCoroutine(clampedFOV, duration));
        }

        public void SetFOV(float targetFOV)
        {
            SetFOV(targetFOV, _fovTransitionDuration.Value);
        }

        public void SetFOVImmediate(float targetFOV)
        {
            if (_cm == null)
                return;

            if (_fovCoroutine != null)
            {
                StopCoroutine(_fovCoroutine);
                _fovCoroutine = null;
            }

            _cm.m_Lens.FieldOfView = Mathf.Clamp(targetFOV, _minFovValue, _maxFovValue);
        }

        public void SetMinFOV(float duration) => SetFOV(_minFovValue, duration);
        public void SetMinFOV() => SetFOV(_minFovValue);
        public void SetMaxFOV(float duration) => SetFOV(_maxFovValue, duration);
        public void SetMaxFOV() => SetFOV(_maxFovValue);

        #endregion

        #region Coroutine

        private IEnumerator FOVTransitionCoroutine(float targetFOV, float duration)
        {
            var startFOV = _cm.m_Lens.FieldOfView;
            var time = 0f;

            while (time < 1f)
            {
                time += Time.deltaTime / duration;
                time = Mathf.Clamp01(time);
                _cm.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
                yield return null;
            }

            _cm.m_Lens.FieldOfView = targetFOV;
            _fovCoroutine = null;
        }

        #endregion
    }
}