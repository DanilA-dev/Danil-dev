using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace D_Dev.LocalizationSystem
{
    public class LocalizationManager : MonoBehaviour
    {
        #region Fields

        private const string DefaultLocaleCode = "en";

        [SerializeField] private bool _autoAssignLocale;
        [ShowIf(nameof(_autoAssignLocale))]
        [SerializeReference] private ILocaleAutoAssigner _localeAutoAssigner;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (_autoAssignLocale)
                TryAssignAutoLocale();
        }

        #endregion

        #region Public

        public static void ChangeLocale(Locale locale)
        {
            if (locale != null)
                LocalizationSettings.SelectedLocale = locale;
        }

        #endregion

        #region Private

        private void TryAssignAutoLocale()
        {
            Locale locale = _localeAutoAssigner?.GetLocale() ?? GetDefaultLocale();
            
            if (locale != null)
                LocalizationSettings.SelectedLocale = locale;
        }

        private static Locale GetDefaultLocale()
        {
            return LocalizationSettings.AvailableLocales.GetLocale(DefaultLocaleCode);
        }

        #endregion
    }
}