public class SkyBlocksBreakableBonus : BonusWithDuration
{
    public bool isBreakable = true;

    public override void ActivateBonus()
    {
        for (int i = 0; i < GameController.instance.blocks.Length; i++)
            GameController.instance.blocks[i].ActivateBlocks(duration, isBreakable);
        base.ActivateBonus();
    }
}