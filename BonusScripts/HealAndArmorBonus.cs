public class HealAndArmorBonus : Bonus
{
    public int amountOfHeal = 35;
    public int amountOfArmor = 15;

    public override void ActivateBonus()
    {
        GameController.instance.player.Heal(amountOfHeal);
        GameController.instance.player.GiveArmor(amountOfArmor);
        base.ActivateBonus();
    }
}
