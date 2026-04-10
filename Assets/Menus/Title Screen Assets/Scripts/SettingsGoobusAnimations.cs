using UnityEngine;

public class TitleGoobusAnimations : MonoBehaviour
{
    public Animator anim;

     void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.LogError("Animator NOT found on Goobus!");
    }

    public void setMenuInt(int i) {
        anim.SetInteger("MenuInt", i);
    }

    public int getMenuInt() {
        return anim.GetInteger("MenuInt");
    }
}
