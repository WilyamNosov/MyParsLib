using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyParsLib.Data
{
    public class JsonData<T>
    {
        public string GetJsonFile(string url)
        {
            var result = "";

            using (StreamReader sr = new StreamReader(url))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
