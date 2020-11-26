public class InfiniteLineBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ChangeLineDuration(duration, true);
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().MakeLineBigger(duration);
        base.ActivateBonus();
    }
}
