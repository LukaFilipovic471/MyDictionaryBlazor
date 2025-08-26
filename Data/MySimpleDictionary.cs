using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyDictionaryBlazor.Data
{
    public class MySimpleDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private TKey[] keys;
        private TValue[] values;
        private int count;
        private int capacity;

        //Osnovni konstruktori za kreiranje
        public MySimpleDictionary(IDictionary<TKey, TValue> dict)
        {
            capacity = dict.Count;
            keys = new TKey[capacity];
            values = new TValue[capacity];
            foreach (var kv in dict)
                dodaj(kv.Key, kv.Value);
        }

        public MySimpleDictionary(int capacity)
        {
            this.capacity = capacity;
            keys = new TKey[capacity];
            values = new TValue[capacity];
            count = 0;
        }

        public MySimpleDictionary()
        {
            keys = new TKey[4];
            values = new TValue[4];
            count = 0;
            capacity = -1;
        }

        //Dodavanje novih elemenata i mogućnost da se iz rečnika dohvati vrednost na osnovu ključa.
        public void dodaj(TKey key, TValue value)
        {
            if (count == capacity)
                return;

            for (int i = 0; i < count; i++)
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                    return;

            if (count == keys.Length)
                addCapacity();

            keys[count] = key;
            values[count] = value;
            count++;
        }

        public TValue dohvati(TKey key)
        {
            for (int i = 0; i < count; i++)
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                    return values[i];
            throw new KeyNotFoundException($"Ključ '{key}' ne postoji.");
        }

        //Provera postojanja ključeva i vrednosti.
        public bool postojiKljuc(TKey key)
        {
            for (int i = 0; i < count; i++)
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                    return true;
            return false;
        }

        public bool postojiVrednost(TValue value)
        {
            for (int i = 0; i < count; i++)
                if (EqualityComparer<TValue>.Default.Equals(values[i], value))
                    return true;
            return false;
        }

        //Uklanjanje pojedinačnih elemenata, kao i brisanje celog sadržaja.
        public void obrisiElement(TKey key)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                {
                    for (int j = i; j < count - 1; j++)
                    {
                        keys[j] = keys[j + 1];
                        values[j] = values[j + 1];
                    }
                    keys[count - 1] = default!;
                    values[count - 1] = default!;
                    count--;
                    return;
                }
            }
        }

        public void obrisi()
        {
            for (int i = 0; i < count; i++)
            {
                keys[i] = default!;
                values[i] = default!;
            }
            count = 0;
        }

        //Pristup određenom elementu
        public TValue this[TKey key]
        {
            get
            {
                for (int i = 0; i < count; i++)
                    if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                        return values[i];
                throw new KeyNotFoundException($"Ključ '{key}' nije pronađen.");
            }
            set
            {
                for (int i = 0; i < count; i++)
                    if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                    {
                        values[i] = value;
                        return;
                    }
                dodaj(key, value);
            }
        }

        //Mogućnost iteriranja kroz elemente rečnika
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //Pregled broja elemenata i lista svih ključeva i vrednosti.
        public int duzina => count;

        public List<TKey> ListaKljuceva
        {
            get
            {
                var list = new List<TKey>(count);
                for (int i = 0; i < count; i++) list.Add(keys[i]);
                return list;
            }
        }

        public List<TValue> ListaVrednosti
        {
            get
            {
                var list = new List<TValue>(count);
                for (int i = 0; i < count; i++) list.Add(values[i]);
                return list;
            }
        }

        //pomocna funkcija
        private void addCapacity()
        {
            int newCapacity = keys.Length * 2;
            var newKeys = new TKey[newCapacity];
            var newValues = new TValue[newCapacity];
            for (int i = 0; i < count; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = values[i];
            }
            keys = newKeys;
            values = newValues;
        }
    }
}
