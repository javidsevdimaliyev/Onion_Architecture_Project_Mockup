using System.Transactions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SolutionName.Application.Attributes;

public sealed class TransactionAttribute : ActionFilterAttribute //, IDisposable
{
    private Transaction _transaction;

    private TransactionScope _transactionScope;
    //   private readonly ILogger _logger;

    public TransactionAttribute()
    {
        Order = 0;
        //    _logger = Logger.Instance;
        //    _logger.Debug("TransactionScope.ctor");
    }

    private void InitTransaction()
    {
        var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
        _transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
        _transaction = Transaction.Current;
        _transaction.TransactionCompleted += _transaction_TransactionCompleted;
    }

    private void _transaction_TransactionCompleted(object sender, TransactionEventArgs e)
    {
        //_logger.Debug("TransactionScope.TransactionCompleted");
    }

    private void DisposeTransaction()
    {
        _transaction.Dispose();
        _transactionScope.Dispose();
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        InitTransaction();
        // _logger.Debug("TransactionScope.OnActionExecuting");
        base.OnActionExecuting(filterContext);
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        var exception = filterContext.Exception;
        var hasError = exception != null;
        //if (!hasError)
        //{
        //    var dict = filterContext.Controller.ViewData;
        //    if (dict != null && dict.ContainsKey(RKeys.TransactionExceptionKey))
        //    {
        //        exception = (Exception)dict[RKeys.TransactionExceptionKey];
        //    }
        //    hasError = exception != null;
        //}
        if (!hasError)
            //  _logger.Debug("TransactionScope.OnActionExecuted.Complete");
            _transactionScope.Complete();
        else
            // _logger.Debug("TransactionScope.OnActionExecuted.Rollback", filterContext.Exception);
            _transaction.Rollback(exception);
        base.OnActionExecuted(filterContext);
        DisposeTransaction();
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        // _logger.Debug("TransactionScope.OnResultExecuting");
        base.OnResultExecuting(filterContext);
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        //  _logger.Debug("TransactionScope.OnResultExecuted");
        base.OnResultExecuted(filterContext);
    }

    ~TransactionAttribute()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transactionScope.Dispose();
            _transaction.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}