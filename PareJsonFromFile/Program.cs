using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace PareJsonFromFile
{
    public class DataModel
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    class Program
    {
        static readonly string SERVER_ADDRESS = "http://mic.duytan.edu.vn:82/SmartMushroom/api/";
        static readonly RestClient _client = new RestClient(SERVER_ADDRESS);


        static void Main(string[] args)
        {
            var path = Path.GetFullPath("countries.json");
            LoadJson(path);
        }

        public static void LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<DataModel> items = JsonConvert.DeserializeObject<List<DataModel>>(json);
                InsertNationToDB(items);

            }

        }

        static void InsertNationToDB(List<DataModel> data)
        {
            foreach (var item in data)
            {
                PostMethod("NATION_LIST", item);
            }
        }

        public static string PostMethod(string tableName, object dataModel)
        {
            var request = new RestRequest(tableName, Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(dataModel);
            var resStatus = _client.Execute(request).StatusCode.ToString();
            return resStatus;
        }


    }
}
