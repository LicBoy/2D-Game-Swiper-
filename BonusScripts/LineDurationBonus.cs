public class LineDurationBonus : Bonus
{
    public float bonusDuration = 30;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().ChangeLineDuration(bonusDuration);
        base.ActivateBonus();
    }
}