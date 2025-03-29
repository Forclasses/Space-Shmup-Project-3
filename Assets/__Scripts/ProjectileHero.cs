using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
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


    // Update is called once per frame
    void Update()
    {
        if( bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp)) {
            Destroy( gameObject );
        }

        if(pos.y > 40){

             Destroy( gameObject );

        }
    }
}
