using UnityEngine;

public class PlayerHealth2D : MonoBehaviour
{
    [SerializeField] Attack attackHitbox;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead;
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 0.9f;
    [SerializeField] public int Attackdamage = 9;

    private float lastAttackTime = 0f;
    private bool isAttacking;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDead) TryAttack();

        if (isAttacking) return;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player die!");
        animator.SetTrigger("Dead");
    }

    public void TryAttack()
    {
        if (isAttacking) return;
        if (isDead) return ;

        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        isAttacking = true;
        Debug.Log("ATTACKED! PLAYER");
        animator.SetTrigger("Attack");
    }

    public void AttackEndP()
    {
        Debug.Log("ATTACK END! PLAYER");
        isAttacking = false;
    }
}
