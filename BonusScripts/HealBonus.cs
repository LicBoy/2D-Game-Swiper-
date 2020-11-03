public class HealBonus : Bonus
{
    public int amountOfHeal = 10;

    public override void ActivateBonus()
    {
        GameController.instance.player.Heal(amountOfHeal);
        base.ActivateBonus();
    }
}