using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Задача20
{
    internal class Program
    {
        class Graph
        {
            public int vershCount;
            public List<int>[] masSmezh;
            public List<int>[] TranspmasSmez;

            private int[,] capacity;
            private int[,] flow;

            public Graph(int vershini)
            {
                vershCount = vershini;
                masSmezh = new List<int>[vershini];
                TranspmasSmez = new List<int>[vershini];

                for(int i = 0; i < vershini; i++)
                {
                    masSmezh[i] = new List<int>();
                    TranspmasSmez[i] = new List<int>();
                    capacity = new int[vershini, vershini];
                    flow = new int[vershini, vershini];
                }
            }

            public void AddRebro(int from, int to)
            {
                masSmezh[from].Add(to);
                TranspmasSmez[to].Add(from);    
            }
            public void AddRebroWithCapacity(int from, int to, int cap)
            {
                masSmezh[from].Add(to);
                masSmezh[to].Add(from);  
                capacity[from, to] = cap;
                
            }
            public void AddUndirectedRebro(int v1, int v2)
            {
                masSmezh[v1].Add(v2);
                masSmezh[v2].Add(v1);
                TranspmasSmez[v1].Add(v2);
                TranspmasSmez[v2].Add(v1);
            }
            public List<int> FindArticulationPoints()
            {
                
                bool[] visited = new bool[vershCount];

                
                int[] tin = new int[vershCount];

               
                int[] low = new int[vershCount];

                
                bool[] isArticulation = new bool[vershCount];

                int timer = 0;


               
                for (int i = 0; i < vershCount; i++)
                {
                    if (!visited[i])
                    {
                       
                        DFS_Articulation(i, -1, visited, tin, low, ref timer, isArticulation);
                    }
                }

               
                List<int> articulationPoints = new List<int>();
                for (int i = 0; i < vershCount; i++)
                {
                    if (isArticulation[i])
                    {
                        articulationPoints.Add(i);
                    }
                }

                return articulationPoints;
            }

            
            private void DFS_Articulation(int v, int parent, bool[] visited, int[] tin, int[] low, ref int timer, bool[] isArticulation)
            {
                
                visited[v] = true;

                
                tin[v] = timer;
                low[v] = timer;
                timer++;

                int childrenCount = 0;  
                foreach (int neighbor in masSmezh[v])
                {
                    
                    if (neighbor == parent) continue;

                    if (visited[neighbor])
                    {
                        
                        low[v] = Math.Min(low[v], tin[neighbor]);
                        
                    }
                    else
                    {
                        
                        DFS_Articulation(neighbor, v, visited, tin, low, ref timer, isArticulation);

                        
                        low[v] = Math.Min(low[v], low[neighbor]);
                        
                        if (parent != -1 && low[neighbor] >= tin[v])
                        {
                            isArticulation[v] = true;
                            
                        }

                        childrenCount++;
                    }
                }

                
                if (parent == -1 && childrenCount > 1)
                {
                    isArticulation[v] = true;
                   
                }

                
            }
            public int Edmonds(int istok,int stok)
            {
                int maxflow = 0;
                int iter = 0;

                int[] pyt = new int[vershCount];

                while(BFS(istok,stok,pyt))
                {
                    iter++;

                    int minProp = FindMin(istok, stok, pyt);

                    updatepotok(istok, stok, pyt, minProp);

                    maxflow += minProp;


                }

                return maxflow;
            }
            private bool BFS(int istok, int stok, int[] parent)
            {
                bool[] visited = new bool[vershCount];
                Queue<int> queue = new Queue<int>();

                queue.Enqueue(istok);
                visited[istok] = true;
                parent[istok] = -1;

             

                while (queue.Count > 0)
                {
                    int current = queue.Dequeue();
                

                 
                    foreach (int neighbor in masSmezh[current])
                    {
                        
                        int residualCapacity = capacity[current, neighbor] - flow[current, neighbor];

                       
                        if (!visited[neighbor] && residualCapacity > 0)
                        {
                            visited[neighbor] = true;
                            parent[neighbor] = current;
                            queue.Enqueue(neighbor);

                            
                            if (neighbor == stok)
                            {
                               
                                return true;
                            }
                        }
                    }
                }

               
                return false;
            }
            private int FindMin(int istok, int stok, int[] parent)
            {
                int pathFlow = int.MaxValue;

               
                for (int v = stok; v != istok; v = parent[v])
                {
                    int u = parent[v];
                    int residual = capacity[u, v] - flow[u, v];
                    pathFlow = Math.Min(pathFlow, residual);
                    
                }

                return pathFlow;
            }
            private void updatepotok(int istok, int stok, int[] parent, int pathFlow)
            {

                for (int v = stok; v != istok; v = parent[v])
                {
                    int u = parent[v];

                    
                    flow[u, v] += pathFlow;
                    
                    flow[v, u] -= pathFlow;

                 
                }
            }
            public List<List<int>> Kosarau()
            {
                bool[] visited = new bool[vershCount];
                
                Stack<int> poradok = new Stack<int>();

                List<List<int>> components = new List<List<int>>();

                for(int i = 0;i < vershCount;i++)
                {
                    if (!visited[i])
                    {
                        DFS1(i, visited, poradok);
                    }
                }
                for(int i = 0;i < vershCount;i++)
                {
                    visited[i] = false;
                }
                while(poradok.Count > 0)
                {
                    int vers = poradok.Pop();
                    if(!visited[vers])
                    {
                        List<int> component = new List<int>();
                        DFS2(vers, visited,component);
                        components.Add(component);
                    }
                }
                return components;



            }

            public void DFS1(int versh, bool[] visted, Stack<int> stack)
            {
                visted[versh] = true;

                foreach(int neigh in masSmezh[versh])
                {
                    if(!visted[neigh])
                    {
                        DFS1(neigh,visted,stack);
                    }
                }

                stack.Push(versh);
            }
            public void DFS2(int versh, bool[] visited, List<int> component)
            {
                visited[versh] = true;
                component.Add(versh);

                foreach(int neigh in TranspmasSmez[versh])
                {
                    if (!visited[neigh])
                    {
                        DFS2(neigh,visited,component);
                    }
                }
            }
            public void PrintGraph()
            {
                Console.WriteLine("\nСписок смежности:");
                for (int i = 0; i < vershCount; i++)
                {
                    Console.Write($"  Вершина {i}: ");
                    if (masSmezh[i].Count == 0)
                        Console.WriteLine("нет ребер");
                    else
                        Console.WriteLine(string.Join(" -> ", masSmezh[i]));
                }
            }

            public void PrintCapacities()
            {
                Console.WriteLine("\nПропускные способности:");
                for (int i = 0; i < vershCount; i++)
                {
                    for (int j = 0; j < vershCount; j++)
                    {
                        if (capacity[i, j] > 0)
                        {
                            Console.WriteLine($"  {i} -> {j}: {capacity[i, j]}");
                        }
                    }
                }
            }

        }
        
        static void Main(string[] args)
        {
            
           
            Graph graphKosaraju = new Graph(8);

            // Компонента 1: 0,1,2
            graphKosaraju.AddRebro(0, 1);
            graphKosaraju.AddRebro(1, 2);
            graphKosaraju.AddRebro(2, 0);

            // Компонента 2: 3,4,5
            graphKosaraju.AddRebro(3, 4);
            graphKosaraju.AddRebro(4, 5);
            graphKosaraju.AddRebro(5, 3);

            // Связь между компонентами
            graphKosaraju.AddRebro(3, 1);

            // Вершина 6 отдельно, вершина 7 отдельно
            graphKosaraju.AddRebro(6, 6);  // петля

            Console.WriteLine("Ориентированный граф:");
            graphKosaraju.PrintGraph();

            List<List<int>> components = graphKosaraju.Kosarau();

            Console.WriteLine($"\n=== РЕЗУЛЬТАТ ===");
            Console.WriteLine($"Найдено компонент сильной связности: {components.Count}");
            for (int i = 0; i < components.Count; i++)
            {
                Console.WriteLine($"  Компонента {i + 1}: {{ {string.Join(", ", components[i])} }}");
            }

          
            Console.WriteLine("\n\n" + new string('=', 60));
            Console.WriteLine("МЕТОД 10: МАКСИМАЛЬНЫЙ ПОТОК (АЛГОРИТМ ЭДМОНДСА-КАРПА)");
            Console.WriteLine(new string('=', 60));

            Graph graphEdmonds = new Graph(6);

       
            graphEdmonds.AddRebroWithCapacity(0, 1, 16);  
            graphEdmonds.AddRebroWithCapacity(0, 2, 13);  
            graphEdmonds.AddRebroWithCapacity(1, 2, 10);  
            graphEdmonds.AddRebroWithCapacity(1, 3, 12);  
            graphEdmonds.AddRebroWithCapacity(2, 1, 4);   
            graphEdmonds.AddRebroWithCapacity(2, 4, 14);  
            graphEdmonds.AddRebroWithCapacity(3, 2, 9);   
            graphEdmonds.AddRebroWithCapacity(3, 5, 20);  
            graphEdmonds.AddRebroWithCapacity(4, 3, 7);   
            graphEdmonds.AddRebroWithCapacity(4, 5, 4);   

            Console.WriteLine("Транспортная сеть:");
            graphEdmonds.PrintGraph();
            graphEdmonds.PrintCapacities();

            int maxFlow = graphEdmonds.Edmonds(0, 5);
            Console.WriteLine($"\nМаксимальный поток: {maxFlow}");

            
            

            Graph graphArticulation = new Graph(7);

            
            graphArticulation.AddUndirectedRebro(0, 1);
            graphArticulation.AddUndirectedRebro(1, 2);
            graphArticulation.AddUndirectedRebro(2, 0);
            graphArticulation.AddUndirectedRebro(2, 3);
            graphArticulation.AddUndirectedRebro(3, 4);
            graphArticulation.AddUndirectedRebro(4, 5);
            graphArticulation.AddUndirectedRebro(5, 3);
            graphArticulation.AddUndirectedRebro(2, 6);

            Console.WriteLine("Неориентированный граф:");
            graphArticulation.PrintGraph();

            List<int> articulationPoints = graphArticulation.FindArticulationPoints();

            Console.WriteLine($"\n=== РЕЗУЛЬТАТ ===");
            if (articulationPoints.Count == 0)
            {
                Console.WriteLine("В графе нет шарниров");
            }
            else
            {
                Console.WriteLine($"Найдено шарниров: {articulationPoints.Count}");
                Console.WriteLine($"Шарниры: {{ {string.Join(", ", articulationPoints)} }}");
            }

            
            
            Console.ReadLine();
        }
    }
}
