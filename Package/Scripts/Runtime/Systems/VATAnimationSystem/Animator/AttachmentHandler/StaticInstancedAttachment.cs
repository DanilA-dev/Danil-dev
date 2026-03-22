using D_Dev.UpdateManagerSystem;
using UnityEngine;

namespace D_Dev.VATAnimationSystem
{
    public class StaticInstancedAttachment : MonoBehaviour, ITickable
    {
        #region Fields

        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Material _material;

        private VertexAnimationsInstancedRenderer.StaticInstance _instance;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (VertexAnimationsInstancedRenderer.Instance == null) return;
            if (_meshFilter == null || _material == null) return;

            _instance = VertexAnimationsInstancedRenderer.Instance.AddStaticInstance(
                _meshFilter.sharedMesh, _material, transform.localToWorldMatrix);

            UpdateManager.Add(this);
        }

        private void OnDisable()
        {
            UpdateManager.Remove(this);

            if (VertexAnimationsInstancedRenderer.Instance != null && _instance != null)
                VertexAnimationsInstancedRenderer.Instance.RemoveStaticInstance(
                    _meshFilter.sharedMesh, _material, _instance);
        }

        private void OnEnable()
        {
            if (_instance == null || VertexAnimationsInstancedRenderer.Instance == null) return;

            VertexAnimationsInstancedRenderer.Instance.AddExistingStaticInstance(
                _meshFilter.sharedMesh, _material, _instance);

            UpdateManager.Add(this);
        }

        private void OnDestroy()
        {
            UpdateManager.Remove(this);

            if (VertexAnimationsInstancedRenderer.Instance != null && _instance != null)
                VertexAnimationsInstancedRenderer.Instance.RemoveStaticInstance(
                    _meshFilter.sharedMesh, _material, _instance);
        }

        #endregion

        #region ITickable

        public void Tick()
        {
            if (this == null || _instance == null) return;
            _instance.Matrix = transform.localToWorldMatrix;
        }

        #endregion
    }
}