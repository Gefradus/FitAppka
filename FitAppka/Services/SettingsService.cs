using System;

namespace FitAppka.Controllers{

public class SettingsServices
{
    public double ActivityLevel(short? activity)
    {
        if (activity == 1) { return 1.2; }
        if (activity == 2) { return 1.3; }
        if (activity == 3) { return 1.5; }
        if (activity == 4) { return 1.7; }
        if (activity == 5) { return 1.9; }
        return 1.5;
    }



    public int CountCalorieTarget(int demand, short? changeGoal, double pace)
        {
            if (changeGoal == 1)
            {
                int kcalTarget = (int)(demand - (pace * 1100));

                if (kcalTarget <= 1000)
                {
                    return 1000;
                }
                else
                {
                    return kcalTarget;
                }
            }

            if (changeGoal == 3)
            {
                int kcalTarget = (int)(demand + (pace * 1100));

                if (kcalTarget <= 1000)
                {
                    return 1000;
                }
                else
                {
                    return kcalTarget;
                }
            }

            if (demand < 1000)
            {
                return 1000;
            }
            else
            {
                return (int)demand;
            }
        }


        public int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity)
        {
            int BMR;
            int age = CountAge((DateTime)birthDate);

            if ((bool)sex)
            {
                BMR = (int)(66 + (13.7 * weight) + (5 * growth) - (6.76 * age));        //
            }                                                                           //
            else                                                                        // Harris-Benedict's method
            {                                                                           //
                BMR = (int)(655 + (9.6 * weight) + (1.85 * growth) - (4.7 * age));      //
            }

            return (int)(BMR * activity);
        }

        public int CountProteinTarget(double? weight, int kcalTarget, short? activity)
        {
            int proteins;
            if (activity < 3)
            {
                proteins = (int)weight;
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.3 * kcalTarget);
                }
            }
            else if (activity == 3 || activity == 4)
            {
                proteins = (int)(1.5 * weight);
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.5 * kcalTarget);
                }
            }
            else
            {
                proteins = (int)(2 * weight);
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.65 * kcalTarget);
                }
            }
        }

        public int CountFatTarget(int kcalTarget)
        {
            return kcalTarget / 36;    //25% of demand from fats (1g of fat has 9kcal)
        }

        public int CountCarbsTarget(int kcalTarget, int proteinsTarget, int fatsTarget)
        {
            return (kcalTarget - ((proteinsTarget * 4) + (fatsTarget * 9))) / 4;
        }


        public int CountAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
}
}