using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Drawing;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace задача__24
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
        public class MyTreeMap<K, Z> where K : IComparable<K>
        {
            public class Node
            {
                public K key;
                public Z value;
                public Node left;
                public Node right;
                public Node(K key, Z value)
                {
                    this.key = key;
                    this.value = value;
                    left = null;
                    right = null;
                }
            }

            private Node root;
            private int size;
            private IComparer<K> comparer;

            public MyTreeMap()
            {
                root = null;
                size = 0;
                comparer = null;
            }

            public MyTreeMap(IComparer<K> comparer)
            {
                root = null;
                size = 0;
                this.comparer = comparer;
            }

            private int Compare(K a, K b)
            {
                if (comparer != null)
                    return comparer.Compare(a, b);
                return a.CompareTo(b);
            }

            public void put(K key, Z value)
            {
                if (root == null)
                {
                    root = new Node(key, value);
                    size++;
                    return;
                }

                Node current = root;
                Node parent = null;
                int cmp = 0;

                while (current != null)
                {
                    parent = current;
                    cmp = Compare(key, current.key);
                    if (cmp < 0)
                        current = current.left;
                    else if (cmp > 0)
                        current = current.right;
                    else
                    {
                        current.value = value;
                        return;
                    }
                }

                Node newNode = new Node(key, value);
                if (cmp < 0)
                    parent.left = newNode;
                else
                    parent.right = newNode;
                size++;
            }

            public Z get(K key)
            {
                Node current = root;
                while (current != null)
                {
                    int cmp = Compare(key, current.key);
                    if (cmp < 0)
                        current = current.left;
                    else if (cmp > 0)
                        current = current.right;
                    else
                        return current.value;
                }
                return default(Z);
            }

            public bool containsKey(K key)
            {
                Node current = root;
                while (current != null)
                {
                    int cmp = Compare(key, current.key);
                    if (cmp < 0) current = current.left;
                    else if (cmp > 0) current = current.right;
                    else return true;
                }
                return false;
            }

            public void remove(K key)
            {
                root = RemoveRecursive(root, key);
            }

            private Node RemoveRecursive(Node node, K key)
            {
                if (node == null) return null;

                int cmp = Compare(key, node.key);
                if (cmp < 0)
                    node.left = RemoveRecursive(node.left, key);
                else if (cmp > 0)
                    node.right = RemoveRecursive(node.right, key);
                else
                {
                    if (node.left == null) return node.right;
                    if (node.right == null) return node.left;

                    Node minNode = FindMin(node.right);
                    node.key = minNode.key;
                    node.value = minNode.value;
                    node.right = RemoveRecursive(node.right, minNode.key);
                    size--;
                }
                return node;
            }

            private Node FindMin(Node node)
            {
                while (node.left != null)
                    node = node.left;
                return node;
            }

            public int Size() => size;
            public bool isEmpty() => size == 0;
            public void clear()
            {
                root = null;
                size = 0;
            }
        }
        public class IntComparer : System.Collections.Generic.IComparer<int>
        {
            public int Compare(int a, int b) => a.CompareTo(b);
        }
        
        static void Main(string[] args)
        {
            int[] sizes = {100000, 1000000};
            int iters = 20;

            double[] hashput = new double[sizes.Length];
            double[] treeput = new double[sizes.Length];
            double[] hashget = new double[sizes.Length];
            double[] treeget = new double[sizes.Length];
            double[] hashremove = new double[sizes.Length];
            double[] treeremove = new double[sizes.Length];

            
            for (int i = 0; i < sizes.Length; i++)
            {
                hashput[i] = testPutHash(sizes[i], iters);
                treeput[i] = testPutTree(sizes[i], iters);
                Console.WriteLine($"  {sizes[i]}: Hash={hashput[i]:F2} мс, Tree={treeput[i]:F2} мс");
            }

            
            for (int i = 0; i < sizes.Length; i++)
            {
                hashget[i] = TestGetHashMap(sizes[i], iters);
                treeget[i] = TestGetTreeMap(sizes[i], iters);
                Console.WriteLine($"  {sizes[i]}: Hash={hashget[i]:F2} мс, Tree={treeget[i]:F2} мс");
            }

           
            for (int i = 0; i < sizes.Length; i++)
            {
                hashremove[i] = TestRemoveHashMap(sizes[i], iters);
                treeremove[i] = TestRemoveTreeMap(sizes[i], iters);
                Console.WriteLine($"  {sizes[i]}: Hash={hashremove[i]:F2} мс, Tree={treeremove[i]:F2} мс");
            }

            DrawGraphs(sizes, hashput, treeput, hashget, treeget, hashremove, treeremove);
        }
        
        static double testPutTree(int size,int iters)
        {
            long totalTime = 0;

            for(int i = 0;i < iters;i++)
            {
                var map = new MyTreeMap<int, int>();
                var timeOper = Stopwatch.StartNew();

                for(int j = 0;j < size;j++)
                {
                    map.put(j, j);
                }

                timeOper.Stop();

                totalTime += timeOper.ElapsedTicks;
            }

            return (totalTime / (double)iters) / TimeSpan.TicksPerMillisecond;
        }

        static double testPutHash(int size,int iters)
        {
            long totTime = 0;

            for(int i = 0;i < iters;i++)
            {
                var map = new MyHashMap<int, int>();
                var timeOper = Stopwatch.StartNew();

                for(int j = 0;j < size;j++)
                {
                    map.put(j, j);
                }

                timeOper.Stop();
                totTime += timeOper.ElapsedTicks;
            }

            return (totTime/ (double)iters) / TimeSpan.TicksPerMillisecond;
        }
        static double TestGetHashMap(int size, int runs)
        {
            
            var map = new MyHashMap<int, int>();
            for (int i = 0; i < size; i++)
                map.put(i, i);

            long totalTicks = 0;

            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < size; i++)
                {
                    int val = map.get(i);
                }

                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            return (totalTicks / (double)runs) / TimeSpan.TicksPerMillisecond;
        }
        static double TestGetTreeMap(int size, int runs)
        {
            
            var map = new MyTreeMap<int, int>();
            for (int i = 0; i < size; i++)
                map.put(i, i);

            long totalTicks = 0;

            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < size; i++)
                {
                    int val = map.get(i);
                }

                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            return (totalTicks / (double)runs) / TimeSpan.TicksPerMillisecond;
        }
        static double TestRemoveHashMap(int size, int runs)
        {
            long totalTicks = 0;

            for (int r = 0; r < runs; r++)
            {
                var map = new MyHashMap<int, int>();
                for (int i = 0; i < size; i++)
                    map.put(i, i);

                var sw = Stopwatch.StartNew();

                for (int i = 0; i < size; i++)
                    map.remove(i);

                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            return (totalTicks / (double)runs) / TimeSpan.TicksPerMillisecond;
        }
        static double TestRemoveTreeMap(int size, int runs)
        {
            long totalTicks = 0;

            for (int r = 0; r < runs; r++)
            {
                var map = new MyTreeMap<int, int>();
                for (int i = 0; i < size; i++)
                    map.put(i, i);

                var sw = Stopwatch.StartNew();

                for (int i = 0; i < size; i++)
                    map.remove(i);

                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            return (totalTicks / (double)runs) / TimeSpan.TicksPerMillisecond;
        }
        static void DrawGraphs(int[] sizes, double[] hp, double[] tp, double[] hg, double[] tg, double[] hr, double[] tr)
        {
            
            Form formPut = new Form { WindowState = FormWindowState.Maximized, Text = "PUT - Сравнение HashMap vs TreeMap" };
            ZedGraphControl zgcPut = new ZedGraphControl { Dock = DockStyle.Fill };
            formPut.Controls.Add(zgcPut);

            GraphPane panePut = zgcPut.GraphPane;
            panePut.Title.Text = "PUT (добавление элементов)";
            panePut.XAxis.Title.Text = "Размер данных (N)";
            panePut.YAxis.Title.Text = "Время (мс)";

            PointPairList putHashPoints = new PointPairList();
            PointPairList putTreePoints = new PointPairList();
            for (int i = 0; i < sizes.Length; i++)
            {
                putHashPoints.Add(sizes[i], hp[i]);
                putTreePoints.Add(sizes[i], tp[i]);
            }

            
            LineItem hashPutCurve = panePut.AddCurve("HashMap_Put", putHashPoints, Color.Blue, SymbolType.Circle);
            hashPutCurve.Line.IsVisible = true;
            hashPutCurve.Line.Width = 2;
            hashPutCurve.Symbol.Size = 8;

            LineItem treePutCurve = panePut.AddCurve("TreeMap_Put", putTreePoints, Color.Red, SymbolType.Diamond);
            treePutCurve.Line.IsVisible = true;
            treePutCurve.Line.Width = 2;
            treePutCurve.Symbol.Size = 8;

            panePut.Legend.IsVisible = true;
            panePut.Legend.Position = LegendPos.Right;
            panePut.YAxis.Scale.MinAuto = true;
            panePut.YAxis.Scale.MaxAuto = true;
            panePut.XAxis.Scale.MinAuto = true;
            panePut.XAxis.Scale.MaxAuto = true;

            
            Form formGet = new Form { WindowState = FormWindowState.Maximized, Text = "GET - Сравнение HashMap vs TreeMap" };
            ZedGraphControl zgcGet = new ZedGraphControl { Dock = DockStyle.Fill };
            formGet.Controls.Add(zgcGet);

            GraphPane paneGet = zgcGet.GraphPane;
            paneGet.Title.Text = "GET (чтение элементов)";
            paneGet.XAxis.Title.Text = "Размер данных (N)";
            paneGet.YAxis.Title.Text = "Время (мс)";

            PointPairList getHashPoints = new PointPairList();
            PointPairList getTreePoints = new PointPairList();
            for (int i = 0; i < sizes.Length; i++)
            {
                getHashPoints.Add(sizes[i], hg[i]);
                getTreePoints.Add(sizes[i], tg[i]);
            }

            LineItem hashGetCurve = paneGet.AddCurve("HashMap_Get", getHashPoints, Color.Green, SymbolType.Circle);
            hashGetCurve.Line.IsVisible = true;
            hashGetCurve.Line.Width = 2;
            hashGetCurve.Symbol.Size = 8;

            LineItem treeGetCurve = paneGet.AddCurve("TreeMap_Get", getTreePoints, Color.Orange, SymbolType.Diamond);
            treeGetCurve.Line.IsVisible = true;
            treeGetCurve.Line.Width = 2;
            treeGetCurve.Symbol.Size = 8;

            paneGet.Legend.IsVisible = true;
            paneGet.Legend.Position = LegendPos.Right;
            paneGet.YAxis.Scale.MinAuto = true;
            paneGet.YAxis.Scale.MaxAuto = true;
            paneGet.XAxis.Scale.MinAuto = true;
            paneGet.XAxis.Scale.MaxAuto = true;

            
            Form formRemove = new Form { WindowState = FormWindowState.Maximized, Text = "REMOVE - Сравнение HashMap vs TreeMap" };
            ZedGraphControl zgcRemove = new ZedGraphControl { Dock = DockStyle.Fill };
            formRemove.Controls.Add(zgcRemove);

            GraphPane paneRemove = zgcRemove.GraphPane;
            paneRemove.Title.Text = "REMOVE (удаление элементов)";
            paneRemove.XAxis.Title.Text = "Размер данных (N)";
            paneRemove.YAxis.Title.Text = "Время (мс)";

            PointPairList removeHashPoints = new PointPairList();
            PointPairList removeTreePoints = new PointPairList();
            for (int i = 0; i < sizes.Length; i++)
            {
                removeHashPoints.Add(sizes[i], hr[i]);
                removeTreePoints.Add(sizes[i], tr[i]);
            }

            LineItem hashRemoveCurve = paneRemove.AddCurve("HashMap_Remove", removeHashPoints, Color.Purple, SymbolType.Circle);
            hashRemoveCurve.Line.IsVisible = true;
            hashRemoveCurve.Line.Width = 2;
            hashRemoveCurve.Symbol.Size = 8;

            LineItem treeRemoveCurve = paneRemove.AddCurve("TreeMap_Remove", removeTreePoints, Color.Brown, SymbolType.Diamond);
            treeRemoveCurve.Line.IsVisible = true;
            treeRemoveCurve.Line.Width = 2;
            treeRemoveCurve.Symbol.Size = 8;

            paneRemove.Legend.IsVisible = true;
            paneRemove.Legend.Position = LegendPos.Right;
            paneRemove.YAxis.Scale.MinAuto = true;
            paneRemove.YAxis.Scale.MaxAuto = true;
            paneRemove.XAxis.Scale.MinAuto = true;
            paneRemove.XAxis.Scale.MaxAuto = true;

           
            using (var bmp = new Bitmap(1200, 800))
            {
                zgcPut.DrawToBitmap(bmp, new Rectangle(0, 0, 1200, 800));
                bmp.Save("graph_put.png");
                Console.WriteLine("Сохранён: graph_put.png");
            }
            using (var bmp = new Bitmap(1200, 800))
            {
                zgcGet.DrawToBitmap(bmp, new Rectangle(0, 0, 1200, 800));
                bmp.Save("graph_get.png");
                Console.WriteLine("Сохранён: graph_get.png");
            }
            using (var bmp = new Bitmap(1200, 800))
            {
                zgcRemove.DrawToBitmap(bmp, new Rectangle(0, 0, 1200, 800));
                bmp.Save("graph_remove.png");
                Console.WriteLine("Сохранён: graph_remove.png");
            }

           
            formPut.Show();
            formGet.Show();
            formRemove.Show();

            Application.Run();
        }


    }
}
