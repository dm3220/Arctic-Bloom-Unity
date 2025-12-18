using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class BookUIController : MonoBehaviour
{
    public enum BookType { Tutorial, Lore}

    [Header("Images")]
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;

    [SerializeField] private Sprite[] tutorialImages;
    [SerializeField] private Sprite[] loreImages;

    [Header("Panels")]
    [SerializeField] private GameObject helpMenuPanel;
    [SerializeField] private GameObject bookPanel;

    [Header("Text")]
    [SerializeField] private TMP_Text titleTMP;
    [SerializeField] private TMP_Text leftPageTMP;
    [SerializeField] private TMP_Text rightPageTMP;

    [Header("Nav buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("Content")]
    [TextArea(2, 8)][SerializeField] private string[] tutorialPages;
    [TextArea(2, 8)][SerializeField] private string[] lorePages;

    private BookType currentBook;
    private int currentSpreadIndex;
    private string[] activePages;

    private void Awake()
    {
        //кнопки навигации
        if (prevButton != null) prevButton.onClick.AddListener(PrevSpread);
        if (nextButton != null) nextButton.onClick.AddListener(NextSpread);
        if (closeButton != null) closeButton.onClick.AddListener(CloseBook);

        //стартовое состояние
        if (bookPanel !=null) bookPanel.SetActive(false);
        if (helpMenuPanel !=null) helpMenuPanel.SetActive(false);
    }

    public void OpenTutorial() => OpenBook(BookType.Tutorial);
    public void OpenLore() => OpenBook(BookType.Lore);

    public void OpenBook(BookType type)
    {
        currentBook = type;
        activePages = (type == BookType.Tutorial) ? tutorialPages : lorePages;

        currentSpreadIndex = 0;

        if (titleTMP != null)
            titleTMP.text = (type == BookType.Tutorial) ? "Обучение" : "Лор";

        if (helpMenuPanel !=null) helpMenuPanel.SetActive(false);
        if (bookPanel !=null) bookPanel.SetActive(true);

        RenderSpread();
    }

    public void CloseBook()
    {
        if (bookPanel !=null) bookPanel.SetActive(false);
        if (helpMenuPanel != null) helpMenuPanel.SetActive(true);
    }

    private void NextSpread()
    {
        if (activePages == null || activePages.Length == 0) return;

        int maxSpreadIndex = GetMaxSpreadIndex(activePages.Length);
        if (currentSpreadIndex < maxSpreadIndex)
        {
            currentSpreadIndex++;
            RenderSpread();
        }
    }

    private void PrevSpread()
    {
        if (activePages == null || activePages.Length == 0) return;

        if (currentSpreadIndex > 0)
        {
            currentSpreadIndex--;
            RenderSpread();
        }
    }

    private void RenderSpread()
    {
        int leftIndex = currentSpreadIndex * 2;
        int rightIndex = leftIndex + 1;

        string leftText = GetPageSafe(activePages, leftIndex);
        string rightText = GetPageSafe(activePages, rightIndex);

        SetPageImage(leftPageImage, leftIndex);
        SetPageImage(rightPageImage, rightIndex);

        if (leftText !=null) leftPageTMP.text = leftText;
        if (rightPageTMP !=null) rightPageTMP.text = rightText;

        int maxSpreadIndex = GetMaxSpreadIndex(activePages?.Length ?? 0);
        if (prevButton !=null) prevButton.interactable = currentSpreadIndex > 0;
        if (nextButton !=null) nextButton.interactable = currentSpreadIndex < maxSpreadIndex;
    }

    private void SetPageImage(Image img, int index)
    {
        if (img == null) return;

        Sprite sprite = null;
        if (currentBook == BookType.Tutorial && index < tutorialImages.Length)
            sprite = tutorialImages[index];
        else if (currentBook == BookType.Tutorial && index < loreImages.Length)
            sprite = loreImages[index];

        img.sprite = sprite;
        img.gameObject.SetActive(sprite != null);
    }

    private static string GetPageSafe(string[] pages, int index)
    {
        if (pages == null || index < 0 || index >= pages.Length) return "";
        return pages[index] ?? "";
    }

    private static int GetMaxSpreadIndex(int pageCount)
    {
        if (pageCount <= 0) return 0;

        return Mathf.Max(0, (pageCount - 1)/2);
    }
}
