using MyParsLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyParsLib.ParseIntoJson
{
    public class ParseJsonService<T>
    {
        private static readonly JsonData<T> _jsonData = new JsonData<T>();

        public List<T> ParseFile(string jsonUrl)
        {
            var jsonData = _jsonData.GetJsonFile(jsonUrl);
            var persons = ConverJSONToObjectArray(jsonData);

            return persons;
        }

        private List<T> ConverJSONToObjectArray(string jsonData)
        {
            var result = new List<T>();
            var countOfFields = typeof(T).GetProperties().Length;
            var values = ParseJSON(jsonData);
            var sortedValues = RemoveDifferentValues(values);

            for (int i = 0; i < sortedValues.Count / countOfFields; i++)
            {
                result.Add(GenerateNewItem(sortedValues, i, countOfFields));
            }

            return result;
        }

        private List<Match> RemoveDifferentValues(MatchCollection values)
        {
            var model = (T)Activator.CreateInstance(typeof(T));
            var properties = model.GetType().GetProperties().Select(prop => prop.Name);

            return values.Where(val => properties.Contains(val.Value.Split(':')[0].Replace("\"", ""))).Select(val => val).ToList();
        }

        private MatchCollection ParseJSON(string jsonData)
        {
            var regexSelector = @"(\S\w*\S:\S\w*-\w*-\w*\S)|(\S\w*\S:\S?\w*\S?)";
            var regex = new Regex(regexSelector);

            return regex.Matches(jsonData);
        }

        private T GenerateNewItem(List<Match> values, int beginIndex, int countOfFields)
        {
            var result = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < countOfFields; i++)
            {
                var value = values[beginIndex * countOfFields + i].ToString().Split(':')[1].Replace("\"", "").Trim(',');

                AddValue(result, value, values, beginIndex, countOfFields, i);
            }

            return result;
        }

        private static void AddValue(T model, string value, List<Match> values, int beginIndex, int countOfFields, int insertIndex)
        {
            if (model.GetType()
                .GetProperty(values[beginIndex * countOfFields + insertIndex].ToString()
                .Split(':')[0].Replace("\"", "")).PropertyType.Name == "Int32")
            {
                model
                    .GetType().GetProperty(values[beginIndex * countOfFields + insertIndex].ToString().Split(':')[0].Replace("\"", ""))
                    .SetValue(model, Int32.Parse(value));
            }
            else if (model.GetType()
                .GetProperty(values[beginIndex * countOfFields + insertIndex].ToString()
                .Split(':')[0].Replace("\"", "")).PropertyType.Name == "DateTime")
            {
                model
                    .GetType().GetProperty(values[beginIndex * countOfFields + insertIndex].ToString().Split(':')[0].Replace("\"", ""))
                    .SetValue(model, DateTime.Parse(value));
            }
            else
            {
                model
                    .GetType().GetProperty(values[beginIndex * countOfFields + insertIndex].ToString().Split(':')[0].Replace("\"", ""))
                    .SetValue(model, value);
            }
        }
    }
}
