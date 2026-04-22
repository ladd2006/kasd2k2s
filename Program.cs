using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
namespace задача_23._1
{
    internal class Program
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
                        return false;
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
        enum Vartype
        { 
            Int,
            Float,
            Double,
            hz
           

        }
        static Vartype getData(string type)
        {
            switch(type.ToLower())
            {
                case "int": return Vartype.Int;
                case "float": return Vartype.Float;
                case "double": return Vartype.Double;
                default: return Vartype.hz;
            }
                
        }

        static void ObrabotkaFile(string fileapth, MyHashMap<string, 
            Entry<Vartype,string>> map, List<string> errorss)
        {

            string text = File.ReadAllText(fileapth);

            Regex reg = new Regex(@"\b(int|float|double)\s+([a-z_][a-z0-9_]*)\s*=\s*(\d+)\s*;",
                RegexOptions.IgnoreCase);       
            
            MatchCollection matches = reg.Matches(text);

            foreach (Match match in matches)
            {
                Vartype type = getData(match.Groups[1].Value);
                string name = match.Groups[2].Value;
                string val = match.Groups[3].Value;

                if (type == Vartype.hz)
                {
                    errorss.Add($"Неккоректный тип данных {match.Groups[1].Value} у перемнной {name} ");
                    continue;
                }

                if (map.containsKey(name))
                {
                    errorss.Add($"Переменная {name} переопределенна");
                    continue;
                }
                
                    Entry<Vartype, string> data = new Entry<Vartype, string>(type, val);
                    map.put(name, data);
                

                    
            }
            
        }
        static void WriteRes(string file, MyHashMap<string,Entry<Vartype,string>> map,
            List<string> errorss)
        {
            string res = "";

            foreach (var val in map.entrySet())
            {
                res += $"{val.value.id.ToString().ToLower()} => {val.id}({val.value.value})\n";
            }
            foreach(string err in errorss)
            {
                res += err + "\n";
            }
            File.WriteAllText(file, res);
        }
        static void Main(string[] args)
        {
            string inputfile = "input.txt";
            string outputfile = "output.txt";

            MyHashMap<string, Entry<Vartype, string>> map = 
                new MyHashMap<string, Entry<Vartype, string>>();

            List<string> errors = new List<string>();

            ObrabotkaFile(inputfile, map, errors);

            WriteRes(outputfile,map, errors);

            Console.Read();
        }
    }
}
