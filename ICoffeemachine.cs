namespace Exercise2;

public interface ICoffeemachine
{
    public void Power();
    private protected void AddWater();
    private protected void Protection();
    private protected int AddCoffeeBeans();
    private protected int AddMilk();
    private protected void CheckIngredients();
    private protected void CheckCleaning();
    private protected void PrepareToWork();
    private protected void Brewing();
    private protected void Cleaning();
    
}