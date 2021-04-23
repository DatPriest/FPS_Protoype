using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Camera cam;
    Player player;
    public float TimerForNextAttack, Cooldown = 1f;
    public Transform localTarget;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        player = GetComponent<Player>();
        TimerForNextAttack = Cooldown;
        Cooldown = audioSource.clip.length - 0.7f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void Attack(Transform target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<EnemyAI>().TakeDamage(player.dmg);
            Debug.Log("Player attacked");
        }
        else
        {
            Debug.Log("Player missed target");
        }
        audioSource.Play();
        player.canAttack = false;
        TimerForNextAttack = Cooldown;
    }


}
