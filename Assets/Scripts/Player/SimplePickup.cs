using UnityEngine;

public class SimplePickup : MonoBehaviour, IInteractable
{
    [Tooltip("Сколько энергии добавить при подборе (в процентах).")]
    public float energyGain = 15f;

    // Вызывается PlayerInteractor'ом
    public void Interact()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeEnergy(energyGain);
            Debug.Log($"+Энергия: {energyGain}");
        }
        Destroy(gameObject);
    }

    // Если хочешь автоподбор триггером — раскомментируй:
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Interact();
    }
    */
}
