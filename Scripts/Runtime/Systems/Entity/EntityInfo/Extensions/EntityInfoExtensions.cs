using D_Dev.ScriptableVaiables;

namespace D_Dev.Entity.Extensions
{
    public static class EntityInfoExtensions
    {
        public static TVariable GetVariable<TVariable>(this EntityInfo entityInfo,
            StringScriptableVariable id)
            where TVariable : BaseScriptableVariable<TVariable>
        {
            foreach (var variable in entityInfo.Variables)
            {
                if (variable.VariableID == id)
                    return variable as TVariable;
            }

            return null;
        }
    }
}