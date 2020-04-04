using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;
    public Animator anim;
    public float speed;
    public Rigidbody2D playerRigid;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);
        speed = playerRigid.inertia;
        anim.SetFloat("Speed", speed);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
            anim.SetBool("Jump", true);
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
            anim.SetBool("Jump", false);
        }

        
    }
}
