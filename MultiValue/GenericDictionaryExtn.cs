using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValue
{
    public class GenericDictionaryExtn<TKey, TValue> where TKey : notnull //Generic class with two generic type parameters
    {
        private readonly Dictionary<TKey, List<TValue>> multiValueDictionary;
        public GenericDictionaryExtn()   //Constructor
        {
            multiValueDictionary = new Dictionary<TKey, List<TValue>>();
        }
        public List<TKey> GetAllKeys() //Return all keys
        {
            return multiValueDictionary.Keys.ToList();
        }
        public List<TValue> GetMemberByKey(TKey key) //Returns the values for given key
        {
            if (multiValueDictionary.ContainsKey(key))
            {
                return multiValueDictionary[key];
            }
            else
            {
                return new List<TValue>();
            }
        }
        public void Add(TKey key, TValue value) //Add member to a given key
        {
            if (multiValueDictionary.ContainsKey(key))
            {
                multiValueDictionary[key].Add(value);                
            }
            else
            {
                multiValueDictionary[key] = new List<TValue>(); //Add key if not exist
                multiValueDictionary[key].Add(value);   //Add member to the new key
            }
        }
        public bool Remove(TKey key, TValue value)   //Remove value for a given key
        {

            if (multiValueDictionary[key].Contains(value))
            {
                multiValueDictionary[key].Remove(value);
                if (multiValueDictionary[key].Count == 0) //Checks member count for the key
                {
                    multiValueDictionary.Remove(key);   //remove key if no members 
                }
                return true;   //removed value
            }
            else
            {
                return false;   // member does not exist
            }
        }
        public void RemoveAll(TKey key)
        {

            multiValueDictionary.Remove(key); //remove all members and the given key

        }
        public bool Clear()
        {
            if (multiValueDictionary.Count > 0)
            {
                multiValueDictionary.Clear(); //remove all keys and members
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool KeyExists(TKey key)     //Check if given key exist
        {
            return multiValueDictionary.ContainsKey(key);

        }
        public bool MemberExists(TKey key, TValue value)
        {
            if (multiValueDictionary.ContainsKey(key) && multiValueDictionary[key].Contains(value)) //check if key exist and the combination of key , member exist
            {
                return true;
            }
            return false;
        }
        public List<TValue> AllMembers()
        {

            List<TValue> members = new List<TValue>();
            foreach (var item in multiValueDictionary)
            {
                var values = item.Value.ToList();
                members.AddRange(values);
            }
            return members;          
        }
        public Dictionary<TKey, List<TValue>> GetItems()
        {
            return multiValueDictionary;
        }
    }
}
