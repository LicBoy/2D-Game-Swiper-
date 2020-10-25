public class SkyBlocksBonus : Bonus
{
    public float duration = 30;

    public override void ActivateBonus()
    {
        GameController.instance.blocks.GetComponent<SkyBlocks>().SendMessage("ActivateBlocks", duration);
        base.ActivateBonus();
    }
}