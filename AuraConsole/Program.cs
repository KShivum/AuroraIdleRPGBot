class Program
{
    
    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(@"Choose an option");
            Console.WriteLine("1. Create a Character");
            
            
            
            
            Console.WriteLine("e: Exit");
            string? result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    CreateCharacter();
                    break;
                
                case "e":
                    return;
            }
        }
    }

    private static void CreateCharacter()
    {
        Console.WriteLine("What is your name?");
        string? result = Console.ReadLine();

        if (String.IsNullOrWhiteSpace(result))
        {
            Console.WriteLine("Invalid Name");
            return;
        }
        
        
        
    }
}