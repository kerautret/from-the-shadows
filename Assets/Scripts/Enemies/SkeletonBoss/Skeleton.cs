﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IResetable
{
    public Transform[] points;
    public float timeBetweenAttacks;
    public GameObject hands;
    public GameObject player1;
    public GameObject player2;
    public GameObject leftZone;
    public GameObject middleZone;
    public GameObject rightZone;
    public GameObject leftZoneBis;
    public GameObject rightZoneBis;

    private int hp = 3;
    private int laneToAttack = 0;
    private string stringDirection;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TriggerAttack", timeBetweenAttacks, timeBetweenAttacks);
    }

    public void TriggerAttack()
    {
        FindTarget();
        string trigger = "Attack" + stringDirection + laneToAttack;
        hands.GetComponent<Animator>().SetTrigger(trigger);
    }

    public void FindTarget()
    {
        float min = Mathf.Infinity;
        for (int i = 0; i < points.Length / 2; i++) {
            float minLeft = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2].position),
                                      Vector3.Distance(player2.transform.position, points[i * 2].position));
            float minRight = Mathf.Min(Vector3.Distance(player1.transform.position, points[i * 2 + 1].position),
                                       Vector3.Distance(player2.transform.position, points[i * 2 + 1].position));
            if (minLeft < min && minLeft < minRight)
            {
                min = minLeft;
                laneToAttack = i;
                stringDirection = "Left";
            } else if (minRight < min && minRight < minLeft)
            {
                min = minRight;
                laneToAttack = i;
                stringDirection = "Right";
            }
        }
    }
    
    public void GetHurt()
    {
        Debug.Log("aïe");
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Battlecry");
        hp--;
        if (hp == 0)
        {
            Die();
            Invoke("DestroyMiddleZone", 3);
        }
           

        if (hp == 2)
            Invoke("DestroyLeftZone",1);
        if (hp == 1)
            Invoke("DestroyRightZone",1);
    }

    public void Die()
    {
        transform.Find("SkeletonFBX").GetComponent<Animator>().SetTrigger("Die");
        CancelInvoke();
    }

    public void Reset()
    {
        hp = 3;

        leftZone.SetActive(true);
        rightZone.SetActive(true);
        leftZoneBis.SetActive(false);
        rightZoneBis.SetActive(false);
    }

    public void DestroyLeftZone()
    {
        leftZone.SetActive(false);
        leftZoneBis.SetActive(true);
    }

    public void DestroyRightZone()
    {
        rightZone.SetActive(false);
        rightZoneBis.SetActive(true);
    }

    public void DestroyMiddleZone()
    {
        middleZone.SetActive(false);
    }
}
