using UnityEngine;

public class Bomb : MonoBehaviour {
    public float ThresholdImpulse = 5;
    public GameObject ExplosionPrefab;

    private void Destruct() {
        Destroy(gameObject);
    }

    private void Boom() {
        GetComponent<PointEffector2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        Invoke("Destruct", 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D c) {
        foreach (ContactPoint2D contact in c.contacts) {
            if (contact.normalImpulse >= ThresholdImpulse) {
                Boom();
                return;
            }
        }
    }

}
