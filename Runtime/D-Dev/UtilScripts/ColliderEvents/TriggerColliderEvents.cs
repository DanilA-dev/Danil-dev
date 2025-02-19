using UniRx;
using UniRx.Triggers;

namespace D_Dev.UtilScripts.ColliderEvents
{
    public class TriggerColliderEvents : BaseColliderEvents
    {
        #region Override

        protected override void InitColliderEvents()
        {
            if (_checkEnter)
                _rigidbody.OnTriggerEnterAsObservable()
                    .Subscribe((c) =>
                    {
                        var passed = _colliderChecker.IsColliderPassed(c);
                        
                        if(passed)
                            OnEnter?.Invoke(c);
                        
                        DebugCollider(c, passed);
                    });
            
            if (_checkExit)
                _rigidbody.OnTriggerExitAsObservable()
                    .Subscribe((c) =>
                    {
                        var passed = _colliderChecker.IsColliderPassed(c);
                        
                        if(passed)
                            OnExit?.Invoke(c);
                        
                        DebugCollider(c, passed);
                    });
        }

        #endregion
    }
}