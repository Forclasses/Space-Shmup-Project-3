using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{


    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    private BoundsCheck bndCheck;

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

        if( bndCheck.LocIs( BoundsCheck.eScreenLocs.offDown )){

            print("Is getting singal");
            Destroy( gameObject );
        }
        //this works well enough for now something is off with my bounds check and locis..

        if (pos.y < -50){
            Destroy( gameObject );

        }
    }

    public virtual void Move(){
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
