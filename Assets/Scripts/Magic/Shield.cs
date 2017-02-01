using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Shield : Magic {

    public Vector3 forwerd;

    GameObject magicalTeam;

    public Shield(Hand _lHand, Hand _rHand, float _scale, GameObject _magicalTeam) : base(_lHand, _rHand, _scale) {
        magicalTeam = Object.Instantiate(_magicalTeam, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public override bool Action() {
        var leftPos = leftHand.PalmPosition.ToUnityScaled() * scale;
        var leftNormal = leftHand.PalmNormal.ToUnity();

        magicalTeam.transform.position = leftPos + leftNormal;

        if(Vector3.Dot(forwerd, leftNormal) < 0.8f) {
            return true;
        }
        return false;
    }

    public override void Destroy() {
        magicalTeam.GetComponent<magicalTeam>().Finish();
    }

}
