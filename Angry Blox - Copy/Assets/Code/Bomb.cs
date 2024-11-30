using UnityEngine;

public class Bomb : MonoBehaviour 
{
    public float ThresholdImpulse = 5;
    public GameObject ExplosionPrefab;
    
    private bool hasDetonated = false;
    private Rigidbody2D projectileRb;

    private void Start()
    {
        var projectile = GameObject.FindGameObjectWithTag("Projectile");
        if (projectile != null)
        {
            projectileRb = projectile.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDetonated) return;

        float impactForce = 0f;
        
        if (collision.gameObject.CompareTag("Projectile") && projectileRb != null)
        {
            // For direct projectile hits, use relative velocity
            Vector2 relativeVelocity = collision.relativeVelocity;
            impactForce = relativeVelocity.magnitude * projectileRb.mass;
        }
        else
        {
            // For other collisions, calculate maximum normal impulse
            float maxImpulse = 0f;
            foreach (var contact in collision.contacts)
            {
                if (contact.normalImpulse > maxImpulse)
                {
                    maxImpulse = contact.normalImpulse;
                }
            }
            impactForce = maxImpulse;
        }
        
        if (impactForce >= ThresholdImpulse)
        {
            Boom();
        }
    }

    private void Boom()
    {
        if (hasDetonated) return;
        
        hasDetonated = true;
        
        var pointEffector = GetComponent<PointEffector2D>();
        if (pointEffector != null)
        {
            pointEffector.enabled = true;
        }
        
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        if (ExplosionPrefab != null)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        }
        
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        Invoke("Destruct", 0.1f);
    }

    private void Destruct()
    {
        Destroy(gameObject);
    }
}