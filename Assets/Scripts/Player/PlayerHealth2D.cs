using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth2D : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 0.9f;
    [SerializeField] private int attackDamage = 9;
    [SerializeField] private Vector2 attackOffset = new Vector2(0.6f, 0f);
    [SerializeField] private LayerMask enemyLayer;

    private int currentHealth;
    private bool isDead;
    private float lastAttackTime;
    private bool isAttacking;

    private void Awake()
    {
        currentHealth = maxHealth;

        // На случай, если забыли назначить в инспекторе
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Q))
            TryAttack();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Player HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player die!");
        animator.SetTrigger("Dead");
    }

    private void TryAttack()
    {
        if (isAttacking) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        isAttacking = true;
        Debug.Log("ATTACKED! PLAYER");
        animator.SetTrigger("Attack");
    }

    // Вызывай ЭТОТ метод Animation Event'ом в момент удара
    public void DealDamage()
    {
        Debug.Log("DealDamage CALLED");
        // Направление по твоему развороту localScale.x (вправо +, влево -)
        float dir = transform.localScale.x >= 0 ? 1f : -1f; // :contentReference[oaicite:0]{index=0}

        Vector2 attackPoint = (Vector2)transform.position +
                              new Vector2(attackOffset.x * dir, attackOffset.y);

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayer);

        // защита от двойного урона, если у врага несколько коллайдеров
        var damaged = new HashSet<EnemyMelee2D>();
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyMelee2D>();
            if (enemy != null && damaged.Add(enemy))
                enemy.TakeDamage(attackDamage);
        }
    }

    // Вызывай Animation Event'ом в конце атаки
    public void AttackEndP()
    {
        Debug.Log("ATTACK END! PLAYER");
        isAttacking = false;
    }

    // (Опционально) чтобы видеть радиус удара в сцене
    private void OnDrawGizmosSelected()
    {
        float dir = transform.localScale.x >= 0 ? 1f : -1f; // :contentReference[oaicite:1]{index=1}
        Vector2 p = (Vector2)transform.position + new Vector2(attackOffset.x * dir, attackOffset.y);
        Gizmos.DrawWireSphere(p, attackRange);
    }
}