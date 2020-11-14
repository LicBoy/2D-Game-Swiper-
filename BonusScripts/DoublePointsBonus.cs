public class DoublePointsBonus : Bonus
{
    public float doublePointsBonusDuration = 30f;

    public override void ActivateBonus()
    {
        GameController.instance.StartCoroutine("DoublePointsBonus", doublePointsBonusDuration);
        base.ActivateBonus();
    }
}