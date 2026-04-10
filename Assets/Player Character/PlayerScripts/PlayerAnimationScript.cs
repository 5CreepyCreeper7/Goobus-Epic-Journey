using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void updateAnimations(float move, bool grounded, bool crouching, bool dashing, int randInt) {
        anim.SetBool("IsGrounded", grounded);
        anim.SetBool("IsCrouching", crouching);
        anim.SetBool("IsDashing", dashing);

        float jumpVelocity = GetComponent<Rigidbody2D>().linearVelocity.y;
        anim.SetFloat("JumpVelocity", jumpVelocity);

        if(randInt == 67) {
            anim.SetTrigger("Random");
        }

        float speed = Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x);
        anim.SetFloat("Speed", speed);
    }

    public void ForceIdle() {
        anim.SetFloat("Speed", 0);
    }
}
