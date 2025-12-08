using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interact Settings")]
    public float radius = 1.5f;
    public Transform interactionTransform;

    protected bool isFocus;
    protected Transform player;
    protected bool hasInteracted;

    public virtual void Interact()
    {
        Debug.Log("Interact with " + name);
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = interactionTransform != null ? interactionTransform.position : transform.position;
        Gizmos.DrawWireSphere(pos, radius);
    }
}
