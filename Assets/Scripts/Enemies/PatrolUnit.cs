﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolUnit : MonoBehaviour, IResetable
{
    public int nbCheckPoint;
    public bool loopPatrol;
    public float patrolSpeed;
    public float chaseSpeed;
    public float perceptionRadius;
    public float keepFocusRadius;

    protected int currentCheckPoint = 0;
    protected int sens = 1; //1 = droite, -1 = gauche
    [SerializeField]
    protected Vector3[] checkPoints;
    [SerializeField]
    protected PatrolState state;

    /// <summary>
    /// returns the next position the Unit should go. It it reaches the limits (whether it is mion or max) the behavior depends on if the 
    /// Unit is set to Looping or not.
    /// </summary>
    public void GetNextCheckPoint()
    {
        if (loopPatrol)
        {
            if (currentCheckPoint + sens > checkPoints.Length - 1)
            {
                currentCheckPoint = 0;
            }
            else if (currentCheckPoint + sens < 0)
            {
                currentCheckPoint = checkPoints.Length - 1;
            }
            else
            {
                currentCheckPoint += sens;
            }

        }
        else
        {
            if (currentCheckPoint + sens > checkPoints.Length - 1 || currentCheckPoint + sens < 0)
            {
                sens *= -1;
            }
            currentCheckPoint += sens;
        }
    }


    public void Reset()
    {
        transform.position = checkPoints[0];
        sens = 1;
        currentCheckPoint = 0;
        state = PatrolState.Patrol;
    }








#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //radius de perception et focus
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, perceptionRadius);
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, keepFocusRadius);


        //pathing de la patrouille
        int cpt = 0;
        for (int i = 0; i < checkPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(checkPoints[i], checkPoints[i + 1]);
            UnityEditor.Handles.Label((checkPoints[i] + checkPoints[i + 1]) / 2, i.ToString());
            cpt++;
        }
        if (loopPatrol)
        {
            Gizmos.DrawLine(checkPoints[0], checkPoints[checkPoints.Length - 1]);
            UnityEditor.Handles.Label((checkPoints[0] + checkPoints[checkPoints.Length - 1]) / 2, cpt.ToString());
        }
    }
    #endif

    #region Editor
    public void GoToCheckPoint(int i)
    {
        transform.position = checkPoints[i];
    }

    public void SetCheckPoint(int i)
    {
        checkPoints[i] = transform.position;
    }

    public void InitCheckPoints()
    {
        checkPoints = new Vector3[nbCheckPoint];
    }

    public void OnValidate()
    {
        if (!Application.isPlaying && PlayerPrefs.GetInt("oldNbCheckPoint", 0) != nbCheckPoint)
        {
            PlayerPrefs.SetInt("oldNbCheckPoint", nbCheckPoint);
            InitCheckPoints();
        }

    }
    #endregion

}

public enum PatrolState { Patrol, Chase, Attack }
