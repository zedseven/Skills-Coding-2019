//Program Name: Skills Ontario Competitor Management Software
//Revision History: Zacchary Dempsey-Plante 2019-05-07
//Purpose: Provides the container class for each competitor's information as well as one for manipulating competition-wide statistics.

using System;
using System.Collections.Generic;

namespace Skills_2019_Coding
{
    public class Competitor
    {
        public string id;
        public string firstName;
        public string lastName;
        public string email;
        public string district;
        public DateTime birthday;
        public string competition;
        public double score;

        public Competitor(string newId, string newFirstName, string newLastName, string newEmail, string newDistrict, DateTime newBirthday, string newCompetition, double newScore)
        {
            id = newId;
            firstName = newFirstName;
            lastName = newLastName;
            email = newEmail;
            district = newDistrict;
            birthday = newBirthday;
            competition = newCompetition;
            score = newScore;
        }
    }
    public class Competition
    {
        public string name;
        public List<double> scores = new List<double>();

        public Competition(string newName)
        {
            name = newName;
        }

        public double GetTotalScore()
        {
            double total = 0;
            foreach (double score in scores)
            {
                total += score;
            }
            return total;
        }
        public double GetAverageScore()
        {
            return GetTotalScore() / scores.Count;
        }
    }
}
