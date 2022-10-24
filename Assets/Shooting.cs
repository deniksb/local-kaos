using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform shootPoint;

    public GameObject bulletPrefab;

    public string enemyTag;

    public float bulletForce = 20f;

    public float DelayBetweenShots = 0.5f;
 
    float lastShootTime;

    ShootType shootType;

    int shootComboStacks = 0;

    int shootComboStackPools = 0;

    public float DelayRemoveComboAttack = 5f;

    float lastActivateComboAttack;

    //Beam shoot variables

    public float range = 100f;

    public float damage = 1;

    private bool beamAttackOn = false;

     void Start()
     {
        lastShootTime = Time.time;

        shootType = ShootType.Projectile;

        lastActivateComboAttack = Time.time - DelayRemoveComboAttack;
     }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time - lastShootTime > DelayBetweenShots)
        {
            if(shootType == ShootType.Projectile)
            {
                ShootProjectile();
            }
            else if(shootType == ShootType.Beam)
            {
                beamAttackOn = !beamAttackOn;
            }
            
            lastShootTime = Time.time;
        }

        if(beamAttackOn)
        {
            ShootBeam();
        }

        activateComboAttack();
    }

    void ShootProjectile()
    {   
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(shootPoint.up * bulletForce, ForceMode2D.Impulse);

        handleShootComboStacks();

        Debug.Log("Stack Pools: " + shootComboStackPools);
    }

    void ShootBeam()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, shootPoint.up, range);

        Debug.DrawRay(shootPoint.position, shootPoint.up, Color.red, 1f);

        if(hit != null && hit.collider.tag == enemyTag)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }


    }

    void handleShootComboStacks()
    {
        if(shootComboStackPools < 3)
        {
            if(shootComboStacks < 15)
            {
                shootComboStacks++;
            }
            else
            {
                shootComboStacks = 0;

                shootComboStackPools++;
            }
        }
    }

    void activateComboAttack()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Activate combo attack");

            if(shootComboStackPools < 3 && shootComboStackPools > 0)
            {
                shootComboStackPools--;

                lastActivateComboAttack = Time.time;

                shootType = ShootType.Beam;

                Debug.Log("Activated small combo attack. Stack pools: " + shootComboStackPools);
            }
            else if(shootComboStackPools == 3)
            {
                shootComboStackPools = 0;

                lastActivateComboAttack = Time.time;

                Debug.Log("Activated ult combo attack. Stack pools: " + shootComboStackPools);
            }
        }

        comboAttackEnd();
    }

    void comboAttackEnd()
    {   
        //Returning back to normal attack after combo has timed out
        if(Time.time - lastActivateComboAttack > DelayRemoveComboAttack)
        {
            shootType = ShootType.Projectile;

            beamAttackOn = false;
        }   
    }

   enum ShootType
   {
    Projectile,

    Beam
   }
}
