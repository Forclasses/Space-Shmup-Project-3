using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField]
    private eWeaponType _type;

    public eWeaponType type {
        get { return( _type ); }
        set { SetType( value ); }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if( bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp)) {
            Destroy( gameObject );
        }


        if(pos.y > 40){

             Destroy( gameObject );

        }
         if (pos.y < -40){
            Destroy( gameObject );

        }
        if (pos.x < -20){
            Destroy( gameObject );

        }
        if (pos.x > 20){
            Destroy( gameObject );

        }

    }

    public void SetType( eWeaponType eType){
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION( _type );
        rend.material.color = def.projectileColor;
    }

    public Vector3 vel {
        get{ return rigid.linearVelocity;}
        set{ rigid.linearVelocity = value; }
    }
}
