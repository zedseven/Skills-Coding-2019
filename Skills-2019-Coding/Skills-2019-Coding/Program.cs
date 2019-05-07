//Program Name: Skills Ontario Competitor Management Software
//Revision History: Zacchary Dempsey-Plante 2019-05-07
//Purpose: This is the main file that handles user input and calls the functions to do the work.

using System;

namespace Skills_2019_Coding
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.LoadConfiguration();
            RuntimeStorage.LoadCompetitors();

            //Handle user input
            if (args.Length > 0)
            {
                //Add Competitor
                if (args[0] == "--add-competitor")
                {
                    if (args.Length == 8)
                    {
                        try
                        {
                            RuntimeStorage.AddCompetitor(args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                            Console.WriteLine("Competitor added successfully.");
                        }
                        catch (RuntimeStorage.InvalidDistrictException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                        catch (RuntimeStorage.InvalidCompetitionException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                        catch (RuntimeStorage.DuplicateCompetitorException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to add a new competitor. Please refer to --help for assistance.");
                    }
                }
                //Add Competition to List
                else if (args[0] == "--add-competition")
                {
                    if (args.Length == 2)
                    {
                        RuntimeStorage.AddCompetition(args[1]);
                        Console.WriteLine($"Competition with name '{args[1]}' added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to add a new competition. Please refer to --help for assistance.");
                    }
                }
                //Remove Competitor By ID
                else if (args[0] == "--remove-competitor")
                {
                    if (args.Length == 2)
                    {
                        try
                        {
                            RuntimeStorage.RemoveById(args[1]);
                            Console.WriteLine($"Competitor with ID #{args[1]} removed successfully.");
                        }
                        catch (RuntimeStorage.CompetitorNotFoundException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to remove a competitor by id. Please refer to --help for assistance.");
                    }
                }
                //Find Competitor By Last Name
                else if (args[0] == "--find-competitor")
                {
                    if (args.Length == 2)
                    {
                        try
                        {
                            RuntimeStorage.FindCompetitorByLastName(args[1]);
                        }
                        catch (RuntimeStorage.CompetitorNotFoundException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to find a competitor by last name. Please refer to --help for assistance.");
                    }
                }
                //Find Competition By Name
                else if (args[0] == "--find-competition")
                {
                    if (args.Length == 2)
                    {
                        try
                        {
                            RuntimeStorage.FindCompetition(args[1]);
                        }
                        catch (RuntimeStorage.CompetitionNotFoundException exception)
                        {
                            Console.WriteLine(exception.Message + " Please refer to --help for assistance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to find a competition by name. Please refer to --help for assistance.");
                    }
                }
                //List Competitors
                else if (args[0] == "--list-competitors")
                {
                    if (args.Length == 1)
                    {
                        RuntimeStorage.ListCompetitors();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to list competitors. Please refer to --help for assistance.");
                    }
                }
                //List Competitions
                else if (args[0] == "--list-competitions")
                {
                    if (args.Length == 1)
                    {
                        RuntimeStorage.ListCompetitions();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to list competitions. Please refer to --help for assistance.");
                    }
                }
                //Top X Competitors
                else if (args[0] == "--top-competitors")
                {
                    if (args.Length == 2)
                    {
                        RuntimeStorage.TopCompetitionCompetitors(args[1]);
                    }
                    else if (args.Length == 3)
                    {
                        try
                        {
                            RuntimeStorage.TopCompetitionCompetitors(args[1], int.Parse(args[2]));
                        }
                        catch(FormatException)
                        {
                            Console.WriteLine("The second parameter must be an integer value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to list top competitors. Please refer to --help for assistance.");
                    }
                }
                //Get Competition Metrics
                else if (args[0] == "--competition-metrics")
                {
                    if (args.Length == 1)
                    {
                        RuntimeStorage.CompetitionMetrics();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number of parameters to get competition metrics. Please refer to --help for assistance.");
                    }
                }
                //Help
                else if(args[0] == "--help")
                {
                    Console.WriteLine("-- Skills Ontario Competitor Management Software --");
                    Console.WriteLine("Function syntax examples:");
                    Console.WriteLine("          Add New Competitor: --add-competitor <first name> <last name> <email> <district> <birthday (dd/mm/yyyy)> <competition> <score>");
                    Console.WriteLine("         Add New Competition: --add-competition <name>");
                    Console.WriteLine("     Remove Competitor By ID: --remove-competitor <id>");
                    Console.WriteLine("Find Competitor By Last Name: --find-competitor <last name>");
                    Console.WriteLine("    Find Competition By Name: --find-competition <name>");
                    Console.WriteLine("            List Competitors: --list-competitors");
                    Console.WriteLine("           List Competitions: --list-competitions");
                    Console.WriteLine("        List Top Competitors: --top-competitors <competition> <(optional) number of top competitors>");
                    Console.WriteLine("     Get Competition Metrics: --competition-metrics");
                }
                else
                {
                    Console.WriteLine("That operation does not exist. Please refer to --help for assistance.");
                }
            }
            else
            {
                Console.WriteLine("Please provide an operation. Refer to --help for assistance.");
            }

            RuntimeStorage.SaveCompetitors();
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
