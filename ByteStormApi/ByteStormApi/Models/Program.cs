using System.Reflection.Metadata;

namespace ByteStormApi.Models
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var dbOperativo = new OperativoContext())
            using (var dbMision = new MisionContext())
            using (var dbEquipo = new EquipoContext())
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Operativo: ");
                var name = Console.ReadLine();

                var blog = new Operativo { Nombre = name };
                dbOperativo.Operativos.Add(blog);
                dbOperativo.SaveChanges();

                // Display all Blogs from the database
                var query = from o in dbOperativo.Operativos
                            orderby o.Nombre
                            select o;

                Console.WriteLine("All Operativos in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Nombre, item.Id);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
