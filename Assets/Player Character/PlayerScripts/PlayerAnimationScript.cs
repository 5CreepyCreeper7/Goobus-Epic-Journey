using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void IdleAnimation()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsCrouching", false);
    }

    public void RunningAnimation()
    {
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsSprinting", false);
        anim.SetBool("IsCrouching", false);
    }

    public void SprintingAnimation()
    {
        anim.SetBool("IsSprinting", true);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsCrouching", false);
    }

    public void CrouchingAnimation()
    {
        anim.SetBool("IsCrouching", true);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsSprinting", false);
    }
}
