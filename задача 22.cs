using System;
using System.Collections.Generic;
using System.IO;

namespace задача_22
{
    public class Entry<T, P>
    {
        public T id;
        public P value;

        public Entry(T id, P value)
        {
            this.id = id;
            this.value = value;
        }
    }
    public class MyHashMap<T, P>
    {
        public LinkedList<Entry<T, P>>[] table;
        public int size;
        public int count;
        public float loadFactor;

        public MyHashMap()
        {
            table = new LinkedList<Entry<T, P>>[16];
            size = 16;
            count = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity)
        {
            table = new LinkedList<Entry<T, P>>[initialCapacity];
            size = initialCapacity;
            count = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity, float loadFactor)
        {
            table = new LinkedList<Entry<T, P>>[initialCapacity];
            size = initialCapacity;
            count = 0;
            this.loadFactor = loadFactor;
        }
        public void recize()
        {
            var old = table;
            int newsize = size * 2;

            table = new LinkedList<Entry<T, P>>[newsize];
            size = newsize;
            count = 0;

            foreach (var buck in old)
            {
                if (buck != null)
                {
                    foreach (var entry in buck)
                    {
                        put(entry.id, entry.value);
                    }
                }
            }


        }
        public int hashCode(T key)
        {
            return Math.Abs(key.GetHashCode()) % size;
        }
        public void clear()
        {
            table = new LinkedList<Entry<T, P>>[size];
            count = 0;
        }
        public bool containsKey(object key)
        {
            if (key == null)
                return false;
            T tKEY = (T)key;

            int hashkey = hashCode(tKEY);
            if (table[hashkey] == null)
                return false;

            foreach (var entry in table[hashkey])
            {
                if (entry.id.Equals(tKEY))
                {
                    return true;
                }
            }
            return false;
        }
        public bool containsValue(object value)
        {
            if (value == null)
                return false;
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var valuee in table[i])
                    {
                        if (valuee.value.Equals(value))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public List<Entry<T, P>> entrySet()
        {
            List<Entry<T, P>> res = new List<Entry<T, P>>();

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var entry in table[i])
                    {
                        res.Add(entry);
                    }
                }

            }
            return res;
        }
        public P get(object key)
        {
            if (key == null)
                return default(P);

            T tKey = (T)key;
            int hashKey = hashCode(tKey);

            if (table[hashKey] != null)
            {
                foreach (var entry in table[hashKey])
                {
                    if (entry.id.Equals(tKey))
                    {
                        return entry.value;
                    }
                }
            }
            return default(P);
        }
        public bool isEmpty()
        {
            if (count == 0) return false;
            return true;

        }
        public List<T> keySet()
        {
            List<T> res = new List<T>();

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var entry in table[i])
                    {
                        res.Add(entry.id);
                    }
                }
            }
            return res;
        }
        public bool put(T key, P value)
        {
            if (key == null)
                return false;


            if (count >= size * loadFactor)
            {
                recize();
            }

            int hashKey = hashCode(key);
            if (table[hashKey] == null)
            {
                table[hashKey] = new LinkedList<Entry<T, P>>();
            }

            foreach (var entry in table[hashKey])
            {
                if (entry.id.Equals(key))
                {
                    entry.value = value;
                    return true;
                }
            }
            table[hashKey].AddLast(new Entry<T, P>(key, value));
            count++;
            return true;

        }
        public bool remove(object key)
        {
            if (key == null) return false;

            T tkey = (T)key;

            int hashKey = hashCode(tkey);
            if (table[hashKey] != null)
            {
                foreach (var entry in table[hashKey])
                {
                    if (entry.id.Equals(tkey))
                    {
                        table[hashKey].Remove(entry);
                        count--;
                        return true;
                    }
                }
            }
            return false;
        }
        public int Size()
        {
            return count;
        }
    }
    internal class Program
    {
        static string CheckTagg(string tag)
        {
            if(tag.Length < 3) return null;
            
            if (tag[0] != '<' || tag[tag.Length - 1] != '>') return null;
            
            int a = 1;
            if (tag[a] == '/') a++;

            if (a >= tag.Length - 1 || !char.IsLetter(tag[a]))return null;

            string restag = "";

            while (a < tag.Length - 1)
            {
                char c = tag[a];
                if (!char.IsLetterOrDigit(c)) return null;

                restag += char.ToLower(c);
                a++;
            }
            return restag;

            
        }
        
        static void Main(string[] args)
        {
            string text = "input.txt";

            MyHashMap<string,int> map = new MyHashMap<string,int>();

            string[] lines = File.ReadAllLines(text);

            foreach(string line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    if(line[i] == '<')
                    {
                        int beg = i;
                        int end = -1;

                        for(int j = i;j < line.Length;j++)
                        {
                            if (line[j] == '>')
                            {
                                end = j;
                                break;
                            }
                        }
                        if(end == - 1) break;

                        string tag = line.Substring(beg, end - beg + 1);

                        string tagg = CheckTagg(tag);
                        if(map.containsKey(tagg))
                        {
                            int oldval = map.get(tagg);
                            map.put(tagg, oldval + 1);
                        }
                        else
                        {
                            map.put(tagg, 1);
                        }
                        i = end + 1;
                    }
                    else
                    {
                        i++;
                    }

                    foreach(var entry in map.entrySet())
                    {
                        Console.WriteLine(entry.id + " : " + entry.value);  
                    }
                }
            }
        }
    }
}
