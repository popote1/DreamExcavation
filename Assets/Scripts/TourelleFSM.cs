using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Actors;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TourelleFSM : MonoBehaviour
{
    public float RangeRadiaus;
    public float LazerLiveTime;
    public float ReloadTime;
    public CircleCollider2D CircleCollider2D;
    public LineRenderer LineRenderer;
    public GameObject OutLine;
    public AudioSource AudioSource;
    [Header("PowerEffect")] 
    public GameObject ZoneEffect;
    public float ZoneSize;
    public GameObject PowerEffect;
    public float EffectTime;
    public float DestructionRange;
    public float AddForceRange;
    public float AddForcePower;

    
    private List<MoveActorV2> enemisInRange=new List<MoveActorV2>();
    private float _timer;
    private float _powereffectTimer;
    
    void Start()
    {
        LineRenderer.SetPosition(0,transform.position+transform.forward*-3);
        LineRenderer.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CircleCollider2D.radius != RangeRadiaus) CircleCollider2D.radius = RangeRadiaus;
        
        if (_timer >= ReloadTime && enemisInRange.Count > 0)
        {
            enemisInRange.RemoveAll(o => o == null);
            if (enemisInRange.Count > 0)
            {
                LineRenderer.enabled = true;
                LineRenderer.SetPosition(1,enemisInRange[0].transform.position);
                _timer = 0;
                AudioSource.Play();
                Destroy(enemisInRange[0].gameObject);
            }
        }

        if (_timer > LazerLiveTime && LineRenderer.enabled)
        {
            LineRenderer.enabled = false;
        }

        if (_timer < ReloadTime)
        {
            _timer += Time.deltaTime;
        }

        if (PowerEffect.activeSelf) {
            _powereffectTimer += Time.deltaTime;
            if (_powereffectTimer >= EffectTime) {
                _powereffectTimer = 0;
                PowerEffect.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<MoveActorV2>() != null) if (!enemisInRange.Contains(other.GetComponent<MoveActorV2>())) enemisInRange.Add( other.GetComponent<MoveActorV2>());
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<MoveActorV2>() != null) if (enemisInRange.Contains(other.GetComponent<MoveActorV2>())) enemisInRange.Remove( other.GetComponent<MoveActorV2>()); 
    }

    public void OnSelect() {
        OutLine.SetActive(true);
    }

    public void OnDeselect() {
        OutLine.SetActive(false);
    }

    public void DoPower(Vector2 origine)
    {
        PowerEffect.transform.position = origine;
        PowerEffect.SetActive(true);
        Collider2D[] affectd = new Collider2D[50];
        Physics2D.OverlapCircle(origine, DestructionRange,new ContactFilter2D().NoFilter(),affectd);
        foreach (Collider2D coll in affectd){
            if (coll != null) {
                if (coll.transform.CompareTag("MoveActor")||coll.transform.CompareTag("Tree")) {
                    Destroy(coll.gameObject);
                }
            }
        }
        Physics2D.OverlapCircle(origine, AddForceRange,new ContactFilter2D().NoFilter(),affectd);
        foreach (Collider2D coll in affectd){
            if (coll != null) {
                if (coll.transform.CompareTag("MoveActor")) {
                    coll.GetComponent<Rigidbody2D>().AddForce((new Vector2(coll.transform.position.x,coll.transform.position.y)-origine).normalized*AddForcePower,ForceMode2D.Impulse);
                }
            }
        }
    }
}
