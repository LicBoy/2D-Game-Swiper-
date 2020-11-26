using System.Drawing;

public class LineBiggerBonus : BonusWithDuration
{
    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().MakeLineBigger(duration);
        base.ActivateBonus();
    }
}
