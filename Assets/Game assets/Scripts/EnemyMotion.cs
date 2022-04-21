using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour
{
    public float speed = 2f; // enemy speed

    public Transform player; // player transform (destination)

    public bool isTargetPlayer = false; // follow player or not (Player tag)

    public int damage = 1; // Damage done to the player

    public float distanceToAttack = 2f; // Attack range

    public float attackRate = 1f; // Attack rate

    public Animator animator; // Animator

    public bool isMoving = true;
    public bool isAttacking = true;

    private Rigidbody _rigidbody;
    private float _timer;
    private EnemyStats stats;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        if (isTargetPlayer == true)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stats = gameObject.GetComponent<EnemyStats>();
    }

    void FixedUpdate()
    {
        Vector3 positionZombie = gameObject.transform.position;
        Vector3 direction = player.position - positionZombie;

        if (isMoving)
        {
            _rigidbody.velocity = direction.normalized * speed + Vector3.up * _rigidbody.velocity.y;
        }

        animator.SetFloat(Animator.StringToHash("Horizontal"), 0.5f, .1f, Time.deltaTime);

        if (isAttacking && Vector3.Distance(positionZombie, player.position) <= distanceToAttack)
        {
            _timer = _timer + Time.deltaTime;

            if (_timer >= attackRate)
            {
                animator.applyRootMotion = true;
                animator.CrossFade("OH_Light_Attack_01", .2f);
                Debug.Log(gameObject.name + " inflige des dégats au joueur !");
                PlayerStats vieDuJoueur = player.GetComponent<PlayerStats>();
                vieDuJoueur.TakeDamage(damage);
                _timer = 0f;
            }
        }
        //Si je suis pas à portée de mon joueur 
        else
        {
            //Je réinstialise mon timer
            _timer = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (stats.currentHealth == 0) {
            isMoving = false;
            isAttacking = false;
        }

        if (isMoving)
        {
            gameObject.transform.LookAt(player, Vector3.up);
        }

    }
}
