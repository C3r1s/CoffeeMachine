using Exercise2;
string userChoice = null;
Coffeemachine coffeemachine = new Coffeemachine(1000, 500, 100);
coffeemachine.Power();
if (!coffeemachine.IsPowered)
{
    Environment.Exit(0);
}
coffeemachine.StartMenu(userChoice);



