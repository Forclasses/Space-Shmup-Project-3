using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
[RequireComponent( typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{


    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    protected bool calledShipDestoryed = false;

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    

    void Update()
    {
        Move();
        //this is not wokring
        //At this point I can't tell if bndCheck is a thing or not.. am to afriad to touch it to check

       if( bndCheck.LocIs( BoundsCheck.eScreenLocs.offDown )){

            //print("Is getting singal");
            Destroy( gameObject );
        }
        //this works well enough for now something is off with my bounds check and locis..

        if (pos.y < -50){
            Destroy( gameObject );

        }
        if (pos.y > 50){
            Destroy( gameObject );

        }
        if (pos.x < -20){
            Destroy( gameObject );

        }
        if (pos.x > 20){
            Destroy( gameObject );

        }


    }

    public virtual void Move(){
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    //am on pg 851

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if ( p != null ){
            if( bndCheck.isOnScreen ){
                health -= Main.GET_WEAPON_DEFINITION( p.type ).damageOnHit;
                if( health <= 0){
                    if(!calledShipDestoryed){
                        calledShipDestoryed = true;
                        Main.SHIP_DESTORYED( this );
                    }
                    Destroy(this.gameObject);
                }
            }

            //back up code incase of annance with bnd check

            //if( (pos.x < 16) && (pos.x > -16) && (pos.y < 34)  ){
             //   health -= Main.GET_WEAPON_DEFINITION( p.type ).damageOnHit;
             //   if( health <= 0){
             //       Destroy(this.gameObject);
             //   }
            //}
            Destroy( otherGO );

        } else {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name );
        }
    }
}
