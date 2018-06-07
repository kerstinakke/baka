﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : Movable
{

    public Vector3 correctRot;
    private float rotError = 2f;

    new public void Start()
    {
        base.Start();
        transform.position = correctPos;
        body.localEulerAngles = correctRot;
    }

    public void setCorrectPos(Vector3 pos) {
        correctPos = pos;
    }

    public override bool LetGo()
    {
        myCollider.gameObject.layer = LayerMask.NameToLayer("Default");
        //print (WithinLimits (posError) + " " + correctPos + " " + transform.position);
        bool both = true;
        if (WithinLimits(posError))
        {
            transform.position = correctPos;
            audioSource.Play();
        }
        else
            both = false;
        if (RotWithinLimits(rotError))
        {
            both = (both && true);
        }
        else
            both = false;
        return both;
    }

    protected virtual bool RotWithinLimits(float error)
    {
        if (Quaternion.Angle(body.localRotation, Quaternion.Euler(correctRot)) <= error)
        {
            return true;
        }
        return false;
    }
}

