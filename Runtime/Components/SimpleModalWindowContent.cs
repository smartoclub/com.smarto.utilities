namespace Smarto.Components
{
    using UnityEngine;

    /// <summary>
    /// Scriptable object that holds the content of a modal window. Can be inherited to exted the content.
    /// </summary>
    public class SimpleModalWindowContentList : ScriptableObject
    {
        public SimpleModalWindowContent[] Content;
    }

    public class SimpleModalWindowContent
    {
        public string Text;
    }
}