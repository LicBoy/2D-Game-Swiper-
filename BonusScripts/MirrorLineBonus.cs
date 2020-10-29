public class MirrorLineBonus : Bonus
{
    public float duration;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().CreateMirrorLine(duration);
        base.ActivateBonus();
    }
}
