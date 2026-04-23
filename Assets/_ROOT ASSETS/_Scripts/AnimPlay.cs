using UnityEngine;

public class AnimPlay : MonoBehaviour
{
    public Animator playAnim;
    public string animationName;
    public static AnimPlay instance;

    private void Awake()
    {
        instance = this;
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
    
    public void PlayAimation()
    {
        playAnim.Play(animationName);
    }
    
}
