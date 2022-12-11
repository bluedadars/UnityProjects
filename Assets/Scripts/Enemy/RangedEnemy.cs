using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack  Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [Header("Collider  Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    [Header("PlayerLayer  Parameters")]
    public LayerMask PlayerMask;
    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;

    [Header("Play Sound")]
    [SerializeField] private AudioClip fireballSound;


    private EnemyPetrol enemyPetrol;


    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPetrol = GetComponentInParent<EnemyPetrol>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        //Attack only when player insight?

        if (PlayerInSight())
        {


            if (cooldownTimer > attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("RangeAttack");

            }
        }
        if (enemyPetrol != null)
            enemyPetrol.enabled = !PlayerInSight();

    }
    public void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        cooldownTimer = 0;
        fireBalls[FindFireball()].transform.position = firePoint.position;
        fireBalls[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    public int FindFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    public bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0,
        Vector2.left, 0, PlayerMask);
        return hit.collider != null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

    }

}
