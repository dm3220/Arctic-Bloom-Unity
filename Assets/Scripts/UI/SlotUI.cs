using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    [Header("Refs (auto)")]
    public Image icon;
    public TMP_Text countText;
    public Image selectHighlight;

    [HideInInspector] public int index;

    public void AutoWire()
    {
        if (icon == null) icon = transform.Find("Icon")?.GetComponent<Image>();
        if (countText == null) countText = transform.Find("Count")?.GetComponent<TMP_Text>();
        if (selectHighlight == null) selectHighlight = transform.Find("Select")?.GetComponent<Image>();
    }
    private void Reset() => AutoWire();
    private void Awake() => AutoWire();

#if UNITY_EDITOR
    private void OnValidate() => AutoWire();
#endif

    // nullable-структура: null означает "в слоте нет стека"
    public void Refresh(ItemStack? stack, bool selected)
    {
        if (selectHighlight) selectHighlight.enabled = selected;

        if (stack == null || stack.Value.IsEmpty)
        {
            if (icon) { icon.enabled = false; icon.sprite = null; }
            if (countText) countText.text = "";
            return;
        }

        var s = stack.Value;

        if (icon)
        {
            icon.enabled = true;
            icon.sprite = s.item != null ? s.item.icon : null;
        }
        if (countText)
            countText.text = (s.count > 1) ? s.count.ToString() : "";
    }
}