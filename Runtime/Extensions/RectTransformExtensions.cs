namespace Smarto.Extensions
{
    using UnityEngine;
    using UnityEngine.UI;

    public static class RectTransformExtensions
    {
        /// <summary>Calculates where to put dialogue bubble based on worldPosition and any desired screen margins. 
        /// Ensure "constrainToViewportMargin" is between 0.0f-1.0f (% of screen) to constrain to screen, or value of -1 lets bubble go off-screen.
        /// Adapted from https://github.com/YarnSpinnerTool/YarnSpinner-Unity/blob/main/Samples~/3D/Scripts/YarnCharacterView.cs </summary>
        public static Vector2 WorldToAnchoredPosition(this RectTransform bubble, Vector3 worldPos, Canvas canvas, Camera gameCamera, float constrainToViewportMargin = -1f)
        {   
            Camera canvasCamera = Camera.main;
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();

            // Canvas "Overlay" mode is special case for ScreenPointToLocalPointInRectangle (see the Unity docs)
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                canvasCamera = null; 
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle( 
                bubble.parent.GetComponent<RectTransform>(), // calculate local point inside parent... NOT inside the dialogue bubble itself
                gameCamera.WorldToScreenPoint(worldPos), 
                canvasCamera, 
                out Vector2 screenPos
            );

            // to force the dialogue bubble to be fully on screen, clamp the bubble rectangle within the screen bounds
            if (constrainToViewportMargin >= 0f)
            {
                // because ScreenPointToLocalPointInRectangle is relative to a Unity UI RectTransform,
                // it may not necessarily match the full screen resolution (i.e. CanvasScaler)

                // it's not really in world space or screen space, it's in a RectTransform "UI space"
                // so we must manually convert our desired screen bounds to this UI space

                bool useCanvasResolution = canvasScaler != null && canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize;
                Vector2 screenSize = Vector2.zero;
                screenSize.x = useCanvasResolution ? canvasScaler.referenceResolution.x : Screen.width;
                screenSize.y = useCanvasResolution ? canvasScaler.referenceResolution.y : Screen.height;

                // calculate "half" values because we are measuring margins based on the center, like a radius
                var halfBubbleWidth = bubble.rect.width / 2;
                var halfBubbleHeight = bubble.rect.height / 2;

                // to calculate margin in UI-space pixels, use a % of the smaller screen dimension
                var margin = screenSize.x < screenSize.y ? screenSize.x * constrainToViewportMargin : screenSize.y * constrainToViewportMargin;

                // finally, clamp the screenPos fully within the screen bounds, while accounting for the bubble's rectTransform anchors
                screenPos.x = Mathf.Clamp( 
                    screenPos.x,
                    margin + halfBubbleWidth - bubble.anchorMin.x * screenSize.x,
                    -(margin + halfBubbleWidth) - bubble.anchorMax.x * screenSize.x + screenSize.x
                );

                screenPos.y = Mathf.Clamp( 
                    screenPos.y, 
                    margin + halfBubbleHeight - bubble.anchorMin.y * screenSize.y, 
                    -(margin + halfBubbleHeight) - bubble.anchorMax.y * screenSize.y + screenSize.y
                );
            }

            return screenPos;
        }
    }
}