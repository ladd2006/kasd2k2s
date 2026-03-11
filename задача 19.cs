using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace задача_19_2_ку
{
    internal class Program
    {


        public class MyTreeSet<E> where E : IComparable<E>
        {

            private const bool RED = true;
            private const bool BLACK = false;

            private static readonly object present = new object();
            public MyTreeMap<E, object> m;
            private class Node
            {
                public E key;
                public Node left;
                public Node right;
                public Node parent;
                public bool color;

                public Node(E key, bool color)
                {
                    this.key = key;
                    this.color = color;
                }
            }

            private Node root;
            private int size;



            public MyTreeSet()
            {
                m = new MyTreeMap<E, object>();
                root = null;
                size = 0;
            }
            public MyTreeSet(MyTreeMap<E, object> m)
            {
                this.m = m;
                root = null;
                size = 0;

            }
            public MyTreeSet(TMC<E> comparator)
            {
                m = new MyTreeMap<E, object>(comparator);
                root = null;
                size = 0;

            }
            public MyTreeSet(E[] s)
            {
                m = new MyTreeMap<E, object>();

                foreach (E e in s)
                {
                    add(e);
                }

            }
            public MyTreeSet(SortedSet<E> s)
            {
                m = new MyTreeMap<E, object>();
                foreach (E e in s)
                {
                    add(e);
                }

            }
            private void balance(Node z)
            {
                while (z.parent != null && z.parent.color == RED)
                {
                    if (z.parent == z.parent.parent.left)
                    {
                        Node y = z.parent.parent.right;
                        //дядя красн
                        if (y != null && y.color == RED)
                        {
                            y.color = BLACK;
                            z.parent.color = BLACK;
                            z.parent.parent.color = RED;
                            z = z.parent.parent;

                        }
                        else
                        {      //дядя черный и справа z
                            if (z == z.parent.right)
                            {
                                z = z.parent;
                                leftRot(z);

                            }
                            //дядя черн, слева z
                            z.parent.color = BLACK;
                            z.parent.parent.color |= RED;
                            rightRot(z.parent.parent);
                        }

                    }
                    else
                    {
                        Node y = z.parent.parent.left;

                        //дядя красный 
                        if (y != null && y.color == RED)
                        {
                            z.parent.color = BLACK;
                            y.color = BLACK;
                            z.parent.parent.color = RED;
                            z = z.parent.parent;

                        }
                        else
                        {
                            //z слева
                            if (z == z.parent.left)
                            {
                                z = z.parent;
                                rightRot(z);
                            }

                            z.parent.color = BLACK;
                            z.parent.parent.color |= RED;

                            leftRot(z.parent.parent);
                        }
                    }

                }
                root.color = BLACK;
            }
            private void leftRot(Node x)
            {
                Node y = x.right;
                x.right = y.left;

                if (y.left != null)
                {
                    y.left.parent = x;
                }

                y.parent = x.parent;

                if (x.parent == null)
                {
                    root = y;
                }

                else if (x == x.parent.left)
                {
                    x.parent.left = y;
                }
                else
                {
                    x.parent.right = y;

                }
                y.left = x;
                x.parent = y;
            }
            private void rightRot(Node y)
            {
                Node x = y.left;

                y.left = x.right;

                if (x.right != null)
                {
                    x.right.parent = y;
                }
                x.parent = y.parent;
                if (y.parent == null)
                {
                    root = x;

                }
                else if (y == y.parent.right)
                {
                    y.parent.right = x;
                }
                else
                {
                    y.parent.left = x;
                }
                x.right = y;
                y.parent = x;
            }
            public bool add(E key)
            {
                Node node = new Node(key, RED);
                Node y = null;
                Node x = root;

                while (x != null)
                {
                    y = x;
                    if (key.CompareTo(x.key) < 0)
                    {
                        x = x.left;
                    }
                    else if (key.CompareTo(x.key) > 0)
                    {
                        x = x.right;
                    }
                    else
                        return false;
                }
                node.parent = y;
                if (y == null)
                {
                    root = node;
                }
                else if (key.CompareTo(y.key) < 0)
                {
                    y.left = node;
                }
                else
                {
                    y.right = node;
                }
                balance(node);

                m.put(key, present);

                return true;
            }
            public void addAll(E[] a)
            {
                foreach (E e in a)
                {
                    add(e);
                }
            }
            public void clear()
            {
                root = null;
                size = 0;
                m.clear();
            }
            public bool contains(object o)
            {
                return m.containsKey(o);
            }
            public bool ContainsAll(E[] a)
            {
                foreach (E e in a)
                {
                    if (!contains(e))
                    {
                        return false;
                    }
                }
                return true;
            }
            public bool IsEmpty()
            {
                if (m.Size() == 0)
                {
                    return true;
                }
                return false;
            }
            public bool remove(object o)
            {
                E key = (E)o;
                Node z = root;

                // ищем узел
                while (z != null)
                {
                    int cmp = key.CompareTo(z.key);

                    if (cmp < 0)
                        z = z.left;
                    else if (cmp > 0)
                        z = z.right;
                    else
                        break;
                }

                if (z == null)
                    return false;

                Node y = z;
                bool originalColor = y.color;
                Node x;

                if (z.left == null)
                {
                    x = z.right;
                    transplant(z, z.right);
                }
                else if (z.right == null)
                {
                    x = z.left;
                    transplant(z, z.left);
                }
                else
                {
                    y = minimum(z.right);
                    originalColor = y.color;
                    x = y.right;

                    if (y.parent == z)
                    {
                        if (x != null)
                            x.parent = y;
                    }
                    else
                    {
                        transplant(y, y.right);
                        y.right = z.right;
                        y.right.parent = y;
                    }

                    transplant(z, y);
                    y.left = z.left;
                    y.left.parent = y;
                    y.color = z.color;
                }

                if (originalColor == BLACK && x != null)
                    fixDelete(x);

                m.remove(key);
                size--;

                return true;
            }
            private void fixDelete(Node x)
            {
                while (x != root && x.color == BLACK)
                {
                    if (x == x.parent.left)
                    {
                        Node w = x.parent.right;

                        if (w.color == RED)
                        {
                            w.color = BLACK;
                            x.parent.color = RED;
                            leftRot(x.parent);
                            w = x.parent.right;
                        }

                        if ((w.left == null || w.left.color == BLACK) &&
                            (w.right == null || w.right.color == BLACK))
                        {
                            w.color = RED;
                            x = x.parent;
                        }
                        else
                        {
                            if (w.right == null || w.right.color == BLACK)
                            {
                                if (w.left != null)
                                    w.left.color = BLACK;

                                w.color = RED;
                                rightRot(w);
                                w = x.parent.right;
                            }

                            w.color = x.parent.color;
                            x.parent.color = BLACK;

                            if (w.right != null)
                                w.right.color = BLACK;

                            leftRot(x.parent);
                            x = root;
                        }
                    }
                    else
                    {
                        Node w = x.parent.left;

                        if (w.color == RED)
                        {
                            w.color = BLACK;
                            x.parent.color = RED;
                            rightRot(x.parent);
                            w = x.parent.left;
                        }

                        if ((w.left == null || w.left.color == BLACK) &&
                            (w.right == null || w.right.color == BLACK))
                        {
                            w.color = RED;
                            x = x.parent;
                        }
                        else
                        {
                            if (w.left == null || w.left.color == BLACK)
                            {
                                if (w.right != null)
                                    w.right.color = BLACK;

                                w.color = RED;
                                leftRot(w);
                                w = x.parent.left;
                            }

                            w.color = x.parent.color;
                            x.parent.color = BLACK;

                            if (w.left != null)
                                w.left.color = BLACK;

                            rightRot(x.parent);
                            x = root;
                        }
                    }
                }

                x.color = BLACK;
            }
            private void transplant(Node u, Node v)
            {
                if (u.parent == null)
                    root = v;
                else if (u == u.parent.left)
                    u.parent.left = v;
                else
                    u.parent.right = v;

                if (v != null)
                    v.parent = u.parent;
            }
            private Node minimum(Node x)
            {
                while (x.left != null)
                    x = x.left;

                return x;
            }
            public void RemoveAll(E[] a)
            {
                foreach(E e in a)
                {
                    remove(e);
                }
            }
            public void retainAll(E[] a)
            {
                List<E> keys = m.keySet();
                foreach(E key in keys)
                {
                    foreach(E value in a)
                    {
                        if(key.CompareTo(value) == 0)
                        {
                            remove(key); 
                        }
                    }    
                }
            }
            public int Size()
            {
                return m.Size();
            }
            public E[] toArray()
            {
                List<E> keys = m.keySet();
                return keys.ToArray();
            }
            public E[] toArray(E[] a)
            {
                List<E> keys = m.keySet();
                if(a == null || a.Length < keys.Count)
                {
                    a = new E[keys.Count];
                }
                for(int i = 0; i < keys.Count; i++)
                {
                    a[i] = keys[i];
                }
                return a;
            }
            public E first()
            {
                Node current = root;
                while(current.left != null)
                {
                    current = current.left;
                }
                return current.key;
            }
            public E last()
            {
                
                Node current = root;

                while (current.right != null)
                {
                    current = current.right;
                }

                return current.key;
            }
            public MyTreeSet<E> subSet(E fromElement, E toElement)
            {
                MyTreeSet<E> result = new MyTreeSet<E>();

                List<E> keys = m.keySet();

                foreach (E key in keys)
                {
                    if (key.CompareTo(fromElement) >= 0 &&
                        key.CompareTo(toElement) < 0)
                    {
                        result.add(key);
                    }
                }

                return result;
            }
            public MyTreeSet<E> headSet(E toElement)
            {
                MyTreeSet<E> result = new MyTreeSet<E>();

                List<E> keys = m.keySet();

                foreach (E key in keys)
                {
                    if (key.CompareTo(toElement) < 0)
                    {
                        result.add(key);
                    }
                }

                return result;
            }
            public MyTreeSet<E> tailSet(E fromElement)
            {
                MyTreeSet<E> result = new MyTreeSet<E>();

                List<E> keys = m.keySet();

                foreach (E key in keys)
                {
                    if (key.CompareTo(fromElement) >= 0)
                    {
                        result.add(key);
                    }
                }

                return result;
            }
            public E ceiling(E obj)
            {
                List<E> key = m.keySet();
                E min = default(E);
                bool find = false;
                foreach (E k in key)
                {
                    if(k.CompareTo(obj) >= 0)
                    {
                        if(!find || k.CompareTo(min) < 0)
                        {
                            min = k;
                            find = true;
                        }
                    }
                }
                if(!find)
                    return default(E);
                return min;
            }
            public E floor(E obj)
            {
                List<E> key = m.keySet();
                E max = default(E);
                foreach (E k in key)
                {
                    if (k.CompareTo(obj) <= 0)
                    {
                        if (k.CompareTo(max) > 0) max = k;
                    }
                }
                return max;
            }
            public E higher(E obj)
            {
                List<E> key = m.keySet();
                E max = default(E);
                bool find = false;
                foreach (E k in key)
                {
                    if (k.CompareTo(obj) > 0)
                    {
                        if (!find || k.CompareTo(max) > 0)
                        {
                            max = k;
                            find = true;
                        }
                    }
                }
                if(!find)
                {
                    return default(E);
                }
                return max;
            }
            public E lower(E obj)
            {
                List<E> key = m.keySet();
                E min = default(E);
                bool find = false;
                foreach (E k in key)
                {
                    if (k.CompareTo(obj) < 0)
                    {
                        if (!find || k.CompareTo(min) < 0)
                        {
                            find = true;

                            min = k;
                        }
                    }
                }
                if(!find)
                {
                    return default(E);
                }
                return min;
            }
            public MyTreeSet<E> haedSet(E upper,bool incl)
            {
                MyTreeSet<E> RES  = new MyTreeSet<E>();

                List<E> keys = m.keySet();

                foreach(E key in keys)
                {
                    if(!incl)
                    {
                        if(key.CompareTo(upper) < 0)
                        {
                            RES.add(key);
                        }
                        
                    }
                    else
                    {
                        if (key.CompareTo(upper) <= 0)
                        {
                            RES.add(key);
                        }
                    }
                }
                return RES;
            }
            public MyTreeSet<E> subSet(E lowerBound,bool lowIncl,E UpperBound,bool highIncl)
            {
                MyTreeSet<E> res = new MyTreeSet<E>();
                List<E> keys = m.keySet();

                foreach (E key in keys)
                {
                    if(lowIncl && highIncl)
                    {
                        if(key.CompareTo(lowerBound) >= 0 && key.CompareTo(UpperBound) <= 0)
                        { 
                            res.add(key);
                        }
                        
                    }
                    else if (!lowIncl && highIncl)
                    {
                        if (key.CompareTo(lowerBound) > 0 && key.CompareTo(UpperBound) <= 0)
                        {
                            res.add(key);
                        }
                    }
                    else if(lowIncl && !highIncl)
                    {
                        if (key.CompareTo(lowerBound) >= 0 && key.CompareTo(UpperBound) < 0)
                        {
                            res.add(key);
                        }
                    }
                    else
                    {
                        if (key.CompareTo(lowerBound) > 0 && key.CompareTo(UpperBound) < 0)
                        {
                            res.add(key);
                        }
                    }
                }
                return res;

            }
            public MyTreeSet<E> tailSet(E from,bool incl)
            {
                MyTreeSet<E> RES = new MyTreeSet<E>();

                List<E> keys = m.keySet();

                foreach (E key in keys)
                {
                    if (!incl)
                    {
                        if (key.CompareTo(from) > 0)
                        {
                            RES.add(key);
                        }

                    }
                    else
                    {
                        if (key.CompareTo(from) >= 0)
                        {
                            RES.add(key);
                        }
                    }
                }
                return RES;
            }
            public E pollLast()
            {
                if(m == null)
                {
                    return default(E);
                }
                Node max = root;
                while(max.left != null)
                {
                    max = max.left;
                }
                return max.key;
            }
            public E pollFirst()
            {
                if (m == null)
                {
                    return default(E);
                }
                Node min = root;
                while (min.right != null)
                {
                    min = min.right;
                }
                return min.key;
            }
            

        }
        
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
            public const bool red = true;
            public const bool black = true;
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
                    int cmp;

                    if (compar != null)
                    {
                        cmp = compar.Compare(keyK, node.key);
                    }
                    else
                    {
                        cmp = keyK.CompareTo(node.key);
                    }

                    if (cmp > 0)
                    {
                        node = node.right;
                    }
                    else if (cmp < 0)
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
                if (root == null)
                {
                    root = new Node(key, znac);
                    size++;
                    return;
                }
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
            MyTreeSet<int> set = new MyTreeSet<int>();

            // добавление элементов
            set.add(10);
            set.add(5);
            set.add(20);
            set.add(15);
            set.add(25);

            Console.WriteLine("Исходное множество:");
            foreach (int x in set.toArray())
                Console.Write(x + " ");
            Console.WriteLine();

            // size
            Console.WriteLine("Размер множества: " + set.Size());

            // contains
            Console.WriteLine("Есть ли 10: " + set.contains(10));

            // first и last
            Console.WriteLine("Минимальный элемент: " + set.first());
            Console.WriteLine("Максимальный элемент: " + set.last());

            // ceiling
            Console.WriteLine("ceiling(12): " + set.ceiling(12));

            // floor
            Console.WriteLine("floor(12): " + set.floor(12));

            // higher
            Console.WriteLine("higher(15): " + set.higher(15));

            // lower
            Console.WriteLine("lower(15): " + set.lower(15));

            // subset
            MyTreeSet<int> sub = set.subSet(10, 20);

            Console.WriteLine("subSet(10,20):");
            foreach (int x in sub.toArray())
                Console.Write(x + " ");
            Console.WriteLine();

            // headSet
            MyTreeSet<int> head = set.headSet(15);

            Console.WriteLine("headSet(15):");
            foreach (int x in head.toArray())
                Console.Write(x + " ");
            Console.WriteLine();

            // tailSet
            MyTreeSet<int> tail = set.tailSet(15);

            Console.WriteLine("tailSet(15):");
            foreach (int x in tail.toArray())
                Console.Write(x + " ");
            Console.WriteLine();

            
            

            Console.WriteLine();
        }
    }
}
