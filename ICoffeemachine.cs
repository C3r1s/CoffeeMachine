namespace Exercise2;

public interface ICoffeemachine
{
    public void Power();
    protected internal void Protection(string name);
    protected internal void CheckEmptyIngredients();
    protected internal void CheckCleaning();
    protected internal void PrepareToBrew(bool Water, bool Milk, bool Coffee);
    public void OrderingMenu();
    protected internal void Cleaning();
    protected internal int AddIngredients(string ingridientName, int ingridientQuanity, int maxQuanity);
}