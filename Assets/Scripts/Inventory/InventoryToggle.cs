using Unity.VisualScripting;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasP;
    private bool isOpen;

    public void Start()
    {
        SetOpen(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetOpen(!isOpen);
        }
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        canvasP.alpha = open ? 1f : 0f;
        canvasP.interactable = open;
        canvasP.blocksRaycasts = open;
    }
}
