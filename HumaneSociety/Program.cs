using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            Animal testanimal = new Animal();
            
            testanimal.AnimalId = 3;
            Client clienttest = new Client();
            clienttest.ClientId = 1;
            Query.Adopt(testanimal, clienttest);
             
            //PointOfEntry.Run();
        }
    }
}
