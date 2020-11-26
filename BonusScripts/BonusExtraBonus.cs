public class BonusExtraBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.GetComponent<BonusGenerator>().DropBonusAmountOfDrops();
        GameController.instance.GetComponent<BonusGenerator>().StartCoroutine("ActivateBonusesDoublerBonus", duration);
        base.ActivateBonus();
    }
}