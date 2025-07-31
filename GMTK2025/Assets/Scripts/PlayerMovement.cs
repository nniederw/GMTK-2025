using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float PlayerSpeed = 1.0f;
    //[SerializeField] private float ReactionSpeed = 5.0f;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        var ix = Input.GetAxis("Horizontal");
        var iy = Input.GetAxis("Vertical");
        Vector2 MovementDir = new Vector2(ix, iy).normalized * PlayerSpeed;
        rb2d.linearVelocity = MovementDir;
        //Vector2.Lerp(rb2d.linearVelocity, MovementDir, Time.fixedDeltaTime * ReactionSpeed);
        //rb2d.AddForce(MovementDir * Time.fixedDeltaTime);
        //rb2d.AddForce(MovementDir * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}