using UnityEngine;

public class TargetBox : MonoBehaviour
{
    /// <summary>
    /// Targets that move past this point score automatically.
    /// </summary>
    public static float OffScreen;

    // make sure one box can only score once
    private bool scoredOnce = false;

    internal void Start() {
        OffScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-100, 0, 0)).x;
    }

    internal void Update()
    {
        if (transform.position.x > OffScreen && !scoredOnce)
            Scored();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !scoredOnce) {
            Scored();
        }
    }

    private void Scored()
    {
        scoredOnce = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        ScoreKeeper.AddToScore(GetComponent<Rigidbody2D>().mass);
    }
}
