public class ArmorBonus : Bonus
{
    public int amountOfArmor = 5;

    public override void ActivateBonus()
    {
        GameController.instance.player.GiveArmor(amountOfArmor);
        base.ActivateBonus();
    }
}