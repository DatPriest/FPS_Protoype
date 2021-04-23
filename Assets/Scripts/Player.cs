using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100;

    public float lastHealth;

    public float dmg = 1f;
    public bool canAttack;

    public GameObject weapon;
    Shotgun activeWeapon;

    protected PlayerMovement pM;

    private void Start()
    {        
        pM = gameObject.GetComponent<PlayerMovement>();
        lastHealth = health;
        activeWeapon = weapon.GetComponent<Shotgun>();
    }

    private void Update()
    {
        Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        if (lastHealth != health)
        {
            Debug.Log($"I've lost some Health {lastHealth - health}, Health:{health}");
            lastHealth = health;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            activeWeapon.PrimaryFire();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            activeWeapon.SecondaryFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            activeWeapon.Reload();
        }
    }

    public void TakeDamage(float amount)
    {
        if (health > amount)
        {
            health -= amount;
        } else if (health <= amount)
        {
            playerDied();
        }
    }

    void playerDied()
    {
        health = 0f;
        Time.timeScale = 0f;
    }
}


