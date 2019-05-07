//Program Name: Skills Ontario Competitor Management Software
//Revision History: Zacchary Dempsey-Plante 2019-05-07
//Purpose: Stores all confiiguration data in one global location for easy access throughout the program, and provides a method for loading what it needs.

using System.IO;

namespace Skills_2019_Coding
{
    public static class Configuration
    {
        public static readonly string districtsFilePath = "districts.txt";
        public static readonly string competitionsFilePath = "competitions.txt";
        public static readonly string competitorsFilePath = "competitors.csv";
        public static string[] allowedDistricts;
        public static string[] allowedCompetitions;

        public static void LoadConfiguration()
        {
            allowedDistricts = File.ReadAllLines(districtsFilePath);
            allowedCompetitions = File.ReadAllLines(competitionsFilePath);
        }
    }
}
