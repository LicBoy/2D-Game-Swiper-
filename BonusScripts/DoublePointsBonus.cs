public class DoublePointsBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.StartCoroutine("DoublePointsBonus", duration);
        base.ActivateBonus();
    }
}