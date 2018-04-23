using System;
using System.Collections.Generic;
using System.Windows.Forms;
#region #usings
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraScheduler.Outlook;
using DevExpress.XtraScheduler.Outlook.Interop;
#endregion #usings



namespace ParameterizedOutlookImport {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e) {
            OutlookImport importer = new OutlookImport(this.schedulerStorage1);

            TimeInterval interval = CalculateCurrentWeekInterval();
            ParameterizedCalendarProvider provider = new ParameterizedCalendarProvider(interval);
            importer.SetCalendarProvider(provider);
            importer.Import(System.IO.Stream.Null);
        }
        private TimeInterval CalculateCurrentWeekInterval() {
		    DateTime start = DateTimeHelper.GetStartOfWeek(DateTime.Today);
		    return new TimeInterval(start, start.AddDays(7));
        }
    }
    #region #outlookcalendarprovider
    public class ParameterizedCalendarProvider: OutlookCalendarProvider {
        TimeInterval interval;
        public ParameterizedCalendarProvider(TimeInterval interval) {
            this.interval = interval != null ? interval : TimeInterval.Empty;
        }
        public TimeInterval Interval { get { return interval; } }

        protected override List<_AppointmentItem> PrepareItemsForExchange(_Items items) {
             List<_AppointmentItem> result = new List<_AppointmentItem>();
             if (items == null)
                return result;

            string filter = FormatInterval(Interval);
            _AppointmentItem olApt = items.Find(filter) as _AppointmentItem;
            while (olApt != null) {
                result.Add(olApt);
                olApt = items.FindNext() as _AppointmentItem;
            }
            return result;
        }
        protected string FormatInterval(TimeInterval interval) {
            return String.Format("[Start] > '{0}' AND [End] < '{1}'",
                interval.Start.ToString("g"), interval.End.ToString("g"));
        }
    }
    #endregion #outlookcalendarprovider
}