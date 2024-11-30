using UnityEngine;

public class TargetBox : MonoBehaviour
{
    private bool hasScored = false;
    
    public static float OffScreen;

    internal void Start() {
        OffScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-100, 0, 0)).x;
    }

    internal void Update()
    {
        if (transform.position.x > OffScreen)
            Scored();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision was with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Scored();
        }
    }

    private void Scored()
    {
        // Only score once
        if (hasScored)
            return;

        // Mark as scored
        hasScored = true;

        // Change color to green
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;

        // Add score based on mass
        var rigidBody = GetComponent<Rigidbody2D>();
        ScoreKeeper.AddToScore(rigidBody.mass);
    }
}
