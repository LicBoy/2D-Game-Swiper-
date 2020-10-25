public class HealAndArmorBonus : Bonus
{
    public int amountOfHeal = 35;
    public int amountOfArmor = 15;

    public override void ActivateBonus()
    {
        GameController.instance.city.GetComponent<CityScript>().Heal(amountOfHeal);
        GameController.instance.city.GetComponent<CityScript>().GiveArmor(amountOfArmor);
        base.ActivateBonus();
    }
}
