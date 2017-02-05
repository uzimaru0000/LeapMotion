using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Shield : Magic {

    Vector3 forwerd;
    GameObject magicalTeam;

    public Shield(Controller _controller, float _scale, GameObject _magicalTeam, Vector3 _forwerd) : base(_controller, _scale) {
        magicalTeam = Object.Instantiate(_magicalTeam, Vector3.zero, Quaternion.identity) as GameObject;
        forwerd = _forwerd;
    }

    public override bool Action() {
        var leftPos = leftHand.PalmPosition.ToUnityScaled() * scale;
        var leftNormal = leftHand.PalmNormal.ToUnity();
        var rightPos = rightHand.PalmPosition.ToUnityScaled() * scale;
        var rightNormal = rightHand.PalmNormal.ToUnity();

        magicalTeam.transform.position = (leftPos + rightPos) / 2 + forwerd;

        if(Vector3.Dot(forwerd, leftNormal) < 0.5f || Vector3.Dot(forwerd, rightNormal) < 0.5f) {
            return true;
        }
        return false;
    }

    public override void Destroy() {
        magicalTeam.GetComponent<magicalTeam>().Finish();
    }

}
