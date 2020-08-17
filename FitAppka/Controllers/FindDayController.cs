using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class FindDayController : Controller
    {
        private readonly FitAppContext _context;
        public FindDayController(FitAppContext context)
        {
            _context = context;
        }

        public IActionResult FindDay(int produktID, int od, int dO, int typSzukania)
        {
            var klientID = _context.Klient.Where(k => k.Login.ToLower() == User.Identity.Name.ToLower()).Select(k => k.KlientId).FirstOrDefault();
            
            var listaDni = ZnajdzDni(od, dO, produktID, typSzukania, klientID);

            UtworzListeProduktow(produktID);

            ViewData["typSzukania"] = typSzukania;
            ViewData["do"] = dO;
            ViewData["od"] = od;  
            ViewData["dniID"] = listaDni;
            ViewData["czyZnaleziono"] = CzyZnalezionoDzien(listaDni);
            ViewData["klientID"] = klientID;

            ViewData["listaMl"] = WodaWDniach(listaDni);                        //gdy wyszukujemy po wodzie
            ViewData["listaKcal"] = KalorieWDniach(listaDni);                   //gdy wyszukujemy po kaloriach
            ViewData["produktID"] = produktID;                                  //gdy wyszukujemy po produktach
            ViewData["listaGramatur"] = GramaturaWDniach(listaDni, produktID);  //gdy wyszukujemy po produktach

            return View(_context.Dzien.ToList());
        }

        private void UtworzListeProduktow(int produktID)
        {
            List<SelectListItem> listaProduktow = new List<SelectListItem>();
            foreach (var item in _context.Produkt)
            {
                if (item.ProduktId == produktID)
                {
                    listaProduktow.Add(new SelectListItem() { Text = item.NazwaProduktu + ", " + (int)item.Kalorie + "kcal", Value = item.ProduktId + "", Selected = true });
                }
                else
                {
                    listaProduktow.Add(new SelectListItem() { Text = item.NazwaProduktu + ", " + (int)item.Kalorie + "kcal", Value = item.ProduktId + "" });
                }
            }

            ViewData["allProducts"] = listaProduktow; 
        }


        private List<int?> WodaWDniach(List<int> listaDni)
        {
            List<int?> listaWody = new List<int?>();

            foreach (var dzienID in listaDni)
            {
                int? wypitaWodaSuma = 0;
                foreach (var dzien in _context.Dzien.ToList())
                {
                    if (dzienID == dzien.DzienId)
                    {
                        wypitaWodaSuma += dzien.WypitaWoda;
                    }
                }
                listaWody.Add(wypitaWodaSuma);             
            }

            return listaWody;
        }

        private List<int> GramaturaWDniach(List<int> listaDni, int produktID)
        {
            List<int> listaGramatur = new List<int>();

            foreach (var dzienID in listaDni)
            {
                int gramaturaLaczna = 0;
                foreach (var dzien in _context.Dzien.ToList())
                {
                    if (dzienID == dzien.DzienId)
                    { 
                        foreach (var posilek in _context.Posilek.ToList())
                        {
                            if (posilek.DzienId == dzienID)
                            {          
                                if (posilek.ProduktId == produktID)
                                {
                                    gramaturaLaczna += (int)posilek.Gramatura;
                                }
                            }
                        }
                    }
                }
                listaGramatur.Add(gramaturaLaczna);
            }
            return listaGramatur;
        }

        private List<decimal> KalorieWDniach(List<int> listaDni)
        {
            List<decimal> listaKalorii = new List<decimal>();
            foreach (var dzienID in listaDni)
            {
                double? sumaKalorii = 0;
                foreach (var dzien in _context.Dzien.ToList())
                {
                    if (dzienID == dzien.DzienId)
                    {
                        foreach (var posilek in _context.Posilek.ToList())
                        {
                            if (posilek.DzienId == dzienID)
                            {
                                sumaKalorii += posilek.Kalorie;
                            }
                        }
                    }
                }
                listaKalorii.Add(Math.Round((decimal)sumaKalorii, 0, MidpointRounding.AwayFromZero));
            }
            return listaKalorii;
        }

        private bool CzyZnalezionoDzien(List<int> listaDni)
        {
            return listaDni.Count > 0;
        }


        private List<int> ZnajdzDni(int od, int dO, int produktID, int typSzukania, int klientID){
            if (typSzukania == 1)
            {
                return ZnajdzDniPoProdukcie(od, dO, produktID, klientID);
            }
            if (typSzukania == 2)
            {
                return ZnajdzDniPoKaloriach(od, dO, klientID);
            }
            if (typSzukania == 3)
            {
                return ZnajdzDniPoWodzie(od, dO, klientID);
            }
            
            return new List<int>();  
        }


        private List<int> ZnajdzDniPoWodzie(int od, int dO, int klientID)
        {
            List<int> listaDni = new List<int>();

            foreach (var item in _context.Dzien.Where(d => d.KlientId == klientID).ToList())
            {
                if(item.WypitaWoda >= od && (item.WypitaWoda <= dO || dO == 0))
                {
                    listaDni.Add(item.DzienId);
                }
            }

            return listaDni;
        }


        private List<int> ZnajdzDniPoKaloriach(int od, int dO, int klientID)
        {
            List<Posilek> posilki = _context.Posilek.ToList();  //wszystkie posilki
            List<int> listaKaloriiWDniach = new List<int>();
            List<int> listaIDDni = new List<int>();
            List<int> listaDni = new List<int>();

            foreach (var item in _context.Dzien.Where(d => d.KlientId == klientID).ToList())
            {
                double? kalorieZDnia = 0;
                foreach (var posilek in posilki)
                {
                    if (posilek.DzienId == item.DzienId)
                    {
                        kalorieZDnia += posilek.Kalorie;
                    }
                }
                listaKaloriiWDniach.Add((int)kalorieZDnia);
                listaIDDni.Add(item.DzienId);
            }

            int ktoryDzien = 0;
            foreach(var kalorie in listaKaloriiWDniach)
            {
                if (kalorie >= od && (kalorie <= dO || dO == 0))
                {
                    listaDni.Add(listaIDDni[ktoryDzien]);
                }

                ktoryDzien++;
            }

            return listaDni;
        }

        private List<int> ZnajdzDniPoProdukcie(int od, int dO, int produktID, int klientID)
        {
            List<Posilek> posilki = _context.Posilek.Where(p => p.ProduktId == produktID).ToList();
            List<int> listaDni = new List<int>();

            foreach(var item in _context.Dzien.Where(d => d.KlientId == klientID).ToList())
            {
                //stworzenie listy posilkow w danym dniu
                List<Posilek> listaPosilkow = posilki.Where(p => p.DzienId == item.DzienId).ToList();
                
                int gramaturaLaczna = 0;
                foreach (var posilek in listaPosilkow)
                {
                    gramaturaLaczna += (int)posilek.Gramatura;
                }

                if (gramaturaLaczna >= od && (gramaturaLaczna <= dO || dO == 0) && gramaturaLaczna != 0)
                {
                    listaDni.Add(item.DzienId);
                }
            }

            return listaDni;
        }
    }
}