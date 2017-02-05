using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Summon : Magic {

    GameObject magicalTeam;
    GameObject golem;

    public Summon(Controller _controller, float _scale, GameObject _magicalTeam, GameObject _golem) : base(_controller, _scale) {
        magicalTeam = Object.Instantiate(_magicalTeam, Vector3.zero, Quaternion.Euler(90, 0, 0)) as GameObject;
        golem = _golem;
    }

    public override bool Action() {
        var leftPos = leftHand.PalmPosition.ToUnityScaled() * scale;
        var rightPos = rightHand.PalmPosition.ToUnityScaled() * scale;
        var leftNormal = leftHand.PalmNormal.ToUnity();
        var rightNormal = rightHand.PalmNormal.ToUnity();
        var leftVerocity = leftHand.PalmVelocity.ToUnityScaled();
        var rightVerocity = rightHand.PalmVelocity.ToUnityScaled();

        if (Vector3.Dot(Vector3.down, leftNormal) < 0.8f && Vector3.Dot(Vector3.down, rightNormal) < 0.8f) {
            return true;
        }

        var pos = (leftPos + rightPos) / 2;
        pos.y = 0.1f;
        magicalTeam.transform.position = pos + Vector3.forward;

        if(((leftVerocity + rightVerocity) / 2).y < -0.8f) {
            Object.Instantiate(golem, magicalTeam.transform.position, Quaternion.identity);
            return true;
        }

        return false;
    }

    public override void Destroy() {
        magicalTeam.GetComponent<magicalTeam>().Finish();
    }

}
