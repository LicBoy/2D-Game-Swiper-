public class ArmorBonus : Bonus
{
    public int amountOfArmor = 5;

    public override void ActivateBonus()
    {
        GameController.instance.city.GetComponent<CityScript>().GiveArmor(amountOfArmor);
        base.ActivateBonus();
    }
}