using D_Dev.ColliderChecker;
using UniRx;
using UniRx.Triggers;

namespace D_Dev.ColliderEvents
{
    public class CollisionColliderEvents : BaseColliderEvents
    {
        #region Override

        protected override void InitColliderEvents()
        {
            if (_collisionDimension == CollisionDimension.Collider3d)
            {
                if (_checkEnter)
                    _rigidbody.OnCollisionEnterAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c.collider);

                            if (passed)
                            {
                                OnEnter?.Invoke(c.collider);
                                Colliders.Add(c.collider);
                            }
                        });

                if (_checkExit)
                    _rigidbody.OnCollisionExitAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c.collider);

                            if (passed)
                            {
                                OnExit?.Invoke(c.collider);
                                if (Colliders.Contains(c.collider))
                                    Colliders.Remove(c.collider);
                            }
                        });
            }
            else 
            {
                if (_checkEnter)
                    _rigidbody2D.OnCollisionEnter2DAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c.collider);

                            if (passed)
                            {
                                OnEnter2D?.Invoke(c.collider);
                                Colliders2D.Add(c.collider);
                            }
                        });

                if (_checkExit)
                    _rigidbody2D.OnCollisionExit2DAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c.collider);

                            if (passed)
                            {
                                OnExit2D?.Invoke(c.collider);
                                if (Colliders2D.Contains(c.collider))
                                    Colliders2D.Remove(c.collider);
                            }
                        });
            }
        }

        #endregion
    }
}
