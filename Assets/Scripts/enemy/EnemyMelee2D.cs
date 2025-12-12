using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMelee2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 1.2f;
    [SerializeField] private float stoppingBuffer = 0.1f;
    [SerializeField] private float chaseDistance = 8f;

    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private float attackRange = 0.7f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    [Header("HP")]
    [SerializeField] private int maxHealth = 20;

    private int currentHealth;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime;
    private bool isDead = false;
    private Vector3 baseScale;
    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        baseScale = transform.localScale;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.Log("EnemyMelee2D: Player with tag 'Player' not found!");
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > chaseDistance)
        {
            StopMovement(); // Speed = 0 → idle
            return;
        }
        if (distance > stoppingDistance)
        {
            MovePlayer();
        }
        else
        {
            StopMovement();
            TryAttack();
        }
    }

    private void MovePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        //движение
        Vector2 targetPos = (Vector2)transform.position + direction * (moveSpeed * Time.deltaTime);
        rb.MovePosition(targetPos);

        if (direction.x > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        else if (direction.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        animator.SetFloat("Speed", moveSpeed);
    }
    
    private void StopMovement()
    {
        animator.SetFloat("Speed", 0f);
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        isAttacking = true;
        Debug.Log("ATTACK START");
        animator.SetTrigger("Attack");
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("ATTACK END");
        isAttacking = false;
    }

    public void DealDamage()
    {
        if (isDead) return;

        Debug.Log("EnemyMelee2D: DealDamage() called");   // 1 – вообще вызывается ли метод

        if (attackPoint == null)
        {
            Debug.LogWarning("EnemyMelee2D: attackPoint is not assigned!");
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hit != null)
        {
            Debug.Log("EnemyMelee2D: hit " + hit.name);   // 2 – кого именно задели

            PlayerHealth2D ph = hit.GetComponent<PlayerHealth2D>();
            if (ph != null)
            {
                Debug.Log("EnemyMelee2D: calling TakeDamage on " + ph.name); // 3
                ph.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("EnemyMelee2D: collider has no PlayerHealth2D!");
            }
        }
        else
        {
            Debug.Log("EnemyMelee2D: no target in attack range"); // 4 – никого не задел
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        rb.simulated = false;

        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
