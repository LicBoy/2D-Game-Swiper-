public class InfiniteLineBonus : Bonus
{
    public float bonusDuration = 10;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ChangeLineDuration(bonusDuration, true);
        base.ActivateBonus();
    }
}
