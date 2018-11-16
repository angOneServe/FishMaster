using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoveX_V_A : MonoBehaviour {
    public float X = 0;
    public float Y = 0;

    public float Vx = 0.25f;  //长度6，12s,速度=0.5f
    public float Vy =0.5f; //高度3，落回地面时间=3/（0.5/2）=12s

    public float ay = -0.041F;
    public float ax = 0;

    public float Fx =1;
    public float Fy =2;

    public float Ft = 2;

    public float rate=0.1f;
    float timer = 0;
    float maxtime = 0;

    public  Transform objctZBX;
    public Transform objct2ZBX;
    // Use this for initialization
    void Start () {
        Vector3 worldPos= objctZBX.localToWorldMatrix.MultiplyPoint(transform.localPosition);
        Debug.Log(worldPos);

       
    }
	
	// Update is called once per frame
	void Update () {
        //X += Vx * Time.deltaTime * rate;//单位时间的累加 update的单位时间是Time.deltaTime，远动快点可以成个比例
        //Y += Vy * Time.deltaTime * rate;

        //Vy += ay * Time.deltaTime * rate;
        //Vx += ax * Time.deltaTime * rate;
        //transform.position = new Vector2(X, Y);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vx = 0.25f;  //长度6，12s,速度=0.5f
        //    Vy = 0.5f; //高度3，落回地面时间=3/（0.5/2）=12s
        //    X = 0;
        //    Y = 0;
        //    ay = -0.041F;
        //    transform.position = new Vector2(0, 0);
        //}


        if (Input.GetMouseButton(0))
        {
   
        }
    }

}
