using System.Linq.Expressions;
using System.Text;

namespace ExpressionTrees.Sql;

/// <summary>
/// A simple SQL expression visitor that converts expression trees to SQL WHERE clauses
/// Similar to what EF Core providers do, but much simpler
/// </summary>
public class SqlWhereExpressionVisitor : ExpressionVisitor
{
    private StringBuilder _sql = new();

    public string Translate(Expression expression)
    {
        _sql.Clear();
        Visit(expression);
        return _sql.ToString();
    }

    private ParameterExpression? _parameter;

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        _parameter = node.Parameters[0];
        _sql.Append("SELECT * FROM ");
        _sql.Append(_parameter.Type.Name);
        _sql.Append("s WHERE "); // plural table name
        Visit(node.Body);
        return node;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sql.Append("(");
        Visit(node.Left);

        _sql.Append(node.NodeType switch
        {
            ExpressionType.AndAlso => " AND ",
            ExpressionType.OrElse => " OR ",
            ExpressionType.Equal => " = ",
            ExpressionType.NotEqual => " <> ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            _ => throw new NotSupportedException($"Unsupported binary operator: {node.NodeType}")
        });

        Visit(node.Right);
        _sql.Append(")");
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression == _parameter)
        {
            _sql.Append(node.Member.Name);
            return node;
        }
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value is string)
        {
            _sql.Append('\'');
            _sql.Append(node.Value);
            _sql.Append('\'');
        }
        else
        {
            _sql.Append(node.Value);
        }
        return node;
    }
}