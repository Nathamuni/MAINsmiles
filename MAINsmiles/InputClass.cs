using Logic;

class inputClass
{
    static void Main(string[] args)
    {
        WeightedGraph inputobj = new WeightedGraph();
        int option ;
        string inputSmiles = "";
        do{
        Console.WriteLine("enter the option : \n1)input smiles notation\n2)print adjacency matrix\n3)print implicit hydrogens\n4)print zagreb index\n");
        option = Convert.ToInt32(Console.ReadLine());
        switch (option) 
        {
            case 1:
                Console.WriteLine("ENTER THE INPUT SMILES NOTATION : ");
                inputSmiles = Console.ReadLine();
                if(!string.IsNullOrEmpty(inputSmiles))
                {   
                    inputobj.initiateVertex(inputSmiles);
                    inputobj.readSmiles(inputSmiles);
                }
                else
                {
                    Console.WriteLine("ENTER A PROPER INPUT");
                }
                break;
            case 2:
                Console.WriteLine("ADJACENCY MATRIX : ");
                inputobj.adjacencyMatrix(inputSmiles);
                break;
            case 3:
                Console.WriteLine("IMPLICIT HYDROGEN COUNT : ");
                inputobj.implicitHydrogenIndex(inputSmiles);
                break;
            case 4:
                Console.WriteLine("ZAGREB INDEX : ");
                inputobj.zagrebIndex(inputobj.adjacencyMatrix(inputSmiles));
                break;
            default:
                Console.WriteLine("INVALID OPTION");
                break;
        }
        Console.WriteLine("DO YOU WANT TO CONTINUE (1/0) :");
        option = Convert.ToInt32(Console.ReadLine());
        }while(option!=0);
        }
    }