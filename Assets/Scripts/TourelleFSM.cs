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
    
    private List<MoveActorV2> enemisInRange=new List<MoveActorV2>();
    private float _timer;
    
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

        Debug.Log(enemisInRange.Count + " énémie dans la zone");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MoveActorV2>() != null)
        {
            if (!enemisInRange.Contains(other.GetComponent<MoveActorV2>())) enemisInRange.Add( other.GetComponent<MoveActorV2>()); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MoveActorV2>() != null) if (enemisInRange.Contains(other.GetComponent<MoveActorV2>())) enemisInRange.Remove( other.GetComponent<MoveActorV2>()); 
    }
}
