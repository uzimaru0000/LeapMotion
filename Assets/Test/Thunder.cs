using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start () {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
	}

    void OnParticleCollision (GameObject other) {
        int num = part.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < num; i++) {
            var pos = collisionEvents[i].intersection;
            var colliders = Physics.OverlapSphere(pos, 0.5f);
            foreach(var obj in colliders) {
                var body = obj.GetComponent<Rigidbody>();
                if(body) {
                    body.AddExplosionForce(10, pos, 1, 0, ForceMode.Impulse);
                }
            } 
        }
    }
}
