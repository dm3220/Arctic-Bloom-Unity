using UnityEngine;
using TMPro;

public class SimpleDialogueUI : MonoBehaviour
{
    public static SimpleDialogueUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text textTMP;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (panel != null)
            panel.SetActive(false);
        else
            Debug.LogWarning("SimpleDialogueUI: panel not assigned");
    }

    public void Show(string text)
    {
        if (!this || panel == null || textTMP == null) return;
        if (!panel) return;

        panel.SetActive(true);
        textTMP.text = text;
    }

    public void Hide()
    {
        if (!this) return;
        if (!panel) return;

        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

}
