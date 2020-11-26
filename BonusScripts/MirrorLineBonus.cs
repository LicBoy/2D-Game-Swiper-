public class MirrorLineBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().CreateMirrorLine(duration);
        base.ActivateBonus();
    }
}
