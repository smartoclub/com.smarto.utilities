namespace Smarto.Components
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    /// <summary>
    /// Base class for modal window in the Canvas with paginated content.
    /// Optionally, plays sounds if a RandomAudioQueue has been added.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public partial class SimpleModalWindow : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("If set to true, it will show the modal window on start.")]
        [SerializeField] protected bool showOnStart;
        [Tooltip("If showOnStart and this are to true, it will call Open() and play an opening animation. Otherwise, the modal will just appear instantly.")]
        [SerializeField] protected bool animateOnStartShow;
        [SerializeField] protected bool rememberPage;
        [Header("Validation")]
        [Tooltip("If set to true, the script will add the appropiate listeners to the buttons if none have been set in the inspector.")]
        [SerializeField] protected bool fixButtonListeners;
        [SerializeField] protected bool supressWarnings;
        [Header("Buttons")]
        [SerializeField] protected Button previousButton;
        [SerializeField] protected Button nextButton;
        [SerializeField] protected Button closeButton;
        [Header("Text")]
        [SerializeField] protected TextMeshProUGUI contentText;
        [SerializeField] protected TextMeshProUGUI numberText;
        [Header("Content")]
        [SerializeField] protected SimpleModalWindowContent content;
        
        protected CanvasGroup canvasGroup;
        protected RandomAudioQueue randomAudioQueue;

        protected int contentIndex;

        protected virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            randomAudioQueue = GetComponent<RandomAudioQueue>();

            validateConfiguration();

            if (showOnStart)
            {
                if (animateOnStartShow)
                {
                    Open();
                }
                else
                {
                    Show();
                }
            }
            else
            {
                Hide();
            }
        }
        
        /// <summary>
        /// Instantly shows the modal window.
        /// </summary>
        public virtual void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if (!rememberPage)
            {
                contentIndex = 0;
            }

            setPage();
        }

        /// <summary>
        /// Instantly hides the modal window.
        /// </summary>
        public virtual void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Used when you want to open the window with an animation.
        /// </summary>
        public virtual void Open()
        {
            Show();
        }

        /// <summary>
        /// Used when you want to close the window with an animation.
        /// </summary>
        public virtual void Close()
        {
            Hide();
        }

        public virtual void NextPage()
        {
            if (contentIndex == content.Content.Length - 1)
                return;
            
            contentIndex++;
            setPage();
            randomAudioQueue?.PlayRandomSound();
        }

        public virtual void PreviousPage()
        {
            if (contentIndex == 0)
                return;

            contentIndex--;
            setPage();
            randomAudioQueue?.PlayRandomSound();
        }

        protected virtual void setPage()
        {
            contentText.SetText(content.Content[contentIndex]);
            numberText?.SetText($"{contentIndex + 1}/{content.Content.Length}");

            previousButton?.gameObject.SetActive(contentIndex == 0 ? false : true);
            nextButton?.gameObject.SetActive(contentIndex == content.Content.Length - 1 ? false : true);            
        }
    }
}