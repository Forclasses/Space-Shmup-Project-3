using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

// at this point I don't know why I keep including these the book has them.. so I add them just to be sure

using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero  S { get; private set; }

    [Header("Inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Dynamic")] [Range(0,4)] [SerializeField]
    private float _shieldLevel = 1;
    //public float shieldLevel = 1;

    [Tooltip( "This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null; //new GameObject();

    void Awake()
    {
        if(S == null) {
        S = this;
    } else {
        Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        // is it bad I think a  second hero ship could be fun
    }
        
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");


        //HEY JUST AS AN ASIDE HERE PLEASE NOTIFY THE CLASS THAT WE NEED TO CHANG OUT INPUT SETTINGS WHEN USING tHE BOOK
        // reddit post You can't use Input.GetAxis or Input.GetKey etc. if you are using the new Unity input system. 
        // Check your settings in Edit > Project Settings > Player > Other Settings > Active Input Handling
        // my add: it needs to be set to hold. I am so glad someone else had this error a few days ago
        // reddit linkly https://www.reddit.com/r/Unity3D/comments/1ixyuwk/how_do_i_fix_the_invalidoperationexception_you/

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult,hAxis*rollMult,0);

        if(Input.GetKeyDown( KeyCode.Space ) ) {

            TempFire();


        }
    }

    void TempFire() {
        GameObject projGO = Instantiate<GameObject>( projectilePrefab );
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.linearVelocity =  Vector3.up * projectileSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //Debug.Log("Shield trigger hit by " + go.gameObject.name);

        if( go == lastTriggerGo ) return;
        lastTriggerGo = go;
        Enemy enemy = go.GetComponent<Enemy>();
        if(enemy != null){
            shieldLevel--;
            Destroy(go);
        }else {
            Debug.LogWarning("Shield trigger hit by non-Enemy" + go.name);
        }
    }

    public float shieldLevel {
        get { return ( _shieldLevel ); }
        private set{
            _shieldLevel = Mathf.Min( value, 4 );
            if(value < 0){
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }

}
