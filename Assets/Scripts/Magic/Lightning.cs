using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class Lightning : Magic {

    GameObject magicalTeam;
    GameObject lightning;

    public Lightning(Controller _controller, float _scale, GameObject _magicalTeam, GameObject _lightning) : base(_controller, _scale) {
        magicalTeam = Object.Instantiate(_magicalTeam, Vector3.zero, Quaternion.identity) as GameObject;
        lightning = _lightning;
    }

    public override bool Action() {
        Finger indexFinger = rightHand.Fingers.Where(x => x.Type() == Finger.FingerType.TYPE_INDEX).ToArray()[0];
        Vector3 tipPos = indexFinger.TipPosition.ToUnityScaled() * scale;
        Vector3 dir = indexFinger.Direction.ToUnity();
        Vector3 velocity = indexFinger.TipVelocity.ToUnityScaled();
        RaycastHit hit;
        int extendNum = rightHand.Fingers.Count(x => x.IsExtended);

        if (extendNum > 2) {
            magicalTeam.GetComponent<magicalTeam>().Finish();
            return true;
        }
        
        if (Physics.Raycast(tipPos, dir, out hit, 15)) {
            magicalTeam.transform.position = hit.point + hit.normal * 0.1f;
            magicalTeam.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
        } else {
            magicalTeam.transform.position = tipPos + indexFinger.Direction.ToUnity() * 15;
            magicalTeam.transform.rotation = Quaternion.identity;
        }

        if (velocity.y <= -0.8f) {
            var pos = magicalTeam.transform.position;
            var rotation = Quaternion.FromToRotation(-Vector3.forward, Vector3.up);
            pos.y = 10;
            var go = Object.Instantiate(lightning, pos, rotation) as GameObject;
            magicalTeam.GetComponent<magicalTeam>().Finish();
            Object.Destroy(go, 5);
            return true;
        }

        return false;
    }

    public override void Destroy() {
        
    }
}
