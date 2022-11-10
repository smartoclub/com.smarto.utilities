namespace Smarto.Extensions
{
    using System;
    using System.Collections;
    
    using UnityEngine;
    using UnityEngine.Events;
    
    using TMPro;

    public static class TMProExtensions
    {
        /// <summary>
        /// <para>Create a "typewriter" effect co-routine where the text appears character by character.</para>
        /// <para>Based upon Yarn Spinner's implementation of the typewriting effect.</para>
        /// </summary>
        /// <param name="text">The TMP component that will be displaying the effect.</param>
        /// <param name="charactersPerSecond">The amount of characters that should by typewritten per second.</param>
        /// <param name="onCharacterTyped">Optional UnityEvent that will be invoked whenever a character is typed. Includes the typed character.</param>
        /// <param name="setup">Optional Action that will be invoked before the text begins typing.</param>
        /// <returns>A co-routine. Don't forget to start it!</returns>
        public static IEnumerator RunTypewriter(this TextMeshProUGUI text, int charactersPerSecond, UnityEvent<char> onCharacterTyped=null, Action setup=null)
        {
            text.maxVisibleCharacters = 0;

            yield return null;

            setup.Invoke();

            int characterCount = text.textInfo.characterCount;

            if (charactersPerSecond <= 0 || characterCount == 0)
            {
                text.maxVisibleCharacters = characterCount;
                yield break;
            }

            float secondsPerLetter = 1.0f / charactersPerSecond;

            float accumulator = Time.deltaTime;

            while (text.maxVisibleCharacters < characterCount)
            {
                while (accumulator >= secondsPerLetter)
                {
                    text.maxVisibleCharacters += 1;
                    accumulator -= secondsPerLetter;

                    onCharacterTyped?.Invoke(text.text[Mathf.Clamp(text.maxVisibleCharacters - 1, 0, text.text.Length - 1)]);
                }

                accumulator += Time.deltaTime;

                yield return null;
            }              
        }
    }
}