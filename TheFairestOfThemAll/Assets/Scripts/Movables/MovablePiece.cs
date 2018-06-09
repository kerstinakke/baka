using System.Collections;
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
        bool posCorrect = false;
        bool rotCorrect = false;
        if (WithinLimits(posError))
        {
            transform.position = correctPos;
            audioSource.Play();
            posCorrect = true;
        }
        if (RotWithinLimits(rotError))
        {
            rotCorrect = true;
        }
        if ((posCorrect || rotCorrect) && !(posCorrect && rotCorrect))
            OverlayEffects.halfCorrect();
        return posCorrect || rotCorrect;
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

