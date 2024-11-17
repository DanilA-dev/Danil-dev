using System;
using Entities;
using UnityEngine;

namespace Project.Scripts
{
    public class TestProjectile : PoolableEntity
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force;
        [SerializeField] private ForceMode _forceMode;

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.forward * _force, _forceMode);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TriggerEnter");
            Release();
        }
    }
}