using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Heater : MonoBehaviour
{
    public float temperaturePerSecond = 1.5f;
    private bool playerInside;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }

    private void Update()
    {
        if (!playerInside) return;
        var gm = GameManager.Instance; if (gm == null) return;
        gm.ChangeTemperature(temperaturePerSecond * Time.deltaTime);
    }
}
