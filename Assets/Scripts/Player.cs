using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health = 100;
    public float Health {
        get
        {
            return health;
        }
        set 
        {
            health = value;
            hud.healthText.text = health.ToString();
        }
    }

    public HUD hud;

    public float lastHealth;

    public float dmg = 1f;
    public bool canAttack;

    public GameObject weapon;
    Shotgun activeWeapon;

    protected PlayerMovement pM;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();
        hud.healthText.text = Health.ToString();
        pM = gameObject.GetComponent<PlayerMovement>();
        lastHealth = Health;
        activeWeapon = weapon.GetComponent<Shotgun>();
    }

    private void Update()
    {
        Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        if (lastHealth != Health)
        {
            Debug.Log($"I've lost some Health {lastHealth - Health}, Health:{Health}");
            lastHealth = Health;
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
        if (Health > amount)
        {
            Health -= amount;
        } else if (Health <= amount)
        {
            playerDied();
        }
    }

    void playerDied()
    {
        Health = 0f;
        Time.timeScale = 0f;
    }

}


