﻿using System;

namespace CustomFSM.Preicate
{
    public class FuncPredicate : IPredicate
    {
        private Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        public bool CanBeUpdated { get; set; } = true;
        public bool Evaluate() => _func.Invoke();
    }
}