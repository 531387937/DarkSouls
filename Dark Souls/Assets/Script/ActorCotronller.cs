using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCotronller : MonoBehaviour {
    public GameObject model;
    public PlayerInput pi;
    public float WalkSpeed;
    public float RunMultiplier=2.0f;
    public float rollvelocity = 1.0f;
    public float jabVelocity;
    private Rigidbody rigid;

    [Header("=======Friction Setting=======")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;


    private Animator anim;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack=true;
    private Vector3 deltaPos;
    private bool lockPlanar = false;
    private CapsuleCollider col;
    private float lerpTarget;
	// Use this for initialization
	void Awake () {
        pi = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        anim = model.GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
        anim.SetFloat("forward", pi.Dmag *Mathf.Lerp(anim.GetFloat("forward"),targetRunMulti,0.2f));
        if(pi.Dmag>0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        if (!lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * WalkSpeed * ((pi.run) ? RunMultiplier : 1.0f);
        }
        if(pi.jump)
        {
            
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if(pi.attack&& CheckState("ground")&&canAttack)
        {
           
            { anim.SetTrigger("attack"); }
        }
        if(rigid.velocity.magnitude>0f&&pi.jump)
        {
            anim.SetTrigger("roll");
        }
        
    }
    void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    private bool CheckState(string stateName,string layerName="Base Layer")
    {       
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }
    /// <summary>
    /// Message processing block
    /// </summary>
    public void OnJumpEnter()
    {
       
        thrustVec = new Vector3(0, 3f, 0);
    }

    public void IsGround()
    {
        anim.SetBool("IsGround", true);
    }
    public void IsNotGround()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        anim.SetBool("IsGround", false);
    }
    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }
    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollvelocity, 0);
    }
    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        
    }
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }
    public void OnAttack1hAEnter()
    {
        lerpTarget = 1.0f;
        
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")),1.0f,0.8f));
        pi.inputEnabled = false;
        //planarVec = new Vector3(0, 0, 0);
    }
    public void OnAttack1hAUpdate()
    {
        //thrustVec = model.transform.forward * anim.GetFloat("Attack1hAVelocity");
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")), lerpTarget, 0.3f));
    }
    public void OnAttackIdleEnter()
    {
        lerpTarget = 0f;
       
        pi.inputEnabled = true;
       
    }
    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")), lerpTarget, 0.3f));
    }
    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC", "Attack"))
        { deltaPos += (0.2f*(Vector3)_deltaPos+0.8f*deltaPos)/2.0f; }
        if (CheckState("attack1hA", "Attack"))
        { deltaPos += (0.2f*(Vector3)_deltaPos + 0.8f*deltaPos) / 2.0f; }
        
    }
}
