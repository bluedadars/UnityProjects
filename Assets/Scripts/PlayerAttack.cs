using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireBallSound;
    [SerializeField] private Animator anim;
    private float cooldownTime;
    private Vector2 direction;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Awake()
    {
        anim.GetComponent<Animator>();
        playerMovement.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTime > attackCooldown && playerMovement.canAttack())
        {
            Attack();
            cooldownTime += Time.deltaTime;
        }


    }
    public void Attack()
    {
        SoundManager.instance.PlaySound(fireBallSound);
        anim.SetTrigger("isAttack");
        cooldownTime = 0;
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

    }

    public int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;



        }
        return 0;
    }
    //creat a 
}
