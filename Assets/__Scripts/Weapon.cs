using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public enum eWeaponType {
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}

[System.Serializable]
public class WeaponDefinition {
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on PowerUp Cube")]
    public string letter;
    [Tooltip("Color of PowerUp Cube")]
    public Color powerUpColor = Color.white;
    [Tooltip("Prefab of weapon mod on player ship")]
    public GameObject weaponModelPrefab;
    [Tooltip("Prefab of projectile going out")]
    public GameObject projectilePrefab;
    [Tooltip("What color is projectile")]
    public Color projectileColor = Color.white;
    [Tooltip("How much dose it hurt enemy")]
    public float damageOnHit = 0;
    [Tooltip("Damage for lazer per second (note: not implemented)")]
    public float damagePerSec = 0;
    [Tooltip("Seconds to delay between shots")]
    public float delayBetweenShots = 0;
    [Tooltip("How fast indivial projectiles go")]
    public float velocity = 50;

    //this is so much stuff ug

}

public class Weapon : MonoBehaviour
{
    // in advance holy moly this is a larg section of code..

    static public Transform PROJECTILE_ANCHOR;
    [Header("Dynamic")]
    [SerializeField]
    [Tooltip("Set this manually while playing no work properly")]
    private eWeaponType _type = eWeaponType.none;
    public WeaponDefinition def;
    public float nextShotTime;

    private GameObject weaponModel;
    private Transform shotPointTrans;

    void Start()
    {
        if(PROJECTILE_ANCHOR == null){
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        shotPointTrans = transform.GetChild( 0 );

        SetType( _type );
        Hero hero = GetComponentInParent<Hero>();
        if(hero != null ) hero.fireEvent += Fire;

    }

    public eWeaponType type {
        get { return( _type ); }
        set { SetType( value ); }
    }

    public void SetType( eWeaponType wt ){
        _type = wt;
        if (type == eWeaponType.none){
            this.gameObject.SetActive(false);
            return;

        } else 
        {
            this.gameObject.SetActive(true);
        }

        def = Main.GET_WEAPON_DEFINITION(_type);
        if( weaponModel != null) Destroy( weaponModel );
        weaponModel = Instantiate<GameObject>(def.weaponModelPrefab, transform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localScale = Vector3.one;

        nextShotTime = 0;
    }

    //ugg whose ever idea was to make switchable wepons I wann pat on the back because I will likely be using some varient of this on the
    //final porject
    //dose not mean I have to LIKE IT THO

    private void Fire() {//in the HOLE
        if( !gameObject.activeInHierarchy ) return;
        if( Time.time < nextShotTime ) return;

        ProjectileHero p;
        Vector3 vel = Vector3.up * def.velocity;

        switch(type){
            case eWeaponType.blaster:
                p = MakeProjectile();
                p.vel = vel;
                break;
            case eWeaponType.spread:
                p = MakeProjectile();
                p.vel = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis( 10, Vector3.back );
                p.vel = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis( -10, Vector3.back );
                p.vel = p.transform.rotation * vel;
                break;
        }

    }

    private ProjectileHero MakeProjectile(){
        GameObject go;
        go = Instantiate<GameObject>(def.projectilePrefab, PROJECTILE_ANCHOR);
        ProjectileHero p = go.GetComponent<ProjectileHero>();
        Vector3 pos = shotPointTrans.position;
        pos.z = 0;
        p.transform.position = pos;

        p.type = type;
        nextShotTime = Time.time + def.delayBetweenShots;
        return( p );
    }

}
