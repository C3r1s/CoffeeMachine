namespace Exercise2;

using static Cli;
enum Profile
{
    Default,
    Custom,
}

enum CoffeeType
{
    None,
    Espresso,
    Cappuccino
}

public class Coffeemachine : ICoffeemachine
{
    private const int MAX_WATER = 1000;
    private const int MAX_COFFEE = 100;
    private const int MAX_MILK = 500;
    private const int COFFEE_PREPARATION_LIMIT = 2;
    private static int SessionCoffeeCups = 0;

    private static Profile UserProfile { get; set; } = Profile.Default;
    private Logger _logger = new Logger();
    private bool CoffeeIsNotEmpty { get; set; }
    private bool MilkIsNotEmpty { get; set; }
    private bool WaterIsNotEmpty { get; set; }
    private bool CleaningChecked { get; set; }
    private bool AllGreen { get; set; }
    private int NumberOfCoffeePreparations { get; set; }

    public bool IsPowered;

    private int _water;
    private int _milk;
    private int _coffee;

    private int Water
    {
        get => _water;
        set { _water = value >= 0 ? (value <= MAX_WATER ? value : MAX_WATER) : 0; }
    }

    private int Milk
    {
        get => _milk;
        set { _milk = value >= 0 ? (value <= MAX_MILK ? value : MAX_MILK) : 0; }
    }

    private int Coffee
    {
        get => _coffee;
        set { _coffee = value >= 0 ? (value <= MAX_COFFEE ? value : MAX_COFFEE) : 0; }
    }

    public Coffeemachine(int water, int milk, int coffee)
    {
        Water = water;
        Milk = milk;
        Coffee = coffee;
    }


    public void Power()
    {
        string userchoice;
        do
        {
            PrintLine("Would you like to power the machine? (y/n)");
            userchoice = Console.ReadLine()!;
            Console.Clear();
            if (userchoice == "y")
            {
                Thread.Sleep(500);
                Console.Clear();
                Animation("Machine is powering up", 3);
                Thread.Sleep(1500);
                Console.Clear();
                PrintLine("Machine is now powered", ConsoleColor.Green);
                _logger.LogWrite($"Machine powered up");
                IsPowered = true;
                _logger.LogWrite($"Power status: {IsPowered}");
                _logger.LogWrite("Power command completed");
            }
            else if (userchoice == "n")
            {
                Console.Clear();
                PrintLine("The machine remained off");
                IsPowered = false;
                _logger.LogWrite($"Power status: {IsPowered}");
                _logger.LogWrite("Power command completed");
            }
            else
            {
                Console.Clear();
                PrintError("Invalid input");
                IsPowered = false;
            }
        } while (userchoice != "y" && userchoice != "n");
    }

    void ICoffeemachine.Protection(string name)
    {
        PrintWarning($"WARNING! {name} tank is full in the machine");
        _logger.LogWrite($"Shielded from overflow by {name}");
        _logger.LogWrite($"Protection completed");
    }

    void ICoffeemachine.CheckEmptyIngredients()
    {
        do
        {
            if (Water <= MAX_WATER && Water != 0)
            {
                WaterIsNotEmpty = true;
                _logger.LogWrite($"Water tank is not empty");
            }
            else if (Water == 0)
            {
                Water = ((ICoffeemachine)this).AddIngredients("Water", Water, MAX_WATER);
                WaterIsNotEmpty = true;
                _logger.LogWrite($"Water tank is empty, adding {MAX_WATER - Water} liters");
            }

            if (Coffee <= MAX_COFFEE && Coffee != 0)
            {
                CoffeeIsNotEmpty = true;
                _logger.LogWrite("Coffee tank is not empty");
            }
            else if (Coffee == 0)
            {
                Coffee = ((ICoffeemachine)this).AddIngredients("Coffee", Coffee, MAX_COFFEE);
                CoffeeIsNotEmpty = true;
                _logger.LogWrite($"Coffee tank is empty, adding {MAX_COFFEE - Coffee} grams");
            }

            if (Milk <= MAX_MILK && Milk != 0)
            {
                MilkIsNotEmpty = true;
                _logger.LogWrite($"Milk tank is not empty");
            }
            else if (Milk == 0)
            {
                Milk = ((ICoffeemachine)this).AddIngredients("Milk", Milk, MAX_MILK);
                MilkIsNotEmpty = true;
                _logger.LogWrite($"Milk tank is empty, adding {MAX_MILK - Milk} liters");
            }
        } while (!(WaterIsNotEmpty && CoffeeIsNotEmpty && MilkIsNotEmpty));
        _logger.LogWrite("CheckEmptyIngredients completed");
    }

    private bool CheckNeededIngridients(string name, int machineIngridient, int neededIngridient)
    {
        _logger.LogWrite($"Checking {name} tank");
        if (machineIngridient < neededIngridient)
        {
            ((ICoffeemachine)this).AddIngredients(name, machineIngridient, MAX_COFFEE);
            _logger.LogWrite($"Added {neededIngridient - machineIngridient} grams of {name}");
        }
        _logger.LogWrite($"Ingridient {name} checked");
        _logger.LogWrite("CheckNeededIngridients command completed");
        
        return true;
    }


    void ICoffeemachine.CheckCleaning()
    {
        _logger.LogWrite("Checking cleaning");
        string userChoice;
        if (NumberOfCoffeePreparations == COFFEE_PREPARATION_LIMIT)
        {
            Console.Clear();
            _logger.LogWrite($"Cleaning required");
            PrintError("The machine is dirty and cannot start brewing");
            do
            {
                PrintLine("Do you want to start cleaning? (y/n)");
                userChoice = Console.ReadLine()!;
                if (userChoice == "y")
                {
                    Console.Clear();
                    ((ICoffeemachine)this).Cleaning();
                    CleaningChecked = true;
                    _logger.LogWrite("CheckCleaning command completed");
                }
                else if (userChoice == "n")
                {
                    Console.Clear();
                    _logger.LogWrite($"Cleaning still required");
                    PrintError("ERROR! The machine is dirty and can't start brewing. \nUser has to start cleaning");
                }
                else
                {
                    Console.Clear();
                    PrintError("Invalid input");
                }
            } while (userChoice != "y");
        }
        else
        {
            CleaningChecked = true;
            _logger.LogWrite("CheckCleaning command completed");
        }
    }


    void ICoffeemachine.PrepareToBrew(bool Water, bool Milk, bool Coffee)
    {
        _logger.LogWrite("Preparing to brew");
        if (WaterIsNotEmpty && CoffeeIsNotEmpty && MilkIsNotEmpty && CleaningChecked)
        {
            if (Coffee && Milk && Water)
            {
                AllGreen = true;
                Console.Clear();
                PrintSucces("The machine is ready to work");
                _logger.LogWrite("PrepareToBrew command completed (all ingredients are in place)");
                Thread.Sleep(1000);
            }
        }
        else
        {
            Console.Clear();
            PrintError("The machine is not ready to work");
            _logger.LogWrite("PrepareToBrew command completed (failure)");
        }
    }

    private void Brewing(CoffeeType choice, int cupsQanity)
    {
        _logger.LogWrite($"Brewing {choice} with {cupsQanity} cups");
        int neededCoffee;
        int neededMilk;
        int neededWater;
        bool enoughWater;
        bool enoughMilk;
        bool enoughCoffee;

        switch (choice)
        {
            case CoffeeType.Cappuccino:
                _logger.LogWrite("Cappuccino brewing started");
                neededCoffee = 7;
                enoughCoffee = CheckNeededIngridients("Coffee", Coffee, neededCoffee);
                Coffee = _coffee - neededCoffee;
                neededMilk = 75;
                enoughMilk = CheckNeededIngridients("Milk", Milk, neededMilk);
                Milk = _milk - neededMilk;
                neededWater = 30;
                Water = _water - neededWater;
                enoughWater = CheckNeededIngridients("Water", Water, neededWater);
                Thread.Sleep(1000);
                ((ICoffeemachine)this).PrepareToBrew(enoughWater, enoughMilk, enoughCoffee);

                Thread.Sleep(500);
                Console.Clear();
                Animation($"Brewing {choice}", 3);
                Console.Clear();
                PrintLine("Brewing complete!", ConsoleColor.Green);
                NumberOfCoffeePreparations++;
                _logger.LogWrite("Brewing completed");
                _logger.LogWrite("Brew command completed");
                break;
            case CoffeeType.Espresso:
                _logger.LogWrite("Espresso brewing started");
                neededCoffee = 10;
                enoughCoffee = CheckNeededIngridients("Coffee", Coffee, neededCoffee);
                Coffee = _coffee - neededCoffee;
                neededMilk = 30;
                enoughMilk = CheckNeededIngridients("Milk", Milk, neededMilk);
                Milk = _milk - neededMilk;
                neededWater = 75;
                enoughWater = CheckNeededIngridients("Water", Water, neededWater);
                Water = _water - neededWater;
                Thread.Sleep(1000);
                ((ICoffeemachine)this).PrepareToBrew(enoughWater, enoughMilk, enoughCoffee);
                Thread.Sleep(500);
                Console.Clear();
                Animation($"Brewing {choice}", 3);
                Console.Clear();
                PrintLine("Brewing complete!", ConsoleColor.Green);
                Thread.Sleep(1000);
                NumberOfCoffeePreparations++;
                _logger.LogWrite("Brewing completed");
                _logger.LogWrite("Brew command completed");
                break;
        }
    }

    void ICoffeemachine.Cleaning()
    {
        _logger.LogWrite("Cleaning started");
        Thread.Sleep(750);
        Console.Clear();
        PrintLine("Preparing to start cleaning");
        Thread.Sleep(2000);
        Animation("Cleaning", 3);
        NumberOfCoffeePreparations--;
        NumberOfCoffeePreparations = 0;
        Console.Clear();
        Animation("Cleaning", 1);
        Console.Clear();
        PrintLine("Cleaning complete!", ConsoleColor.Green);
        _logger.LogWrite("Cleaning completed");
        _logger.LogWrite("Cleaning command completed");
    }

    int ICoffeemachine.AddIngredients(string ingridientName, int ingridientQuanity, int maxQuanity)
    {
        _logger.LogWrite("AddIngridents command started");
        string userChoice;
        Console.Clear();
        _logger.LogWrite($"Error! {ingridientName} reqired");
        PrintError($"There is no {ingridientName}, would you like to add? (y/n)");
        userChoice = Console.ReadLine()!;
        Console.Clear();
        Console.Clear();
        if (userChoice == "y")
        {
            PrintLine($"How many {ingridientName} do you want to add?");
            int quantity = int.Parse(Console.ReadLine()!);
            Console.Clear();
            if (quantity > 0 && quantity <= maxQuanity)
            {
                ingridientQuanity = quantity;
            }
            else
            {
                ((ICoffeemachine)this).Protection(ingridientName);
            }
            _logger.LogWrite($"{ingridientName} added");
        }
        else if (userChoice == "n")
        {
            Console.Clear();
            PrintError("ERROR! There is no {name}");
            _logger.LogWrite("AddIngridents command completed (failure)");
        }
        else
        {
            Console.Clear();
            PrintError("Invalid input");
        }
        _logger.LogWrite("AddIngridents command completed");
        return ingridientQuanity;
    }

    public void StartMenu(string choice)
    {
        _logger.LogWrite("StartMenu command started");
        do
        {
            PrintMenu();
            choice = Console.ReadLine()!;
            switch (choice)
            {
                case "1":
                    if (UserProfile == Profile.Default)
                    {
                        _logger.LogWrite($"Brewing Coffee in {UserProfile} profile");
                        DefaultMenu(CoffeeType.Cappuccino, 2);
                        choice = "n";
                    }
                    else
                    {
                        do
                        {
                            _logger.LogWrite($"Brewing Coffee in {UserProfile} profile");
                            OrderingMenu();
                            PrintWarning("Do you want to continue? (y/n)");
                            choice = Console.ReadLine()!;
                        } while (choice == "y");
                    }

                    break;
                case "2":
                    IngridientsInfoMenu();
                    choice = "yes";
                    break;
                case "3":
                    ProfilesMenu();
                    choice = "yes";
                    break;
                case "4":
                    RecipesMenu();
                    choice = "yes";
                    break;
                case "6":
                    _logger.ShowLog();
                    choice = "yes";
                    break;
                default:
                    _logger.LogWrite("Exiting");
                    break;
            }
        } while (choice == "yes" || choice == "n");
    }

    public void OrderingMenu()
    {
        _logger.LogWrite("OrderingMenu command started");
        int cupsReady = 0;
        bool powered = IsPowered;
        if (powered)
        {
            Console.Clear();
            PrintLine("\n\tWhich coffee do you want to brew?");
            Thread.Sleep(200);
            PrintLine("\t1. Espresso", ConsoleColor.Green);
            Thread.Sleep(200);
            PrintLine("\t2. Cappuccino", ConsoleColor.Green);
            string userChoice = Console.ReadLine()!;
            Console.Clear();
            CoffeeType userCoffee = CoffeeType.None;
            switch (userChoice)
            {
                case "1":
                    userCoffee = CoffeeType.Espresso;
                    break;
                case "2":
                    userCoffee = CoffeeType.Cappuccino;
                    break;
            }
            _logger.LogWrite($"Coffee chosen: {userCoffee}");
            ((ICoffeemachine)this).CheckEmptyIngredients();
            string mode = CupsQuanityMenu();
            int cupsQanity;
            switch (mode)
            {
                case "1":
                    ((ICoffeemachine)this).CheckCleaning();
                    Brewing(userCoffee, 1);
                    PrintLine($"\n\tEnjoy your cup of {userCoffee}!", ConsoleColor.Green);
                    _logger.LogWrite("OrderingMenu command completed");
                    break;
                case "2":
                    cupsQanity = MultipleCupsMenu();
                    do
                    {
                        ((ICoffeemachine)this).CheckCleaning();
                        Brewing(userCoffee, cupsQanity);
                        cupsReady++;
                    } while (cupsReady != cupsQanity);

                    PrintLine($"\n\tHere your {cupsReady} cups of {userCoffee}!", ConsoleColor.Green);
                    _logger.LogWrite("OrderingMenu command completed");
                    break;
                case "3":
                    cupsQanity = 3;
                    do
                    {
                        ((ICoffeemachine)this).CheckCleaning();
                        Brewing(userCoffee, cupsQanity);
                        cupsReady++;
                    } while (cupsReady != cupsQanity);

                    PrintLine($"\n\tHere your {cupsReady} cups of {userCoffee}!", ConsoleColor.Green);
                    _logger.LogWrite("OrderingMenu command completed");
                    break;
            }
        }
        else
        {
            Console.Clear();
            PrintError("Machine is not powered");
            _logger.LogWrite("OrderingMenu command completed(Failure)");
        }
    }

    public void IngridientsInfoMenu()
    {
        _logger.LogWrite("IngridientsInfoMenu command started");
        Thread.Sleep(200);
        Console.Clear();
        PrintLine("\n\tCurrent ingridients:");
        Thread.Sleep(200);
        PrintLine($"\tCoffee: {Coffee} grams", ConsoleColor.DarkYellow);
        Thread.Sleep(200);
        PrintLine($"\tMilk: {Milk} ml");
        Thread.Sleep(200);
        PrintLine($"\tWater: {Water} ml", ConsoleColor.Cyan);
        string userChoice = Console.ReadLine()!;
        switch (userChoice)
        {
            default:
                _logger.LogWrite("IngridientsInfoMenu command completed");
                break;
        }

        Console.Clear();
    }


    private string CupsQuanityMenu()
    {
        _logger.LogWrite("CupsQuanityMenu command started");
        Thread.Sleep(200);
        Console.Clear();
        PrintLine("\n\tPlease, choose the work mode:");
        Thread.Sleep(200);
        PrintLine("\t1. Single cup");
        Thread.Sleep(200);
        PrintLine("\t2. Multiple cups");
        Thread.Sleep(200);
        PrintLine("\t3. Three in a row");
        _logger.LogWrite("CupsQuanityMenu command completed");
        return Console.ReadLine()!;
    }

    private int MultipleCupsMenu()
    {
        _logger.LogWrite("MultipleCupsMenu command started");
        Thread.Sleep(200);
        Console.Clear();
        PrintLine("\n\tHow many cups do you want to brew?");
        _logger.LogWrite("MultipleCupsMenu command completed");
        return int.Parse(Console.ReadLine()!);
    }

    private void RecipesMenu()
    {
        _logger.LogWrite("RecipesMenu command started");
        string userChoice;
        do
        {
            Console.Clear();
            PrintLine("\n\tWhich recipe do you want to see?");
            Thread.Sleep(200);
            PrintLine("\t1. Espresso", ConsoleColor.Green);
            Thread.Sleep(200);
            PrintLine("\t2. Cappuccino", ConsoleColor.Green);
            Thread.Sleep(200);
            PrintLine("\t3. Exit", ConsoleColor.Red);
            userChoice = Console.ReadLine()!;
            Console.Clear();
            switch (userChoice)
            {
                case "1":
                    Console.Clear();
                    PrintLine("\n\tEspresso");
                    Thread.Sleep(200);
                    PrintLine("\t1.Water - 75 ml", ConsoleColor.Cyan);
                    Thread.Sleep(200);
                    PrintLine("\t2.Coffee - 10 mg", ConsoleColor.DarkYellow);
                    Thread.Sleep(200);
                    PrintLine("\t3.Milk - 30 ml");
                    userChoice = Console.ReadLine()!;
                    break;
                case "2":
                    Console.Clear();
                    PrintLine("\n\tCappuccino");
                    Thread.Sleep(200);
                    PrintLine("\t1.Water - 30 ml", ConsoleColor.Cyan);
                    Thread.Sleep(200);
                    PrintLine("\t2.Coffee - 7 mg", ConsoleColor.DarkYellow);
                    Thread.Sleep(200);
                    PrintLine("\t3.Milk - 75 ml");
                    userChoice = Console.ReadLine()!;
                    break;
                default:
                    break;
            }
        } while (userChoice != "3");
        _logger.LogWrite("RecipesMenu command completed");
    }

    private void ProfilesMenu()
    {
        _logger.LogWrite("ProfilesMenu command started");
        Console.Clear();
        Thread.Sleep(200);
        PrintWarning($"\n\tYour current coffee profile: {UserProfile}");
        Thread.Sleep(200);
        PrintLine("\n\tChoose your coffee profile:");
        Thread.Sleep(200);
        PrintLine("\t1. Default", ConsoleColor.Green);
        Thread.Sleep(200);
        PrintLine("\t2. Custom", ConsoleColor.Green);
        Thread.Sleep(200);
        PrintLine("\t3. Exit", ConsoleColor.Red);
        string userChoice = Console.ReadLine()!;
        Console.Clear();
        switch (userChoice)
        {
            case "1":
                UserProfile = Profile.Default;
                _logger.LogWrite($"Coffee profile changed to Default");
                _logger.LogWrite("ProfilesMenu command completed");
                break;
            case "2":
                UserProfile = Profile.Custom;
                _logger.LogWrite($"Coffee profile changed to Custom");
                _logger.LogWrite("ProfilesMenu command completed");
                break;
            default:
                _logger.LogWrite("ProfilesMenu command completed");
                break;
        }
    }

    private void DefaultMenu(CoffeeType coffee, int cupsQanity)
    {
        _logger.LogWrite("DefaultMenu command started");
        int cupsReady = 0;
        ((ICoffeemachine)this).CheckEmptyIngredients();
        do
        {
            ((ICoffeemachine)this).CheckCleaning();
            Brewing(coffee, cupsQanity);
            cupsReady++;
        } while (cupsReady != cupsQanity);

        PrintLine($"\tHere your {cupsReady} cups of {coffee}!", ConsoleColor.Green);
        _logger.LogWrite("DefaultMenu command completed");
    }
}