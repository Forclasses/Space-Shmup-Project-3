using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class Enemy_2 : Enemy
{
    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;
    [Tooltip("Determines how much the sine wave will ease the interpolation")]
    public float sinEccentricity = 0.6f;

    public AnimationCurve rotCurve;
    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;

    [SerializeField] private Quaternion baseRotation;
    [SerializeField] private Vector3 p0, p1;


    void Start()
    {
       p0 = Vector3.zero;
       p0.x = -bndCheck.camWidth - bndCheck.radius;
       p0.y = Random.Range( -bndCheck.camHeight, bndCheck.camHeight);

       p1 = Vector2.zero;
       p1.x = bndCheck.camWidth + bndCheck.radius;
       p1.y = Random.Range( -bndCheck.camHeight, bndCheck.camHeight);

       if (Random.value > 0.5f){
        p0.x *= -1;
        p0.y *= -1;
       }

      

       birthTime = Time.time;

       transform.position = p0;
       transform.LookAt(p1, Vector3.back);
       baseRotation = transform.rotation;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u>1){
            Destroy(this.gameObject );
            return;
        }
        float shipRot = rotCurve.Evaluate( u ) * 360;
        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);

        u = u + sinEccentricity*(Mathf.Sin(u*Mathf.PI*2));

        pos = (1-u)*p0 + u*p1;

    }

    
}
