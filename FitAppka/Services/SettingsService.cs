using System;

namespace FitAppka.Controllers{

public class SettingsServices
{
    public double PoziomAktywnosci(short? aktywnosc)
    {
        if (aktywnosc == 1) { return 1.2; }
        if (aktywnosc == 2) { return 1.3; }
        if (aktywnosc == 3) { return 1.5; }
        if (aktywnosc == 4) { return 1.7; }
        if (aktywnosc == 5) { return 1.9; }
        return 1.5;
    }



    public int CountCalorieTarget(int zapotrzebowanie, short? celZmian, double tempo)
        {
            //-----------------------------schudnięcie--------------------------
            if (celZmian == 1)
            {
                int celKcal = (int)(zapotrzebowanie - (tempo * 1100));

                if (celKcal <= 1000)
                {
                    return 1000;
                }
                else
                {
                    return celKcal;
                }
            }

            //-----------------------------przytycie--------------------------
            if (celZmian == 3)
            {
                int celKcal = (int)(zapotrzebowanie + (tempo * 1100));

                if (celKcal <= 1000)
                {
                    return 1000;
                }
                else
                {
                    return celKcal;
                }
            }

            //-----------------------------utrzymanie--------------------------
            if (zapotrzebowanie < 1000)
            {
                return 1000;
            }
            else
            {
                return (int)zapotrzebowanie;
            }
        }


        public int CountCaloricDemand(bool plec, DateTime? dataUrodzenia, int? wzrost, double waga, double aktywnosc)
        {
            int BMR;
            int wiek = ObliczWiek((DateTime)dataUrodzenia);

            if (plec)
            {
                BMR = (int)(66 + (13.7 * waga) + (5 * wzrost) - (6.76 * wiek));         //
            }                                                                           //
            else                                                                        // METODA Harrisa-Benedicta
            {                                                                           //
                BMR = (int)(655 + (9.6 * waga) + (1.85 * wzrost) - (4.7 * wiek));       //
            }

            return (int)(BMR * aktywnosc);

        }

        public int CountProteinTarget(double? waga, int celKalorii, short? aktywnosc)
        {
            int bialko;
            if (aktywnosc < 3)
            {
                bialko = (int)waga;
                if (bialko < celKalorii)
                {
                    return bialko;
                }
                else
                {
                    return (int)(0.3 * celKalorii);
                }
            }
            else if (aktywnosc == 3 || aktywnosc == 4)
            {
                bialko = (int)(1.5 * waga);
                if (bialko < celKalorii)
                {
                    return bialko;
                }
                else
                {
                    return (int)(0.5 * celKalorii);
                }
            }
            else
            {
                bialko = (int)(2 * waga);
                if (bialko < celKalorii)
                {
                    return bialko;
                }
                else
                {
                    return (int)(0.65 * celKalorii);
                }
            }
        }

        public int CountFatTarget(int celKalorii)
        {
            return celKalorii / 36;    //25% zapotrzebowania z tłuszczu (każdy gram tłuszczu ma 9kcal)
        }

        public int CountCarbsTarget(int celKalorie, int celBialko, int celTluszcze)
        {
            return (celKalorie - ((celBialko * 4) + (celTluszcze * 9))) / 4;
        }


        public int ObliczWiek(DateTime dataUrodzenia)
        {
            int age = DateTime.Today.Year - dataUrodzenia.Year;
            if (dataUrodzenia.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
}
}