using SolutionName.Application.Models.Requests;
using SolutionName.Application.Models.Shared;
using SolutionName.Application.Utilities.Utility;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

namespace SolutionName.Application.Utilities.Extensions;

public static class QueryExtension
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int? page, int? pageSize)
    {
        var skip = page ?? 0;
        var take = pageSize ?? 50;
        return query.Skip(skip * take).Take(take);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, params SortModel[] sortModels)
    {
        var expression = source.Expression;
        var count = 0;
        foreach (var item in sortModels)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, item.ColId);
            var isDesc = string.Equals(item.Sort.Trim(), "DESC".Trim());
            var method = isDesc ? count == 0 ? "OrderByDescending" : "ThenByDescending" :
                count == 0 ? "OrderBy" : "ThenBy";
            expression = Expression.Call(typeof(Queryable), method,
                new[] { source.ElementType, selector.Type },
                expression, Expression.Quote(Expression.Lambda(selector, parameter)));
            count++;
        }

        return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
    }

    public static IQueryable<K> AddFilter<K>(this IQueryable<K> query, PagingRequest pagingRequest, out int count)
    {
        if (pagingRequest.Filters is not null)
        {
            foreach (var filter in pagingRequest.Filters!)
            {
                string filterFieldName = string.Empty;
                object filterValue = null;
                string equalityType = GetEquality(filter.EqualityType);

                if (filter.FieldName.IndexOf("IdHash") > -1)
                {
                    filterValue = TextEncryption.Decrypt<int>(filter.Value.ToString());
                    filterFieldName = filter.FieldName.Replace("IdHash", "Id");
                }
                else
                {
                    filterValue = filter.Value;
                    filterFieldName = filter.FieldName;
                }

                string body = string.Empty;

                // Check data type of K 
                Type? propertyType = typeof(K).GetProperty(filterFieldName)?.PropertyType;

                var jsonElementValueKind = ((JsonElement)filterValue).ValueKind;

                propertyType.ThrowIfNull(HttpResponseStatus.FilterFieldNotValid);

                if (propertyType == typeof(DateTime))
                {
                    body += BuildDateExpression<K>(filterValue, filterFieldName, equalityType);
                }
                else if (propertyType == typeof(bool))
                {
                    body += BuildBooleanExpression(filterValue, filterFieldName);
                }
                // else if (propertyType == typeof(string)) // TODO : write this filter methods
                // {
                //     body += BuildStringExpression(filterValue, body, filterFieldName, equalityType);
                // }
                // else
                // {
                //     body += BuildDefaultExpression(filterValue, body, filterFieldName, equalityType);
                // }


                if (filter.EqualityType == nameof(EqualityType.StartsWith))
                {
                    body += $"{filterFieldName}.StartsWith(\"{filterValue}\")";
                }
                else if (filter.EqualityType == nameof(EqualityType.Contains))
                {
                    body += $"{filterFieldName}.Contains(\"{filterValue}\")";
                }

                #region EQUAL // TODO: Remove and refactor this part 

                else if (filter.EqualityType == nameof(EqualityType.Equal))
                {
                    if (filterValue is int || (jsonElementValueKind == JsonValueKind.Number))
                    {
                        body += $"{filterFieldName}=={filterValue}";
                    }

                    else if (jsonElementValueKind == JsonValueKind.String && propertyType == typeof(string))
                    {
                        if (filterValue.ToString().Equals("null"))
                            body += $"{filterFieldName} is null";
                        else
                            body += $"{filterFieldName}==\"{filterValue}\"";
                    }

                    else if (jsonElementValueKind == JsonValueKind.Null)
                    {
                        body += $"{filterFieldName} is null";
                    }
                }

                #endregion


                Console.WriteLine($"x => x.{body}");

                query = query.Where($"x => x.{body}");
            }
        }

        count = query.Count();

        query = pagingRequest is not null && pagingRequest.SortOptions!.Any()
            ? query.OrderBy(pagingRequest.SortOptions!.ToArray()) // Apply sorting
            : query.OrderBy(new SortModel(column: "Id", isDesc: true));

        query = query.Skip((pagingRequest!.Page) * pagingRequest.PageSize).Take(pagingRequest.PageSize);
        return query;
    }

    #region Helpers methods
    private static string BuildBooleanExpression(
       object filterValue,
       string filterFieldName)
    {
        var parsed = bool.TryParse(filterValue.ToString(), out bool boolValue);

        ThrowExceptionExtension.ThrowIfFalse(parsed, HttpResponseStatus.MissingParameter);

        return boolValue ? $"{filterFieldName}" : $"{filterFieldName} == false";
    }

    private static string BuildDateExpression<K>(
        object filterValue,
        string filterFieldName,
        string equalityType)
    {
        bool tryParse = DateTime.TryParse(filterValue.ToString(), out DateTime date);

        ThrowExceptionExtension.ThrowIfFalse(tryParse, HttpResponseStatus.FilterFieldNotValid);

        string formattedDate = date.ToString("yyyy-MM-dd");
        var body = $"{filterFieldName}.Date {equalityType} \"{formattedDate}\"";
        return body;
    }

    private static string BuildDefaultExpression(
        object filterValue,
        string filterFieldName,
        string equalityType)
    {
        throw new NotImplementedException();
    }

    private static string BuildStringExpression(
        object filterValue,
        string filterFieldName,
        string equalityType)
    {
        throw new NotImplementedException();
    }


    private static string GetEquality(string equalityType) =>
       equalityType switch
       {
           nameof(EqualityType.Equal) => "==",
           nameof(EqualityType.NotEqual) => "!=",
           nameof(EqualityType.LessThan) => "<",
           nameof(EqualityType.LessThanOrEqual) => "<=",
           nameof(EqualityType.GreaterThan) => ">",
           nameof(EqualityType.GreaterThanOrEqual) => ">=",
           nameof(EqualityType.StartsWith) => ".StartsWith(@0)",
           nameof(EqualityType.EndsWith) => ".EndsWith(@0)",
           nameof(EqualityType.Contains) => ".Contains(@0)",
           nameof(EqualityType.DoesNotContain) => ".Contains(@0) == false",
           _ => "=="
       };
    #endregion




}