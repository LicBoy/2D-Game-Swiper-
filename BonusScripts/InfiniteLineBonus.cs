public class InfiniteLineBonus : BonusWithDuration
{
    public const float fixedDuration = 10f;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ChangeLineDuration(fixedDuration, true);
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().MakeLineBigger(fixedDuration);
        base.ActivateBonus();
    }
}
