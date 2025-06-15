using System.Linq.Expressions;

using DataLib;

Expression<Func<Racer, bool>> expression = r => r.Country == "Brazil" && r.Wins > 6;

DisplayTree(0, "Lambda", expression);

static void DisplayTree(int indent, string message, Expression expression)
{
    string output = $"{new string('>', indent)} {message}! " + 
        $"NodeType: {expression.NodeType}; Expr: {expression}";

    indent++;

    switch (expression.NodeType)
    {
        case ExpressionType.Lambda:
            Console.WriteLine(output);
            LambdaExpression lambdaExpr = (LambdaExpression)expression;
            foreach (var parameter in lambdaExpr.Parameters)
            {
                DisplayTree(indent, "Parameter", parameter);
            }
            DisplayTree(indent, "Body", lambdaExpr.Body);
            break;
        case ExpressionType.Constant:
            ConstantExpression constExpr = (ConstantExpression)expression;
            Console.WriteLine($"{output} Const Value: {constExpr.Value}");
            break;
        case ExpressionType.Parameter:
            ParameterExpression paramExpr = (ParameterExpression)expression;
            Console.WriteLine($"{output} Param Type: {paramExpr.Type.Name}");
            break;
        case ExpressionType.Equal:
        case ExpressionType.AndAlso:
        case ExpressionType.GreaterThan:
            if (expression is BinaryExpression binaryExpression)
            {
                Console.WriteLine($"{output} Method: {binaryExpression.Method?.Name}");
                DisplayTree(indent, "Left", binaryExpression.Left);
                DisplayTree(indent, "Right", binaryExpression.Right);
            }
            else
            {
                Console.WriteLine(output);
            }

            break;
        case ExpressionType.MemberAccess:
            MemberExpression memberExpr = (MemberExpression)expression;
            Console.WriteLine($"{output} Member Name: {memberExpr.Member.Name}, Type: {memberExpr.Type.Name}");
            if (memberExpr.Expression is not null)
            {
                DisplayTree(indent, "Member Expr", memberExpr.Expression);
            }
            break;
        default:
            Console.WriteLine();
            Console.WriteLine($"{expression.NodeType} {expression.Type.Name}");
            break;
    }
}