using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using NUnit.Framework;

public class Enemy_3 : Enemy
{

    //ever notice how these headers get more complicated despite REALLY NOT NEEDING TO BE
    [Header("Enemy_3 Inscribed Fields")]// like this could be just inscribed.
    public float  lifeTime = 5;
    public Vector2 midpointYRange = new Vector2( 1.5f, 3);
    //also tool tips whyy
    [Tooltip("If true, the baeszier points are drawn in scene pane.")]
    public bool drawDebugInfo = true;
    // and this header can just say dynamic but NOOO
    [Header("Enemy_3 private feilds")]
    [SerializeField]
    private Vector3[] points;
    [SerializeField]
    private float birthTime;

    //uggg
    void Start()
    {
        points = new Vector3[3];
        points[0] = pos;
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        points[1] = Vector3.zero;
        points[1].x = Random.Range( xMin, xMax);
        float midYMult = Random.Range(midpointYRange[0], midpointYRange[1]);
        points[1].y = -bndCheck.camHeight * midYMult;

        points[2] = Vector3.zero;
        points[2].y = pos.y;
        points[2].x = Random.Range( xMin, xMax );
        birthTime = Time.time;

        if ( drawDebugInfo ) DrawDebug();
    }

    public override void Move()
    {
        float u = (Time.time - birthTime ) / lifeTime;
        if(u>1){
            Destroy(this.gameObject);
            return;

        }
        // no idea if this will work but other wise U can't complie due to vlist issue

        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;

        //transform.rotation = Quaternion.Euler( u * 180, 0, 0);

        //pos = Utils.Bezier( u, points);
    }

    void DrawDebug(){

        Debug.DrawLine( points[0], points[1], Color.cyan, lifeTime);
        Debug.DrawLine( points[1], points[2], Color.yellow, lifeTime);

        float numSection = 20;
        Vector2 prevPoint = points[0];
        Color col;
        //Vector3 pt;
        for ( int i = 1; i < numSection; i++){
            float u = i/numSection;
            //pt= Utils.Bezier( u, points); not working do to vlist error
            col = Color.Lerp( Color.cyan, Color.yellow , u);
            //Debug.DrawLine(prevPoint, PreTestAttribute, col, lifeTIme);
            //prevPoint = pt; 
            // need to figure out the vList things...
        }




    }

}
