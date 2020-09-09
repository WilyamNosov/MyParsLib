using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyParsLib.Data
{
    public class JsonData<T>
    {
        public string GetJsonFile()
        {
            var result = "";

            using (StreamReader sr = new StreamReader("../../../Json/Resourses.json"))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        public void WriteJsonFile(string jsonUrl)
        {
            var data = "";

            using (StreamReader sr = new StreamReader(jsonUrl))
            {
                data = sr.ReadToEnd();
            }

            using (StreamWriter sw = new StreamWriter("../../../Json/Resourses.json"))
            {
                sw.Write(data);
            }
        }
    }
}
