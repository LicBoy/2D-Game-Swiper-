public class LineDurationBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ChangeLineDuration(duration);
        base.ActivateBonus();
    }
}