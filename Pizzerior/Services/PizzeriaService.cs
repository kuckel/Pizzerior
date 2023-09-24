#nullable disable 

using Newtonsoft.Json;

using Pizzerior.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Culture = new CultureInfo("sv-SE"),
            DateFormatString = "yyyy-MM-dd"

        };

        public PizzeriaService()
        {

        }

        public List<Pizzeria> GetAll()
        {
            if(File.Exists(dataPath()) )
            {
                string json = File.ReadAllText(dataPath());
                return DeserializeFromJson<List<Pizzeria>>(json);
            }
            else
            {
                return null;
            }

        }


        public ObservableCollection<Pizzeria> GetAllCollection()
        {
            ObservableCollection<Pizzeria> tmpColList= new ObservableCollection<Pizzeria>();
            List<Pizzeria> tmpList = GetAll();
            if (tmpList != null && tmpList.Count > 0)
            {
                
                tmpList.ForEach(x => tmpColList.Add(x));
            }
            return tmpColList;
       }

 

        public Pizzeria Get(string id)
        {
            string json = File.ReadAllText(dataPath());
            return DeserializeFromJson<List<Pizzeria>>(json).Where(x=>x.PizzeriaID == id).FirstOrDefault() ;
        }

        public bool Create(List<Pizzeria> pizzerior)
        {
            try
            {
                var json = SerializeToJson(pizzerior);
                SaveJsonAsFile(json, dataPath());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Pizzeria Update(Pizzeria pizzeria)
        {
            List<Pizzeria> tmpList = GetAll();
            if (tmpList != null)
            {
                tmpList.Add(pizzeria);
            }
            else 
            {
                tmpList= new List<Pizzeria>();
                tmpList.Add(pizzeria);
            }
            var json = SerializeToJson(tmpList);
            SaveJsonAsFile(json, dataPath());
            return pizzeria;
        }

        public bool Delete(string id)
        {
            List<Pizzeria> tmpList = GetAll();
            if (tmpList != null)
            {
                Pizzeria pizzeria = tmpList.Where(x => x.PizzeriaID == id).FirstOrDefault();
                tmpList.Remove(pizzeria);
                var json = SerializeToJson(tmpList);
                SaveJsonAsFile(json, dataPath());
                return true;
            }
            else { return false; }

        }


        private string dataPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["JsonDataPath"];
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
