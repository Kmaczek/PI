using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class BinanceResponse<T>
    {
        public T ResponseDto { get; set; }
        public OutcomeStatus Status { get; set; }
        public List<Notification> Notifications = new List<Notification>();
    }

    public class Notification
    {
        public string Message { get; set; }
        public NotificationSeverity Severity { get; set; }

        public static Notification AddInformation(string message)
        {
            var notification = new Notification()
            {
                Severity = NotificationSeverity.Information,
                Message = message
            };

            return notification;
        }

        public static Notification AddWarning(string message)
        {
            var notification = new Notification()
            {
                Severity = NotificationSeverity.Warning,
                Message = message
            };

            return notification;
        }

        public static Notification AddError(string message)
        {
            var notification = new Notification()
            {
                Severity = NotificationSeverity.Error,
                Message = message
            };

            return notification;
        }
    }

    public enum NotificationSeverity
    {
        Information,
        Warning,
        Error
    }

    public enum OutcomeStatus
    {
        OK,
        Failed
    }
}
