
using System;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PlutoContext();

            // LINQ syntax
            var query =
                from c in context.Courses
                where c.Name.Contains("C#")
                orderby c.Name
                select c;

            Console.WriteLine("/nLINQ syntax");
            foreach (var course in query)
            {
                Console.WriteLine(course.Name);
            }

            // Extension Method
            var courses = context.Courses
                .Where(c => c.Name.Contains("C#"))
                .OrderBy(c => c.Name);

            Console.WriteLine("/nExtension Method");
            foreach (var course in courses)
            {
                Console.WriteLine(course.Name);
            }

            //Group Join
            var query1 =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new {AuthorName = a.Name, Courses = g.Count()};

            Console.WriteLine("/nGroup Join");
            foreach (var x in query1)
            {
                Console.WriteLine("{0}: {1}",x.AuthorName,x.Courses);
            }

            //Cross Join
            var query2 =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            Console.WriteLine("/nCross Join");
            foreach (var x in query2)
            {
                Console.WriteLine("{0}: {1}", x.AuthorName, x.CourseName);
            }

        }
    }
}
