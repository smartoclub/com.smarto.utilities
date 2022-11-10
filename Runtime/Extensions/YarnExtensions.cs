namespace Smarto.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Events;

    using TMPro;
    using Yarn.Unity;

    public static class YarnExtensions
    {
        /// <summary>
        /// <para>Variation of the "typewriter" effect with utilities for Yarn's LocalizedLine, namely adding a UnityEvent that has the line as a parameter.</para>
        /// </summary>
        /// <param name="text">The TMP component that will be displaying the effect.</param>
        /// <param name="charactersPerSecond">The amount of characters that should by typewritten per second.</param>
        /// <param name="localizedLine">The Yarn LocalizedLine that is being typed.</param>
        /// <param name="onCharacterTyped">The event that shall be invoked each time a character is typed.</param>
        /// <param name="setup">Optional Action that will be invoked before the text begins typing.</param>
        /// <returns>A co-routine. Don't forget to start it!</returns>
        public static IEnumerator RunTypewriter(this TextMeshProUGUI text, int charactersPerSecond, LocalizedLine localizedLine, UnityEvent<LocalizedLine, char> onCharacterTyped=null, Action setup=null)
        {
            UnityEvent<char> eventWrapper = new UnityEvent<char>();

            eventWrapper.AddListener((x) => onCharacterTyped.Invoke(localizedLine, x));
            
            return text.RunTypewriter(charactersPerSecond, eventWrapper, setup);
        }

        /// <summary>
        /// <para>Given a enumerable of metadata tags in the form "tag1:value1 tag2:value2" returns whether the enumerable contains the given tag.</para>
        /// <para>Works for node and line metadata.</para>
        /// </summary>
        /// <param name="metadata">A line or node metadata structure.</param>
        /// <param name="tag">The tag to look for.</param>
        /// <returns>True if the tag is contained in the enumerable, false otherwise.</returns>
        public static bool ContainsTag(this IEnumerable<string> metadata, string tag)
        {
            if (metadata == null)
                return false;
            
            foreach (string item in metadata)
            {
                if (item.Split(":")[0] == tag)
                {
                    return true;
                }
            }
            
            return false;
        }
    
        /// <summary>
        /// <para>Given a enumerable of metadata tags in the form "tag1:value1 tag2:value2" returns the value of the given tag.</para>
        /// <para>Works for node and line metadata.</para>
        /// </summary>
        /// <param name="metadata">A line or node metadata enumerable.</param>
        /// <param name="tag">The tag whose value we want.</param>
        /// <returns>The value of the tag as a string.</returns>
        public static string GetTagValue(this IEnumerable<string> metadata, string tag)
        {
            if (metadata == null)
                throw new System.ArgumentNullException();
    
            foreach (string item in metadata)
            {
                if (item.Split(":")[0] == tag)
                {
                    return item.Split(":")[1];
                }
            }
    
            throw new System.ArgumentOutOfRangeException("Tag not found in given metadata.");     
        }        
    }
}