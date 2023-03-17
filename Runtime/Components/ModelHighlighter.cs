namespace Smarto.Components
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;
    
    /// <summary>
    /// Allows you to highlight a model via script by increasing value (brightness) of the material colors.
    /// </summary>
    [TypeInfoBox("Allows you to highlight a model via script by increasing the value (brightness) of the material colors.")]
    public class HighlightModel : MonoBehaviour
    {
        [SerializeField] List<Renderer> renderers;
        [SerializeField] float valueDelta;

        List<Material> highlightMaterials = new List<Material>();
        List<Color> originalColors;

        void Start()
        {
            foreach (Renderer renderer in renderers)
            {
                highlightMaterials.AddRange(renderer.materials);
            }

            // Isn't there a cleaner way to create a list of certain size?
            originalColors = new Color[highlightMaterials.Count].ToList();
        }

        public void EnableHighlight()
        {
            for (int i = 0; i < highlightMaterials.Count; i++)
            {
                originalColors[i] = highlightMaterials[i].color;

                float h, s, v;

                Color.RGBToHSV(highlightMaterials[i].color, out h, out s, out v);

                highlightMaterials[i].color = Color.HSVToRGB(h, s, v + valueDelta);
            }
        }

        public void DisableHighlight()
        {
            for (int i = 0; i < highlightMaterials.Count; i++)
            {
                highlightMaterials[i].color = originalColors[i];
            }
        }
    }
}