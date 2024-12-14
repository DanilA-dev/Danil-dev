using UniRx;
using UniRx.Triggers;

namespace Project.Scripts.ColliderEvents
{
    public class CollisionColliderEvents : BaseColliderEvents
    {
        protected override void InitColliderEvents()
        {
            if (_checkEnter)
                _rigidbody.OnCollisionEnterAsObservable()
                    .Subscribe((c) =>
                    {
                        var passed = _colliderChecker.IsColliderPassed(c.collider);
                        
                        if(passed)
                            OnEnter?.Invoke(c.collider);
                        
                        DebugCollider(c.collider, passed);
                    });
            
            if (_checkExit)
                _rigidbody.OnCollisionExitAsObservable()
                    .Subscribe((c) =>
                    {
                        var passed = _colliderChecker.IsColliderPassed(c.collider);
                        
                        if(passed)
                            OnExit?.Invoke(c.collider);
                        
                        DebugCollider(c.collider, passed);
                    });
        }
    }
}