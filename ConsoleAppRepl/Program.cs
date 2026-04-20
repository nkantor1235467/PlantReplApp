//should be using domain name space
using System.Runtime.CompilerServices;
using System.Text.Json;
using ConsoleTables;
using Domain;

namespace ConsoleAppRepl;

class Program
{
    static async Task Main(string[] args)
    {

        bool running = true;
        
        //creates a new list
        var plantList = new List<Plant>();

        //creates an instance of a plant object
        Plant p = new Plant()
        {
            Genus = "Mertensia",
            Species = "virginica",
            HowMany = 57,
        };

        //adds the plant that was just created to the list of plants
        plantList.Add(p);

        //checks if there is already a file with the plantList, if there is, it reads the file and creates the plant list based on it
        string path = Directory.GetCurrentDirectory() + @"/MyPlantListFile.txt";

                        if (!File.Exists(path))
                        {
                            using(StreamWriter sw = File.CreateText(path))
                            {
                                string jsonString = JsonSerializer.Serialize(plantList);
                                await sw.WriteLineAsync(jsonString);
                                
                            }
                        } else
                        {
                                //write code here that reads the file and then saves the content of file to plantlist
                                string plantlistjson = await File.ReadAllTextAsync(path);
                                plantList = JsonSerializer.Deserialize<List<Plant>>(plantlistjson)!;
                        }

        //while loop menu that allows the user to choose from multiple options
        while (running)
        {
            Console.WriteLine("Please enter command or type help for help or type exit to exit");
            string userInput = Console.ReadLine()!.ToLower();

            switch (userInput){
                case "help":
                    Console.WriteLine("Options: AddPlant, ListPlants, SearchPlant, SearchPlantLINQ, Exit");
                    break;

                case "exit":
                    Console.WriteLine("Goodbye!");
                    running = false;

                    using(var sw = File.CreateText(path))
                    {
                        string json = JsonSerializer.Serialize(plantList);
                        await sw.WriteLineAsync(json);
                    }
                    break;

                case "addplant":
                    Console.WriteLine("Now adding a plant to the database.");
                    Console.WriteLine("Please enter plant Genus.");
                    var plantGenus = Console.ReadLine();
                    Console.WriteLine("Please enter plant Species.");
                    //the ToLower command makes the species name lowercase as it should be
                    var plantSpecies = Console.ReadLine()!.ToLower();
                    Console.WriteLine("Please enter the number of plants in the study area.");
                    var plantHowMany = int.Parse(Console.ReadLine()!);
                    var plant = new Plant()
                    {
                        Genus = plantGenus!,
                        Species = plantSpecies!,
                        HowMany = plantHowMany!,
                    };
                    plantList.Add(plant);
                    Console.WriteLine("Plant added.");
                    break;

                case "listplants":
                    Console.WriteLine("Now listing all plants in the database.");
                    for( int i = 0; i < plantList.Count; i++)
                    {
                        Console.WriteLine($"ID: {plantList[i].PlantID}");
                        Console.WriteLine("Genus: " + plantList[i].Genus);
                        Console.WriteLine("Species: " + plantList[i].Species);
                        Console.WriteLine("There are " + plantList[i].HowMany + " members of this species in the study area.");
                    }
                    
                    //console table
                    var table = new ConsoleTable("ID", "Genus", "Species", "Plants in study area");
                    foreach(Plant pl in plantList)
                    {
                    table.AddRow(pl.PlantID, pl.Genus, pl.Species, pl.HowMany);
                    }
                    table.Write();

                    break;

                case "searchplant":
                    Console.WriteLine("Enter the plant genus name:");
                    //save genus name as a variable
                    var plantGenustoSearch = Console.ReadLine();
                    Console.WriteLine("Enter the plant species name:");
                    //save species name as a variable
                    var plantSpeciestoSearch = Console.ReadLine()!.ToLower();
                    
                    //for loop over plantlist
                        for(int i=0; i < plantList.Count; i++)
                        {
                            //if the plant genus = plant genus to search and plant species = plant species to search run the following code
                            if (plantGenustoSearch == plantList[i].Genus && plantSpeciestoSearch == plantList[i].Species){
                            Console.WriteLine("Plant found! Here is info on the plant.");
                            Console.WriteLine($"ID: {plantList[i].PlantID}");
                            Console.WriteLine("Genus: " + plantList[i].Genus);
                            Console.WriteLine("Species: " + plantList[i].Species);
                            Console.WriteLine("There are " + plantList[i].HowMany + " members of this species in the study area.");
                            bool challenge4 = true;
                            while (challenge4)
                            {
                                Console.WriteLine("If you wish to update this plant record, type update. If you wish to delete this plant record, type delete. If you wish to return to the main menu, type return.");
                                string userInputchallenge4 = Console.ReadLine()!;

                                switch (userInputchallenge4)
                                {
                                    case "return":
                                    Console.WriteLine("Returning to main menu");
                                    challenge4 = false;
                                    break;

                                    case "update":
                                    //code to update a plant
                                    Console.WriteLine("How many additional plants have you found in the study area?");
                                    var newplantnumber = int.Parse(Console.ReadLine()!);
                                    plantList[i].HowMany = newplantnumber + plantList[i].HowMany;
                                    Console.WriteLine(plantList[i].Genus + " " + plantList[i].Species + " updated.");
                                    break;

                                    case "delete":
                                    //code to delete a plant from the database
                                    var DeletedGenus = plantList[i].Genus;
                                    var DeletedSpecies = plantList[i].Species;
                                    plantList.RemoveAt(i);
                                    Console.WriteLine(DeletedGenus + " " + DeletedSpecies + " deleted.");
                                    Console.WriteLine("Returning to main menu");
                                    challenge4 = false;
                                    break;
                                }
                            }
                            }       else
                            {
                                Console.WriteLine("...");
                            }
                        }
                    break;

                    case "searchplantlinq":
                    
                        Console.WriteLine("This code lets you search for a plant based on the number of plants");
                        Console.WriteLine("Enter the number of plants in the study area:");
                        int HowMany = int.Parse(Console.ReadLine()!);

                        IEnumerable<Plant> queriedPlants = 
                            from queriedplant in plantList
                            where queriedplant.HowMany == HowMany
                            select queriedplant;

                        if (!queriedPlants.Any())
                        {
                            Console.WriteLine("No plants found");
                            break;
                        }
                        foreach (var queriedplant in queriedPlants)
                        {
                            Console.WriteLine("ID: " + queriedplant.PlantID + " Genus: " + queriedplant.Genus + " Species: " + queriedplant.Species);
                        }
                    
                    break;
            }
        }
    }
}
