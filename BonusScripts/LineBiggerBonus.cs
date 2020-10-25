using System.Drawing;

public class LineBiggerBonus : Bonus
{
    public float duration = 30;

    public override void ActivateBonus()
    {
        GameController.instance.lineRenderer.GetComponent<MouseMovement>().MakeLineBigger(duration);
        base.ActivateBonus();
    }
}
