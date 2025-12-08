using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("¬заимодействие")]
    public float interactionRadius = 1.5f;
    public LayerMask interactableLayer = ~0;   // по умолчанию все слои
    public KeyCode interactKey = KeyCode.E;

    [Header("UI-подсказка")]
    public GameObject interactionHint; // перетащи сюда TMP-объект с текстом "E Ч взаимодействовать"

    Collider2D current;

    void Update()
    {
        FindClosest();

        if (current != null && Input.GetKeyDown(interactKey))
        {
            // Ћюбой компонент с IInteractable
            var interactable = current.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                if (interactionHint) interactionHint.SetActive(false);
                current = null;
            }
        }
    }

    void FindClosest()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);
        Collider2D best = null;
        float bestDist = float.PositiveInfinity;

        foreach (var h in hits)
        {
            if (h == null || h.gameObject == gameObject) continue;
            if (h.GetComponent<IInteractable>() == null) continue; // берем только интерактивы

            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = h;
            }
        }

        current = best;
        if (interactionHint) interactionHint.SetActive(current != null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
