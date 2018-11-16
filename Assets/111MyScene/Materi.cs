using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Materi : MonoBehaviour {
    public Vector4 r1;
    public Vector4 r2;
    public Vector4 r3;
    public Vector4 r4;
    Vector3 v1 = new Vector3(1, 1, 1);
    public  Matrix4x4 m1 = Matrix4x4.identity;
    Matrix4x4 m2 = Matrix4x4.identity;
	// Use this for initialization
	void Start () {
  
     

    }
	
	// Update is called once per frame
	void Update () {

        m1.SetRow(0, r1);
        m1.SetRow(1, r2);
        m1.SetRow(2, r3);
        m1.SetRow(3, r4);
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = v1;
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            transform.position = m1.MultiplyPoint(v1);
        }
        
    }
}
