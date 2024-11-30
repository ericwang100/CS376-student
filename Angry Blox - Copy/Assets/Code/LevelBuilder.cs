using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    public GameObject targetBoxPrefab;
    public GameObject explosiveBoxPrefab;
    public GameObject projectilePrefab;
    public GameObject groundPrefab;

    void Start()
    {
        SetupPhysics();
        CreatePhysicsMaterial();
        SetupCamera();
        BuildLevel();
    }

    void SetupPhysics()
    {
        Physics2D.gravity = new Vector2(0, -15f);

        // Note: Default material properties need to be set in Project Settings manually
        // or through a custom editor script as they're not accessible through regular API
        // Choose whatever material properties you have like :)
    }

    PhysicsMaterial2D CreatePhysicsMaterial()
    {
        PhysicsMaterial2D boxMaterial = new PhysicsMaterial2D("BoxMaterial");
        boxMaterial.friction = 0.4f;
        boxMaterial.bounciness = 0.1f;
        return boxMaterial;
    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 8;
    }

    void BuildLevel()
    {
        PhysicsMaterial2D boxMaterial = CreatePhysicsMaterial();

        GameObject ground = Instantiate(groundPrefab, new Vector3(0, -4, 0), Quaternion.identity);
        ground.transform.localScale = new Vector3(20, 1, 1);
        ground.tag = "Ground";

        GameObject projectile = Instantiate(projectilePrefab, new Vector3(-8, 0, 0), Quaternion.identity);
        SetupProjectile(projectile);

        CreateTargetBox(new Vector3(5, -3, 0), 3, boxMaterial);
        CreateTargetBox(new Vector3(7, -3, 0), 3, boxMaterial);
        CreateTargetBox(new Vector3(9, -3, 0), 3, boxMaterial);
        CreateTargetBox(new Vector3(6, -1, 0), 2, boxMaterial);
        CreateExplosiveBox(new Vector3(8, -1, 0), 1.5f, boxMaterial);
        CreateTargetBox(new Vector3(7, 1, 0), 1, boxMaterial);
        CreateTargetBox(new Vector3(9, 1, 0), 1, boxMaterial);
    }

    void SetupProjectile(GameObject projectile)
    {
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.mass = 2f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;

        SpringJoint2D spring = projectile.GetComponent<SpringJoint2D>();
        spring.frequency = 4.5f;
        spring.dampingRatio = 0.8f;
        spring.distance = 2f;
        spring.maxDistance = 3f;
    }

    void CreateTargetBox(Vector3 position, float mass, PhysicsMaterial2D material)
    {
        GameObject box = Instantiate(targetBoxPrefab, position, Quaternion.identity);
        Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
        rb.mass = mass;
        
        BoxCollider2D collider = box.GetComponent<BoxCollider2D>();
        collider.sharedMaterial = material;
    }

    void CreateExplosiveBox(Vector3 position, float mass, PhysicsMaterial2D material)
    {
        GameObject box = Instantiate(explosiveBoxPrefab, position, Quaternion.identity);
        
        Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
        rb.mass = mass;
        
        BoxCollider2D collider = box.GetComponent<BoxCollider2D>();
        collider.sharedMaterial = material;

        Bomb bomb = box.GetComponent<Bomb>();
        bomb.ThresholdImpulse = 4f;

        PointEffector2D pointEffector = box.GetComponent<PointEffector2D>();
        pointEffector.forceMagnitude = 15f;
        pointEffector.forceVariation = 0f;
        pointEffector.forceSource = EffectorForceSource2D.ForceSource;
        pointEffector.enabled = false; // Start disabled
    }
}