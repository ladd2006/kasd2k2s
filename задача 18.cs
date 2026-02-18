using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace задача_18_2_ку
{
    internal class Program
    {
        public class MyTreeMap<K, Z> where K : IComparable<K>
        {
            public class Pair
            {
                public K key;
                public Z znach;
                public Pair(K key, Z znach)
                {
                    this.key = key;
                    this.znach = znach;
                }
            }
            public class Node
            {
                public K key;
                public Z znac;
                public Node left;
                public Node right;
                public Node(K key, Z nac)
                {
                    this.key = key;
                    this.znac = nac;
                    left = null;
                    right = null;
                }
            }
            public Node root;
            public int size;
            public TMC<K> compar;

            public MyTreeMap()
            {
                root = null;
                size = 0;
                compar = null;

            }
            public MyTreeMap(TMC<K> compa)
            {
                root = null;
                size = 0;
                compar = compa;
            }
            //чистим
            public void clear()
            {
                root = null;
                size = 0;

            }
            //чекает есть ли ключ
            public bool containsKey(object key)
            {
                Node node = root;
                K keyK = (K)key;
                while (node != null)
                {
                    if (compar.Compare(keyK, node.key) > 0)
                    {
                        node = node.right;
                    }
                    if (compar.Compare(keyK, node.key) < 0)
                    {
                        node = node.left;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            //чекает есть значение
            public bool containsValue(object value)
            {
                List<Pair> allpair = getNodeAll();
                foreach (Pair pair in allpair)
                {
                    if (value.Equals(pair.znach))
                    {
                        return true;
                    }
                }
                return false;
            }
            //множество
            public List<Pair> entrySet()
            {

                return getNodeAll();
            }
            //возвр знач
            public Z get(object key)
            {
                K k = (K)key;
                Node current = root;
                List<Pair> allnode = getNodeAll();

                foreach (Pair node in allnode)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(k, current.key);
                    }
                    else
                    {
                        temp = k.CompareTo(current.key);
                    }

                    if (temp < 0)
                    {
                        current = current.left;
                    }
                    else if (temp > 0)
                    {
                        current = current.right;
                    }
                    else
                    {
                        return current.znac;
                    }

                }
                return default(Z);
            }           
            public bool isEmpty()
            {
                if (size == 0)
                {
                    return false;
                }
                return true;
            }
            //возвр всех ключей
            public List<K> keySet()
            {
                List<K> keys = new List<K>();

                foreach (Pair pair in getNodeAll())
                {
                    keys.Add(pair.key);
                }
                return keys;
            }
            //добав пары
            public void put(K key, Z znac)
            {
                Node current = root;
                Node parent = null;
                int temp = 0;
                while (current != null)
                {
                    parent = current;
                    if (compar != null)
                    {
                        temp = compar.Compare(key, current.key);
                    }
                    else
                    {
                        temp = key.CompareTo(current.key);
                    }
                    if (temp < 0)
                    {
                        current = current.left;

                    }
                    else if (temp > 0)
                    {
                        current = current.right;
                    }
                    else
                    {
                        current.znac = znac;

                    }
                }
                Node newNode = new Node(key, znac);
                if (temp < 0)
                {
                    parent.left = newNode;
                }
                else
                {
                    parent.right = newNode;
                }
                size++;
            }
            
            public void remove(object key)
            {
                List<Pair> res = new List<Pair>();
                List<Pair> allpair = new List<Pair>();
                K delkey = (K)key;

                foreach (Pair pair in allpair)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(delkey, pair.key);

                    }
                    else
                    {
                        temp = delkey.CompareTo(pair.key);
                    }
                    if (temp != 0)
                    {
                        res.Add(pair);
                    }
                }
                clear();
                foreach (Pair pair in res)
                {
                    put(pair.key, pair.znach);
                }
            }
            public int Size()
            {
                return size;
            }
            public K firstKey()
            {
                Node current = root;
                while (current.left != null)
                {
                    current = current.left;
                }
                return current.key;
            }
            public K lastKey()
            {
                Node current = root;
                while (current.right != null)
                {
                    current = current.right;
                }
                return current.key;
            }
            //изобр с ключом меньше end и тд.
            public MyTreeMap<K, Z> headMap(K end)
            {
                MyTreeMap<K, Z> resMap = new MyTreeMap<K, Z>();
                List<Pair> allpairs = new List<Pair>();

                foreach (Pair pair in allpairs)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, end);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(end);
                    }

                    if (temp < 0)
                    {
                        resMap.put(pair.key, pair.znach);
                    }
                }
                return resMap;
            }
            public MyTreeMap<K, Z> subMap(K start, K end)
            {
                MyTreeMap<K, Z> resMap = new MyTreeMap<K, Z>();
                List<Pair> allpairs = new List<Pair>();

                foreach (Pair pair in allpairs)
                {
                    int temp1, temp2;

                    if (compar != null)
                    {
                        temp2 = compar.Compare(pair.key, end);
                        temp1 = compar.Compare(pair.key, start);
                    }
                    else
                    {
                        temp1 = pair.key.CompareTo(start);
                        temp2 = pair.key.CompareTo(end);
                    }

                    if (temp1 >= 0 && temp2 < 0)
                    {
                        resMap.put(pair.key, pair.znach);
                    }
                }
                return resMap;

            }
            public MyTreeMap<K, Z> tailMap(K start)
            {
                MyTreeMap<K, Z> resMap = new MyTreeMap<K, Z>();
                List<Pair> allpairs = new List<Pair>();

                foreach (Pair pair in allpairs)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, start);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(start);
                    }

                    if (temp > 0)
                    {
                        resMap.put(pair.key, pair.znach);
                    }
                }
                return resMap;

            }
            //возвр пары
            public Pair lowerEnrty(K key)
            {
                List<Pair> allpairs = getNodeAll();
                foreach (Pair pair in allpairs)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp < 0)
                    {
                        return new Pair(pair.key, pair.znach);
                    }
                }
                return default(Pair);
            }
            public Pair floorEntry(K key)
            {
                List<Pair> allpairs = getNodeAll();
                foreach (Pair pair in allpairs)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp <= 0)
                    {
                        return new Pair(pair.key, pair.znach);
                    }
                }
                return default(Pair);
            }
            public Pair higherEnrty(K key)
            {
                List<Pair> allpairs = getNodeAll();
                foreach (Pair pair in allpairs)
                {
                    int temp;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp > 0)
                    {
                        return new Pair(pair.key, pair.znach);
                    }
                }
                return default(Pair);
            }
            public Pair ceilingEnrty(K key)
            {
                List<Pair> allpairs = getNodeAll();
                foreach (Pair pair in allpairs)
                {
                    int temp = 0;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp >= 0)
                    {
                        return new Pair(pair.key, pair.znach);
                    }
                }
                return default(Pair);
            }
            public K lowerKey(K key)
            {
                List<Pair> allpairs = getNodeAll();

                foreach (Pair pair in allpairs)
                {
                    int temp = 0;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp < 0)
                    {
                        return pair.key;
                    }

                }
                return default(K);
            }
            public K floorKey(K key)
            {
                List<Pair> allpairs = getNodeAll();

                foreach (Pair pair in allpairs)
                {
                    int temp = 0;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp <= 0)
                    {
                        return pair.key;
                    }

                }
                return default(K);
            }
            public K higherrKey(K key)
            {
                List<Pair> allpairs = getNodeAll();

                foreach (Pair pair in allpairs)
                {
                    int temp = 0;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp > 0)
                    {
                        return pair.key;
                    }

                }
                return default(K);
            }
            public K ceielngKey(K key)
            {
                List<Pair> allpairs = getNodeAll();

                foreach (Pair pair in allpairs)
                {
                    int temp = 0;
                    if (compar != null)
                    {
                        temp = compar.Compare(pair.key, key);
                    }
                    else
                    {
                        temp = pair.key.CompareTo(key);
                    }
                    if (temp >= 0)
                    {
                        return pair.key;
                    }

                }
                return default(K);
            }
            public Pair pollFirst()
            {
                K fir = firstKey();
                Z znachen = get(fir);

                remove(fir);
                return new Pair(fir, znachen);
            }
            public Pair pollLast()
            {
                K last = lastKey();
                Z znachen = get(last);

                remove(last);
                return new Pair(last, znachen);
            }
            public Pair FirstEntr()
            {
                K fir = firstKey();
                Z znachen = get(fir);


                return new Pair(fir, znachen);
            }
            public Pair LastEntr()
            {
                K last = lastKey();
                Z znachen = get(last);


                return new Pair(last, znachen);
            }
            public List<Pair> getNodeAll()
            {
                List<Pair> res = new List<Pair>();

                Stack<Node> stack = new Stack<Node>();
                Node current = root;
                while (stack.Count > 0 || current != null)
                {
                    while (current != null)
                    {
                        stack.Push(current);
                        current = current.left;
                    }
                    current = stack.Pop();
                    res.Add(new Pair(current.key, current.znac));
                    current = current.right;
                }
                return res;
            }
        }
        public interface TMC<K>
        {
            int Compare(K a, K b);
        }
        static void Main(string[] args)
        {
        }
    }
}

