using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI diamondText;
        [SerializeField] private Canvas startCanvas, gameCanvas, winCanvas, loseCanvas;
        public static UIManager Instance { get; set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            
            LoadInitialValues();
        }
        
        private void OnEnable()
        {
            EventManager.Instance.onPressPlayButton += OnPressPlayButton;
            EventManager.Instance.onWinLevel += OnWinLevel;
            EventManager.Instance.onLoseLevel += OnLoseLevel;
            EventManager.Instance.onCollectAnItem += OnCollectAnItem;
        }

        private void OnDisable()
        {
            EventManager.Instance.onPressPlayButton -= OnPressPlayButton;
            EventManager.Instance.onWinLevel -= OnWinLevel;
            EventManager.Instance.onLoseLevel -= OnLoseLevel;
            EventManager.Instance.onCollectAnItem -= OnCollectAnItem;
        }

        private void LoadInitialValues()
        {
            levelText.text = "LEVEL " + 1;
            diamondText.text = 1.ToString();
        }
        
        private void OnPressPlayButton()
        {
            HideCanvas(startCanvas);
        }

        private void OnWinLevel()
        {
            HideCanvas(gameCanvas);
            ShowCanvas(winCanvas);
        }
        
        private void OnLoseLevel()
        {
            HideCanvas(gameCanvas);
            ShowCanvas(loseCanvas);
        }
        
        private void OnCollectAnItem(CollectableItemType collectableItemType)
        {
            if (collectableItemType == CollectableItemType.Diamond)
            {
                diamondText.text = (int.Parse(diamondText.text) + 1).ToString(); // Increase diamond count
            }
        }
        
        public void OnPressNextButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnPressRetryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ShowCanvas(Canvas canvas)
        {
            if (canvas == null) 
                return;
            StartCoroutine(ProcessCanvas(canvas, true));
        }

        private void HideCanvas(Canvas canvas)
        {
            if (canvas == null) 
                return;
            
            StartCoroutine(ProcessCanvas(canvas, false));
        }
        
        private IEnumerator ProcessCanvas(Canvas canvas, bool enable)
        {
            yield return new WaitForSeconds(0.2f);
            canvas.enabled = enable;
        }

    }
}