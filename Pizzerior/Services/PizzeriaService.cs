#nullable disable 

using Newtonsoft.Json;

using Pizzerior.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Pizzerior.Services
{
    public class PizzeriaService : IPizzeriaService
    {
        ILoggerService _loggerService;

        readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Culture = new CultureInfo("sv-SE"),
            DateFormatString = "yyyy-MM-dd"

        };

        public PizzeriaService()
        {
            _loggerService= new LoggerService();
        }

        public List<Pizzeria> GetAll()
        {
            try
            {
                if (File.Exists(dataPath()))
                {
                    string json = File.ReadAllText(dataPath());
                    return DeserializeFromJson<List<Pizzeria>>(json);
                }
                else
                {
                    List<Pizzeria> pizzerior= new List<Pizzeria>();
                    Pizzeria newPizzeria = new Pizzeria();
                    newPizzeria.PizzeriaID = System.Guid.NewGuid().ToString();
                    newPizzeria.Namn = "Exempelpizzeria";
                    newPizzeria.Adress = "Exempelgatan 4";
                    newPizzeria.PostNr = "123 45";
                    newPizzeria.IntroBild = "Missing-image.png";
                    newPizzeria.PostOrt = "Exempelorten";
                    pizzerior.Add(newPizzeria); 
                    string jsonTxt = SerializeToJson<List<Pizzeria>>(pizzerior);
                    SaveJsonAsFile(jsonTxt, dataPath());
                    return pizzerior;


                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        public bool IsPizzeriaUnique(String pizzeriaNamn, string pizzeriaId)
        {
            List<Pizzeria> list = GetAll();
            if (list != null)
            {
               Pizzeria pz = list.Where(x => x.Namn == pizzeriaNamn || x.PizzeriaID == pizzeriaId).FirstOrDefault();
               if(pz!=null) { return false; } else { return true; }               
            } else { return true; }

        }


        public ObservableCollection<Pizzeria> GetAllCollection()
        {
            try
            {
                ObservableCollection<Pizzeria> tmpColList = new ObservableCollection<Pizzeria>();
                List<Pizzeria> tmpList = GetAll();
                if (tmpList != null && tmpList.Count > 0)
                {
                    tmpList.ForEach(x => tmpColList.Add(x));
                }

                foreach (Pizzeria piz in tmpColList)
                {
                    if (piz.Omdomen != null && piz.Omdomen.Count > 0)
                    {
                        int sumBetyg = piz.Omdomen.Sum(x => x.Betyg);
                        int sumDeltagare = piz.Omdomen.Count();
                        if (sumBetyg != 0 && sumBetyg != 0)
                        {
                            piz.Betyg = (sumBetyg / sumDeltagare);
                        }
                    }
                }
                return tmpColList;
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
 
       }

 

        public Pizzeria Get(string id)
        {
            try
            {
                string json = File.ReadAllText(dataPath());
                return DeserializeFromJson<List<Pizzeria>>(json).Where(x => x.PizzeriaID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public bool Create(Pizzeria pizza)
        {
            try
            {
                List<Pizzeria> tmpList = new List<Pizzeria>();
                 tmpList = GetAll().OrderByDescending(x => x.Modifierad).ToList();
                if (tmpList == null)
                {
                     tmpList= new List<Pizzeria>();            
                }
                tmpList.Add(pizza);  
                var json = SerializeToJson(tmpList);
                if (!string.IsNullOrEmpty(json))
                {
                    File.Delete(dataPath());
                    SaveJsonAsFile(json, dataPath());
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }


        public Pizzeria Update(Pizzeria pizzeria)
        {
            try
            {
                List<Pizzeria> tmpList = GetAll();
                if (tmpList != null)
                {
                    Pizzeria p = tmpList.Where(x => x.PizzeriaID == pizzeria.PizzeriaID).FirstOrDefault();
                    p.Namn = pizzeria.Namn;
                    p.Adress = pizzeria.Adress;
                    p.PostNr = pizzeria.PostNr;
                    p.IntroBild = pizzeria.IntroBild;
                    p.Modifierad = DateTime.Now;
                    p.PostOrt = pizzeria.PostOrt;
                    if (p.Omdomen == null)
                    {
                        p.Omdomen = new List<Omdome>();
                    }
                    else
                    {
                        p.Omdomen = pizzeria.Omdomen;
                    }


                }
                else
                {
                    tmpList = new List<Pizzeria>();
                    tmpList.Add(pizzeria);
                }

                var json = SerializeToJson(tmpList);
                if (!string.IsNullOrEmpty(json))
                {
                    //File.Delete(dataPath());
                    SaveJsonAsFile(json, dataPath());
                    return pizzeria;
                }
                else { return null; }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null; 
            }


        }

        public bool Delete(string id)
        {
            try
            {
                List<Pizzeria> tmpList = GetAll().OrderByDescending(x => x.Modifierad).ToList();
                if (tmpList != null)
                {
                    Pizzeria pizzeria = tmpList.Where(x => x.PizzeriaID == id).FirstOrDefault();
                    tmpList.Remove(pizzeria);
                    var json = SerializeToJson(tmpList);
                    if (!string.IsNullOrEmpty(json))
                    {
                        File.Delete(dataPath());
                        SaveJsonAsFile(json, dataPath());
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,ex);
                return false;
            }

        }


        private string dataPath()
        {
            return Directory.GetCurrentDirectory() + @"\Data\data.json";
        }


        private string GenerateDatabaseID()
        {
            Guid g = Guid.NewGuid();
            BigInteger bigInt = new BigInteger(g.ToByteArray());
            string retVal = bigInt.ToString();
            if (retVal.Contains("-") == true) { retVal = retVal.Replace("-", "0"); }
            return retVal;
        }


        private string SerializeToJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data, jsonSerializerSettings);
        }

        private T DeserializeFromJson<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        private bool SaveJsonAsFile(string json, string fullPathTosaveTo)
        {
            File.WriteAllText(fullPathTosaveTo, json);
            return File.Exists(fullPathTosaveTo);
        }


    }
}
