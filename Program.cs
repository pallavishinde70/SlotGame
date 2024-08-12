using System; 
using System.Collections.Generic; //  to use generic collections like Dictionary

class SlotMachineGame 
{
    //  jagged array for the reels, each reel has an array of symbols
    private static readonly string[][] reels = new string[][]
    {
        new string[] { "sym2", "sym7", "sym7", "sym1", "sym1", "sym5", "sym1", "sym4", "sym5", "sym3", "sym2", "sym3", "sym8", "sym4", "sym5", "sym2", "sym8", "sym5", "sym7", "sym2" },
        new string[] { "sym1", "sym6", "sym7", "sym6", "sym5", "sym5", "sym8", "sym5", "sym5", "sym4", "sym7", "sym2", "sym5", "sym7", "sym1", "sym5", "sym6", "sym8", "sym7", "sym6", "sym3", "sym3", "sym6", "sym7", "sym3" },
        new string[] { "sym5", "sym2", "sym7", "sym8", "sym3", "sym2", "sym6", "sym2", "sym2", "sym5", "sym3", "sym5", "sym1", "sym6", "sym3", "sym2", "sym4", "sym1", "sym6", "sym8", "sym6", "sym3", "sym4", "sym4", "sym8", "sym1", "sym7", "sym6", "sym1", "sym6" },
        new string[] { "sym2", "sym6", "sym3", "sym6", "sym8", "sym8", "sym3", "sym6", "sym8", "sym1", "sym5", "sym1", "sym6", "sym3", "sym6", "sym7", "sym2", "sym5", "sym3", "sym6", "sym8", "sym4", "sym1", "sym5", "sym7" },
        new string[] { "sym7", "sym8", "sym2", "sym3", "sym4", "sym1", "sym3", "sym2", "sym2", "sym4", "sym4", "sym2", "sym6", "sym4", "sym1", "sym6", "sym1", "sym6", "sym4", "sym8" }
    };

    // dictionary for the paytable, mapping symbols to their payouts for 3, 4, and 5 matches
    private static readonly Dictionary<string, int[]> paytable = new Dictionary<string, int[]>()
    {
        { "sym1", new int[] { 1, 2, 3 } }, // Payouts for symbol 1
        { "sym2", new int[] { 1, 2, 3 } }, // Payouts for symbol 2
        { "sym3", new int[] { 1, 2, 5 } }, 
        { "sym4", new int[] { 2, 5, 10 } }, 
        { "sym5", new int[] { 5, 10, 15 } }, 
        { "sym6", new int[] { 5, 10, 15 } }, 
        { "sym7", new int[] { 5, 10, 20 } }, 
        { "sym8", new int[] { 10, 20, 50 } } 
    };

    static void Main() 
    {
        Random randomobj = new Random(); // Create a new Random object to generate random numbers
        int[] stopPositions = new int[5]; // Array to hold the stop positions for the 5 reels
        string[,] screen = new string[3, 5]; // 2D array to hold the symbols shown on the screen (3 rows and 5 columns)

        // Generate random stop positions for each reel
        for (int i = 0; i < 5; i++) 
        {
            stopPositions[i] = randomobj.Next(reels[i].Length); // Get a random stop position for the current reel
        }

        // Fill the screen based on stop positions
        for (int col = 0; col < 5; col++) // Loop through each column (reel)
        {
            int stopPos = stopPositions[col]; // stop position for the current reel
            for (int row = 0; row < 3; row++) // Loop through  3 rows
            {
                screen[row, col] = reels[col][(stopPos + row) % reels[col].Length]; // Fill the screen with symbols
                // The % operator wraps around if the stopPos + row exceeds the length of the reel
                //Stop Positions: 11, 3, 4, 2, 4
                //Screen:
                //sym3 sym6 sym3 sym3 sym4
            }
        }

        Console.WriteLine("Stop Positions: " + string.Join(", ", stopPositions)); // Print the stop positions

        Console.WriteLine("Screen:"); // Print the header for the screen
        for (int row = 0; row < 3; row++) // Loop through each row
        {
            for (int col = 0; col < 5; col++) // Loop through each column
            {
                Console.Write($"{screen[row, col]} "); // Print each symbol followed by a space
            }
            Console.WriteLine(); // Move to the next line after printing a row
        }

        CalculateWinnings(screen); // Call the CalculateWinnings method to evaluate wins
    }

    private static void CalculateWinnings(string[,] screen) // Method to calculate winnings based on the displayed screen
    {
        Dictionary<string, int> symbolCounts = new Dictionary<string, int>(); // Dictionary to count occurrences of symbols
        Dictionary<string, List<string>> winDetails = new Dictionary<string, List<string>>(); // Dictionary to store win details for each symbol

        // Count symbols in each column for winning calculations
        for (int col = 0; col < 5; col++) // Loop through each column
        {
            string symbol = screen[0, col]; // Get the symbol from the top row of the current column
            if (!symbolCounts.ContainsKey(symbol)) // Check if this symbol is already counted
            {
                symbolCounts[symbol] = 0; // Initialize the count for this symbol
            }
            symbolCounts[symbol]++; // Increment the count 
        }

        // Check for wins 
        int totalWinnings = 0; // Variable to keep track of total winnings
        foreach (var entry in symbolCounts) // Loop through each symbol and its count
        {
            string symbol = entry.Key; // Get the symbol
            int count = entry.Value; // Get the count of this symbol
            if (paytable.ContainsKey(symbol)) // Check if the symbol is in the paytable
            {
                if (count >= 3) // Only check for payouts if there are 3 or more matches
                {
                    int payout = paytable[symbol][count - 3]; // Get the payout for the number of matches
                    totalWinnings += payout; // Add the payout to total winnings

                    // Record winning details
                    if (!winDetails.ContainsKey(symbol)) // Check if we have already recorded details for this symbol
                    {
                        winDetails[symbol] = new List<string>(); // Initialize a new list for this symbol
                    }
                    winDetails[symbol].Add($"{symbol} x{count} = {payout}"); // Add the winning detail to the list
                }
            }
        }

        // Display winnings
        Console.WriteLine($"Total wins: {totalWinnings}"); // Print the total winnings
        foreach (var entry in winDetails) // Loop through each symbol that has winning details
        {
            string symbol = entry.Key; // Get the symbol
            foreach (var detail in entry.Value) // Loop through each winning detail for this symbol
            {
                Console.WriteLine($"- {detail}"); // Print the winning detail
            }
        }
    }
}
