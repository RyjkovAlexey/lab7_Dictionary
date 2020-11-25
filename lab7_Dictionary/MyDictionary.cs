using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7_Dictionary
{
    [Serializable]
    class MyDictionary
    {
        private Dictionary<String,String> dictionary = new Dictionary<string, string>();
        private string nameDictionary;

        public MyDictionary(string nameDictionary)
        {
            this.nameDictionary = nameDictionary;
        }

        public string Search(string word)
        {
            if (dictionary.ContainsKey(word))
            {
                return dictionary[word];
            }
            else
            {
                return "Не найдено";
            }
        }

        public void Add(string word, string translateWord)
        {
            dictionary.Add(word,translateWord);
        }

        public void Remove(string word)
        {
            dictionary.Remove(word);
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool ContainsValue(string word)
        {
            return dictionary.ContainsValue(word);
        }

        public static bool ContainsDictionary(List<MyDictionary> myDictionaries, string nameDict)
        {
            bool flag = false;
            myDictionaries.ForEach(dict=>
            {
                if (dict.nameDictionary.Equals(nameDict))
                {
                    flag = true;
                }
            });
            return flag;
        }

        public MyDictionary Merge(MyDictionary dictionary, string newDictionaryName)
        {
            MyDictionary newDictionary = new MyDictionary(newDictionaryName);
            var keys = this.dictionary.Keys;
            foreach (var key in keys)
            {
                newDictionary.Add(key,this.dictionary[key]);
            }

            keys = dictionary.dictionary.Keys;
            foreach (var key in keys)
            {
                string tempValue = dictionary.dictionary[key];
                if (!newDictionary.ContainsKey(key)&&!newDictionary.ContainsValue(tempValue))
                {
                    newDictionary.Add(key,tempValue);
                }
            }

            return newDictionary;
        }

        public MyDictionary Crossing(MyDictionary dictionary, string newDictionaryName)
        {
            MyDictionary newDictionary = new MyDictionary(newDictionaryName);
            var keys = this.dictionary.Keys;
            foreach (var key in keys)
            {
                string tempValue = this.dictionary[key];
                if (dictionary.ContainsKey(key)&&dictionary.Search(key).Equals(tempValue))
                {
                    newDictionary.Add(key,tempValue);
                }
            }

            return newDictionary;
        }

        public MyDictionary Subtraction(MyDictionary dictionary, string newDictionaryName)
        {
            MyDictionary newDictionary = new MyDictionary(newDictionaryName);
            var keys = this.dictionary.Keys;
            foreach (var key in keys)
            {
                string tempValue = this.dictionary[key];
                if (!dictionary.ContainsKey(key)&&!dictionary.ContainsValue(tempValue))
                {
                    newDictionary.Add(key,tempValue);
                }
            }

            keys = dictionary.dictionary.Keys;
            foreach (var key in keys)
            {
                string tempValue = dictionary.dictionary[key];
                if (!this.dictionary.ContainsKey(key)&&!this.dictionary.ContainsValue(tempValue))
                {
                    newDictionary.Add(key,tempValue);
                }
            }

            return newDictionary;
        }

        public string NameDictionary
        {
            get => nameDictionary;
        }

        public Dictionary<string, string> Dictionary
        {
            get => this.dictionary;
        }
    }
}
