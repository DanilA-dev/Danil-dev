using D_Dev.UpdateManagerSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.VATAnimationSystem
{
    public class StaticInstancedAttachment : MonoBehaviour, ITickable
    {
        #region Fields

        [Title("Disabled Components")] 
        [SerializeField] private Renderer _renderer;
        [Title("Base")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Material _material;

        private VertexAnimationsInstancedRenderer.StaticInstance _instance;

        #endregion

        #region Monobehaviour

        private void Reset()
        {
            if(TryGetComponent(out _renderer))
                _material = _renderer.sharedMaterial;
            
            TryGetComponent(out _meshFilter);
        }

        private void Start()
        {
            UpdateManager.Add(this);

            
            if (VertexAnimationsInstancedRenderer.Instance == null)
                return;
            
            if (_meshFilter == null || _material == null)
                return;

            _renderer.enabled = false;
            _instance = VertexAnimationsInstancedRenderer.Instance.AddStaticInstance(
                _meshFilter.sharedMesh, _material, transform.localToWorldMatrix);
        }


        private void OnEnable()
        {
            if (_instance == null || VertexAnimationsInstancedRenderer.Instance == null)
                return;

            VertexAnimationsInstancedRenderer.Instance.AddExistingStaticInstance(
                _meshFilter.sharedMesh, _material, _instance);
        }

        private void OnDisable()
        {
            if (VertexAnimationsInstancedRenderer.Instance != null && _instance != null)
                VertexAnimationsInstancedRenderer.Instance.RemoveStaticInstance(
                    _meshFilter.sharedMesh, _material, _instance);
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
            if (this == null || _instance == null)
                return;
            
            _instance.Matrix = transform.localToWorldMatrix;
        }

        #endregion
    }
}