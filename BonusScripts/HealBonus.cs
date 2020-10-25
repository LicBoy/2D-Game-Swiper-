public class HealBonus : Bonus
{
    public int amountOfHeal = 10;

    public override void ActivateBonus()
    {
        GameController.instance.city.GetComponent<CityScript>().Heal(amountOfHeal);
        base.ActivateBonus();
    }
}