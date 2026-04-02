#if !ODIN_INSPECTOR
// Stub attributes so the package compiles without Odin Inspector.
// When Odin is installed, ODIN_INSPECTOR is defined and this file is excluded.
namespace Sirenix.OdinInspector
{
    using System;
    using System.Diagnostics;

    public enum ButtonStyle { CompactBox, Box }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() { }
        public ButtonAttribute(ButtonStyle style) { }
        public ButtonAttribute(string name) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ShowInInspectorAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ShowIfAttribute : Attribute
    {
        public ShowIfAttribute(string condition) { }
        public ShowIfAttribute(string condition, object value) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class HideIfAttribute : Attribute
    {
        public HideIfAttribute(string condition) { }
        public HideIfAttribute(string condition, object value) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class FoldoutGroupAttribute : Attribute
    {
        public FoldoutGroupAttribute(string groupName) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class PropertyOrderAttribute : Attribute
    {
        public PropertyOrderAttribute(float order) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class PropertySpaceAttribute : Attribute
    {
        public PropertySpaceAttribute() { }
        public PropertySpaceAttribute(float spaceBefore) { }
        public PropertySpaceAttribute(float spaceBefore, float spaceAfter) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class InfoBoxAttribute : Attribute
    {
        public InfoBoxAttribute(string message) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class RequiredAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ReadOnlyAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class HideLabelAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class GUIColorAttribute : Attribute
    {
        public GUIColorAttribute(float r, float g, float b, float a = 1f) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : Attribute
    {
        public OnValueChangedAttribute(string action) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ValidateInputAttribute : Attribute
    {
        public ValidateInputAttribute(string condition) { }
        public ValidateInputAttribute(string condition, string message) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class HorizontalGroupAttribute : Attribute
    {
        public HorizontalGroupAttribute() { }
        public HorizontalGroupAttribute(string groupName) { }
    }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class InlineEditorAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class PreviewFieldAttribute : Attribute { }

    [Conditional("ODIN_INSPECTOR")] [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ListDrawerSettingsAttribute : Attribute
    {
        public string ListElementLabelName { get; set; }
    }
}
#endif
