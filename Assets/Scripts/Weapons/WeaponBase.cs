using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour
{
    public enum WeaponType : byte { NONE = 0, PISTOL = 1, SMG = 2, SHOTGUN = 3, SNIPER = 4, AR = 5}
    public enum BulletType : byte { NONE = 0, PISTOL = 1, SMG = 2, SHOTGUN = 3, SNIPER = 4, AR = 5 }

    public GameObject ammoPrefab;
    public int dmg;
    AudioSource audioSource;
    ParticleSystem particleSystem;

    protected int clip { get; set; }
    public int clipSize;

    protected float reloadTime;
    bool isReloading;
    bool isPlayer;

    public Vector3 upRecoil;
    Vector3 originalRotation;
    Text Ammunition;


    // Debug Values
    Camera cam;

    protected virtual void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //particleSystem = gameObject.AddComponent<ParticleSystem>(); // DEBUG
        //particleSystem.Pause();
        Ammunition = GameObject.Find("Ammunition").GetComponent<Text>();
        if (audioSource != null)
            reloadTime = audioSource.clip.length;
    }

    public virtual void Start()
    {
        originalRotation = transform.localEulerAngles;

        isPlayer = (gameObject.tag == "Player" ? true : false);
        if (isPlayer)
        {
            cam = transform.parent.gameObject.GetComponentInChildren<Camera>();
        }
    }

    public virtual void PrimaryFire()
    {

        if (isReloading) return;
        if (clip <= 0)
        {
            Reload();
            return;
        }

        cam = transform.parent.gameObject.GetComponentInChildren<Camera>();

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Shoot();
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Enemy")))
        {
            Transform objectHit = hit.transform;
            if (!isPlayer && objectHit.CompareTag("Enemy"))
            {
                float enemyHealth = objectHit.GetComponent<EnemyAI>().TakeDamage(dmg);
                if ( enemyHealth <= 0)
                {
                    GameObject.FindGameObjectWithTag("HUD")
                        .GetComponent<HUD>()
                        .AddScore(1);

                    if (enemyHealth < 0)
                        Debug.Log(enemyHealth);                                                
                }
            }
            else Debug.Log($"Objecthit : {objectHit.tag}");
        }
    }

    public virtual void SecondaryFire()
    {

    }

    protected virtual void Shoot()
    {
        StartCoroutine(Recoil());
        audioSource.Play();
        clip -= 1;
        Ammunition.text = $"{clip} / {clipSize}";
    }

    protected virtual IEnumerator ReloadCoroutine()
    {
        if (audioSource.isPlaying)
        {
            isReloading = true;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReloadCoroutine());
        }
        else
            isReloading = false;
        clip = clipSize;
        Ammunition.text = $"{clip} / {clipSize}";
    }

    public virtual void Reload()
    {

        if (clip == clipSize)
            FullSizeReload();
        else if (clip < clipSize)
            StartCoroutine(ReloadCoroutine());        
    }

    protected virtual void FullSizeReload()
    {
        
    }

    protected virtual IEnumerator Recoil()
    {
        transform.localEulerAngles += upRecoil;
        //Camera.main.transform.rotation;
        yield return new WaitForSeconds(0.5f);
        StopRecoil();

    }

    protected virtual void StopRecoil()
    {
        transform.localEulerAngles = originalRotation;
    }
}