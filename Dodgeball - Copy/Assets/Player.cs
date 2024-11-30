using UnityEngine;

/// <summary>
/// Control the player on screen
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Prefab for the orbs we will shoot
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// How fast our engines can accelerate us
    /// </summary>
    public float EnginePower = 1;
    
    /// <summary>
    /// How fast we turn in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// How fast we should shoot our orbs
    /// </summary>
    public float OrbVelocity = 10;

    private Rigidbody2D rb; // TODO: from readme

    void Start()
    {
        // TODO: from the readme
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handle moving and firing.
    /// Called by Uniity every 1/50th of a second, regardless of the graphics card's frame rate
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        Manoeuvre();
        MaybeFire();
    }

    /// <summary>
    /// Fire if the player is pushing the button for the Fire axis
    /// Unlike the Enemies, the player has no cooldown, so they shoot a whole blob of orbs
    /// </summary>
    void MaybeFire()
    {
        // TODO
        if(Input.GetButton("Fire1")){ // not sure about the button .. check it after running
            // FireOrb();
            // from readme; So change MaybeFire so that it calls FireOrb() ten times, if the button is held down.
            for (int i=0; i<10; i++)
			{
				FireOrb();
			}
        }
    }

    /// <summary>
    /// Fire one orb.  The orb should be placed one unit "in front" of the player.
    /// transform.right will give us a vector in the direction the player is facing.
    /// It should move in the same direction (transform.right), but at speed OrbVelocity.
    /// </summary>
    private void FireOrb()
    {
        // TODO
        GameObject orb = Instantiate(OrbPrefab, (transform.position + transform.right * 1.5f), Quaternion.identity);
        // https://docs.unity3d.com/ScriptReference/Transform-right.html
        // had to add a constant value bc when placing the orbs at transform.right it collided with the player... not sure where it is caused / should check during OHs
        // for the position we add transform.right to place it one unit in front of the players direction

        Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>(); // ref: https://docs.unity3d.com/ScriptReference/Rigidbody2D.html
        orbRb.velocity = transform.right * OrbVelocity; // to set the speed
    }

    /// <summary>
    /// Accelerate and rotate as directed by the player
    /// Apply a force in the direction (Horizontal, Vertical) with magnitude EnginePower
    /// Note that this is in *world* coordinates, so the direction of our thrust doesn't change as we rotate
    /// Set our angularVelocity to the Rotate axis time RotateSpeed
    /// </summary>
    void Manoeuvre()
    {
        // TODO
        float playerX = Input.GetAxis("Horizontal");
        float playerY = Input.GetAxis("Vertical");
        Vector2 playerDirection = new Vector2(playerX, playerY) * EnginePower;

        rb.AddForce(playerDirection); // https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html

        rb.angularVelocity = Input.GetAxis("Rotate") * RotateSpeed; //playerX // this: Set our angularVelocity to the Rotate axis time RotateSpeed
        // degrees per second
        // https://docs.unity3d.com/ScriptReference/Rigidbody2D.html
    }

    /// <summary>
    /// If this is called, we got knocked off screen.  Deduct a point!
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(-1);
    }
}
