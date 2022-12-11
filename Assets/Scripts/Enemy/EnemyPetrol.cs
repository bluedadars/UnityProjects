using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPetrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameter")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;
    [Header("Idle Timer")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    void Awake()
    {
        initScale = enemy.localScale;
        // anim = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        anim.SetBool("Moving", false);
    }
    void Update()
    {

        if (movingLeft)
        {

            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
            {
                //change direction
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                //change direction
                DirectionChange();
            }
        }

    }

    private void DirectionChange()
    {
        anim.SetBool("Moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;

    }
    public void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);


        //Move enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        //Move in that direction

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);

    }




}

