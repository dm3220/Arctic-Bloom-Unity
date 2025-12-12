using UnityEngine;

public class Attack : MonoBehaviour
{
    private PlayerHealth2D player;
    private bool hasHit;

    public void Awake()
    {
        player = GetComponentInParent<PlayerHealth2D>();
    }
    public void EnableHitbox()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        hasHit = false;
    }

    public void DisableHitbox()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasHit) return;
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyMelee2D>();
            if (enemy != null) enemy.TakeDamage(player.Attackdamage);
            hasHit = true;
        }
    }
}
