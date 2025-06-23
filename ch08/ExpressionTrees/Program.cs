using System.Linq.Expressions;
using DataLib;
using ExpressionTrees.Sql;

// Sample expression to convert to SQL
Expression<Func<Racer, bool>> expression = r => r.Country == "Brazil" && r.Wins > 6;

// Create SQL translator and generate SQL
SqlWhereExpressionVisitor sqlTranslator = new();
string sql = sqlTranslator.Translate(expression);

Console.WriteLine("Expression:");
Console.WriteLine(expression);
Console.WriteLine("\nGenerated SQL:");
Console.WriteLine(sql);
