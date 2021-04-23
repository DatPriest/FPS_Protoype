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

    private Vector3 velocity;
    bool isGrounded;
    bool isSprinting = false;

    Transform target;
    float attackRange = 2f;

    bool playerIsVisible = true;

    float TimerForNextAttack, Cooldown = 3f;

    float dmg = 1f;
    float health = 1f;

    public AudioClip[] deaths;
    public AudioSource audioSource;

    private void Start()
    {
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


        if (target == player.transform)
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

        }
        /*
        Vector3 move = transform.right * x + transform.forward * z;

        if (isSprinting)
            controller.Move(move * speed * sprintFactor * Time.deltaTime);
        else
            controller.Move(move * speed * Time.deltaTime);



        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/



        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    void Attack()
    {
        target
            .GetComponent<Player>()
            .TakeDamage(dmg);
    }

    public void TakeDamage(float amount)
    {
        if (health > amount)
        {
            health -= amount;
        }
        else if (health <= amount)
        {
            health = 0;
            StartCoroutine("Die");
        }
        Debug.Log(health);

    }

    private void OnDestroy()
    {

    }
    IEnumerator Die()
    {
        int randomIndex = Random.Range(0, deaths.Length);
        audioSource.PlayOneShot(deaths[randomIndex]);

        yield return new WaitForSeconds(deaths[randomIndex].length);
        
        Destroy(gameObject);
    }
}