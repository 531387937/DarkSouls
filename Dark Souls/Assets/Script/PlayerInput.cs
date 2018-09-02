using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
 [Header("-----key settings-----")]
 public string keyUp;
 public string keyDown;
 public string keyLeft;
 public string keyRight;

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;


    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;
    [Header("-----Output signals-----")]
    public float Dup;
    public float Dright;
    public Vector3 Dvec;
    public float Dmag;
    public float Jup;
    public float Jright;

    //1.pressing signal
    public bool run;
    //2.trigger once signal
    public bool jump;
    private bool lastjump;
    public bool attack;
    private bool lastattack;
    //3.double trigger
    [Header("-----Others-----")]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;


	
	// Update is called once per frame
	void Update () {
        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        targetDup = (Input.GetKey(keyUp)?1.0f:0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if(!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }
        
        Dup = Mathf.SmoothDamp(Dup, targetDup,ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        Vector2 tempDAxist = SquareToCircle(new Vector2(Dright, Dup));
        Dmag = Mathf.Sqrt((tempDAxist.x * tempDAxist.x) + (tempDAxist.y * tempDAxist.y));
       
        Dvec = transform.forward * Dup + transform.right * Dright;

        run = Input.GetKey(keyA);

        bool tempJump = Input.GetKey(keyB);

        if (tempJump != lastjump && tempJump) 
        {
            jump = true;
           
        }
        else
        {
            jump = false;
        }
        lastjump = tempJump;

        bool tempAttack = Input.GetKey(keyC);

        if (tempAttack != lastattack && tempAttack)
        {
            attack = true;
            print("attack");
        }
        else
        {
            attack = false;
        }
        lastattack = tempAttack;
    }
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
   
}
