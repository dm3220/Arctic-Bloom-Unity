using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition _instance;
    public static SceneTransition Instance
    {
        get
        {
            if (_instance == null)
            {
                // Попробуем найти в сцене (включая неактивные)
                var found = FindObjectOfType<SceneTransition>(includeInactive: true);
                if (found != null)
                {
                    _instance = found;
                    _instance.InitializeIfNeeded();
                }
                else
                {
                    // Автосоздание, если забыли положить на сцену
                    var go = new GameObject("SceneTransition (Auto)");
                    _instance = go.AddComponent<SceneTransition>();
                    _instance.CreateDefaultFadeCanvas(); // создадим FadeCanvas + FadeImage
                    _instance.InitializeIfNeeded();
                    Debug.LogWarning("SceneTransition автоматически создан. Рекомендуется добавить его в сцену вручную.");
                }
            }
            return _instance;
        }
    }

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    private bool _initialized;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        InitializeIfNeeded();
    }

    private void InitializeIfNeeded()
    {
        if (_initialized) return;

        DontDestroyOnLoad(gameObject);

        if (fadeCanvasGroup == null)
        {
            // Попробуем найти CanvasGroup в сцене
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>(includeInactive: true);
            if (fadeCanvasGroup == null)
            {
                // Если нет — создадим дефолтный
                CreateDefaultFadeCanvas();
            }
        }

        // И сам холст с CanvasGroup сохраняем между сценами
        if (fadeCanvasGroup != null)
        {
            var root = fadeCanvasGroup.transform.root;
            DontDestroyOnLoad(root.gameObject);
        }

        _initialized = true;
    }

    private IEnumerator Start()
    {
        // Плавно убираем чёрный экран при запуске
        if (fadeCanvasGroup != null)
            yield return Fade(1f, 0f);
    }

    /// <summary>Плавный переход на сцену.</summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
    }

    /// <summary>Удобно назначать в Button.OnClick(string).</summary>
    public void LoadSceneEvent(string sceneName)
    {
        LoadScene(sceneName);
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        if (fadeCanvasGroup != null)
            yield return Fade(0f, 1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        // На новой сцене ещё раз удостоверимся, что у нас есть CanvasGroup
        if (fadeCanvasGroup == null)
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>(includeInactive: true);

        if (fadeCanvasGroup != null)
            yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        fadeCanvasGroup.alpha = from;
        fadeCanvasGroup.blocksRaycasts = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
        fadeCanvasGroup.blocksRaycasts = to > 0.001f;
    }

    /// <summary>Создаёт минимальный FadeCanvas + CanvasGroup + чёрный Image, растянутый на экран.</summary>
    private void CreateDefaultFadeCanvas()
    {
        // Canvas
        var canvasGO = new GameObject("FadeCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<GraphicRaycaster>();
        canvasGO.AddComponent<CanvasScaler>();

        var group = canvasGO.AddComponent<CanvasGroup>();
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;

        // Image на весь экран
        var imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(canvasGO.transform, false);
        var img = imageGO.AddComponent<Image>();
        img.color = Color.black;

        // Растягиваем на весь экран
        var rt = imageGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;   // (0,0)
        rt.anchorMax = Vector2.one;    // (1,1)
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        fadeCanvasGroup = group;
    }
}
