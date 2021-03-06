# EF_LINQ_Linq

## Restriction
- LINQ syntax
```csharp
from c in context.Courses
where c.Level == 1
select c;
```
- Extension method
```csharp
context.Courses.Where(c => c.Level == 1);
```

## Ordering
- LINQ syntax
```csharp
from c in context.Courses
where c.Level == 1
orderby c.Name, c.Price descending
select c;
```
- Extension method
```csharp
context.Courses
	.OrderBy(c => c.Name) // or OrderByDescending
	.ThenBy(c => c.Level); // or ThenByDescending
```

## Projection
- LINQ syntax
```csharp
from c in context.Courses
select new { Course = c.Name, AuthorName = c.Author.Name };
```
- Extension method
```csharp
context.Courses.Select(c => new 
	{ 
		CourseName = c.Name, 
		AuthorName = c.Author.Name // no join required
	});

// flattens hierarchical lists
var tags = context.Courses.SelectMany(c => c.Tags);
```

## Grouping
- LINQ syntax
```csharp
from c in context.Courses
group c by c.Level into g
select g;

from c in context.Courses
group c by c.Level into g
select new { Level = g.Key, Courses = g };
```
- Extension method
```csharp
var groups = context.Courses.GroupBy(c => c.Level);
```

## Inner Join
Use when there is no relationship between your entities and you need to link them based on a
key.
- LINQ syntax
```csharp
from a in context.Authors
join c in context.Courses on a.Id equals c.AuthorId
select new { Course = c.Name, Author = a.Name };
```
- Extension method
```csharp
var authors = context.Authors.Join(context.Courses,
	a => a.Id, // key on the left side
	c => c.AuthorId, // key on the right side,
	(author, course) => // what to do once matched
		new
		{
			AuthorName = author.Name,
			CourseName = course.Name
		}
);
```

## Group Join
Useful when you need to group objects by a property and count the number of objects in each
group. In SQL we do this with LEFT JOIN, COUNT(*) and GROUP BY. In LINQ, we use group
join.
- LINQ syntax
```csharp
from a in context.Authors
join c in context.Courses on a.Id equals c.AuthorId
into g
select new { Author = a.Name, Courses = c.Count() };
```
- Extension method
```csharp
var authors = context.Authors.GroupJoin(context.Courses,
	a => a.Id, // key on the left side
	c => c.AuthorId, // key on the right side, 
	(author, courses) => // what to do once matched 
		new
		{
			AuthorName = author,
			Courses = courses.Count()
		}
);
```

## Cross Join
To get full combinations of all objects on the left and the ones on the right.
- LINQ syntax
```csharp
from a in context.Authors
from c in context.Courses
select new { Author = a.Name, Course = c.Name };
```
- Extension method
```csharp
var combos = context.Authors.SelectMany(a => context.Courses, 
	(author, course) => new {
		AuthorName = author.Name,
		CourseName = course.Name
});
```

## Partitioning
To get records in a given page.
- Extension method
```csharp
context.Courses.Skip(10).Take(10);
```

### Element Operators
- Extension method
```csharp
// throws an exception if no elements found
context.Courses.First();
context.Courses.First(c => c.Level == 1);

// returns null if no elements found
context.Courses.FirstOrDefault();

// not supported by SQL Server
context.Courses.Last();
context.Courses.LastOrDefault();
context.Courses.Single(c => c.Id == 1);
context.Courses.SingleOrDefault(c => c.Id == 1);
```

## Quantifying
- Extension method
```csharp
bool allInLevel1 = context.Courses.All(c => c.Level == 1);
bool anyInLevel1 = context.Courses.Any(c => c.Level == 1);
```

## Aggregating
- Extension method
```csharp
int count = context.Courses.Count();
int count = context.Courses.Count(c => c.Level == 1);
var max = context.Courses.Max(c => c.Price);
var min = context.Courses.Min(c => c.Price);
var avg = context.Courses.Average(c => c.Price);
var sum = context.Courses.Sum(c => c.Price);
```

## Deferred Execution
Query will not excute until:
- Iterating over query variable
- Calling ToList, ToArray, ToDictionary
- Calling First, Last, Single, Count, Max, Min, Average


## Lazy Loading
- Turn off lazy loading, should use when develop web application.
```csharp
public class PlutoContext : DbContext
{
    public PlutoContext()
        : base("name=PlutoContext")
    {
        this.Configuration.LazyLoadingEnabled = false;
    }
}
```

## Eager Loading
```csharp
var courses = context.Courses.Include(c => c.Author).ToList();
```