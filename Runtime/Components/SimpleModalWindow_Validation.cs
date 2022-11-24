namespace Smarto.Components
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public abstract partial class BaseModalWindow<T> : MonoBehaviour where T : ModalWindowContent
    {
        protected virtual void validateConfiguration()
        {
            Assert.IsNotNull(content, "[SimpleModalWindow] content can not be null.");
            Assert.IsTrue(content.Length > 0, "[SimpleModalWindow] content array is empty.");
            Assert.IsNotNull(contentText, "[SimpleModalWindow] contentText can not be null.");

            if (content.Length > 1)
            {
                Assert.IsNotNull(previousButton, "[SimpleModalWindow] content has more than one page, but previousButton has not been set.");
                Assert.IsNotNull(nextButton, "[SimpleModalWindow] content has more than one page, but nextButton has not been set.");

                if (previousButton.onClick.GetPersistentEventCount() == 0)
                {
                    if (fixButtonListeners)
                    {
                        previousButton.onClick.AddListener(PreviousPage);
                        
                        if (!supressWarnings)
                        {
                            Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to previousButton. Added PreviousPage().");
                        } 
                    }
                    else if (!supressWarnings)
                    {
                        Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to previousButton.");
                    }
                }

                if (nextButton.onClick.GetPersistentEventCount() == 0)
                {
                    if (fixButtonListeners)
                    {
                        nextButton.onClick.AddListener(NextPage);

                        if (!supressWarnings)
                        {
                            Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to nextButton. Added NextPage().");
                        }
                    }
                    else if (!supressWarnings)
                    {
                        Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to nextButton.");
                    }
                }                
            }

            if (closeButton != null && closeButton.onClick.GetPersistentEventCount() == 0)
            {
                if (fixButtonListeners)
                {
                    closeButton.onClick.AddListener(Close);

                    if (!supressWarnings)
                    {
                        Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to closeButton. Added Close().");
                    }                    
                }
                else if (!supressWarnings)
                {
                    Debug.LogWarning("[SimpleModalWindow] No persistent events have been added to closeButton.");
                }
            }
        }
    }
}