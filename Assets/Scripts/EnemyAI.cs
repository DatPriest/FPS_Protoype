using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class EnemyAI : MonoBehaviour
{
    public CharacterController controller;
    GameObject player;

    public float speed = 0.1f;
    public float gravity = -15.81f;
    public float jumpHeight = 3f;
    public float sprintFactor = 2f; // Fallback Value

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public ParticleSystem deathVFX;

    private Vector3 velocity;
    bool isGrounded;
    bool isSprinting = false;

    Transform target;
    float attackRange = 2f;

    bool playerIsVisible = true;

    float TimerForNextAttack, Cooldown = 3f;

    float dmg = 1f;
    public float health = 1f;

    public AudioClip[] deaths;
    public AudioSource audioSource;

    public GameObject particleList;


    private void Start()
    {
        particleList = GameObject.Find("ParticleList");
        deathVFX.Stop();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        TimerForNextAttack = Cooldown;
        deaths = Resources.LoadAll<AudioClip>("Sounds/Deaths/Zombies");
    }

    private void FixedUpdate()
    {
        if (health > 0)
            CalculateMovement();
        else
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    void CalculateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;


        if (target != null && target == player.transform)
        {
            transform.LookAt(target.position);
            Vector3 move = transform.forward;

            if (Vector3.Distance(transform.position, target.position) > attackRange)
            {
                speed = 3f;
                controller.Move(move * speed * Time.deltaTime);
            }
            else
            {
                if (TimerForNextAttack > 0)
                {
                    TimerForNextAttack -= Time.deltaTime;
                }
                else if (TimerForNextAttack <= 0)
                {
                    Attack();
                    TimerForNextAttack = Cooldown;
                }
            }
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            target = player.transform;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    void Attack()
    {
        target
            .GetComponent<Player>()
            .TakeDamage(dmg);
    }

    public float TakeDamage(float amount)
    {
        if (health > amount)
        {
            health -= amount;
        }
        else if (health <= amount)
        {
            health = 0;
            Die();
        }
        return health;

    }
    private void OnDestroy()
    {
        // How can I delete this after a few Seconds!
        var t = Instantiate(new GameObject(), transform);
        var a = t.AddComponent<AudioSource>();
        a = audioSource;
        int randomIndex = Random.Range(0, deaths.Length);
        audioSource.PlayOneShot(deaths[randomIndex]);
        yield return new WaitForSeconds(deaths[randomIndex].length);

    }

    void Die()
    {        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(2, 4, 2));
    }
}