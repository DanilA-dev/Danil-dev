using System;
using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.VATAnimationSystem
{
    public abstract class BaseVertexAnimationAttachmentHandler : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class BoneAttachment
        {
            public string BoneName;
            public Transform AttachRoot;
            
            public int BoneIndex { get; set; }
        }

        #endregion

        #region Fields

        [SerializeField] private VertexAnimationData _data;
        [SerializeField] private BaseVertexAnimator _animator;
        [SerializeField] private List<BoneAttachment> _attachments = new();

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_data == null)
                return;

            foreach (var attachment in _attachments)
                attachment.BoneIndex = _data.GetBoneIndex(attachment.BoneName);
        }

        #endregion

        #region Public

        public void UpdateAttachments()
        {
            if (_data == null || _data.BoneTexture == null)
                return;
            
            if (_animator == null || _animator.CurrentClip == null)
                return;

            int frame = GetCurrentFrame();

            foreach (var attachment in _attachments)
            {
                if (attachment.AttachRoot == null || attachment.BoneIndex < 0) continue;

                ReadBoneTexture(attachment.BoneIndex, frame, out Vector3 pos, out Quaternion rot);
                attachment.AttachRoot.position = pos;
                attachment.AttachRoot.rotation = rot;
            }
        }

        #endregion

        #region Private

        private int GetCurrentFrame()
        {
            var clip = _animator.CurrentClip;
            float t = Mathf.Clamp01(_animator.NormalizedTime);
            return Mathf.RoundToInt(clip.StartFrame + t * (clip.FrameCount - 1));
        }

        private void ReadBoneTexture(int boneIndex, int frame, out Vector3 pos, out Quaternion rot)
        {
            int texX = boneIndex * 2;
            Color posData = _data.BoneTexture.GetPixel(texX, frame);
            Color rotData = _data.BoneTexture.GetPixel(texX + 1, frame);

            pos = new Vector3(posData.r, posData.g, posData.b);
            rot = new Quaternion(rotData.r, rotData.g, rotData.b, rotData.a);
        }

        #endregion
    }
}