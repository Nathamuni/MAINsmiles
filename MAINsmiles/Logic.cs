
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class WeightedGraph
    {
        //INPUT SMILES   
        static int[] ElementIndexs; 
        static int elementCount=0;

        //ADJACENCY LIST CONTAINING FOR GRAPH NODES CONNECTIONS
        public Dictionary<int, Dictionary<int, int>> adjacencyList;

        //CREATING OBJECT <GRAPH> 
        static WeightedGraph graph = new WeightedGraph();

        //USED FOR STORING THE IMPLICIT HYDROGENS COUNT 
        // HydrogenCount --> < INDEX_VALUE , NUMBER OF IMPLICIT HYDROGENS >
        public Dictionary<int,int> HydrogenCount;
        
       //readonly
       // Elements --> CONTAINS ALL THE < ELEMENT_NAME , VALENCE_ELECTRONS > PAIR
        public static readonly Dictionary<string,int> Elements = new Dictionary<string, int>()
        {
            {"H",1},{"He",2},{"Li",1},{"Be",2},{"B",3},{"C",4},{"N",5},
	        {"O",6},{"F",7},{"Ne",8},{"Na",1},{"Mg",2},{"Al",3},{"Si",5},
            {"P",5},{"S",6},{"Cl",7},{"Ar",8},{"K",1},{"Ca",2},{"Sc",3},
            {"Ti",4},{"V",5},{"Cr",6},{"Mn",7},{"Fe",8},{"Co",9},{"Ni",10},
            {"Cu",11},{"Zn",12},{"Ga",3},{"Ge",3},{"As",5},{"Se",6},{"Br",7},
            {"Kr",8},{"Rb",1},{"Sr",2},{"Y",3},{"Zr",4},{"Nb",5},{"Mo",6},
            {"Tc",7},{"Ru",8},{"Rh",9},{"Pd",10},{"Ag",11},{"Cd",12},{"In",3},
            {"Sn",4},{"Sb",5},{"Te",6},{"I",10},{"Xe",8},{"Cs",1},{"Ba",2},
            {"La",3},{"Ce",4},{"Pr",5},{"Nd",6},{"Pm",7},{"Sm",8},{"Eu",9},
            {"Gd",10},{"Tb",11},{"Dy",12},{"Ho",13},{"Er",14},{"Tm",15},{"Yb",16},
            {"Lu",3},{"Hf",4},{"Ta",5},{"W",6},{"Re",7},{"Os",8},{"Ir",9},
            {"Pt",10},{"Au",11},{"Hg",12},{"Pb",4},{"Bi",5},{"Po",6},
            {"At",7},{"Rn",8},{"Fr",1},{"Ra",2},{"Ac",3},{"Th",4},{"Pa",5},
            {"U",6},{"Np",7},{"Pu",8},{"Am",9},{"Cm",10},{"Bk",11},{"Cf",12},
            {"Es",13},{"Fm",14},{"Md",15},{"No",16},{"Lr",3},{"Rf",4},{"Db",5},
            {"Sg",6},{"Bh",7},{"Hs",8},{"Mt",9},{"Ds",10},{"Rg",11},{"Cn",12},
            {"Nh",3},{"Fl",4},{"Mc",5},{"Lv",6},{"Ts",7},{"Og",8}
        };

        // bonds --> CONTAINS ALL THE < BOND , BOND_VALUE > PAIRS
        public static readonly Dictionary<char,int> bonds = new Dictionary<char, int>()
        {
            {'-',1},{'=',2},{'#',3},{'$', 4},{'.', 0},{':',1}
        };

        //CONSTRUCTOR 
        public WeightedGraph()
        {
            //INITIALIZING adjacencyList AND HydrogenCount AS EMPTY LISTS
            this.adjacencyList = new Dictionary<int , Dictionary<int, int>>(){};
            this.HydrogenCount = new Dictionary<int, int>(){};
        }

        //VERTEX CREATION-->Add new vertex
        public void addVertex(int vertex) 
        {
            if(!this.adjacencyList.ContainsKey(vertex))
            {
                this.adjacencyList.Add(vertex, new Dictionary<int, int>());
            }
            else
            {
                Console.WriteLine("this vertex is in use");
            }
        }

        //CREATION OF EDGES BETWEEN NODES--> New edge between 2 vertices
        public void addEdge(int v1, int v2, int weight) 
        {
            if (this.adjacencyList.ContainsKey(v1) && this.adjacencyList.ContainsKey(v2))
            {
                this.adjacencyList[v1].Add(v2, weight);
                this.adjacencyList[v2].Add(v1, weight);
            }
            else
            {
                Console.WriteLine(v1);
                Console.WriteLine(v2);
                Console.WriteLine("Error: Vertex does not exist");
            }
        }

        //nearestIndex-> when given a index it finds the nearest element index on the left and right side the given index 
        static int nearestIndex(int idx,int side)
        {
            if(side==1)
            { 
                for(int i=idx-1;i>=0;i--)
                {
                    if(ElementIndexs[i]==1)
                    {
                        idx=i;
                        break;
                    }
                }
            }
            if(side==0)
            {
                for(int i=idx+1;i<ElementIndexs.Length;i++)
                {
                    if(ElementIndexs[i]==1)
                    {
                        idx=i;
                        break;
                    }
                }
            }
            return idx;
        }

        //implicitHydrogenIndex--> CALCULATES THE IMPLICIT HYDROGEN COUNT OF EACH ELEMENT AND ADDS IT TO THE HydrogenCount DICTIONARY
        public void implicitHydrogenIndex(string input)
        {
            char[] res = input.ToCharArray();
            int valency=0;
            int bondsCount=0;
            int element= 0;
            string firstLetter = "";
            string secondLetter = "";
            bool isit =false;
            foreach (var x in graph.adjacencyList)
            {
                valency=0;
                bondsCount=0;
                firstLetter = char.ToString(res[x.Key]);
                secondLetter = "";
                isit = false;
                if(x.Key<res.Length-1) 
                {
                    secondLetter = char.ToString(res[x.Key+1]);
                }
                string newelement = string.Join("",firstLetter,secondLetter);
                if(Elements.ContainsKey(newelement))
                {
                    isit=true;
                }
                if(isit==false)
                {
                    element=Elements[char.ToString(res[x.Key])];
                }
                else
                {
                    element=Elements[newelement];
                }
                foreach(KeyValuePair<int, int> y in x.Value)
                {
                    bondsCount+=y.Value;
                }
                valency=element-bondsCount;
                graph.HydrogenCount.Add(x.Key,valency);
            }
            foreach(KeyValuePair<int, int> ele in graph.HydrogenCount)
            {
                Console.WriteLine(ele.Key +"-->"+ ele.Value);
            }
        }

        // adjacencyMatrix --> GENERATES THE ADJACENCY MATRIX 
        public int[,] adjacencyMatrix(string input)
        {
            //size --> NO OF ELEMENTS IN THE GIVEN INPUT SMILES NOTATION
            //Matrix_size --> LENGTH OF THE GIVEN INPUT SMILES NOTATION 
            char[] res = input.ToCharArray();
            int sum=0,n=0,m=0,size = elementCount,Matrix_size = res.Length; 
            int[,] array = new int[Matrix_size,Matrix_size];
            int[] check = new int[Matrix_size];
            int[,] adjacencyMatrix = new int[size,size];

            //GENERATE THE BIG MATRIX
            foreach(var x in graph.adjacencyList)
            {
                foreach(KeyValuePair<int, int> y in x.Value)
                {
                    array[x.Key,y.Key] = 1;
                }
            }

            //LOGIC TO REDUCE THE BIG MATRIX TO SMALLER ONE
            for(int i=0;i<Matrix_size;i++)
            {
                sum=0;
                for(int j=0;j<Matrix_size;j++)
                {
                    sum+=array[i,j];
                }
                if(sum!=0) check[i]=1;
            }

            for(int i=0;i<Matrix_size;i++)
            {
                if(check[i]!=0)
                {
                    n=0;
                    for(int j=0;j<Matrix_size;j++)
                    {
                        if(check[j]!=0) 
                        {
                            adjacencyMatrix[m,n]=array[i,j];
                            n++;
                        }
                    }
                    m++;
                }
            }

            //PRINTING THE ADJACENCY MATRIX
            for(int i=0;i<size;i++)
            {
                for(int j=0;j<size;j++)
                {
                    Console.Write(adjacencyMatrix[i,j]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        
            return adjacencyMatrix;
        }
        public void zagrebIndex(int[,] adjacencyMatrix)
        {
            if(adjacencyMatrix.GetLength(0)==adjacencyMatrix.GetLength(1))
            {
                int[] degreeOfVertices = new int[adjacencyMatrix.GetLength(0)];
                int count=0;

                for(int i=0;i<adjacencyMatrix.GetLength(0);i++)
                {
                    count=0;
                    for(int j=0;j<adjacencyMatrix.GetLength(0);j++)
                    {
                        if(adjacencyMatrix[i,j]==1) count++;
                    }
                    degreeOfVertices[i]=count;
                }

                int M1 = 0;
                for(int i=0;i<degreeOfVertices.Length;i++)
                {
                    M1+=degreeOfVertices[i]*degreeOfVertices[i];
                }
        
                int M2=0;
                double connindex=0;

                for(int i=0;i<adjacencyMatrix.GetLength(0);i++)
                {
                    for(int j=0;j<adjacencyMatrix.GetLength(0);j++)
                    {
                        if(i<j)
                        {
                            if(adjacencyMatrix[i,j]==1)
                            {
                                M2+=degreeOfVertices[i]*degreeOfVertices[j];
                                connindex+=1/Math.Sqrt(degreeOfVertices[i]*degreeOfVertices[j]);
                            }
                        }
                    }
                }
                Console.WriteLine("M1 value : "+M1);
                Console.Write("M2 value : "+M2);
                Console.Write("\nrandic connectivity index : "+connindex+"\n");
            }
            else
            {
                Console.WriteLine("RE-DO ADJACENCY MATRIX");
            }
        }

        public void readSmiles(string input)
        {
            char[] res = input.ToCharArray();
            branch(res);
            ring(res);
            linear(res);
            //PRINTING THE GRAPH
            // VERTEX-->VERTEX : BOND-VALUE --> format of printing the graph
            foreach (var x in graph.adjacencyList)
            { 
                Console.WriteLine(x.Key);
                foreach (KeyValuePair<int, int> y in x.Value)
                {
                    Console.WriteLine("vertex " + x.Key + " ---> " + y.Key + ": "+y.Value);
                }
            }
        }


        static void branch(char[] res)
        {
            //used for branch calculations.
            int[] arr = new int[100];
            int count = 0;     

            for(int i=0;i<res.Length;i++)
            {
                //CHECK/LOGIC FOR BRANCH STRUCTURES IN INPUT SMILES NOTATION
                if((res[i]==')'))
                {
                    count--;
                    if(i!=res.Length-1)
                    {
                        if(bonds.ContainsKey(res[i+1]))
                        {
                            graph.addEdge(nearestIndex(i,0),arr[count],bonds[res[i+1]]);
                        }
                        else if(ElementIndexs[i+1]==1)
                        {
                            graph.addEdge(i+1,arr[count],1);
                        }
                    }
                }
                if((res[i]=='(')&&(res[i-1]!=')'))
                {
                    if(char.IsLower(res[i-1]))
                    {
                        arr[count] = i-2;
                    }
                    else 
                    {
                        arr[count] = i-1;
                    }
                    count++;
                }
                if((res[i]=='(')&&(res[i-1]==')'))
                {
                    if(bonds.ContainsKey(res[i+1]))
                    {
                        graph.addEdge(arr[count++],nearestIndex(i,0),bonds[res[i+1]]);
                    }
                    else
                    {
                        if(ElementIndexs[i+1]==1) graph.addEdge(arr[count++],nearestIndex(i,0),1);
                    }
                }
                else if((res[i]=='(')&&(res[i-1]!=')'))
                {
                    if(bonds.ContainsKey(res[i+1]))
                    {
                        graph.addEdge(nearestIndex(i,1),nearestIndex(i,0),bonds[res[i+1]]);
                    }
                    else
                    {
                        graph.addEdge(nearestIndex(i,1),nearestIndex(i,0),1);
                    }
                }
            }
        }


        static void linear(char[] res)
        {
            for(int i=1;i<res.Length;i++)
            {
                if(res[i]=='(' || res[i]==')') continue;
                else
                {   
                    if(i!=res.Length-1 && char.IsDigit(res[i]) && res[i+1]!=')')
                    {
                        if(ElementIndexs[i+1]==1){
                            graph.addEdge(nearestIndex(i,1),i+1,1);
                        }
                        else{
                            graph.addEdge(nearestIndex(i,1),nearestIndex(i,0),bonds[res[i+1]]);
                        }
                    }
                    if(ElementIndexs[i]==0 && i!=res.Length-1 && !bonds.ContainsKey(res[i]) && !char.IsDigit(res[i]))
                    {
                        if(ElementIndexs[i+1]==1 && ElementIndexs[i-1]==1)
                        {
                            graph.addEdge(i-1,i+1,1);
                        }
                    }
                    if(ElementIndexs[i-1]==1 && ElementIndexs[i]==1)
                    {
                        graph.addEdge(i-1,i,1);
                    }
                    if(bonds.ContainsKey(res[i]) && res[i-1]!='(' && res[i-1]!=')' && !char.IsDigit(res[i+1]))
                    {
                        graph.addEdge(nearestIndex(i,1),nearestIndex(i,0),bonds[res[i]]);
                    }
                }
            }
        }
        static void ring(char[] res)
        {
            for(int i=0;i<res.Length;i++)
            {
                if(char.IsDigit(res[i]))
                {
                    for(int j=i+1;j<res.Length;j++)
                    {
                        if(res[i]==res[j]) 
                        {
                            int bond_value = 1;
                            if(bonds.ContainsKey(res[i-1]) && bonds.ContainsKey(res[j-1]) && (res[i-1]==res[j-1])) bond_value=bonds[res[i-1]];
                            else if(bonds.ContainsKey(res[i-1])) bond_value=bonds[res[i-1]];
                            else if(bonds.ContainsKey(res[j-1])) bond_value=bonds[res[j-1]];
                            else bond_value=1;
                            graph.addEdge(nearestIndex(i,1),nearestIndex(j,1),bond_value);
                        }
                    }
                }
            }
        }
        
        //initiateVertex -> used to initiate vertices in the graph
        public void initiateVertex(string input)
        {
            //TRAVERSING THROUGH THE INPUT SMILES NOTATION INITIATING GRAPH VERTICES
            char[] res = input.ToCharArray();
            ElementIndexs = new int[res.Length];
            for(int i=0;i<res.Length-1;i++)
            {
                if(Elements.ContainsKey(char.ToString(res[i])+char.ToString(res[i+1])) || Elements.ContainsKey(char.ToString(res[i])))
                {
                    ++elementCount;
                    ++ElementIndexs[i];
                    graph.addVertex(i);
                }
            }
            if(Elements.ContainsKey(Char.ToString(res[res.Length-1])))
            {
                ++elementCount;
                ++ElementIndexs[res.Length-1];
                graph.addVertex(res.Length-1);
            }
            for(int i=0;i<ElementIndexs.Length;i++) 
            {
                Console.Write(ElementIndexs[i]+" ");
            }
            Console.WriteLine();
        }
    }
}
 