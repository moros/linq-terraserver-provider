# linq-terraserver-provider
Demo app about creating custom IQueryable LINQ Provider

This is a simple class library app about creating a custom IQueryable LINQ Provider from the "Walkthrough: Creating an IQueryable LINQ Provider" from Microsoft.
https://msdn.microsoft.com/en-us/library/bb546158.aspx

- Shows how to implement the interfaces of IQueryable<T>, IOrderedQueryable<T>, and IQueryProvider
- Implements the Queryable.Where method by using an expression tree visitor subclass.
- Creates an expression tree visitor subclass that extracts information from the LINQ query to use in a web service request.
- Other cool things
