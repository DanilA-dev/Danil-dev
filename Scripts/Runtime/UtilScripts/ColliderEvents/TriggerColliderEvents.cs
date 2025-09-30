using D_Dev.ColliderChecker;
using D_Dev.ColliderEvents;
using UniRx;
using UniRx.Triggers;

namespace D_Dev.ColliderEvents
{
    public class TriggerColliderEvents : BaseColliderEvents
    {
        #region Override

        protected override void InitColliderEvents()
        {
            if (_collisionDimension == CollisionDimension.Collider3d)
            {
                if (_checkEnter)
                    _rigidbody.OnTriggerEnterAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c);

                            if (passed)
                            {
                                OnEnter?.Invoke(c);
                                Colliders.Add(c);
                            }
                        });

                if (_checkExit)
                    _rigidbody.OnTriggerExitAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c);

                            if (passed)
                            {
                                OnExit?.Invoke(c);
                                if (Colliders.Contains(c))
                                    Colliders.Remove(c);
                            }
                        });
            }
            else 
            {
                if (_checkEnter)
                    _rigidbody2D.OnTriggerEnter2DAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c);

                            if (passed)
                            {
                                OnEnter2D?.Invoke(c);
                                Colliders2D.Add(c);
                            }
                        });

                if (_checkExit)
                    _rigidbody2D.OnTriggerExit2DAsObservable()
                        .Subscribe((c) =>
                        {
                            var passed = _colliderChecker.IsColliderPassed(c);

                            if (passed)
                            {
                                OnExit2D?.Invoke(c);
                                if (Colliders2D.Contains(c))
                                    Colliders2D.Remove(c);
                            }
                        });
            }
        }

        #endregion
    }
}
