namespace Smarto.Components
{
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.EventSystems;

    using System;
    using System.Collections.Generic;
    using System.Linq;


    using TMPro;

    /// <summary>
    /// Allows the use of the TMP <link> tag to create a hyperlink in a UI.
    /// Adapted from https://gitlab.com/jonnohopkins/tmp-hyperlinks/-/blob/master/Assets/OpenHyperlinks.cs
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPHyperlink : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] bool doesColorChangeOnHover = true;
        [SerializeField] Color hoverColor = new Color(60f / 255f, 120f / 255f, 1f);

        int currentLink = -1;
        TextMeshProUGUI text;
        List<Color32[]> originalVertexColors = new List<Color32[]>();

        // We assume we're using a Overlay camera so we pass null as the camera argument.

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();

            Assert.IsNotNull(text, "[TMPHyperlink] No TextMeshProUGUI component was found.");
        }

        void LateUpdate()
        {
            if (!doesColorChangeOnHover)
                return;

            var isHoveringOver = TMP_TextUtilities.IsIntersectingRectTransform(text.rectTransform, Input.mousePosition, null);
            int linkIndex = isHoveringOver ? TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null) : -1;

            if (currentLink != -1 && linkIndex != currentLink)
            {
                setLinkToColor(currentLink, (linkIdx, vertIdx) => originalVertexColors[linkIdx][vertIdx]);
                originalVertexColors.Clear();
                currentLink = -1;
            }

            if (linkIndex != -1 && linkIndex != currentLink)
            {
                currentLink = linkIndex;
                originalVertexColors = setLinkToColor(linkIndex, (_linkIdx, _vertIdx) => hoverColor);
            }            
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
            
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }

        List<Color32[]> setLinkToColor(int linkIndex, Func<int, int, Color32> colorForLinkAndVert)
        {
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];

            var oldVertColors = new List<Color32[]>(); // store the old character colors

            for( int i = 0; i < linkInfo.linkTextLength; i++ )
            {
                int characterIndex = linkInfo.linkTextfirstCharacterIndex + i;
                var charInfo = text.textInfo.characterInfo[characterIndex];
                int meshIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
                oldVertColors.Add(vertexColors.ToArray());

                if( charInfo.isVisible ) {
                    vertexColors[vertexIndex + 0] = colorForLinkAndVert(i, vertexIndex + 0);
                    vertexColors[vertexIndex + 1] = colorForLinkAndVert(i, vertexIndex + 1);
                    vertexColors[vertexIndex + 2] = colorForLinkAndVert(i, vertexIndex + 2);
                    vertexColors[vertexIndex + 3] = colorForLinkAndVert(i, vertexIndex + 3);
                }
            }

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            return oldVertColors;
        }
    }
}