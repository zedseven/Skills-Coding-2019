//Program Name: Skills Ontario Competitor Management Software
//Revision History: Zacchary Dempsey-Plante 2019-05-07
//Purpose: Does most of the actual work and stores all competitor information in one global location.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skills_2019_Coding
{
    public static class RuntimeStorage
    {
        public static List<Competitor> competitors = new List<Competitor>();
        //This could theoretically be replaced by using competitors.Count - 1, but that would break if a competitor had an ID higher than the number of competitors.
        public static int highestId;

        //Runtime Exception Definitions
        public class InvalidDistrictException : Exception
        {
            public InvalidDistrictException(string message) : base(message) { }
        }
        public class InvalidCompetitionException : Exception
        {
            public InvalidCompetitionException(string message) : base(message) { }
        }
        public class DuplicateCompetitorException : Exception
        {
            public DuplicateCompetitorException(string message) : base(message) { }
        }
        public class CompetitorNotFoundException : Exception
        {
            public CompetitorNotFoundException(string message) : base(message) { }
        }
        public class CompetitionNotFoundException : Exception
        {
            public CompetitionNotFoundException(string message) : base(message) { }
        }

        //File IO Methods
        public static void SaveCompetitors()
        {
            //Build the CSV file to save out
            string fileOutputBuffer = "ID,First Name,Last Name,Email,District,Birthday,Competition,Score" + Environment.NewLine;
            foreach (Competitor competitor in competitors)
            {
                fileOutputBuffer += competitor.id + ',';
                fileOutputBuffer += competitor.firstName + ',';
                fileOutputBuffer += competitor.lastName + ',';
                fileOutputBuffer += competitor.email + ',';
                fileOutputBuffer += competitor.district + ',';
                fileOutputBuffer += competitor.birthday.ToString("dd/MM/yyyy") + ',';
                fileOutputBuffer += competitor.competition + ',';
                fileOutputBuffer += competitor.score * 100.0 + Environment.NewLine;
            }
            File.WriteAllText(Configuration.competitorsFilePath, fileOutputBuffer);
        }
        public static void LoadCompetitors()
        {
            if (File.Exists(Configuration.competitorsFilePath))
            {
                string[] fileLines = File.ReadAllLines(Configuration.competitorsFilePath);
                bool firstLine = true;
                foreach (string fileLine in fileLines)
                {
                    //Skip the first entry in the csv, as this is simply a header for external parsing
                    if (!firstLine)
                    {
                        string[] rawCompetitorValues = fileLine.Split(',');
                        Competitor competitor = new Competitor(rawCompetitorValues[0],
                            rawCompetitorValues[1],
                            rawCompetitorValues[2],
                            rawCompetitorValues[3],
                            rawCompetitorValues[4],
                            DateTime.ParseExact(rawCompetitorValues[5], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            rawCompetitorValues[6],
                            double.Parse(rawCompetitorValues[7]) / 100.0);
                        competitors.Add(competitor);
                        int newHighestId = int.Parse(competitor.id);
                        if (newHighestId > highestId)
                            highestId = newHighestId;
                    }
                    firstLine = false;
                }
            }
        }
        public static void AddCompetition(string name)
        {
            File.AppendAllText(Configuration.competitionsFilePath, name + Environment.NewLine);
        }

        //General User Operation Methods
        public static void AddCompetitor(string newFirstName, string newLastName, string newEmail, string newDistrict, string newBirthday, string newCompetition, string newScore)
        {
            //Verify that the competitor has not already been added
            if (!competitors.Any(newCompetitor => newCompetitor.firstName == newFirstName && newCompetitor.lastName == newLastName))
            {
                //Verify that the specified district is valid according to the file
                if (Configuration.allowedDistricts.Contains(newDistrict))
                {
                    //Verify that the specified competition is valid according to the file
                    if (Configuration.allowedCompetitions.Contains(newCompetition))
                    {
                        highestId++;
                        Competitor competitor = new Competitor(highestId.ToString().PadLeft(8, '0'),
                            newFirstName,
                            newLastName,
                            newEmail,
                            newDistrict,
                            DateTime.ParseExact(newBirthday, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            newCompetition,
                            double.Parse(newScore) / 100.0);
                        competitors.Add(competitor);
                    }
                    else
                    {
                        throw new InvalidCompetitionException($"The competition '{newCompetition}' is not in the list of competitions.");
                    }
                }
                else
                {
                    throw new InvalidDistrictException($"The district '{newDistrict}' is not in the list of permitted districts.");
                }
            }
            else
            {
                throw new DuplicateCompetitorException($"The competitor '{newFirstName} {newLastName}' is already in the list of competitors.");
            }
        }
        public static void RemoveById(string id)
        {
            int competitorIndex = competitors.FindIndex(testCompetitor => testCompetitor.id == id);
            if (competitorIndex > -1)
            {
                competitors.RemoveAt(competitorIndex);
            }
            else
            {
                throw new CompetitorNotFoundException($"Competitor with ID '{id}' could not be found.");
            }
        }
        public static void RemoveById(int id)
        {
            RemoveById(id.ToString().PadLeft(8, '0'));
        }
        public static void FindCompetitorByLastName(string lastName)
        {
            Competitor competitor = competitors.Where(testCompetitor => testCompetitor.lastName == lastName).FirstOrDefault();
            if (competitor != null)
            {
                Console.WriteLine($"Competitor ID: #{competitor.id}:");
                Console.WriteLine($"Name: {competitor.lastName}, {competitor.firstName}");
                Console.WriteLine($"Email: {competitor.email}");
                Console.WriteLine($"District: {competitor.district}");
                Console.WriteLine($"Competition: {competitor.competition}");
                Console.WriteLine($"Score: {competitor.score * 100.0}%");
            }
            else
            {
                throw new CompetitorNotFoundException($"Competitor with last name '{lastName}' could not be found.");
            }
        }
        public static void FindCompetition(string name)
        {
            Competition competition = GenerateCompetitionList().Where(testCompetition => testCompetition.name == name).FirstOrDefault();
            if (competition != null)
            {
                PrintCompetition(competition);
            }
            else
            {
                throw new CompetitionNotFoundException($"Competition with name '{name}' could not be found.");
            }
        }
        public static void ListCompetitors()
        {
            List<Competitor> sortedCompetitors = competitors.OrderBy(sortCompetitor => sortCompetitor.lastName)
                .ThenBy(sortCompetitor => sortCompetitor.firstName)
                .ToList();
            PrintCompetitorList(sortedCompetitors);
        }
        public static void TopCompetitionCompetitors(string competition, int topCount = 3)
        {
            List<Competitor> sortedCompetitors = competitors.Where(testCompetitor => testCompetitor.competition == competition)
                .OrderBy(sortCompetitor => sortCompetitor.score)
                .Take(topCount)
                .ToList();
            PrintCompetitorList(sortedCompetitors);
        }
        public static void ListCompetitions()
        {
            PrintCompetitionList(GenerateCompetitionList());
        }
        public static void CompetitionMetrics()
        {
            List<Competition> competitions = GenerateCompetitionList();
            double totalScore = 0;
            int totalContestants = 0;
            foreach (Competition competition in competitions)
            {
                totalContestants += competition.scores.Count;
                totalScore += competition.GetTotalScore();
            }
            Console.WriteLine($"Average Number of Competitors per Competition: {((double)totalContestants / competitions.Count).ToString("0.0")}");
            Console.WriteLine($"Average Score: {(totalScore / totalContestants * 100.0).ToString("00.0")}%");
        }

        //Utility Methods
        private static List<Competition> GenerateCompetitionList()
        {
            List<Competition> competitions = new List<Competition>();
            foreach (Competitor competitor in competitors)
            {
                Competition competition = competitions.Where(testCompetition => testCompetition.name == competitor.competition).FirstOrDefault();
                if (competition == null)
                {
                    competition = new Competition(competitor.competition);
                    competitions.Add(competition);
                }
                competition.scores.Add(competitor.score);
            }
            return competitions;
        }
        private static void PrintCompetitor(Competitor competitor)
        {
            Console.WriteLine($"#{competitor.id}: {competitor.firstName} {competitor.lastName} ({competitor.email}) of {competitor.district} in {competitor.competition} with a score of {competitor.score * 100.0}%");
        }
        private static void PrintCompetitorList(List<Competitor> competitors)
        {
            foreach (Competitor competitor in competitors)
            {
                PrintCompetitor(competitor);
            }
        }
        private static void PrintCompetition(Competition competition)
        {
            Console.WriteLine($"{competition.name}: {competition.scores.Count} competitor{(competition.scores.Count != 1 ? "s" : "")}, with an average score of {competition.GetAverageScore() * 100.0}%");
        }
        private static void PrintCompetitionList(List<Competition> competitions)
        {
            foreach (Competition competition in competitions)
            {
                PrintCompetition(competition);
            }
        }
    }
}
