using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlocksAnimationsScript : MonoBehaviour
{
    private Animation animation;
    private string idleAnimName = "SkyBlockIdleAnim";
    private string appearAnimName = "SkyBlocksAppearAnim";
    private string disappearAnimName = "SkyBlocksDisappearAnim";


    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public void PlayAppearIdleAnimation()
    {
        animation.Play(appearAnimName);
        animation.PlayQueued(idleAnimName, QueueMode.CompleteOthers);
    }

    public void PlayDisappearAnimation()
    {
        animation.Play(disappearAnimName);
    }

    public void RenewBonusTimeAnimation()
    {
        animation.Play(disappearAnimName);
        animation.PlayQueued(appearAnimName);
        animation.PlayQueued(idleAnimName);
    }
}
