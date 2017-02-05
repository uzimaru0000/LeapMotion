using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Golem : MonoBehaviour {
 
    public float speed;
    public float deathTime;

    Vector3 pos;
    GameObject target;
    Rigidbody body;
    float timer;
    bool isDeath;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();

        Search();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isDeath) {
            if (target) pos = target.transform.position;
            pos.y = transform.position.y;
            transform.LookAt(pos);

            if ((pos - transform.position).magnitude >= 0.5f) {
                body.AddForce(transform.forward * speed);
            } else {
                Search();
            }

            var power = 0.1f * Mathf.Abs(Mathf.Sin(timer));
            body.MovePosition(transform.position + transform.up * power);
            timer += Time.deltaTime;

            if (timer >= deathTime) Destruct(); 
        }
	}
    
    void Search() {
        var collider = Physics.OverlapSphere(transform.position, 10);
        var count = collider.Count(x => x.CompareTag("Enemy"));
        foreach (var obj in collider) {
            if (obj.CompareTag("Enemy")) {
                var rand = Random.Range(0, 100);
                if(rand < 100 / count) {
                    target = obj.gameObject;
                }
            }
        }

        if (!target) {
            pos = Random.insideUnitSphere * 10;
        }
    }

    void Destruct() {
        isDeath = true;
        foreach(Transform child in transform) {
            var cbody = child.gameObject.AddComponent<Rigidbody>();
            Destroy(child.gameObject, 5);
        }
        transform.DetachChildren();

        var colliders = Physics.OverlapSphere(transform.position, 5);
        foreach(var obj in colliders) {
            var b = obj.GetComponent<Rigidbody>();
            if(b) {
                b.AddExplosionForce(10, transform.position + Random.insideUnitSphere * 0.25f, 5, 0, ForceMode.Impulse);
            }
        }
    }
}
