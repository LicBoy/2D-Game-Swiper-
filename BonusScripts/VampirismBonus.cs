public class VampirismBonus : BonusWithDuration
{
    public int amountOfHeal = 1;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ActivateVampirism(duration, amountOfHeal);
        base.ActivateBonus();
    }
}